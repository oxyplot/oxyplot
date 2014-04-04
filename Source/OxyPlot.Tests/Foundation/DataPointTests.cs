// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointTests.cs" company="OxyPlot">
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
//   Provides unit tests for the <see cref="DataPoint" /> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="DataPoint" /> type.
    /// </summary>
    [TestFixture]
    public class DataPointTests
    {
        /// <summary>
        /// Tests the <see cref="DataPoint.IsDefined" /> method.
        /// </summary>
        public class IsDefined
        {
            /// <summary>
            /// Given valid points, <c>true</c> is returned.
            /// </summary>
            [Test]
            public void ValidPoints()
            {
                Assert.IsTrue(new DataPoint(1, 2).IsDefined());
                Assert.IsTrue(new DataPoint(double.MaxValue, double.MaxValue).IsDefined());
                Assert.IsTrue(new DataPoint(double.MinValue, double.MinValue).IsDefined());
            }

            /// <summary>
            /// Given invalid points, <c>false</c> is returned.
            /// </summary>
            [Test]
            public void InvalidPoints()
            {
                Assert.IsFalse(new DataPoint(double.NaN, double.NaN).IsDefined());
                Assert.IsFalse(new DataPoint(double.NaN, 2).IsDefined());
                Assert.IsFalse(new DataPoint(2, double.NaN).IsDefined());
                var p = DataPoint.Undefined;
                Assert.IsFalse(p.IsDefined());
            }
        }
    }
}