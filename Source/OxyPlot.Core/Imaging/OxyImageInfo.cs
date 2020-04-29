// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyImageInfo.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides information about an <see cref="OxyImage" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides information about an <see cref="OxyImage" />.
    /// </summary>
    public class OxyImageInfo
    {
        /// <summary>
        /// Gets or sets the width in pixels.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height in pixels.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the bits per pixel.
        /// </summary>
        /// <value>The bits per pixel.</value>
        public int BitsPerPixel { get; set; }

        /// <summary>
        /// Gets or sets the horizontal resolution of the image.
        /// </summary>
        /// <value>The resolution in dots per inch (dpi).</value>
        public double DpiX { get; set; }

        /// <summary>
        /// Gets or sets the vertical resolution of the image.
        /// </summary>
        /// <value>The resolution in dots per inch (dpi).</value>
        public double DpiY { get; set; }
    }
}