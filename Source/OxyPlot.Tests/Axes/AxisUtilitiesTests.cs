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
        /// Provides unit tests for the <see cref="AxisUtilities.CreateTickValues" /> method.
        /// </summary>
        public class CreateTickValues
        {
            /// <summary>
            /// Given normal values around zero,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void NormalValuesAroundZero()
            {
                var values = Axis.CreateTickValues(-0.0515724495834661, 0.016609368598352, 0.02);
                CollectionAssert.AreEqual(new[] { -0.06, -0.04, -0.02, 0 }, values);
            }

            /// <summary>
            /// Given big values around zero,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void BigValuesAroundZero()
            {
                var values = Axis.CreateTickValues(-0.0515724495834661e30, 0.016609368598352e30, 0.02e30);
                CollectionAssert.AreEqual(new[] { -0.06e30, -0.04e30, -0.02e30, 0 }, values);
            }

            /// <summary>
            /// Given small values around zero,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void SmallValuesAroundZero()
            {
                var values = Axis.CreateTickValues(-0.0515724495834661e-30, 0.016609368598352e-30, 0.02e-30);
                CollectionAssert.AreEqual(new[] { -0.06e-30, -0.04e-30, -0.02e-30, 0 }, values);
            }

            /// <summary>
            /// Given normal positive values,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void NormalPositiveValues()
            {
                var values = Axis.CreateTickValues(0.016609368598352, 0.0515724495834661, 0.02);
                CollectionAssert.AreEqual(new[] { 0.02, 0.04 }, values);
            }

            /// <summary>
            /// Given big positive values,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void BigPositiveValues()
            {
                var values = Axis.CreateTickValues(0.016609368598352e30, 0.0515724495834661e30, 0.02e30);
                CollectionAssert.AreEqual(new[] { 0.02e30, 0.04e30 }, values);
            }

            /// <summary>
            /// Given small positive values,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void SmallPositiveValues()
            {
                var values = Axis.CreateTickValues(0.016609368598352e-30, 0.0515724495834661e-30, 0.02e-30);
                CollectionAssert.AreEqual(new[] { 0.02e-30, 0.04e-30 }, values);
            }

            /// <summary>
            /// Given step with many digits,
            /// correct values are returned.
            /// </summary>
            [Test]
            public void StepWithManyDigits()
            {
                var values = Axis.CreateTickValues(0, Math.PI * 2, Math.PI);
                CollectionAssert.AreEqual(new[] { 0, Math.PI, Math.PI * 2 }, values);
            }

            /// <summary>
            /// Ensures tick values are not including floating point error to such an extent that it would show up when printed 
            /// </summary>
            [Test]
            public void FloatingPointAccumulation()
            {
                var values = Axis.CreateTickValues(0.58666699999999994, 0.9233, 0.05);
                foreach (var val in values)
                {
                    Assert.LessOrEqual(string.Format("{0:}", val).Length, 4);
                }
            }
        }
    }
}