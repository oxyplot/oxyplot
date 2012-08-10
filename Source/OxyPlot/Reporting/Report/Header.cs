// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Header.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a header.
    /// </summary>
    public class Header : ReportItem
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the chapter number(s).
        /// </summary>
        public string Chapter { get; set; }

        /// <summary>
        ///   Gets or sets the level of the header (1-5).
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///   Gets or sets the header text.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
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
        /// <param name="w">
        /// The w.
        /// </param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteHeader(this);
        }

        #endregion
    }
}