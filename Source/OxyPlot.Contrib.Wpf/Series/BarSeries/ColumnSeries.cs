// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.BarSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    using OxyPlot.Series;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.BarSeries
    /// </summary>
    public class ColumnSeries : BarSeriesBase<ColumnItem>
    {
        /// <summary>
        /// Identifies the <see cref="ColumnWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register(
            "ColumnWidth", typeof(double), typeof(ColumnSeries), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries" /> class.
        /// </summary>
        public ColumnSeries()
        {
            this.InternalSeries = new OxyPlot.Series.ColumnSeries();
        }

        /// <summary>
        /// Gets or sets ColumnWidth.
        /// </summary>
        public double ColumnWidth
        {
            get
            {
                return (double)this.GetValue(ColumnWidthProperty);
            }

            set
            {
                this.SetValue(ColumnWidthProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.ColumnSeries)series;
            s.ColumnWidth = this.ColumnWidth;
        }
    }
}