// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XYAxisSeriesTests.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="XYAxisSeries" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;

    using NUnit.Framework;
    
    using OxyPlot.Series;

    /// <summary>
    /// Provides unit tests for the <see cref="XYAxisSeries" /> class.
    /// </summary>
    public class XYAxisSeriesTests
    {
        /// <summary>
        /// Test class just to allow methods in XYAxisSeries to be tested.
        /// </summary>
        private class TestAxisSeries : XYAxisSeries
        {
            public override void Render(IRenderContext rc)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Generate lots of random data, then checking the FindWindowStartIndex method
        /// against this data.
        /// </summary>
        [Test]
        public void RunFuzzTest()
        {
            for (int i = 0; i < 10000; ++i)
            {
                FuzzIteration(i);
            }
        }

        /// <summary>
        /// Generate lots of random data, then checking the FindWindowStartIndex method
        /// against this data. This time with data containing NaN-values.
        /// </summary>
        [Test]
        public void RunFuzzWithNanTest()
        {
            for (int i = 0; i < 10000; ++i)
            {
                FuzzIterationWithNan(i);
            }
        }

        private static void FuzzIteration(int seed)
        {
            var xyAxiseries = new TestAxisSeries();

            var N = 15;
            var random = new Random(seed);
            var testData = new System.Collections.Generic.List<double>();
            var currentValue = random.NextDouble() - 0.5;
            for (int i = 0; i < N; ++i)
            {
                testData.Add(currentValue);
                currentValue += random.NextDouble();
            }

            var targetX = random.NextDouble() * (N / 2.0) - 1.0;
            var guess = random.Next(0, testData.Count);

            int foundIndex = xyAxiseries.FindWindowStartIndex(testData, x => x, targetX, guess);

            if (foundIndex > 0)
                Assert.LessOrEqual(testData[foundIndex], targetX, "At " + seed);
            if (foundIndex < testData.Count - 1)
                Assert.GreaterOrEqual(testData[foundIndex + 1], targetX, "At " + seed);
        }

        private static void FuzzIterationWithNan(int seed)
        {
            var xyAxiseries = new TestAxisSeries();

            var N = 15;
            var random = new Random(seed);
            var testData = new System.Collections.Generic.List<double>();
            var currentValue = random.NextDouble() - 0.5;
            for (int i = 0; i < N; ++i)
            {
                if (random.Next(4) == 0)
                    testData.Add(double.NaN);
                testData.Add(currentValue);
                currentValue += random.NextDouble();
            }

            double? PrevNonNan(int index)
            {
                while (index >= 0)
                {
                    if (double.IsNaN(testData[index]) == false)
                        return testData[index];
                    index -= 1;
                }

                return null;
            }

            double? NextNonNan(int index)
            {
                while (index >= 0)
                {
                    if (double.IsNaN(testData[index]) == false)
                        return testData[index];
                    index += 1;
                }

                return null;
            }

            var targetX = random.NextDouble() * (N / 2.0) - 1.0;
            var guess = random.Next(0, testData.Count);

            int foundIndex = xyAxiseries.FindWindowStartIndex(testData, x => x, targetX, guess);

            if (foundIndex > 0)
            {
                if (double.IsNaN(testData[foundIndex]))
                {
                    var prevNonNaN = PrevNonNan(foundIndex-1);
                    if (prevNonNaN.HasValue)
                        Assert.LessOrEqual(prevNonNaN, targetX, "At " + seed);
                }
                else
                {
                    Assert.LessOrEqual(testData[foundIndex], targetX, "At " + seed);
                }
            }

            if (foundIndex < testData.Count - 1)
            {
                if (double.IsNaN(testData[foundIndex + 1]))
                {
                    var nextNonNaN = NextNonNan(foundIndex+1);
                    if (nextNonNaN.HasValue)
                        Assert.GreaterOrEqual(nextNonNaN, targetX, "At " + seed);
                }
                else
                {
                    Assert.GreaterOrEqual(testData[foundIndex + 1], targetX, "At " + seed);
                }
            }
        }
    }
}
