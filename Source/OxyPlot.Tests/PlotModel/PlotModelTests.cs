// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelTests.cs" company="OxyPlot">
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
namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using ExampleLibrary;

    using NUnit.Framework;

    using OxyPlot.Axes;
    using OxyPlot.Wpf;

    using LineSeries = OxyPlot.Series.LineSeries;
    using LinearAxis = OxyPlot.Axes.LinearAxis;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PlotModelTests
    {
        [Test]
        public void Update_AllExamples_ThrowsNoExceptions()
        {
            foreach (var example in Examples.GetList())
            {
                example.PlotModel.Update();
            }
        }

        [Test]
        public void ToSvg_TestPlot_ValidSvgString()
        {
            var plotModel = TestModels.CreateTestModel1();

            var rc = new ShapesRenderContext(null);
            var svg1 = plotModel.ToSvg(800, 500, true, rc);
            SvgAssert.IsValidDocument(svg1);
            var svg2 = plotModel.ToSvg(800, 500, false, rc);
            SvgAssert.IsValidElement(svg2);
        }

        [Test]
        public void SaveSvg_TestPlot_ValidSvgFile()
        {
            var plotModel = TestModels.CreateTestModel1();

            const string FileName = "PlotModelTests_Test1.svg";
            var rc = new ShapesRenderContext(null);
            var svg = plotModel.ToSvg(800, 500, false, rc);
            File.WriteAllText(FileName, svg);
            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void B11_Backgrounds()
        {
            var plot = new PlotModel("Backgrounds");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            var yaxis1 = new LinearAxis(AxisPosition.Left, "Y1") { Key = "Y1", StartPosition = 0, EndPosition = 0.5 };
            var yaxis2 = new LinearAxis(AxisPosition.Left, "Y2") { Key = "Y2", StartPosition = 0.5, EndPosition = 1 };
            plot.Axes.Add(yaxis1);
            plot.Axes.Add(yaxis2);

            Action<LineSeries> addExamplePoints = ls =>
                {
                    ls.Points.Add(new DataPoint(3, 13));
                    ls.Points.Add(new DataPoint(10, 47));
                    ls.Points.Add(new DataPoint(30, 23));
                    ls.Points.Add(new DataPoint(40, 65));
                    ls.Points.Add(new DataPoint(80, 10));
                };

            var ls1 = new LineSeries { Background = OxyColors.LightSeaGreen, YAxisKey = "Y1" };
            addExamplePoints(ls1);
            plot.Series.Add(ls1);

            var ls2 = new LineSeries { Background = OxyColors.LightSkyBlue, YAxisKey = "Y2" };
            addExamplePoints(ls2);
            plot.Series.Add(ls2);

            // OxyAssert.AreEqual(plot, "B11");
        }
    }
}