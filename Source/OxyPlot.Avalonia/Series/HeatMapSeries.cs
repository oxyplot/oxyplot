// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatMapSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   HeatMapSeries Avalonia wrapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// HeatMapSeries Avalonia wrapper
    /// </summary>
    public class HeatMapSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies this <see cref="DataProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double[,]> DataProperty = AvaloniaProperty.Register<HeatMapSeries, double[,]>(nameof(Data), new double[0, 0], validate: (obj, val) =>
        {
            if (val == null)
            {
                throw new System.ArgumentException();
            }

            return val;
        });

        /// <summary>
        /// Identifies this <see cref="X0Property"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> X0Property = AvaloniaProperty.Register<HeatMapSeries, double>(nameof(X0), default(double));

        /// <summary>
        /// Identifies this <see cref="X1Property"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> X1Property = AvaloniaProperty.Register<HeatMapSeries, double>(nameof(X1), default(double));

        /// <summary>
        /// Identifies this <see cref="Y0Property"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> Y0Property = AvaloniaProperty.Register<HeatMapSeries, double>(nameof(Y0), default(double));

        /// <summary>
        /// Identifies this <see cref="Y1Property"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> Y1Property = AvaloniaProperty.Register<HeatMapSeries, double>(nameof(Y1), default(double));

        /// <summary>
        /// Identifies this <see cref="ColorAxisKeyProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> ColorAxisKeyProperty = AvaloniaProperty.Register<HeatMapSeries, string>(nameof(ColorAxisKey), default(string));

        /// <summary>
        /// Identifies this <see cref="LowColorProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> LowColorProperty = AvaloniaProperty.Register<HeatMapSeries, Color>(nameof(LowColor), default(Color));

        /// <summary>
        /// Identifies this <see cref="HighColorProperty"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> HighColorProperty = AvaloniaProperty.Register<HeatMapSeries, Color>(nameof(HighColor), default(Color));

        /// <summary>
        /// Initializes static members of the <see cref="HeatMapSeries"/> class.
        /// </summary>
        static HeatMapSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(HeatMapSeries), new StyledPropertyMetadata<string>(OxyPlot.Series.HeatMapSeries.DefaultTrackerFormatString));
            DataProperty.Changed.AddClassHandler<HeatMapSeries>(DataChanged);
            X0Property.Changed.AddClassHandler<HeatMapSeries>(AppearanceChanged);
            X1Property.Changed.AddClassHandler<HeatMapSeries>(AppearanceChanged);
            Y0Property.Changed.AddClassHandler<HeatMapSeries>(AppearanceChanged);
            Y1Property.Changed.AddClassHandler<HeatMapSeries>(AppearanceChanged);
            TrackerFormatStringProperty.Changed.AddClassHandler<HeatMapSeries>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "HeatMapSeries" /> class.
        /// </summary>
        public HeatMapSeries()
        {
            Data = new double[0, 0];
            InternalSeries = new OxyPlot.Series.HeatMapSeries { Data = Data };
        }

        /// <summary>
        /// Gets or sets LowColor
        /// </summary>
        public Color LowColor
        {
            get
            {
                return GetValue(LowColorProperty);
            }

            set
            {
                SetValue(LowColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets HighColor
        /// </summary>
        public Color HighColor
        {
            get
            {
                return GetValue(LowColorProperty);
            }

            set
            {
                SetValue(LowColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ColorAxisKey property.
        /// </summary>
        public string ColorAxisKey
        {
            get
            {
                return GetValue(ColorAxisKeyProperty);
            }

            set
            {
                SetValue(ColorAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets X0.
        /// </summary>
        public double X0
        {
            get
            {
                return GetValue(X0Property);
            }

            set
            {
                SetValue(X0Property, value);
            }
        }

        /// <summary>
        /// Gets or sets X1
        /// </summary>
        public double X1
        {
            get
            {
                return GetValue(X1Property);
            }

            set
            {
                SetValue(X1Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Y0
        /// </summary>
        public double Y0
        {
            get
            {
                return GetValue(Y0Property);
            }

            set
            {
                SetValue(Y0Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Y1
        /// </summary>
        public double Y1
        {
            get
            {
                return GetValue(Y1Property);
            }

            set
            {
                SetValue(Y1Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Data
        /// </summary>
        public double[,] Data
        {
            get
            {
                return GetValue(DataProperty);
            }

            set
            {
                SetValue(DataProperty, value);
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
            SynchronizeProperties(InternalSeries);
            return InternalSeries;
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
            s.Data = Data ?? new double[0, 0];
            s.X0 = X0;
            s.X1 = X1;
            s.Y0 = Y0;
            s.Y1 = Y1;
        }
    }
}