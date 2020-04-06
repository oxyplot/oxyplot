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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a color axis that contains colors for specified ranges.
    /// </summary>
    public class RangeColorAxis : LinearAxis, IColorAxis
    {
        /// <summary>
        /// The ranges
        /// </summary>
        private readonly List<ColorRange> ranges = new List<ColorRange>();

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

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pass">The render pass.</param>
        public override void Render(IRenderContext rc, int pass)
        {
            if (this.Position == AxisPosition.None)
            {
                return;
            }

            if (pass == 0)
            {
                double distance = this.AxisDistance;
                double left = this.PlotModel.PlotArea.Left;
                double top = this.PlotModel.PlotArea.Top;
                double width = this.MajorTickSize - 2;
                double height = this.MajorTickSize - 2;

                const int TierShift = 0;

                switch (this.Position)
                {
                    case AxisPosition.Left:
                        left = this.PlotModel.PlotArea.Left - TierShift - width - distance;
                        top = this.PlotModel.PlotArea.Top;
                        break;
                    case AxisPosition.Right:
                        left = this.PlotModel.PlotArea.Right + TierShift + distance;
                        top = this.PlotModel.PlotArea.Top;
                        break;
                    case AxisPosition.Top:
                        left = this.PlotModel.PlotArea.Left;
                        top = this.PlotModel.PlotArea.Top - TierShift - height - distance;
                        break;
                    case AxisPosition.Bottom:
                        left = this.PlotModel.PlotArea.Left;
                        top = this.PlotModel.PlotArea.Bottom + TierShift + distance;
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
                        OxyColors.Undefined,
                        0,
                        this.EdgeRenderingMode);
                };

                // if the axis is reversed then the min and max values need to be swapped.
                double effectiveMaxY = this.Transform(this.IsReversed ? this.ActualMinimum : this.ActualMaximum);
                double effectiveMinY = this.Transform(this.IsReversed ? this.ActualMaximum : this.ActualMinimum);

                foreach (ColorRange range in this.ranges)
                {
                    double ylow = this.Transform(range.LowerBound);
                    double yhigh = this.Transform(range.UpperBound);

                    if (this.IsHorizontal())
                    {
                        if (ylow < effectiveMinY)
                        {
                            ylow = effectiveMinY;
                        }

                        if (yhigh > effectiveMaxY)
                        {
                            yhigh = effectiveMaxY;
                        }
                    }
                    else
                    {
                        if (ylow > effectiveMinY)
                        {
                            ylow = effectiveMinY;
                        }

                        if (yhigh < effectiveMaxY)
                        {
                            yhigh = effectiveMaxY;
                        }
                    }

                    drawColorRect(ylow, yhigh, range.Color);
                }

                double highLowLength = 10;
                if (this.IsHorizontal())
                {
                    highLowLength *= -1;
                }

                if (!this.LowColor.IsUndefined())
                {
                    double ylow = this.Transform(this.ActualMinimum);
                    drawColorRect(ylow, ylow + highLowLength, this.LowColor);
                }

                if (!this.HighColor.IsUndefined())
                {
                    double yhigh = this.Transform(this.ActualMaximum);
                    drawColorRect(yhigh, yhigh - highLowLength, this.HighColor);
                }
            }

            var r = new HorizontalAndVerticalAxisRenderer(rc, this.PlotModel);
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
