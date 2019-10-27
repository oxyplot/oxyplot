// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   RectangleSeries WPF wrapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;

    using OxyPlot.Series;

    /// <summary>
    /// RectangleSeries WPF wrapper
    /// </summary>
    public class RectangleSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies the <see cref="CanTrackerInterpolatePoints"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanTrackerInterpolatePointsProperty =
            DependencyProperty.Register(
                "CanTrackerInterpolatePoints",
                typeof(bool),
                typeof(RectangleSeries),
                new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MappingProperty = DependencyProperty.Register(
            "Mapping", typeof(Func<object, RectangleItem>), typeof(RectangleSeries), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies this <see cref="ColorAxisKeyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorAxisKeyProperty = DependencyProperty.Register(
            "ColorAxisKey",
            typeof(string),
            typeof(RectangleSeries),
            new PropertyMetadata(default(string)));

        /// <summary>
        /// Initializes static members of the <see cref="RectangleSeries"/> class.
        /// </summary>
        static RectangleSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(RectangleSeries), new PropertyMetadata(OxyPlot.Series.RectangleSeries.DefaultTrackerFormatString, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "RectangleSeries" /> class.
        /// </summary>
        public RectangleSeries()
        {
            this.InternalSeries = new OxyPlot.Series.RectangleSeries();
        }

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
        public Func<object, RectangleItem> Mapping
        {
            get
            {
                return (Func<object, RectangleItem>)this.GetValue(MappingProperty);
            }

            set
            {
                this.SetValue(MappingProperty, value);
            }
        }        

        /// <summary>
        /// Gets or sets ColorAxisKey property.
        /// </summary>
        public string ColorAxisKey
        {
            get
            {
                return (string)this.GetValue(ColorAxisKeyProperty);
            }

            set
            {
                this.SetValue(ColorAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// The <see cref="Series"/>.
        /// </returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.RectangleSeries)series;
            s.ItemsSource = this.ItemsSource;
            s.CanTrackerInterpolatePoints = this.CanTrackerInterpolatePoints;
            s.Mapping = this.Mapping;
            s.ColorAxisKey = this.ColorAxisKey;
        }
    }
}