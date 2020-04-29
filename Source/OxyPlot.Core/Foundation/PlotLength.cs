// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotLength.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents absolute or relative lengths in data or screen space.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Represents absolute or relative lengths in data or screen space.
    /// </summary>
    public struct PlotLength : IEquatable<PlotLength>
    {
        /// <summary>
        /// The unit type
        /// </summary>
        private readonly PlotLengthUnit unit;

        /// <summary>
        /// The value
        /// </summary>
        private readonly double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotLength" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="unit">The unit.</param>
        public PlotLength(double value, PlotLengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        /// <value>The type of the unit.</value>
        public PlotLengthUnit Unit
        {
            get
            {
                return this.unit;
            }
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:PlotLength" /> object have the same value.
        /// </summary>
        /// <param name="other">The length to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(PlotLength other)
        {
            return this.value.Equals(other.value) && this.unit.Equals(other.unit);
        }
    }
}