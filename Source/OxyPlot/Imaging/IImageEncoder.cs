// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImageEncoder.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides functionality to encode an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides functionality to encode an image.
    /// </summary>
    public interface IImageEncoder
    {
        /// <summary>
        /// Encodes the specified pixels.
        /// </summary>
        /// <param name="pixels">
        /// The pixel data (bottom line first).
        /// </param>
        /// <returns>
        /// The image data.
        /// </returns>
        byte[] Encode(OxyColor[,] pixels);

        /// <summary>
        /// Encodes the specified 8-bit indexed pixels.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The image data.</returns>
        byte[] Encode(byte[,] pixels, OxyColor[] palette);
    }
}