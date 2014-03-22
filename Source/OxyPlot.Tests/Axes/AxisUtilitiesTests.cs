// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisUtilitiesTests.cs" company="OxyPlot">
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
//   Provides unit tests for the <see cref="AxisUtilities"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;

    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="AxisUtilities"/> class.
    /// </summary>
    [TestFixture]
    public class AxisUtilitiesTests
    {
        /// <summary>
        /// Provides unit tests for the <see cref="AxisUtilities.CreateTickValues"/> method.
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
                var values = AxisUtilities.CreateTickValues(-0.0515724495834661, 0.016609368598352, 0.02);
                CollectionAssert.AreEqual(new[] { -0.06, -0.04, -0.02, 0 }, values);
            }

            /// <summary>
            /// Given big values around zero,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void BigValuesAroundZero()
            {
                var values = AxisUtilities.CreateTickValues(-0.0515724495834661e30, 0.016609368598352e30, 0.02e30);
                CollectionAssert.AreEqual(new[] { -0.06e30, -0.04e30, -0.02e30, 0 }, values);
            }

            /// <summary>
            /// Given small values around zero,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void SmallValuesAroundZero()
            {
                var values = AxisUtilities.CreateTickValues(-0.0515724495834661e-30, 0.016609368598352e-30, 0.02e-30);
                CollectionAssert.AreEqual(new[] { -0.06e-30, -0.04e-30, -0.02e-30, 0 }, values);
            }

            /// <summary>
            /// Given normal positive values,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void NormalPositiveValues()
            {
                var values = AxisUtilities.CreateTickValues(0.016609368598352, 0.0515724495834661, 0.02);
                CollectionAssert.AreEqual(new[] { 0.02, 0.04 }, values);
            }

            /// <summary>
            /// Given big positive values,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void BigPositiveValues()
            {
                var values = AxisUtilities.CreateTickValues(0.016609368598352e30, 0.0515724495834661e30, 0.02e30);
                CollectionAssert.AreEqual(new[] { 0.02e30, 0.04e30 }, values);
            }

            /// <summary>
            /// Given small positive values,
            /// 'nice' values are returned.
            /// </summary>
            [Test]
            public void SmallPositiveValues()
            {
                var values = AxisUtilities.CreateTickValues(0.016609368598352e-30, 0.0515724495834661e-30, 0.02e-30);
                CollectionAssert.AreEqual(new[] { 0.02e-30, 0.04e-30 }, values);
            }

            /// <summary>
            /// Given step with many digits,
            /// correct values are returned.
            /// </summary>
            [Test]
            public void StepWithManyDigits()
            {
                var values = AxisUtilities.CreateTickValues(0, Math.PI * 2, Math.PI);
                CollectionAssert.AreEqual(new[] { 0, Math.PI, Math.PI * 2 }, values);
            }
        }
    }
}