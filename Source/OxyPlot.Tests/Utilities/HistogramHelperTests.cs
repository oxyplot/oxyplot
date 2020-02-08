// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramHelperTests.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class HistogramHelperTests
    {
        [Test]
        public void CreateUniformBins_InvalidParameters()
        {
            // explicitly disallow extreme values for start and end
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(double.NaN, 0.0, 5));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(double.NegativeInfinity, 0.0, 5));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(double.PositiveInfinity, 0.0, 5));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(0.0, double.NaN, 5));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(0.0, double.NegativeInfinity, 5));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(0.0, double.PositiveInfinity, 5));

            // disallow binCount < 1
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(0.0, 1.0, -100));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(0.0, 1.0, 0));

            // disallow start >= end
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(1.0, 0.0, 5));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.CreateUniformBins(1.0, 1.0, 5));
        }

        [Test]
        public void CreateUniformBins_SingleBin()
        {
            var start = double.Epsilon;
            var end = 1.0;

            var breaks = HistogramHelpers.CreateUniformBins(start, end, 1);

            Assert.AreEqual(2, breaks.Count);
            Assert.AreEqual(breaks[0], start);
            Assert.AreEqual(breaks[1], end);
        }

        [Test]
        public void CreateUniformBins_IntegerBins()
        {
            int[] primes = new int[] { 1, 2, 3, 5, 7, 11, 13, 17 };

            // ensure we produce nice integer results over a reasonable range
            int end = primes.Aggregate((a, b) => a * b);
            int start = -end;

            foreach (var interval in primes)
            {
                int count = (end - start) / interval;

                var breaks = HistogramHelpers.CreateUniformBins(start, end, count);

                Assert.AreEqual(count, breaks.Count - 1);

                int i = 0;
                for (int b = start; b <= end; b += interval, i++)
                {
                    Assert.AreEqual(b, breaks[i]);
                }
            }
        }

        [Test]
        public void CreateUniformBins_BasicTest()
        {
            // arbitrary unpleasent numbers
            var start = -0.123456789E10;
            var end = 0.987654321E10;
            var counts = new[] { 13, 17, 113, 1023, 1234567, 1234569 };

            foreach (var count in counts)
            {
                var breaks = HistogramHelpers.CreateUniformBins(start, end, count);

                // ensure that CreateUniformBins gives back the exact same start and end
                Assert.AreEqual(count + 1, breaks.Count);
                Assert.AreEqual(breaks[0], start);
                Assert.AreEqual(breaks[count], end);

                // ensure the gab between breaks is reasonably consistent
                double expectedGap = (end - start) / count;

                for (int i = 0; i < count; i++)
                {
                    double gap = breaks[i + 1] - breaks[i];
                    Assert.AreEqual(expectedGap, gap, 1E-5); // near limits of resoltuion
                }
            }
        }

        [Test]
        public void Collect_InvalidParameters()
        {
            // valid test data
            var testSamples = new double[] { 1, 2, 3 };
            var testBreaks = new double[] { 1, 2, 3 };

            // choice of test options
            var testOptions = new BinningOptions(BinningOutlierMode.RejectOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);

            // disallow null parameters
            Assert.Throws(typeof(ArgumentNullException), () => HistogramHelpers.Collect(null, testBreaks, testOptions));
            Assert.Throws(typeof(ArgumentNullException), () => HistogramHelpers.Collect(testSamples, null, testOptions));
            Assert.Throws(typeof(ArgumentNullException), () => HistogramHelpers.Collect(testSamples, testBreaks, null));

            // disallow fewer than 2 distinct bin breaks
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(testSamples, new double[] { }, testOptions));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(testSamples, new double[] { 1.0 }, testOptions));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(testSamples, new double[] { 1.0, 1.0 }, testOptions));

            // disallow NaN and infinite samples
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(new double[] { 1.0, double.NaN, 2.0 }, testBreaks, testOptions));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(new double[] { 1.0, double.PositiveInfinity, 2.0 }, testBreaks, testOptions));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(new double[] { 1.0, double.NegativeInfinity, 2.0 }, testBreaks, testOptions));

            // disallow NaN and infinite bin breaks
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(testSamples, new double[] { 1.0, double.NaN, 2.0 }, testOptions));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(testSamples, new double[] { 1.0, double.PositiveInfinity, 2.0 }, testOptions));
            Assert.Throws(typeof(ArgumentException), () => HistogramHelpers.Collect(testSamples, new double[] { 1.0, double.NegativeInfinity, 2.0 }, testOptions));

            // BinningOutlierMode.RejectOutliers disallows outliers
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => HistogramHelpers.Collect(new double[] { 0.0 }, testBreaks, testOptions));
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => HistogramHelpers.Collect(new double[] { 3.0 }, testBreaks, testOptions));
        }

        // Samples and BinBreaks that expose edge cases
        private static readonly double[] EdgeCaseSamples = new double[] { -1, 0 - 1E-10, 0, 1E-10, 2, 2, 2.5, 3, 4 - 1E-10, 4, 4 + 1E-10, 5 };
        private static readonly double[] EdgeCaseBreaks = new double[] { 0, 1, 2, 3, 4 };

        [Test]
        public void Collect_LowerBoundExclusive()
        {
            var expectedCounts = new int[] { 2, 0, 3, 2 };

            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues));
            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.IgnoreOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues));
        }

        [Test]
        public void Collect_LowerBoundInclusive()
        {
            var expectedCounts = new int[] { 2, 0, 3, 3 };

            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.IncludeExtremeValues));
            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.IgnoreOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.IncludeExtremeValues));
        }

        [Test]
        public void Collect_UpperBoundExclusive()
        {
            var expectedCounts = new int[] { 1, 2, 2, 2 };

            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveUpperBound, BinningExtremeValueMode.ExcludeExtremeValues));
            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.IgnoreOutliers, BinningIntervalType.InclusiveUpperBound, BinningExtremeValueMode.ExcludeExtremeValues));
        }

        [Test]
        public void Collect_UpperBoundInclusive()
        {
            var expectedCounts = new int[] { 2, 2, 2, 2 };

            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveUpperBound, BinningExtremeValueMode.IncludeExtremeValues));
            TestCollect(EdgeCaseSamples, EdgeCaseBreaks, expectedCounts, new BinningOptions(BinningOutlierMode.IgnoreOutliers, BinningIntervalType.InclusiveUpperBound, BinningExtremeValueMode.IncludeExtremeValues));
        }

        [Test]
        public void Collect_EmptySample()
        {
            // when Collect is unable to bin any items, it should return areas of 0 rather than NaN

            var emptySample = new double[] { };
            var breaks = new double[] { 0.0, 1.0, 2.0 };
            var expectedCounts = new int[] { 0, 0 };
            var expectedAreas = new double[] { 0.0, 0.0 };

            foreach (var binningMode in new[] { BinningOutlierMode.CountOutliers, BinningOutlierMode.IgnoreOutliers })
            {
                var binningOptions = new BinningOptions(binningMode, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);
                TestCollect(emptySample, breaks, expectedCounts, expectedAreas, binningOptions);
            }
        }

        [Test]
        public void Collect_OutOfOrderBinBreaks()
        {
            // binBreaks should be ordered by Collect
            var sample = new double[] { 2, 3, 9, 8, 7 };
            var outOfOrderBreaks = new double[] { 0.0, 10.0, 5.0, 15.0 };
            var expectedCounts = new int[] { 2, 3, 0 };
            var expectedAreas = new double[] { 0.4, 0.6, 0.0 };

            foreach (var binningMode in new[] { BinningOutlierMode.CountOutliers, BinningOutlierMode.IgnoreOutliers })
            {
                var binningOptions = new BinningOptions(binningMode, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);
                TestCollect(sample, outOfOrderBreaks, expectedCounts, expectedAreas, binningOptions);
            }
        }

        [Test]
        public void Collect_OutlierSamples()
        {
            // when Collect is unable to bin any items, it should return areas of 0 rather than NaN

            var outlierSamples = new double[] { -1.0, 2.0, 3.0 };
            var breaks = new double[] { 0.0, 1.0, 2.0 };
            var expectedCounts = new int[] { 0, 0 };
            var expectedAreas = new double[] { 0.0, 0.0 };

            foreach (var binningMode in new[] { BinningOutlierMode.CountOutliers, BinningOutlierMode.IgnoreOutliers })
            {
                var binningOptions = new BinningOptions(binningMode, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);
                TestCollect(outlierSamples, breaks, expectedCounts, expectedAreas, binningOptions);
            }
        }

        /// <summary>
        /// Performs a basic test on <see cref="HistogramHelpers.Collect(System.Collections.Generic.IEnumerable{double}, System.Collections.Generic.IReadOnlyList{double}, BinningOptions)"/>.
        /// </summary>
        /// <param name="samples">The samples to collect.</param>
        /// <param name="breaks">The bin breaks that define the bins.</param>
        /// <param name="expectedCounts">The expected counts.</param>
        /// <param name="binningOptions">The binning options to use.</param>
        private static void TestCollect(double[] samples, double[] breaks, int[] expectedCounts, BinningOptions binningOptions)
        {
            // compute areas from counts
            int expectedCount = binningOptions.OutlierMode == BinningOutlierMode.CountOutliers ? samples.Length : expectedCounts.Sum();

            Assume.That(expectedCount > 0);

            var expectedAreas = expectedCounts.Select(c => (double)c / expectedCount).ToArray();

            TestCollect(samples, breaks, expectedCounts, expectedAreas, binningOptions);
        }

        /// <summary>
        /// Performs a basic test on <see cref="HistogramHelpers.Collect(System.Collections.Generic.IEnumerable{double}, System.Collections.Generic.IReadOnlyList{double}, BinningOptions)"/>.
        /// </summary>
        /// <param name="samples">The samples to collect.</param>
        /// <param name="breaks">The bin breaks that define the bins.</param>
        /// <param name="expectedCounts">The expected counts.</param>
        /// <param name="expectedAreas">The expected counts.</param>
        /// <param name="binningOptions">The binning options to use.</param>
        private static void TestCollect(double[] samples, double[] breaks, int[] expectedCounts, double[] expectedAreas, BinningOptions binningOptions)
        {
            Assume.That(breaks.Length - 1 == expectedCounts.Length);
            Assume.That(breaks.Length - 1 == expectedAreas.Length);

            var items = HistogramHelpers.Collect(samples, breaks, binningOptions).ToArray();

            // check number of items
            Assert.AreEqual(expectedAreas.Length, items.Length);

            // check areas and counts
            for (int i = 0; i < expectedAreas.Length; i++)
            {
                Assert.AreEqual(expectedAreas[i], items[i].Area, expectedAreas[i] * 1E-15);
                Assert.AreEqual(expectedCounts[i], items[i].Count);
            }

            // check item ranges
            var orderedBreaks = breaks.Distinct().OrderBy(b => b).ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                Assert.AreEqual(orderedBreaks[i], items[i].RangeStart);
                Assert.AreEqual(orderedBreaks[i + 1], items[i].RangeEnd);
            }
        }
    }
}
