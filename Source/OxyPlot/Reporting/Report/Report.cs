// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Report.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Globalization;

    /// <summary>
    /// Represents a report.
    /// </summary>
    public class Report : ReportItem
    {
        #region Public Properties

        /// <summary>
        ///   Gets the actual culture.
        /// </summary>
        public CultureInfo ActualCulture
        {
            get
            {
                return this.Culture ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        ///   Gets or sets Author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///   Gets or sets the culture.
        /// </summary>
        /// <value>
        ///   The culture.
        /// </value>
        public CultureInfo Culture { get; set; }

        /// <summary>
        ///   Gets or sets SubTitle.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        ///   Gets or sets Title.
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        public override void Write(IReportWriter w)
        {
            this.UpdateParent(this);
            this.UpdateFigureNumbers();
            base.Write(w);
        }

        #endregion
    }
}