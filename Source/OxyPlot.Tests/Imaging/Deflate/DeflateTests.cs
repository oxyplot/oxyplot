// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeflateTests.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics;

    using NUnit.Framework;

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