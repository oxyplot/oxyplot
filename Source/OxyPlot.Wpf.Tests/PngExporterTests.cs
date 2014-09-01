// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Tests the <see cref="PngExporter" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System.Globalization;
    using System.IO;

    using NUnit.Framework;
    using OxyPlot.Tests;

    /// <summary>
    /// Tests the <see cref="PngExporter" />.
    /// </summary>
    [TestFixture]
    public class PngExporterTests
    {
        /// <summary>
        /// Exports to a file and verifies that the file exists.
        /// </summary>
        [Test]
        public void ExportToFile()
        {
            var plotModel = TestModels.CreateTestModel1();
            const string FileName = "PngExporterTests_Plot1.png";
            var exporter = new PngExporter { Width = 400, Height = 300 };
            using (var stream = File.OpenWrite(FileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(FileName));
        }

        /// <summary>
        /// Exports with yellow background to a file and verifies that the file exists.
        /// </summary>
        [Test]
        public void ExportWithDifferentBackground()
        {
            var plotModel = TestModels.CreateTestModel1();
            const string FileName = "PngExporterTests_BackgroundYellow.png";
            var exporter = new PngExporter { Width = 400, Height = 300, Background = OxyColors.Yellow };
            using (var stream = File.OpenWrite(FileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(FileName));
        }

        /// <summary>
        /// Exports with higher resolution and verifies that the file exists.
        /// </summary>
        /// <param name="factor">The resolution factor.</param>
        [Test]
        [TestCase(2)]
        [TestCase(4)]
        public void ExportWithResolution(double factor)
        {
            var resolution = (int)(96 * factor);
            var plotModel = TestModels.CreateTestModel1();
            var fileName = string.Format(CultureInfo.InvariantCulture, "PngExporterTests_ExportWithResolution_{0}dpi.png", resolution);
            var exporter = new PngExporter { Width = (int)(400 * factor), Height = (int)(300 * factor), Resolution = resolution };
            using (var stream = File.OpenWrite(fileName))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(fileName));
            PngAssert.AreEqual(Path.Combine("Baseline", fileName), fileName, fileName, Path.Combine("Diff", fileName));
        }
    }
}