﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using ExampleLibrary;
    using NUnit.Framework;

    using OxyPlot.Axes;
    using OxyPlot.Pdf;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PdfExporterTests
    {
        [Test]
        public void Export_AllExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            var destinationDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "PdfExporterTests_ExampleLibrary");
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            // A4
            const double Width = 297 / 25.4 * 72;
            const double Height = 210 / 25.4 * 72;

            foreach (var example in Examples.GetListForAutomatedTest())
            {
                void ExportModelAndCheckFileExists(PlotModel model, string fileName)
                {
                    if (model == null)
                    {
                        return;
                    }

                    var path = Path.Combine(destinationDirectory, FileNameUtilities.CreateValidFileName(fileName, ".pdf"));
                    using (var s = File.Create(path))
                    {
                        PdfExporter.Export(model, s, Width, Height);
                    }

                    Assert.IsTrue(File.Exists(path));
                }

                ExportModelAndCheckFileExists(example.PlotModel, $"{example.Category} - {example.Title}");
            }
        }

        [Test]
        public void Export_Unicode()
        {
            var model = new PlotModel { Title = "Unicode support ☺", DefaultFont = "Arial" };
            model.Axes.Add(new LinearAxis { Title = "λ", Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Title = "Ж", Position = AxisPosition.Left });
            var exporter = new PdfExporter { Width = 400, Height = 400 };
            using (var stream = File.OpenWrite("Unicode.pdf"))
            {
                exporter.Export(model, stream);
            }
        }
    }
}
