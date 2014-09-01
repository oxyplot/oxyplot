// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringBitReader.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a <see cref="BitReader" /> based on a <see cref="string" /> of bits.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides a <see cref="BitReader" /> based on a <see cref="string" /> of bits.
    /// </summary>
    public class StringBitReader : BitReader
    {
        /// <summary>
        /// The text
        /// </summary>
        private string text;

        /// <summary>
        /// The index
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBitReader" /> class.
        /// </summary>
        /// <param name="text">The source string.</param>
        public StringBitReader(string text)
        {
            if (!Regex.IsMatch(text, "[01]*"))
            {
                throw new ArgumentException("str");
            }

            this.text = text;
            this.index = 0;
        }

        /// <summary>
        /// Reads a byte from the stream.
        /// </summary>
        /// <returns>The byte.</returns>
        public override int Read()
        {
            if (this.index == this.text.Length)
            {
                return -1;
            }

            int result = this.text[this.index] - '0';
            this.index++;
            return result;
        }

        /// <summary>
        /// Reads a bit from the stream.
        /// </summary>
        /// <returns>Returns 0 or 1 if a bit is available, or throws an EOFException if the end of stream is reached.</returns>
        /// <exception cref="System.Exception">End of stream reached</exception>
        public override int ReadNoEof()
        {
            int result = this.Read();
            if (result != -1)
            {
                return result;
            }

            throw new Exception("End of stream reached");
        }

        /// <summary>
        /// Closes this stream and the underlying InputStream.
        /// </summary>
        public override void Close()
        {
            this.index = this.text.Length;
        }

        /// <summary>
        /// Returns the current bit position, which is between 0 and 7 inclusive. The number of bits remaining in the current byte is 8 minus this number.
        /// </summary>
        /// <returns>The bit position.</returns>
        public override int GetBitPosition()
        {
            return this.index % 8;
        }

        /// <summary>
        /// Discards the remainder of the current byte and reads the next byte from the stream.
        /// </summary>
        /// <returns>The byte.</returns>
        public override int ReadByte()
        {
            while (this.index % 8 != 0)
            {
                this.ReadNoEof();
            }

            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                result |= this.ReadNoEof() << i;
            }

            return result;
        }
    }
}