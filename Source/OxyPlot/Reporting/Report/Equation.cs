// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Equation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents an equation.
    /// </summary>
    public class Equation : ReportItem
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets Caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        ///   Gets or sets Content.
        /// </summary>
        public string Content { get; set; }

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
            w.WriteEquation(this);
        }

        #endregion
    }
}