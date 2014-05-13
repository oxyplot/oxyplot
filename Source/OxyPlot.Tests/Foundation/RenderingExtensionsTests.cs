// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingExtensionsTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
// <summary>
//   Provides unit tests for the <see cref="RenderingExtensions" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using NSubstitute;

    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="RenderingExtensions" /> class.
    /// </summary>
    public class RenderingExtensionsTests
    {
        /// <summary>
        /// Tests the <see cref="RenderingExtensions.DrawClippedLine" /> extension method.
        /// </summary>
        public class DrawClippedLine
        {
            /// <summary>
            /// Given an empty array.
            /// </summary>
            [Test]
            public void EmptyArray()
            {
                var points = new ScreenPoint[0];
                var clippingRectangle = new OxyRect(0.3, -0.5, 0.5, 1);
                var rc = Substitute.For<IRenderContext>();
                var received = new List<ScreenPoint>();
                rc.DrawClippedLine(points, clippingRectangle, 1, OxyColors.Black, 1, null, OxyPenLineJoin.Miter, false, null, received.AddRange);
                Assert.AreEqual(0, received.Count);
            }

            /// <summary>
            /// Given a single point inside the clipping rectangle.
            /// </summary>
            [Test]
            public void SinglePointInside()
            {
                var points = new[] { new ScreenPoint(0.5, 0) };
                var clippingRectangle = new OxyRect(0.3, -0.5, 0.5, 1);
                var rc = Substitute.For<IRenderContext>();
                var received = new List<ScreenPoint>();
                rc.DrawClippedLine(points, clippingRectangle, 1, OxyColors.Black, 1, null, OxyPenLineJoin.Miter, false, null, received.AddRange);
                Assert.AreEqual(2, received.Count);
            }

            /// <summary>
            /// Given a single point outside the clipping rectangle.
            /// </summary>
            [Test]
            public void SinglePointOutside()
            {
                var points = new[] { new ScreenPoint(0, 0) };
                var clippingRectangle = new OxyRect(0.3, -0.5, 0.5, 1);
                var rc = Substitute.For<IRenderContext>();
                var received = new List<ScreenPoint>();
                rc.DrawClippedLine(points, clippingRectangle, 1, OxyColors.Black, 1, null, OxyPenLineJoin.Miter, false, null, received.AddRange);
                Assert.AreEqual(0, received.Count);
            }

            /// <summary>
            /// Given a line that crosses the clipping rectangle.
            /// </summary>
            [Test]
            public void CrossingLine()
            {
                var points = new[] { new ScreenPoint(0, 0), new ScreenPoint(100, 0) };
                var clippingRectangle = new OxyRect(30, -50, 50, 100);
                var rc = Substitute.For<IRenderContext>();
                var received = new List<ScreenPoint>();
                rc.DrawClippedLine(points, clippingRectangle, 1, OxyColors.Black, 1, null, OxyPenLineJoin.Miter, false, null, received.AddRange);
                Assert.AreEqual(2, received.Count);
                Assert.AreEqual(new ScreenPoint(30, 0), received[0]);
                Assert.AreEqual(new ScreenPoint(80, 0), received[1]);
            }
        }
    }
}