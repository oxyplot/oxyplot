// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotFigure.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a plot figure.
    /// </summary>
    public class PlotFigure : Figure
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets Height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        ///   Gets or sets PlotModel.
        /// </summary>
        public PlotModel PlotModel { get; set; }

        /// <summary>
        ///   Gets or sets Width.
        /// </summary>
        public double Width { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        public override void WriteContent(IReportWriter w)
        {
            w.WritePlot(this);
        }

        #endregion
    }
}