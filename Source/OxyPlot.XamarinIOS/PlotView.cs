// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
			this.UserInteractionEnabled = true;
			this.MultipleTouchEnabled = true;
			this.KeepAspectRatioWhenPinching = true;
		}
			
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

		public PlotModel ActualModel {
			get {
				return this.Model;
			}
		}

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
				this.model.Update (updateData);
			}

			this.SetNeedsDisplay ();
		}

		public void SetCursorType (CursorType cursorType)
		{
		}

		public void ShowTracker (TrackerHitResult trackerHitResult)
		{
		}

		public void ShowZoomRectangle (OxyRect rectangle)
		{
		}

		public void SetClipboardText (string text)
		{
		}

		public override void InvalidateIntrinsicContentSize ()
		{
			base.InvalidateIntrinsicContentSize ();
			System.Diagnostics.Debug.WriteLine ("InvalidateIntrinsicContentSize()");
		}

		public override void Draw (System.Drawing.RectangleF rect)
		{
			var context = UIGraphics.GetCurrentContext ();
			if (this.model.Background.IsVisible ()) {
				context.SetFillColor (this.model.Background.ToCGColor ());
				context.FillRect (rect);
				// TODO: is it possible to set the background color?
				// this.BackgroundColor = plot.Background.ToUIColor ();
			}
		
			var renderer = new MonoTouchRenderContext (context);
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