// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using ExampleLibrary;
    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PdfExporterTests
    {
        static readonly string DestinationDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "PdfExporterTests_ExampleLibrary");

        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(DestinationDirectory);
        }

        [Test]
        [TestCaseSource(typeof(ExampleLibrary.Examples), nameof(ExampleLibrary.Examples.GetListForAutomatedTest))]
        public void Export_AllExamplesInExampleLibrary_CheckThatAllFilesExist(ExampleLibrary.ExampleInfo example)
        {
            // A4
            const double Width = 297 / 25.4 * 72;
            const double Height = 210 / 25.4 * 72;

            void ExportModelAndCheckFileExists(PlotModel model, string fileName)
            {
                if (model == null)
                {
                    return;
                }

                var path = Path.Combine(DestinationDirectory, FileNameUtilities.CreateValidFileName(fileName, ".pdf"));
                using (var s = File.Create(path))
                {
                    PdfExporter.Export(model, s, Width, Height);
                }

                Assert.IsTrue(File.Exists(path));
            }

            ExportModelAndCheckFileExists(example.PlotModel, $"{example.Category} - {example.Title}");
        }
    }
}
