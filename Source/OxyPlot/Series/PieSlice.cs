// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieSlice.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represent a slice of a <see cref="PieSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represent a slice of a <see cref="PieSeries" />.
    /// </summary>
    public class PieSlice : ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PieSlice" /> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        public PieSlice(string label, double value)
        {
            this.Fill = OxyColors.Automatic;
            this.Label = label;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets the actual fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualFillColor
        {
            get { return this.Fill.GetActualColor(this.DefaultFillColor); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the slice is exploded.
        /// </summary>
        public bool IsExploded { get; set; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Gets or sets the default fill color.
        /// </summary>
        /// <value>The default fill color.</value>
        internal OxyColor DefaultFillColor { get; set; }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(
                this.GetType(), "{0}, {1}", this.Label, this.Value);
        }
    }
}