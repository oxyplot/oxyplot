// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineAnnotationType.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies the definition of the line in a <see cref="LineAnnotation" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Specifies the definition of the line in a <see cref="LineAnnotation" />.
    /// </summary>
    public enum LineAnnotationType
    {
        /// <summary>
        /// Horizontal line given by the Y property
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical line given by the X property
        /// </summary>
        Vertical,

        /// <summary>
        /// Linear equation y=mx+b given by the Slope and Intercept properties
        /// </summary>
        LinearEquation
    }
}