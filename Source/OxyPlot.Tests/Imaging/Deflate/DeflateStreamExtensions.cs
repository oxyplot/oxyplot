// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeflateStreamExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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