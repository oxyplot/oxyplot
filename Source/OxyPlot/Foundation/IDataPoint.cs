// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataPoint.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// DataPoint interface.
    /// </summary>
    public interface IDataPoint : ICodeGenerating
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        double X { get; set; }

        /// <summary>
        ///   Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        double Y { get; set; }

        #endregion
    }
}