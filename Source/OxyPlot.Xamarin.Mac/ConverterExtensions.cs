// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods that converts between MonoTouch and OxyPlot types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.Mac
{
    using System;

    using CoreGraphics;
 
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
        public static ScreenPoint ToScreenPoint(this CGPoint p)
        {
            return new ScreenPoint(p.X, p.Y);
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
        /// Converts a <see cref="ScreenPoint" /> to a <see cref="CGPoint" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
        public static CGPoint Convert(this ScreenPoint p)
        {
            return new CGPoint((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a pixel center aligned <see cref="CGPoint" />.
        /// </summary>
        /// <param name="p">The point to convert.</param>
        /// <returns>The converted point.</returns>
        public static CGPoint ConvertAliased(this ScreenPoint p)
        {
            return new CGPoint(0.5f + (int)p.X, 0.5f + (int)p.Y);
        }

        /// <summary>
        /// Converts a <see cref="OxyRect" /> to a pixel center aligned <see cref="CGRect" />.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>The converted rectangle.</returns>
        public static CGRect ConvertAliased(this OxyRect rect)
        {
            float x = 0.5f + (int)rect.Left;
            float y = 0.5f + (int)rect.Top;
            float ri = 0.5f + (int)rect.Right;
            float bo = 0.5f + (int)rect.Bottom;
            return new CGRect(x, y, ri - x, bo - y);
        }

        /// <summary>
        /// Converts a <see cref="OxyRect" /> to a <see cref="CGRect" />.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>The converted rectangle.</returns>
        public static CGRect Convert(this OxyRect rect)
        {
            var left = (int)rect.Left;
            var right = (int)rect.Right;
            var top = (int)rect.Top;
            var bottom = (int)rect.Bottom;
            return new CGRect(left, top, right - left, bottom - top);
        }
    }
}