// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CohenSutherlandClippingTests.cs" company="OxyPlot">
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
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class CohenSutherlandClippingTests
    {
        [Test]
        public void IsInside()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            Assert.IsTrue(clipping.IsInside(0.5, 0.5));
            Assert.IsFalse(clipping.IsInside(-0.5, 0.5));
        }

        [Test]
        public void ClipLine_EndpointsOutsideArea()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            var p0 = new ScreenPoint(0.3, -0.2);
            var p1 = new ScreenPoint(0.6, 1.3);
            Assert.IsTrue(clipping.ClipLine(ref p0, ref p1));
            Assert.AreEqual(0, p0.Y);
            Assert.AreEqual(1, p1.Y);
        }

        [Test]
        public void ClipLine_EndpointsInsideArea()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            var p0 = new ScreenPoint(0.3, 0.2);
            var p1 = new ScreenPoint(0.6, 0.8);
            Assert.IsTrue(clipping.ClipLine(ref p0, ref p1));
            Assert.AreEqual(0.2, p0.Y);
            Assert.AreEqual(0.8, p1.Y);
        }

        [Test]
        public void ClipLine_LineOutsideArea()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            var p0 = new ScreenPoint(0.3, -0.2);
            var p1 = new ScreenPoint(0.6, -0.2);
            Assert.IsFalse(clipping.ClipLine(ref p0, ref p1));
            Assert.AreEqual(-0.2, p0.Y);
            Assert.AreEqual(-0.2, p1.Y);
        }
    }
}