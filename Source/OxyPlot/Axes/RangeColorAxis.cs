// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeColorAxis.cs" company="OxyPlot">
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
//   Represents a color axis that contains colors for specified ranges.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a color axis that contains colors for specified ranges.
    /// </summary>
    public class RangeColorAxis : ColorAxis
    {
        /// <summary>
        /// The ranges
        /// </summary>
        private readonly List<ColorRange> ranges = new List<ColorRange>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeColorAxis"/> class.
        /// </summary>
        public RangeColorAxis()
        {
            this.Position = AxisPosition.None;
            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;
        }

        /// <summary>
        /// Gets or sets the color of values above the maximum value.
        /// </summary>
        /// <value> The color of the high values. </value>
        public OxyColor HighColor { get; set; }

        /// <summary>
        /// Gets or sets the color of values below the minimum value.
        /// </summary>
        /// <value> The color of the low values. </value>
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
        /// Gets the palette index of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The palette index.
        /// </returns>
        /// <remarks>
        /// If the value is less than minimum, 0 is returned. If the value is greater than maximum, Palette.Colors.Count+1 is returned.
        /// </remarks>
        public override int GetPaletteIndex(double value)
        {
            if (this.LowColor != null && value < this.ranges[0].LowerBound)
            {
                return -1;
            }

            if (this.HighColor != null && value > this.ranges[this.ranges.Count - 1].UpperBound)
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
        /// <returns>
        /// The color.
        /// </returns>
        public override OxyColor GetColor(int paletteIndex)
        {
            if (paletteIndex == int.MinValue)
            {
                return OxyColors.Gray;
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

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="model">The model.</param>
        /// <param name="axisLayer">The rendering order.</param>
        /// <param name="pass">The render pass.</param>
        public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer, int pass)
        {
            if (this.Position == AxisPosition.None)
            {
                return;
            }

            if (pass == 0)
            {
                double left = model.PlotArea.Left;
                double top = model.PlotArea.Top;
                double width = this.MajorTickSize - 2;
                double height = this.MajorTickSize - 2;

                const int TierShift = 0;

                switch (this.Position)
                {
                    case AxisPosition.Left:
                        left = model.PlotArea.Left - TierShift - width;
                        top = model.PlotArea.Top;
                        break;
                    case AxisPosition.Right:
                        left = model.PlotArea.Right + TierShift;
                        top = model.PlotArea.Top;
                        break;
                    case AxisPosition.Top:
                        left = model.PlotArea.Left;
                        top = model.PlotArea.Top - TierShift - height;
                        break;
                    case AxisPosition.Bottom:
                        left = model.PlotArea.Left;
                        top = model.PlotArea.Bottom + TierShift;
                        break;
                }

                Action<double, double, OxyColor> drawColorRect = (ylow, yhigh, color) =>
                {
                    double ymin = Math.Min(ylow, yhigh);
                    double ymax = Math.Max(ylow, yhigh);
                    rc.DrawRectangle(
                        this.IsHorizontal()
                            ? new OxyRect(ymin, top, ymax - ymin, height)
                            : new OxyRect(left, ymin, width, ymax - ymin),
                        color,
                        null);
                };

                foreach (ColorRange range in this.ranges)
                {
                    double ylow = this.Transform(range.LowerBound);
                    double yhigh = this.Transform(range.UpperBound);

                    double ymax = this.Transform(ActualMaximum);
                    double ymin = this.Transform(ActualMinimum);

                    if (ylow < ymax)
                    {
                        continue;
                    }

                    if (yhigh > ymin)
                    {
                        continue;
                    }

                    if (ylow > ymin)
                    {
                        ylow = ymin;
                    }

                    if (yhigh < ymax)
                    {
                        yhigh = ymax;
                    }

                    drawColorRect(ylow, yhigh, range.Color);
                }

                double highLowLength = 10;
                if (this.IsHorizontal())
                {
                    highLowLength *= -1;
                }

                if (this.LowColor != null)
                {
                    double ylow = this.Transform(this.ActualMinimum);
                    drawColorRect(ylow, ylow + highLowLength, this.LowColor);
                }

                if (this.HighColor != null)
                {
                    double yhigh = this.Transform(this.ActualMaximum);
                    drawColorRect(yhigh, yhigh - highLowLength, this.HighColor);
                }
            }

            var r = new HorizontalAndVerticalAxisRenderer(rc, model);
            r.Render(this, pass);
        }

        /// <summary>
        /// Defines a range.
        /// </summary>
        private class ColorRange
        {
            /// <summary>
            /// Gets or sets the color.
            /// </summary>
            /// <value>
            /// The color.
            /// </value>
            public OxyColor Color { get; set; }

            /// <summary>
            /// Gets or sets the lower bound.
            /// </summary>
            /// <value>
            /// The lower bound.
            /// </value>
            public double LowerBound { get; set; }

            /// <summary>
            /// Gets or sets the upper bound.
            /// </summary>
            /// <value>
            /// The upper bound.
            /// </value>
            public double UpperBound { get; set; }
        }
    }
}