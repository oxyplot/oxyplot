// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporterTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.IO;
    using OxyPlot;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for PdfExporter.
    /// </summary>
    [TestFixture]
    public class PngExporterTests
    {
        [Test]
        public PlotModel CreateModel()
        {
            var model = new PlotModel("Test");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
            model.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 100, "sin(x)"));
            return model;
        }

        [Test]
        public void ExportToFile()
        {
            PlotModel model = this.CreateModel();
            const string FileName = "plot1.png";
            PngExporter.Export(model, FileName, 400, 300);
            Assert.IsTrue(File.Exists(FileName));
        }
    }
}