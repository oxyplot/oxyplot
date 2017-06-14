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
        /// Exports to a stream and verifies that the stream is not empty.
        /// </summary>
        [Test]
        public void ExportToStream()
        {
            var plotModel = TestModels.CreateTestModel1();
            var exporter = new PngExporter { Width = 400, Height = 300 };
            var stream = new MemoryStream();
            exporter.Export(plotModel, stream);

            Assert.IsTrue(stream.Length > 0);
        }

        /// <summary>
        /// Exports to a file and verifies that the file exists.
        /// </summary>
        [Test]
        public void ExportToFile()
        {
            var plotModel = TestModels.CreateTestModel1();
            const string FileName = "PngExporterTests_Plot1.png";
            var exporter = new PngExporter { Width = 400, Height = 300 };
            exporter.ExportToFile(plotModel, FileName);

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
        [Ignore("Issue #759")]
        [Test]
        [TestCase(2)]
        [TestCase(4)]
        public void ExportWithResolution(double factor)
        {
            var resolution = (int)(96 * factor);
            var plotModel = TestModels.CreateTestModel1();
            Directory.CreateDirectory("Actual");
            var fileName = string.Format(CultureInfo.InvariantCulture, "PngExporterTests_ExportWithResolution_{0}dpi.png", resolution);
            var exporter = new PngExporter { Width = (int)(400 * factor), Height = (int)(300 * factor), Resolution = resolution };
            var actual = Path.Combine("Actual", fileName);
            using (var stream = File.OpenWrite(actual))
            {
                exporter.Export(plotModel, stream);
            }

            Assert.IsTrue(File.Exists(actual));
            PngAssert.AreEqual(Path.Combine("Baseline", fileName), actual, fileName, Path.Combine("Diff", fileName));
        }
    }
}