// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingFigure.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a drawing report item.
    /// </summary>
    /// <remarks>
    /// Drawing currently only supports SVG format.
    /// </remarks>
    public class DrawingFigure : Figure
    {
        #region Enums

        /// <summary>
        /// The drawing format.
        /// </summary>
        public enum DrawingFormat
        {
            /// <summary>
            ///   The svg.
            /// </summary>
            Svg
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///   Gets or sets Format.
        /// </summary>
        public DrawingFormat Format { get; set; }

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
            w.WriteDrawing(this);
        }

        #endregion
    }
}