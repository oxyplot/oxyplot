// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bitmap.cs" company="OxyPlot">
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
//   Gets the size of the BMP file in bytes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IcoMaker
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public void Init()
        {
            biSize = (uint)Marshal.SizeOf(this);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }

    public class Bitmap
    {
        public ushort Type { get; set; }

        /// <summary>
        /// Gets the size of the BMP file in bytes.
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// reserved; actual value depends on the application that creates the image
        /// </summary>
        public ushort Reserved1 { get; set; }

        /// <summary>
        /// reserved; actual value depends on the application that creates the image
        /// </summary>
        public ushort Reserved2 { get; set; }

        /// <summary>
        /// the offset, i.e. starting address, of the byte where the bitmap image data (pixel array) can be found.
        /// </summary>
        public uint OffBits { get; set; }
        public int Width { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// Unless BITMAPCOREHEADER is used, uncompressed Windows bitmaps also can be stored from the top to bottom, when the Image Height value is negative.
        /// </remarks>
        public int Height { get; private set; }

        public ushort Planes { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// Contemporary DIB Headers allow pixel formats with 1, 2, 4, 8, 16, 24 and 32 bits per pixel (bpp).
        /// </remarks>
        public ushort BitCount { get; private set; }
        public uint Compression { get; private set; }
        public uint SizeImage { get; private set; }
        public int XPelsPerMeter { get; private set; }
        public int YPelsPerMeter { get; private set; }
        public uint ColorsUsed { get; private set; }
        public uint ColorsImportant { get; private set; }
        private RGBQUAD[] Colors;

        private byte[] PixelArray;

        public static Bitmap Load(Stream s, bool excludeFileHeader = false)
        {
            var r = new BinaryReader(s);
            var b = new Bitmap();
            if (!excludeFileHeader)
            {
                // File header (14bytes)
                b.Type = r.ReadUInt16();
                b.Size = r.ReadUInt32();
                b.Reserved1 = r.ReadUInt16();
                b.Reserved2 = r.ReadUInt16();
                b.OffBits = r.ReadUInt32();
            }

            var infoHeaderOffset = excludeFileHeader ? 0 : 14;
            var infoHeaderSize = r.ReadUInt32();
            switch (infoHeaderSize)
            {
                case 40:
                    // BITMAPINFOHEADER (40bytes)
                    b.Width = r.ReadInt32();
                    b.Height = r.ReadInt32();
                    b.Planes = r.ReadUInt16();
                    b.BitCount = r.ReadUInt16();
                    b.Compression = r.ReadUInt32();
                    b.SizeImage = r.ReadUInt32();
                    b.XPelsPerMeter = r.ReadInt32();
                    b.YPelsPerMeter = r.ReadInt32();
                    b.ColorsUsed = r.ReadUInt32();
                    b.ColorsImportant = r.ReadUInt32();
                    break;
                default:
                    throw new FormatException("Bitmap format not supported (Size of DIB header=" + infoHeaderSize + ")");
            }

            var colorTableOffset = infoHeaderOffset + b.SizeImage;
            s.Position = colorTableOffset;
            int numberOfColors = 1 << b.BitCount;
            b.Colors = new RGBQUAD[numberOfColors];
            for (int i = 0; i < numberOfColors; i++)
            {
                b.Colors[i].rgbBlue = r.ReadByte();
                b.Colors[i].rgbGreen = r.ReadByte();
                b.Colors[i].rgbRed = r.ReadByte();
                b.Colors[i].rgbReserved = r.ReadByte();
            }

            int rowSize = ((b.BitCount * b.Width + 31) / 32) * 4;
            int pixelArraySize = rowSize * Math.Abs(b.Height);
            b.PixelArray = r.ReadBytes(pixelArraySize);
            return b;
        }

        public void Save(Stream s, bool excludeFileHeader = false)
        {
            var r = new BinaryWriter(s);
            if (!excludeFileHeader)
            {
                r.Write(this.Type);
                r.Write(this.Size);
                r.Write(this.Reserved1);
                r.Write(this.Reserved2);
                r.Write(this.OffBits);
            }

            var infoHeaderOffset = excludeFileHeader ? 0 : 14;
            uint infoHeaderSize = 40;
            r.Write(infoHeaderSize);
            r.Write(this.Width);
            r.Write(this.Height);
            r.Write(this.Planes);
            r.Write(this.BitCount);
            r.Write(this.Compression);
            r.Write(this.SizeImage);
            r.Write(this.XPelsPerMeter);
            r.Write(this.YPelsPerMeter);
            r.Write(this.ColorsUsed);
            r.Write(this.ColorsImportant);

            int numberOfColors = 1 << this.BitCount;
            for (int i = 0; i < numberOfColors; i++)
            {
                r.Write(this.Colors[i].rgbBlue);
                r.Write(this.Colors[i].rgbGreen);
                r.Write(this.Colors[i].rgbRed);
                r.Write(this.Colors[i].rgbReserved);
            }

            int rowSize = ((this.BitCount * this.Width + 31) / 32) * 4;
            int pixelArraySize = rowSize * Math.Abs(this.Height);
            r.Write(this.PixelArray);
        }

        public static Bitmap LoadFromFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return Load(stream);
            }
        }
    }
}