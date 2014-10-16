// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods that converts between MonoTouch and OxyPlot types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.XamarinIOS
{
    using System;
    using System.Drawing;

    using MonoTouch.CoreGraphics;
    using MonoTouch.UIKit;
    using OxyPlot;

    /// <summary>
    /// Provides extension methods that converts between MonoTouch and OxyPlot types.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Converts a <see cref="System.Drawing.PointF" /> to a <see cref="ScreenPoint" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
        public static ScreenPoint ToScreenPoint(this PointF p)
        {
            return new ScreenPoint(p.X, p.Y);
        }

        /// <summary>
        /// Converts <see cref="UITouch" /> event arguments to <see cref="OxyTouchEventArgs" />.
        /// </summary>
        /// <param name="touch">The touch event arguments.</param>
        /// <param name="view">The view.</param>
        /// <returns>The converted arguments.</returns>
        public static OxyTouchEventArgs ToTouchEventArgs(this UITouch touch, UIView view)
        {
            var location = touch.LocationInView(view);
            return new OxyTouchEventArgs
            {
                Position = new ScreenPoint(location.X, location.Y),
                DeltaTranslation = new ScreenVector(0, 0),
                DeltaScale = new ScreenVector(1, 1)
            };
        }

        /// <summary>
        /// Converts a <see cref="OxyColor" /> to a <see cref="CGColor" />.
        /// </summary>
        /// <param name="c">The color to convert.</param>
        /// <returns>The converted color.</returns>
        // ReSharper disable once InconsistentNaming
        public static CGColor ToCGColor(this OxyColor c)
        {
            return new CGColor(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        /// <summary>
        /// Converts a <see cref="OxyColor" /> to a <see cref="UIColor" />.
        /// </summary>
        /// <param name="c">The color to convert.</param>
        /// <returns>The converted color.</returns>
        // ReSharper disable once InconsistentNaming
        public static UIColor ToUIColor(this OxyColor c)
        {
            return new UIColor(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        /// <summary>
        /// Converts a <see cref="LineJoin" /> to a <see cref="CGLineCap" />.
        /// </summary>
        /// <param name="lineJoin">The line join.</param>
        /// <returns>The converted join.</returns>
        public static CGLineJoin Convert(this LineJoin lineJoin)
        {
            switch (lineJoin)
            {
            case LineJoin.Bevel:
                return CGLineJoin.Bevel;
            case LineJoin.Miter:
                return CGLineJoin.Miter;
            case LineJoin.Round:
                return CGLineJoin.Round;
            default:
                throw new InvalidOperationException("Invalid join type.");
            }
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a <see cref="PointF" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
        public static PointF Convert(this ScreenPoint p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a pixel center aligned <see cref="PointF" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
        public static PointF ConvertAliased(this ScreenPoint p)
        {
            return new PointF(0.5f + (float)Math.Round(p.X), 0.5f + (float)Math.Round(p.Y));
        }

        /// <summary>
        /// Converts a <see cref="OxyRect" /> to a pixel center aligned <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>The converted rectangle.</returns>
        public static RectangleF ConvertAliased(this OxyRect rect)
        {
            float x = 0.5f + (float)Math.Round(rect.Left);
            float y = 0.5f + (float)Math.Round(rect.Top);
            float ri = 0.5f + (float)Math.Round(rect.Right);
            float bo = 0.5f + (float)Math.Round(rect.Bottom);
            return new RectangleF(x, y, ri - x, bo - y);
        }

        /// <summary>
        /// Converts a <see cref="OxyRect" /> to a <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>The converted rectangle.</returns>
        public static RectangleF Convert(this OxyRect rect)
        {
            return new RectangleF((float)rect.Left, (float)rect.Top, (float)(rect.Right - rect.Left), (float)(rect.Bottom-rect.Top));
        }
    }
}