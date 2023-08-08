// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeflateTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;
    using System;
    using System.Diagnostics.CodeAnalysis;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class DeflateTests
    {
        [Test]
        public void Deflate_InflateAndDeflate_ShouldEqualOriginal()
        {
            for (var n = 100; n < 20000; n += 100)
            {
                for (var seed = 0; seed < 3; seed++)
                {
                    var input = new byte[n];
                    for (var i = 0; i < n; i++)
                    {
                        input[i] = (byte)(Math.Sin(i) * 255);
                    }

                    // Compress with System.IO.Compression
                    var compressedData = input.Compress();

                    // Decompress with System.IO.Compression
                    var deflated1 = compressedData.Decompress();
                    AssertArrayEquals(deflated1, input);

                    // Decompress with local Deflate implementation
                    var deflated2 = Deflate.Decompress(compressedData);
                    AssertArrayEquals(deflated2, input);
                }
            }
        }

        private static void AssertArrayEquals<T>(T[] refOut, T[] actualOut)
        {
            Assert.AreEqual(refOut.Length, actualOut.Length, "Different length");
            for (var i = 0; i < refOut.Length; i++)
            {
                Assert.AreEqual(refOut[i], actualOut[i], "Different at byte " + i);
            }
        }
    }
}
