// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationLayer.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The annotation layer.
    /// </summary>
    public enum AnnotationLayer
    {
        /// <summary>
        ///   Render the annotation below the gridlines of the axes.
        /// </summary>
        BelowAxes,
        
        /// <summary>
        ///   Render the annotation below the series.
        /// </summary>
        BelowSeries, 

        /// <summary>
        ///   Render the annotation above the series.
        /// </summary>
        AboveSeries
    }
}