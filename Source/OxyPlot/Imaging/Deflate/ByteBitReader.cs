// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteBitReader.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        /// <remarks>Either in the range 0x00 to 0xFF, or -1 if the end of stream is reached</remarks>
        private int bitPosition;

        /// <summary>
        /// The is end of stream.
        /// </summary>
        /// <remarks>Always between 1 and 8, inclusive</remarks>
        private bool isEndOfStream;

        /// <summary>
        /// The next bits.
        /// </summary>
        /// <remarks>Underlying byte stream to read from</remarks>
        private int nextBits;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteBitReader" /> class.
        /// </summary>
        /// <param name="s">The arguments.</param>
        /// <exception cref="System.ArgumentException">Argument is <c>null</c></exception>
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
        /// <returns>The <see cref="int" />.</returns>
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
        /// <returns>The <see cref="int" />.</returns>
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
        /// <returns>The <see cref="int" />.</returns>
        public override int GetBitPosition()
        {
            return this.bitPosition % 8;
        }

        /// <summary>
        /// Discards the remainder of the current byte and reads the next byte from the stream.
        /// </summary>
        /// <returns>The <see cref="int" />.</returns>
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