// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Header.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a header.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a header.
    /// </summary>
    public class Header : ReportItem
    {
        /// <summary>
        /// Gets or sets the chapter number(s).
        /// </summary>
        public string Chapter { get; set; }

        /// <summary>
        /// Gets or sets the level of the header (1-5).
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Returns a string that represents the header.
        /// </summary>
        /// <returns>A string that represents the header.</returns>
        public override string ToString()
        {
            string h = string.Empty;
            if (this.Chapter != null)
            {
                h += this.Chapter + " ";
            }

            h += this.Text;
            return h;
        }

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">The w.</param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteHeader(this);
        }
    }
}