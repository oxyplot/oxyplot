// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Base class for data series
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using System;

    /// <summary>
    /// Base class for data series
    /// </summary>
    public abstract class DataPointSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies the <see cref="CanTrackerInterpolatePoints"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> CanTrackerInterpolatePointsProperty = AvaloniaProperty.Register<DataPointSeries, bool>(nameof(CanTrackerInterpolatePoints), false);

        /// <summary>
        /// Identifies the <see cref="DataFieldX"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldXProperty = AvaloniaProperty.Register<DataPointSeries, string>(nameof(DataFieldX), null);

        /// <summary>
        /// Identifies the <see cref="DataFieldY"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldYProperty = AvaloniaProperty.Register<DataPointSeries, string>(nameof(DataFieldY), null);

        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Func<object, DataPoint>> MappingProperty = AvaloniaProperty.Register<DataPointSeries, Func<object, DataPoint>>(nameof(Mapping));

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints
        {
            get
            {
                return GetValue(CanTrackerInterpolatePointsProperty);
            }

            set
            {
                SetValue(CanTrackerInterpolatePointsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldX.
        /// </summary>
        public string DataFieldX
        {
            get
            {
                return GetValue(DataFieldXProperty);
            }

            set
            {
                SetValue(DataFieldXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldY.
        /// </summary>
        public string DataFieldY
        {
            get
            {
                return GetValue(DataFieldYProperty);
            }

            set
            {
                SetValue(DataFieldYProperty, value);
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
                return GetValue(MappingProperty);
            }

            set
            {
                SetValue(MappingProperty, value);
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
            s.ItemsSource = Items;
            s.DataFieldX = DataFieldX;
            s.DataFieldY = DataFieldY;
            s.CanTrackerInterpolatePoints = CanTrackerInterpolatePoints;
            s.Mapping = Mapping;
        }

        static DataPointSeries()
        {
            CanTrackerInterpolatePointsProperty.Changed.AddClassHandler<DataPointSeries>(AppearanceChanged);
            DataFieldXProperty.Changed.AddClassHandler<DataPointSeries>(DataChanged);
            DataFieldYProperty.Changed.AddClassHandler<DataPointSeries>(DataChanged);
        }
    }
}