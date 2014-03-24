// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IColorAxis.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines functionality for color axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    /// <summary>
    /// Defines functionality for color axes.
    /// </summary>
    public interface IColorAxis
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