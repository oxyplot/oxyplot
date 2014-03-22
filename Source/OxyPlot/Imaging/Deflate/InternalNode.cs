// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalNode.cs" company="OxyPlot">
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
//   Represents an internal node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Represents an internal node.
    /// </summary>
    internal sealed class InternalNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalNode"/> class.
        /// </summary>
        /// <param name="leftChild">The left child.</param>
        /// <param name="rightChild">The right child.</param>
        public InternalNode(Node leftChild, Node rightChild)
        {
            if (leftChild == null)
            {
                throw new ArgumentException("Argument is null", "leftChild");
            }

            if (rightChild == null)
            {
                throw new ArgumentException("Argument is null", "rightChild");
            }

            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }

        /// <summary>
        /// Gets the left child.
        /// </summary>
        public Node LeftChild { get; private set; }

        /// <summary>
        /// Gets the right child.
        /// </summary>
        public Node RightChild { get; private set; }
    }
}