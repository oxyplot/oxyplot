// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayHelperTests.cs" company="OxyPlot">
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
    public class ArrayHelperTests
    {
        [Test]
        public void CreateVector_ByDelta_ReturnsExpectedArray()
        {
            var v = ArrayHelper.CreateVector(0, 1, 0.1);
            Assert.AreEqual(11, v.Length);
            Assert.AreEqual(0, v[0]);
            Assert.AreEqual(0.3, v[3]);
            Assert.AreEqual(0.6, v[6]);
            Assert.AreEqual(0.7, v[7]);
            Assert.AreEqual(1, v[10]);
        }

        [Test]
        public void CreateVector_ByNumberOfSteps_ReturnsExpectedArray()
        {
            var v = ArrayHelper.CreateVector(0, 1, 11);
            Assert.AreEqual(11, v.Length);
            Assert.AreEqual(0, v[0]);
            Assert.AreEqual(0.3, v[3]);
            Assert.AreEqual(0.6, v[6]);
            Assert.AreEqual(0.7, v[7]);
            Assert.AreEqual(1, v[10]);
        }

        [Test]
        public void Evaluate()
        {
            var xvector = ArrayHelper.CreateVector(0, 1, 0.1);
            var yvector = ArrayHelper.CreateVector(0, 1, 0.1);
            var dvector = ArrayHelper.Evaluate((x, y) => x * y, xvector, yvector);

            Assert.AreEqual(10, dvector.GetUpperBound(0));
            Assert.AreEqual(10, dvector.GetUpperBound(1));
            Assert.AreEqual(0, dvector[0, 0]);
            Assert.AreEqual(1, dvector[10, 10]);
            Assert.AreEqual(0.3 * 0.4, dvector[3, 4]);
        }
    }
}