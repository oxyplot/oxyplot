// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporterTests.cs" company="OxyPlot">
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

    using OxyPlot.Series;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class SvgExporterTests
    {
        [Test]
        public void ExportToString_TestPlot_ValidSvgDocument()
        {
            var plotModel = new PlotModel { Title = "Test plot" };
            plotModel.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "Math.Sin"));
            var svg = SvgExporter.ExportToString(plotModel, 800, 500, true);
            SvgAssert.IsValidDocument(svg);
        }

        [Test]
        public void ExportToString_TestPlot_ValidSvgString()
        {
            var plotModel = new PlotModel { Title = "Test plot" };
            plotModel.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "Math.Sin"));
            var svg = SvgExporter.ExportToString(plotModel, 800, 500, false);
            SvgAssert.IsValidElement(svg);
        }

        [Test]
        public void Export_TestPlot_ValidSvgString()
        {
            var plotModel = new PlotModel { Title = "Test plot" };
            const string FileName = "SvgExporterTests_Plot1.svg";
            plotModel.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "Math.Sin"));
            using (var s = File.Create(FileName))
            {
                SvgExporter.Export(plotModel, s, 800, 500, true);
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void Export_AllExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            const string DestinationDirectory = "SvgExporterTests_ExampleLibrary";
            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            foreach (var example in Examples.GetList())
            {
                if (example.PlotModel == null)
                {
                    continue;
                }

                var path = Path.Combine(DestinationDirectory, FileNameUtilities.CreateValidFileName(example.Category + " - " + example.Title, ".svg"));
                using (var s = File.Create(path))
                {
                    SvgExporter.Export(example.PlotModel, s, 800, 500, true);
                }

                Assert.IsTrue(File.Exists(path));
            }
        }
    }
}