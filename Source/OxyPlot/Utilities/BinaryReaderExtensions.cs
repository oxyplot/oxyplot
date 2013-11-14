// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="OxyPlot">
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
//   Provides extension methods to the <see cref="BinaryReader" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Provides extension methods to the <see cref="BinaryReader" />.
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a string of the specified length (in bytes).
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <param name="length">The length.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The string.</returns>
        public static string ReadString(this BinaryReader r, int length, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetString(r.ReadBytes(length), 0, length);
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <param name="isLittleEndian">Read as little endian (Intel convention) if set to <c>true</c>.</param>
        /// <returns>The unsigned integer.</returns>
        [CLSCompliant(false)]
        public static uint ReadUInt32(this BinaryReader r, bool isLittleEndian)
        {
            return isLittleEndian ? r.ReadUInt32() : r.ReadBigEndianUInt32();
        }

        /// <summary>
        /// Reads a signed 32-bit integer.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <param name="isLittleEndian">Read as little endian (Intel convention) if set to <c>true</c>.</param>
        /// <returns>The signed integer.</returns>
        public static int ReadInt32(this BinaryReader r, bool isLittleEndian)
        {
            return isLittleEndian ? r.ReadInt32() : r.ReadBigEndianInt32();
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <param name="isLittleEndian">Read as little endian (Intel convention) if set to <c>true</c>.</param>
        /// <returns>The unsigned integer.</returns>
        [CLSCompliant(false)]
        public static ushort ReadUInt16(this BinaryReader r, bool isLittleEndian)
        {
            return isLittleEndian ? r.ReadUInt16() : r.ReadBigEndianUInt16();
        }

        /// <summary>
        /// Reads an 64-bit floating point value.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <param name="isLittleEndian">Read as little endian (Intel convention) if set to <c>true</c>.</param>
        /// <returns>The floating point number.</returns>        
        public static double ReadDouble(this BinaryReader r, bool isLittleEndian)
        {
            return isLittleEndian ? r.ReadDouble() : r.ReadBigEndianDouble();
        }

        /// <summary>
        /// Reads an array of unsigned 32-bit integers.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="isLittleEndian">Read as little endian (Intel convention) if set to <c>true</c>.</param>
        /// <returns>
        /// The unsigned integer array.
        /// </returns>
        [CLSCompliant(false)]
        public static uint[] ReadUInt32Array(this BinaryReader r, int count, bool isLittleEndian)
        {
            var result = new uint[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = isLittleEndian ? r.ReadUInt32() : r.ReadBigEndianUInt32();
            }

            return result;
        }

        /// <summary>
        /// Reads an array of unsigned 16-bit integers.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <param name="count">The number of values to read.</param>
        /// <param name="isLittleEndian">Read as little endian (Intel convention) if set to <c>true</c>.</param>
        /// <returns>
        /// The unsigned integer array.
        /// </returns>
        [CLSCompliant(false)]
        public static ushort[] ReadUInt16Array(this BinaryReader r, int count, bool isLittleEndian)
        {
            var result = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = isLittleEndian ? r.ReadUInt16() : r.ReadBigEndianUInt16();
            }

            return result;
        }

        /// <summary>
        /// Reads a big endian (Motorola convention) unsigned 32-bit integer.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <returns>The unsigned integer.</returns>
        [CLSCompliant(false)]
        public static uint ReadBigEndianUInt32(this BinaryReader r)
        {
            byte[] a32 = r.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToUInt32(a32, 0);
        }

        /// <summary>
        /// Reads a big endian (Motorola convention) signed 32-bit integer.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <returns>The signed integer.</returns>
        public static int ReadBigEndianInt32(this BinaryReader r)
        {
            byte[] a32 = r.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToInt32(a32, 0);
        }

        /// <summary>
        /// Reads a big endian (Motorola convention) unsigned 16-bit integer.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <returns>The unsigned integer.</returns>
        [CLSCompliant(false)]
        public static ushort ReadBigEndianUInt16(this BinaryReader r)
        {
            byte[] a16 = r.ReadBytes(2);
            Array.Reverse(a16);
            return BitConverter.ToUInt16(a16, 0);
        }

        /// <summary>
        /// Reads a big endian (Motorola convention) 64-bit floating point number.
        /// </summary>
        /// <param name="r">The reader.</param>
        /// <returns>A <see cref="double"/>.</returns>
        public static double ReadBigEndianDouble(this BinaryReader r)
        {
            byte[] a = r.ReadBytes(8);
            Array.Reverse(a);
            return BitConverter.ToDouble(a, 0);
        }
    }
}