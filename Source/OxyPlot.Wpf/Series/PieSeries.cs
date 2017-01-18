// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.LineSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a wrapper of OxyPlot.PieSeries.
    /// </summary>
    public class PieSeries : ItemsSeries
    {
        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                "Stroke",
                typeof(Color),
                typeof(PieSeries),
                new PropertyMetadata(Colors.White, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Diameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(
                "Diameter",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="InnerDiameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InnerDiameterProperty =
            DependencyProperty.Register(
                "InnerDiameter",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                "StartAngle",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AngleSpan"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AngleSpanProperty =
            DependencyProperty.Register(
                "AngleSpan",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(360.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AngleIncrement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AngleIncrementProperty =
            DependencyProperty.Register(
                "AngleIncrement",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendFormatProperty =
            DependencyProperty.Register(
                "LegendFormat",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="OutsideLabelFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OutsideLabelFormatProperty =
            DependencyProperty.Register(
                "OutsideLabelFormat",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata("{2:0} %", AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="InsideLabelColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InsideLabelColorProperty =
            DependencyProperty.Register(
                "InsideLabelColor",
                typeof(Color),
                typeof(PieSeries),
                new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="InsideLabelFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InsideLabelFormatProperty =
            DependencyProperty.Register(
                "InsideLabelFormat",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata("{1}", AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="InsideLabelPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InsideLabelPositionProperty =
            DependencyProperty.Register(
                "InsideLabelPosition",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.5, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AreInsideLabelsAngled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AreInsideLabelsAngledProperty =
            DependencyProperty.Register(
                "AreInsideLabelsAngled",
                typeof(bool),
                typeof(PieSeries),
                new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TickDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickDistanceProperty =
            DependencyProperty.Register(
                "TickDistance",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TickRadialLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickRadialLengthProperty =
            DependencyProperty.Register(
                "TickRadialLength",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(6.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TickHorizontalLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickHorizontalLengthProperty =
            DependencyProperty.Register(
                "TickHorizontalLength",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(8.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TickLabelDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickLabelDistanceProperty =
            DependencyProperty.Register(
                "TickLabelDistance",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(4.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ExplodedDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExplodedDistanceProperty =
            DependencyProperty.Register(
                "ExplodedDistance",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0));

        /// <summary>
        /// Identifies the <see cref="LabelField"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFieldProperty =
            DependencyProperty.Register(
                "LabelField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="ValueField"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueFieldProperty =
            DependencyProperty.Register(
                "ValueField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="ColorField"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorFieldProperty =
            DependencyProperty.Register(
                "ColorField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="IsExplodedField"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExplodedFieldProperty =
            DependencyProperty.Register(
                "IsExplodedField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Initializes static members of the <see cref="PieSeries"/> class.
        /// </summary>
        static PieSeries()
        {
            TrackerFormatStringProperty
                .OverrideMetadata(
                    typeof(PieSeries),
                    new PropertyMetadata(OxyPlot.Series.PieSeries.DefaultTrackerFormatString, AppearanceChanged));
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
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inner diameter.
        /// </summary>
        public double InnerDiameter
        {
            get { return (double)GetValue(InnerDiameterProperty); }
            set { SetValue(InnerDiameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle span.
        /// </summary>
        public double AngleSpan
        {
            get { return (double)GetValue(AngleSpanProperty); }
            set { SetValue(AngleSpanProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle increment.
        /// </summary>
        public double AngleIncrement
        {
            get { return (double)GetValue(AngleIncrementProperty); }
            set { SetValue(AngleIncrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the legend format.
        /// </summary>
        public string LegendFormat
        {
            get { return (string)GetValue(LegendFormatProperty); }
            set { SetValue(LegendFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the outside label format.
        /// </summary>
        public string OutsideLabelFormat
        {
            get { return (string)GetValue(OutsideLabelFormatProperty); }
            set { SetValue(OutsideLabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the inside labels.
        /// </summary>
        public Color InsideLabelColor
        {
            get { return (Color)GetValue(InsideLabelColorProperty); }
            set { SetValue(InsideLabelColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inside label format.
        /// </summary>
        public string InsideLabelFormat
        {
            get { return (string)GetValue(InsideLabelFormatProperty); }
            set { SetValue(InsideLabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the inside label position.
        /// </summary>
        public double InsideLabelPosition
        {
            get { return (double)GetValue(InsideLabelPositionProperty); }
            set { SetValue(InsideLabelPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inside labels are angled.
        /// </summary>
        public bool AreInsideLabelsAngled
        {
            get { return (bool)GetValue(AreInsideLabelsAngledProperty); }
            set { SetValue(AreInsideLabelsAngledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance from the edge of the pie slice to the tick line.
        /// </summary>
        public double TickDistance
        {
            get { return (double)GetValue(TickDistanceProperty); }
            set { SetValue(TickDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the length of the radial part of the tick line.
        /// </summary>
        public double TickRadialLength
        {
            get { return (double)GetValue(TickRadialLengthProperty); }
            set { SetValue(TickRadialLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the length of the horizontal part of the tick.
        /// </summary>
        public double TickHorizontalLength
        {
            get { return (double)GetValue(TickHorizontalLengthProperty); }
            set { SetValue(TickHorizontalLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance from the tick line to the outside label.
        /// </summary>
        public double TickLabelDistance
        {
            get { return (double)GetValue(TickLabelDistanceProperty); }
            set { SetValue(TickLabelDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the exploded distance.
        /// </summary>
        public double ExplodedDistance
        {
            get { return (double)GetValue(ExplodedDistanceProperty); }
            set { SetValue(ExplodedDistanceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property containing the label.
        /// </summary>
        public string LabelField
        {
            get { return (string)GetValue(LabelFieldProperty); }
            set { SetValue(LabelFieldProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property containing the value.
        /// </summary>
        public string ValueField
        {
            get { return (string)GetValue(ValueFieldProperty); }
            set { SetValue(ValueFieldProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property containing the color.
        /// </summary>
        public string ColorField
        {
            get { return (string)GetValue(ColorFieldProperty); }
            set { SetValue(ColorFieldProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the property indicating whether the item
        /// is exploded.
        /// </summary>
        public string IsExplodedField
        {
            get { return (string)GetValue(IsExplodedFieldProperty); }
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
