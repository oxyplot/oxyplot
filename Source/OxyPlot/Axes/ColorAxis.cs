// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorAxis.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   The color axis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    /// <summary>
    /// Provides an abstract base class for color axes.
    /// </summary>
    public abstract class ColorAxis : Axis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAxis"/> class.
        /// </summary>
        protected ColorAxis()
        {
            this.AxisDistance = 20;
            this.InvalidNumberColor = OxyColors.Gray;
        }

        /// <summary>
        /// Gets or sets the color used to represent NaN values.
        /// </summary>
        /// <value>
        /// A <see cref="OxyColor"/> that defines the color. The default value is <c>OxyColors.Gray</c>.
        /// </value>
        public OxyColor InvalidNumberColor { get; set; }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="paletteIndex">
        /// The color map index.
        /// </param>
        /// <returns>
        /// The color.
        /// </returns>
        public abstract OxyColor GetColor(int paletteIndex);

        /// <summary>
        /// Gets the palette index of the specified value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The palette index.
        /// </returns>
        /// <remarks>
        /// If the value is less than minimum, 0 is returned. If the value is greater than maximum, Palette.Colors.Count+1 is returned.
        /// </remarks>
        public abstract int GetPaletteIndex(double value);

        /// <summary>
        /// Determines whether the axis is used for X/Y values.
        /// </summary>
        /// <returns>
        /// <c>true</c> if it is an XY axis; otherwise, <c>false</c> .
        /// </returns>
        public override bool IsXyAxis()
        {
            return false;
        }

        /// <summary>
        /// Gets the color for the specified value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The color.
        /// </returns>
        public virtual OxyColor GetColor(double value)
        {
            return this.GetColor(this.GetPaletteIndex(value));
        }
    }
}