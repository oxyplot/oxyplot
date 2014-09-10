// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an axis with linear scale.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;

    /// <summary>
    /// Represents an axis with linear scale.
    /// </summary>
    public class LinearAxis : Axis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAxis" /> class.
        /// </summary>
        public LinearAxis()
        {
            this.FractionUnit = 1.0;
            this.FractionUnitSymbol = null;
            this.FormatAsFractions = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAxis" /> class.
        /// </summary>
        /// <param name="position">The position of the axis.</param>
        /// <param name="title">The title.</param>
        [Obsolete]
        public LinearAxis(AxisPosition position, string title)
            : this()
        {
            this.Position = position;
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAxis" /> class.
        /// </summary>
        /// <param name="position">The position of the axis.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="title">The title.</param>
        [Obsolete]
        public LinearAxis(AxisPosition position, double minimum = double.NaN, double maximum = double.NaN, string title = null)
            : this(position, minimum, maximum, double.NaN, double.NaN, title)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAxis" /> class.
        /// </summary>
        /// <param name="position">The position of the axis.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="majorStep">The major step.</param>
        /// <param name="minorStep">The minor step.</param>
        /// <param name="title">The title.</param>
        [Obsolete]
        public LinearAxis(AxisPosition position, double minimum, double maximum, double majorStep, double minorStep, string title = null)
            : this(position, title)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.MajorStep = majorStep;
            this.MinorStep = minorStep;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to format numbers as fractions.
        /// </summary>
        public bool FormatAsFractions { get; set; }

        /// <summary>
        /// Gets or sets the fraction unit. Remember to set FormatAsFractions to <c>true</c>.
        /// </summary>
        /// <value>The fraction unit.</value>
        public double FractionUnit { get; set; }

        /// <summary>
        /// Gets or sets the fraction unit symbol. Use FractionUnit = Math.PI and FractionUnitSymbol = "π" if you want the axis to show "π/2,π,3π/2,2π" etc. Use FractionUnit = 1 and FractionUnitSymbol = "L" if you want the axis to show "0,L/2,L" etc. Remember to set FormatAsFractions to <c>true</c>.
        /// </summary>
        /// <value>The fraction unit symbol.</value>
        public string FractionUnitSymbol { get; set; }

        /// <summary>
        /// Determines whether the axis is used for X/Y values.
        /// </summary>
        /// <returns><c>true</c> if it is an XY axis; otherwise, <c>false</c> .</returns>
        public override bool IsXyAxis()
        {
            return true;
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value to format.</param>
        /// <returns>The formatted value.</returns>
        protected override string FormatValueOverride(double x)
        {
            if (this.FormatAsFractions)
            {
                return FractionHelper.ConvertToFractionString(x, this.FractionUnit, this.FractionUnitSymbol, 1e-6, this.ActualCulture, this.StringFormat);
            }

            return base.FormatValueOverride(x);
        }
    }
}