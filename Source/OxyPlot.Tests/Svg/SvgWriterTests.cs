// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgWriterTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class SvgWriterTests
    {
        [Test]
        public void WriteEllipse()
        {
            const string FileName = "SvgWriterTests_WriteEllipse.svg";
            using (var w = new SvgWriter(FileName, 200, 200))
            {
                w.WriteEllipse(
                    10,
                    10,
                    100,
                    60,
                    w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WriteLine()
        {
            const string FileName = "SvgWriterTests_WriteLine.svg";
            using (var w = new SvgWriter(FileName, 200, 200))
            {
                w.WriteLine(
                    new ScreenPoint(10, 10),
                    new ScreenPoint(150, 80),
                    w.CreateStyle(null, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WritePolygon()
        {
            const string FileName = "SvgWriterTests_WritePolygon.svg";
            using (var w = new SvgWriter(FileName, 200, 200))
            {
                w.WritePolygon(CreatePointList(), w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WritePolyline()
        {
            const string FileName = "SvgWriterTests_WritePolyLine.svg";
            using (var w = new SvgWriter(FileName, 200, 200))
            {
                w.WritePolyline(CreatePointList(), w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WriteRectangle()
        {
            const string FileName = "SvgWriterTests_WriteRectangle.svg";
            using (var w = new SvgWriter(FileName, 200, 200))
            {
                w.WriteRectangle(10, 20, 150, 80, w.CreateStyle(OxyColors.Green, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WriteText()
        {
            const string FileName = "SvgWriterTests_WriteText.svg";
            using (var w = new SvgWriter(FileName, 200, 200))
            {
                w.WriteText(new ScreenPoint(10, 10), "Hello world!", OxyColors.Black);
            }

            SvgAssert.IsValidFile(FileName);
        }

        private static IEnumerable<ScreenPoint> CreatePointList()
        {
            return new List<ScreenPoint>
                {
                    new ScreenPoint(10, 20),
                    new ScreenPoint(80, 30),
                    new ScreenPoint(140, 120),
                    new ScreenPoint(30, 140)
                };
        }
    }
}