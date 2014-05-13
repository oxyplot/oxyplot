// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImageDecoder.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Specifies functionality to decode an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality to decode an image.
    /// </summary>
    public interface IImageDecoder
    {
        /// <summary>
        /// Gets information about the image in the specified byte array.
        /// </summary>
        /// <param name="bytes">The image data.</param>
        /// <returns>An <see cref="OxyImageInfo" /> structure.</returns>
        OxyImageInfo GetImageInfo(byte[] bytes);

        /// <summary>
        /// Decodes an image from the specified byte array.
        /// </summary>
        /// <param name="bytes">The image data.</param>
        /// <returns>The 32-bit pixel data. The indexing is [x,y] where [0,0] is top-left.</returns>
        OxyColor[,] Decode(byte[] bytes);
    }
}