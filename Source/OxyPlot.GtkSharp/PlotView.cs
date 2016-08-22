// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.GtkSharp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using Gdk;

    using Gtk;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    [Serializable]
    public partial class PlotView : DrawingArea, IPlotView
    {
        /// <summary>
        /// The category for the properties of this control.
        /// </summary>
        private const string OxyPlotCategory = "OxyPlot";

        /// <summary>
        /// The invalidate lock.
        /// </summary>
        private readonly object invalidateLock = new object();

        /// <summary>
        /// The model lock.
        /// </summary>
        private readonly object modelLock = new object();

        /// <summary>
        /// The rendering lock.
        /// </summary>
        private readonly object renderingLock = new object();

        /// <summary>
        /// The render context.
        /// </summary>
        private readonly GraphicsRenderContext renderContext;

        /// <summary>
        /// The current model (holding a reference to this plot view).
        /// </summary>
        [NonSerialized]
        private PlotModel currentModel;

        /// <summary>
        /// The is model invalidated.
        /// </summary>
        private bool isModelInvalidated;

        /// <summary>
        /// The model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        /// The update data flag.
        /// </summary>
        private bool updateDataFlag = true;

        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private OxyRect? zoomRectangle;

        /// <summary>
        /// The default controller
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        public PlotView()
        {
            this.renderContext = new GraphicsRenderContext();

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            this.DoubleBuffered = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            this.PanCursor = new Cursor(CursorType.Hand1);
            this.ZoomRectangleCursor = new Cursor(CursorType.Sizing);
            this.ZoomHorizontalCursor = new Cursor(CursorType.SbHDoubleArrow);
            this.ZoomVerticalCursor = new Cursor(CursorType.SbVDoubleArrow);
            this.AddEvents((int)(EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.EnterNotifyMask | EventMask.LeaveNotifyMask | EventMask.ScrollMask | EventMask.KeyPressMask | EventMask.PointerMotionMask));
            this.CanFocus = true;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Category(OxyPlotCategory)]
        public PlotModel Model
        {
            get
            {
                return this.model;
            }

            set
            {
                if (this.model != value)
                {
                    this.model = value;
                    this.OnModelChanged();
                }
            }
        }

        /// <summary>
        /// Gets the actual <see cref="OxyPlot.Model" /> of the control.
        /// </summary>
        Model IView.ActualModel
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Gets the actual <see cref="PlotModel" /> of the control.
        /// </summary>
        public PlotModel ActualModel
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Gets the actual controller.
        /// </summary>
        /// <value>
        /// The actual <see cref="IController" />.
        /// </value>
        IController IView.ActualController
        {
            get
            {
                return this.ActualController;
            }
        }

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        public OxyRect ClientArea
        {
            get
            {
                return new OxyRect(0, 0, Allocation.Width, Allocation.Height);
            }
        }

        /// <summary>
        /// Gets the actual plot controller.
        /// </summary>
        /// <value>The actual plot controller.</value>
        public IPlotController ActualController
        {
            get
            {
                return this.Controller ?? (this.defaultController ?? (this.defaultController = new PlotController()));
            }
        }

        /// <summary>
        /// Gets or sets the plot controller.
        /// </summary>
        /// <value>The controller.</value>
        public IPlotController Controller { get; set; }

        /// <summary>
        /// Gets or sets the pan cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor PanCursor { get; set; }

        /// <summary>
        /// Gets or sets the horizontal zoom cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor ZoomHorizontalCursor { get; set; }

        /// <summary>
        /// Gets or sets the rectangle zoom cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor ZoomRectangleCursor { get; set; }

        /// <summary>
        /// Gets or sets the vertical zoom cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor ZoomVerticalCursor { get; set; }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public void HideTracker()
        {
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            this.zoomRectangle = null;
            this.QueueDraw();
        }

        /// <summary>
        /// Invalidates the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
        public void InvalidatePlot(bool updateData)
        {
            lock (this.invalidateLock)
            {
                this.isModelInvalidated = true;
                this.updateDataFlag = this.updateDataFlag || updateData;
            }

            this.QueueDraw();
        }

        /// <summary>
        /// Called when the Model property has been changed.
        /// </summary>
        public void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.currentModel != null)
                {
                    ((IPlotModel)this.currentModel).AttachPlotView(null);
                    this.currentModel = null;
                }

                if (this.Model != null)
                {
                    ((IPlotModel)this.Model).AttachPlotView(this);
                    this.currentModel = this.Model;
                }
            }

            this.InvalidatePlot(true);
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="data">The data.</param>
        public void ShowTracker(TrackerHitResult data)
        {
            // not implemented for GtkSharp
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect rectangle)
        {
            this.zoomRectangle = rectangle;
            this.QueueDraw();
        }

        /// <summary>
        /// Sets the clipboard text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetClipboardText(string text)
        {
            try
            {
                // todo: can't get the following solution to work
                // http://stackoverflow.com/questions/5707990/requested-clipboard-operation-did-not-succeed
                this.GetClipboard(Gdk.Selection.Clipboard).Text = text;
            }
            catch (ExternalException)
            {
                // Requested Clipboard operation did not succeed.
                // MessageBox.Show(this, ee.Message, "OxyPlot");
            }
        }

        /// <summary>
        /// Called when the mouse button is pressed.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        protected override bool OnButtonPressEvent(EventButton e)
        {
            this.GrabFocus();

            return this.ActualController.HandleMouseDown(this, e.ToMouseDownEventArgs());
        }

        /// <summary>
        /// Called on mouse move events.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        protected override bool OnMotionNotifyEvent(EventMotion e)
        {
            return this.ActualController.HandleMouseMove(this, e.ToMouseEventArgs());
        }

        /// <summary>
        /// Called when the mouse button is released.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        protected override bool OnButtonReleaseEvent(EventButton e)
        {
            return this.ActualController.HandleMouseUp(this, e.ToMouseUpEventArgs());
        }

        /// <summary>
        /// Called when the mouse wheel is scrolled.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        protected override bool OnScrollEvent(EventScroll e)
        {
            return this.ActualController.HandleMouseWheel(this, e.ToMouseWheelEventArgs());
        }

        /// <summary>
        /// Called when the mouse enters the widget.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        protected override bool OnEnterNotifyEvent(EventCrossing e)
        {
            return this.ActualController.HandleMouseEnter(this, e.ToMouseEventArgs());
        }

        /// <summary>
        /// Called when the mouse leaves the widget.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        protected override bool OnLeaveNotifyEvent(EventCrossing e)
        {
            return this.ActualController.HandleMouseLeave(this, e.ToMouseEventArgs());
        }

        /// <summary>
        /// Called on KeyPress event.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        /// <returns>True if event was handled?</returns>
        protected override bool OnKeyPressEvent(EventKey e)
        {
            return this.ActualController.HandleKeyDown(this, e.ToKeyEventArgs());
        }

        /// <summary>
        /// Draws the plot to a cairo context within the specified bounds.
        /// </summary>
        /// <param name="cr">The cairo context to use for drawing.</param>
        void DrawPlot (Cairo.Context cr)
        {
            try
            {
                lock (this.invalidateLock)
                {
                    if (this.isModelInvalidated)
                    {
                        if (this.model != null)
                        {
                            ((IPlotModel)this.model).Update(this.updateDataFlag);
                            this.updateDataFlag = false;
                        }

                        this.isModelInvalidated = false;
                    }
                }

                lock (this.renderingLock)
                {
                    this.renderContext.SetGraphicsTarget(cr);
                    if (this.model != null)
                    {
                        if (!this.model.Background.IsUndefined())
                        {
                            this.renderContext.DrawRectangle(Allocation.ToOxyRect(), this.model.Background, OxyColors.Undefined, 0);
                        }

                        ((IPlotModel)this.model).Render(this.renderContext, Allocation.Width, Allocation.Height);
                    }

                    if (this.zoomRectangle.HasValue)
                    {
                        this.renderContext.DrawRectangle(this.zoomRectangle.Value, OxyColor.FromArgb(0x40, 0xFF, 0xFF, 0x00), OxyColors.Transparent, 1.0);
                    }
                }
            }
            catch (Exception paintException)
            {
                var trace = new StackTrace(paintException);
                Debug.WriteLine(paintException);
                Debug.WriteLine(trace);

                // using (var font = new Font("Arial", 10))
                {
                    // int width; int height;
                    // this.GetSizeRequest(out width, out height);
                    Debug.Assert(false, "OxyPlot paint exception: " + paintException.Message);

                    // g.ResetTransform();
                    // g.DrawString(, font, Brushes.Red, width / 2, height / 2, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }
        }
    }
}