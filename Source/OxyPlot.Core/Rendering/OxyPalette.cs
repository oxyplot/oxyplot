// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPalette.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a palette of colors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a palette of colors.
    /// </summary>
    public class OxyPalette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPalette" /> class.
        /// </summary>
        public OxyPalette()
        {
            this.Colors = new List<OxyColor>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPalette" /> class.
        /// </summary>
        /// <param name="colors">The colors.</param>
        public OxyPalette(params OxyColor[] colors)
        {
            this.Colors = new List<OxyColor>(colors);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPalette" /> class.
        /// </summary>
        /// <param name="colors">The colors.</param>
        public OxyPalette(IEnumerable<OxyColor> colors)
        {
            this.Colors = new List<OxyColor>(colors);
        }

        /// <summary>
        /// Gets or sets the colors.
        /// </summary>
        /// <value>The colors.</value>
        public IList<OxyColor> Colors { get; set; }

        /// <summary>
        /// Interpolates the specified colors to a palette of the specified size.
        /// </summary>
        /// <param name="paletteSize">The size of the palette.</param>
        /// <param name="colors">The colors.</param>
        /// <returns>A palette.</returns>
        public static OxyPalette Interpolate(int paletteSize, params OxyColor[] colors)
        {
            if (colors == null || colors.Length == 0 || paletteSize < 1)
            {
                // There is no color to interpolate or no color required.
                return new OxyPalette(new OxyColor[0]);
            }

            var palette = new OxyColor[paletteSize];

            double incrementStepSize = (paletteSize == 1) ? 0 : (1.0d / (paletteSize - 1));

            for (int i = 0; i < paletteSize; i++)
            {
                double y = i * incrementStepSize;
                double x = y * (colors.Length - 1);
                int i0 = (int)x;
                int i1 = i0 + 1 < colors.Length ? i0 + 1 : i0;
                palette[i] = OxyColor.Interpolate(colors[i0], colors[i1], x - i0);
            }

            return new OxyPalette(palette);
        }

        /// <summary>
        /// Creates a palette with reversed color order.
        /// </summary>
        /// <returns>The reversed <see cref="OxyPalette" />.</returns>
        public OxyPalette Reverse()
        {
            return new OxyPalette(this.Colors.Reverse());
        }
    }
}