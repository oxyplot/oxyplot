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
        ///   Gets the X.
        /// </summary>
        /// <value>The X.</value>
        double X { get; }

        /// <summary>
        ///   Gets the Y.
        /// </summary>
        /// <value>The Y.</value>
        double Y { get; }

        #endregion
    }
}