// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Image.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Image files (.png, .jpg)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Image files (.png, .jpg)
    /// </summary>
    public class Image : Figure
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets Source.
        /// </summary>
        public string Source { get; set; }

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
            w.WriteImage(this);
        }

        #endregion
    }
}