// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotFigure.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a plot figure.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a plot figure.
    /// </summary>
    public class PlotFigure : Figure
    {
        /// <summary>
        /// Gets or sets the height of the figure.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the plot source.
        /// </summary>
        public PlotModel PlotModel { get; set; }

        /// <summary>
        /// Gets or sets the width of the figure.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Writes the figure to the specified <see cref="IReportWriter" />.
        /// </summary>
        /// <param name="w">The target <see cref="IReportWriter" />.</param>
        public override void WriteContent(IReportWriter w)
        {
            w.WritePlot(this);
        }
    }
}