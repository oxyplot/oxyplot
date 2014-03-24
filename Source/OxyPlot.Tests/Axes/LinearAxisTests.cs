// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxisTests.cs" company="OxyPlot">
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
//   Provides unit tests for the <see cref="LinearAxis"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    using OxyPlot.Axes;

    /// <summary>
    /// Provides unit tests for the <see cref="LinearAxis" /> class.
    /// </summary>
    [TestFixture]
    public class LinearAxisTests
    {
        /// <summary>
        /// Tests the <see cref="LinearAxis.FormatAsFractions" /> property.
        /// </summary>
        public class FormatAsFractions
        {
            /// <summary>
            /// Given 0, the FormatValue method should return 0.
            /// </summary>
            [Test]
            public void FormatAsFractionsZero()
            {
                var axis = new LinearAxis
                {
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π"
                };
                Assert.AreEqual("0", axis.FormatValue(0));
            }

            /// <summary>
            /// Given PI/2, the FormatValue method should return π/2.
            /// </summary>
            [Test]
            public void FormatAsFractionsPiHalf()
            {
                var axis = new LinearAxis
                {
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π"
                };
                Assert.AreEqual("π/2", axis.FormatValue(0.5 * Math.PI));
            }

            /// <summary>
            /// Given 2*PI, the FormatValue method should return 2π.
            /// </summary>
            [Test]
            public void FormatAsFractionsTwoPi()
            {
                var axis = new LinearAxis
                {
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π"
                };
                Assert.AreEqual("2π", axis.FormatValue(2 * Math.PI));
            }

            /// <summary>
            /// Given 3/2*PI, the FormatValue method should return 3π/2.
            /// </summary>
            [Test]
            public void FormatAsFractionsThreeHalfPi()
            {
                var axis = new LinearAxis
                {
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π"
                };
                Assert.AreEqual("3π/2", axis.FormatValue(3d / 2 * Math.PI));
            }

            /// <summary>
            /// Given 4 and a format string, the FormatValue method should return 1.273π.
            /// </summary>
            [Test]
            public void FormatAsFractionsWithStringFormat()
            {
                var model = new PlotModel { Culture = CultureInfo.InvariantCulture };
                var axis = new LinearAxis
                {
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π",
                    StringFormat = "0.###"
                };
                model.Axes.Add(axis);
                model.Update();
                Assert.AreEqual("1.273π", axis.FormatValue(4));
            }
        }
    }
}