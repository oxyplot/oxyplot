// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationLayer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies the layer for an <see cref="Annotation" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Specifies the layer for an <see cref="Annotation" />.
    /// </summary>
    public enum AnnotationLayer
    {
        /// <summary>
        /// Render the annotation below the gridlines of the axes.
        /// </summary>
        BelowAxes,

        /// <summary>
        /// Render the annotation below the series.
        /// </summary>
        BelowSeries,

        /// <summary>
        /// Render the annotation above the series.
        /// </summary>
        AboveSeries
    }
}