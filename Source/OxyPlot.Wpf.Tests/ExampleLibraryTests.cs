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
                void ExportAndCompareToBaseline(PlotModel model, string fileName)
                {
                    if (model == null)
                    {
                        return;
                    }

                    var baselinePath = Path.Combine(BaselineDirectory, fileName);
                    var path = Path.Combine(DestinationDirectory, fileName);
                    var diffpath = Path.Combine(DiffDirectory, fileName);

                    PngExporter.Export(model, path, 800, 500);
                    if (File.Exists(baselinePath))
                    {
                        PngAssert.AreEqual(baselinePath, path, example.Title, diffpath);
                    }
                    else
                    {
                        File.Copy(path, baselinePath);
                    }
                }

                ExportAndCompareToBaseline(example.PlotModel, CreateValidFileName($"{example.Category} - {example.Title}", ".png"));
            }
        }

        /// <summary>
        /// Creates a valid file name.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>A file name.</returns>
        private static string CreateValidFileName(string title, string extension)
        {
            string validFileName = title.Trim();
            var invalidFileNameChars = "/?<>\\:*|\0\t\r\n".ToCharArray();
            foreach (var invalChar in invalidFileNameChars)
            {
                validFileName = validFileName.Replace(invalChar.ToString(), string.Empty);
            }

            foreach (var invalChar in invalidFileNameChars)
            {
                validFileName = validFileName.Replace(invalChar.ToString(), string.Empty);
            }

            if (validFileName.Length > 160)
            {
                // safe value threshold is 260
                validFileName = validFileName.Remove(156) + "...";
            }

            return validFileName + extension;
        }
    }
}
