// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item used in the BarSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item used in the BarSeries.
    /// </summary>
    public class BarItem : BarItemBase, ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem" /> class.
        /// </summary>
        public BarItem()
        {
            this.Value = double.NaN;
            this.Color = OxyColors.Automatic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="categoryIndex">Index of the category.</param>
        public BarItem(double value, int categoryIndex = -1)
        {
            this.Color = OxyColors.Automatic;
            this.Value = value;
            this.CategoryIndex = categoryIndex;
        }

        /// <summary>
        /// Gets or sets the color of the item.
        /// </summary>
        /// <remarks>If the color is not specified (default), the color of the series will be used.</remarks>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the value of the item.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public virtual string ToCode()
        {
            if (!this.Color.IsUndefined())
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2}", this.Value, this.CategoryIndex, this.Color.ToCode());
            }

            if (this.CategoryIndex != -1)
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Value, this.CategoryIndex);
            }

            return CodeGenerator.FormatConstructor(this.GetType(), "{0}", this.Value);
        }
    }
}
