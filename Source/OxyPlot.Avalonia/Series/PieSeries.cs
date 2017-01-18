// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.LineSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// This is a wrapper of OxyPlot.PieSeries.
    /// </summary>
    public class PieSeries : ItemsSeries
    {
        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> StrokeProperty = AvaloniaProperty.Register<PieSeries, Color>(nameof(Stroke), Colors.White);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(StrokeThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="Diameter"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> DiameterProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(Diameter), 1.0);

        /// <summary>
        /// Identifies the <see cref="InnerDiameter"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> InnerDiameterProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(InnerDiameter), 0.0);

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StartAngleProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(StartAngle), 0.0);

        /// <summary>
        /// Identifies the <see cref="AngleSpan"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AngleSpanProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(AngleSpan), 360.0);

        /// <summary>
        /// Identifies the <see cref="AngleIncrement"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AngleIncrementProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(AngleIncrement), 1.0);

        /// <summary>
        /// Identifies the <see cref="LegendFormat"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LegendFormatProperty = AvaloniaProperty.Register<PieSeries, string>(nameof(LegendFormat), null);

        /// <summary>
        /// Identifies the <see cref="OutsideLabelFormat"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> OutsideLabelFormatProperty = AvaloniaProperty.Register<PieSeries, string>(nameof(OutsideLabelFormat), "{2:0} %");

        /// <summary>
        /// Identifies the <see cref="InsideLabelColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> InsideLabelColorProperty = AvaloniaProperty.Register<PieSeries, Color>(nameof(InsideLabelColor), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="InsideLabelFormat"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> InsideLabelFormatProperty = AvaloniaProperty.Register<PieSeries, string>(nameof(InsideLabelFormat), "{1}");

        /// <summary>
        /// Identifies the <see cref="InsideLabelPosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> InsideLabelPositionProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(InsideLabelPosition), 0.5);

        /// <summary>
        /// Identifies the <see cref="AreInsideLabelsAngled"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> AreInsideLabelsAngledProperty = AvaloniaProperty.Register<PieSeries, bool>(nameof(AreInsideLabelsAngled), false);

        /// <summary>
        /// Identifies the <see cref="TickDistance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TickDistanceProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(TickDistance), 0.0);

        /// <summary>
        /// Identifies the <see cref="TickRadialLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TickRadialLengthProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(TickRadialLength), 6.0);

        /// <summary>
        /// Identifies the <see cref="TickHorizontalLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TickHorizontalLengthProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(TickHorizontalLength), 8.0);

        /// <summary>
        /// Identifies the <see cref="TickLabelDistance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TickLabelDistanceProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(TickLabelDistance), 4.0);

        /// <summary>
        /// Identifies the <see cref="ExplodedDistance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> ExplodedDistanceProperty = AvaloniaProperty.Register<PieSeries, double>(nameof(ExplodedDistance), 0.0);

        /// <summary>
        /// Identifies the <see cref="LabelField"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LabelFieldProperty = AvaloniaProperty.Register<PieSeries, string>(nameof(LabelField), null);

        /// <summary>
        /// Identifies the <see cref="ValueField"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> ValueFieldProperty = AvaloniaProperty.Register<PieSeries, string>(nameof(ValueField), null);

        /// <summary>
        /// Identifies the <see cref="ColorField"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> ColorFieldProperty = AvaloniaProperty.Register<PieSeries, string>(nameof(ColorField), null);

        /// <summary>
        /// Identifies the <see cref="IsExplodedField"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> IsExplodedFieldProperty = AvaloniaProperty.Register<PieSeries, string>(nameof(IsExplodedField), null);

        /// <summary>
        /// Initializes static members of the <see cref="PieSeries"/> class.
        /// </summary>
        static PieSeries()
        {
            TrackerFormatStringProperty
                .OverrideMetadata(
                    typeof(PieSeries),
                    new StyledPropertyMetadata<string>(OxyPlot.Series.PieSeries.DefaultTrackerFormatString));
            StrokeProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            DiameterProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            InnerDiameterProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            StartAngleProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            AngleSpanProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            AngleIncrementProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            LegendFormatProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            OutsideLabelFormatProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            InsideLabelColorProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            InsideLabelFormatProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            InsideLabelPositionProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            AreInsideLabelsAngledProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            TickDistanceProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            TickRadialLengthProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            TickHorizontalLengthProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            TickLabelDistanceProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
            LabelFieldProperty.Changed.AddClassHandler<PieSeries>(DataChanged);
            ValueFieldProperty.Changed.AddClassHandler<PieSeries>(DataChanged);
            ColorFieldProperty.Changed.AddClassHandler<PieSeries>(DataChanged);
            IsExplodedFieldProperty.Changed.AddClassHandler<PieSeries>(DataChanged);
            TrackerFormatStringProperty.Changed.AddClassHandler<PieSeries>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSeries"/> class.
        /// </summary>
        public PieSeries()
        {
            InternalSeries = new OxyPlot.Series.PieSeries();
        }

        /// <summary>
        /// Creates the underlying model.
        /// </summary>
        /// <returns>A series.</returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            SynchronizeProperties(InternalSeries);
            return InternalSeries;
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public Color Stroke
        {
            get { return GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public double Diameter
        {
            get { return GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner diameter.
        /// </summary>
        public double InnerDiameter
        {
            get { return GetValue(InnerDiameterProperty); }
            set { SetValue(InnerDiameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        public double StartAngle
        {
            get { return GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle span.
        /// </summary>
        public double AngleSpan
        {
            get { return GetValue(AngleSpanProperty); }
            set { SetValue(AngleSpanProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle increment.
        /// </summary>
        public double AngleIncrement
        {
            get { return GetValue(AngleIncrementProperty); }
            set { SetValue(AngleIncrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the legend format.
        /// </summary>
        public string LegendFormat
        {
            get { return GetValue(LegendFormatProperty); }
            set { SetValue(LegendFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the outside label format.
        /// </summary>
        public string OutsideLabelFormat
        {
            get { return GetValue(OutsideLabelFormatProperty); }
            set { SetValue(OutsideLabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the inside labels.
        /// </summary>
        public Color InsideLabelColor
        {
            get { return GetValue(InsideLabelColorProperty); }
            set { SetValue(InsideLabelColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inside label format.
        /// </summary>
        public string InsideLabelFormat
        {
            get { return GetValue(InsideLabelFormatProperty); }
            set { SetValue(InsideLabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inside label position.
        /// </summary>
        public double InsideLabelPosition
        {
            get { return GetValue(InsideLabelPositionProperty); }
            set { SetValue(InsideLabelPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inside labels are angled.
        /// </summary>
        public bool AreInsideLabelsAngled
        {
            get { return GetValue(AreInsideLabelsAngledProperty); }
            set { SetValue(AreInsideLabelsAngledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance from the edge of the pie slice to the tick line.
        /// </summary>
        public double TickDistance
        {
            get { return GetValue(TickDistanceProperty); }
            set { SetValue(TickDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the length of the radial part of the tick line.
        /// </summary>
        public double TickRadialLength
        {
            get { return GetValue(TickRadialLengthProperty); }
            set { SetValue(TickRadialLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the length of the horizontal part of the tick.
        /// </summary>
        public double TickHorizontalLength
        {
            get { return GetValue(TickHorizontalLengthProperty); }
            set { SetValue(TickHorizontalLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance from the tick line to the outside label.
        /// </summary>
        public double TickLabelDistance
        {
            get { return GetValue(TickLabelDistanceProperty); }
            set { SetValue(TickLabelDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the exploded distance.
        /// </summary>
        public double ExplodedDistance
        {
            get { return GetValue(ExplodedDistanceProperty); }
            set { SetValue(ExplodedDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property containing the label.
        /// </summary>
        public string LabelField
        {
            get { return GetValue(LabelFieldProperty); }
            set { SetValue(LabelFieldProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property containing the value.
        /// </summary>
        public string ValueField
        {
            get { return GetValue(ValueFieldProperty); }
            set { SetValue(ValueFieldProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property containing the color.
        /// </summary>
        public string ColorField
        {
            get { return GetValue(ColorFieldProperty); }
            set { SetValue(ColorFieldProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property indicating whether the item
        /// is exploded.
        /// </summary>
        public string IsExplodedField
        {
            get { return GetValue(IsExplodedFieldProperty); }
            set { SetValue(IsExplodedFieldProperty, value); }
        }

        /// <summary>
        /// Synchronizes the properties of this object with the underlying series object.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.PieSeries)series;

            s.Stroke = Stroke.ToOxyColor();
            s.StrokeThickness = StrokeThickness;
            s.Diameter = Diameter;
            s.InnerDiameter = InnerDiameter;
            s.StartAngle = StartAngle;
            s.AngleSpan = AngleSpan;
            s.AngleIncrement = AngleIncrement;

            s.LegendFormat = LegendFormat;

            s.OutsideLabelFormat = OutsideLabelFormat;
            s.InsideLabelColor = InsideLabelColor.ToOxyColor();
            s.InsideLabelFormat = InsideLabelFormat;
            s.InsideLabelPosition = InsideLabelPosition;
            s.AreInsideLabelsAngled = AreInsideLabelsAngled;

            s.TickDistance = TickDistance;
            s.TickRadialLength = TickRadialLength;
            s.TickHorizontalLength = TickHorizontalLength;
            s.TickLabelDistance = TickLabelDistance;

            s.ExplodedDistance = ExplodedDistance;

            s.LabelField = LabelField;
            s.ValueField = ValueField;
            s.ColorField = ColorField;
            s.IsExplodedField = IsExplodedField;
        }
    }
}
