// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Element.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for graphics elements.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides an abstract base class for graphics elements.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Gets the parent model of the element.
        /// </summary>
        /// <value>
        /// The <see cref="Model" /> that is the parent of the element.
        /// </value>
        public Model Parent { get; internal set; }
    }
}