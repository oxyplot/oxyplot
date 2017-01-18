// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearBarSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.Series.LinearBarSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.Series.LinearBarSeries
    /// </summary>
    public class LinearBarSeries : DataPointSeries
    {
        /// <summary>
        /// Identifies the <see cref="BarWidth" /> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> BarWidthProperty = AvaloniaProperty.Register<LinearBarSeries, double>(nameof(BarWidth), 1.0);

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> FillColorProperty = AvaloniaProperty.Register<LinearBarSeries, Color>(nameof(FillColor), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="StrokeColor" /> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> StrokeColorProperty = AvaloniaProperty.Register<LinearBarSeries, Color>(nameof(StrokeColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<LinearBarSeries, double>(nameof(StrokeThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="NegativeFillColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> NegativeFillColorProperty = AvaloniaProperty.Register<LinearBarSeries, Color>(nameof(NegativeFillColor), MoreColors.Undefined);

        /// <summary>
        /// Identifies the <see cref="NegativeStrokeColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> NegativeStrokeColorProperty = AvaloniaProperty.Register<LinearBarSeries, Color>(nameof(NegativeStrokeColor), MoreColors.Undefined);

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearBarSeries" /> class.
        /// </summary>
        public LinearBarSeries()
        {
            InternalSeries = new OxyPlot.Series.LinearBarSeries();
        }

        /// <summary>
        /// Gets or sets the bar width.
        /// </summary>
        public double BarWidth
        {
            get
            {
                return GetValue(BarWidthProperty);
            }

            set
            {
                SetValue(BarWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public Color FillColor
        {
            get
            {
                return GetValue(FillColorProperty);
            }

            set
            {
                SetValue(FillColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return GetValue(StrokeColorProperty);
            }

            set
            {
                SetValue(StrokeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return GetValue(StrokeThicknessProperty);
            }

            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the negative fill color.
        /// </summary>
        public Color NegativeFillColor
        {
            get
            {
                return GetValue(NegativeFillColorProperty);
            }

            set
            {
                SetValue(NegativeFillColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the negative stroke color.
        /// </summary>
        public Color NegativeStrokeColor
        {
            get
            {
                return GetValue(NegativeStrokeColorProperty);
            }

            set
            {
                SetValue(NegativeStrokeColorProperty, value);
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
            var s = (OxyPlot.Series.LinearBarSeries)series;
            s.BarWidth = BarWidth;
            s.FillColor = FillColor.ToOxyColor();
            s.StrokeColor = StrokeColor.ToOxyColor();
            s.StrokeThickness = StrokeThickness;
            s.NegativeFillColor = NegativeFillColor.ToOxyColor();
            s.NegativeStrokeColor = NegativeStrokeColor.ToOxyColor();
        }

        static LinearBarSeries()
        {
            BarWidthProperty.Changed.AddClassHandler<LinearBarSeries>(AppearanceChanged);
            FillColorProperty.Changed.AddClassHandler<LinearBarSeries>(AppearanceChanged);
            StrokeColorProperty.Changed.AddClassHandler<LinearBarSeries>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<LinearBarSeries>(AppearanceChanged);
            NegativeFillColorProperty.Changed.AddClassHandler<LinearBarSeries>(AppearanceChanged);
            NegativeStrokeColorProperty.Changed.AddClassHandler<LinearBarSeries>(AppearanceChanged);
        }
    }
}