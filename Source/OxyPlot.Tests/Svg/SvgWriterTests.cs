// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleExtensionsTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class SvgWriterTests
    {
        [Test]
        public void WriteEllipse()
        {
            var path = "ellipse.svg";
            using (var w = new SvgWriter(path, 200, 200))
            {
                w.WriteEllipse(
                    10,
                    10,
                    100,
                    60,
                    w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }
            // Assert.IsTrue(IsValid(path));
        }

        [Test]
        public void WriteLine()
        {
            using (var w = new SvgWriter("line.svg", 200, 200))
            {
                w.WriteLine(new ScreenPoint(10, 10), new ScreenPoint(150, 80),
                    w.CreateStyle(null, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }
        }

        [Test]
        public void WritePolygon()
        {
            using (var w = new SvgWriter("polygon.svg", 200, 200))
            {
                w.WritePolygon(CreatePointList(),
                    w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }
        }

        [Test]
        public void WritePolyline()
        {
            using (var w = new SvgWriter("polyline.svg", 200, 200))
            {
                w.WritePolyline(CreatePointList(), w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }
        }

        [Test]
        public void WriteRectangle()
        {
            using (var w = new SvgWriter("rectangle.svg", 200, 200))
            {
                w.WriteRectangle(10, 20, 150, 80, w.CreateStyle(OxyColors.Green, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }
        }

        [Test]
        public void WriteText()
        {
            using (var w = new SvgWriter("text.svg", 200, 200))
            {
                w.WriteText(new ScreenPoint(10, 10), "Hello world!", OxyColors.Black);
            }
        }

        private static IList<ScreenPoint> CreatePointList()
        {
            var points = new List<ScreenPoint>();
            points.Add(new ScreenPoint(10, 20));
            points.Add(new ScreenPoint(80, 30));
            points.Add(new ScreenPoint(140, 120));
            points.Add(new ScreenPoint(30, 140));
            return points;
        }

    }
}