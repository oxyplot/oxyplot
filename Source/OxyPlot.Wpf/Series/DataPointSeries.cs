// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Base class for data series
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Base class for data series
    /// </summary>
    public abstract class DataPointSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies the <see cref="CanTrackerInterpolatePoints"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanTrackerInterpolatePointsProperty =
            DependencyProperty.Register(
                "CanTrackerInterpolatePoints",
                typeof(bool),
                typeof(DataPointSeries),
                new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldXProperty = DependencyProperty.Register(
            "DataFieldX", typeof(string), typeof(DataPointSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldYProperty = DependencyProperty.Register(
            "DataFieldY", typeof(string), typeof(DataPointSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MappingProperty = DependencyProperty.Register(
            "Mapping", typeof(Func<object, DataPoint>), typeof(DataPointSeries), new UIPropertyMetadata(null));

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
        /// Gets or sets DataFieldX.
        /// </summary>
        public string DataFieldX
        {
            get
            {
                return (string)this.GetValue(DataFieldXProperty);
            }

            set
            {
                this.SetValue(DataFieldXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldY.
        /// </summary>
        public string DataFieldY
        {
            get
            {
                return (string)this.GetValue(DataFieldYProperty);
            }

            set
            {
                this.SetValue(DataFieldYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the mapping.
        /// </summary>
        /// <value>The mapping.</value>
        public Func<object, DataPoint> Mapping
        {
            get
            {
                return (Func<object, DataPoint>)this.GetValue(MappingProperty);
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
            var s = (OxyPlot.Series.DataPointSeries)series;
            s.ItemsSource = this.ItemsSource;
            s.DataFieldX = this.DataFieldX;
            s.DataFieldY = this.DataFieldY;
            s.CanTrackerInterpolatePoints = this.CanTrackerInterpolatePoints;
            s.Mapping = this.Mapping;
        }
    }
}