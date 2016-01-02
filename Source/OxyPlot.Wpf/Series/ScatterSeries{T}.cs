// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterSeries{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a base class for scatter series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using OxyPlot.Series;

    /// <summary>
    /// Provides a base class for scatter series.
    /// </summary>
    /// <typeparam name="T">The type of the points.</typeparam>
    public abstract class ScatterSeries<T> : XYAxisSeries where T : ScatterPoint
    {
        /// <summary>
        /// Identifies the <see cref="BinSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinSizeProperty = DependencyProperty.Register(
            "BinSize", typeof(int), typeof(ScatterSeries<T>), new PropertyMetadata(0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldSizeProperty = DependencyProperty.Register(
            "DataFieldSize", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldTag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldTagProperty = DependencyProperty.Register(
            "DataFieldTag", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldValueProperty = DependencyProperty.Register(
            "DataFieldValue", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldXProperty = DependencyProperty.Register(
            "DataFieldX", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="DataFieldY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldYProperty = DependencyProperty.Register(
            "DataFieldY", typeof(string), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MappingProperty = DependencyProperty.Register(
            "Mapping", typeof(Func<object, ScatterPoint>), typeof(ScatterSeries<T>), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerFill"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerFillProperty = DependencyProperty.Register(
            "MarkerFill", typeof(Color), typeof(ScatterSeries<T>), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerOutline"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerOutlineProperty = DependencyProperty.Register(
            "MarkerOutline", typeof(ScreenPoint[]), typeof(ScatterSeries<T>), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register(
            "MarkerSize", typeof(double), typeof(ScatterSeries<T>), new PropertyMetadata(5.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerStroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeProperty = DependencyProperty.Register(
            "MarkerStroke", typeof(Color), typeof(ScatterSeries<T>), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeThicknessProperty =
            DependencyProperty.Register(
                "MarkerStrokeThickness",
                typeof(double),
                typeof(ScatterSeries<T>),
                new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register(
            "MarkerType",
            typeof(MarkerType),
            typeof(ScatterSeries<T>),
            new PropertyMetadata(MarkerType.Square, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "ScatterSeries{T}" /> class.
        /// </summary>
        protected ScatterSeries()
        {
            this.InternalSeries = new OxyPlot.Series.ScatterSeries();
        }

        /// <summary>
        /// Gets or sets bin size.
        /// </summary>
        public int BinSize
        {
            get
            {
                return (int)this.GetValue(BinSizeProperty);
            }

            set
            {
                this.SetValue(BinSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets size data field.
        /// </summary>
        public string DataFieldSize
        {
            get
            {
                return (string)this.GetValue(DataFieldSizeProperty);
            }

            set
            {
                this.SetValue(DataFieldSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets tag data field.
        /// </summary>
        public string DataFieldTag
        {
            get
            {
                return (string)this.GetValue(DataFieldTagProperty);
            }

            set
            {
                this.SetValue(DataFieldTagProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets value (color) data field.
        /// </summary>
        public string DataFieldValue
        {
            get
            {
                return (string)this.GetValue(DataFieldValueProperty);
            }

            set
            {
                this.SetValue(DataFieldValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets X data field.
        /// </summary>
        public string DataFieldX
        {
            get
            {
                return (string)this.GetValue(DataFieldXProperty);
            }

            set
            {
                this.SetValue(DataFieldXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Y data field.
        /// </summary>
        public string DataFieldY
        {
            get
            {
                return (string)this.GetValue(DataFieldYProperty);
            }

            set
            {
                this.SetValue(DataFieldYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets mapping function.
        /// </summary>
        public Func<object, T> Mapping
        {
            get
            {
                return (Func<object, T>)this.GetValue(MappingProperty);
            }

            set
            {
                this.SetValue(MappingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets fill color of the markers.
        /// </summary>
        public Color MarkerFill
        {
            get
            {
                return (Color)this.GetValue(MarkerFillProperty);
            }

            set
            {
                this.SetValue(MarkerFillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom outline of the markers.
        /// </summary>
        public ScreenPoint[] MarkerOutline
        {
            get
            {
                return (ScreenPoint[])this.GetValue(MarkerOutlineProperty);
            }

            set
            {
                this.SetValue(MarkerOutlineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the markers.
        /// </summary>
        public double MarkerSize
        {
            get
            {
                return (double)this.GetValue(MarkerSizeProperty);
            }

            set
            {
                this.SetValue(MarkerSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets color of the marker strokes.
        /// </summary>
        public Color MarkerStroke
        {
            get
            {
                return (Color)this.GetValue(MarkerStrokeProperty);
            }

            set
            {
                this.SetValue(MarkerStrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets thickness of the marker strokes.
        /// </summary>
        public double MarkerStrokeThickness
        {
            get
            {
                return (double)this.GetValue(MarkerStrokeThicknessProperty);
            }

            set
            {
                this.SetValue(MarkerStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets type of the markers.
        /// </summary>
        public MarkerType MarkerType
        {
            get
            {
                return (MarkerType)this.GetValue(MarkerTypeProperty);
            }

            set
            {
                this.SetValue(MarkerTypeProperty, value);
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
            var s = (OxyPlot.Series.ScatterSeries<T>)series;
            s.MarkerFill = this.MarkerFill.ToOxyColor();
            s.MarkerStroke = this.MarkerStroke.ToOxyColor();
            s.MarkerStrokeThickness = this.MarkerStrokeThickness;
            s.MarkerType = this.MarkerType;
            s.MarkerSize = this.MarkerSize;
            s.DataFieldX = this.DataFieldX;
            s.DataFieldY = this.DataFieldY;
            s.DataFieldSize = this.DataFieldSize;
            s.DataFieldValue = this.DataFieldValue;
            s.DataFieldTag = this.DataFieldTag;
            s.ItemsSource = this.ItemsSource;
            s.BinSize = this.BinSize;
            s.Mapping = this.Mapping;
            s.MarkerOutline = this.MarkerOutline;
            s.ColorAxisKey = this.ColorAxisKey;
        }
    }
}