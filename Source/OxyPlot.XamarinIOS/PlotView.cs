// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.XamarinIOS
{
    using MonoTouch.CoreGraphics;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    using OxyPlot;

    [Register ("PlotView")]
    public class PlotView : UIView, IPlotControl
    {
        private PlotModel model;
        private IPlotController controller;
        private IPlotController defaultController;

        public PlotView ()
        {
			// TODO: virtual method calls in constructor, how to avoid this?
            this.UserInteractionEnabled = true;
            this.MultipleTouchEnabled = true;
            this.KeepAspectRatioWhenPinching = true;
			this.BackgroundColor = UIColor.White;
        }

		/// <summary>
		/// Gets or sets the <see cref="PlotModel"/> tho show in the view. 
		/// </summary>
		/// <value>The <see cref="PlotModel"/>.</value>
		public PlotModel Model {
            get {
                return this.model;
            }

            set {
                if (this.model != value) {
                    this.model = value;
                    this.InvalidatePlot (true);
                }
            }

        }

		/// <summary>
		/// Gets or sets the <see cref="IPlotController"/> that handles input events.
		/// </summary>
		/// <value>The <see cref="IPlotController"/>.</value>
        public IPlotController Controller {
            get {
                return this.controller;
            }

            set {
                if (this.controller != value) {
                    this.controller = value;
                }
            }
        }

		/// <summary>
		/// Gets the actual <see cref="PlotModel"/> to show.
		/// </summary>
		/// <value>The actual model.</value>
        public PlotModel ActualModel {
            get {
                return this.Model;
            }
        }

		/// <summary>
		/// Gets the actual <see cref="IPlotController"/>.
		/// </summary>
		/// <value>The actual plot controller.</value>
        public IPlotController ActualController {
            get {
                return this.Controller ?? (this.defaultController ?? (this.defaultController = new PlotController ()));
            }
        }

        public bool KeepAspectRatioWhenPinching { get; set;}

        public void HideTracker ()
        {
        }

        public void HideZoomRectangle ()
        {
        }

        public void InvalidatePlot (bool updateData = true)
        {
            if (this.model != null) {
				// TODO: update the model on a background thread
                this.model.Update (updateData);
            }

			var backgroundColor = this.model.Background;
			if (!backgroundColor.IsVisible()) {
				backgroundColor = OxyColors.White;
			}

			this.BackgroundColor = backgroundColor.ToUIColor ();
            this.SetNeedsDisplay ();
        }

        public void SetCursorType (CursorType cursorType)
        {
			// No cursor on iOS
        }

        public void ShowTracker (TrackerHitResult trackerHitResult)
        {
			// TODO: how to show a tracker on iOS
			// the tracker must be moved away from the finger...
        }

        public void ShowZoomRectangle (OxyRect rectangle)
        {
			// Not needed - better with pinch events on iOS?
        }

        public void SetClipboardText (string text)
        {
			UIPasteboard.General.SetValue (new NSString (text), "public.utf8-plain-text");
        }

		/// <summary>
		/// Draws the content of the view.
		/// </summary>
		/// <param name="rect">The rectangle to draw.</param>
        public override void Draw (System.Drawing.RectangleF rect)
        {
			var renderer = new MonoTouchRenderContext (UIGraphics.GetCurrentContext ());
            this.model.Render (renderer, rect.Width, rect.Height);
        }

        public override void MotionBegan (UIEventSubtype motion, UIEvent evt)
        {
            base.MotionBegan (motion, evt);
            if (motion == UIEventSubtype.MotionShake) {
                this.ActualController.HandleGesture (this, new OxyShakeGesture (), new OxyKeyEventArgs());
            }
        }

        public override void TouchesBegan (NSSet touches, UIEvent evt)
        {
            base.TouchesBegan (touches, evt);
            var touch = touches.AnyObject as UITouch;
            if (touch != null) {
                this.ActualController.HandleTouchStarted (this, touch.ToTouchEventArgs (this));
            }
        }

        public override void TouchesMoved (NSSet touches, UIEvent evt)
        {
            // it seems to be easier to handle touch events here than using UIPanGesturRecognizer and UIPinchGestureRecognizer
            base.TouchesMoved (touches, evt);

            // convert the touch points to an array
            var ta = touches.ToArray<UITouch> ();

            if (ta.Length > 0) {
                // get current and previous location of the first touch point
                var t1 = ta [0];
                var l1 = t1.LocationInView (this).ToScreenPoint ();
                var pl1 = t1.PreviousLocationInView (this).ToScreenPoint ();
                var l = l1;
                var t = l1 - pl1;
                var s = new ScreenVector (1, 1);
                if (ta.Length > 1) {
                    // get current and previous location of the second touch point
                    var t2 = ta [1];
                    var l2 = t2.LocationInView (this).ToScreenPoint ();
                    var pl2 = t2.PreviousLocationInView (this).ToScreenPoint ();
                    var d = l1 - l2;
                    var pd = pl1 - pl2;
                    if (!this.KeepAspectRatioWhenPinching) {
                        var scalex = System.Math.Abs (pd.X) > 0 ? System.Math.Abs (d.X / pd.X) : 1;
                        var scaley = System.Math.Abs (pd.Y) > 0 ? System.Math.Abs (d.Y / pd.Y) : 1;
                        s = new ScreenVector (scalex, scaley);
                    } else {
                        var scale = pd.Length > 0 ? d.Length / pd.Length : 1;
                        s = new ScreenVector (scale, scale);
                    }
                }

                var e = new OxyTouchEventArgs { Position = l, DeltaTranslation = t, DeltaScale = s };
                this.ActualController.HandleTouchDelta (this, e);
            }
        }

        public override void TouchesEnded (NSSet touches, UIEvent evt)
        {
            base.TouchesEnded (touches, evt);
            var touch = touches.AnyObject as UITouch;
            if (touch != null) {
                this.ActualController.HandleTouchCompleted (this, touch.ToTouchEventArgs (this));
            }
        }

        public override void TouchesCancelled (NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled (touches, evt);
            var touch = touches.AnyObject as UITouch;
            if (touch != null) {
                this.ActualController.HandleTouchCompleted (this, touch.ToTouchEventArgs (this));
            }
        }
    }
}