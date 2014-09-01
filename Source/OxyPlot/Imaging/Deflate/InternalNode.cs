// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalNode.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        /// Initializes a new instance of the <see cref="InternalNode" /> class.
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