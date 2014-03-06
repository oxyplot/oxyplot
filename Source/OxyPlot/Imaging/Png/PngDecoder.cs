// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngDecoder.cs" company="OxyPlot">
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
//   Implements support for decoding png images.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// Implements support for decoding png images.
    /// </summary>
    public class PngDecoder : IImageDecoder
    {
        /// <summary>
        /// Gets information about the image in the specified byte array.
        /// </summary>
        /// <param name="bytes">The image data.</param>
        /// <returns>
        /// An <see cref="OxyImageInfo" /> structure.
        /// </returns>
        /// <exception cref="System.FormatException">Wrong length of pHYs chunk.</exception>
        public OxyImageInfo GetImageInfo(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            var inputReader = new BinaryReader(ms);
            // ReSharper disable UnusedVariable
            var signature = inputReader.ReadBytes(8);
            var headerLength = inputReader.ReadBigEndianUInt32();
            var headerType = inputReader.ReadString(4);
            var width = (int)inputReader.ReadBigEndianUInt32();
            var height = (int)inputReader.ReadBigEndianUInt32();
            var bitDepth = inputReader.ReadByte();
            var colorType = (ColorType)inputReader.ReadByte();
            var compressionMethod = (CompressionMethod)inputReader.ReadByte();
            var filterMethod = (FilterMethod)inputReader.ReadByte();
            var interlaceMethod = (InterlaceMethod)inputReader.ReadByte();
            // ReSharper disable once InconsistentNaming
            var headerCRC = inputReader.ReadBigEndianUInt32();

            // ReSharper restore UnusedVariable            
            double dpix = 96;
            double dpiy = 96;
            while (true)
            {
                var length = (int)inputReader.ReadBigEndianUInt32();
                var type = inputReader.ReadString(4);
                if (type == "IEND")
                {
                    break;
                }

                switch (type)
                {
                    case "pHYs":
                        {
                            if (length != 9)
                            {
                                throw new FormatException("Wrong length of pHYs chunk.");
                            }

                            var ppux = inputReader.ReadBigEndianUInt32();
                            var ppuy = inputReader.ReadBigEndianUInt32();
                            // ReSharper disable once UnusedVariable
                            var unit = inputReader.ReadByte();
                            dpix = ppux * 0.0254;
                            dpiy = ppuy * 0.0254;
                            break;
                        }

                    default:
                        {
                            ms.Position += length;
                            break;
                        }
                }

                // Read CRC
                inputReader.ReadBigEndianUInt32();
            }

            return new OxyImageInfo { Width = width, Height = height, DpiX = dpix, DpiY = dpiy, BitsPerPixel = bitDepth };
        }

        /// <summary>
        /// Decodes an image from the specified stream.
        /// </summary>
        /// <param name="s">The stream.</param>
        /// <returns>
        /// The 32-bit pixel data, indexed as [x,y].
        /// </returns>
        public OxyColor[,] Decode(Stream s)
        {
            // http://www.w3.org/TR/PNG/
            // http://en.wikipedia.org/wiki/Portable_Network_Graphics
            var inputReader = new BinaryReader(s);
            var signature = inputReader.ReadBytes(8);
            if (signature[0] != 0x89 || signature[1] != 0x50 || signature[2] != 0x4E || signature[3] != 0x47
                || signature[4] != 0x0D || signature[5] != 0x0A || signature[6] != 0x1A || signature[7] != 0x0A)
            {
                throw new FormatException("Invalid signature.");
            }

            var headerLength = inputReader.ReadBigEndianUInt32();
            if (headerLength != 13)
            {
                throw new FormatException("Header not supported.");
            }

            var headerType = inputReader.ReadString(4);
            if (headerType != "IHDR")
            {
                throw new FormatException("Invalid header.");
            }

            var width = (int)inputReader.ReadBigEndianUInt32();
            var height = (int)inputReader.ReadBigEndianUInt32();
            var bitDepth = inputReader.ReadByte();
            var colorType = (ColorType)inputReader.ReadByte();
            var compressionMethod = (CompressionMethod)inputReader.ReadByte();
            var filterMethod = (FilterMethod)inputReader.ReadByte();
            var interlaceMethod = (InterlaceMethod)inputReader.ReadByte();
            // ReSharper disable once UnusedVariable
            // ReSharper disable once InconsistentNaming
            var headerCRC = inputReader.ReadBigEndianUInt32();
            // ReSharper disable once NotAccessedVariable
            double dpix = 96;
            // ReSharper disable once NotAccessedVariable
            double dpiy = 96;

            if (bitDepth != 8)
            {
                throw new NotImplementedException();
            }

            if (colorType != ColorType.TrueColorWithAlpha)
            {
                throw new NotImplementedException();
            }

            if (compressionMethod != CompressionMethod.Deflate)
            {
                throw new NotImplementedException();
            }

            if (filterMethod != FilterMethod.None)
            {
                throw new NotImplementedException();
            }

            if (interlaceMethod != InterlaceMethod.None)
            {
                throw new NotImplementedException();
            }

            var ms = new MemoryStream();
            while (true)
            {
                var length = (int)inputReader.ReadBigEndianUInt32();
                var type = inputReader.ReadString(4);
                if (type == "IEND")
                {
                    break;
                }

                switch (type)
                {
                    case "PLTE":
                        throw new NotImplementedException();

                    case "IDAT":
                        {
                            // ReSharper disable UnusedVariable
                            var method = inputReader.ReadByte();
                            var check = inputReader.ReadByte();
                            // ReSharper restore UnusedVariable
                            var bytes = inputReader.ReadBytes(length - 6);
                            var expectedCheckSum = inputReader.ReadBigEndianUInt32();

                            var deflatedBytes = Deflate(bytes);
                            var actualCheckSum = PngEncoder.Adler32(deflatedBytes);

                            if (actualCheckSum != expectedCheckSum)
                            {
                                throw new FormatException("Invalid checksum.");
                            }

                            ms.Write(deflatedBytes, 0, deflatedBytes.Length);
                            break;
                        }

                    case "pHYs":
                        {
                            if (length != 9)
                            {
                                throw new FormatException("Wrong length of pHYs chunk.");
                            }

                            var ppux = inputReader.ReadBigEndianUInt32();
                            var ppuy = inputReader.ReadBigEndianUInt32();
                            // ReSharper disable once UnusedVariable
                            var unit = inputReader.ReadByte();
                            // ReSharper disable once RedundantAssignment
                            dpix = ppux * 0.0254;
                            // ReSharper disable once RedundantAssignment
                            dpiy = ppuy * 0.0254;
                            break;
                        }

                    default:
                        {
                            // ReSharper disable once UnusedVariable
                            var bytes = inputReader.ReadBytes(length);
                            break;
                        }
                }

                // ReSharper disable once UnusedVariable
                var crc = inputReader.ReadBigEndianUInt32();
            }

            var pixels = new OxyColor[width, height];
            ms.Position = 0;
            for (int y = height - 1; y >= 0; y--)
            {
                ms.ReadByte();
                for (int x = 0; x < width; x++)
                {
                    var red = (byte)ms.ReadByte();
                    var green = (byte)ms.ReadByte();
                    var blue = (byte)ms.ReadByte();
                    var alpha = (byte)ms.ReadByte();

                    pixels[x, y] = OxyColor.FromArgb(alpha, red, green, blue);
                }
            }

            /*
            var bitReader = new ByteBitReader(ms);
            for (int y = 0; y < height; y++)
            {
                var filterType = bitReader.readByte();
                for (int x = 0; x < width; x++)
                {
                    var red = (byte)bitReader.ReadBits(bitDepth);
                    var green = (byte)bitReader.ReadBits(bitDepth);
                    var blue = (byte)bitReader.ReadBits(bitDepth);
                    var alpha = (byte)bitReader.ReadBits(bitDepth);

                    pixels[x, y] = OxyColor.FromArgb(alpha, red, green, blue);
                }
            }*/

            if (ms.Position != ms.Length)
            {
                throw new InvalidOperationException();
            }

            return pixels;
        }

        /// <summary>
        /// Deflates the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The deflated bytes.</returns>
        private static byte[] Deflate(byte[] bytes)
        {
            // http://en.wikipedia.org/wiki/DEFLATE
#if NET
            var inputStream = new MemoryStream(bytes);
            var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress);
            var buffer = new byte[4096];
            var outputStream = new MemoryStream();
            while (true)
            {
                var l = deflateStream.Read(buffer, 0, buffer.Length);
                outputStream.Write(buffer, 0, l);

                if (l < buffer.Length)
                {
                    break;
                }
            }

            return outputStream.ToArray();
#elif BCL_COMPRESSION
            // TODO: https://www.nuget.org/packages/Microsoft.Bcl.Compression/
#else
            return OxyPlot.Deflate.Decompress(bytes);
#endif
        }
    }
}