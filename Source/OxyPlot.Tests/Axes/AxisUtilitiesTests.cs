// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisUtilitiesTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="AxisUtilities" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;

    using NUnit.Framework;

    using OxyPlot.Axes;

    /// <summary>
    /// Provides unit tests for the <see cref="AxisUtilities" /> class.
    /// </summary>
    [TestFixture]
    public class AxisUtilitiesTests
    {
        /// <summary>
        /// Given normal values around zero,
        /// when tick values are created,
        /// 'nice' values are returned.
        /// </summary>
        [Test]
        public void CreateTickValuesForNormalValuesAroundZero()
        {
            var values = AxisUtilities.CreateTickValues(-0.0515724495834661, 0.016609368598352, 0.02);
            CollectionAssert.AreEqual(new[] { -0.06, -0.04, -0.02, 0 }, values);
        }

        /// <summary>
        /// Given big values around zero,
        /// when tick values are created,
        /// 'nice' values are returned.
        /// </summary>
        [Test]
        public void CreateTickValuesForBigValuesAroundZero()
        {
            var values = AxisUtilities.CreateTickValues(-0.0515724495834661e30, 0.016609368598352e30, 0.02e30);
            CollectionAssert.AreEqual(new[] { -0.06e30, -0.04e30, -0.02e30, 0 }, values);
        }

        /// <summary>
        /// Given small values around zero,
        /// when tick values are created,
        /// 'nice' values are returned.
        /// </summary>
        [Test]
        public void CreateTickValuesForSmallValuesAroundZero()
        {
            var values = AxisUtilities.CreateTickValues(-0.0515724495834661e-30, 0.016609368598352e-30, 0.02e-30);
            CollectionAssert.AreEqual(new[] { -0.06e-30, -0.04e-30, -0.02e-30, 0 }, values);
        }

        /// <summary>
        /// Given normal positive values,
        /// when tick values are created,
        /// 'nice' values are returned.
        /// </summary>
        [Test]
        public void CreateTickValuesForNormalPositiveValues()
        {
            var values = AxisUtilities.CreateTickValues(0.016609368598352, 0.0515724495834661, 0.02);
            CollectionAssert.AreEqual(new[] { 0.02, 0.04 }, values);
        }

        /// <summary>
        /// Given big positive values,
        /// when tick values are created,
        /// 'nice' values are returned.
        /// </summary>
        [Test]
        public void CreateTickValuesForBigPositiveValues()
        {
            var values = AxisUtilities.CreateTickValues(0.016609368598352e30, 0.0515724495834661e30, 0.02e30);
            CollectionAssert.AreEqual(new[] { 0.02e30, 0.04e30 }, values);
        }

        /// <summary>
        /// Given small positive values,
        /// when tick values are created,
        /// 'nice' values are returned.
        /// </summary>
        [Test]
        public void CreateTickValuesForSmallPositiveValues()
        {
            var values = AxisUtilities.CreateTickValues(0.016609368598352e-30, 0.0515724495834661e-30, 0.02e-30);
            CollectionAssert.AreEqual(new[] { 0.02e-30, 0.04e-30 }, values);
        }

        /// <summary>
        /// Given step with many digits,
        /// when tick values are created,
        /// correct values are returned.
        /// </summary>
        [Test]
        public void CreateTickValuesForStepWithManyDigits()
        {
            var values = AxisUtilities.CreateTickValues(0, Math.PI * 2, Math.PI);
            CollectionAssert.AreEqual(new[] { 0, Math.PI, Math.PI * 2 }, values);
        }

        /// <summary>
        /// Ensures tick values are not including floating point error to such an extent that it would show up when printed 
        /// </summary>
        [Test]
        public void CreateTickValuesForFloatingPointAccumulation()
        {
            var values = AxisUtilities.CreateTickValues(0.58666699999999994, 0.9233, 0.05);
            foreach (var val in values)
            {
                Assert.LessOrEqual(string.Format("{0:}", val).Length, 4);
            }
        }

        /// <summary>
        /// Calculates the minor interval given the major interval.
        /// </summary>
        /// <param name="majorInterval">The major interval.</param>
        /// <param name="expectedMinorInterval">The expected minor interval.</param>
        [Test]
        [TestCase(1e-100, .2e-100)]
        [TestCase(2e-100, .5e-100)]
        [TestCase(5e-100, 1e-100)]
        [TestCase(1, 0.2)]
        [TestCase(2, 0.5)]
        [TestCase(5, 1)]
        [TestCase(20, 5)]
        [TestCase(1e100, .2e100)]
        [TestCase(2e100, .5e100)]
        [TestCase(5e100, 1e100)]
        public void CalculateMinorInterval(double majorInterval, double expectedMinorInterval)
        {
            Assert.That(AxisUtilities.CalculateMinorInterval(majorInterval), Is.EqualTo(expectedMinorInterval).Within(expectedMinorInterval * 1e-10), "minorInterval calculation");
#if DEBUG
            Assert.That(AxisUtilities.CalculateMinorInterval2(majorInterval), Is.EqualTo(expectedMinorInterval).Within(expectedMinorInterval * 1e-10), "minorInterval calculation 2");
#endif
        }
    }
}