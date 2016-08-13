// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeries.cs" company="OxyPlot">
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
    public class BarSeries : BarSeriesBase<BarItem>
    {
        /// <summary>
        /// Identifies the <see cref="BarWidth"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> BarWidthProperty = AvaloniaProperty.Register<BarSeries, double>(nameof(BarWidth), 1.0);

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeries" /> class.
        /// </summary>
        public BarSeries()
        {
            InternalSeries = new OxyPlot.Series.BarSeries();
        }

        /// <summary>
        /// Gets or sets the bar width.
        /// </summary>
        public double BarWidth
        {
            get
            {
                return GetValue(BarWidthProperty);
            }

            set
            {
                SetValue(BarWidthProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.BarSeries)series;
            s.BarWidth = BarWidth;
        }

        static BarSeries()
        {
            BarWidthProperty.Changed.AddClassHandler<BarSeries>(AppearanceChanged);
        }
    }
}