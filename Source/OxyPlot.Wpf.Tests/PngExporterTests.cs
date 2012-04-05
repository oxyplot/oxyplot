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
        public PlotModel CreateSimpleModel()
        {
            var model = new PlotModel("Test");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
            model.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 100, "sin(x)"));
            return model;
        }

        [Test]
        public void Export_SimpleModel_CheckThatFileExists()
        {
            var model = this.CreateSimpleModel();
            const string fileName = "plot1.png";
            PngExporter.Export(model, fileName, 400, 300);
            Assert.IsTrue(File.Exists(fileName));
        }

        [Test]
        public void Export_AllExamplesInExampleLibrary()
        {
            var destinationDirectory = "ExampleLibrary";

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            foreach (var example in ExampleLibrary.Examples.GetList())
            {
                var path = Path.Combine(destinationDirectory, StringHelper.CreateValidFileName(example.Category + " - " + example.Title, ".png"));
                PngExporter.Export(example.PlotModel, path, 800, 500, OxyColors.White);
            }
        }

    }
}