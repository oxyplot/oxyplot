// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides useful extension methods for enumerable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Provides useful extension methods for enumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            var list = source as IList<TSource>;
            return list != null ? CreateReverseIterator(list) : Enumerable.Reverse(source);
        }

        /// <summary>
        /// Creates a reverse iterator for a list.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A list of values to reverse</param>
        /// <returns>A sequence whose elements correspond to those of the input list in reverse order.</returns>
        private static IEnumerable<TSource> CreateReverseIterator<TSource>(IList<TSource> source)
        {
            for (var i = source.Count - 1; i >= 0; --i)
            {
                yield return source[i];
            }
        }

        /// <summary>
        /// Calculates the percentile for the list of values
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <param name="percentile">The percentile to be calculated.</param>
        /// <returns>The percentile of the list</returns>
        public static double PercentilePos(this IEnumerable<double> values, double percentile)
        {
            double percentilePosVal;
            double percentileVal;
            if (values == null || !values.Any())
            {
                return double.NaN;
            }
            if (percentile > 1 && percentile <= 100)
            {
                percentileVal = percentile / 100;
            }
            else if (percentile <= 1 && percentile > 0)
            {
                percentileVal = percentile;
            }
            else
            {
                throw new ArgumentException("percentile value is invalid");
            }

            int numberCount = values.Count();
            int index = (int)(values.Count() * percentileVal);
            var sortedNumbers = values.OrderBy(n => n);

            if ((sortedNumbers.Count() - 1) < index)
            {
                index = sortedNumbers.Count() - 1;
            }
            try
            {
                if ((numberCount % 2 == 0))
                {
                    if (index > 0)
                    {
                        percentilePosVal = (sortedNumbers.ElementAt(index) + sortedNumbers.ElementAt(index - 1)) / 2;
                    }
                    else
                    {
                        percentilePosVal = sortedNumbers.ElementAt(index);
                    }
                }
                else
                {
                    percentilePosVal = sortedNumbers.ElementAt(index);
                }
            }
            catch (Exception err)
            {
                return double.NaN;
            }
            return percentilePosVal;
        }

        /// <summary>
        /// Calculates the variance of the list of values.
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <param name="mean">The mean of the values</param>
        /// <param name="start">From where to start</param>
        /// <param name="end">Where to end</param>
        /// <returns>The variance of the list</returns>
        public static double Variance(this IEnumerable<double> values, double mean, int start, int end)
        {
            double variance1 = 0;

            int i = start;
            while (i < end)
            {
                variance1 += Math.Pow((values.ElementAt(i) - mean), 2);
                Math.Max(Interlocked.Increment(ref i), i - 1);
            }

            int n = end - start;
            if (start > 0)
            {
                n -= 1;
            }

            return variance1 / (n);
        }

        /// <summary>
        /// Calculates the variance of the list of values.
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <returns>The variance of the list</returns>
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            return !values.Any() ? 0 : values.StandardDeviation(0, values.Count());
        }

        /// <summary>
        /// Calculates the standard deviation of the list of values.
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <param name="start">From where to start</param>
        /// <param name="end">Where to end</param>
        /// <returns>The standard deviation of the list</returns>
        public static double StandardDeviation(this IEnumerable<double> values, int start, int end)
        {
            double mean = values.Mean(start, end);
            double variance = values.Variance(mean, start, end);

            return Math.Sqrt(variance);
        }

        /// <summary>
        /// Calculates the median of the list of values
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <returns>The median of the list</returns>
        public static double Median(this IEnumerable<double> values)
        {
            return values.PercentilePos(50);

        }

        /// <summary>
        /// Calculates the first quartile of the list of values
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <returns>The first quartile of the list</returns>
        public static double FirstQuartile(this IEnumerable<double> values)
        {
            return values.PercentilePos(25);
        }

        /// <summary>
        /// Calculates the third quartile of the list of values
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <returns>The third quartile of the list</returns>
        public static double ThirdQuartile(this IEnumerable<double> values)
        {

            return values.PercentilePos(75);

        }


        /// <summary>
        /// Calculates the mean of the list of values
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <returns>The mean quartile of the list</returns>
        public static double Mean(this IEnumerable<double> values)
        {
            return !values.Any() ? 0 : values.Mean(0, values.Count());
        }

        /// <summary>
        /// Calculates the mean of the list of values
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <param name="start">From where to start</param>
        /// <param name="end">Where to end</param>
        /// <returns>The mean quartile of the list</returns>
        public static double Mean(this IEnumerable<double> values, int start, int end)
        {
            double s = 0;

            int i = start;
            while (i < end)
            {
                s += values.ElementAt(i);
                Math.Max(Interlocked.Increment(ref i), i - 1);
            }

            return s / (end - start);
        }


    }
}
