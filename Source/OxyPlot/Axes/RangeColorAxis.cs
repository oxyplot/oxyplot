// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeColorAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a color axis that contains colors for specified ranges.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using OxyPlot.Axes.Rendering;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a color axis that contains colors for specified ranges.
    /// </summary>
    public class RangeColorAxis : LinearAxis, IColorAxis
    {
        /// <summary>
        /// The ranges
        /// </summary>
        internal readonly List<ColorRange> ranges = new List<ColorRange>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeColorAxis" /> class.
        /// </summary>
        public RangeColorAxis()
        {
            this.Position = AxisPosition.None;
            this.AxisDistance = 20;

            this.LowColor = OxyColors.Undefined;
            this.HighColor = OxyColors.Undefined;
            this.InvalidNumberColor = OxyColors.Gray;

            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;
        }

        /// <summary>
        /// Gets or sets the color used to represent NaN values.
        /// </summary>
        /// <value>A <see cref="OxyColor" /> that defines the color. The default value is <c>OxyColors.Gray</c>.</value>
        public OxyColor InvalidNumberColor { get; set; }

        /// <summary>
        /// Gets or sets the color of values above the maximum value.
        /// </summary>
        /// <value>The color of the high values.</value>
        public OxyColor HighColor { get; set; }

        /// <summary>
        /// Gets or sets the color of values below the minimum value.
        /// </summary>
        /// <value>The color of the low values.</value>
        public OxyColor LowColor { get; set; }

        /// <summary>
        /// Adds a range.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <param name="color">The color.</param>
        public void AddRange(double lowerBound, double upperBound, OxyColor color)
        {
            this.ranges.Add(new ColorRange { LowerBound = lowerBound, UpperBound = upperBound, Color = color });
        }

        /// <summary>
        /// Clears the ranges.
        /// </summary>
        public void ClearRanges()
        {
            this.ranges.Clear();
        }

        /// <summary>
        /// Gets the palette index of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The palette index.</returns>
        /// <remarks>If the value is less than minimum, 0 is returned. If the value is greater than maximum, Palette.Colors.Count+1 is returned.</remarks>
        public int GetPaletteIndex(double value)
        {
            if (!this.LowColor.IsUndefined() && value < this.ranges[0].LowerBound)
            {
                return -1;
            }

            if (!this.HighColor.IsUndefined() && value > this.ranges[this.ranges.Count - 1].UpperBound)
            {
                return this.ranges.Count;
            }

            // TODO: change to binary search?
            for (int i = 0; i < this.ranges.Count; i++)
            {
                var range = this.ranges[i];
                if (range.LowerBound <= value && range.UpperBound > value)
                {
                    return i;
                }
            }

            return int.MinValue;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="paletteIndex">The color map index.</param>
        /// <returns>The color.</returns>
        public OxyColor GetColor(int paletteIndex)
        {
            if (paletteIndex == int.MinValue)
            {
                return this.InvalidNumberColor;
            }

            if (paletteIndex == -1)
            {
                return this.LowColor;
            }

            if (paletteIndex == this.ranges.Count)
            {
                return this.HighColor;
            }

            return this.ranges[paletteIndex].Color;
        }

        /// <inheritdoc />
        public override void Render(IRenderContext rc, int pass)
        {
            var renderer = new RangeColorAxisRenderer(rc, this.PlotModel);
            renderer.Render(this, pass);
        }

        /// <summary>
        /// Defines a range.
        /// </summary>
        internal class ColorRange
        {
            /// <summary>
            /// Gets or sets the color.
            /// </summary>
            /// <value>The color.</value>
            public OxyColor Color { get; set; }

            /// <summary>
            /// Gets or sets the lower bound.
            /// </summary>
            /// <value>The lower bound.</value>
            public double LowerBound { get; set; }

            /// <summary>
            /// Gets or sets the upper bound.
            /// </summary>
            /// <value>The upper bound.</value>
            public double UpperBound { get; set; }
        }
    }
}
