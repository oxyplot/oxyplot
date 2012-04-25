// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataPointProvider.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides functionality to create data points for items in an <see cref="ItemsSeries"/>.
    /// </summary>
    public interface IDataPointProvider
    {
        #region Public Methods

        /// <summary>
        /// Gets the data point.
        /// </summary>
        /// <returns>
        /// The data point. 
        /// </returns>
        DataPoint GetDataPoint();

        #endregion
    }
}