// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NayukiDeflateTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Unit tests based on Nayjuki's <a href="https://github.com/nayuki/DEFLATE/blob/master/test/nayuki/deflate/DecompressorTest.java">DEFLATE</a> project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests based on Nayjuki's <a href="https://github.com/nayuki/DEFLATE/blob/master/test/nayuki/deflate/DecompressorTest.java">DEFLATE</a> project.
    /// </summary>
    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class NayukiDeflateTests
    {
        /* Test cases */

        [Test]
        public void testReservedBlockType()
        {
            // Reserved block type
            Assert.Throws<FormatException>(() => test("1 11 00000", string.Empty));
        }

        [Test]
        public void testEofInBlockType()
        {
            // Partial block type
            Assert.Throws<Exception>(() => test("1 0", string.Empty));
        }

        [Test]
        public void testUncompressedEmpty()
        {
            // Uncompressed block len=0: (empty)
            test("1 00 00000   0000000000000000 1111111111111111", string.Empty);
        }

        [Test]
        public void testUncompressedThreeBytes()
        {
            // Uncompressed block len=3: 05 14 23
            test("1 00 00000   1100000000000000 0011111111111111   10100000 00101000 11000100", "05 14 23");
        }

        [Test, Ignore("")] // TODO: add ignore reason.
        public void testUncompressedTwoBlocks()
        {
            // Uncompressed block len=1: 05
            // Uncompressed block len=2: 14 23
            test("0 00 00000   0100000000000000 1011111111111111   10100000 00101000   1 00 00000   1000000000000000 0111111111111111   11000100", "05 14 23");
        }

        [Test, Ignore("")] // TODO: add ignore reason.
        public void testUncompressedEofBeforeLength()
        {
            // Uncompressed block (partial padding) (no length)
            Assert.Throws<FormatException>(() => test("1 00 000", string.Empty));
        }

        [Test]
        public void testUncompressedEofInLength()
        {
            // Uncompressed block (partial length)
            Assert.Throws<Exception>(() => test("1 00 00000 0000000000", string.Empty));
        }

        [Test]
        public void testUncompressedMismatchedLength()
        {
            // Uncompressed block (mismatched len and nlen)
            Assert.Throws<FormatException>(() => test("1 00 00000 0010000000010000 1111100100110101", string.Empty));
        }

        [Test, Ignore("")] // TODO: add ignore reason.
        public void testUncompressedBlockNoFinalBlock()
        {
            // Uncompressed block len=0: (empty)
            // No final block
            Assert.Throws<FormatException>(() => test("0 00 00000   0000000000000000 1111111111111111", string.Empty));
        }

        [Test]
        public void testFixedHuffmanEmpty()
        {
            // Fixed Huffman block: End
            test("1 10 0000000", string.Empty);
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

        [Test]
        public void testFixedHuffmanInvalidLengthCode286()
        {
            // Fixed Huffman block: #286
            Assert.Throws<FormatException>(() => test("1 10 11000110", string.Empty));
        }

        [Test]
        public void testFixedHuffmanInvalidLengthCode287()
        {
            // Fixed Huffman block: #287
            Assert.Throws<FormatException>(() => test("1 10 11000111", string.Empty));
        }

        [Test]
        public void testFixedHuffmanInvalidDistanceCode30()
        {
            // Fixed Huffman block: 00 #257 #30
            Assert.Throws<FormatException>(() => test("1 10 00110000 0000001 11110", string.Empty));
        }

        [Test]
        public void testFixedHuffmanInvalidDistanceCode31()
        {
            // Fixed Huffman block: 00 #257 #31
            Assert.Throws<FormatException>(() => test("1 10 00110000 0000001 11111", string.Empty));
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
            var blockHeader = "1 01";
            var codeCounts = "00000 10000 1111";
            var codeLenCodeLens = "000 000 100 000 000 000 000 000 000 000 000 000 000 000 000 000 000 100 000";
            var codeLens = "0 11111111 10101011 0 0 0";
            var data = "1";
            test(blockHeader + codeCounts + codeLenCodeLens + codeLens + data, string.Empty);
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
            var blockHeader = "1 01";
            var codeCounts = "00000 00000 0111";
            var codeLenCodeLens = "000 000 100 010 000 000 000 000 000 000 000 000 000 000 000 000 000 010";
            var codeLens = "01111111 00101011 11 11 10";
            var data = "1";
            test(blockHeader + codeCounts + codeLenCodeLens + codeLens + data, string.Empty);
        }

        [Test]
        public void testDynamicHuffmanCodeLengthRepeatAtStart()
        {
            // Dynamic Huffman block:
            //   numLitLen=257, numDist=1, numCodeLen=18
            //   codeLenCodeLen = 0:0, 1:1, 2:0, ..., 15:0, 16:1, 17:0, 18:0
            //   Literal/length/distance code lengths: #16+00
            var blockHeader = "1 01";
            var codeCounts = "00000 00000 0111";
            var codeLenCodeLens = "100 000 000 000 000 000 000 000 000 000 000 000 000 000 000 000 000 100";
            var codeLens = "1";
            Assert.Throws<FormatException>(() => test(blockHeader + codeCounts + codeLenCodeLens + codeLens, string.Empty));
        }

        [Test]
        public void testDynamicHuffmanTooManyCodeLengthItems()
        {
            // Dynamic Huffman block:
            //   numLitLen=257, numDist=1, numCodeLen=18
            //   codeLenCodeLen = 0:0, 1:1, 2:0, ..., 15:0, 16:0, 17:0, 18:1
            //   Literal/length/distance code lengths: 1 1 #18+1111111 #18+1101100
            var blockHeader = "1 01";
            var codeCounts = "00000 00000 0111";
            var codeLenCodeLens = "000 000 100 000 000 000 000 000 000 000 000 000 000 000 000 000 000 100";
            var codeLens = "0 0 11111111 10011011";
            Assert.Throws<FormatException>(() => test(blockHeader + codeCounts + codeLenCodeLens + codeLens, string.Empty));
        }

        /* Utility method */

        // 'input' is a string of 0's and 1's (with optional spaces) representing the input bit sequence.
        // 'refOutput' is a string of pairs of hexadecimal digits (with optional spaces) representing
        // the expected decompressed output byte sequence.
        private static void test(string input, string refOutput)
        {
            refOutput = refOutput.Replace(" ", string.Empty);
            if (refOutput.Length % 2 != 0)
            {
                throw new ArgumentException();
            }

            var refOut = new byte[refOutput.Length / 2];
            for (int i = 0; i < refOut.Length; i++)
            {
                refOut[i] = (byte)int.Parse(refOutput.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            input = input.Replace(" ", string.Empty);
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
