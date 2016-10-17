// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRectSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Base class for data series represented by areas
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Base class for data series
    /// </summary>
    public abstract class DataRectSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies the <see cref="CanTrackerInterpolatePoints"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanTrackerInterpolatePointsProperty =
            DependencyProperty.Register(
                "CanTrackerInterpolatePoints",
                typeof(bool),
                typeof(DataRectSeries),
                new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MappingProperty = DependencyProperty.Register(
            "Mapping", typeof(Func<object, DataRect>), typeof(DataRectSeries), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints
        {
            get
            {
                return (bool)this.GetValue(CanTrackerInterpolatePointsProperty);
            }

            set
            {
                this.SetValue(CanTrackerInterpolatePointsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the mapping.
        /// </summary>
        /// <value>The mapping.</value>
        public Func<object, DataRect> Mapping
        {
            get
            {
                return (Func<object, DataRect>)this.GetValue(MappingProperty);
            }

            set
            {
                this.SetValue(MappingProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.DataRectSeries)series;
            s.ItemsSource = this.ItemsSource;
            s.CanTrackerInterpolatePoints = this.CanTrackerInterpolatePoints;
            s.Mapping = this.Mapping;
        }
    }
}