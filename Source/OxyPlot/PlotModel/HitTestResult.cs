// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HitTestResult.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a <see cref="UIPlotElement"/> hit test result.
    /// </summary>
    public class HitTestResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HitTestResult"/> class.
        /// </summary>
        public HitTestResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HitTestResult"/> class.
        /// </summary>
        /// <param name="nhp">The nearest hit point.</param>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        public HitTestResult(ScreenPoint nhp, object item = null, double index = 0)
        {
            this.NearestHitPoint = nhp;
            this.Item = item;
            this.Index = index;
        }

        #region Public Properties

        /// <summary>
        ///   Gets or sets the index of the hit (if available).
        /// </summary>
        /// <value> The index. </value>
        /// <remarks>
        /// If the hit was in the middle between point 1 and 2, index = 1.5.
        /// </remarks>
        public double Index { get; set; }

        /// <summary>
        ///   Gets or sets the item of the hit.
        /// </summary>
        /// <value> The item. </value>
        public object Item { get; set; }

        /// <summary>
        ///   Gets or sets the position of the nearest hit point.
        /// </summary>
        /// <value> The nearest hit point. </value>
        public ScreenPoint NearestHitPoint { get; set; }

        #endregion
    }
}