// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentImage.cs" company="OxyPlot">
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
//   Represents an image that can be included in a <see cref="PortableDocument" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an image that can be included in a <see cref="PortableDocument"/>.
    /// </summary>
    public class PortableDocumentImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortableDocumentImage"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitsPerComponent">The number of bits per component.</param>
        /// <param name="bits">The bits.</param>
        /// <param name="maskBits">The bits of the mask.</param>
        /// <param name="interpolate">Interpolate if set to <c>true</c>.</param>
        /// <param name="colorSpace">The color space.</param>
        public PortableDocumentImage(int width, int height, int bitsPerComponent, byte[] bits, byte[] maskBits = null, bool interpolate = true, ColorSpace colorSpace = ColorSpace.DeviceRGB)
        {
            this.Width = width;
            this.Height = height;
            this.BitsPerComponent = bitsPerComponent;
            this.ColorSpace = colorSpace;
            this.Bits = bits;
            this.MaskBits = maskBits;
            this.Interpolate = interpolate;
            this.ColorSpace = ColorSpace.DeviceRGB;
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the bits per component.
        /// </summary>
        /// <value>
        /// The bits per component.
        /// </value>
        public int BitsPerComponent { get; private set; }

        /// <summary>
        /// Gets the color space.
        /// </summary>
        /// <value>
        /// The color space.
        /// </value>
        public ColorSpace ColorSpace { get; private set; }

        /// <summary>
        /// Gets the bits.
        /// </summary>
        /// <value>
        /// The bits.
        /// </value>
        public byte[] Bits { get; private set; }

        /// <summary>
        /// Gets the mask bits.
        /// </summary>
        /// <value>
        /// The mask bits.
        /// </value>
        public byte[] MaskBits { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the image is interpolated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if interpolated; otherwise, <c>false</c>.
        /// </value>
        public bool Interpolate { get; private set; }
    }
}