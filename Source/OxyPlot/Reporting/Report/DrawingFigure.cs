// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingFigure.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a drawing report item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a drawing report item.
    /// </summary>
    /// <remarks>Drawing currently only supports SVG format.</remarks>
    public class DrawingFigure : Figure
    {
        /// <summary>
        /// The drawing format.
        /// </summary>
        public enum DrawingFormat
        {
            /// <summary>
            /// The svg.
            /// </summary>
            Svg
        }

        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets Format.
        /// </summary>
        public DrawingFormat Format { get; set; }

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">The w.</param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteDrawing(this);
        }
    }
}