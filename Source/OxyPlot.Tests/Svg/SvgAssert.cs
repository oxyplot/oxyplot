// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgAssert.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides methods to assert against the svg schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    /// <summary>
    /// Provides methods to assert against the svg schema.
    /// </summary>
    public static class SvgAssert
    {
        /// <summary>
        /// Asserts that the specified file is a valid svg file.
        /// </summary>
        /// <param name="path">The path to the svg file.</param>
        public static void IsValidFile(string path)
        {
            // todo
        }

        /// <summary>
        /// Asserts that the specified string is a valid svg document (including ?xml and !DOCTYPE).
        /// </summary>
        /// <param name="content">The svg document.</param>
        public static void IsValidDocument(string content)
        {
            // todo
        }

        /// <summary>
        /// Asserts that the specified string is a valid svg element (<svg>...</svg>).
        /// </summary>
        /// <param name="content">The svg element.</param>
        public static void IsValidElement(string content)
        {
            // todo
        }
    }
}