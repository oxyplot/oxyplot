// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp.Tests
{
    using NUnit.Framework;
    using OxyPlot.SkiaSharp;
    using System.IO;

    [TestFixture]
    public class SkiaSvgExporterTests
    {
        private const string SVG_FOLDER = "SkiaSVG";
        private string outputDirectory;

        [Test]
        public void Export_SomeExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            var exporter = new SkiaSvgExporter { Width = 1000, Height = 750 };
            var directory = Path.Combine(this.outputDirectory, "ExampleLibrary");
            ExportTest.Export_FirstExampleOfEachExampleGroup_CheckThatAllFilesExist(exporter, directory, ".svg");
        }

        [OneTimeSetUp]
        public void Setup()
        {
            this.outputDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, SVG_FOLDER);
            Directory.CreateDirectory(this.outputDirectory);
        }
    }
}
