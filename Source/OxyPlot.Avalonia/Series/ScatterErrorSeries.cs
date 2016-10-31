// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterErrorSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.ScatterErrorSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    using OxyPlot.Series;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.ScatterErrorSeries
    /// </summary>
    public class ScatterErrorSeries : ScatterSeries<ScatterErrorPoint>
    {
        /// <summary>
        /// Identifies the <see cref="DataFieldErrorX"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldErrorXProperty = AvaloniaProperty.Register<ScatterErrorSeries, string>(nameof(DataFieldErrorX), null);

        /// <summary>
        /// Identifies the <see cref="DataFieldErrorY"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldErrorYProperty = AvaloniaProperty.Register<ScatterErrorSeries, string>(nameof(DataFieldErrorY), null);

        /// <summary>
        /// Identifies the <see cref="ErrorBarColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> ErrorBarColorProperty = AvaloniaProperty.Register<ScatterErrorSeries, Color>(nameof(ErrorBarColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="ErrorBarStopWidth"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> ErrorBarStopWidthProperty = AvaloniaProperty.Register<ScatterErrorSeries, double>(nameof(ErrorBarStopWidth), 4.0);

        /// <summary>
        /// Identifies the <see cref="ErrorBarStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> ErrorBarStrokeThicknessProperty = AvaloniaProperty.Register<ScatterErrorSeries, double>(nameof(ErrorBarStrokeThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="MinimumErrorSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumErrorSizeProperty = AvaloniaProperty.Register<ScatterErrorSeries, double>(nameof(MinimumErrorSize), 0d);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterErrorSeries"/> class.
        /// </summary>
        public ScatterErrorSeries()
        {
            InternalSeries = new OxyPlot.Series.ScatterErrorSeries();
        }

        /// <summary>
        /// Gets or sets the data field X error.
        /// </summary>
        /// <value>
        /// The data field error.
        /// </value>
        public string DataFieldErrorX
        {
            get { return GetValue(DataFieldErrorXProperty); }
            set { SetValue(DataFieldErrorXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the data field Y error.
        /// </summary>
        /// <value>
        /// The data field error.
        /// </value>
        public string DataFieldErrorY
        {
            get { return GetValue(DataFieldErrorYProperty); }
            set { SetValue(DataFieldErrorYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the error bar.
        /// </summary>
        /// <value>
        /// The color of the error bar.
        /// </value>
        public Color ErrorBarColor
        {
            get { return GetValue(ErrorBarColorProperty); }
            set { SetValue(ErrorBarColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the error bar stop.
        /// </summary>
        /// <value>
        /// The width of the error bar stop.
        /// </value>
        public double ErrorBarStopWidth
        {
            get { return GetValue(ErrorBarStopWidthProperty); }
            set { SetValue(ErrorBarStopWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the error bar stroke thickness.
        /// </summary>
        /// <value>
        /// The error bar stroke thickness.
        /// </value>
        public double ErrorBarStrokeThickness
        {
            get { return GetValue(ErrorBarStrokeThicknessProperty); }
            set { SetValue(ErrorBarStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum size (relative to <see cref="ScatterSeries{T}.MarkerSize" />) of the error bars to be shown. 
        /// </summary>
        public double MinimumErrorSize
        {
            get { return GetValue(MinimumErrorSizeProperty); }
            set { SetValue(MinimumErrorSizeProperty, value); }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.ScatterErrorSeries)series;
            s.DataFieldErrorX = DataFieldErrorX;
            s.DataFieldErrorY = DataFieldErrorY;
            s.ErrorBarColor = ErrorBarColor.ToOxyColor();
            s.ErrorBarStopWidth = ErrorBarStopWidth;
            s.ErrorBarStrokeThickness = ErrorBarStrokeThickness;
            s.MinimumErrorSize = MinimumErrorSize;
        }

        static ScatterErrorSeries()
        {
            DataFieldErrorXProperty.Changed.AddClassHandler<ScatterErrorSeries>(DataChanged);
            DataFieldErrorYProperty.Changed.AddClassHandler<ScatterErrorSeries>(DataChanged);
            ErrorBarColorProperty.Changed.AddClassHandler<ScatterErrorSeries>(AppearanceChanged);
            ErrorBarStopWidthProperty.Changed.AddClassHandler<ScatterErrorSeries>(AppearanceChanged);
            ErrorBarStrokeThicknessProperty.Changed.AddClassHandler<ScatterErrorSeries>(AppearanceChanged);
            MinimumErrorSizeProperty.Changed.AddClassHandler<ScatterErrorSeries>(AppearanceChanged);
        }
    }
}