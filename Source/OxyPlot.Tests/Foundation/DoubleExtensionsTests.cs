// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleExtensionsTests.cs" company="OxyPlot">
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
    public class DoubleExtensionsTests
    {
        [Test]
        public void RemoveNoise()
        {
            double d1 = 3 * 0.1; // 0.30000000000000004
            double d2 = d1.RemoveNoise();
            Assert.AreNotEqual(0.3, d1);
            Assert.AreEqual(0.3, d2);
        }

        [Test]
        public void RemoveNoise2()
        {
            double d1 = 3 * 0.1; // 0.30000000000000004
            double d2 = d1.RemoveNoise2();
            Assert.AreNotEqual(0.3, d1);
            Assert.AreEqual(0.3, d2);
        }

        [Test]
        public void RemoveNoiseFromDoubleMath_1()
        {
            double d = 3 * 0.1;
            double d2 = d.RemoveNoiseFromDoubleMath();
            Assert.AreEqual(0.3, d2);
        }

        [Test]
        public void RemoveNoiseFromDoubleMath_2()
        {
            double d = -0.018 + 0.001 * 19;
            double d2 = d.RemoveNoiseFromDoubleMath();
            Assert.AreEqual(0.001, d2);
        }

        [Test, Ignore]
        public void RemoveNoiseFromDoubleMath_3()
        {
            // issue 9961
            double d = 0.000999999999999997;
            double d2 = d.RemoveNoiseFromDoubleMath();
            Assert.AreEqual(0.001, d2);
        }
    }
}