// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotElementExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The transposable plot element extensions.
    /// </summary>
    public static class PlotElementExtensions
    {
        /// <summary>
        /// Transforms from a screen point to a data point by the axes of this series.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="x">The x coordinate of the screen point.</param>
        /// <param name="y">The y coordinate of the screen point.</param>
        /// <returns>A data point.</returns>
        public static DataPoint InverseTransform(this IXyAxisPlotElement element, double x, double y)
        {
            return element.InverseTransform(new ScreenPoint(x, y));
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
        public static void Orientate(this ITransposablePlotElement element, ref HorizontalAlignment ha, ref VerticalAlignment va)
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
        /// Transforms the specified data point to a screen point by the axes of this series.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="x">The x coordinate of the data point.</param>
        /// <param name="y">The y coordinate of the data point.</param>
        /// <returns>A screen point.</returns>
        public static ScreenPoint Transform(this IXyAxisPlotElement element, double x, double y)
        {
            return element.Transform(new DataPoint(x, y));
        }
    }
}
