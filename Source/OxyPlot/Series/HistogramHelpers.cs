// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramHelpers.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides methods to collect data samples into bins for use with a <see cref="HistogramSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    /// <summary>
    /// Provides methods to collect data samples into bins for use with a <see cref="HistogramSeries" />.
    /// </summary>
    public static class HistogramHelpers
    {
        /// <summary>
        /// Collects samples into <paramref name="binCount"/> bins (<see cref="HistogramItem" />), uniformly distributed between <paramref name="start"/> and <paramref name="end"/>.
        /// </summary>
        /// <param name="samples">The samples to collect into bins.</param>
        /// <param name="start">The inclusive lower-bound of the first bin.</param>
        /// <param name="end">The exclusive upper-bound of the last bin.</param>
        /// <param name="binCount">The number of bins to create.</param>
        /// <param name="countUnplaced">A value indicating whether items not placed in bins should be considered when computing frequencies.</param>
        /// <returns>An enumeration of <see cref="HistogramItem" /> corresponding to uniformly generated bins with heights computed from the proportion of samples placed within.</returns>
        public static IEnumerable<HistogramItem> Collect(IEnumerable<double> samples, double start, double end, int binCount, bool countUnplaced)
        {
            if (binCount < 1)
            {
                throw new ArgumentException("Bin Count cannot be less than one.", nameof(binCount));
            }

            if (end <= start)
            {
                throw new ArgumentException("End cannot be less than or equal to Start.", nameof(end));
            }
            
            List<double> binBreaks = new List<double>(binCount);

            for (int i = 0; i <= binCount; i++)
            {
                binBreaks.Add(start + (((end - start) / binCount) * i));
            }

            return Collect(samples, binBreaks, countUnplaced);
        }

        /// <summary>
        /// Collects samples into tightly packed bins (<see cref="HistogramItem" />) defined by <paramref name="binBreaks"/>.
        /// </summary>
        /// <param name="samples">The samples to collect into bins.</param>
        /// <param name="binBreaks">The start and end values for the bins.</param>
        /// <param name="countUnplaced">A value indicating whether items not placed in bins should be considered when computing frequencies.</param>
        /// <returns>An enumerations of <see cref="HistogramItem" /> corresponding to the generated bins with heights computed from the proportion of samples placed within.</returns>
        public static IEnumerable<HistogramItem> Collect(IEnumerable<double> samples, IReadOnlyList<double> binBreaks, bool countUnplaced)
        {
            // determin ranges
            double[] orderedBreaks = binBreaks.Distinct().OrderBy(b => b).ToArray();

            // count samples
            List<int> counts = new List<int>();
            long total = 0;
            
            for (int i = 0; i < binBreaks.Count - 1; i++)
            {
                counts.Add(0);
            }

            foreach (double sample in samples)
            {
                int idx = System.Array.BinarySearch(orderedBreaks, sample);

                bool placed = false;
                
                if (idx >= 0)
                {
                    // exact match, place in the corresponding bin (exclude last bin)
                    if (idx < counts.Count)
                    {
                        counts[idx] += 1;
                        placed = true;
                    }
                }
                else
                {
                    // inexact match, place in lower bin
                    idx = ~idx - 1;
                    
                    if (idx >= 0 && idx < counts.Count)
                    {
                        counts[idx] += 1;
                        placed = true;
                    }
                }

                if (placed || countUnplaced)
                {
                    total++;
                }
            }

            // create items
            List<HistogramItem> items = new List<HistogramItem>(counts.Count);
            
            for (int i = 0; i < binBreaks.Count - 1; i++)
            {
                items.Add(new HistogramItem(binBreaks[i], binBreaks[i + 1], (double)counts[i] / total));
            }

            return items;
        }
    }
}
