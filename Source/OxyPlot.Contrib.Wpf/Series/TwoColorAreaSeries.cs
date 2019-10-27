// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorAreaSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The WPF wrapper for OxyPlot.TwoColorAreaSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The WPF wrapper for OxyPlot.TwoColorAreaSeries.
    /// </summary>
    public class TwoColorAreaSeries : AreaSeries
    {
        /// <summary>
        /// Identifies the <see cref="Dashes2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Dashes2Property = DependencyProperty.Register(
            "Dashes2", typeof(DoubleCollection), typeof(TwoColorAreaSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Fill2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Fill2Property = DependencyProperty.Register(
            "Fill2", typeof(Color), typeof(TwoColorAreaSeries), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineStyle2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyle2Property = DependencyProperty.Register(
            "LineStyle2", typeof(LineStyle), typeof(TwoColorAreaSeries), new PropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Limit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LimitProperty = DependencyProperty.Register(
            "Limit", typeof(double), typeof(TwoColorAreaSeries), new PropertyMetadata(0d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerFill2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerFill2Property = DependencyProperty.Register(
            "MarkerFill2", typeof(Color), typeof(TwoColorAreaSeries), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerStroke2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerStroke2Property = DependencyProperty.Register(
            "MarkerStroke2", typeof(Color), typeof(TwoColorAreaSeries), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "TwoColorAreaSeries" /> class.
        /// </summary>
        public TwoColorAreaSeries()
        {
            this.InternalSeries = new OxyPlot.Series.TwoColorAreaSeries();
        }

        /// <summary>
        /// Gets or sets the dash array for the rendered line that is below the limit (overrides <see cref="LineStyle" />).
        /// </summary>
        public DoubleCollection Dashes2
        {
            get
            {
                return (DoubleCollection)this.GetValue(Dashes2Property);
            }

            set
            {
                this.SetValue(Dashes2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Fill below the limit line.
        /// </summary>
        public Color Fill2
        {
            get
            {
                return (Color)this.GetValue(Fill2Property);
            }

            set
            {
                this.SetValue(Fill2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Marker Fill which is below the limit line.
        /// </summary>
        public Color MarkerFill2
        {
            get
            {
                return (Color)this.GetValue(MarkerFill2Property);
            }

            set
            {
                this.SetValue(MarkerFill2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Marker Stroke which is below the limit line.
        /// </summary>
        public Color MarkerStroke2
        {
            get
            {
                return (Color)this.GetValue(MarkerStroke2Property);
            }

            set
            {
                this.SetValue(MarkerStroke2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets the line style for the part of the line that is below the limit.
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
        /// Gets or sets a baseline for the series.
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
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.TwoColorAreaSeries)series;
            s.Fill = this.Fill.ToOxyColor();
            s.Fill2 = this.Fill2.ToOxyColor();
            s.MarkerFill2 = this.MarkerFill2.ToOxyColor();
            s.MarkerStroke2 = this.MarkerStroke2.ToOxyColor();
            s.Limit = this.Limit;
            s.Dashes2 = this.Dashes2?.ToArray();
            s.LineStyle2 = this.LineStyle2;
        }
    }
}