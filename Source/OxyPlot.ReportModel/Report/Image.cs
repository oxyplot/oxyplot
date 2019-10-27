// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Image.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an image report item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents an image report item.
    /// </summary>
    public class Image : Figure
    {
        /// <summary>
        /// Gets or sets Source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">The w.</param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteImage(this);
        }
    }
}