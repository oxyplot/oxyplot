// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportExtensions.cs" company="OxyPlot">
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
    using System.Drawing;

    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

	/// <summary>
	/// Provides extension methods related to export.
	/// </summary>
    public static class ExportExtensions
    {
		/// <summary>
		/// Stores the specified <see cref="UIView" /> to a PNG file.
		/// </summary>
		/// <returns>The PNG data.</returns>
		/// <param name="view">The view to export.</param>
		/// <param name="rect">The rectangle to export.</param>
        public static NSData ToPng(this UIView view, RectangleF rect)
        {
            return view.GetImage(rect).AsPNG();
        }

		/// <summary>
		/// Gets the image for the specified <see cref="UIView" /> .
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="view">The view.</param>
		/// <param name="rect">The rectangle.</param>
        public static UIImage GetImage(this UIView view, RectangleF rect)
        {
            UIGraphics.BeginImageContext(rect.Size);
            view.Draw(rect);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

		/// <summary>
		/// Exports the specified <see cref="UIView" /> to a PDF file.
		/// </summary>
		/// <returns>The PDF data.</returns>
		/// <param name="view">The view to export.</param>
		/// <param name="rect">The rectangle to export.</param>
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