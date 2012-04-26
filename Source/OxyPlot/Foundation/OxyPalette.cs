// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPalette.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a palette of colors.
    /// </summary>
    public class OxyPalette
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPalette"/> class. 
        ///   Initializes a new instance of the <see cref="OxyPalette"/> class.
        /// </summary>
        public OxyPalette()
        {
            this.Colors = new List<OxyColor>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPalette"/> class. 
        /// </summary>
        /// <param name="colors">
        /// The colors. 
        /// </param>
        public OxyPalette(params OxyColor[] colors)
        {
            this.Colors = new List<OxyColor>(colors);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPalette"/> class. 
        /// </summary>
        /// <param name="colors">
        /// The colors. 
        /// </param>
        public OxyPalette(IEnumerable<OxyColor> colors)
        {
            this.Colors = new List<OxyColor>(colors);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the colors.
        /// </summary>
        /// <value> The colors. </value>
        public IList<OxyColor> Colors { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Interpolates the specified colors to a palette of the specified size.
        /// </summary>
        /// <param name="paletteSize">
        /// The size of the palette. 
        /// </param>
        /// <param name="colors">
        /// The colors. 
        /// </param>
        /// <returns>
        /// A palette. 
        /// </returns>
        public static OxyPalette Interpolate(int paletteSize, params OxyColor[] colors)
        {
            var palette = new OxyColor[paletteSize];
            for (int i = 0; i < paletteSize; i++)
            {
                double y = (double)i / (paletteSize - 1);
                double x = y * (colors.Length - 1);
                int i0 = (int)x;
                int i1 = i0 + 1 < colors.Length ? i0 + 1 : i0;
                palette[i] = OxyColor.Interpolate(colors[i0], colors[i1], x - i0);
            }

            return new OxyPalette(palette);
        }

        #endregion
    }
}
