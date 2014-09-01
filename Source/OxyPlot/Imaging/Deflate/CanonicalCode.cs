// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanonicalCode.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   A canonical Huffman code. Immutable. Code length 0 means no code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A canonical Huffman code. Immutable. Code length 0 means no code.
    /// </summary>
    /// <remarks><p>
    /// The code is a c# port of the DEFLATE project by Nayuki Minase at <a href="https://github.com/nayuki/DEFLATE">github</a>.
    /// Original source code: <a href="https://github.com/nayuki/DEFLATE/blob/master/src/nayuki/deflate/CircularDictionary.java">CircularDictionary.java</a>.
    /// </p>
    /// <p>
    /// A canonical Huffman code only describes the code length of each symbol. The codes can be reconstructed from this information. In this implementation, symbols with lower code lengths, breaking ties by lower symbols, are assigned lexicographically lower codes.
    /// Example:
    /// Code lengths (canonical code):
    /// Symbol A: 1
    /// Symbol B: 3
    /// Symbol C: 0 (no code)
    /// Symbol D: 2
    /// Symbol E: 3
    /// Huffman codes (generated from canonical code):
    /// Symbol A: 0
    /// Symbol B: 110
    /// Symbol C: None
    /// Symbol D: 10
    /// Symbol E: 111
    /// </p></remarks>
    internal class CanonicalCode
    {
        /// <summary>
        /// The code lengths
        /// </summary>
        private readonly int[] codeLengths;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanonicalCode" /> class.
        /// </summary>
        /// <param name="codeLengths">The code lengths.</param>
        /// <remarks>The constructor does not check that the array of code lengths results in a complete Huffman tree, being neither underfilled nor overfilled.</remarks>
        public CanonicalCode(int[] codeLengths)
        {
            if (codeLengths == null)
            {
                throw new Exception("Argument is null");
            }

            this.codeLengths = new int[codeLengths.Length];
            Array.Copy(codeLengths, this.codeLengths, codeLengths.Length);

            foreach (int x in codeLengths)
            {
                if (x < 0)
                {
                    throw new Exception("Illegal code length");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanonicalCode" /> class based on the given code tree.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="symbolLimit">The symbol limit.</param>
        public CanonicalCode(CodeTree tree, int symbolLimit)
        {
            this.codeLengths = new int[symbolLimit];
            this.BuildCodeLengths(tree.Root, 0);
        }

        /// <summary>
        /// Gets the symbol limit.
        /// </summary>
        /// <returns>The limit.</returns>
        public int GetSymbolLimit()
        {
            return this.codeLengths.Length;
        }

        /// <summary>
        /// Gets the length of the code.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The length.</returns>
        /// <exception cref="System.Exception">Symbol out of range</exception>
        public int GetCodeLength(int symbol)
        {
            if (symbol < 0 || symbol >= this.codeLengths.Length)
            {
                throw new Exception("Symbol out of range");
            }

            return this.codeLengths[symbol];
        }

        /// <summary>
        /// Converts the canonical code to a code tree.
        /// </summary>
        /// <returns>The code tree.</returns>
        /// <exception cref="System.Exception">This canonical code does not represent a Huffman code tree
        /// or
        /// This canonical code does not represent a Huffman code tree</exception>
        public CodeTree ToCodeTree()
        {
            var nodes = new List<Node>();
            for (int i = this.codeLengths.Max(); i >= 1; i--)
            {  // Descend through positive code lengths
                var newNodes = new List<Node>();

                // Add leaves for symbols with code length i
                for (int j = 0; j < this.codeLengths.Length; j++)
                {
                    if (this.codeLengths[j] == i)
                    {
                        newNodes.Add(new Leaf(j));
                    }
                }

                // Merge nodes from the previous deeper layer
                for (int j = 0; j < nodes.Count; j += 2)
                {
                    newNodes.Add(new InternalNode(nodes[j], nodes[j + 1]));
                }

                nodes = newNodes;
                if (nodes.Count % 2 != 0)
                {
                    throw new Exception("This canonical code does not represent a Huffman code tree");
                }
            }

            if (nodes.Count != 2)
            {
                throw new Exception("This canonical code does not represent a Huffman code tree");
            }

            return new CodeTree(new InternalNode(nodes[0], nodes[1]), this.codeLengths.Length);
        }

        /// <summary>
        /// Builds the code lengths.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="depth">The depth.</param>
        /// <exception cref="System.Exception">Symbol has more than one code
        /// or
        /// Symbol exceeds symbol limit
        /// or
        /// Illegal node type</exception>
        private void BuildCodeLengths(Node node, int depth)
        {
            if (node is InternalNode)
            {
                var internalNode = (InternalNode)node;
                this.BuildCodeLengths(internalNode.LeftChild, depth + 1);
                this.BuildCodeLengths(internalNode.RightChild, depth + 1);
            }
            else if (node is Leaf)
            {
                int symbol = ((Leaf)node).Symbol;
                if (this.codeLengths[symbol] != 0)
                {
                    throw new Exception("Symbol has more than one code");  // Because CodeTree has a checked constraint that disallows a symbol in multiple leaves
                }

                if (symbol >= this.codeLengths.Length)
                {
                    throw new Exception("Symbol exceeds symbol limit");
                }

                this.codeLengths[symbol] = depth;
            }
            else
            {
                throw new Exception("Illegal node type");
            }
        }
    }
}