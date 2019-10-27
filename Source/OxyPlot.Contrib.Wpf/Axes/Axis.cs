// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Axis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The axis base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The axis base.
    /// </summary>
    public abstract class Axis : FrameworkElement
    {
        /// <summary>
        /// Identifies the <see cref="AbsoluteMaximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AbsoluteMaximumProperty =
            DependencyProperty.Register(
                "AbsoluteMaximum",
                typeof(double),
                typeof(Axis),
                new PropertyMetadata(double.MaxValue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AbsoluteMinimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AbsoluteMinimumProperty =
            DependencyProperty.Register(
                "AbsoluteMinimum",
                typeof(double),
                typeof(Axis),
                new PropertyMetadata(double.MinValue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Angle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof(double), typeof(Axis), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AxisTickToLabelDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisTickToLabelDistanceProperty =
            DependencyProperty.Register(
                "AxisTickToLabelDistance", typeof(double), typeof(Axis), new PropertyMetadata(4.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AxisTitleDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisTitleDistanceProperty =
            DependencyProperty.Register("AxisTitleDistance", typeof(double), typeof(Axis), new PropertyMetadata(4.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AxisDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisDistanceProperty =
            DependencyProperty.Register("AxisDistance", typeof(double), typeof(Axis), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AxislineColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxislineColorProperty = DependencyProperty.Register(
            "AxislineColor", typeof(Color), typeof(Axis), new UIPropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AxislineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxislineStyleProperty = DependencyProperty.Register(
            "AxislineStyle", typeof(LineStyle), typeof(Axis), new UIPropertyMetadata(LineStyle.None, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AxislineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxislineThicknessProperty =
            DependencyProperty.Register("AxislineThickness", typeof(double), typeof(Axis), new UIPropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ClipTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClipTitleProperty = DependencyProperty.Register(
            "ClipTitle", typeof(bool), typeof(Axis), new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="EndPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EndPositionProperty = DependencyProperty.Register(
            "EndPosition", typeof(double), typeof(Axis), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ExtraGridlineColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlineColorProperty =
            DependencyProperty.Register(
                "ExtraGridlineColor", typeof(Color), typeof(Axis), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ExtraGridlineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlineStyleProperty =
            DependencyProperty.Register(
                "ExtraGridlineStyle",
                typeof(LineStyle),
                typeof(Axis),
                new PropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ExtraGridlineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlineThicknessProperty =
            DependencyProperty.Register(
                "ExtraGridlineThickness", typeof(double), typeof(Axis), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ExtraGridlines"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlinesProperty = DependencyProperty.Register(
            "ExtraGridLines", typeof(double[]), typeof(Axis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="FilterFunction"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterFunctionProperty = DependencyProperty.Register(
            "FilterFunction", typeof(Func<double, bool>), typeof(Axis), new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="FilterMaxValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterMaxValueProperty = DependencyProperty.Register(
            "FilterMaxValue", typeof(double), typeof(Axis), new PropertyMetadata(double.MaxValue, DataChanged));

        /// <summary>
        /// Identifies the <see cref="FilterMinValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterMinValueProperty = DependencyProperty.Register(
            "FilterMinValue", typeof(double), typeof(Axis), new PropertyMetadata(double.MinValue, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Font"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FontProperty = DependencyProperty.Register(
            "Font", typeof(string), typeof(Axis), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="FontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize", typeof(double), typeof(Axis), new PropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="FontWeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
            "FontWeight", typeof(FontWeight), typeof(Axis), new PropertyMetadata(FontWeights.Normal, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="IntervalLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IntervalLengthProperty = DependencyProperty.Register(
            "IntervalLength", typeof(double), typeof(Axis), new PropertyMetadata(60.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="IsAxisVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsAxisVisibleProperty = DependencyProperty.Register(
            "IsAxisVisible", typeof(bool), typeof(Axis), new PropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="IsPanEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register(
            "IsPanEnabled", typeof(bool), typeof(Axis), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="IsZoomEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register(
            "IsZoomEnabled", typeof(bool), typeof(Axis), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Key"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key", typeof(string), typeof(Axis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="LabelFormatter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFormatterProperty = DependencyProperty.Register(
            "LabelFormatter", typeof(Func<double, string>), typeof(Axis), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Layer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(
            "Layer", typeof(Axes.AxisLayer), typeof(Axis), new PropertyMetadata(Axes.AxisLayer.BelowSeries, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MajorGridlineColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorGridlineColorProperty =
            DependencyProperty.Register(
                "MajorGridlineColor",
                typeof(Color),
                typeof(Axis),
                new PropertyMetadata(Color.FromArgb(0x40, 0, 0, 0), AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MajorGridlineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorGridlineStyleProperty =
            DependencyProperty.Register(
                "MajorGridlineStyle",
                typeof(LineStyle),
                typeof(Axis),
                new PropertyMetadata(LineStyle.None, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MajorGridlineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorGridlineThicknessProperty =
            DependencyProperty.Register(
                "MajorGridlineThickness", typeof(double), typeof(Axis), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MajorStep"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorStepProperty = DependencyProperty.Register(
            "MajorStep", typeof(double), typeof(Axis), new PropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MajorTickSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MajorTickSizeProperty = DependencyProperty.Register(
            "MajorTickSize", typeof(double), typeof(Axis), new PropertyMetadata(7.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MaximumPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumPaddingProperty = DependencyProperty.Register(
            "MaximumPadding", typeof(double), typeof(Axis), new PropertyMetadata(0.01, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(Axis), new PropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MaximumRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRangeProperty = DependencyProperty.Register(
            "MaximumRange", typeof(double), typeof(Axis), new PropertyMetadata(double.PositiveInfinity, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinimumPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumPaddingProperty = DependencyProperty.Register(
            "MinimumPadding", typeof(double), typeof(Axis), new PropertyMetadata(0.01, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(Axis), new PropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinimumRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumRangeProperty = DependencyProperty.Register(
            "MinimumRange", typeof(double), typeof(Axis), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinorGridlineColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorGridlineColorProperty =
            DependencyProperty.Register(
                "MinorGridlineColor",
                typeof(Color),
                typeof(Axis),
                new PropertyMetadata(Color.FromArgb(0x20, 0, 0, 0), AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinorGridlineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorGridlineStyleProperty =
            DependencyProperty.Register(
                "MinorGridlineStyle",
                typeof(LineStyle),
                typeof(Axis),
                new PropertyMetadata(LineStyle.None, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinorGridlineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorGridlineThicknessProperty =
            DependencyProperty.Register(
                "MinorGridlineThickness", typeof(double), typeof(Axis), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinorStep"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorStepProperty = DependencyProperty.Register(
            "MinorStep", typeof(double), typeof(Axis), new PropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinorTickSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorTickSizeProperty = DependencyProperty.Register(
            "MinorTickSize", typeof(double), typeof(Axis), new PropertyMetadata(4.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PositionAtZeroCrossing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionAtZeroCrossingProperty =
            DependencyProperty.Register(
                "PositionAtZeroCrossing", typeof(bool), typeof(Axis), new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof(Axes.AxisPosition), typeof(Axis), new PropertyMetadata(Axes.AxisPosition.Left, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PositionTier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionTierProperty = DependencyProperty.Register(
            "PositionTier", typeof(int), typeof(Axis), new PropertyMetadata(0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StartPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartPositionProperty = DependencyProperty.Register(
            "StartPosition", typeof(double), typeof(Axis), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StringFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            "StringFormat", typeof(string), typeof(Axis), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(Color), typeof(Axis), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));
        
        /// <summary>
        /// Identifies the <see cref="TickStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TickStyleProperty = DependencyProperty.Register(
            "TickStyle", typeof(Axes.TickStyle), typeof(Axis), new PropertyMetadata(Axes.TickStyle.Outside, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TicklineColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TicklineColorProperty = DependencyProperty.Register(
            "TicklineColor", typeof(Color), typeof(Axis), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleClippingLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleClippingLengthProperty =
            DependencyProperty.Register(
                "TitleClippingLength", typeof(double), typeof(Axis), new UIPropertyMetadata(0.9, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleColorProperty = DependencyProperty.Register(
            "TitleColor", typeof(Color), typeof(Axis), new UIPropertyMetadata(OxyColors.Automatic.ToColor(), AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleFont"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleFontProperty = DependencyProperty.Register(
            "TitleFont", typeof(string), typeof(Axis), new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            "TitleFontSize", typeof(double), typeof(Axis), new UIPropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleFontWeightProperty =
            DependencyProperty.Register(
                "TitleFontWeight", typeof(FontWeight), typeof(Axis), new UIPropertyMetadata(FontWeights.Normal, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleFormatString"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleFormatStringProperty =
            DependencyProperty.Register(
                "TitleFormatString", typeof(string), typeof(Axis), new PropertyMetadata("{0} [{1}]", AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitlePosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitlePositionProperty = DependencyProperty.Register(
            "TitlePosition", typeof(double), typeof(Axis), new PropertyMetadata(0.5, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Axis), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Unit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(
            "Unit", typeof(string), typeof(Axis), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="UseSuperExponentialFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UseSuperExponentialFormatProperty =
            DependencyProperty.Register(
                "UseSuperExponentialFormat", typeof(bool), typeof(Axis), new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Gets or sets the internal axis.
        /// </summary>
        public Axes.Axis InternalAxis { get; protected set; }

        /// <summary>
        /// Gets or sets the absolute maximum. This is only used for the UI control. It will not be possible to zoom/pan beyond this limit.
        /// </summary>
        public double AbsoluteMaximum
        {
            get
            {
                return (double)this.GetValue(AbsoluteMaximumProperty);
            }

            set
            {
                this.SetValue(AbsoluteMaximumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the absolute minimum. This is only used for the UI control. It will not be possible to zoom/pan beyond this limit.
        /// </summary>
        public double AbsoluteMinimum
        {
            get
            {
                return (double)this.GetValue(AbsoluteMinimumProperty);
            }

            set
            {
                this.SetValue(AbsoluteMinimumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Angle.
        /// </summary>
        public double Angle
        {
            get
            {
                return (double)this.GetValue(AngleProperty);
            }

            set
            {
                this.SetValue(AngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets AxisDistance.
        /// </summary>
        public double AxisDistance
        {
            get
            {
                return (double)this.GetValue(AxisDistanceProperty);
            }

            set
            {
                this.SetValue(AxisDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets AxisTickToLabelDistance.
        /// </summary>
        public double AxisTickToLabelDistance
        {
            get
            {
                return (double)this.GetValue(AxisTickToLabelDistanceProperty);
            }

            set
            {
                this.SetValue(AxisTickToLabelDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets AxisTitleDistance.
        /// </summary>
        public double AxisTitleDistance
        {
            get
            {
                return (double)this.GetValue(AxisTitleDistanceProperty);
            }

            set
            {
                this.SetValue(AxisTitleDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the axis line.
        /// </summary>
        /// <value>The color of the axis line.</value>
        public Color AxislineColor
        {
            get
            {
                return (Color)this.GetValue(AxislineColorProperty);
            }

            set
            {
                this.SetValue(AxislineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the axis line style.
        /// </summary>
        /// <value>The axis line style.</value>
        public LineStyle AxislineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(AxislineStyleProperty);
            }

            set
            {
                this.SetValue(AxislineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the axis line thickness.
        /// </summary>
        /// <value>The axis line thickness.</value>
        public double AxislineThickness
        {
            get
            {
                return (double)this.GetValue(AxislineThicknessProperty);
            }

            set
            {
                this.SetValue(AxislineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [clip title].
        /// </summary>
        /// <value><c>true</c> if [clip title]; otherwise, <c>false</c> .</value>
        public bool ClipTitle
        {
            get
            {
                return (bool)this.GetValue(ClipTitleProperty);
            }

            set
            {
                this.SetValue(ClipTitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets EndPosition.
        /// </summary>
        public double EndPosition
        {
            get
            {
                return (double)this.GetValue(EndPositionProperty);
            }

            set
            {
                this.SetValue(EndPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridlineColor.
        /// </summary>
        public Color ExtraGridlineColor
        {
            get
            {
                return (Color)this.GetValue(ExtraGridlineColorProperty);
            }

            set
            {
                this.SetValue(ExtraGridlineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridlineStyle.
        /// </summary>
        public LineStyle ExtraGridlineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(ExtraGridlineStyleProperty);
            }

            set
            {
                this.SetValue(ExtraGridlineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridlineThickness.
        /// </summary>
        public double ExtraGridlineThickness
        {
            get
            {
                return (double)this.GetValue(ExtraGridlineThicknessProperty);
            }

            set
            {
                this.SetValue(ExtraGridlineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridLines.
        /// </summary>
        public double[] ExtraGridlines
        {
            get
            {
                return (double[])this.GetValue(ExtraGridlinesProperty);
            }

            set
            {
                this.SetValue(ExtraGridlinesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the filter function.
        /// </summary>
        /// <value>The filter function.</value>
        public Func<double, bool> FilterFunction
        {
            get
            {
                return (Func<double, bool>)this.GetValue(FilterFunctionProperty);
            }

            set
            {
                this.SetValue(FilterFunctionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FilterMaxValue.
        /// </summary>
        public double FilterMaxValue
        {
            get
            {
                return (double)this.GetValue(FilterMaxValueProperty);
            }

            set
            {
                this.SetValue(FilterMaxValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FilterMinValue.
        /// </summary>
        public double FilterMinValue
        {
            get
            {
                return (double)this.GetValue(FilterMinValueProperty);
            }

            set
            {
                this.SetValue(FilterMinValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Font.
        /// </summary>
        public string Font
        {
            get
            {
                return (string)this.GetValue(FontProperty);
            }

            set
            {
                this.SetValue(FontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FontSize.
        /// </summary>
        public double FontSize
        {
            get
            {
                return (double)this.GetValue(FontSizeProperty);
            }

            set
            {
                this.SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(FontWeightProperty);
            }

            set
            {
                this.SetValue(FontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the interval length.
        /// </summary>
        public double IntervalLength
        {
            get
            {
                return (double)this.GetValue(IntervalLengthProperty);
            }

            set
            {
                this.SetValue(IntervalLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the axis is visible.
        /// </summary>
        public bool IsAxisVisible
        {
            get
            {
                return (bool)this.GetValue(IsAxisVisibleProperty);
            }

            set
            {
                this.SetValue(IsAxisVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether pan is enabled.
        /// </summary>
        public bool IsPanEnabled
        {
            get
            {
                return (bool)this.GetValue(IsPanEnabledProperty);
            }

            set
            {
                this.SetValue(IsPanEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether zoom is enabled.
        /// </summary>
        public bool IsZoomEnabled
        {
            get
            {
                return (bool)this.GetValue(IsZoomEnabledProperty);
            }

            set
            {
                this.SetValue(IsZoomEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the axis key.
        /// </summary>
        public string Key
        {
            get
            {
                return (string)this.GetValue(KeyProperty);
            }

            set
            {
                this.SetValue(KeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label formatter.
        /// </summary>
        public Func<double, string> LabelFormatter
        {
            get
            {
                return (Func<double, string>)this.GetValue(LabelFormatterProperty);
            }

            set
            {
                this.SetValue(LabelFormatterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        public Axes.AxisLayer Layer
        {
            get
            {
                return (Axes.AxisLayer)this.GetValue(LayerProperty);
            }

            set
            {
                this.SetValue(LayerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the major gridlines.
        /// </summary>
        public Color MajorGridlineColor
        {
            get
            {
                return (Color)this.GetValue(MajorGridlineColorProperty);
            }

            set
            {
                this.SetValue(MajorGridlineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the line style of the major gridlines.
        /// </summary>
        public LineStyle MajorGridlineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(MajorGridlineStyleProperty);
            }

            set
            {
                this.SetValue(MajorGridlineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MajorGridlineThickness.
        /// </summary>
        public double MajorGridlineThickness
        {
            get
            {
                return (double)this.GetValue(MajorGridlineThicknessProperty);
            }

            set
            {
                this.SetValue(MajorGridlineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MajorStep.
        /// </summary>
        public double MajorStep
        {
            get
            {
                return (double)this.GetValue(MajorStepProperty);
            }

            set
            {
                this.SetValue(MajorStepProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MajorTickSize.
        /// </summary>
        public double MajorTickSize
        {
            get
            {
                return (double)this.GetValue(MajorTickSizeProperty);
            }

            set
            {
                this.SetValue(MajorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Maximum.
        /// </summary>
        public double Maximum
        {
            get
            {
                return (double)this.GetValue(MaximumProperty);
            }

            set
            {
                this.SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MaximumPadding.
        /// </summary>
        public double MaximumPadding
        {
            get
            {
                return (double)this.GetValue(MaximumPaddingProperty);
            }

            set
            {
                this.SetValue(MaximumPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MaximumRange.
        /// </summary>
        public double MaximumRange
        {
            get
            {
                return (double)this.GetValue(MaximumRangeProperty);
            }

            set
            {
                this.SetValue(MaximumRangeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Minimum.
        /// </summary>
        public double Minimum
        {
            get
            {
                return (double)this.GetValue(MinimumProperty);
            }

            set
            {
                this.SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinimumPadding.
        /// </summary>
        public double MinimumPadding
        {
            get
            {
                return (double)this.GetValue(MinimumPaddingProperty);
            }

            set
            {
                this.SetValue(MinimumPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinimumRange.
        /// </summary>
        public double MinimumRange
        {
            get
            {
                return (double)this.GetValue(MinimumRangeProperty);
            }

            set
            {
                this.SetValue(MinimumRangeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorGridlineColor.
        /// </summary>
        public Color MinorGridlineColor
        {
            get
            {
                return (Color)this.GetValue(MinorGridlineColorProperty);
            }

            set
            {
                this.SetValue(MinorGridlineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorGridlineStyle.
        /// </summary>
        public LineStyle MinorGridlineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(MinorGridlineStyleProperty);
            }

            set
            {
                this.SetValue(MinorGridlineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorGridlineThickness.
        /// </summary>
        public double MinorGridlineThickness
        {
            get
            {
                return (double)this.GetValue(MinorGridlineThicknessProperty);
            }

            set
            {
                this.SetValue(MinorGridlineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorStep.
        /// </summary>
        public double MinorStep
        {
            get
            {
                return (double)this.GetValue(MinorStepProperty);
            }

            set
            {
                this.SetValue(MinorStepProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorTickSize.
        /// </summary>
        public double MinorTickSize
        {
            get
            {
                return (double)this.GetValue(MinorTickSizeProperty);
            }

            set
            {
                this.SetValue(MinorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public Axes.AxisPosition Position
        {
            get
            {
                return (Axes.AxisPosition)this.GetValue(PositionProperty);
            }

            set
            {
                this.SetValue(PositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether PositionAtZeroCrossing.
        /// </summary>
        public bool PositionAtZeroCrossing
        {
            get
            {
                return (bool)this.GetValue(PositionAtZeroCrossingProperty);
            }

            set
            {
                this.SetValue(PositionAtZeroCrossingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the position tier which defines in which tier the axis is displayed.
        /// </summary>
        /// <remarks>The bigger the value the further afar is the axis from the graph.</remarks>
        public int PositionTier
        {
            get
            {
                return (int)this.GetValue(PositionTierProperty);
            }

            set
            {
                this.SetValue(PositionTierProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        public double StartPosition
        {
            get
            {
                return (double)this.GetValue(StartPositionProperty);
            }

            set
            {
                this.SetValue(StartPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the string format.
        /// </summary>
        public string StringFormat
        {
            get
            {
                return (string)this.GetValue(StringFormatProperty);
            }

            set
            {
                this.SetValue(StringFormatProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text color
        /// </summary>
        public Color TextColor
        {
            get
            {
                return (Color)this.GetValue(TextColorProperty);
            }

            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tick style.
        /// </summary>
        public Axes.TickStyle TickStyle
        {
            get
            {
                return (Axes.TickStyle)this.GetValue(TickStyleProperty);
            }

            set
            {
                this.SetValue(TickStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tick line color.
        /// </summary>
        public Color TicklineColor
        {
            get
            {
                return (Color)this.GetValue(TicklineColorProperty);
            }

            set
            {
                this.SetValue(TicklineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }

            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of the title clipping.
        /// </summary>
        /// <value>The length of the title clipping.</value>
        public double TitleClippingLength
        {
            get
            {
                return (double)this.GetValue(TitleClippingLengthProperty);
            }

            set
            {
                this.SetValue(TitleClippingLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the title.
        /// </summary>
        /// <value>The color of the title.</value>
        public Color TitleColor
        {
            get
            {
                return (Color)this.GetValue(TitleColorProperty);
            }

            set
            {
                this.SetValue(TitleColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the title font.
        /// </summary>
        /// <value>The title font.</value>
        public string TitleFont
        {
            get
            {
                return (string)this.GetValue(TitleFontProperty);
            }

            set
            {
                this.SetValue(TitleFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the title font.
        /// </summary>
        /// <value>The size of the title font.</value>
        public double TitleFontSize
        {
            get
            {
                return (double)this.GetValue(TitleFontSizeProperty);
            }

            set
            {
                this.SetValue(TitleFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the title font weight.
        /// </summary>
        /// <value>The title font weight.</value>
        public FontWeight TitleFontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(TitleFontWeightProperty);
            }

            set
            {
                this.SetValue(TitleFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TitleFormatString.
        /// </summary>
        public string TitleFormatString
        {
            get
            {
                return (string)this.GetValue(TitleFormatStringProperty);
            }

            set
            {
                this.SetValue(TitleFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TitlePosition.
        /// </summary>
        public double TitlePosition
        {
            get
            {
                return (double)this.GetValue(TitlePositionProperty);
            }

            set
            {
                this.SetValue(TitlePositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Unit.
        /// </summary>
        public string Unit
        {
            get
            {
                return (string)this.GetValue(UnitProperty);
            }

            set
            {
                this.SetValue(UnitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UseSuperExponentialFormat.
        /// </summary>
        public bool UseSuperExponentialFormat
        {
            get
            {
                return (bool)this.GetValue(UseSuperExponentialFormatProperty);
            }

            set
            {
                this.SetValue(UseSuperExponentialFormatProperty, value);
            }
        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns>An axis object.</returns>
        public abstract Axes.Axis CreateModel();

        /// <summary>
        /// The visual appearance changed.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The event args.</param>
        protected static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Axis)d).OnVisualChanged();
        }

        /// <summary>
        /// The data changed.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The event args.</param>
        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Axis)d).OnDataChanged();
        }

        /// <summary>
        /// The on data changed handler.
        /// </summary>
        protected void OnDataChanged()
        {
            var pc = this.Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        /// <summary>
        /// The on property changed handler.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.OwnerType == this.GetType())
            {
                var fpm = e.Property.GetMetadata(e.Property.OwnerType) as FrameworkPropertyMetadata;
                if (fpm != null && fpm.AffectsRender)
                {
                    var plot = this.Parent as IPlotView;
                    if (plot != null)
                    {
                        plot.InvalidatePlot();
                    }
                }
            }
        }

        /// <summary>
        /// Handles changed visuals.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot(false);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected virtual void SynchronizeProperties()
        {
            var a = this.InternalAxis;
            a.AbsoluteMaximum = this.AbsoluteMaximum;
            a.AbsoluteMinimum = this.AbsoluteMinimum;
            a.Angle = this.Angle;
            a.AxisDistance = this.AxisDistance;
            a.AxislineColor = this.AxislineColor.ToOxyColor();
            a.AxislineStyle = this.AxislineStyle;
            a.AxislineThickness = this.AxislineThickness;
            a.AxisTitleDistance = this.AxisTitleDistance;
            a.AxisTickToLabelDistance = this.AxisTickToLabelDistance;
            a.ClipTitle = this.ClipTitle;
            a.EndPosition = this.EndPosition;
            a.ExtraGridlineColor = this.ExtraGridlineColor.ToOxyColor();
            a.ExtraGridlineStyle = this.ExtraGridlineStyle;
            a.ExtraGridlineThickness = this.ExtraGridlineThickness;
            a.ExtraGridlines = this.ExtraGridlines;
            a.FilterFunction = this.FilterFunction;
            a.FilterMaxValue = this.FilterMaxValue;
            a.FilterMinValue = this.FilterMinValue;
            a.Font = this.Font;
            a.FontSize = this.FontSize;
            a.FontWeight = this.FontWeight.ToOpenTypeWeight();
            a.IntervalLength = this.IntervalLength;
            a.IsPanEnabled = this.IsPanEnabled;
            a.IsAxisVisible = this.IsAxisVisible;
            a.IsZoomEnabled = this.IsZoomEnabled;
            a.Key = this.Key;
            a.Layer = this.Layer;
            a.MajorGridlineColor = this.MajorGridlineColor.ToOxyColor();
            a.MinorGridlineColor = this.MinorGridlineColor.ToOxyColor();
            a.MajorGridlineStyle = this.MajorGridlineStyle;
            a.MinorGridlineStyle = this.MinorGridlineStyle;
            a.MajorGridlineThickness = this.MajorGridlineThickness;
            a.MinorGridlineThickness = this.MinorGridlineThickness;
            a.MajorStep = this.MajorStep;
            a.MajorTickSize = this.MajorTickSize;
            a.MinorStep = this.MinorStep;
            a.MinorTickSize = this.MinorTickSize;
            a.Minimum = this.Minimum;
            a.Maximum = this.Maximum;
            a.MinimumRange = this.MinimumRange;
            a.MaximumRange = this.MaximumRange;
            a.MinimumPadding = this.MinimumPadding;
            a.MaximumPadding = this.MaximumPadding;
            a.Position = this.Position;
            a.PositionTier = this.PositionTier;
            a.PositionAtZeroCrossing = this.PositionAtZeroCrossing;
            a.StartPosition = this.StartPosition;
            a.StringFormat = this.StringFormat;
            a.TextColor = this.TextColor.ToOxyColor();
            a.TicklineColor = this.TicklineColor.ToOxyColor();
            a.TitleClippingLength = this.TitleClippingLength;
            a.TitleColor = this.TitleColor.ToOxyColor();
            a.TitleFont = this.TitleFont;
            a.TitleFontSize = this.TitleFontSize;
            a.TitleFontWeight = this.TitleFontWeight.ToOpenTypeWeight();
            a.TitleFormatString = this.TitleFormatString;
            a.Title = this.Title;
            a.ToolTip = this.ToolTip != null ? this.ToolTip.ToString() : null;
            a.TickStyle = this.TickStyle;
            a.TitlePosition = this.TitlePosition;
            a.Unit = this.Unit;
            a.UseSuperExponentialFormat = this.UseSuperExponentialFormat;
            a.LabelFormatter = this.LabelFormatter;
        }
    }
}