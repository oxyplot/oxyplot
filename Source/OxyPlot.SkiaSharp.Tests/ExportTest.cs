// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportTest.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp.Tests
{
    using ExampleLibrary;
    using NUnit.Framework;
    using System.IO;
    using System.Linq;

    public static class ExportTest
    {
        public static void Export_FirstExampleOfEachExampleGroup_CheckThatAllFilesExist(IExporter exporter, string directory, string extension)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (var example in Examples.GetList().GroupBy(example => example.Category).Select(g => g.First()))
            {
                void ExportModelAndCheckFileExists(PlotModel model, string fileName)
                {
                    if (model == null)
                    {
                        return;
                    }

                    var path = Path.Combine(directory, FileNameUtilities.CreateValidFileName(fileName, extension));
                    using (var s = File.Create(path))
                    {
                        exporter.Export(model, s);
                    }

                    Assert.IsTrue(File.Exists(path));
                }

                ExportModelAndCheckFileExists(example.PlotModel, $"{example.Category} - {example.Title}");
            }
        }
    }
}
