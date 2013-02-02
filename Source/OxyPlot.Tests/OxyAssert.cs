// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyAssert.cs" company="OxyPlot">
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
//   Provides methods to assert that plots look as expected.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Tests
{
    using System.IO;

    using NUnit.Framework;

    using OxyPlot.Wpf;

    /// <summary>
    /// Provides methods to assert that plots look as expected.
    /// </summary>
    public static class OxyAssert
    {
        /// <summary>
        /// Asserts that a plot is equal to the plot stored in the "baseline" folder.
        /// 1. Renders the plot to file.svg
        /// 2. If the baseline does not exist, the current plot is copied to the baseline folder.
        /// 3. Checks that the svg file is equal to a baseline svg.
        /// </summary>
        /// <param name="plot">The plot.</param>
        /// <param name="name">The name of the baseline file.</param>
        public static void AreEqual(PlotModel plot, string name)
        {
            // string name = new System.Diagnostics.StackFrame(1).GetMethod().Name;
            string path = name + ".svg";
            string baseline = @"baseline\" + path;
            using (var s = File.Create(path))
            {
                var rc = new ShapesRenderContext(null);
                SvgExporter.Export(plot, s, 800, 500, false, rc);
            }

            if (!Directory.Exists("baseline"))
            {
                Directory.CreateDirectory("baseline");
            }

            if (!File.Exists(baseline))
            {
                File.Copy(path, baseline);
                return;
            }

            var baselineSvg = File.ReadAllText(baseline);
            var actualSvg = File.ReadAllText(path);

            Assert.IsTrue(string.Equals(baselineSvg, actualSvg), "Actual svg is not equal to baseline (" + Path.GetFullPath(baseline) + ")");
        }
    }
}