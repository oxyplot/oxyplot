// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.AreaSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AreaSeries
    /// </summary>
    public class AreaSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="Color2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Color2Property = DependencyProperty.Register(
            "Color2", typeof(Color), typeof(Series), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ConstantY2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConstantY2Property = DependencyProperty.Register(
            "ConstantY2", typeof(double), typeof(AreaSeries), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldX2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldX2Property = DependencyProperty.Register(
            "DataFieldX2", typeof(string), typeof(AreaSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldY2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldY2Property = DependencyProperty.Register(
            "DataFieldY2", typeof(string), typeof(AreaSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Fill"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Color), typeof(AreaSeries), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Reverse2"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Reverse2Property = DependencyProperty.Register(
            "Reverse2", typeof(bool), typeof(AreaSeries), new PropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "AreaSeries" /> class.
        /// </summary>
        public AreaSeries()
        {
            this.InternalSeries = new OxyPlot.Series.AreaSeries();
        }

        /// <summary>
        /// Gets or sets the color of the second line.
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
        /// Gets or sets ConstantY2.
        /// </summary>
        public double ConstantY2
        {
            get
            {
                return (double)this.GetValue(ConstantY2Property);
            }

            set
            {
                this.SetValue(ConstantY2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldX2.
        /// </summary>
        public string DataFieldX2
        {
            get
            {
                return (string)this.GetValue(DataFieldX2Property);
            }

            set
            {
                this.SetValue(DataFieldX2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldY2.
        /// </summary>
        public string DataFieldY2
        {
            get
            {
                return (string)this.GetValue(DataFieldY2Property);
            }

            set
            {
                this.SetValue(DataFieldY2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Fill.
        /// </summary>
        public Color Fill
        {
            get
            {
                return (Color)this.GetValue(FillProperty);
            }

            set
            {
                this.SetValue(FillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Reverse2.
        /// </summary>
        public bool Reverse2
        {
            get
            {
                return (bool)this.GetValue(Reverse2Property);
            }

            set
            {
                this.SetValue(Reverse2Property, value);
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
            var s = (OxyPlot.Series.AreaSeries)series;
            s.Color2 = this.Color2.ToOxyColor();
            s.DataFieldX2 = this.DataFieldX2;
            s.DataFieldY2 = this.DataFieldY2;
            s.ConstantY2 = this.ConstantY2;
            s.Fill = this.Fill.ToOxyColor();
            s.Reverse2 = this.Reverse2;
        }
    }
}