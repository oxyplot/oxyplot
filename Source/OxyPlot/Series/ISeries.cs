// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The Series interface.
    /// </summary>
    public interface ISeries
    {
        #region Public Properties

        /// <summary>
        ///   Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the Series on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        void Render(IRenderContext rc, PlotModel model);

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="legendBox">
        /// The legend rectangle.
        /// </param>
        void RenderLegend(IRenderContext rc, OxyRect legendBox);

        #endregion
    }
}