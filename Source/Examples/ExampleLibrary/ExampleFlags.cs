// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleFlags.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    /// <summary>
    /// Properties of an example.
    /// </summary>
    [Flags]
    public enum ExampleFlags
    {
        /// <summary>
        /// Transpose the axes, so that horizontal axes become vertical and vice versa.
        /// </summary>
        Transpose = 1,

        /// <summary>
        /// Reverse the axes, so that their start and end positions are mirrored within the plot area.
        /// </summary>
        Reverse = 2,
    }
}
