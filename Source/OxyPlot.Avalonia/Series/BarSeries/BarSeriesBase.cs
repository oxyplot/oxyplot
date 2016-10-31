// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.BarSeriesBase
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    using OxyPlot.Series;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.BarSeriesBase
    /// </summary>
    public class BarSeriesBase : CategorizedSeries
    {
        /// <summary>
        /// Identifies the <see cref="BaseValue"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> BaseValueProperty = AvaloniaProperty.Register<BarSeriesBase, double>(nameof(BaseValue), 0.0);

        /// <summary>
        /// Identifies the <see cref="ColorField"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> ColorFieldProperty = AvaloniaProperty.Register<BarSeriesBase, string>(nameof(ColorField), null);

        /// <summary>
        /// Identifies the <see cref="FillColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> FillColorProperty = AvaloniaProperty.Register<BarSeriesBase, Color>(nameof(FillColor), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="IsStacked"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> IsStackedProperty = AvaloniaProperty.Register<BarSeriesBase, bool>(nameof(IsStacked), false);

        /// <summary>
        /// Identifies the <see cref="LabelFormatString"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LabelFormatStringProperty = AvaloniaProperty.Register<BarSeriesBase, string>(nameof(LabelFormatString), null);

        /// <summary>
        /// Identifies the <see cref="LabelMargin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LabelMarginProperty = AvaloniaProperty.Register<BarSeriesBase, double>(nameof(LabelMargin), 2.0);

        /// <summary>
        /// Identifies the <see cref="LabelPlacement"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LabelPlacement> LabelPlacementProperty = AvaloniaProperty.Register<BarSeriesBase, LabelPlacement>(nameof(LabelPlacement), LabelPlacement.Outside);

        /// <summary>
        /// Identifies the <see cref="NegativeFillColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> NegativeFillColorProperty = AvaloniaProperty.Register<BarSeriesBase, Color>(nameof(NegativeFillColor), MoreColors.Undefined);

        /// <summary>
        /// Identifies the <see cref="StackGroup"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> StackGroupProperty = AvaloniaProperty.Register<BarSeriesBase, string>(nameof(StackGroup), string.Empty);

        /// <summary>
        /// Identifies the <see cref="StrokeColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> StrokeColorProperty = AvaloniaProperty.Register<BarSeriesBase, Color>(nameof(StrokeColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<BarSeriesBase, double>(nameof(StrokeThickness), 0.0);

        /// <summary>
        /// Identifies the <see cref="ValueField"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> ValueFieldProperty = AvaloniaProperty.Register<BarSeriesBase, string>(nameof(ValueField), null);

        /// <summary>
        /// Initializes static members of the <see cref="BarSeriesBase" /> class.
        /// </summary>
        static BarSeriesBase()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(BarSeriesBase), new StyledPropertyMetadata<string>(OxyPlot.Series.BarSeriesBase.DefaultTrackerFormatString));
            BaseValueProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            ColorFieldProperty.Changed.AddClassHandler<BarSeriesBase>(DataChanged);
            FillColorProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            IsStackedProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            LabelFormatStringProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            LabelMarginProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            LabelPlacementProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            NegativeFillColorProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            StackGroupProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            StrokeColorProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            ValueFieldProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
            TrackerFormatStringProperty.Changed.AddClassHandler<BarSeriesBase>(AppearanceChanged);
        }

        /// <summary>
        /// Gets or sets BaseValue.
        /// </summary>
        public double BaseValue
        {
            get
            {
                return GetValue(BaseValueProperty);
            }

            set
            {
                SetValue(BaseValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color field.
        /// </summary>
        /// <value>The color field.</value>
        public string ColorField
        {
            get
            {
                return GetValue(ColorFieldProperty);
            }

            set
            {
                SetValue(ColorFieldProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the fill color.
        /// </summary>
        /// <value>The color of the fill color.</value>
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
        /// Gets or sets a value indicating whether the series is stacked.
        /// </summary>
        public bool IsStacked
        {
            get
            {
                return GetValue(IsStackedProperty);
            }

            set
            {
                SetValue(IsStackedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label format string.
        /// </summary>
        /// <value>The label format string.</value>
        public string LabelFormatString
        {
            get
            {
                return GetValue(LabelFormatStringProperty);
            }

            set
            {
                SetValue(LabelFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label margin.
        /// </summary>
        /// <value>The label margin.</value>
        public double LabelMargin
        {
            get
            {
                return GetValue(LabelMarginProperty);
            }

            set
            {
                SetValue(LabelMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label placement.
        /// </summary>
        /// <value>The label placement.</value>
        public LabelPlacement LabelPlacement
        {
            get
            {
                return GetValue(LabelPlacementProperty);
            }

            set
            {
                SetValue(LabelPlacementProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets NegativeFillColor.
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
        /// Gets or sets the stack group.
        /// </summary>
        /// <value>The stack group.</value>
        public string StackGroup
        {
            get
            {
                return GetValue(StackGroupProperty);
            }

            set
            {
                SetValue(StackGroupProperty, value);
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
        /// Gets or sets the value field.
        /// </summary>
        public string ValueField
        {
            get
            {
                return GetValue(ValueFieldProperty);
            }

            set
            {
                SetValue(ValueFieldProperty, value);
            }
        }

        /// <summary>
        /// Creates the model.
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
            var s = (OxyPlot.Series.BarSeriesBase)series;
            s.BaseValue = BaseValue;
            s.ColorField = ColorField;
            s.FillColor = FillColor.ToOxyColor();
            s.IsStacked = IsStacked;
            s.NegativeFillColor = NegativeFillColor.ToOxyColor();
            s.StrokeColor = StrokeColor.ToOxyColor();
            s.StrokeThickness = StrokeThickness;
            s.StackGroup = StackGroup;
            s.ValueField = ValueField;
            s.LabelFormatString = LabelFormatString;
            s.LabelMargin = LabelMargin;
            s.LabelPlacement = LabelPlacement;
        }
    }
}