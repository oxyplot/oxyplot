// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelUtilities.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides utility functions for plot elements.
    /// </summary>
    public static class PlotElementUtilities
    {
        /// <summary>
        /// Gets the clipping rectangle defined by the Axis the <see cref="IXyAxisPlotElement"/> uses.
        /// </summary>
        /// <param name="element">The <see cref="IXyAxisPlotElement" />.</param>
        /// <returns>The clipping rectangle.</returns>
        public static OxyRect GetClippingRect(IXyAxisPlotElement element)
        {
            var p1 = new ScreenPoint(element.XAxis.ScreenMin.X, element.YAxis.ScreenMin.Y);
            var p2 = new ScreenPoint(element.XAxis.ScreenMax.X, element.YAxis.ScreenMax.Y);
            return new OxyRect(p1, p2);
        }

        /// <summary>
        /// Gets the clipping rectangle defined by the Axis the <see cref="ITransposablePlotElement"/> uses while being aware of the orientation.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <returns>The clipping rectangle.</returns>
        public static OxyRect GetOrientatedClippingRect(ITransposablePlotElement element)
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
        public static DataPoint InverseTransform(IXyAxisPlotElement element, ScreenPoint p)
        {
            return element.XAxis.InverseTransform(p.X, p.Y, element.YAxis);
        }

        /// <summary>
        /// Transforms from a screen point to a data point by the axes of this series while being aware of the orientation.
        /// </summary>
        /// <param name="element">The <see cref="ITransposablePlotElement" />.</param>
        /// <param name="p">The screen point.</param>
        /// <returns>A data point.</returns>
        public static DataPoint InverseTransformOrientated(ITransposablePlotElement element, ScreenPoint p)
        {
            return InverseTransform(element, element.Orientate(p));
        }

        /// <summary>
        /// Transforms the specified coordinates to a screen point by the axes of the plot element.
        /// </summary>
        /// <param name="element">The plot element.</param>
        /// <param name="p">The data point.</param>
        /// <returns>A screen point.</returns>
        public static ScreenPoint Transform(IXyAxisPlotElement element, DataPoint p)
        {
            return element.XAxis.Transform(p.X, p.Y, element.YAxis);
        }

        /// <summary>
        /// Transforms the specified coordinates to a screen point by the axes of the plot element while being aware of the orientation.
        /// </summary>
        /// <param name="element">The plot element.</param>
        /// <param name="p">The data point.</param>
        /// <returns>A screen point.</returns>
        public static ScreenPoint TransformOrientated(ITransposablePlotElement element, DataPoint p)
        {
            return element.Orientate(Transform(element, p));
        }
    }
}
