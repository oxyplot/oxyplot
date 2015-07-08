// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a view that can show a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.Android
{
    using System;

    using global::Android.Content;
    using global::Android.Graphics;
    using global::Android.Util;
    using global::Android.Views;

    using OxyPlot;

    /// <summary>
    /// Represents a view that can show a <see cref="PlotModel" />.
    /// </summary>
    public class PlotView : View, IPlotView
    {
        /// <summary>
        /// The factor that scales from OxyPlot´s device independent pixels (96 dpi) to 
        /// Android´s density-independent pixels (160 dpi).
        /// </summary>
        /// <remarks>See <a href="http://developer.android.com/guide/practices/screens_support.html">Supporting multiple screens.</a>.</remarks>
        public const double Scale = 160d / 96d;

        /// <summary>
        /// The rendering lock object.
        /// </summary>
        private readonly object renderingLock = new object();

        /// <summary>
        /// The invalidation lock object.
        /// </summary>
        private readonly object invalidateLock = new object();

        /// <summary>
        /// The touch points of the previous touch event.
        /// </summary>
        private ScreenPoint[] previousTouchPoints;

        /// <summary>
        /// The current model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        /// The default controller
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// The current render context.
        /// </summary>
        private CanvasRenderContext rc;

        /// <summary>
        /// The model invalidated flag.
        /// </summary>
        private bool isModelInvalidated;

        /// <summary>
        /// The update data flag.
        /// </summary>
        private bool updateDataFlag = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>Use this constructor when creating the view from code.</remarks>
        public PlotView(Context context) :
            base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attribute set.</param>
        /// <remarks>This constructor is called when inflating the view from XML.</remarks>
        public PlotView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attribute set.</param>
        /// <param name="defStyle">The definition style.</param>
        /// <remarks>This constructor performs inflation from XML and applies a class-specific base style.</remarks>
        public PlotView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
        }

        /// <summary>
        /// Gets or sets the plot model.
        /// </summary>
        /// <value>The model.</value>
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
                    if (this.model != null)
                    {
                        ((IPlotModel)this.model).AttachPlotView(null);
                        this.model = null;
                    }

                    if (value != null)
                    {
                        ((IPlotModel)value).AttachPlotView(this);
                        this.model = value;
                    }

                    this.InvalidatePlot();
                }
            }
        }

        /// <summary>
        /// Gets or sets the plot controller.
        /// </summary>
        /// <value>The controller.</value>
        public IPlotController Controller { get; set; }

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
                return new OxyRect(0, 0, this.Width, this.Height);
            }
        }

        /// <summary>
        /// Gets the actual <see cref="IPlotController" /> of the control.
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
        }

        /// <summary>
        /// Invalidates the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data bindings will be updated.</param>
        public void InvalidatePlot(bool updateData = true)
        {
            lock (this.invalidateLock)
            {
                this.isModelInvalidated = true;
                this.updateDataFlag = this.updateDataFlag || updateData;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(CursorType cursorType)
        {
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">The tracker data.</param>
        public void ShowTracker(TrackerHitResult trackerHitResult)
        {
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect rectangle)
        {
        }

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetClipboardText(string text)
        {
        }

        /// <summary>
        /// Handles key down events.
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <param name="e">The event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            var handled = base.OnKeyDown(keyCode, e);
            if (!handled)
            {
                handled = this.ActualController.HandleKeyDown(this, e.ToKeyEventArgs());
            }

            return handled;
        }

        /// <summary>
        /// Handles touch screen motion events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            var handled = base.OnTouchEvent(e);
            if (!handled)
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                        handled = this.OnTouchDownEvent(e);
                        break;
                    case MotionEventActions.Move:
                        handled = this.OnTouchMoveEvent(e);
                        break;
                    case MotionEventActions.Up:
                        handled = this.OnTouchUpEvent(e);
                        break;
                }
            }

            return handled;
        }

        /// <summary>
        /// Draws the content of the control.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            var actualModel = this.ActualModel;
            if (actualModel == null)
            {
                return;
            }

            if (!actualModel.Background.IsUndefined())
            {
                canvas.DrawColor(actualModel.Background.ToColor());
            }
            else
            {
                // Use white as default background color
                canvas.DrawColor(Color.White);
            }

            lock (this.invalidateLock)
            {
                if (this.isModelInvalidated)
                {
                    ((IPlotModel)actualModel).Update(this.updateDataFlag);
                    this.updateDataFlag = false;
                    this.isModelInvalidated = false;
                }
            }

            lock (this.renderingLock)
            {
                if (this.rc == null)
                {
                    this.rc = new CanvasRenderContext(Scale);
                }

                this.rc.SetTarget(canvas);
                using (var bounds = new Rect())
                {
                    canvas.GetClipBounds(bounds);
                    var width = bounds.Right - bounds.Left;
                    var height = bounds.Bottom - bounds.Top;
                    ((IPlotModel)actualModel).Render(this.rc, width / Scale, height / Scale);
                }
            }
        }

        /// <summary>
        /// Handles touch down events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        private bool OnTouchDownEvent(MotionEvent e)
        {
            var args = e.ToTouchEventArgs(Scale);
            var handled = this.ActualController.HandleTouchStarted(this, args);
            this.previousTouchPoints = e.GetTouchPoints(Scale);
            return handled;
        }

        /// <summary>
        /// Handles touch move events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        private bool OnTouchMoveEvent(MotionEvent e)
        {
            var currentTouchPoints = e.GetTouchPoints(Scale);
            var args = new OxyTouchEventArgs(currentTouchPoints, this.previousTouchPoints);
            var handled = this.ActualController.HandleTouchDelta(this, args);
            this.previousTouchPoints = currentTouchPoints;
            return handled;
        }

        /// <summary>
        /// Handles touch released events.
        /// </summary>
        /// <param name="e">The motion event arguments.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        private bool OnTouchUpEvent(MotionEvent e)
        {
            return this.ActualController.HandleTouchCompleted(this, e.ToTouchEventArgs(Scale));
        }
    }
}