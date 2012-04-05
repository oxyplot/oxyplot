// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using ExampleLibrary;
using NUnit.Framework;

namespace OxyPlot.Tests
{
    using System.IO;
    using System.Xml.Serialization;

    [TestFixture]
    public class PlotModelTests
    {
        [Test]
        public void SaveSvg_AllExamplesInExampleLibrary_CheckThatFileExists()
        {
            var destinationDirectory = "svg";
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            foreach (var example in Examples.GetList())
            {
                var path = Path.Combine(destinationDirectory, StringHelper.CreateValidFileName(example.Category + " - " + example.Title, ".svg"));
                example.PlotModel.SaveSvg(path, 800, 500);
                Assert.IsTrue(File.Exists(path));
            }
        }

        [Test]
        public void SaveSvg_SimplePlot_ValidSvgFile()
        {
            var path = "test1.svg";
            var plot = new PlotModel("f(x)=sin(x)");
            var ls = new FunctionSeries(Math.Sin, 0, 30, 300);
            plot.Series.Add(ls);
            plot.SaveSvg(path, 800, 400);
            // Assert.IsTrue(SvgValidator.IsValid(path));
        }

        [Test]
        public void XmlSerialize_PlotModel()
        {
            var plot = new PlotModel("test1");
            var ls = new LineSeries();
            for (double i = 0; i < 30; i += 0.1)
            {
                ls.Points.Add(new DataPoint(i, Math.Sin(i) * 20));
            }

            plot.Update();
            // plot.XmlSerialize("test.xml");
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

            Action<LineSeries> AddExamplePoints = ls =>
                                                      {
                                                          ls.Points.Add(new DataPoint(3, 13));
                                                          ls.Points.Add(new DataPoint(10, 47));
                                                          ls.Points.Add(new DataPoint(30, 23));
                                                          ls.Points.Add(new DataPoint(40, 65));
                                                          ls.Points.Add(new DataPoint(80, 10));
                                                      };

            var ls1 = new LineSeries { Background = OxyColors.LightSeaGreen, YAxisKey = "Y1" };
            AddExamplePoints(ls1);
            plot.Series.Add(ls1);

            var ls2 = new LineSeries { Background = OxyColors.LightSkyBlue, YAxisKey = "Y2" };
            AddExamplePoints(ls2);
            plot.Series.Add(ls2);

            //  OxyAssert.AreEqual(plot, "B11");
        }        
    }
}