// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfReportWriterTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
            r.AddPlot(new PlotModel("Plot 1"), null, 600, 400);
            r.AddPlot(new PlotModel("Plot 2"), null, 600, 400);

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
            r.AddPlot(new PlotModel("Plot 1"), "First plot", 600, 400);
            r.AddPlot(new PlotModel("Plot 2"), "Second plot", 600, 400);
            return r;
        }
    }
}