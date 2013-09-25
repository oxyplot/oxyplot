// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgWriterTests.cs" company="OxyPlot">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

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
            using (var s = File.Create(FileName))
            {
                using (var w = new SvgWriter(s, 200, 200))
                {
                    w.WriteEllipse(
                        10,
                        10,
                        100,
                        60,
                        w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
                }
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WriteLine()
        {
            const string FileName = "SvgWriterTests_WriteLine.svg";
            using (var s = File.Create(FileName))
            {
                using (var w = new SvgWriter(s, 200, 200))
                {
                    w.WriteLine(
                        new ScreenPoint(10, 10),
                        new ScreenPoint(150, 80),
                        w.CreateStyle(null, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
                }
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WritePolygon()
        {
            const string FileName = "SvgWriterTests_WritePolygon.svg";
            using (var s = File.Create(FileName))
            {
                using (var w = new SvgWriter(s, 200, 200))
                {
                    w.WritePolygon(
                        CreatePointList(),
                        w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
                }
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WritePolyline()
        {
            const string FileName = "SvgWriterTests_WritePolyLine.svg";
            using (var s = File.Create(FileName))
            {
                using (var w = new SvgWriter(s, 200, 200))
                {
                    w.WritePolyline(
                        CreatePointList(),
                        w.CreateStyle(OxyColors.Blue, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
                }
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WriteRectangle()
        {
            const string FileName = "SvgWriterTests_WriteRectangle.svg";
            using (var s = File.Create(FileName))
            {
                using (var w = new SvgWriter(s, 200, 200))
                {
                    w.WriteRectangle(
                        10,
                        20,
                        150,
                        80,
                        w.CreateStyle(
                            OxyColors.Green, OxyColors.Black, 2, LineStyleHelper.GetDashArray(LineStyle.Solid)));
                }
            }

            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void WriteText()
        {
            const string FileName = "SvgWriterTests_WriteText.svg";
            using (var s = File.Create(FileName))
            {
                using (var w = new SvgWriter(s, 200, 200))
                {
                    w.WriteText(new ScreenPoint(10, 10), "Hello world!", OxyColors.Black);
                }
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