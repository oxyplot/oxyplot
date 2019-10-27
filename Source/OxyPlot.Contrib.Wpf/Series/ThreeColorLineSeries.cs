// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreeColorLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The WPF wrapper for OxyPlot.ThreeColorLineSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The WPF wrapper for OxyPlot.ThreeColorLineSeries.
    /// </summary>
    public class ThreeColorLineSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="ColorLo"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorLoProperty = DependencyProperty.Register(
            "ColorLo", typeof(Color), typeof(ThreeColorLineSeries), new UIPropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ColorHi"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorHiProperty = DependencyProperty.Register(
            "ColorHi", typeof(Color), typeof(ThreeColorLineSeries), new UIPropertyMetadata(Colors.Red, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LimitLo"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LimitLoProperty = DependencyProperty.Register(
            "LimitLo", typeof(double), typeof(ThreeColorLineSeries), new UIPropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LimitHi"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LimitHiProperty = DependencyProperty.Register(
            "LimitHi", typeof(double), typeof(ThreeColorLineSeries), new UIPropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineStyleLo"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyleLoProperty = DependencyProperty.Register(
            "LineStyleLo",
            typeof(LineStyle),
            typeof(ThreeColorLineSeries),
            new UIPropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineStyleHi"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyleHiProperty = DependencyProperty.Register(
            "LineStyleHi",
            typeof(LineStyle),
            typeof(ThreeColorLineSeries),
            new UIPropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "ThreeColorLineSeries" /> class.
        /// </summary>
        public ThreeColorLineSeries()
        {
            this.InternalSeries = new OxyPlot.Series.ThreeColorLineSeries();
        }

        /// <summary>
        /// Gets or sets ColorLo.
        /// </summary>
        public Color ColorLo
        {
            get
            {
                return (Color)this.GetValue(ColorLoProperty);
            }

            set
            {
                this.SetValue(ColorLoProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ColorHi.
        /// </summary>
        public Color ColorHi
        {
            get
            {
                return (Color)this.GetValue(ColorHiProperty);
            }

            set
            {
                this.SetValue(ColorHiProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LimitLo.
        /// </summary>
        public double LimitLo
        {
            get
            {
                return (double)this.GetValue(LimitLoProperty);
            }

            set
            {
                this.SetValue(LimitLoProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LimitHi.
        /// </summary>
        public double LimitHi
        {
            get
            {
                return (double)this.GetValue(LimitHiProperty);
            }

            set
            {
                this.SetValue(LimitHiProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyleLo.
        /// </summary>
        public LineStyle LineStyleLo
        {
            get
            {
                return (LineStyle)this.GetValue(LineStyleLoProperty);
            }

            set
            {
                this.SetValue(LineStyleLoProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyleHi.
        /// </summary>
        public LineStyle LineStyleHi
        {
            get
            {
                return (LineStyle)this.GetValue(LineStyleHiProperty);
            }

            set
            {
                this.SetValue(LineStyleHiProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.ThreeColorLineSeries)series;
            s.LimitLo = this.LimitLo;
            s.ColorLo = this.ColorLo.ToOxyColor();
            s.LimitHi = this.LimitHi;
            s.ColorHi = this.ColorHi.ToOxyColor();
        }
    }
}