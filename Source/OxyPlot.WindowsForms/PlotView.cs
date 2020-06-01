// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    [Serializable]
    public class PlotView : Control, IPlotView
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
        /// The tracker label.
        /// </summary>
        [NonSerialized]
        private Label trackerLabel;

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
        /// The default controller.
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// The update data flag.
        /// </summary>
        private bool updateDataFlag = true;

        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private Rectangle zoomRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        public PlotView()
        {
            this.renderContext = new GraphicsRenderContext();

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            this.DoubleBuffered = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            this.PanCursor = Cursors.Hand;
            this.ZoomRectangleCursor = Cursors.SizeNWSE;
            this.ZoomHorizontalCursor = Cursors.SizeWE;
            this.ZoomVerticalCursor = Cursors.SizeNS;

            var DoCopy = new DelegatePlotCommand<OxyKeyEventArgs>((view, controller, args) => this.DoCopy(view, args));
            this.ActualController.BindKeyDown(OxyKey.C, OxyModifierKeys.Control, DoCopy);
        }

        /// <summary>
        /// Gets the actual model in the view.
        /// </summary>
        /// <value>
        /// The actual model.
        /// </value>
        Model IView.ActualModel
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
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
                return new OxyRect(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width, this.ClientRectangle.Height);
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
        /// Gets or sets the plot controller.
        /// </summary>
        /// <value>The controller.</value>
        [Browsable(false)]
        [DefaultValue(null)]
        [Category(OxyPlotCategory)]
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
            if (this.trackerLabel != null)
            {
                this.trackerLabel.Visible = false;
            }
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            this.zoomRectangle = Rectangle.Empty;
            this.Invalidate();
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

            this.Invalidate();
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
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(CursorType cursorType)
        {
            switch (cursorType)
            {
                case CursorType.Pan:
                    this.Cursor = this.PanCursor;
                    break;
                case CursorType.ZoomRectangle:
                    this.Cursor = this.ZoomRectangleCursor;
                    break;
                case CursorType.ZoomHorizontal:
                    this.Cursor = this.ZoomHorizontalCursor;
                    break;
                case CursorType.ZoomVertical:
                    this.Cursor = this.ZoomVerticalCursor;
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;
            }
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="data">The data.</param>
        public void ShowTracker(TrackerHitResult data)
        {
            if (this.trackerLabel == null)
            {
                this.trackerLabel = new Label { Parent = this, BackColor = Color.LightSkyBlue, AutoSize = true, Padding = new Padding(5) };
            }

            this.trackerLabel.Text = data.ToString();
            this.trackerLabel.Top = (int)data.Position.Y - this.trackerLabel.Height;
            this.trackerLabel.Left = (int)data.Position.X - (this.trackerLabel.Width / 2);
            this.trackerLabel.Visible = true;
            this.trackerLabel.UseMnemonic = false;
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect rectangle)
        {
            this.zoomRectangle = new Rectangle((int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Width, (int)rectangle.Height);
            this.Invalidate();
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
                Clipboard.SetText(text);
            }
            catch (ExternalException ee)
            {
                // Requested Clipboard operation did not succeed.
                MessageBox.Show(this, ee.Message, "OxyPlot");
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.Focus();
            this.Capture = true;

            this.ActualController.HandleMouseDown(this, e.ToMouseDownEventArgs(GetModifiers()));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.ActualController.HandleMouseMove(this, e.ToMouseEventArgs(GetModifiers()));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Capture = false;
            this.ActualController.HandleMouseUp(this, e.ToMouseUpEventArgs(GetModifiers()));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.ActualController.HandleMouseEnter(this, e.ToMouseEventArgs(GetModifiers()));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ActualController.HandleMouseLeave(this, e.ToMouseEventArgs(GetModifiers()));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            this.ActualController.HandleMouseWheel(this, e.ToMouseWheelEventArgs(GetModifiers()));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
                    this.renderContext.SetGraphicsTarget(e.Graphics);

                    if (this.model != null)
                    {
                        if (!this.model.Background.IsUndefined())
                        {
                            using (var brush = new SolidBrush(this.model.Background.ToColor()))
                            {
                                e.Graphics.FillRectangle(brush, e.ClipRectangle);
                            }
                        }

                        ((IPlotModel)this.model).Render(this.renderContext, new OxyRect(0, 0, this.Width, this.Height));
                    }

                    if (this.zoomRectangle != Rectangle.Empty)
                    {
                        using (var zoomBrush = new SolidBrush(Color.FromArgb(0x40, 0xFF, 0xFF, 0x00)))
                        using (var zoomPen = new Pen(Color.Black))
                        {
                            zoomPen.DashPattern = new float[] { 3, 1 };
                            e.Graphics.FillRectangle(zoomBrush, this.zoomRectangle);
                            e.Graphics.DrawRectangle(zoomPen, this.zoomRectangle);
                        }
                    }
                }
            }
            catch (Exception paintException)
            {
                var trace = new StackTrace(paintException);
                Debug.WriteLine(paintException);
                Debug.WriteLine(trace);
                using (var font = new Font("Arial", 10))
                {
                    e.Graphics.ResetTransform();
                    e.Graphics.DrawString(
                        "OxyPlot paint exception: " + paintException.Message, font, Brushes.Red, this.Width * 0.5f, this.Height * 0.5f, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.PreviewKeyDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PreviewKeyDownEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            var args = new OxyKeyEventArgs { ModifierKeys = GetModifiers(), Key = e.KeyCode.Convert() };
            this.ActualController.HandleKeyDown(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Disposes the PlotView.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources or not.</param>
        protected override void Dispose(bool disposing)
        {
            bool disposed = this.IsDisposed;

            base.Dispose(disposing);

            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                this.renderContext.Dispose();
            }
        }

        /// <summary>
        /// Gets the current modifier keys.
        /// </summary>
        /// <returns>A <see cref="OxyModifierKeys" /> value.</returns>
        private static OxyModifierKeys GetModifiers()
        {
            var modifiers = OxyModifierKeys.None;

            // ReSharper disable once RedundantNameQualifier
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                modifiers |= OxyModifierKeys.Shift;
            }

            // ReSharper disable once RedundantNameQualifier
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                modifiers |= OxyModifierKeys.Control;
            }

            // ReSharper disable once RedundantNameQualifier
            if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                modifiers |= OxyModifierKeys.Alt;
            }

            return modifiers;
        }

        /// <summary>
        /// Performs the copy operation.
        /// </summary>
        private void DoCopy(IPlotView view, OxyInputEventArgs args)
        {
            var exporter = new PngExporter
            {
                Width = this.ClientRectangle.Width,
                Height = this.ClientRectangle.Height,
            };

            var bitmap = exporter.ExportToBitmap(this.ActualModel);
            Clipboard.SetImage(bitmap);
        }
    }
}
