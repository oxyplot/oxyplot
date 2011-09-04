//-----------------------------------------------------------------------
// <copyright file="ISeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

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
