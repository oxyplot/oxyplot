// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleLibraryTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Provides unit tests for the example library.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.IO;

    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the example library.
    /// </summary>
    public class ExampleLibraryTests
    {
        /// <summary>
        /// Compares all examples with a baseline in "ExampleLibrary.Baseline". Creates the baseline
        /// if it does not exist.
        /// </summary>
        [Test]
        public void CompareWithBaseline()
        {
            const string DestinationDirectory = "ExampleLibrary";
            const string BaselineDirectory = "ExampleLibrary.Baseline";

            if (!Directory.Exists(BaselineDirectory))
            {
                Directory.CreateDirectory(BaselineDirectory);
            }

            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            foreach (var example in ExampleLibrary.Examples.GetList())
            {
                if (example.PlotModel == null)
                {
                    continue;
                }

                var baselinePath = Path.Combine(
                    BaselineDirectory,
                    StringHelper.CreateValidFileName(example.Category + " - " + example.Title, ".png"));
                var path = Path.Combine(
                    DestinationDirectory,
                    StringHelper.CreateValidFileName(example.Category + " - " + example.Title, ".png"));
                var diffpath = Path.Combine(
                    DestinationDirectory,
                    StringHelper.CreateValidFileName(example.Category + " - " + example.Title, ".DIFF.png"));
                Console.WriteLine(path);
                PngExporter.Export(example.PlotModel, path, 800, 500, OxyColors.White);
                if (File.Exists(baselinePath))
                {
                    PngAssert.AreEqual(baselinePath, path, example.Title, diffpath);
                }
                else
                {
                    File.Copy(path, baselinePath);
                }
            }
        }
    }
}