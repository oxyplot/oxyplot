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
    public class PieSlice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "PieSlice" /> class.
        /// </summary>
        public PieSlice()
        {
            this.Fill = OxyColors.Automatic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSlice" /> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        public PieSlice(string label, double value)
            : this()
        {
            this.Label = label;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets Fill.
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
        /// Gets or sets a value indicating whether IsExploded.
        /// </summary>
        public bool IsExploded { get; set; }

        /// <summary>
        /// Gets or sets Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the default fill color.
        /// </summary>
        /// <value>The default fill color.</value>
        internal OxyColor DefaultFillColor { get; set; }
    }
}