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
    public abstract partial class Element
    {
        /// <summary>
        /// Gets the parent model of the element.
        /// </summary>
        /// <value>
        /// The <see cref="Model" /> that is the parent of the element.
        /// </value>
        public Model Parent { get; internal set; }

        /// <summary>
        /// Tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A hit test result.
        /// </returns>
        public HitTestResult HitTest(HitTestArguments args)
        {
            return this.HitTestOverride(args);
        }

        /// <summary>
        /// When overridden in a derived class, tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// The result of the hit test.
        /// </returns>
        protected virtual HitTestResult HitTestOverride(HitTestArguments args)
        {
            return null;
        }
    }
}
