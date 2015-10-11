// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a view that can show a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.iOS
{
    using Foundation;
    using OxyPlot;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UIKit;

    /// <summary>
    /// Provides a view that can show a <see cref="PlotModel" />. 
    /// </summary>
    [Register("PlotView")]
    public class PlotView : UIView, IPlotView
    {
        /// <summary>
        /// The current plot model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        /// The default plot controller.
        /// </summary>
        private IPlotController defaultController;

        private PanZoomGestureRecognizer panZoomGesture = new PanZoomGestureRecognizer();
               
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Xamarin.iOS.PlotView"/> class.
        /// </summary>
        public PlotView()
        {
            this.Initialize ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Xamarin.iOS.PlotView"/> class.
        /// </summary>
        /// <param name="frame">The initial frame.</param>
        public PlotView(CoreGraphics.CGRect frame) : base(frame)
        {
            this.Initialize ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Xamarin.iOS.PlotView"/> class.
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
            this.UserInteractionEnabled = true;
            this.MultipleTouchEnabled = true;
            this.BackgroundColor = UIColor.White;
            this.KeepAspectRatioWhenPinching = true;

			this.panZoomGesture.AddTarget(HandlePanZoomGesture);

            // Do not intercept touches on overlapping views
            this.panZoomGesture.ShouldReceiveTouch += (recognizer, touch) => touch.View == this;
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
                return this.Controller ?? (this.defaultController ?? (this.defaultController = new PlotController()));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OxyPlot.Xamarin.iOS.PlotView"/> keeps the aspect ratio when pinching.
        /// </summary>
        /// <value><c>true</c> if keep aspect ratio when pinching; otherwise, <c>false</c>.</value>
        public bool KeepAspectRatioWhenPinching
        {
            get { return this.panZoomGesture.KeepAspectRatioWhenPinching; }
            set { this.panZoomGesture.KeepAspectRatioWhenPinching = value; }
        }

        /// <summary>
        /// How far apart touch points must be on a certain axis to enable scaling that axis.
        /// (only applies if KeepAspectRatioWhenPinching == false)
        /// </summary>
        public double ZoomThreshold
        {
            get { return this.panZoomGesture.ZoomThreshold; }
            set { this.panZoomGesture.ZoomThreshold = value; }
        }

        /// <summary>
        /// If <c>true</c>, and KeepAspectRatioWhenPinching is <c>false</c>, a zoom-out gesture
        /// can turn into a zoom-in gesture if the fingers cross. Setting to <c>false</c> will
        /// instead simply stop the zoom at that point.
        /// </summary>
        public bool AllowPinchPastZero
        {
            get { return this.panZoomGesture.AllowPinchPastZero; }
            set { this.panZoomGesture.AllowPinchPastZero = value; }
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

            this.SetNeedsDisplay();
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(CursorType cursorType)
        {
            // No cursor on iOS
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">The tracker data.</param>
        public void ShowTracker(TrackerHitResult trackerHitResult)
        {
            // TODO: how to show a tracker on iOS
            // the tracker must be moved away from the finger...
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect rectangle)
        {
            // Not needed - better with pinch events on iOS?
        }

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetClipboardText(string text)
        {
            UIPasteboard.General.SetValue(new NSString(text), "public.utf8-plain-text");
        }

        /// <summary>
        /// Draws the content of the view.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        public override void Draw(CoreGraphics.CGRect rect)
        {
            var actualModel = (IPlotModel)this.model;
            if (actualModel != null)
            {
                var context = UIGraphics.GetCurrentContext ();
                using (var renderer = new CoreGraphicsRenderContext(context))
                {
                    if (actualModel.Background.IsVisible())
                    {
                        context.SetFillColor (actualModel.Background.ToCGColor ());
                        context.FillRect (rect);
                    }

                    actualModel.Render(renderer, rect.Width, rect.Height);
                }
            }
        }

        /// <summary>
        /// Method invoked when a motion (a shake) has started.
        /// </summary>
        /// <param name="motion">The motion subtype.</param>
        /// <param name="evt">The event arguments.</param>
        public override void MotionBegan(UIEventSubtype motion, UIEvent evt)
        {
            base.MotionBegan(motion, evt);
            if (motion == UIEventSubtype.MotionShake)
            {
                this.ActualController.HandleGesture(this, new OxyShakeGesture(), new OxyKeyEventArgs());
            }
        }

		/// <summary>
		/// Used to add/remove the gesture recognizer so that it
		/// doesn't prevent the PlotView from being garbage-collected.
		/// </summary>
		/// <param name="newsuper">New superview</param>
		public override void WillMoveToSuperview (UIView newsuper)
		{
			if (newsuper == null)
			{
				this.RemoveGestureRecognizer (this.panZoomGesture);
			}
			else if (this.Superview == null)
			{
				this.AddGestureRecognizer (this.panZoomGesture);
			}

			base.WillMoveToSuperview (newsuper);
		}

        private void HandlePanZoomGesture()
        {
            switch (panZoomGesture.State)
            {
                case UIGestureRecognizerState.Began:
                    ActualController.HandleTouchStarted(this, panZoomGesture.TouchEventArgs);
                    break;
                case UIGestureRecognizerState.Changed:
                    ActualController.HandleTouchDelta(this, panZoomGesture.TouchEventArgs);
                    break;
                case UIGestureRecognizerState.Ended:
                case UIGestureRecognizerState.Cancelled:
                    ActualController.HandleTouchCompleted(this, panZoomGesture.TouchEventArgs);
                    break;
            }
        }
    }
}