// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp.Tests
{
    using ExampleLibrary;
    using NUnit.Framework;
    using OxyPlot.SkiaSharp;
    using System.IO;

    [TestFixture]
    public class SvgExporterTests
    {
        private const string SVG_FOLDER = "SVG";
        private string outputDirectory;

        [Test]
        public void Export_SomeExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            var exporter = new SvgExporter { Width = 1000, Height = 750 };
            var directory = Path.Combine(this.outputDirectory, "ExampleLibrary");
            ExportTest.Export_FirstExampleOfEachExampleGroup_CheckThatAllFilesExist(exporter, directory, ".svg");
        }

        [Test]
        public void TestMultilineAlignment()
        {
            var exporter = new SvgExporter { Width = 1000, Height = 750 };
            var model = RenderingCapabilities.DrawMultilineTextAlignmentRotation();
            using var stream = File.Create(Path.Combine(this.outputDirectory, "Multiline-Alignment.svg"));
            exporter.Export(model, stream);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            this.outputDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, SVG_FOLDER);
            Directory.CreateDirectory(this.outputDirectory);
        }

        [Test]
        public void BackgroundColor()
        {
            var model = ShowCases.CreateNormalDistributionModel();
            model.Background = OxyColors.AliceBlue;
            var exporter = new SvgExporter { Width = 1000, Height = 750 };
            using var stream = File.Create(Path.Combine(this.outputDirectory, "Background.svg"));
            exporter.Export(model, stream);
        }

    }
}
