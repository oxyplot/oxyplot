// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a view that can show a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.MonoTouch
{
    using global::MonoTouch.Foundation;
    using global::MonoTouch.UIKit;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot;

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

        /// <summary>
        /// Up to 2 touches being currently tracked in a pan/zoom.
        /// </summary>
        private List<UITouch> activeTouches = new List<UITouch>();

        /// <summary>
        /// How far apart touch points must be on a certain axis to enable scaling that axis
        /// </summary>
        private const double threshold = 20d;


        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.XamarinIOS.PlotView"/> class.
        /// </summary>
        public PlotView()
        {
            this.Initialize ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.XamarinIOS.PlotView"/> class.
        /// </summary>
        /// <param name="frame">The initial frame.</param>
        public PlotView(System.Drawing.RectangleF frame) : base(frame)
        {
            this.Initialize ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.XamarinIOS.PlotView"/> class.
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
                return this.Controller ?? (this.defaultController ?? (this.defaultController = new PlotController()));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OxyPlot.XamarinIOS.PlotView"/> keeps the aspect ratio when pinching.
        /// </summary>
        /// <value><c>true</c> if keep aspect ratio when pinching; otherwise, <c>false</c>.</value>
        public bool KeepAspectRatioWhenPinching { get; set; }

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
                this.BackgroundColor = actualModel.Background.ToUIColor();
            }
            else
            {
                // Use white as default background color
                this.BackgroundColor = UIColor.White;
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
        public override void Draw(System.Drawing.RectangleF rect)
        {
            if (this.model != null)
            {
                using (var renderer = new MonoTouchRenderContext(UIGraphics.GetCurrentContext()))
                {
                    ((IPlotModel)this.model).Render(renderer, rect.Width, rect.Height);
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
        /// Called when a touch gesture begins.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            if (this.activeTouches.Count >= 2)
            {
                // we already have two touches
                return;
            }

            // Grab 1-2 touches to track
            var newTouches = touches.ToArray<UITouch>();
            var firstTouch = !this.activeTouches.Any();

            activeTouches.AddRange(newTouches.Take(2 - this.activeTouches.Count));

            if (firstTouch)
            {
                // HandleTouchStarted initializes the entire multitouch gesture,
                // with the first touch used for panning.
                //
                ActualController.HandleTouchStarted(this, this.activeTouches.First().ToTouchEventArgs(this));
            }
        }

        /// <summary>
        /// Called when a touch gesture is moving.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            if (activeTouches.Count > 0)
            {
                // get current and previous location of the first touch point
                var t1 = this.activeTouches.First();
                var l1 = t1.LocationInView(this).ToScreenPoint();
                var pl1 = t1.Phase == UITouchPhase.Moved ? t1.PreviousLocationInView(this).ToScreenPoint() : l1;

                var l = l1;
                var t = l1 - pl1;
                var s = new ScreenVector(1, 1);

                if (activeTouches.Count > 1)
                {
                    // get current and previous location of the second touch point
                    var t2 = this.activeTouches.ElementAt(1);
                    var l2 = t2.LocationInView(this).ToScreenPoint();
                    var pl2 = t2.Phase == UITouchPhase.Moved ? t2.PreviousLocationInView(this).ToScreenPoint() : l2;

                    var d = l1 - l2;
                    var pd = pl1 - pl2;

                    if (!this.KeepAspectRatioWhenPinching)
                    {
                        var scalex = Math.Abs(pd.X) > PlotView.threshold && Math.Abs(d.X) > PlotView.threshold ? Math.Abs(d.X / pd.X) : 1;
                        var scaley = Math.Abs(pd.Y) > PlotView.threshold && Math.Abs(d.Y) > PlotView.threshold ? Math.Abs(d.Y / pd.Y) : 1;
                        s = new ScreenVector(scalex, scaley);
                    }
                    else
                    {
                        var scale = pd.Length > 0 ? d.Length / pd.Length : 1;
                        s = new ScreenVector(scale, scale);
                    }
                }

                var e = new OxyTouchEventArgs { Position = l, DeltaTranslation = t, DeltaScale = s };
                this.ActualController.HandleTouchDelta(this, e);
            }
        }

        /// <summary>
        /// Called when a touch gesture ends.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            // We already have the only two touches we care about, so ignore the params
            //
            var secondTouch = this.activeTouches.ElementAtOrDefault(1);

            if (secondTouch != null && secondTouch.Phase == UITouchPhase.Ended)
            {
                this.activeTouches.Remove(secondTouch);
            }

            var firstTouch = this.activeTouches.FirstOrDefault();

            if (firstTouch != null && firstTouch.Phase == UITouchPhase.Ended)
            {
                this.activeTouches.Remove(firstTouch);

                ActualController.HandleTouchCompleted(this, firstTouch.ToTouchEventArgs(this));

                if (secondTouch != null && secondTouch.Phase != UITouchPhase.Ended)
                {
                    // Restart with the former-secondary touch now promoted to primary touch (congratulations!)
                    //
                    ActualController.HandleTouchStarted(this, secondTouch.ToTouchEventArgs(this));
                }
            }
        }

        /// <summary>
        /// Called when a touch gesture is cancelled.
        /// </summary>
        /// <param name="touches">The touches.</param>
        /// <param name="evt">The event arguments.</param>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            // TODO: Is it possible for one touch to be canceled while others remain in play?

            var touch = this.activeTouches.FirstOrDefault();
            if (touch != null && touch.Phase == UITouchPhase.Cancelled)
            {
                this.ActualController.HandleTouchCompleted(this, touch.ToTouchEventArgs(this));
            }
        }
    }
}