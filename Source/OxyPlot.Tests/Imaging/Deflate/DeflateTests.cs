// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeflateTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class DeflateTests
    {
        [Test]
        public void Deflate_InflateAndDeflate_ShouldEqualOriginal()
        {
            for (int n = 100; n < 20000; n += 100)
            {
                for (int seed = 0; seed < 3; seed++)
                {
                    var input = new byte[n];
                    for (int i = 0; i < n; i++)
                    {
                        input[i] = (byte)(Math.Sin(i) * 255);
                    }

                    // Compress with System.IO.Compression
                    var compressedData = input.Compress();

                    // Decompress with System.IO.Compression
                    var w1 = Stopwatch.StartNew();
                    var deflated1 = compressedData.Decompress();
                    var t1 = w1.ElapsedTicks;
                    AssertArrayEquals(deflated1, input);

                    // Decompress with local Deflate implementation
                    var w2 = Stopwatch.StartNew();
                    var deflated2 = Deflate.Decompress(compressedData);
                    var t2 = w2.ElapsedTicks;
                    AssertArrayEquals(deflated2, input);

                    Console.WriteLine("{0}:{1}/{2}", n, t1, t2);
                }
            }
        }

        private static void AssertArrayEquals<T>(T[] refOut, T[] actualOut)
        {
            Assert.AreEqual(refOut.Length, actualOut.Length, "Different length");
            for (int i = 0; i < refOut.Length; i++)
            {
                Assert.AreEqual(refOut[i], actualOut[i], "Different at byte " + i);
            }
        }
    }
}