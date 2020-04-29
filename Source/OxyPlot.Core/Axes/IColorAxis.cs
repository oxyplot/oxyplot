// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IColorAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality for color axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    /// <summary>
    /// Specifies functionality for color axes.
    /// </summary>
    public interface IColorAxis : IPlotElement
    {
        /// <summary>
        /// Gets the color of the specified index in the color palette.
        /// </summary>
        /// <param name="paletteIndex">The color map index (less than NumberOfEntries).</param>
        /// <returns>The color.</returns>
        OxyColor GetColor(int paletteIndex);

        /// <summary>
        /// Gets the palette index of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The palette index.</returns>
        /// <remarks>If the value is less than minimum, 0 is returned. If the value is greater than maximum, Palette.Colors.Count+1 is returned.</remarks>
        int GetPaletteIndex(double value);
    }
}