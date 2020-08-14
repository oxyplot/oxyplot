// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearColorAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a linear color axis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a linear color axis.
    /// </summary>
    public class LinearColorAxis : LinearAxis, IColorAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearColorAxis" /> class.
        /// </summary>
        public LinearColorAxis()
        {
            this.Position = AxisPosition.None;
            this.AxisDistance = 20;

            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;
            this.Palette = OxyPalettes.Viridis();

            this.LowColor = OxyColors.Undefined;
            this.HighColor = OxyColors.Undefined;
            this.InvalidNumberColor = OxyColors.Gray;
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
        /// Gets or sets the palette.
        /// </summary>
        /// <value>The palette.</value>
        public OxyPalette Palette { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to render the colors as an image.
        /// </summary>
        /// <value><c>true</c> if the rendering should use an image; otherwise, <c>false</c>.</value>
        public bool RenderAsImage { get; set; }

        /// <summary>
        /// Determines whether the axis is used for X/Y values.
        /// </summary>
        /// <returns><c>true</c> if it is an XY axis; otherwise, <c>false</c> .</returns>
        public override bool IsXyAxis()
        {
            return false;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="paletteIndex">The color map index (less than NumberOfEntries).</param>
        /// <returns>The color.</returns>
        public OxyColor GetColor(int paletteIndex)
        {
            if (paletteIndex == int.MinValue)
            {
                return this.InvalidNumberColor;
            }

            if (paletteIndex == 0)
            {
                return this.LowColor;
            }

            if (paletteIndex == this.Palette.Colors.Count + 1)
            {
                return this.HighColor;
            }

            return this.Palette.Colors[paletteIndex - 1];
        }

        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <returns>The colors.</returns>
        public IEnumerable<OxyColor> GetColors()
        {
            yield return this.LowColor;
            foreach (var color in this.Palette.Colors)
            {
                yield return color;
            }

            yield return this.HighColor;
        }

        /// <summary>
        /// Gets the palette index of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The palette index.</returns>
        /// <remarks>If the value is less than minimum, 0 is returned. If the value is greater than maximum, Palette.Colors.Count+1 is returned.</remarks>
        public int GetPaletteIndex(double value)
        {
            if (double.IsNaN(value))
            {
                return int.MinValue;
            }

            if (!this.LowColor.IsUndefined() && value < this.ActualMinimum)
            {
                return 0;
            }

            if (!this.HighColor.IsUndefined() && value > this.ActualMaximum)
            {
                return this.Palette.Colors.Count + 1;
            }

            int index = 1 + (int)((value - this.ActualMinimum) / (this.ActualMaximum - this.ActualMinimum) * this.Palette.Colors.Count);

            if (index < 1)
            {
                index = 1;
            }

            if (index > this.Palette.Colors.Count)
            {
                index = this.Palette.Colors.Count;
            }

            return index;
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

            if (this.Palette == null)
            {
                throw new InvalidOperationException("No Palette defined for color axis.");
            }

            if (pass == 0)
            {
                double distance = this.AxisDistance;
                double left = this.PlotModel.PlotArea.Left;
                double top = this.PlotModel.PlotArea.Top;
                double width = this.MajorTickSize - 2;
                double height = this.MajorTickSize - 2;

                switch (this.Position)
                {
                    case AxisPosition.Left:
                        left = this.PlotModel.PlotArea.Left - this.PositionTierMinShift - width - distance;
                        top = this.PlotModel.PlotArea.Top;
                        break;
                    case AxisPosition.Right:
                        left = this.PlotModel.PlotArea.Right + this.PositionTierMinShift + distance;
                        top = this.PlotModel.PlotArea.Top;
                        break;
                    case AxisPosition.Top:
                        left = this.PlotModel.PlotArea.Left;
                        top = this.PlotModel.PlotArea.Top - this.PositionTierMinShift - height - distance;
                        break;
                    case AxisPosition.Bottom:
                        left = this.PlotModel.PlotArea.Left;
                        top = this.PlotModel.PlotArea.Bottom + this.PositionTierMinShift + distance;
                        break;
                }

                if (this.RenderAsImage)
                {
                    var axisLength = this.Transform(this.ActualMaximum) - this.Transform(this.ActualMinimum);
                    bool reverse = axisLength > 0;
                    axisLength = Math.Abs(axisLength);

                    if (this.IsHorizontal())
                    {
                        var colorAxisImage = this.GenerateColorAxisImage(reverse);
                        rc.DrawImage(colorAxisImage, left, top, axisLength, height, 1, true);
                    }
                    else
                    {
                        var colorAxisImage = this.GenerateColorAxisImage(reverse);
                        rc.DrawImage(colorAxisImage, left, top, width, axisLength, 1, true);
                    }
                }
                else
                {
                    Action<double, double, OxyColor> drawColorRect = (ylow, yhigh, color) =>
                                       {
                                           double ymin = Math.Min(ylow, yhigh);
                                           double ymax = Math.Max(ylow, yhigh) + 0.5;
                                           rc.DrawRectangle(
                                               this.IsHorizontal()
                                                   ? new OxyRect(ymin, top, ymax - ymin, height)
                                                   : new OxyRect(left, ymin, width, ymax - ymin),
                                               color,
                                               OxyColors.Undefined,
                                               0,
                                               this.EdgeRenderingMode);
                                       };

                    int n = this.Palette.Colors.Count;
                    for (int i = 0; i < n; i++)
                    {
                        double ylow = this.Transform(this.GetLowValue(i));
                        double yhigh = this.Transform(this.GetHighValue(i));
                        drawColorRect(ylow, yhigh, this.Palette.Colors[i]);
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
            }

            base.Render(rc, pass);
        }

        /// <summary>
        /// Gets the high value of the specified palette index.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <returns>The value.</returns>
        protected double GetHighValue(int paletteIndex)
        {
            return this.GetLowValue(paletteIndex + 1);
        }

        /// <summary>
        /// Gets the low value of the specified palette index.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <returns>The value.</returns>
        protected double GetLowValue(int paletteIndex)
        {
            return ((double)paletteIndex / this.Palette.Colors.Count * (this.ActualMaximum - this.ActualMinimum))
                   + this.ActualMinimum;
        }

        /// <summary>
        /// Generates the image used to render the color axis.
        /// </summary>
        /// <param name="reverse">Reverse the colors if set to <c>true</c>.</param>
        /// <returns>An <see cref="OxyImage" /> used to render the color axis.</returns>
        private OxyImage GenerateColorAxisImage(bool reverse)
        {
            int n = this.Palette.Colors.Count;
            var buffer = this.IsHorizontal() ? new OxyColor[n, 1] : new OxyColor[1, n];
            for (var i = 0; i < n; i++)
            {
                var color = this.Palette.Colors[i];
                var i2 = reverse ? n - 1 - i : i;
                if (this.IsHorizontal())
                {
                    buffer[i2, 0] = color;
                }
                else
                {
                    buffer[0, i2] = color;
                }
            }

            return OxyImage.Create(buffer, ImageFormat.Png);
        }
    }
}
