// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NayukiDeflateTests.cs" company="OxyPlot">
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
//   Unit tests based on <a href="https://github.com/nayuki/DEFLATE/blob/master/test/nayuki/deflate/DecompressorTest.java">DecompressorTest.java</a>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests based on <a href="https://github.com/nayuki/DEFLATE/blob/master/test/nayuki/deflate/DecompressorTest.java">DecompressorTest.java</a>.
    /// </summary>
    [TestFixture]
    public class NayukiDeflateTests
    {
        /* Test cases */

        [Test, ExpectedException]
        public void testReservedBlockType()
        {
            // Reserved block type
            test("1 11 00000", "");
        }


        [Test, ExpectedException]
        public void testEofInBlockType()
        {
            // Partial block type
            test("1 0", "");
        }


        [Test]
        public void testUncompressedEmpty()
        {
            // Uncompressed block len=0: (empty)
            test("1 00 00000   0000000000000000 1111111111111111", "");
        }


        [Test]
        public void testUncompressedThreeBytes()
        {
            // Uncompressed block len=3: 05 14 23
            test("1 00 00000   1100000000000000 0011111111111111   10100000 00101000 11000100", "05 14 23");
        }


        [Test, Ignore]
        public void testUncompressedTwoBlocks()
        {
            // Uncompressed block len=1: 05
            // Uncompressed block len=2: 14 23
            test("0 00 00000   0100000000000000 1011111111111111   10100000 00101000   1 00 00000   1000000000000000 0111111111111111   11000100", "05 14 23");
        }


        [Test, ExpectedException, Ignore]
        public void testUncompressedEofBeforeLength()
        {
            // Uncompressed block (partial padding) (no length)
            test("1 00 000", "");
        }


        [Test, ExpectedException]
        public void testUncompressedEofInLength()
        {
            // Uncompressed block (partial length)
            test("1 00 00000 0000000000", "");
        }


        [Test, ExpectedException]
        public void testUncompressedMismatchedLength()
        {
            // Uncompressed block (mismatched len and nlen)
            test("1 00 00000 0010000000010000 1111100100110101", "");
        }


        [Test, ExpectedException, Ignore]
        public void testUncompressedBlockNoFinalBlock()
        {
            // Uncompressed block len=0: (empty)
            // No final block
            test("0 00 00000   0000000000000000 1111111111111111", "");
        }


        [Test]
        public void testFixedHuffmanEmpty()
        {
            // Fixed Huffman block: End
            test("1 10 0000000", "");
        }


        [Test]
        public void testFixedHuffmanLiterals()
        {
            // Fixed Huffman block: 00 80 8F 90 C0 FF End
            test("1 10 00110000 10110000 10111111 110010000 111000000 111111111 0000000", "00 80 8F 90 C0 FF");
        }


        [Test]
        public void testFixedHuffmanNonOverlappingRun()
        {
            // Fixed Huffman block: 00 01 02 (3,3) End
            test("1 10 00110000 00110001 00110010 0000001 00010 0000000", "00 01 02 00 01 02");
        }


        [Test]
        public void testFixedHuffmanOverlappingRun0()
        {
            // Fixed Huffman block: 01 (1,4) End
            test("1 10 00110001 0000010 00000 0000000", "01 01 01 01 01");
        }


        [Test]
        public void testFixedHuffmanOverlappingRun1()
        {
            // Fixed Huffman block: 8E 8F (2,5) End
            test("1 10 10111110 10111111 0000011 00001 0000000", "8E 8F 8E 8F 8E 8F 8E");
        }


        [Test, ExpectedException]
        public void testFixedHuffmanInvalidLengthCode286()
        {
            // Fixed Huffman block: #286
            test("1 10 11000110", "");
        }


        [Test, ExpectedException]
        public void testFixedHuffmanInvalidLengthCode287()
        {
            // Fixed Huffman block: #287
            test("1 10 11000111", "");
        }


        [Test, ExpectedException]
        public void testFixedHuffmanInvalidDistanceCode30()
        {
            // Fixed Huffman block: 00 #257 #30
            test("1 10 00110000 0000001 11110", "");
        }


        [Test, ExpectedException]
        public void testFixedHuffmanInvalidDistanceCode31()
        {
            // Fixed Huffman block: 00 #257 #31
            test("1 10 00110000 0000001 11111", "");
        }


        [Test]
        public void testDynamicHuffmanEmpty()
        {
            // Dynamic Huffman block:
            //   numCodeLen=19
            //     codeLenCodeLen = 0:0, 1:1, 2:0, ..., 15:0, 16:0, 17:0, 18:1
            //   numLitLen=257, numDist=2
            //     litLenCodeLen = 0:1, 1:0, ..., 255:0, 256:1
            //     distCodeLen = 0:1, 1:1
            //   Data: End
            String blockHeader = "1 01";
            String codeCounts = "00000 10000 1111";
            String codeLenCodeLens = "000 000 100 000 000 000 000 000 000 000 000 000 000 000 000 000 000 100 000";
            String codeLens = "0 11111111 10101011 0 0 0";
            String data = "1";
            test(blockHeader + codeCounts + codeLenCodeLens + codeLens + data, "");
        }


        [Test]
        public void testDynamicHuffmanEmptyNoDistanceCode()
        {
            // Dynamic Huffman block:
            //   numCodeLen=18
            //     codeLenCodeLen = 0:2, 1:2, 2:0, ..., 15:0, 16:0, 17:0, 18:1
            //   numLitLen=257, numDist=1
            //     litLenCodeLen = 0:0, ..., 254:0, 255:1, 256:1
            //     distCodeLen = 0:0
            //   Data: End
            String blockHeader = "1 01";
            String codeCounts = "00000 00000 0111";
            String codeLenCodeLens = "000 000 100 010 000 000 000 000 000 000 000 000 000 000 000 000 000 010";
            String codeLens = "01111111 00101011 11 11 10";
            String data = "1";
            test(blockHeader + codeCounts + codeLenCodeLens + codeLens + data, "");
        }


        [Test, ExpectedException]
        public void testDynamicHuffmanCodeLengthRepeatAtStart()
        {
            // Dynamic Huffman block:
            //   numLitLen=257, numDist=1, numCodeLen=18
            //   codeLenCodeLen = 0:0, 1:1, 2:0, ..., 15:0, 16:1, 17:0, 18:0
            //   Literal/length/distance code lengths: #16+00
            String blockHeader = "1 01";
            String codeCounts = "00000 00000 0111";
            String codeLenCodeLens = "100 000 000 000 000 000 000 000 000 000 000 000 000 000 000 000 000 100";
            String codeLens = "1";
            test(blockHeader + codeCounts + codeLenCodeLens + codeLens, "");
        }


        [Test, ExpectedException]
        public void testDynamicHuffmanTooManyCodeLengthItems()
        {
            // Dynamic Huffman block:
            //   numLitLen=257, numDist=1, numCodeLen=18
            //   codeLenCodeLen = 0:0, 1:1, 2:0, ..., 15:0, 16:0, 17:0, 18:1
            //   Literal/length/distance code lengths: 1 1 #18+1111111 #18+1101100
            String blockHeader = "1 01";
            String codeCounts = "00000 00000 0111";
            String codeLenCodeLens = "000 000 100 000 000 000 000 000 000 000 000 000 000 000 000 000 000 100";
            String codeLens = "0 0 11111111 10011011";
            test(blockHeader + codeCounts + codeLenCodeLens + codeLens, "");
        }



        /* Utility method */

        // 'input' is a string of 0's and 1's (with optional spaces) representing the input bit sequence.
        // 'refOutput' is a string of pairs of hexadecimal digits (with optional spaces) representing
        // the expected decompressed output byte sequence.
        private static void test(String input, string refOutput)
        {
            refOutput = refOutput.Replace(" ", "");
            if (refOutput.Length % 2 != 0)
                throw new ArgumentException();
            byte[] refOut = new byte[refOutput.Length / 2];
            for (int i = 0; i < refOut.Length; i++)
                refOut[i] = (byte)int.Parse(refOutput.Substring(i * 2, 2), NumberStyles.HexNumber);

            input = input.Replace(" ", "");
            var inputStream = new StringBitReader(input);
            byte[] actualOut = Deflate.Decompress(inputStream);
            AssertArrayEquals(refOut, actualOut);
        }

        private static void AssertArrayEquals<T>(T[] refOut, T[] actualOut)
        {
            Assert.AreEqual(refOut.Length, actualOut.Length);
            for (int i = 0; i < refOut.Length; i++)
            {
                Assert.AreEqual(refOut[i], actualOut[i]);
            }
        }
    }
}