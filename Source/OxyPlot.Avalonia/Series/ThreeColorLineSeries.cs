// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreeColorLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The Avalonia wrapper for OxyPlot.ThreeColorLineSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// The Avalonia wrapper for OxyPlot.ThreeColorLineSeries.
    /// </summary>
    public class ThreeColorLineSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="ColorLo"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> ColorLoProperty = AvaloniaProperty.Register<ThreeColorLineSeries, Color>(nameof(ColorLo), Colors.Blue);

        /// <summary>
        /// Identifies the <see cref="ColorHi"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> ColorHiProperty = AvaloniaProperty.Register<ThreeColorLineSeries, Color>(nameof(ColorHi), Colors.Red);

        /// <summary>
        /// Identifies the <see cref="LimitLo"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LimitLoProperty = AvaloniaProperty.Register<ThreeColorLineSeries, double>(nameof(LimitLo), 0.0);

        /// <summary>
        /// Identifies the <see cref="LimitHi"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LimitHiProperty = AvaloniaProperty.Register<ThreeColorLineSeries, double>(nameof(LimitHi), 0.0);

        /// <summary>
        /// Identifies the <see cref="LineStyleLo"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyleLoProperty = AvaloniaProperty.Register<ThreeColorLineSeries, LineStyle>(nameof(LineStyleLo), LineStyle.Solid);

        /// <summary>
        /// Identifies the <see cref="LineStyleHi"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyleHiProperty = AvaloniaProperty.Register<ThreeColorLineSeries, LineStyle>(nameof(LineStyleHi), LineStyle.Solid);

        /// <summary>
        /// Initializes a new instance of the <see cref = "ThreeColorLineSeries" /> class.
        /// </summary>
        public ThreeColorLineSeries()
        {
            InternalSeries = new OxyPlot.Series.ThreeColorLineSeries();
        }

        /// <summary>
        /// Gets or sets ColorLo.
        /// </summary>
        public Color ColorLo
        {
            get
            {
                return GetValue(ColorLoProperty);
            }

            set
            {
                SetValue(ColorLoProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ColorHi.
        /// </summary>
        public Color ColorHi
        {
            get
            {
                return GetValue(ColorHiProperty);
            }

            set
            {
                SetValue(ColorHiProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LimitLo.
        /// </summary>
        public double LimitLo
        {
            get
            {
                return GetValue(LimitLoProperty);
            }

            set
            {
                SetValue(LimitLoProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LimitHi.
        /// </summary>
        public double LimitHi
        {
            get
            {
                return GetValue(LimitHiProperty);
            }

            set
            {
                SetValue(LimitHiProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyleLo.
        /// </summary>
        public LineStyle LineStyleLo
        {
            get
            {
                return GetValue(LineStyleLoProperty);
            }

            set
            {
                SetValue(LineStyleLoProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyleHi.
        /// </summary>
        public LineStyle LineStyleHi
        {
            get
            {
                return GetValue(LineStyleHiProperty);
            }

            set
            {
                SetValue(LineStyleHiProperty, value);
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
            s.LimitLo = LimitLo;
            s.ColorLo = ColorLo.ToOxyColor();
            s.LimitHi = LimitHi;
            s.ColorHi = ColorHi.ToOxyColor();
        }

        static ThreeColorLineSeries()
        {
            ColorLoProperty.Changed.AddClassHandler<ThreeColorLineSeries>(AppearanceChanged);
            ColorHiProperty.Changed.AddClassHandler<ThreeColorLineSeries>(AppearanceChanged);
            LimitLoProperty.Changed.AddClassHandler<ThreeColorLineSeries>(AppearanceChanged);
            LimitHiProperty.Changed.AddClassHandler<ThreeColorLineSeries>(AppearanceChanged);
            LineStyleLoProperty.Changed.AddClassHandler<ThreeColorLineSeries>(AppearanceChanged);
            LineStyleHiProperty.Changed.AddClassHandler<ThreeColorLineSeries>(AppearanceChanged);
        }
    }
}