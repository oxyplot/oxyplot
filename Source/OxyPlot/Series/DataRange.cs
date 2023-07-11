// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRange.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an interval defined by two doubles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    /// <summary>
    /// Represents an interval defined by two doubles.
    /// </summary>
    public struct DataRange : ICodeGenerating
    {
        /// <summary>
        /// The undefined data range.
        /// </summary>
        public static readonly DataRange Undefined = default;

        private readonly double minimum;
        private readonly double maximum;
        private readonly bool isDefined;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRange"/> struct.
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        public DataRange(double min, double max)
        {
            if (double.IsNaN(min) || double.IsNaN(max))
            {
                throw new ArgumentException("NaN values are not permitted");
            }

            if (max < min)
            {
                throw new ArgumentException("max must be larger or equal min");
            }

            this.minimum = min;
            this.maximum = max;

            this.isDefined = true;
        }

        /// <summary>
        /// Gets the lower bound of the data range.
        /// </summary>
        public double Minimum => this.minimum;

        /// <summary>
        /// Gets the upper bound of the data range.
        /// </summary>
        public double Maximum => this.maximum;

        /// <summary>
        /// Gets the difference between maximum and minimum.
        /// </summary>
        public double Range => this.Maximum - this.Minimum;

        /// <summary>
        /// Determines whether this data range is defined.
        /// </summary>
        /// <returns><c>true</c> if this instance is defined, otherwise <c>false</c>.</returns>
        public bool IsDefined()
        {
            return this.isDefined;
        }

        /// <summary>
        /// Determines whether the specified value lies
        /// within the closed interval of the data range.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <returns><c>true</c> if value in range, otherwise <c>false</c>.</returns>
        public bool Contains(double value)
        {
            return value >= this.Minimum && value <= this.Maximum;
        }

        /// <summary>
        /// Determines whether the specified data range
        /// intersects with this instance.
        /// </summary>
        /// <param name="other">the other interval to be checked.</param>
        /// <returns><c>true</c> if intersects, otherwise <c>false</c>.</returns>
        public bool IntersectsWith(DataRange other)
        {
            return (this.IsDefined() && other.IsDefined()) &&
                   (this.Contains(other.Minimum) ||
                    this.Contains(other.Maximum) ||
                    other.Contains(this.Minimum) ||
                    other.Contains(this.Maximum));
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Minimum, this.Maximum);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"[{this.Minimum}, {this.Maximum}]";
        }
    }
}
