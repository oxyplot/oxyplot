// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimatorTests.cs" company="OxyPlot">
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
//   Provides unit tests for the <see cref="Decimator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests.Rendering.Utilities
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="Decimator" /> class.
    /// </summary>
    [TestFixture]
    public class DecimatorTests
    {
        /// <summary>
        /// Tests decimation of a list of points.
        /// </summary>
        [Test]
        public void Decimate()
        {
            // TODO: write some better tests
            var input = new List<ScreenPoint>();
            var output = new List<ScreenPoint>();
            int n = 1000;
            for (int i = 0; i < n; i++)
            {
                input.Add(new ScreenPoint(Math.Round((double)i / n), Math.Sin(i)));
            }

            Decimator.Decimate(input, output);
            Assert.AreEqual(6, output.Count);
        }
    }
}