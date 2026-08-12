// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxisTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="LinearAxis" /> class.
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
        /// Tests the <see cref="LinearAxis.GetHashCode" /> method.
        /// </summary>
        public new class GetHashCode
        {
            /// <summary>
            /// Given two axes with identical content, verify that the hash codes are different.
            /// </summary>
            [Test]
            public void TwoEqualAxes()
            {
                var axis1 = new LinearAxis();
                var axis2 = new LinearAxis();
                Assert.That(axis1.GetHashCode() != axis2.GetHashCode(), Is.True);
            }
        }

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
                Assert.That(axis.FormatValue(0), Is.EqualTo("0"));
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
                Assert.That(axis.FormatValue(0.5 * Math.PI), Is.EqualTo("π/2"));
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
                Assert.That(axis.FormatValue(2 * Math.PI), Is.EqualTo("2π"));
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
                Assert.That(axis.FormatValue(3d / 2 * Math.PI), Is.EqualTo("3π/2"));
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
                ((IPlotModel)model).Update(true);
                Assert.That(axis.FormatValue(4), Is.EqualTo("1.273π"));
            }
        }
    }
}