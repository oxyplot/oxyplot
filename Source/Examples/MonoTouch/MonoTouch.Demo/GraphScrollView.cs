using System;
using MonoTouch.UIKit;
using System.Drawing;

using OxyPlot;
using OxyPlot.MonoTouch;
using ExampleLibrary;

namespace MonoTouch.Demo
{
	public class GraphScrollView : UIScrollView
	{
		public GraphScrollView (ExampleInfo exampleInfo, RectangleF rect)
			: base(rect)
		{
			ShowsVerticalScrollIndicator = true;
			ShowsHorizontalScrollIndicator = true;
			BouncesZoom = true;
			PagingEnabled = false;
			DecelerationRate = UIScrollView.DecelerationRateNormal;
			BackgroundColor = UIColor.DarkGray;
			MaximumZoomScale = 5f;
			MinimumZoomScale = 1f;
			ContentSize = new SizeF(rect.Size.Width * 5, rect.Size.Height * 5);
			
			var image = new UIImageView(new GraphView(exampleInfo).GetImage(rect));
			
			AddSubview(image);
			
			this.ViewForZoomingInScrollView = delegate(UIScrollView scrollView) {
				return image;
			};
		}
	}
}

