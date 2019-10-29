// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolTippedPlotElement.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   A wrapper around PlotElement that can also represent elements of the plot that are not exposed (currently just the plot title) or the absence of an element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// A wrapper around <see cref="OxyPlot.PlotElement"/> that can also represent elements of the plot that are not exposed (currently just the plot title) or the absence of an element.
    /// </summary>
    public class ToolTippedPlotElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTippedPlotElement"/> class.
        /// </summary>
        /// <param name="type">The type of element that the new instance will represent.</param>
        /// <param name="el">What <see cref="OxyPlot.PlotElement"/> the new instance will represent.</param>
        protected ToolTippedPlotElement(ToolTippedPlotElementType type, PlotElement el = null)
        {
            switch (type)
            {
                case ToolTippedPlotElementType.Null:
                    break;

                case ToolTippedPlotElementType.PlotElement:
                    this.PlotElement = el;
                    break;

                case ToolTippedPlotElementType.PlotTitleArea:
                    this.IsPlotTitleArea = true;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The types of things that a <see cref="ToolTippedPlotElement"/> can represent.
        /// </summary>
        protected enum ToolTippedPlotElementType
        {
            /// <summary>
            /// Represents the absence of an element.
            /// </summary>
            Null,

            /// <summary>
            /// Represents the plot title area pseudo-element.
            /// </summary>
            PlotTitleArea,

            /// <summary>
            /// Represents a plot element.
            /// </summary>
            PlotElement
        }

        /// <summary>
        /// Gets the wrapped <see cref="OxyPlot.PlotElement"/>, or null if there is no wrapped <see cref="OxyPlot.PlotElement"/> or if this represents the plot title.
        /// </summary>
        public PlotElement PlotElement { get; private set; } = null;

        /// <summary>
        /// Gets a value indicating whether this represents the plot title area.
        /// </summary>
        public bool IsPlotTitleArea { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether this represents something in the plot.
        /// </summary>
        public bool Exists
        {
            get
            {
                return this.PlotElement != null || this.IsPlotTitleArea;
            }
        }

        /// <summary>
        /// Creates and returns a new instance of the <see cref="ToolTippedPlotElement"/> class that represents a <see cref="OxyPlot.PlotElement"/>.
        /// </summary>
        /// <param name="el">The PlotElement to be wrapped.</param>
        public static ToolTippedPlotElement FromElement(PlotElement el)
        {
            return new ToolTippedPlotElement(ToolTippedPlotElementType.PlotElement, el);
        }

        /// <summary>
        /// Creates and returns a new instance of the <see cref="ToolTippedPlotElement"/> class that represents a pseudo-element (the plot title area or nothing).
        /// </summary>
        /// <param name="isPlotTitleArea">Whether the returned <see cref="ToolTippedPlotElement"/> will be the plot title area or the absence of an element.</param>
        public static ToolTippedPlotElement FromPseudoElement(bool isPlotTitleArea = false)
        {
            if (isPlotTitleArea)
            {
                return new ToolTippedPlotElement(ToolTippedPlotElementType.PlotTitleArea);
            }
            return new ToolTippedPlotElement(ToolTippedPlotElementType.Null);
        }

        /// <summary>
        /// Verifies the equivalence of two <see cref="ToolTippedPlotElement"/>s.
        /// </summary>
        /// <param name="t">The other <see cref="ToolTippedPlotElement"/>.</param>
        /// <returns>True if the two <see cref="ToolTippedPlotElement"/>s are equivalent.</returns>
        public bool IsEquivalentWith(ToolTippedPlotElement t)
        {
            return this.Exists && t.Exists &&
                (this.PlotElement == t.PlotElement ||
                (this.IsPlotTitleArea == true && t.IsPlotTitleArea == true));
        }

        /// <summary>
        /// Verifies the equivalence of a <see cref="ToolTippedPlotElement"/> and a <see cref="OxyPlot.PlotElement"/>.
        /// </summary>
        /// <param name="el">The <see cref="OxyPlot.PlotElement"/>.</param>
        /// <returns>True if the <see cref="ToolTippedPlotElement"/> is equivalent with the <see cref="OxyPlot.PlotElement"/>.</returns>
        public bool IsEquivalentWith(PlotElement el)
        {
            return this.Exists && this.PlotElement == el;
        }

        /// <summary>
        /// Computes the hash code of the <see cref="ToolTippedPlotElement"/>.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        public override int GetHashCode()
        {
            return this.PlotElement != null ? this.PlotElement.GetHashCode() :
                this.IsPlotTitleArea.GetHashCode();
        }

        /// <summary>
        /// Useful for debugging.
        /// </summary>
        /// <returns>A string representation of the <see cref="ToolTippedPlotElement"/>.</returns>
        public override string ToString()
        {
            if (this.PlotElement != null)
            {
                return $"PlotElement";
            }

            if (this.IsPlotTitleArea)
            {
                return "PlotTitleArea";
            }

            return "None";
        }
    }
}
