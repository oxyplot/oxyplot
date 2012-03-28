// -----------------------------------------------------------------------
// <copyright file="PlotElement.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Abstract base class for all plottable elements (Axes, Annotations, Series).
    /// </summary>
    [Serializable]
    public abstract class PlotElement
    {
        /// <summary>
        /// Gets or sets an arbitrary object value that can be used to store custom information about this plot element.
        /// </summary>
        /// <value>
        /// The intended value. This property has no default value.
        /// </value>
        /// <remarks>
        /// This property is analogous to Tag properties in other Microsoft programming models. 
        /// Tag is intended to provide a pre-existing property location where you can store some basic custom information 
        /// about any PlotElement without requiring you to subclass an element.
        /// </remarks>
        public object Tag { get; set; }
    }
}
