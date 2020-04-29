// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkerType.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the marker type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Defines the marker type.
    /// </summary>
    public enum MarkerType
    {
        /// <summary>
        /// Do not render markers.
        /// </summary>
        None,

        /// <summary>
        /// Render markers as circles.
        /// </summary>
        Circle,

        /// <summary>
        /// Render markers as squares.
        /// </summary>
        Square,

        /// <summary>
        /// Render markers as diamonds.
        /// </summary>
        Diamond,

        /// <summary>
        /// Render markers as triangles.
        /// </summary>
        Triangle,

        /// <summary>
        /// Render markers as crosses (note: this marker type requires the stroke color to be set).
        /// </summary>
        /// <remarks>This marker type requires the stroke color to be set.</remarks>
        Cross,

        /// <summary>
        /// Renders markers as plus signs (note: this marker type requires the stroke color to be set).
        /// </summary>
        /// <remarks>This marker type requires the stroke color to be set.</remarks>
        Plus,

        /// <summary>
        /// Renders markers as stars (note: this marker type requires the stroke color to be set).
        /// </summary>
        /// <remarks>This marker type requires the stroke color to be set.</remarks>
        Star,

        /// <summary>
        /// Render markers by a custom shape (defined by outline).
        /// </summary>
        Custom
    }
}