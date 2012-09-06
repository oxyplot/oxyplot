// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// The color axis.
    /// </summary>
    public class ColorAxis : Axis
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAxis"/> class. 
        ///   Initializes a new instance of the <see cref="ColorAxis"/> class.
        /// </summary>
        public ColorAxis()
        {
            this.Position = AxisPosition.None;
            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the color of values above the maximum value.
        /// </summary>
        /// <value> The color of the high values. </value>
        public OxyColor HighColor { get; set; }

        /// <summary>
        ///   Gets or sets the color of values below the minimum value.
        /// </summary>
        /// <value> The color of the low values. </value>
        public OxyColor LowColor { get; set; }

        /// <summary>
        ///   Gets or sets the palette.
        /// </summary>
        /// <value> The palette. </value>
        public OxyPalette Palette { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="paletteIndex">
        /// The color map index (less than NumberOfEntries). 
        /// </param>
        /// <returns>
        /// The color. 
        /// </returns>
        public OxyColor GetColor(int paletteIndex)
        {
            if (paletteIndex == -1)
            {
                return this.LowColor;
            }

            if (paletteIndex == -2)
            {
                return this.HighColor;
            }

            return this.Palette.Colors[paletteIndex];
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
        public OxyColor GetColor(double value)
        {
            return this.GetColor(this.GetPaletteIndex(value));
        }

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
        /// If the value is less than minimum, -1 is returned. If the value is greater than maximum, -2 is returned.
        /// </remarks>
        public int GetPaletteIndex(double value)
        {
            if (this.LowColor != null && value < this.Minimum)
            {
                return -1;
            }

            if (this.HighColor != null && value > this.Maximum)
            {
                return -2;
            }

            int index =
                (int)
                ((value - this.ActualMinimum) / (this.ActualMaximum - this.ActualMinimum) * this.Palette.Colors.Count);

            if (index < 0)
            {
                index = 0;
            }

            if (index >= this.Palette.Colors.Count)
            {
                index = this.Palette.Colors.Count - 1;
            }

            return index;
        }

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
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The render context. 
        /// </param>
        /// <param name="model">
        /// The model. 
        /// </param>
        /// <param name="axisLayer">
        /// The rendering order. 
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer)
        {
            if (this.Position == AxisPosition.None)
            {
                return;
            }

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
                        null);
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

            base.Render(rc, model, axisLayer);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the high value of the specified palette index.
        /// </summary>
        /// <param name="paletteIndex">
        /// Index of the palette. 
        /// </param>
        /// <returns>
        /// The value. 
        /// </returns>
        protected double GetHighValue(int paletteIndex)
        {
            return this.GetLowValue(paletteIndex + 1);
        }

        /// <summary>
        /// Gets the low value of the specified palette index.
        /// </summary>
        /// <param name="paletteIndex">
        /// Index of the palette. 
        /// </param>
        /// <returns>
        /// The value. 
        /// </returns>
        protected double GetLowValue(int paletteIndex)
        {
            return ((double)paletteIndex / this.Palette.Colors.Count * (this.ActualMaximum - this.ActualMinimum))
                   + this.ActualMinimum;
        }

        #endregion
    }
}