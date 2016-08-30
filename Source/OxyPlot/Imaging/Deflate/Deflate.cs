// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Deflate.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements DEFLATE decompression.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// Implements DEFLATE decompression.
    /// </summary>
    /// <remarks>The code is a c# port of the DEFLATE project by Nayuki Minase at <a href="https://github.com/nayuki/DEFLATE">github</a>.
    /// Original source code: <a href="https://github.com/nayuki/DEFLATE/blob/master/src/nayuki/deflate/Decompressor.java"><c>Decompressor.java</c></a>.</remarks>
    public class Deflate : IDisposable
    {
        /// <summary>
        /// The fixed literal length code.
        /// </summary>
        private static readonly CodeTree FixedLiteralLengthCode;

        /// <summary>
        /// The fixed distance code.
        /// </summary>
        private static readonly CodeTree FixedDistanceCode;

        /// <summary>
        /// The dictionary.
        /// </summary>
        private readonly CircularDictionary dictionary;

        /// <summary>
        /// The input.
        /// </summary>
        private readonly BitReader input;

        /// <summary>
        /// The output.
        /// </summary>
        private readonly BinaryWriter output;

        /// <summary>
        /// The output stream.
        /// </summary>
        private readonly MemoryStream outputStream;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes static members of the <see cref="Deflate" /> class.
        /// </summary>
        static Deflate()
        {
            var llcodelens = new int[288];
            Arrays.Fill(llcodelens, 0, 144, 8);
            Arrays.Fill(llcodelens, 144, 256, 9);
            Arrays.Fill(llcodelens, 256, 280, 7);
            Arrays.Fill(llcodelens, 280, 288, 8);
            FixedLiteralLengthCode = new CanonicalCode(llcodelens).ToCodeTree();

            var distcodelens = new int[32];
            Arrays.Fill(distcodelens, 0, 32, 5);
            FixedDistanceCode = new CanonicalCode(distcodelens).ToCodeTree();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deflate" /> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        private Deflate(BitReader reader)
        {
            this.input = reader;
            this.outputStream = new MemoryStream();
            this.output = new BinaryWriter(this.outputStream);
            this.dictionary = new CircularDictionary(32 * 1024);

            // Process the stream of blocks
            while (true)
            {
                // Block header
                var isFinal = this.input.ReadNoEof() != -1; // bfinal
                var type = this.ReadInt(2); // btype

                // Decompress by type
                if (type == 0)
                {
                    this.DecompressUncompressedBlock();
                }
                else if (type == 1 || type == 2)
                {
                    CodeTree litLenCode, distCode;
                    if (type == 1)
                    {
                        litLenCode = FixedLiteralLengthCode;
                        distCode = FixedDistanceCode;
                    }
                    else
                    {
                        var temp = this.DecodeHuffmanCodes();
                        litLenCode = temp[0];
                        distCode = temp[1];
                    }

                    this.DecompressHuffmanBlock(litLenCode, distCode);
                }
                else if (type == 3)
                {
                    throw new FormatException("Invalid block type");
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (isFinal)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Decompresses the data from the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An array of <see cref="byte" />.</returns>
        public static byte[] Decompress(Stream input)
        {
            var decomp = new Deflate(new ByteBitReader(input));
            return decomp.outputStream.ToArray();
        }

        /// <summary>
        /// Decompresses the data from the specified <see cref="BitReader" />.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An array of <see cref="byte" />.</returns>
        public static byte[] Decompress(BitReader input)
        {
            var decomp = new Deflate(input);
            return decomp.outputStream.ToArray();
        }

        /// <summary>
        /// Decompresses the specified data.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An array of <see cref="byte" />.</returns>
        public static byte[] Decompress(byte[] input)
        {
            return Decompress(new MemoryStream(input));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// For handling dynamic Huffman codes.
        /// </summary>
        /// <returns>A sequence of <see cref="CodeTree" /> items.</returns>
        private CodeTree[] DecodeHuffmanCodes()
        {
            var numLitLenCodes = this.ReadInt(5) + 257; // hlit  + 257
            var numDistCodes = this.ReadInt(5) + 1; // hdist +   1

            var numCodeLenCodes = this.ReadInt(4) + 4; // hclen +   4
            var codeLenCodeLen = new int[19];
            codeLenCodeLen[16] = this.ReadInt(3);
            codeLenCodeLen[17] = this.ReadInt(3);
            codeLenCodeLen[18] = this.ReadInt(3);
            codeLenCodeLen[0] = this.ReadInt(3);
            for (var i = 0; i < numCodeLenCodes - 4; i++)
            {
                if (i % 2 == 0)
                {
                    codeLenCodeLen[8 + (i / 2)] = this.ReadInt(3);
                }
                else
                {
                    codeLenCodeLen[7 - (i / 2)] = this.ReadInt(3);
                }
            }

            var codeLenCode = new CanonicalCode(codeLenCodeLen).ToCodeTree();

            var codeLens = new int[numLitLenCodes + numDistCodes];
            var runVal = -1;
            var runLen = 0;
            for (var i = 0; i < codeLens.Length; i++)
            {
                if (runLen > 0)
                {
                    codeLens[i] = runVal;
                    runLen--;
                }
                else
                {
                    var sym = this.DecodeSymbol(codeLenCode);
                    if (sym < 16)
                    {
                        codeLens[i] = sym;
                        runVal = sym;
                    }
                    else
                    {
                        if (sym == 16)
                        {
                            if (runVal == -1)
                            {
                                throw new FormatException("No code length value to copy");
                            }

                            runLen = this.ReadInt(2) + 3;
                        }
                        else if (sym == 17)
                        {
                            runVal = 0;
                            runLen = this.ReadInt(3) + 3;
                        }
                        else if (sym == 18)
                        {
                            runVal = 0;
                            runLen = this.ReadInt(7) + 11;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        i--;
                    }
                }
            }

            if (runLen > 0)
            {
                throw new FormatException("Run exceeds number of codes");
            }

            // Create code trees
            var litLenCodeLen = Arrays.CopyOf(codeLens, numLitLenCodes);
            var litLenCode = new CanonicalCode(litLenCodeLen).ToCodeTree();

            var distCodeLen = Arrays.CopyOfRange(codeLens, numLitLenCodes, codeLens.Length);
            CodeTree distCode;
            if (distCodeLen.Length == 1 && distCodeLen[0] == 0)
            {
                distCode = null; // Empty distance code; the block shall be all literal symbols
            }
            else
            {
                distCode = new CanonicalCode(distCodeLen).ToCodeTree();
            }

            return new[] { litLenCode, distCode };
        }

        /// <summary>
        /// Decompress an uncompressed block.
        /// </summary>
        private void DecompressUncompressedBlock()
        {
            // Discard bits to align to byte boundary
            while (this.input.GetBitPosition() != 0)
            {
                this.input.Read();
            }

            // Read length
            var len = this.ReadInt(16);
            var nlen = this.ReadInt(16);
            if ((len ^ 0xFFFF) != nlen)
            {
                throw new FormatException("Invalid length in uncompressed block");
            }

            // Copy bytes
            for (var i = 0; i < len; i++)
            {
                var temp = this.input.ReadByte();
                if (temp == -1)
                {
                    throw new Exception("EOF");
                }

                this.output.Write((byte)temp);
                this.dictionary.Append(temp);
            }
        }

        /// <summary>
        /// Decompresses a Huffman block.
        /// </summary>
        /// <param name="litLenCode">The litLen code.</param>
        /// <param name="distCode">The distance code.</param>
        private void DecompressHuffmanBlock(CodeTree litLenCode, CodeTree distCode)
        {
            if (litLenCode == null)
            {
                throw new InvalidOperationException();
            }

            while (true)
            {
                var sym = this.DecodeSymbol(litLenCode);
                if (sym == 256)
                {
                    // End of block
                    break;
                }

                if (sym < 256)
                {
                    // Literal byte
                    this.output.Write((byte)sym);
                    this.dictionary.Append(sym);
                }
                else
                {
                    // Length and distance for copying
                    var len = this.DecodeRunLength(sym);
                    if (distCode == null)
                    {
                        throw new FormatException("Length symbol encountered with empty distance code");
                    }

                    var distSym = this.DecodeSymbol(distCode);
                    var dist = this.DecodeDistance(distSym);
                    this.dictionary.Copy(dist, len, this.output);
                }
            }
        }

        /// <summary>
        /// Decodes the specified symbol.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>The <see cref="int" />.</returns>
        private int DecodeSymbol(CodeTree code)
        {
            var currentNode = code.Root;
            while (true)
            {
                var temp = this.input.ReadNoEof();
                Node nextNode;
                if (temp == 0)
                {
                    nextNode = currentNode.LeftChild;
                }
                else if (temp == 1)
                {
                    nextNode = currentNode.RightChild;
                }
                else
                {
                    throw new Exception();
                }

                if (nextNode is Leaf)
                {
                    return ((Leaf)nextNode).Symbol;
                }

                if (nextNode is InternalNode)
                {
                    currentNode = (InternalNode)nextNode;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Decodes the run length.
        /// </summary>
        /// <param name="sym">The symbol.</param>
        /// <returns>The <see cref="int" />.</returns>
        private int DecodeRunLength(int sym)
        {
            if (sym < 257 || sym > 285)
            {
                throw new FormatException("Invalid run length symbol: " + sym);
            }

            if (sym <= 264)
            {
                return sym - 254;
            }

            if (sym <= 284)
            {
                var i = (sym - 261) / 4; // Number of extra bits to read
                return ((((sym - 265) % 4) + 4) << i) + 3 + this.ReadInt(i);
            }

            return 258;
        }

        /// <summary>
        /// Decodes distance.
        /// </summary>
        /// <param name="sym">The symbol.</param>
        /// <returns>The <see cref="int" />.</returns>
        private int DecodeDistance(int sym)
        {
            if (sym <= 3)
            {
                return sym + 1;
            }

            if (sym <= 29)
            {
                var i = (sym / 2) - 1; // Number of extra bits to read
                return (((sym % 2) + 2) << i) + 1 + this.ReadInt(i);
            }

            throw new FormatException("Invalid distance symbol: " + sym);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.output.Dispose();
                    this.outputStream.Dispose();
                }
            }

            this.disposed = true;
        }

        /* Utility method */

        /// <summary>
        /// Reads the specified number of bits.
        /// </summary>
        /// <param name="numBits">The number of bits to read.</param>
        /// <returns>The <see cref="int" />.</returns>
        private int ReadInt(int numBits)
        {
            if (numBits < 0 || numBits >= 32)
            {
                throw new ArgumentException();
            }

            var result = 0;
            for (var i = 0; i < numBits; i++)
            {
                result |= this.input.ReadNoEof() << i;
            }

            return result;
        }
    }
}