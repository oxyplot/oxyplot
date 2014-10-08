// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XYAxisSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Abstract base class for series that use X and Y axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// Abstract base class for series that use X and Y axes.
    /// </summary>
    public abstract class XYAxisSeries : ItemsSeries
    {
        /// <summary>
        /// Identifies the <see cref="XAxisKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisKeyProperty = DependencyProperty.Register(
            "XAxisKey", typeof(string), typeof(XYAxisSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="YAxisKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisKeyProperty = DependencyProperty.Register(
            "YAxisKey", typeof(string), typeof(XYAxisSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref="XYAxisSeries"/> class.
        /// </summary>
        static XYAxisSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(XYAxisSeries), new PropertyMetadata(OxyPlot.Series.XYAxisSeries.DefaultTrackerFormatString, AppearanceChanged));
        }

        /// <summary>
        /// Gets or sets the x-axis key.
        /// </summary>
        public string XAxisKey
        {
            get
            {
                return (string)this.GetValue(XAxisKeyProperty);
            }

            set
            {
                this.SetValue(XAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the y axis key.
        /// </summary>
        public string YAxisKey
        {
            get
            {
                return (string)this.GetValue(YAxisKeyProperty);
            }

            set
            {
                this.SetValue(YAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.XYAxisSeries)series;
            s.XAxisKey = this.XAxisKey;
            s.YAxisKey = this.YAxisKey;
        }
    }
}