// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods related to export.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.iOS
{
    using CoreGraphics;

    using Foundation;
    using UIKit;

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
        public static NSData ToPng(this UIView view, CGRect rect)
        {
            return view.GetImage(rect).AsPNG();
        }

        /// <summary>
        /// Gets the image for the specified <see cref="UIView" /> .
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="view">The view.</param>
        /// <param name="rect">The rectangle.</param>
        public static UIImage GetImage(this UIView view, CGRect rect)
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
        public static NSData ToPdf(this UIView view, CGRect rect)
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