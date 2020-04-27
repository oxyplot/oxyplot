// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JpegExporterTests.cs" company="OxyPlot">
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
    public class JpegExporterTests
    {
        private const string JPEG_FOLDER = "JPEG";
        private string outputDirectory;

        [OneTimeSetUp]
        public void Setup()
        {
            this.outputDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, JPEG_FOLDER);
            Directory.CreateDirectory(this.outputDirectory);
        }

        [Test]
        public void Export_SomeExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            var exporter = new JpegExporter { Width = 400, Height = 300 };
            var directory = Path.Combine(this.outputDirectory, "ExampleLibrary");
            ExportTest.Export_FirstExampleOfEachExampleGroup_CheckThatAllFilesExist(exporter, directory, ".jpg");
        }

        [Test]
        public void ExportWithDifferentBackground()
        {
            var plotModel = CreateTestModel1();
            plotModel.Background = OxyColors.Yellow;
            var exporter = new PngExporter { Width = 400, Height = 300 };
            var fileName = Path.Combine(this.outputDirectory, "BackgroundYellow.jpg");
            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(90)]
        [TestCase(100)]
        public void ExportWithQuality(int quality)
        {
            var plotModel = CreateTestModel1();
            var directory = Path.Combine(this.outputDirectory, "Quality");
            Directory.CreateDirectory(directory);
            var exporter = new JpegExporter { Width = 400, Height = 300, Quality = quality };

            var fileName = Path.Combine(directory, $"Quality_{quality}.jpg");
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
