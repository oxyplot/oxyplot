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
        /// Determines whether the axis is logarithmic.
        /// </summary>
        /// <returns><c>true</c> if it is a logarithmic axis; otherwise, <c>false</c> .</returns>
        public override bool IsLogarithmic()
        {
            return false;
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