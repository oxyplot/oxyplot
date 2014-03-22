// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteBitReader.cs" company="OxyPlot">
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
//   The byte bit reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// The byte bit reader.
    /// </summary>
    public class ByteBitReader : BitReader
    {
        /// <summary>
        /// The input.
        /// </summary>
        private readonly BinaryReader input;

        /// <summary>
        /// The bit position.
        /// </summary>
        /// <remarks>
        /// Either in the range 0x00 to 0xFF, or -1 if the end of stream is reached
        /// </remarks>
        private int bitPosition;

        /// <summary>
        /// The is end of stream.
        /// </summary>
        /// <remarks>
        /// Always between 1 and 8, inclusive
        /// </remarks>
        private bool isEndOfStream;

        /// <summary>
        /// The next bits.
        /// </summary>
        /// <remarks>
        /// Underlying byte stream to read from
        /// </remarks>
        private int nextBits;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteBitReader"/> class.
        /// </summary>
        /// <param name="s">
        /// The arguments.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Argument is null
        /// </exception>
        public ByteBitReader(Stream s)
        {
            if (s == null)
            {
                throw new ArgumentException("Argument is null");
            }

            this.input = new BinaryReader(s);
            this.bitPosition = 8;
            this.isEndOfStream = false;
        }

        /// <summary>
        /// Reads a bit from the stream. Returns 0 or 1 if a bit is available, or -1 if the end of stream is reached. The end of stream always occurs on a byte boundary.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int Read()
        {
            if (this.isEndOfStream)
            {
                return -1;
            }

            if (this.bitPosition == 8)
            {
                this.nextBits = this.input.ReadByte();
                if (this.nextBits == -1)
                {
                    this.isEndOfStream = true;
                    return -1;
                }

                this.bitPosition = 0;
            }

            // TODO: should be >>> ?
            var result = (this.nextBits >> this.bitPosition) & 1;
            this.bitPosition++;
            return result;
        }

        /// <summary>
        /// Reads a bit from the stream. Returns 0 or 1 if a bit is available, or throws an EOFException if the end of stream is reached.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int ReadNoEof()
        {
            var result = this.Read();
            if (result != -1)
            {
                return result;
            }

            throw new Exception("End of stream reached");
        }

        /// <summary>
        /// Gets the bit position.
        /// </summary>
        /// Returns the current bit position, which is between 0 and 7 inclusive. The number of bits remaining in the current byte is 8 minus this number.
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetBitPosition()
        {
            return this.bitPosition % 8;
        }

        /// <summary>
        /// Discards the remainder of the current byte and reads the next byte from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int ReadByte()
        {
            this.bitPosition = 8;
            return this.input.ReadByte();
        }

        /// <summary>
        /// Closes this stream and the underlying InputStream.
        /// </summary>
        public override void Close()
        {
            // input.Close();
        }
    }
}