// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.BarSeries&lt;T&gt;.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// This is a WPF wrapper of OxyPlot.BarSeries&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public class BarSeriesBase<T> : BarSeriesBase
        where T : OxyPlot.Series.BarItemBase, new()
    {
        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            if (this.ItemsSource == null)
            {
                var s = (OxyPlot.Series.BarSeriesBase<T>)series;
                s.Items.Clear();
                foreach (T item in this.Items)
                {
                    s.Items.Add(item);
                }
            }
        }
    }
}