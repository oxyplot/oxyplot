// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JpegExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.ImageSharp.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;

    using OxyPlot.Series;
    using OxyPlot.ImageSharp;
    using OxyPlot.Annotations;

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
            var exporter = new JpegExporter(400, 300);
            var directory = Path.Combine(this.outputDirectory, "ExampleLibrary");
            ExportTest.Export_FirstExampleOfEachExampleGroup_CheckThatAllFilesExist(exporter, directory, ".jpg");
        }

        [Test]
        public void ExportToStream()
        {
            var plotModel = CreateTestModel1();
            var exporter = new JpegExporter(400, 300);
            var stream = new MemoryStream();
            exporter.Export(plotModel, stream);

            Assert.IsTrue(stream.Length > 0);
        }

        [Test]
        public void ExportToFile()
        {
            var plotModel = CreateTestModel1();
            var fileName = Path.Combine(this.outputDirectory, "Plot1.jpg");
            JpegExporter.Export(plotModel, fileName, 400, 300);

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void ExportWithDifferentBackground()
        {
            var plotModel = CreateTestModel1();
            plotModel.Background = OxyColors.Yellow;
            var fileName = Path.Combine(this.outputDirectory, "Background_Yellow.jpg");
            var exporter = new JpegExporter(400, 300);
            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        [TestCase(0.75)]
        [TestCase(1.2)]
        [TestCase(2)]
        [TestCase(3.1415)]
        public void ExportWithResolution(double factor)
        {
            var resolution = (int)(96 * factor);
            var plotModel = CreateTestModel1();
            var directory = Path.Combine(this.outputDirectory, "Resolution");
            Directory.CreateDirectory(directory);

            var fileName = Path.Combine(directory, $"Resolution{resolution}.jpg");
            var exporter = new JpegExporter((int)(400 * factor), (int)(300 * factor), resolution);

            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void PlotBackgroundImageTest(bool interpolate)
        {
            // this is a test of the DrawImage function; don't add pointless backgrounds to your plots

            var plotModel = CreateTestModel1();

            var pixelData = new OxyColor[5, 5];
            for (int i = 0; i < pixelData.GetLength(0); i++)
            {
                for (int j = 0; j < pixelData.GetLength(1); j++)
                {
                    pixelData[i, j] = OxyColor.FromArgb(255, 128, (byte)((i * 255) / pixelData.GetLength(0)), (byte)((j * 255) / pixelData.GetLength(1)));
                }
            }

            var oxyImage = OxyImage.Create(pixelData, ImageFormat.Png);
            var imageAnnotation = new ImageAnnotation()
            {
                ImageSource = oxyImage,
                X = new PlotLength(-0.0, PlotLengthUnit.RelativeToPlotArea),
                Y = new PlotLength(-0.0, PlotLengthUnit.RelativeToPlotArea),
                Width = new PlotLength(1.0, PlotLengthUnit.RelativeToPlotArea),
                Height = new PlotLength(1.0, PlotLengthUnit.RelativeToPlotArea),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Interpolate = interpolate
            };
            plotModel.Annotations.Add(imageAnnotation);

            var fileName = Path.Combine(this.outputDirectory, $"PlotBackground{(interpolate ? "Interpolated" : "Pixelated")}.jpg");
            var exporter = new JpegExporter(400, 300);
            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void LargeImageTest(bool interpolate)
        {
            // this is a test of the DrawImage function; don't add pointless backgrounds to your plots

            var plotModel = CreateTestModel1();

            var pixelData = new OxyColor[5, 5];
            for (int i = 0; i < pixelData.GetLength(0); i++)
            {
                for (int j = 0; j < pixelData.GetLength(1); j++)
                {
                    pixelData[i, j] = OxyColor.FromArgb(255, 128, (byte)((i * 255) / pixelData.GetLength(0)), (byte)((j * 255) / pixelData.GetLength(1)));
                }
            }

            var oxyImage = OxyImage.Create(pixelData, ImageFormat.Png);
            var imageAnnotation = new ImageAnnotation()
            {
                ImageSource = oxyImage,
                X = new PlotLength(-1, PlotLengthUnit.RelativeToViewport),
                Y = new PlotLength(-1, PlotLengthUnit.RelativeToViewport),
                Width = new PlotLength(3, PlotLengthUnit.RelativeToViewport),
                Height = new PlotLength(3, PlotLengthUnit.RelativeToViewport),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Interpolate = interpolate
            };
            plotModel.Annotations.Add(imageAnnotation);

            var fileName = Path.Combine(this.outputDirectory, $"LargeImage{(interpolate ? "Interpolated" : "Pixelated")}.jpg");
            var exporter = new JpegExporter(400, 300);
            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void TestMultilineAlignment()
        {
            var plotModel = ExampleLibrary.RenderingCapabilities.DrawMultilineTextAlignmentRotation();
            var fileName = Path.Combine(this.outputDirectory, "Text.png");
            JpegExporter.Export(plotModel, fileName, 700, 700);

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
