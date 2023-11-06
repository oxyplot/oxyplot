// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryColorAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a categorized color axis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using OxyPlot.Axes.Rendering;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a categorized color axis.
    /// </summary>
    public class CategoryColorAxis : CategoryAxis, IColorAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryColorAxis" /> class.
        /// </summary>
        public CategoryColorAxis()
        {
            this.Palette = new OxyPalette();
        }

        /// <summary>
        /// Gets or sets the invalid category color.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor InvalidCategoryColor { get; set; }

        /// <summary>
        /// Gets or sets the palette.
        /// </summary>
        /// <value>The palette.</value>
        public OxyPalette Palette { get; set; }

        /// <summary>
        /// Gets the color of the specified index in the color palette.
        /// </summary>
        /// <param name="paletteIndex">The color map index (less than NumberOfEntries).</param>
        /// <returns>The color.</returns>
        public OxyColor GetColor(int paletteIndex)
        {
            if (paletteIndex == -1)
            {
                return this.InvalidCategoryColor;
            }

            if (paletteIndex >= this.Palette.Colors.Count)
            {
                return this.InvalidCategoryColor;
            }

            return this.Palette.Colors[paletteIndex];
        }

        /// <summary>
        /// Gets the palette index of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The palette index.</returns>
        /// <remarks>If the value is less than minimum, 0 is returned. If the value is greater than maximum, Palette.Colors.Count+1 is returned.</remarks>
        public int GetPaletteIndex(double value)
        {
            return (int)value;
        }

        /// <inheritdoc />
        public override void Render(IRenderContext rc, int pass)
        {
            var renderer = new CategoryColorAxisRenderer(rc, this.PlotModel);
            renderer.Render(this, pass);
        }

        /// <summary>
        /// Gets the high value.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <returns>The value.</returns>
        protected internal double GetHighValue(int paletteIndex, IList<double> majorLabelValues)
        {
            double highValue = paletteIndex >= this.Palette.Colors.Count - 1
                                   ? this.ClipMaximum
                                   : (majorLabelValues[paletteIndex] + majorLabelValues[paletteIndex + 1]) / 2;
            return highValue;
        }

        /// <summary>
        /// Gets the low value.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <returns>The value.</returns>
        protected internal double GetLowValue(int paletteIndex, IList<double> majorLabelValues)
        {
            double lowValue = paletteIndex == 0
                                  ? this.ClipMinimum
                                  : (majorLabelValues[paletteIndex - 1] + majorLabelValues[paletteIndex]) / 2;
            return lowValue;
        }
    }
}
