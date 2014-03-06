// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BitReader.cs" company="OxyPlot">
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
//   Implements a binary reader that can read bits.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.IO;

    /// <summary>
    /// Implements a binary reader that can read bits.
    /// </summary>
    public abstract class BitReader
    {
        /// <summary>
        /// Reads a byte from the stream.
        /// </summary>
        /// <returns>The byte.</returns>
        public abstract int Read();

        /// <summary>
        /// Reads a bit from the stream. 
        /// </summary>
        /// <returns>Returns 0 or 1 if a bit is available, or throws an EOFException if the end of stream is reached.</returns>
        public abstract int ReadNoEof();

        /// <summary>
        /// Closes this stream and the underlying InputStream.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Returns the current bit position, which is between 0 and 7 inclusive. The number of bits remaining in the current byte is 8 minus this number.
        /// </summary>
        /// <returns>The bit position.</returns>
        public abstract int GetBitPosition();

        /// <summary>
        /// Discards the remainder of the current byte and reads the next byte from the stream.
        /// </summary>
        /// <returns>The byte.</returns>
        public abstract int ReadByte();

        /// <summary>
        /// Reads the specified number of bits.
        /// </summary>
        /// <param name="bits">The number of bits.</param>
        /// <returns>The bits.</returns>
        /// <exception cref="System.IO.IOException">Reading past EOF.</exception>
        public int ReadBits(int bits)
        {
            int r = 0;
            for (int i = 0; i < bits; i++)
            {
                var bit = this.Read();
                if (bit == -1)
                {
                    throw new IOException();
                }

                r += bit << i;
            }

            return r;
        }
    }
}