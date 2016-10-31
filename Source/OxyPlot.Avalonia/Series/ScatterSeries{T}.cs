// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterSeries{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a base class for scatter series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;
    using OxyPlot.Series;
    using System;

    /// <summary>
    /// Provides a base class for scatter series.
    /// </summary>
    /// <typeparam name="T">The type of the points.</typeparam>
    public abstract class ScatterSeries<T> : XYAxisSeries where T : ScatterPoint
    {
        /// <summary>
        /// Identifies the <see cref="BinSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<int> BinSizeProperty = AvaloniaProperty.Register<ScatterSeries<T>, int>(nameof(BinSize), 0);

        /// <summary>
        /// Identifies the <see cref="DataFieldSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldSizeProperty = AvaloniaProperty.Register<ScatterSeries<T>, string>(nameof(DataFieldSize), null);

        /// <summary>
        /// Identifies the <see cref="DataFieldTag"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldTagProperty = AvaloniaProperty.Register<ScatterSeries<T>, string>(nameof(DataFieldTag), null);

        /// <summary>
        /// Identifies the <see cref="DataFieldValue"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldValueProperty = AvaloniaProperty.Register<ScatterSeries<T>, string>(nameof(DataFieldValue), null);

        /// <summary>
        /// Identifies the <see cref="DataFieldX"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldXProperty = AvaloniaProperty.Register<ScatterSeries<T>, string>(nameof(DataFieldX), null);

        /// <summary>
        /// Identifies the <see cref="DataFieldY"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DataFieldYProperty = AvaloniaProperty.Register<ScatterSeries<T>, string>(nameof(DataFieldY), null);

        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Func<object, ScatterPoint>> MappingProperty = AvaloniaProperty.Register<ScatterSeries<T>, Func<object, ScatterPoint>>(nameof(Mapping), null);

        /// <summary>
        /// Identifies the <see cref="MarkerFill"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> MarkerFillProperty = AvaloniaProperty.Register<ScatterSeries<T>, Color>(nameof(MarkerFill), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="MarkerOutline"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<ScreenPoint[]> MarkerOutlineProperty = AvaloniaProperty.Register<ScatterSeries<T>, ScreenPoint[]>(nameof(MarkerOutline), null);

        /// <summary>
        /// Identifies the <see cref="MarkerSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MarkerSizeProperty = AvaloniaProperty.Register<ScatterSeries<T>, double>(nameof(MarkerSize), 5.0);

        /// <summary>
        /// Identifies the <see cref="MarkerStroke"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> MarkerStrokeProperty = AvaloniaProperty.Register<ScatterSeries<T>, Color>(nameof(MarkerStroke), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="MarkerStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MarkerStrokeThicknessProperty = AvaloniaProperty.Register<ScatterSeries<T>, double>(nameof(MarkerStrokeThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="MarkerType"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<MarkerType> MarkerTypeProperty = AvaloniaProperty.Register<ScatterSeries<T>, MarkerType>(nameof(MarkerType), MarkerType.Square);

        /// <summary>
        /// Initializes a new instance of the <see cref = "ScatterSeries{T}" /> class.
        /// </summary>
        protected ScatterSeries()
        {
            InternalSeries = new OxyPlot.Series.ScatterSeries();
        }

        /// <summary>
        /// Gets or sets bin size.
        /// </summary>
        public int BinSize
        {
            get
            {
                return GetValue(BinSizeProperty);
            }

            set
            {
                SetValue(BinSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets size data field.
        /// </summary>
        public string DataFieldSize
        {
            get
            {
                return GetValue(DataFieldSizeProperty);
            }

            set
            {
                SetValue(DataFieldSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets tag data field.
        /// </summary>
        public string DataFieldTag
        {
            get
            {
                return GetValue(DataFieldTagProperty);
            }

            set
            {
                SetValue(DataFieldTagProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets value (color) data field.
        /// </summary>
        public string DataFieldValue
        {
            get
            {
                return GetValue(DataFieldValueProperty);
            }

            set
            {
                SetValue(DataFieldValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets X data field.
        /// </summary>
        public string DataFieldX
        {
            get
            {
                return GetValue(DataFieldXProperty);
            }

            set
            {
                SetValue(DataFieldXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Y data field.
        /// </summary>
        public string DataFieldY
        {
            get
            {
                return GetValue(DataFieldYProperty);
            }

            set
            {
                SetValue(DataFieldYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets mapping function.
        /// </summary>
        public Func<object, T> Mapping
        {
            get
            {
                return (Func<object, T>)GetValue(MappingProperty);
            }

            set
            {
                SetValue(MappingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets fill color of the markers.
        /// </summary>
        public Color MarkerFill
        {
            get
            {
                return GetValue(MarkerFillProperty);
            }

            set
            {
                SetValue(MarkerFillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom outline of the markers.
        /// </summary>
        public ScreenPoint[] MarkerOutline
        {
            get
            {
                return GetValue(MarkerOutlineProperty);
            }

            set
            {
                SetValue(MarkerOutlineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the markers.
        /// </summary>
        public double MarkerSize
        {
            get
            {
                return GetValue(MarkerSizeProperty);
            }

            set
            {
                SetValue(MarkerSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets color of the marker strokes.
        /// </summary>
        public Color MarkerStroke
        {
            get
            {
                return GetValue(MarkerStrokeProperty);
            }

            set
            {
                SetValue(MarkerStrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets thickness of the marker strokes.
        /// </summary>
        public double MarkerStrokeThickness
        {
            get
            {
                return GetValue(MarkerStrokeThicknessProperty);
            }

            set
            {
                SetValue(MarkerStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets type of the markers.
        /// </summary>
        public MarkerType MarkerType
        {
            get
            {
                return GetValue(MarkerTypeProperty);
            }

            set
            {
                SetValue(MarkerTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color axis key.
        /// </summary>
        /// <value>The color axis key.</value>
        public string ColorAxisKey { get; set; }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>The series.</returns>
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
            var s = (OxyPlot.Series.ScatterSeries<T>)series;
            s.MarkerFill = MarkerFill.ToOxyColor();
            s.MarkerStroke = MarkerStroke.ToOxyColor();
            s.MarkerStrokeThickness = MarkerStrokeThickness;
            s.MarkerType = MarkerType;
            s.MarkerSize = MarkerSize;
            s.DataFieldX = DataFieldX;
            s.DataFieldY = DataFieldY;
            s.DataFieldSize = DataFieldSize;
            s.DataFieldValue = DataFieldValue;
            s.DataFieldTag = DataFieldTag;
            s.ItemsSource = Items;
            s.BinSize = BinSize;
            s.Mapping = Mapping;
            s.MarkerOutline = MarkerOutline;
            s.ColorAxisKey = ColorAxisKey;
        }

        static ScatterSeries()
        {
            BinSizeProperty.Changed.AddClassHandler<ScatterSeries<T>>(AppearanceChanged);
            DataFieldSizeProperty.Changed.AddClassHandler<ScatterSeries<T>>(DataChanged);
            DataFieldTagProperty.Changed.AddClassHandler<ScatterSeries<T>>(DataChanged);
            DataFieldValueProperty.Changed.AddClassHandler<ScatterSeries<T>>(DataChanged);
            DataFieldXProperty.Changed.AddClassHandler<ScatterSeries<T>>(DataChanged);
            DataFieldYProperty.Changed.AddClassHandler<ScatterSeries<T>>(DataChanged);
            MappingProperty.Changed.AddClassHandler<ScatterSeries<T>>(DataChanged);
            MarkerFillProperty.Changed.AddClassHandler<ScatterSeries<T>>(AppearanceChanged);
            MarkerOutlineProperty.Changed.AddClassHandler<ScatterSeries<T>>(AppearanceChanged);
            MarkerSizeProperty.Changed.AddClassHandler<ScatterSeries<T>>(AppearanceChanged);
            MarkerStrokeProperty.Changed.AddClassHandler<ScatterSeries<T>>(AppearanceChanged);
            MarkerStrokeThicknessProperty.Changed.AddClassHandler<ScatterSeries<T>>(AppearanceChanged);
            MarkerTypeProperty.Changed.AddClassHandler<ScatterSeries<T>>(AppearanceChanged);
        }
    }
}