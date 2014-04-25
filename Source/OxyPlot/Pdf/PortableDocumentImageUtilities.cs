// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentImageUtilities.cs" company="OxyPlot">
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
//   Provides utility methods related to <see cref="PortableDocumentImage"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides utility methods related to <see cref="PortableDocumentImage" />.
    /// </summary>
    public static class PortableDocumentImageUtilities
    {
        /// <summary>
        /// Converts the specified <see cref="OxyImage" /> to a <see cref="PortableDocumentImage" />.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c>.</param>
        /// <returns>The converted image.</returns>
        public static PortableDocumentImage Convert(OxyImage image, bool interpolate)
        {
            OxyColor[,] pixels;
            try
            {
                pixels = image.GetPixels();
            }
            catch
            {
                // TODO: remove this try/catch block when image decoder is implemented.
                return null;
            }

            var bits = new byte[image.Width * image.Height * 3];
            var maskBits = new byte[image.Width * image.Height];
            int i = 0;
            int j = 0;

            // Start at the top row
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    maskBits[j++] = pixels[x, y].A;
                    bits[i++] = pixels[x, y].R;
                    bits[i++] = pixels[x, y].G;
                    bits[i++] = pixels[x, y].B;
                }
            }

            return new PortableDocumentImage(image.Width, image.Height, 8, bits, maskBits, interpolate);
        }
    }
}