// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The WPF wrapper for OxyPlot.TwoColorLineSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The WPF wrapper for OxyPlot.TwoColorLineSeries.
    /// </summary>
    public class TwoColorLineSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="Color2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Color2Property = DependencyProperty.Register(
            "Color2", typeof(Color), typeof(TwoColorLineSeries), new UIPropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Limit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LimitProperty = DependencyProperty.Register(
            "Limit", typeof(double), typeof(TwoColorLineSeries), new UIPropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineStyle2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyle2Property = DependencyProperty.Register(
            "LineStyle2",
            typeof(LineStyle),
            typeof(TwoColorLineSeries),
            new UIPropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "TwoColorLineSeries" /> class.
        /// </summary>
        public TwoColorLineSeries()
        {
            this.InternalSeries = new OxyPlot.Series.TwoColorLineSeries();
        }

        /// <summary>
        /// Gets or sets Color2.
        /// </summary>
        public Color Color2
        {
            get
            {
                return (Color)this.GetValue(Color2Property);
            }

            set
            {
                this.SetValue(Color2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Limit.
        /// </summary>
        public double Limit
        {
            get
            {
                return (double)this.GetValue(LimitProperty);
            }

            set
            {
                this.SetValue(LimitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyle2.
        /// </summary>
        public LineStyle LineStyle2
        {
            get
            {
                return (LineStyle)this.GetValue(LineStyle2Property);
            }

            set
            {
                this.SetValue(LineStyle2Property, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.TwoColorLineSeries)series;
            s.Limit = this.Limit;
            s.Color2 = this.Color2.ToOxyColor();
        }
    }
}