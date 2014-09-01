// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyImage.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents an image.
    /// </summary>
    public class OxyImage
    {
        /// <summary>
        /// The image data.
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// The pixels
        /// </summary>
        //// TODO: remove when PNG decoder is implemented
        private OxyColor[,] pixels;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyImage" /> class from the specified stream.
        /// </summary>
        /// <param name="s">A stream that provides the image data.</param>
        public OxyImage(Stream s)
            : this(GetBytes(s))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyImage" /> class from a byte array.
        /// </summary>
        /// <param name="bytes">The image bytes.</param>
        public OxyImage(byte[] bytes)
        {
            this.data = bytes;
            this.Format = GetImageFormat(bytes);
            this.UpdateImageInfo();
        }

        /// <summary>
        /// Gets the image format.
        /// </summary>
        /// <value>The format.</value>
        public ImageFormat Format { get; private set; }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the number of bits per pixel.
        /// </summary>
        /// <value>The bits per pixel.</value>
        public int BitsPerPixel { get; private set; }

        /// <summary>
        /// Gets the horizontal resolution of the image.
        /// </summary>
        /// <value>The resolution in dots per inch (dpi).</value>
        public double DpiX { get; private set; }

        /// <summary>
        /// Gets the vertical resolution of the image.
        /// </summary>
        /// <value>The resolution in dots per inch (dpi).</value>
        public double DpiY { get; private set; }

        /// <summary>
        /// Creates an image from 8-bit indexed pixels.
        /// </summary>
        /// <param name="pixels">The pixels indexed as [x,y]. [0,0] is top-left.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="format">The image format.</param>
        /// <param name="encoderOptions">The encoder options.</param>
        /// <returns>An <see cref="OxyImage" /></returns>
        public static OxyImage Create(
            byte[,] pixels,
            OxyColor[] palette,
            ImageFormat format,
            ImageEncoderOptions encoderOptions = null)
        {
            var encoder = GetEncoder(format, encoderOptions);
            return new OxyImage(encoder.Encode(pixels, palette));
        }

        /// <summary>
        /// Creates an image from 32-bit <c>true</c>-color pixels.
        /// </summary>
        /// <param name="pixels">The pixels indexed as [x,y]. [0,0] is top-left.</param>
        /// <param name="format">The image format.</param>
        /// <param name="encoderOptions">The encoder options.</param>
        /// <returns>An <see cref="OxyImage" /></returns>
        public static OxyImage Create(OxyColor[,] pixels, ImageFormat format, ImageEncoderOptions encoderOptions = null)
        {
            var encoder = GetEncoder(format, encoderOptions);
            var image = new OxyImage(encoder.Encode(pixels));

            // TODO: remove when PNG decoder is implemented
            image.pixels = pixels;

            return image;
        }

        /// <summary>
        /// Gets the image data.
        /// </summary>
        /// <returns>The image data as a byte array.</returns>
        public byte[] GetData()
        {
            return this.data;
        }

        /// <summary>
        /// Gets the pixels of the image.
        /// </summary>
        /// <returns>The pixels in an array [width,height]. [0,0] is top-left.</returns>
        public OxyColor[,] GetPixels()
        {
            // TODO: remove when PNG decoder is implemented
            if (this.pixels != null)
            {
                return this.pixels;
            }

            var decoder = GetDecoder(this.Format);
            return decoder.Decode(this.data);
        }

        /// <summary>
        /// Gets the <see cref="IImageDecoder" /> for the specified format.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <returns>The <see cref="IImageDecoder" />.</returns>
        private static IImageDecoder GetDecoder(ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Bmp:
                    return new BmpDecoder();

                case ImageFormat.Png:
                    return new PngDecoder();

                case ImageFormat.Jpeg:
                    throw new NotImplementedException();

                default:
                    throw new InvalidOperationException("Image format not supported");
            }
        }

        /// <summary>
        /// Gets the <see cref="IImageEncoder" /> for the specified format.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <param name="encoderOptions">The image encoder options.</param>
        /// <returns>The <see cref="IImageEncoder" />.</returns>
        private static IImageEncoder GetEncoder(ImageFormat format, ImageEncoderOptions encoderOptions)
        {
            switch (format)
            {
                case ImageFormat.Bmp:
                    if (encoderOptions == null)
                    {
                        encoderOptions = new BmpEncoderOptions();
                    }

                    if (!(encoderOptions is BmpEncoderOptions))
                    {
                        throw new ArgumentException("encoderOptions");
                    }

                    return new BmpEncoder((BmpEncoderOptions)encoderOptions);

                case ImageFormat.Png:
                    if (encoderOptions == null)
                    {
                        encoderOptions = new PngEncoderOptions();
                    }

                    if (!(encoderOptions is PngEncoderOptions))
                    {
                        throw new ArgumentException("encoderOptions");
                    }

                    return new PngEncoder((PngEncoderOptions)encoderOptions);

                case ImageFormat.Jpeg:

                    throw new NotImplementedException();

                default:
                    throw new InvalidOperationException("Image format not supported");
            }
        }

        /// <summary>
        /// Gets the image format.
        /// </summary>
        /// <param name="bytes">The image bytes.</param>
        /// <returns>The <see cref="ImageFormat" /></returns>
        private static ImageFormat GetImageFormat(byte[] bytes)
        {
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xD8)
            {
                return ImageFormat.Jpeg;
            }

            if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D)
            {
                return ImageFormat.Bmp;
            }

            if (bytes.Length >= 4 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            {
                return ImageFormat.Png;
            }

            return ImageFormat.Unknown;
        }

        /// <summary>
        /// Gets the byte array from the specified stream.
        /// </summary>
        /// <param name="s">The stream.</param>
        /// <returns>A byte array.</returns>
        private static byte[] GetBytes(Stream s)
        {
            using (var ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Updates the image information.
        /// </summary>
        private void UpdateImageInfo()
        {
            var decoder = GetDecoder(this.Format);
            var info = decoder.GetImageInfo(this.data);

            if (info != null)
            {
                this.Width = info.Width;
                this.Height = info.Height;
                this.BitsPerPixel = info.BitsPerPixel;
                this.DpiX = info.DpiX;
                this.DpiY = info.DpiY;
            }
        }
    }
}