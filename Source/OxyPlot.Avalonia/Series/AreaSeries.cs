// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.AreaSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.AreaSeries
    /// </summary>
    public class AreaSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="Color2"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> Color2Property = AvaloniaProperty.Register<Series, Color>(nameof(Color2), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="ConstantY2"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> ConstantY2Property = AvaloniaProperty.Register<AreaSeries, double>(nameof(ConstantY2), 0.0);

        /// <summary>
        /// Identifies the <see cref="DataFieldX2"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldX2Property = AvaloniaProperty.Register<AreaSeries, string>(nameof(DataFieldX2), null);

        /// <summary>
        /// Identifies the <see cref="DataFieldY2"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldY2Property = AvaloniaProperty.Register<AreaSeries, string>(nameof(DataFieldY2), null);

        /// <summary>
        /// Identifies the <see cref="Fill"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> FillProperty = AvaloniaProperty.Register<AreaSeries, Color>(nameof(Fill), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="Reverse2"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> Reverse2Property = AvaloniaProperty.Register<AreaSeries, bool>(nameof(Reverse2), true);

        /// <summary>
        /// Initializes a new instance of the <see cref = "AreaSeries" /> class.
        /// </summary>
        public AreaSeries()
        {
            InternalSeries = new OxyPlot.Series.AreaSeries();
        }

        /// <summary>
        /// Gets or sets the color of the second line.
        /// </summary>
        public Color Color2
        {
            get
            {
                return GetValue(Color2Property);
            }

            set
            {
                SetValue(Color2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets ConstantY2.
        /// </summary>
        public double ConstantY2
        {
            get
            {
                return GetValue(ConstantY2Property);
            }

            set
            {
                SetValue(ConstantY2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldX2.
        /// </summary>
        public string DataFieldX2
        {
            get
            {
                return GetValue(DataFieldX2Property);
            }

            set
            {
                SetValue(DataFieldX2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldY2.
        /// </summary>
        public string DataFieldY2
        {
            get
            {
                return GetValue(DataFieldY2Property);
            }

            set
            {
                SetValue(DataFieldY2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Fill.
        /// </summary>
        public Color Fill
        {
            get
            {
                return GetValue(FillProperty);
            }

            set
            {
                SetValue(FillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Reverse2.
        /// </summary>
        public bool Reverse2
        {
            get
            {
                return GetValue(Reverse2Property);
            }

            set
            {
                SetValue(Reverse2Property, value);
            }
        }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>The internal series.</returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            SynchronizeProperties(InternalSeries);
            return InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.AreaSeries)series;
            s.Color2 = Color2.ToOxyColor();
            s.DataFieldX2 = DataFieldX2;
            s.DataFieldY2 = DataFieldY2;
            s.ConstantY2 = ConstantY2;
            s.Fill = Fill.ToOxyColor();
            s.Reverse2 = Reverse2;
        }

        static AreaSeries()
        {
            Color2Property.Changed.AddClassHandler<AreaSeries>(AppearanceChanged);
            ConstantY2Property.Changed.AddClassHandler<AreaSeries>(DataChanged);
            DataFieldX2Property.Changed.AddClassHandler<AreaSeries>(DataChanged);
            DataFieldY2Property.Changed.AddClassHandler<AreaSeries>(DataChanged);
            FillProperty.Changed.AddClassHandler<AreaSeries>(AppearanceChanged);
            Reverse2Property.Changed.AddClassHandler<AreaSeries>(AppearanceChanged);
        }
    }
}