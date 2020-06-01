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
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    using OxyPlot.Axes;

    /// <summary>
    /// Provides unit tests for the <see cref="AxisUtilities" /> class.
    /// </summary>
    [TestFixture]
    public class AxisUtilitiesTests
    {
        [Test]
        public void CreateTickValues_ThrowsForInvalidStep()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AxisUtilities.CreateTickValues(1, 10, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => AxisUtilities.CreateTickValues(1, 10, -1));
        }

        [Test]
        public void CreateTickValues_MaxTicks()
        {
            Assert.AreEqual(1000, AxisUtilities.CreateTickValues(1, 100, 0.000001).Count);
            Assert.AreEqual(100, AxisUtilities.CreateTickValues(1, 100, 0.000001, 100).Count);
            Assert.AreEqual(0, AxisUtilities.CreateTickValues(1, 100, 0.000001, 0).Count);
        }

        [Test]
        public void TestCreateTickValues()
        {
            // from, to, step, expected
            var testCases = new[]
            {
                new Tuple<double, double, double, double[]>(1, 1, .4, Array.Empty<double>()),
                new Tuple<double, double, double, double[]>(1, 1, 0.02, new [] { 1d }),
                new Tuple<double, double, double, double[]>(-0.0515724495834661, 0.016609368598352, 0.02, new[] { -0.04, -0.02, 0 }),
                new Tuple<double, double, double, double[]>(0.016609368598352, 0.0515724495834661, 0.02, new[] {  0.02, 0.04 }),
                new Tuple<double, double, double, double[]>(0, Math.PI * 2, Math.PI, new[] { 0, Math.PI, Math.PI * 2 }),
                new Tuple<double, double, double, double[]>(1.000000000001, 4.999999999999, 1, new[] { 1d, 2, 3, 4, 5 }),
            };

            foreach (var testCase in testCases)
            {
                var from = testCase.Item1;
                var to = testCase.Item2;
                var step = testCase.Item3;
                var expected = testCase.Item4;

                TestPositiveAndNegative(from, to, step, expected);
            }

            void TestPositiveAndNegative(double from, double to, double step, IList<double> expected)
            {
                TestNormalAndReversed(from, to, step, expected);
                TestNormalAndReversed(-from, -to, step, expected.Select(d => -d).ToList());
            }

            void TestNormalAndReversed(double from, double to, double step, IList<double> expected)
            {
                TestLargeAndSmall(from, to, step, expected);
                TestLargeAndSmall(to, from, step, expected.Reverse().ToList());
            }

            void TestLargeAndSmall(double from, double to, double step, IList<double> expected)
            {
                Test(from, to, step, expected);
                Test(from * 1e50, to * 1e50, step * 1e50, expected.Select(d => d * 1e50).ToList());
                Test(from * 1e-50, to * 1e-50, step * 1e-50, expected.Select(d => d * 1e-50).ToList());
            }

            void Test(double from, double to, double step, IList<double> expected)
            {
                var actual = AxisUtilities.CreateTickValues(from, to, step);
                Assert.That(actual, Is.EqualTo(expected).AsCollection.Within(step * 1e-7));
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

        [Test]
        public void TestFilterRedundantMinorTicks()
        {
            // majorTicks, minorTicks, expected
            var testCases = new double[][][]
            {
                new [] { Array.Empty<double>(), Array.Empty<double>(), Array.Empty<double>() },
                new [] { new[] { 1d }, Array.Empty<double>(), Array.Empty<double>() },
                new [] { Array.Empty<double>(), new[] { 1d }, new[] { 1d } },
                new [] { new[] { 1d }, new[] { 1d }, Array.Empty<double>() },
                new [] { new[] { 1d }, new[] { 1.000000000001 }, new[] { 1.000000000001 } },
                new [] { new[] { 1d }, new[] { 3d }, new[] { 3d } },
                new [] { new[] { 1d, 2, 3, 4, 5 }, new[] { 1d, 2, 3, 4, 5 }, Array.Empty<double>() },
                new [] { new[] { 1d, 3, 5 }, new[] { 1d, 2, 3, 4, 5, 6 }, new[] { 2d, 4, 6 } },
                new [] { new[] { 1d, 3, 5 }, new[] { 1.5 }, new[] { 1.5 } },
                new [] { new[] { 1d, 3, 5 }, new[] { 0d, 1 }, new[] { 0d } },
                new [] { new[] { 1e10, 1e10 + 4 }, new[] { 1e10, 1e10 + 1, 1e10 + 2, 1e10 + 3, 1e10 + 4 }, new[] { 1e10 + 1, 1e10 + 2, 1e10 + 3 } },
            };

            foreach (var testCase in testCases)
            {
                var majorTicks = testCase[0];
                var minorTicks = testCase[1];
                var expected = testCase[2];

                TestPositiveAndNegative(majorTicks, minorTicks, expected);
            }

            void TestPositiveAndNegative(IList<double> majorTicks, IList<double> minorTicks, IList<double> expected)
            {
                TestNormalAndReversed(majorTicks, minorTicks, expected);
                TestNormalAndReversed(majorTicks.Select(d => -d).ToList(), minorTicks.Select(d => -d).ToList(), expected.Select(d => -d).ToList());
            }

            void TestNormalAndReversed(IList<double> majorTicks, IList<double> minorTicks, IList<double> expected)
            {
                TestExactAndInexact(majorTicks, minorTicks, expected);
                TestExactAndInexact(majorTicks.Reverse().ToList(), minorTicks.Reverse().ToList(), expected.Reverse().ToList());
            }

            void TestExactAndInexact(IList<double> majorTicks, IList<double> minorTicks, IList<double> expected)
            {
                TestLargeAndSmall(majorTicks, minorTicks, expected);

                // in this case we need an exact match
                if (majorTicks.Count == 1 && minorTicks.Count == 1 && majorTicks[0] == minorTicks[0])
                {
                    return;
                }

                var rng = new Random(1);
                TestLargeAndSmall(majorTicks.Select(d => d + ((rng.NextDouble() - 0.5) * 1e-6)).ToList(), minorTicks, expected);
            }

            void TestLargeAndSmall(IList<double> majorTicks, IList<double> minorTicks, IList<double> expected)
            {
                Test(majorTicks, minorTicks, expected);
                Test(majorTicks.Select(d => d * 1e50).ToList(), minorTicks.Select(d => d * 1e50).ToList(), expected.Select(d => d * 1e50).ToList());
                Test(majorTicks.Select(d => d * 1e-50).ToList(), minorTicks.Select(d => d * 1e-50).ToList(), expected.Select(d => d * 1e-50).ToList());
            }

            void Test(IList<double> majorTicks, IList<double> minorTicks, IList<double> expected)
            {
                var actual = AxisUtilities.FilterRedundantMinorTicks(majorTicks, minorTicks);
                CollectionAssert.AreEqual(expected, actual);
            }
        }
    }
}
