// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImageEncoder.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality to encode an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality to encode an image.
    /// </summary>
    public interface IImageEncoder
    {
        /// <summary>
        /// Encodes the specified pixels.
        /// </summary>
        /// <param name="pixels">The pixel data. The indexing is [x,y] where [0,0] is top-left.</param>
        /// <returns>The image data.</returns>
        byte[] Encode(OxyColor[,] pixels);

        /// <summary>
        /// Encodes the specified 8-bit indexed pixels.
        /// </summary>
        /// <param name="pixels">The indexed pixel data. The indexing is [x,y] where [0,0] is top-left.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The image data.</returns>
        byte[] Encode(byte[,] pixels, OxyColor[] palette);
    }
}