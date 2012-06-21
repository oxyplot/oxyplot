using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoTouch.Demo
{
	public static class Export
	{
		public static NSData ToPng(this UIView view, RectangleF rect)
		{
			return view.GetImage(rect).AsPNG();
		}
		
		public static UIImage GetImage(this UIView view, RectangleF rect)
		{
			UIGraphics.BeginImageContext(rect.Size);
			view.Draw(rect);
			var image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			
			return image;
		}
		
		public static NSData ToPdf(this UIView view, RectangleF rect)
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