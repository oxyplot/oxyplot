// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphView.cs" company="OxyPlot">
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

	[Register("PlotView")]
	public sealed class PlotView : UIView
	{
		private PlotModel model;

		public PlotView ()
		{
			this.MultipleTouchEnabled = true;
		}

		public PlotModel Model 		{
			get
			{
				return this.model;
			}
			set
			{
				if (this.model != value) 	{
					this.model = value;
				}
			}
		}

		public override void InvalidateIntrinsicContentSize ()
		{
			base.InvalidateIntrinsicContentSize ();
			System.Diagnostics.Debug.WriteLine ("InvalidateIntrinsicContentSize()");
		}

		public override void Draw (System.Drawing.RectangleF rect)
		{
			this.model.Update(true);

			var context = UIGraphics.GetCurrentContext();
			if (this.model.Background.IsVisible())
			{
				context.SetFillColor (this.model.Background.ToCGColor ());
				context.FillRect (rect);
				//				this.BackgroundColor = plot.Background.ToUIColor ();
			}
		
			var renderer = new MonoTouchRenderContext(context);
			this.model.Render(renderer, rect.Width, rect.Height);
		}

		public override void MotionBegan (UIEventSubtype motion, UIEvent evt)
		{
			base.MotionBegan (motion, evt);
		}

		public override void MotionEnded (UIEventSubtype motion, UIEvent evt)
		{
			base.MotionEnded (motion, evt);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
		}

		public override void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled (touches, evt);
			System.Diagnostics.Debug.WriteLine ("TouchesCancelled");
		}
	}
}