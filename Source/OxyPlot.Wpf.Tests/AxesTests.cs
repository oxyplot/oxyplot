// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxesTests.cs" company="OxyPlot">
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
namespace OxyPlot.Wpf.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class AxesTests
    {
        [Test]
        public void Axis()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.Axes.Axis), typeof(Axis));
        }

        [Test]
        public void LinearAxis()
        {
            var s1 = new OxyPlot.Axes.LinearAxis();
            var s2 = new LinearAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void LogarithmicAxis()
        {
            var s1 = new OxyPlot.Axes.LogarithmicAxis();
            var s2 = new LogarithmicAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void DateTimeAxis()
        {
            var s1 = new OxyPlot.Axes.DateTimeAxis();
            var s2 = new DateTimeAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void TimeSpanAxis()
        {
            var s1 = new OxyPlot.Axes.TimeSpanAxis();
            var s2 = new TimeSpanAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void CategoryAxis()
        {
            var s1 = new OxyPlot.Axes.CategoryAxis();
            var s2 = new CategoryAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void AngleAxis()
        {
            var s1 = new OxyPlot.Axes.AngleAxis();
            var s2 = new AngleAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void MagnitudeAxis()
        {
            var s1 = new OxyPlot.Axes.MagnitudeAxis();
            var s2 = new MagnitudeAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
    }
}