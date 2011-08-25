// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Paragraph.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The paragraph.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// The paragraph.
    /// </summary>
    public class Paragraph : ReportItem
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteParagraph(this);
        }

        #endregion
    }
}