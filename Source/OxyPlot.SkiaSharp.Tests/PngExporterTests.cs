// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp.Tests
{
    using System;
    using System.IO;

    using NUnit.Framework;

    using OxyPlot.Series;
    using OxyPlot.SkiaSharp;

    [TestFixture]
    public class PngExporterTests
    {
        private const string PNG_FOLDER = "PNG";
        private string outputDirectory;

        [OneTimeSetUp]
        public void Setup()
        {
            this.outputDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, PNG_FOLDER);
            Directory.CreateDirectory(this.outputDirectory);
        }

        [Test]
        public void Export_SomeExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            var exporter = new PngExporter { Width = 400, Height = 300 };
            var directory = Path.Combine(this.outputDirectory, "ExampleLibrary");
            ExportTest.Export_FirstExampleOfEachExampleGroup_CheckThatAllFilesExist(exporter, directory, ".png");
        }

        [Test]
        public void ExportToStream()
        {
            var plotModel = CreateTestModel1();
            var exporter = new PngExporter { Width = 400, Height = 300 };
            var stream = new MemoryStream();
            exporter.Export(plotModel, stream);

            Assert.IsTrue(stream.Length > 0);
        }

        [Test]
        public void ExportToFile()
        {
            var plotModel = CreateTestModel1();
            var fileName = Path.Combine(this.outputDirectory, "Plot1.png");
            PngExporter.Export(plotModel, fileName, 400, 300);

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void ExportWithDifferentBackground()
        {
            var plotModel = CreateTestModel1();
            plotModel.Background = OxyColors.Yellow;
            var fileName = Path.Combine(this.outputDirectory, "Background_Yellow.png");
            var exporter = new PngExporter { Width = 400, Height = 300 };
            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        [TestCase(1.2)]
        [TestCase(2)]
        [TestCase(3.1415)]
        public void ExportWithResolution(double factor)
        {
            var resolution = (int)(96 * factor);
            var plotModel = CreateTestModel1();
            var directory = Path.Combine(this.outputDirectory, "Resolution");
            Directory.CreateDirectory(directory);

            var fileName = Path.Combine(directory, $"Resolution{resolution}.png");
            var exporter = new PngExporter { Width = (int)(400 * factor), Height = (int)(300 * factor), Dpi = resolution };

            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        private static PlotModel CreateTestModel1()
        {
            var model = new PlotModel { Title = "Test 1" };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "sin(x)"));
            return model;
        }
    }
}
