using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.Demo
{
	public class Export
	{
		private UIView view;
		private RectangleF rect;
		
		public Export (UIView view, SizeF size)
		{
			this.view = view;
			this.rect = new RectangleF(new PointF(0,0), size);
		}
		
		public NSData ToPng()
		{
			UIGraphics.BeginImageContext(rect.Size);
			view.Draw(rect);
			var image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			
			return image.AsPNG();
		}
		
		public NSData ToPdf()
		{
			var data = new NSMutableData();
			UIGraphics.BeginPDFContext(data, rect, null);
			UIGraphics.BeginPDFPage();
			view.Draw(rect);
			UIGraphics.EndPDFContent();
			
			return data;
		}
	}
}