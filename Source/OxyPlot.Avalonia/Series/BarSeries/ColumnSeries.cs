// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.BarSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{

    using OxyPlot.Series;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.BarSeries
    /// </summary>
    public class ColumnSeries : BarSeriesBase<ColumnItem>
    {
        /// <summary>
        /// Identifies the <see cref="ColumnWidth"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> ColumnWidthProperty = AvaloniaProperty.Register<ColumnSeries, double>(nameof(ColumnWidth), 1.0);

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries" /> class.
        /// </summary>
        public ColumnSeries()
        {
            InternalSeries = new OxyPlot.Series.ColumnSeries();
        }

        /// <summary>
        /// Gets or sets ColumnWidth.
        /// </summary>
        public double ColumnWidth
        {
            get
            {
                return GetValue(ColumnWidthProperty);
            }

            set
            {
                SetValue(ColumnWidthProperty, value);
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
            s.ColumnWidth = ColumnWidth;
        }

        static ColumnSeries()
        {
            ColumnWidthProperty.Changed.AddClassHandler<ColumnSeries>(AppearanceChanged);
        }
    }
}