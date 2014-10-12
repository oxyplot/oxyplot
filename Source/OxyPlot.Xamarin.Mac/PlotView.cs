// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a view that can show a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.Mac
{
    using Foundation;
    using AppKit;

    using OxyPlot;

    /// <summary>
    /// Provides a view that can show a <see cref="PlotModel" />. 
    /// </summary>
    [Register("PlotView")]
    public class PlotView : NSView, IPlotView
    {
        /// <summary>
        /// The current plot model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        /// The default plot controller.
        /// </summary>
        private IPlotController defaultController;
               
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Xamarin.Mac.PlotView"/> class.
        /// </summary>
        public PlotView()
        {
            this.Initialize ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Xamarin.Mac.PlotView"/> class.
        /// </summary>
        /// <param name="frame">The initial frame.</param>
        public PlotView(CoreGraphics.CGRect frame) : base(frame)
        {
            this.Initialize ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Xamarin.Mac.PlotView"/> class.
        /// </summary>
        /// <param name="coder">Coder.</param>
        [Export ("initWithCoder:")]
        public PlotView(NSCoder coder) : base (coder)
        {
            this.Initialize ();
        }

        /// <summary>
        /// Uses the new layout.
        /// </summary>
        /// <returns><c>true</c>, if new layout was used, <c>false</c> otherwise.</returns>
        [Export ("requiresConstraintBasedLayout")]
        bool UseNewLayout ()
        {
            return true;
        }

        /// <summary>
        /// Initialize the view.
        /// </summary>
        private void Initialize() {
            this.AcceptsTouchEvents = true;
        }

        /// <summary>
        /// Gets or sets the <see cref="PlotModel"/> to show in the view. 
        /// </summary>
        /// <value>The <see cref="PlotModel"/>.</value>
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
                    this.InvalidatePlot();
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IPlotController"/> that handles input events.
        /// </summary>
        /// <value>The <see cref="IPlotController"/>.</value>
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
        /// Gets the actual <see cref="PlotModel"/> to show.
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
                // TODO
                return new OxyRect(0, 0, 100, 100);
            }
        }

        /// <summary>
        /// Gets the actual <see cref="IPlotController"/>.
        /// </summary>
        /// <value>The actual plot controller.</value>
        public IPlotController ActualController
        {
            get
            {
                return this.Controller ?? (this.defaultController ?? (this.defaultController = CreateDefaultController()));
            }
        }

        private PlotController CreateDefaultController(){
            var c = new PlotController ();
            c.UnbindMouseDown (OxyMouseButton.Left);
            c.BindMouseDown (OxyMouseButton.Left, PlotCommands.PanAt);
            return c;
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
        /// <param name="updateData">If set to <c>true</c> update data.</param>
        public void InvalidatePlot(bool updateData = true)
        {
            var actualModel = this.model;
            if (actualModel != null)
            {
                // TODO: update the model on a background thread
                ((IPlotModel)actualModel).Update(updateData);
            }

            if (actualModel != null && !actualModel.Background.IsUndefined())
            {
                // this.BackgroundColor = actualModel.Background.ToUIColor();
            }
            else
            {
                // Use white as default background color
                // this.BackgroundColor = UIColor.White;
            }

            this.NeedsDisplay = true;
//            this.SetNeedsDisplay();
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(CursorType cursorType)
        {
            this.ResetCursorRects ();
            var cursor = Convert (cursorType);
            if (cursor!=null)
            this.AddCursorRect (this.Bounds, cursor);
        }

        public static NSCursor Convert(CursorType cursorType){
            switch (cursorType) {
            case CursorType.Default:
                return null;
            case CursorType.Pan:
                return NSCursor.PointingHandCursor;
            case CursorType.ZoomHorizontal:
                return NSCursor.ResizeUpDownCursor;
            case CursorType.ZoomVertical:
                return NSCursor.ResizeLeftRightCursor;
            case CursorType.ZoomRectangle:
                return NSCursor.CrosshairCursor;
            default:
                return null;
            }
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">The tracker data.</param>
        public void ShowTracker(TrackerHitResult trackerHitResult)
        {
            // TODO
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect rectangle)
        {
            // TODO
        }

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetClipboardText(string text)
        {
            // TODO
            // UIPasteboard.General.SetValue(new NSString(text), "public.utf8-plain-text");
        }

        /// <summary>
        /// Draws the content of the view.
        /// </summary>
        /// <param name="dirtyRect">The rectangle to draw.</param>
        public override void DrawRect(CoreGraphics.CGRect dirtyRect)
        {
            if (this.model != null)
            {
                var context = NSGraphicsContext.CurrentContext.GraphicsPort;
                context.TranslateCTM(0f, dirtyRect.Height);
                context.ScaleCTM(1f, -1f);
                // TODO: scale font matrix??
                using (var renderer = new CoreGraphicsRenderContext(context))
                {
                        ((IPlotModel)this.model).Render(renderer, dirtyRect.Width, dirtyRect.Height);
                    }
            }
        }

        public override void MouseDown (NSEvent theEvent)
        {
            base.MouseDown (theEvent);
            this.ActualController.HandleMouseDown (this, theEvent.ToMouseDownEventArgs(this.Bounds));
        }

        public override void MouseDragged (NSEvent theEvent)
        {
            base.MouseDragged (theEvent);
            this.ActualController.HandleMouseMove (this, theEvent.ToMouseEventArgs (this.Bounds));
        }

        public override void MouseMoved (NSEvent theEvent)
        {
            base.MouseMoved (theEvent);
            this.ActualController.HandleMouseMove (this, theEvent.ToMouseEventArgs (this.Bounds));
        }

        public override void MouseUp (NSEvent theEvent)
        {
            base.MouseUp (theEvent);
            this.ActualController.HandleMouseUp (this, theEvent.ToMouseEventArgs (this.Bounds));
        }

        public override void MouseEntered (NSEvent theEvent)
        {
            base.MouseEntered (theEvent);
            this.ActualController.HandleMouseEnter (this, theEvent.ToMouseEventArgs (this.Bounds));
        }

        public override void MouseExited (NSEvent theEvent)
        {
            base.MouseExited (theEvent);
            this.ActualController.HandleMouseLeave (this, theEvent.ToMouseEventArgs (this.Bounds));
        }

        public override void ScrollWheel (NSEvent theEvent)
        {
            // TODO: use scroll events to pan?
            base.ScrollWheel (theEvent);
            this.ActualController.HandleMouseWheel (this, theEvent.ToMouseWheelEventArgs (this.Bounds));
        }

        public override void OtherMouseDown (NSEvent theEvent)
        {
            base.OtherMouseDown (theEvent);
        }

        public override void RightMouseDown (NSEvent theEvent)
        {
            base.RightMouseDown (theEvent);
        }

        public override void KeyDown (NSEvent theEvent)
        {
            base.KeyDown (theEvent);
            this.ActualController.HandleKeyDown (this, theEvent.ToKeyEventArgs ());
        }

        public override void TouchesBeganWithEvent (NSEvent theEvent)
        {
            base.TouchesBeganWithEvent (theEvent);
        }

        public override void MagnifyWithEvent (NSEvent theEvent)
        {
            base.MagnifyWithEvent (theEvent);
            // TODO: handle pinch event
            // https://developer.apple.com/library/mac/documentation/cocoa/conceptual/eventoverview/HandlingTouchEvents/HandlingTouchEvents.html
        }

        public override void SmartMagnify (NSEvent withEvent)
        {
            base.SmartMagnify (withEvent);
        }

        public override void SwipeWithEvent (NSEvent theEvent)
        {
            base.SwipeWithEvent (theEvent);
        }
    }
}