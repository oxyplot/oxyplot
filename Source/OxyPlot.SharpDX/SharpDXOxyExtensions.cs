// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharpDXOxyExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents class, that contains SharpDX and Oxy extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SharpDX
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using global::SharpDX;
    using DXLineJoin = global::SharpDX.Direct2D1.LineJoin;
    using Ellipse = global::SharpDX.Direct2D1.Ellipse;

    /// <summary>
    /// Represents class, that contains SharpDX and Oxy extensions.
    /// </summary>
    internal static class SharpDXOxyExtensions
    {
        /// <summary>
        /// Converts <see cref="ScreenPoint"/> to <see cref="Vector2"/>.
        /// </summary>
        /// <param name="point">The point to convert,</param>
        /// <param name="aliased">Indicate, whether result should be aliased</param>
        /// <returns>Return <see cref="Vector2"/></returns>
        public static Vector2 ToVector2(this ScreenPoint point, bool aliased = false)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            if (aliased)
            {
                return new Vector2(0.5f + (int)point.X, 0.5f + (int)point.Y);
            }

            return new Vector2((float)point.X, (float)point.Y);
        }

        /// <summary>
        /// Converts <see cref="OxyRect"/> to <see cref="Ellipse"/>.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>Return <see cref="Ellipse"/></returns>
        public static Ellipse ToEllipse(this OxyRect rect)
        {
            return new Ellipse(rect.Center.ToVector2(), (float)rect.Width / 2, (float)rect.Height / 2);
        }

        /// <summary>
        /// Converts <see cref="OxyRect"/> to <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>Return <see cref="RectangleF"/></returns>
        public static RectangleF ToRectangleF(this OxyRect rect)
        {
            return new RectangleF((float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

        /// <summary>
        /// Converts <see cref="LineJoin"/> to <see cref="DXLineJoin"/>.
        /// </summary>
        /// <param name="lineJoin">The <see cref="LineJoin"/> to convert.</param>
        /// <returns>Return <see cref="LineJoin"/></returns>
        public static DXLineJoin ToDXLineJoin(this LineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case LineJoin.Miter:
                    return DXLineJoin.Miter;
                case LineJoin.Round:
                    return DXLineJoin.Round;
                case LineJoin.Bevel:
                    return DXLineJoin.Bevel;
                default:
                    return DXLineJoin.MiterOrBevel;
            }
        }

        /// <summary>
        /// Converts <see cref="OxyColor"/> to <see cref="Color4"/>.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>Return <see cref="Color4"/></returns>
        public static Color4 ToDXColor(this OxyColor color)
        {
            return new Color4(color.R * 1f / 255f, color.G * 1f / 255f, color.B * 1f / 255f, color.A * 1f / 255f);
        }
    }
}
