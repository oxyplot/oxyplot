// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BmpEncoder.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements support for encoding bmp images.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// Implements support for encoding bmp images.
    /// </summary>
    public class BmpEncoder : IImageEncoder
    {
        /// <summary>
        /// The options
        /// </summary>
        private readonly BmpEncoderOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="BmpEncoder" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public BmpEncoder(BmpEncoderOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Encodes the specified image data to png.
        /// </summary>
        /// <param name="pixels">The pixel data (bottom line first).</param>
        /// <returns>The png image data.</returns>
        public byte[] Encode(OxyColor[,] pixels)
        {
            int width = pixels.GetLength(0);
            int height = pixels.GetLength(1);

            var bytes = new byte[width * height * 4];
            int k = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bytes[k++] = pixels[x, y].B;
                    bytes[k++] = pixels[x, y].G;
                    bytes[k++] = pixels[x, y].R;
                    bytes[k++] = pixels[x, y].A;
                }
            }

            var ms = new MemoryStream();
            var w = new BinaryWriter(ms);

            const int OffBits = 14 + 40;
            var size = OffBits + bytes.Length;

            // Bitmap file header (14 bytes)
            w.Write((byte)'B');
            w.Write((byte)'M');
            w.Write((uint)size);
            w.Write((ushort)0);
            w.Write((ushort)0);
            w.Write((uint)OffBits);

            // Bitmap info header (40 bytes)
            WriteBitmapInfoHeader(w, width, height, 32, bytes.Length, this.options.DpiX, this.options.DpiY);

            // Bitmap info V4 header (108 bytes)
            //// WriteBitmapV4Header(w, width, height, 32, bytes.Length, this.options.DpiX, this.options.DpiY);

            // Pixel array (from bottom-left corner)
            w.Write(bytes);

            return ms.ToArray();
        }

        /// <summary>
        /// Encodes the specified 8-bit indexed pixels.
        /// </summary>
        /// <param name="pixels">The pixels.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The image data.</returns>
        public byte[] Encode(byte[,] pixels, OxyColor[] palette)
        {
            if (palette.Length == 0)
            {
                throw new ArgumentException("Palette not defined.", "palette");
            }

            if (palette.Length > 256)
            {
                throw new ArgumentException("Too many colors in the palette.", "palette");
            }

            int width = pixels.GetLength(0);
            int height = pixels.GetLength(1);
            var length = width * height;

            var ms = new MemoryStream();
            var w = new BinaryWriter(ms);

            var offBits = 14 + 40 + (4 * palette.Length);
            var size = offBits + length;

            // Bitmap file header (14 bytes)
            w.Write((byte)'B');
            w.Write((byte)'M');
            w.Write((uint)size);
            w.Write((ushort)0);
            w.Write((ushort)0);
            w.Write((uint)offBits);

            // Bitmap info header
            WriteBitmapInfoHeader(w, width, height, 8, length, this.options.DpiX, this.options.DpiY, palette.Length);

            // Color table
            foreach (var color in palette)
            {
                w.Write(color.B);
                w.Write(color.G);
                w.Write(color.R);
                w.Write(color.A);
            }

            // Pixel array (from bottom-left corner)

            // The bits representing the bitmap pixels are packed in rows. The size of each row is rounded up to a
            // multiple of 4 bytes (a 32-bit DWORD) by padding.
            // For images with height > 1, multiple padded rows are stored consecutively, forming a Pixel Array.

            // ReSharper disable once PossibleLossOfFraction
            int rowSize = (int)Math.Floor((double)((8 * width) + 31) / 32) * 4;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    w.Write(pixels[x, y]);
                }

                // padding
                for (int j = width; j < rowSize; j++)
                {
                    w.Write((byte)0);
                }
            }

            return ms.ToArray();
        }

        /// <summary>
        /// Writes the bitmap info header.
        /// </summary>
        /// <param name="w">The writer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitsPerPixel">The number of bits per pixel.</param>
        /// <param name="length">The length of the pixel data.</param>
        /// <param name="dpix">The horizontal resolution (dpi).</param>
        /// <param name="dpiy">The vertical resolution (dpi).</param>
        /// <param name="colors">The number of colors.</param>
        private static void WriteBitmapInfoHeader(BinaryWriter w, int width, int height, int bitsPerPixel, int length, double dpix, double dpiy, int colors = 0)
        {
            w.Write((uint)40);
            w.Write((uint)width);
            w.Write((uint)height);
            w.Write((ushort)1);
            w.Write((ushort)bitsPerPixel);
            w.Write((uint)0);
            w.Write((uint)length);

            // Convert resolutions to pixels per meter
            w.Write((uint)(dpix / 0.0254));
            w.Write((uint)(dpiy / 0.0254));

            w.Write((uint)colors);
            w.Write((uint)colors);
        }

        /// <summary>
        /// Writes the bitmap V4 header.
        /// </summary>
        /// <param name="w">The writer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitsPerPixel">The number of bits per pixel.</param>
        /// <param name="length">The length.</param>
        /// <param name="dpi">The resolution.</param>
        /// <param name="colors">The number of colors.</param>
        private static void WriteBitmapV4Header(BinaryWriter w, int width, int height, int bitsPerPixel, int length, int dpi, int colors = 0)
        {
            // Convert resolution to pixels per meter
            var ppm = (uint)(dpi / 0.0254);

            w.Write((uint)108);
            w.Write((uint)width);
            w.Write((uint)height);
            w.Write((ushort)1);
            w.Write((ushort)bitsPerPixel);
            w.Write((uint)3);
            w.Write((uint)length);
            w.Write(ppm);
            w.Write(ppm);
            w.Write((uint)colors);
            w.Write((uint)colors);

            // Write the channel bit masks
            w.Write(0x00FF0000);
            w.Write(0x0000FF00);
            w.Write(0x000000FF);
            w.Write(0xFF000000);

            // Write the color space
            w.Write((uint)0x206E6957);
            w.Write(new byte[3 * 3 * 4]);

            // Write the gamma RGB
            w.Write((uint)0);
            w.Write((uint)0);
            w.Write((uint)0);
        }
    }
}