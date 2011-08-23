// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeries.cs" company="OxyPlot">
//   see http://oxyplot.codeplex.com
// </copyright>
// <summary>
//   The series interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// The series interface.
    /// </summary>
    public interface ISeries
    {
        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        OxyPlot.Series CreateModel();

        #endregion
    }
}