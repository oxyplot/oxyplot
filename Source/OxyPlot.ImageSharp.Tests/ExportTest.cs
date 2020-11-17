// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportTest.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.ImageSharp.Tests
{
    using ExampleLibrary;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class ExportTest
    {
        public static void ExportExamples_CheckThatAllFilesExist(IEnumerable<ExampleInfo> examples, IExporter exporter, string directory, string extension)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (var example in examples)
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
