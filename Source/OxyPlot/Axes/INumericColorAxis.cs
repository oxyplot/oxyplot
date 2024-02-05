// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INumericColorAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    /// <summary>
    /// Specifies functionality for numeric color axes.
    /// </summary>
    public interface INumericColorAxis : IColorAxis
    {
        /// <summary>
        /// Gets or sets the palette.
        /// </summary>
        /// <value>The palette.</value>
        OxyPalette Palette { get; set; }

        /// <summary>
        /// Gets or sets the color of values above the maximum value.
        /// </summary>
        /// <value>The color of the high values.</value>
        OxyColor HighColor { get; set; }

        /// <summary>
        /// Gets or sets the color of values below the minimum value.
        /// </summary>
        /// <value>The color of the low values.</value>
        OxyColor LowColor { get; set; }

        /// <summary>
        /// Gets or sets the color used to represent NaN values.
        /// </summary>
        /// <value>A <see cref="OxyColor" /> that defines the color. The default value is <c>OxyColors.Gray</c>.</value>
        OxyColor InvalidNumberColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to render the colors as an image.
        /// </summary>
        /// <value><c>true</c> if the rendering should use an image; otherwise, <c>false</c>.</value>
        bool RenderAsImage { get; set; }
    }
}
