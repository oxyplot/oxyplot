// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections;

    /// <summary>
    /// Abstract base class for series that use X and Y axes.
    /// </summary>
    public abstract class ItemsSeries : XYAxisSeries
    {
        #region Methods

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        protected override void SynchronizeProperties(OxyPlot.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.ItemsSeries)series;
            s.ItemsSource = this.ItemsSource;
        }

        #endregion
    }
}