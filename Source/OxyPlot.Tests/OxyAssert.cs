// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyAssert.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides methods to assert that plots look as expected.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.IO;

    using NUnit.Framework;

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
                SvgExporter.Export(plot, s, 800, 500, false);
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