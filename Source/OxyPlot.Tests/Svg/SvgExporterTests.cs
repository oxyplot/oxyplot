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
    using System.Linq;
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
                void ExportModelAndCheckFileExists(PlotModel model, string fileName)
                {
                    if (model == null)
                    {
                        return;
                    }

                    var path = Path.Combine(DestinationDirectory, FileNameUtilities.CreateValidFileName(fileName, ".svg"));
                    using (var s = File.Create(path))
                    {
                        SvgExporter.Export(model, s, 800, 500, true);
                    }

                    Assert.IsTrue(File.Exists(path));
                }

                ExportModelAndCheckFileExists(example.PlotModel, $"{example.Category} - {example.Title}");

                if (example.IsTransposable)
                {
                    ExportModelAndCheckFileExists(example.GetModel(ExampleFlags.Transpose), $"{example.Category} - {example.Title} - Transposed");
                }

                if (example.IsReversible)
                {
                    ExportModelAndCheckFileExists(example.GetModel(ExampleFlags.Reverse), $"{example.Category} - {example.Title} - Reversed");
                }
            }
        }

        [Test]
        public void Export_BoundedTest()
        {
            const string DestinationDirectory = "SvgExporterTests_Meh";
            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            var fileName = "BoundedTest";
            var path = Path.Combine(DestinationDirectory, FileNameUtilities.CreateValidFileName(fileName, ".svg"));

            var width = 550;
            var height = 550;
            var whole = new OxyRect(0, 0, width, height);

            var rect = new OxyRect(50, 50, 400, 400);
            //var model = ExampleLibrary.PolarPlotExamples.ArchimedeanSpiral();
            var model = ExampleLibrary.AxisExamples.PositionTier();
            model.Title = "Title";
            model.Subtitle = "SubTitle";
            model.PlotAreaBorderColor = OxyColors.Black;
            model.PlotAreaBorderThickness = new OxyThickness(1.0);
            var textMeasurer = new PdfRenderContext(width, height, model.Background);
            
            using (var stream = new FileStream(path, FileMode.Create))
            using (var rc = new SvgRenderContext(stream, width, height, false, textMeasurer, model.Background, true))
            {
                ((IPlotModel)model).Update(true);
                ((IPlotModel)model).Render(rc, rect);
                rc.DrawClippedRectangle(whole, rect.Inflate(2.0, 2.0), OxyColors.Transparent, OxyColors.Blue, 1.0, EdgeRenderingMode.Adaptive);
                rc.DrawClippedRectangle(whole, model.PlotBounds, OxyColors.Transparent, OxyColors.Black, 1.0, EdgeRenderingMode.Adaptive);
                rc.DrawClippedRectangle(whole, whole, OxyColors.Transparent, OxyColors.Black, 1.0, EdgeRenderingMode.Adaptive);
                rc.Complete();
                rc.Flush();
            }
        }
    }
}
