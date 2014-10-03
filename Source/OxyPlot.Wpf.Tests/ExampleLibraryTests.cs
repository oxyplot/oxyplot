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
                    FileNameUtilities.CreateValidFileName(example.Category + " - " + example.Title, ".png"));
                var path = Path.Combine(
                    DestinationDirectory,
                    FileNameUtilities.CreateValidFileName(example.Category + " - " + example.Title, ".png"));
                var diffpath = Path.Combine(
                    DestinationDirectory,
                    FileNameUtilities.CreateValidFileName(example.Category + " - " + example.Title, ".DIFF.png"));
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