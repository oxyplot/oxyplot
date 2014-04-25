// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeflateStreamExtensions.cs" company="OxyPlot">
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
//   Provides extension methods for <see cref="DeflateStream" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// Provides extension methods for <see cref="DeflateStream" />.
    /// </summary>
    public static class DeflateStreamExtensions
    {
        /// <summary>
        /// Compresses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The compressed data.</returns>
        public static byte[] Compress(this byte[] input)
        {
            var inputStream = new MemoryStream(input);
            var outputStream = new MemoryStream();
            var compressor = new DeflateStream(outputStream, CompressionMode.Compress);
            inputStream.CopyTo(compressor);
            compressor.Close();
            return outputStream.ToArray();
        }

        /// <summary>
        /// Decompresses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] Decompress(this byte[] input)
        {
            var inflatedStream = new MemoryStream(input);
            var outputStream = new MemoryStream();
            var decompressor = new DeflateStream(inflatedStream, CompressionMode.Decompress);
            decompressor.CopyTo(outputStream);
            decompressor.Close();
            return outputStream.ToArray();
        }
    }
}