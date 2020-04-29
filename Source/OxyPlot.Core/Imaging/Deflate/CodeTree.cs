// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeTree.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The code tree.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// The code tree.
    /// </summary>
    /// <remarks>The code is a c# port of Nayuki Minase's DEFLATE project at <a href="https://github.com/nayuki/DEFLATE">GitHub</a>.
    /// Original source code: <a href="https://github.com/nayuki/DEFLATE/blob/master/src/nayuki/deflate/CodeTree.java">CodeTree.java</a>.</remarks>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    internal class CodeTree
    {
        /// <summary>
        /// Stores the code for each symbol, or <c>null</c> if the symbol has no code.
        /// For example, if symbol 5 has code 10011, then codes.get(5) is the list [1, 0, 0, 1, 1].
        /// </summary>
        private readonly List<List<int>> codes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTree" /> class. Every symbol in the tree 'root' must be strictly less than 'symbolLimit'.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="symbolLimit">The symbol limit.</param>
        public CodeTree(InternalNode root, int symbolLimit)
        {
            if (root == null)
            {
                throw new Exception("Argument is null");
            }

            this.Root = root;

            this.codes = new List<List<int>>(); // Initially all null
            for (var i = 0; i < symbolLimit; i++)
            {
                this.codes.Add(null);
            }

            this.BuildCodeList(root, new List<int>()); // Fills 'codes' with appropriate data
        }

        /// <summary>
        /// Gets the root.
        /// </summary>
        public InternalNode Root { get; private set; } // Not null

        /// <summary>
        /// Gets the code for the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>A <see cref="List{T}" /> of codes.</returns>
        public List<int> GetCode(int symbol)
        {
            if (symbol < 0)
            {
                throw new Exception("Illegal symbol");
            }

            if (this.codes[symbol] == null)
            {
                throw new Exception("No code for given symbol");
            }

            return this.codes[symbol];
        }

        /// <summary>
        /// Returns a string showing all the codes in this tree. The format is subject to change. Useful for debugging.
        /// </summary>
        /// <returns>The <see cref="string" />.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            NodeString(string.Empty, this.Root, sb);
            return sb.ToString();
        }

        /// <summary>
        /// Appends the code of the specified node to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="node">The node.</param>
        /// <param name="sb">The string builder.</param>
        /// <exception cref="System.Exception">Illegal node type</exception>
        private static void NodeString(string prefix, Node node, StringBuilder sb)
        {
            var n = node as InternalNode;
            if (n != null)
            {
                var internalNode = n;
                NodeString(prefix + "0", internalNode.LeftChild, sb);
                NodeString(prefix + "1", internalNode.RightChild, sb);
            }
            else
            {
                var leaf = node as Leaf;
                if (leaf != null)
                {
                    sb.Append(string.Format("Code {0}: Symbol {1}", prefix, leaf.Symbol));
                }
                else
                {
                    throw new Exception("Illegal node type");
                }
            }
        }

        /// <summary>
        /// Builds the code list.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="prefix">The prefix.</param>
        private void BuildCodeList(Node node, List<int> prefix)
        {
            if (node is InternalNode)
            {
                var internalNode = (InternalNode)node;

                prefix.Add(0);
                this.BuildCodeList(internalNode.LeftChild, prefix);
                prefix.RemoveAt(prefix.Count - 1);

                prefix.Add(1);
                this.BuildCodeList(internalNode.RightChild, prefix);
                prefix.RemoveAt(prefix.Count - 1);
            }
            else if (node is Leaf)
            {
                var leaf = (Leaf)node;
                if (leaf.Symbol >= this.codes.Count)
                {
                    throw new Exception("Symbol exceeds symbol limit");
                }

                if (this.codes[leaf.Symbol] != null)
                {
                    throw new Exception("Symbol has more than one code");
                }

                this.codes[leaf.Symbol] = new List<int>(prefix);
            }
            else
            {
                throw new Exception("Illegal node type");
            }
        }
    }
}