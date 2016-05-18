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

    public class PieSeries : ItemsSeries
    {
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                "Stroke",
                typeof(Color),
                typeof(PieSeries),
                new PropertyMetadata(Colors.White, AppearanceChanged));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(1.0, AppearanceChanged));

        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(
                "Diameter",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(1.0, AppearanceChanged));

        public static readonly DependencyProperty InnerDiameterProperty =
            DependencyProperty.Register(
                "InnerDiameter",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0, AppearanceChanged));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                "StartAngle",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0, AppearanceChanged));

        public static readonly DependencyProperty AngleSpanProperty =
            DependencyProperty.Register(
                "AngleSpan",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(360.0, AppearanceChanged));

        public static readonly DependencyProperty AngleIncrementProperty =
            DependencyProperty.Register(
                "AngleIncrement",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(1.0, AppearanceChanged));

        public static readonly DependencyProperty LegendFormatProperty =
            DependencyProperty.Register(
                "LegendFormat",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, AppearanceChanged));

        public static readonly DependencyProperty OutsideLabelFormatProperty =
            DependencyProperty.Register(
                "OutsideLabelFormat",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata("{2:0} %", AppearanceChanged));

        public static readonly DependencyProperty InsideLabelColorProperty =
            DependencyProperty.Register(
                "InsideLabelColor",
                typeof(Color),
                typeof(PieSeries),
                new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        public static readonly DependencyProperty InsideLabelFormatProperty =
            DependencyProperty.Register(
                "InsideLabelFormat",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata("{1}", AppearanceChanged));

        public static readonly DependencyProperty InsideLabelPositionProperty =
            DependencyProperty.Register(
                "InsideLabelPosition",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.5, AppearanceChanged));

        public static readonly DependencyProperty AreInsideLabelsAngledProperty =
            DependencyProperty.Register(
                "AreInsideLabelsAngled",
                typeof(bool),
                typeof(PieSeries),
                new PropertyMetadata(false, AppearanceChanged));

        public static readonly DependencyProperty TickDistanceProperty =
            DependencyProperty.Register(
                "TickDistance",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0, AppearanceChanged));

        public static readonly DependencyProperty TickRadialLengthProperty =
            DependencyProperty.Register(
                "TickRadialLength",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(6.0, AppearanceChanged));

        public static readonly DependencyProperty TickHorizontalLengthProperty =
            DependencyProperty.Register(
                "TickHorizontalLength",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(8.0, AppearanceChanged));

        public static readonly DependencyProperty TickLabelDistanceProperty =
            DependencyProperty.Register(
                "TickLabelDistance",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(4.0, AppearanceChanged));

        public static readonly DependencyProperty ExplodedDistanceProperty =
            DependencyProperty.Register(
                "ExplodedDistance",
                typeof(double),
                typeof(PieSeries),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty LabelFieldProperty =
            DependencyProperty.Register(
                "LabelField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty ValueFieldProperty =
            DependencyProperty.Register(
                "ValueField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty ColorFieldProperty =
            DependencyProperty.Register(
                "ColorField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty IsExplodedFieldProperty =
            DependencyProperty.Register(
                "IsExplodedField",
                typeof(string),
                typeof(PieSeries),
                new PropertyMetadata(null, DataChanged));

        static PieSeries()
        {
            TrackerFormatStringProperty
                .OverrideMetadata(
                    typeof(PieSeries),
                    new PropertyMetadata(OxyPlot.Series.PieSeries.DefaultTrackerFormatString, AppearanceChanged));
        }

        public PieSeries()
        {
            InternalSeries = new OxyPlot.Series.PieSeries();
        }

        public override OxyPlot.Series.Series CreateModel()
        {
            SynchronizeProperties(InternalSeries);
            return InternalSeries;
        }

        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        public double InnerDiameter
        {
            get { return (double)GetValue(InnerDiameterProperty); }
            set { SetValue(InnerDiameterProperty, value); }
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double AngleSpan
        {
            get { return (double)GetValue(AngleSpanProperty); }
            set { SetValue(AngleSpanProperty, value); }
        }

        public double AngleIncrement
        {
            get { return (double)GetValue(AngleIncrementProperty); }
            set { SetValue(AngleIncrementProperty, value); }
        }

        public string LegendFormat
        {
            get { return (string)GetValue(LegendFormatProperty); }
            set { SetValue(LegendFormatProperty, value); }
        }

        public string OutsideLabelFormat
        {
            get { return (string)GetValue(OutsideLabelFormatProperty); }
            set { SetValue(OutsideLabelFormatProperty, value); }
        }

        public Color InsideLabelColor
        {
            get { return (Color)GetValue(InsideLabelColorProperty); }
            set { SetValue(InsideLabelColorProperty, value); }
        }

        public string InsideLabelFormat
        {
            get { return (string)GetValue(InsideLabelFormatProperty); }
            set { SetValue(InsideLabelFormatProperty, value); }
        }

        public double InsideLabelPosition
        {
            get { return (double)GetValue(InsideLabelPositionProperty); }
            set { SetValue(InsideLabelPositionProperty, value); }
        }

        public bool AreInsideLabelsAngled
        {
            get { return (bool)GetValue(AreInsideLabelsAngledProperty); }
            set { SetValue(AreInsideLabelsAngledProperty, value); }
        }

        public double TickDistance
        {
            get { return (double)GetValue(TickDistanceProperty); }
            set { SetValue(TickDistanceProperty, value); }
        }

        public double TickRadialLength
        {
            get { return (double)GetValue(TickRadialLengthProperty); }
            set { SetValue(TickRadialLengthProperty, value); }
        }

        public double TickHorizontalLength
        {
            get { return (double)GetValue(TickHorizontalLengthProperty); }
            set { SetValue(TickHorizontalLengthProperty, value); }
        }

        public double TickLabelDistance
        {
            get { return (double)GetValue(TickLabelDistanceProperty); }
            set { SetValue(TickLabelDistanceProperty, value); }
        }

        public double ExplodedDistance
        {
            get { return (double)GetValue(ExplodedDistanceProperty); }
            set { SetValue(ExplodedDistanceProperty, value); }
        }

        public string LabelField
        {
            get { return (string)GetValue(LabelFieldProperty); }
            set { SetValue(LabelFieldProperty, value); }
        }

        public string ValueField
        {
            get { return (string)GetValue(ValueFieldProperty); }
            set { SetValue(ValueFieldProperty, value); }
        }

        public string ColorField
        {
            get { return (string)GetValue(ColorFieldProperty); }
            set { SetValue(ColorFieldProperty, value); }
        }

        public string IsExplodedField
        {
            get { return (string)GetValue(IsExplodedFieldProperty); }
            set { SetValue(IsExplodedFieldProperty, value); }
        }

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
