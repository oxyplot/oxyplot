// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TornadoBarItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item for the TornadoBarSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item for the TornadoBarSeries.
    /// </summary>
    public class TornadoBarItem : BarItemBase, ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TornadoBarItem" /> class.
        /// </summary>
        public TornadoBarItem()
        {
            this.Minimum = double.NaN;
            this.Maximum = double.NaN;
            this.BaseValue = double.NaN;
            this.MinimumColor = OxyColors.Automatic;
            this.MaximumColor = OxyColors.Automatic;
        }

        /// <summary>
        /// Gets or sets the base value.
        /// </summary>
        public double BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the color for the maximum bar.
        /// </summary>
        public OxyColor MaximumColor { get; set; }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the color for the minimum bar.
        /// </summary>
        public OxyColor MinimumColor { get; set; }

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public string ToCode()
        {
            if (!this.MaximumColor.IsUndefined())
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(),
                    "{0},{1},{2},{3},{4}",
                    this.Minimum,
                    this.Maximum,
                    this.BaseValue,
                    this.MinimumColor.ToCode(),
                    this.MaximumColor.ToCode());
            }

            if (!this.MinimumColor.IsUndefined())
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(),
                    "{0},{1},{2},{3}",
                    this.Minimum,
                    this.Maximum,
                    this.BaseValue,
                    this.MinimumColor.ToCode());
            }

            if (!double.IsNaN(this.BaseValue))
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2}", this.Minimum, this.Maximum, this.BaseValue);
            }

            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Minimum, this.Maximum);
        }
    }
}