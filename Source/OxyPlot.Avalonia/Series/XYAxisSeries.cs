// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XYAxisSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Abstract base class for series that use X and Y axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{

    /// <summary>
    /// Abstract base class for series that use X and Y axes.
    /// </summary>
    public abstract class XYAxisSeries : ItemsSeries
    {
        /// <summary>
        /// Identifies the <see cref="XAxisKey"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> XAxisKeyProperty = AvaloniaProperty.Register<XYAxisSeries, string>(nameof(XAxisKey), null);

        /// <summary>
        /// Identifies the <see cref="YAxisKey"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> YAxisKeyProperty = AvaloniaProperty.Register<XYAxisSeries, string>(nameof(YAxisKey), null);

        /// <summary>
        /// Initializes static members of the <see cref="XYAxisSeries"/> class.
        /// </summary>
        static XYAxisSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(XYAxisSeries), new StyledPropertyMetadata<string>(OxyPlot.Series.XYAxisSeries.DefaultTrackerFormatString));
            XAxisKeyProperty.Changed.AddClassHandler<XYAxisSeries>(AppearanceChanged);
            YAxisKeyProperty.Changed.AddClassHandler<XYAxisSeries>(AppearanceChanged);
            TrackerFormatStringProperty.Changed.AddClassHandler<XYAxisSeries>(AppearanceChanged);
        }

        /// <summary>
        /// Gets or sets the x-axis key.
        /// </summary>
        public string XAxisKey
        {
            get
            {
                return GetValue(XAxisKeyProperty);
            }

            set
            {
                SetValue(XAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the y axis key.
        /// </summary>
        public string YAxisKey
        {
            get
            {
                return GetValue(YAxisKeyProperty);
            }

            set
            {
                SetValue(YAxisKeyProperty, value);
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
            s.XAxisKey = XAxisKey;
            s.YAxisKey = YAxisKey;
        }
    }
}