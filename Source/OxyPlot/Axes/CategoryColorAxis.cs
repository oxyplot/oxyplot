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
    using System;
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

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="model">The model.</param>
        /// <param name="axisLayer">The layer.</param>
        /// <param name="pass">The pass.</param>
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

                switch (this.Position)
                {
                    case AxisPosition.Left:
                        left = model.PlotArea.Left - this.PositionTierMinShift - width;
                        top = model.PlotArea.Top;
                        break;
                    case AxisPosition.Right:
                        left = model.PlotArea.Right + this.PositionTierMinShift;
                        top = model.PlotArea.Top;
                        break;
                    case AxisPosition.Top:
                        left = model.PlotArea.Left;
                        top = model.PlotArea.Top - this.PositionTierMinShift - height;
                        break;
                    case AxisPosition.Bottom:
                        left = model.PlotArea.Left;
                        top = model.PlotArea.Bottom + this.PositionTierMinShift;
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
                        OxyColors.Undefined);
                };

                IList<double> majorLabelValues;
                IList<double> majorTickValues;
                IList<double> minorTickValues;
                this.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);

                int n = this.Palette.Colors.Count;
                for (int i = 0; i < n; i++)
                {
                    double low = this.Transform(this.GetLowValue(i, majorLabelValues));
                    double high = this.Transform(this.GetHighValue(i, majorLabelValues));
                    drawColorRect(low, high, this.Palette.Colors[i]);
                }
            }

            base.Render(rc, model, axisLayer, pass);
        }

        /// <summary>
        /// Gets the high value of the specified palette index.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <returns>The value.</returns>
        protected double GetHighValue(int paletteIndex)
        {
            IList<double> majorLabelValues;
            IList<double> majorTickValues;
            IList<double> minorTickValues;
            this.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            var highValue = this.GetHighValue(paletteIndex, majorLabelValues);
            return highValue;
        }

        /// <summary>
        /// Gets the high value.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <returns>The value.</returns>
        private double GetHighValue(int paletteIndex, IList<double> majorLabelValues)
        {
            double highValue = paletteIndex >= this.Palette.Colors.Count - 1
                                   ? this.ActualMaximum
                                   : (majorLabelValues[paletteIndex] + majorLabelValues[paletteIndex + 1]) / 2;
            return highValue;
        }

        /// <summary>
        /// Gets the low value.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <returns>The value.</returns>
        private double GetLowValue(int paletteIndex, IList<double> majorLabelValues)
        {
            double lowValue = paletteIndex == 0
                                  ? this.ActualMinimum
                                  : (majorLabelValues[paletteIndex - 1] + majorLabelValues[paletteIndex]) / 2;
            return lowValue;
        }
    }
}