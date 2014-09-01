// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Paragraph.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a paragraph.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a paragraph.
    /// </summary>
    public class Paragraph : ReportItem
    {
        /// <summary>
        /// Gets or sets the paragraph text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Writes the content of the paragraph.
        /// </summary>
        /// <param name="w">The target <see cref="IReportWriter" />.</param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteParagraph(this);
        }
    }
}