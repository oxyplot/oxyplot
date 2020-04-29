// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Leaf.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a leaf.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Represents a leaf.
    /// </summary>
    internal class Leaf : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf" /> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <exception cref="System.ArgumentException">Illegal symbol value;symbol</exception>
        public Leaf(int symbol)
        {
            if (symbol < 0)
            {
                throw new ArgumentException("Illegal symbol value", "symbol");
            }

            this.Symbol = symbol;
        }

        /// <summary>
        /// Gets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public int Symbol { get; private set; }
    }
}