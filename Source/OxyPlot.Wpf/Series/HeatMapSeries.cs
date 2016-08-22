// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatMapSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   HeatMapSeries WPF wrapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// HeatMapSeries WPF wrapper
    /// </summary>
    public class HeatMapSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies this <see cref="DataProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data",
            typeof(double[,]),
            typeof(HeatMapSeries),
            new PropertyMetadata(new double[0, 0], DataChanged),
            value => value != null);

        /// <summary>
        /// Identifies this <see cref="X0Property"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty X0Property = DependencyProperty.Register(
            "X0",
            typeof(double),
            typeof(HeatMapSeries),
            new PropertyMetadata(default(double), AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="X1Property"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty X1Property = DependencyProperty.Register(
            "X1",
            typeof(double),
            typeof(HeatMapSeries),
            new PropertyMetadata(default(double), AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="Y0Property"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Y0Property = DependencyProperty.Register(
            "Y0",
            typeof(double),
            typeof(HeatMapSeries),
            new PropertyMetadata(default(double), AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="Y1Property"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register(
            "Y1",
            typeof(double),
            typeof(HeatMapSeries),
            new PropertyMetadata(default(double), AppearanceChanged));

        /// <summary>
        /// Identifies this <see cref="ColorAxisKeyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorAxisKeyProperty = DependencyProperty.Register(
            "ColorAxisKey",
            typeof(string),
            typeof(HeatMapSeries),
            new PropertyMetadata(default(string)));

        /// <summary>
        /// Identifies this <see cref="LowColorProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LowColorProperty = DependencyProperty.Register(
            "LowColor",
            typeof(Color),
            typeof(HeatMapSeries),
            new PropertyMetadata(default(Color)));

        /// <summary>
        /// Identifies this <see cref="HighColorProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HighColorProperty = DependencyProperty.Register(
            "HighColor",
            typeof(Color),
            typeof(HeatMapSeries),
            new PropertyMetadata(default(Color)));

        /// <summary>
        /// Initializes static members of the <see cref="HeatMapSeries"/> class.
        /// </summary>
        static HeatMapSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(HeatMapSeries), new PropertyMetadata(OxyPlot.Series.HeatMapSeries.DefaultTrackerFormatString, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "HeatMapSeries" /> class.
        /// </summary>
        public HeatMapSeries()
        {
            this.Data = new double[0, 0];
            this.InternalSeries = new OxyPlot.Series.HeatMapSeries { Data = this.Data };
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
        /// Gets or sets X0.
        /// </summary>
        public double X0
        {
            get
            {
                return (double)this.GetValue(X0Property);
            }

            set
            {
                this.SetValue(X0Property, value);
            }
        }

        /// <summary>
        /// Gets or sets X1
        /// </summary>
        public double X1
        {
            get
            {
                return (double)this.GetValue(X1Property);
            }

            set
            {
                this.SetValue(X1Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Y0
        /// </summary>
        public double Y0
        {
            get
            {
                return (double)this.GetValue(Y0Property);
            }

            set
            {
                this.SetValue(Y0Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Y1
        /// </summary>
        public double Y1
        {
            get
            {
                return (double)this.GetValue(Y1Property);
            }

            set
            {
                this.SetValue(Y1Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Data
        /// </summary>
        public double[,] Data
        {
            get
            {
                return (double[,])this.GetValue(DataProperty);
            }

            set
            {
                this.SetValue(DataProperty, value);
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
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);

            var s = (OxyPlot.Series.HeatMapSeries)series;
            s.Data = this.Data ?? new double[0, 0];
            s.X0 = this.X0;
            s.X1 = this.X1;
            s.Y0 = this.Y0;
            s.Y1 = this.Y1;
        }
    }
}