// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinningOptions.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Provides options describing how binning should be performed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Specifies the behaviour for handling elements which cannot be assigned to any bin.
    /// </summary>
    public enum BinningOutlierMode
    {
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if any element cannot be assigned to any bin.
        /// </summary>
        RejectOutliers,

        /// <summary>
        /// Counts outliers when computing statistics.
        /// </summary>
        IgnoreOutliers,

        /// <summary>
        /// Ignores outliers when computing statistics.
        /// </summary>
        CountOutliers,
    }

    /// <summary>
    /// Specifies the type of bounds used for binning.
    /// </summary>
    public enum BinningIntervalType
    {
        /// <summary>
        /// Bins have an incusive lower bound.
        /// </summary>
        InclusiveLowerBound,

        /// <summary>
        /// Bins have an incusive upper bound.
        /// </summary>
        InclusiveUpperBound,
    }

    /// <summary>
    /// Specifies the behaviour for handing extreme values which would be excluded by an exclusive bound.
    /// </summary>
    public enum BinningExtremeValueMode
    {
        /// <summary>
        /// Extreme values should be excluded if they do not fall on an inclusive bound.
        /// </summary>
        ExcludeExtremeValues,

        /// <summary>
        /// Extreme values should always be included.
        /// </summary>
        IncludeExtremeValues,
    }

    /// <summary>
    /// Represents options for methods that perform binning.
    /// </summary>
    public class BinningOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinningOptions"/> class.
        /// </summary>
        /// <param name="outlierMode">Specifies the behaviour for handling elements which cannot be assigned to any bin.</param>
        /// <param name="intervalType">The type of interval that each bin represents.</param>
        /// <param name="extremeValuesMode">Specifies whether extreme values should be assigned to the corresponding extreme bin.</param>
        public BinningOptions(BinningOutlierMode outlierMode, BinningIntervalType intervalType, BinningExtremeValueMode extremeValuesMode)
        {
            if (outlierMode != BinningOutlierMode.RejectOutliers &&
                outlierMode != BinningOutlierMode.CountOutliers &&
                outlierMode != BinningOutlierMode.IgnoreOutliers)
            {
                throw new ArgumentException(nameof(outlierMode), "Unsupported binning outlier mode");
            }

            if (intervalType != BinningIntervalType.InclusiveLowerBound &&
                intervalType != BinningIntervalType.InclusiveUpperBound)
            {
                throw new ArgumentException(nameof(outlierMode), "Unsupported bin interval type");
            }

            if (intervalType != BinningIntervalType.InclusiveLowerBound &&
                intervalType != BinningIntervalType.InclusiveUpperBound)
            {
                throw new ArgumentException(nameof(outlierMode), "Unsupported bin interval type");
            }

            this.OutlierMode = outlierMode;
            this.IntervalType = intervalType;
            this.ExtremeValuesMode = extremeValuesMode;
        }

        /// <summary>
        /// Gets a value specififying the behaviour for handling elements which cannot be assigned to any bin.
        /// </summary>
        public BinningOutlierMode OutlierMode { get; }

        /// <summary>
        /// Gets a value specififying the type of interval that each bin represents.
        /// </summary>
        public BinningIntervalType IntervalType { get; }

        /// <summary>
        /// Gets a value specififying the behaviour for handing extreme values which would be excluded by an exclusive bound.
        /// </summary>
        public BinningExtremeValueMode ExtremeValuesMode { get; }
    }
}
