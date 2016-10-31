// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The Avalonia wrapper for OxyPlot.TwoColorLineSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// The Avalonia wrapper for OxyPlot.TwoColorLineSeries.
    /// </summary>
    public class TwoColorLineSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="Color2"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> Color2Property = AvaloniaProperty.Register<TwoColorLineSeries, Color>(nameof(Color2), Colors.Blue);

        /// <summary>
        /// Identifies the <see cref="Limit"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LimitProperty = AvaloniaProperty.Register<TwoColorLineSeries, double>(nameof(Limit), 0.0);

        /// <summary>
        /// Identifies the <see cref="LineStyle2"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyle2Property = AvaloniaProperty.Register<TwoColorLineSeries, LineStyle>(nameof(LineStyle2), LineStyle.Solid);

        /// <summary>
        /// Initializes a new instance of the <see cref = "TwoColorLineSeries" /> class.
        /// </summary>
        public TwoColorLineSeries()
        {
            InternalSeries = new OxyPlot.Series.TwoColorLineSeries();
        }

        /// <summary>
        /// Gets or sets Color2.
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
        /// Gets or sets Limit.
        /// </summary>
        public double Limit
        {
            get
            {
                return GetValue(LimitProperty);
            }

            set
            {
                SetValue(LimitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyle2.
        /// </summary>
        public LineStyle LineStyle2
        {
            get
            {
                return GetValue(LineStyle2Property);
            }

            set
            {
                SetValue(LineStyle2Property, value);
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
            s.Limit = Limit;
            s.Color2 = Color2.ToOxyColor();
        }

        static TwoColorLineSeries()
        {
            Color2Property.Changed.AddClassHandler<TwoColorLineSeries>(AppearanceChanged);
            LimitProperty.Changed.AddClassHandler<TwoColorLineSeries>(AppearanceChanged);
            LineStyle2Property.Changed.AddClassHandler<TwoColorLineSeries>(AppearanceChanged);
        }
    }
}