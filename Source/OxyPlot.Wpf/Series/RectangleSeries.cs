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
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// RectangleSeries WPF wrapper
    /// </summary>
    public class RectangleSeries : DataRectSeries
    {
        /// <summary>
        /// Identifies this <see cref="ColorAxisKeyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorAxisKeyProperty = DependencyProperty.Register(
            "ColorAxisKey",
            typeof(string),
            typeof(RectangleSeries),
            new PropertyMetadata(default(string)));

        /// <summary>
        /// Identifies this <see cref="LowColorProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LowColorProperty = DependencyProperty.Register(
            "LowColor",
            typeof(Color),
            typeof(RectangleSeries),
            new PropertyMetadata(default(Color)));

        /// <summary>
        /// Identifies this <see cref="HighColorProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HighColorProperty = DependencyProperty.Register(
            "HighColor",
            typeof(Color),
            typeof(RectangleSeries),
            new PropertyMetadata(default(Color)));

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
        /// Gets or sets LowColor
        /// </summary>
        public Color LowColor
        {
            get
            {
                return (Color)this.GetValue(LowColorProperty);
            }

            set
            {
                this.SetValue(LowColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets HighColor
        /// </summary>
        public Color HighColor
        {
            get
            {
                return (Color)this.GetValue(LowColorProperty);
            }

            set
            {
                this.SetValue(LowColorProperty, value);
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
    }
}