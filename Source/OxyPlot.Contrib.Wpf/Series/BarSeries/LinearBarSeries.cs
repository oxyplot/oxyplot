// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearBarSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.Series.LinearBarSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.Series.LinearBarSeries
    /// </summary>
    public class LinearBarSeries : DataPointSeries
    {
        /// <summary>
        /// Identifies the <see cref="BarWidth" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty BarWidthProperty = DependencyProperty.Register("BarWidth", typeof(double), typeof(LinearBarSeries), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(LinearBarSeries), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeColor" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register("StrokeColor", typeof(Color), typeof(LinearBarSeries), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(LinearBarSeries), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="NegativeFillColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NegativeFillColorProperty = DependencyProperty.Register("NegativeFillColor", typeof(Color), typeof(LinearBarSeries), new PropertyMetadata(MoreColors.Undefined, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="NegativeStrokeColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NegativeStrokeColorProperty = DependencyProperty.Register("NegativeStrokeColor", typeof(Color), typeof(LinearBarSeries), new PropertyMetadata(MoreColors.Undefined, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearBarSeries" /> class.
        /// </summary>
        public LinearBarSeries()
        {
            this.InternalSeries = new OxyPlot.Series.LinearBarSeries();
        }

        /// <summary>
        /// Gets or sets the bar width.
        /// </summary>
        public double BarWidth
        {
            get
            {
                return (double)this.GetValue(BarWidthProperty);
            }

            set
            {
                this.SetValue(BarWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public Color FillColor
        {
            get
            {
                return (Color)this.GetValue(FillColorProperty);
            }

            set
            {
                this.SetValue(FillColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return (Color)this.GetValue(StrokeColorProperty);
            }

            set
            {
                this.SetValue(StrokeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return (double)this.GetValue(StrokeThicknessProperty);
            }

            set
            {
                this.SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the negative fill color.
        /// </summary>
        public Color NegativeFillColor
        {
            get
            {
                return (Color)this.GetValue(NegativeFillColorProperty);
            }

            set
            {
                this.SetValue(NegativeFillColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the negative stroke color.
        /// </summary>
        public Color NegativeStrokeColor
        {
            get
            {
                return (Color)this.GetValue(NegativeStrokeColorProperty);
            }

            set
            {
                this.SetValue(NegativeStrokeColorProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>The internal series.</returns>
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
            var s = (OxyPlot.Series.LinearBarSeries)series;
            s.BarWidth = this.BarWidth;
            s.FillColor = this.FillColor.ToOxyColor();
            s.StrokeColor = this.StrokeColor.ToOxyColor();
            s.StrokeThickness = this.StrokeThickness;
            s.NegativeFillColor = this.NegativeFillColor.ToOxyColor();
            s.NegativeStrokeColor = this.NegativeStrokeColor.ToOxyColor();
        }
    }
}