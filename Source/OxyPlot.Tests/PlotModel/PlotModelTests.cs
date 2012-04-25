// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using ExampleLibrary;

    using NUnit.Framework;

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

            var svg1 = plotModel.ToSvg(800, 500, true);
            SvgAssert.IsValidDocument(svg1);
            var svg2 = plotModel.ToSvg(800, 500);
            SvgAssert.IsValidElement(svg2);
        }

        [Test]
        public void SaveSvg_TestPlot_ValidSvgFile()
        {
            var plotModel = TestModels.CreateTestModel1();

            const string FileName = "PlotModelTests_Test1.svg";
            plotModel.SaveSvg(FileName, 800, 500);
            SvgAssert.IsValidFile(FileName);
        }

#if SERIALIZATION_ENABLED
        [Test]
        public void XmlSerialize_PlotModel_ValidXml()
        {
            // var plotModel = TestModels.CreateTestModel1();
            // plot.XmlSerialize("test.xml");
        }
#endif

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