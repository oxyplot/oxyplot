// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineJoin.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines how to join line segments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Defines how to join line segments.
    /// </summary>
    public enum LineJoin
    {
        /// <summary>
        /// Line joins use regular angular vertices.
        /// </summary>
        Miter,

        /// <summary>
        /// Line joins use rounded vertices.
        /// </summary>
        Round,

        /// <summary>
        /// Line joins use beveled vertices.
        /// </summary>
        Bevel
    }
}