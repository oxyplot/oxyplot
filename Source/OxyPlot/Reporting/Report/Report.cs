// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Report.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a report.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Globalization;

    /// <summary>
    /// Represents a report.
    /// </summary>
    public class Report : ReportItem
    {
        /// <summary>
        /// Gets the actual culture.
        /// </summary>
        public CultureInfo ActualCulture
        {
            get
            {
                return this.Culture ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Writes the report to a <see cref="IReportWriter" />.
        /// </summary>
        /// <param name="w">The target <see cref="IReportWriter" />.</param>
        public override void Write(IReportWriter w)
        {
            this.UpdateParent(this);
            this.UpdateFigureNumbers();
            base.Write(w);
        }
    }
}