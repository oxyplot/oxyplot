// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentImageUtilities.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides utility methods related to <see cref="PortableDocumentImage" />.
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