// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleLibraryTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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

    using OxyPlot.Tests;

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
        public void ExportPngAndCompareWithBaseline()
        {
            const string DestinationDirectory = "ExampleLibrary.Actual";
            const string BaselineDirectory = "ExampleLibrary.Baseline";
            const string DiffDirectory = "ExampleLibrary.Diff";

            if (!Directory.Exists(BaselineDirectory))
            {
                Directory.CreateDirectory(BaselineDirectory);
            }

            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            if (!Directory.Exists(DiffDirectory))
            {
                Directory.CreateDirectory(DiffDirectory);
            }

            foreach (var example in ExampleLibrary.Examples.GetList())
            {
                if (example.PlotModel == null)
                {
                    continue;
                }

                var filename = FileNameUtilities.CreateValidFileName(example.Category + " - " + example.Title, ".png");
                var baselinePath = Path.Combine(BaselineDirectory, filename);
                var path = Path.Combine(DestinationDirectory, filename);
                var diffpath = Path.Combine(DiffDirectory, filename);

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