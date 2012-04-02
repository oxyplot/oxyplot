// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The color axis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The color axis.
    /// </summary>
    public class ColorAxis : Axis
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAxis"/> class. 
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

            int index = (int)((value - this.Minimum) / (this.Maximum - this.Minimum) * this.Palette.Colors.Count);

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
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="model">The model.</param>
        /// <param name="axisLayer">The rendering order.</param>
        public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer)
        {
        }
        #endregion
    }
}