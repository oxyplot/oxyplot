// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgAssert.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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