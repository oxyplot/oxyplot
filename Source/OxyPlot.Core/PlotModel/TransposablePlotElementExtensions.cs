// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransposablePlotElementExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the TransposablePlotElementExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The transposable plot element extensions.
    /// </summary>
    public static class TransposablePlotElementExtensions
    {
        /// <summary>
        /// Gets the clipping rectangle.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <returns>The clipping rectangle.</returns>
        public static OxyRect GetClippingRect(this ITransposablePlotElement element)
        {
            var p1 = new ScreenPoint(element.XAxis.ScreenMin.X, element.YAxis.ScreenMin.Y);
            var p2 = new ScreenPoint(element.XAxis.ScreenMax.X, element.YAxis.ScreenMax.Y);
            return new OxyRect(element.Orientate(p1), element.Orientate(p2));
        }

        /// <summary>
        /// Transforms from a screen point to a data point by the axes of this series.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="p">The screen point.</param>
        /// <returns>A data point.</returns>
        public static DataPoint InverseTransform(this ITransposablePlotElement element, ScreenPoint p)
        {
            p = element.Orientate(p);
            return element.XAxis.InverseTransform(p.X, p.Y, element.YAxis);
        }

        /// <summary>
        /// Checks if the series is transposed.
        /// </summary>
        /// <returns>True if the series is transposed, False otherwise.</returns>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        public static bool IsTransposed(this ITransposablePlotElement element)
        {
            return element.XAxis.IsVertical();
        }

        /// <summary>
        /// Transposes the ScreenPoint if the series is transposed.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="point">The <see cref="ScreenPoint" /> to orientate.</param>
        /// <returns>The oriented point.</returns>
        public static ScreenPoint Orientate(this ITransposablePlotElement element, ScreenPoint point)
        {
            return element.IsTransposed() ? new ScreenPoint(point.Y, point.X) : point;
        }

        /// <summary>
        /// Transposes the ScreenVector if the series is transposed. Reverses the respective direction if X or Y axis are reversed.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="vector">The <see cref="ScreenVector" /> to orientate.</param>
        /// <returns>The oriented vector.</returns>
        public static ScreenVector Orientate(this ITransposablePlotElement element, ScreenVector vector)
        {
            vector = new ScreenVector(
                element.XAxis.IsReversed ? -vector.X : vector.X,
                element.YAxis.IsReversed ? -vector.Y : vector.Y);
            return element.IsTransposed() ? new ScreenVector(-vector.Y, -vector.X) : vector;
        }

        /// <summary>
        /// Orientates a HorizontalAlignment and a VerticalAlignment according to whether the Series is transposed or the Axes are reversed.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="ha">The <see cref="HorizontalAlignment" /> to orientate.</param>
        /// <param name="va">The <see cref="VerticalAlignment" /> to orientate.</param>
        public static void Orientate(
            this ITransposablePlotElement element,
            ref HorizontalAlignment ha,
            ref VerticalAlignment va)
        {
            if (element.XAxis.IsReversed)
            {
                ha = (HorizontalAlignment)(-(int)ha);
            }

            if (element.YAxis.IsReversed)
            {
                va = (VerticalAlignment)(-(int)va);
            }

            if (element.IsTransposed())
            {
                var orientatedHa = (HorizontalAlignment)(-(int)va);
                va = (VerticalAlignment)(-(int)ha);
                ha = orientatedHa;
            }
        }

        /// <summary>
        /// Transforms the specified coordinates to a screen point by the axes of this series.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A screen point.</returns>
        public static ScreenPoint Transform(this ITransposablePlotElement element, double x, double y)
        {
            return element.Orientate(element.XAxis.Transform(x, y, element.YAxis));
        }

        /// <summary>
        /// Transforms the specified data point to a screen point by the axes of this series.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="p">The point.</param>
        /// <returns>A screen point.</returns>
        public static ScreenPoint Transform(this ITransposablePlotElement element, DataPoint p)
        {
            return element.Transform(p.X, p.Y);
        }
    }
}
