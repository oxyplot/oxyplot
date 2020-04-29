// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageEncoderOptions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for image encoder options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides an abstract base class for image encoder options.
    /// </summary>
    public abstract class ImageEncoderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEncoderOptions" /> class.
        /// </summary>
        protected ImageEncoderOptions()
        {
            this.DpiX = 96;
            this.DpiY = 96;
        }

        /// <summary>
        /// Gets or sets the horizontal resolution (in dots per inch).
        /// </summary>
        /// <value>The resolution. The default value is 96 dpi.</value>
        public double DpiX { get; set; }

        /// <summary>
        /// Gets or sets the vertical resolution (in dots per inch).
        /// </summary>
        /// <value>The resolution. The default value is 96 dpi.</value>
        public double DpiY { get; set; }
    }
}