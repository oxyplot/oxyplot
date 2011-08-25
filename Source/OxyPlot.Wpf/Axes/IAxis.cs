// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The axis interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// The axis interface.
    /// </summary>
    public interface IAxis
    {
        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        OxyPlot.Axis CreateModel();

        #endregion
    }
}