// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfReportWriterTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using NUnit.Framework;

    using OxyPlot.Reporting;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PdfReportWriterTests
    {
        [Test]
        public void ExportToStream_ReportWithPlots_CheckThatOutputFileExists()
        {
            const string FileName = "ExportToStream.pdf";
            var s = File.Create(FileName);
            using (var w = new PdfReportWriter(s))
            {
                w.WriteReport(CreateReport(), new ReportStyle());
            }

            Assert.IsTrue(new FileInfo(FileName).Length > 0);
        }

        [Test]
        public void ExportToFileName_ReportWithPlots_CheckThatOutputFileExists()
        {
            const string FileName = "ExportToFileName.pdf";
            using (var w = new PdfReportWriter(FileName))
            {
                w.WriteReport(CreateReport(), new ReportStyle());
            }

            Assert.IsTrue(new FileInfo(FileName).Length > 0);
        }

        [Test]
        public void ExportToStream_ReportWithNoCaptions_CheckThatOutputFileExists()
        {
            const string FileName = "ReportWithNoPlotCaptions.pdf";
            var s = File.Create(FileName);
            var r = new Report();
            r.AddHeader(1, "Test");
            r.AddPlot(new PlotModel { Title = "Plot 1" }, null, 600, 400);
            r.AddPlot(new PlotModel { Title = "Plot 2" }, null, 600, 400);

            using (var w = new PdfReportWriter(s))
            {
                w.WriteReport(r, new ReportStyle());
            }

            Assert.IsTrue(new FileInfo(FileName).Length > 0);
        }

        private static Report CreateReport()
        {
            var r = new Report();
            r.AddHeader(1, "Test");
            r.AddPlot(new PlotModel { Title = "Plot 1" }, "First plot", 600, 400);
            r.AddPlot(new PlotModel { Title = "Plot 2" }, "Second plot", 600, 400);
            return r;
        }
    }
}