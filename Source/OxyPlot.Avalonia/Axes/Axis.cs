// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Axis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The axis base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Controls;
    using global::Avalonia.Media;
    using System;

    /// <summary>
    /// The axis base.
    /// </summary>
    public abstract class Axis : Control
    {
        /// <summary>
        /// Identifies the <see cref="AbsoluteMaximum"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AbsoluteMaximumProperty = AvaloniaProperty.Register<Axis, double>(nameof(AbsoluteMaximum), double.MaxValue);

        /// <summary>
        /// Identifies the <see cref="AbsoluteMinimum"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AbsoluteMinimumProperty = AvaloniaProperty.Register<Axis, double>(nameof(AbsoluteMinimum), double.MinValue);

        /// <summary>
        /// Identifies the <see cref="Angle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AngleProperty = AvaloniaProperty.Register<Axis, double>(nameof(Angle), 0.0);

        /// <summary>
        /// Identifies the <see cref="AxisTickToLabelDistance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AxisTickToLabelDistanceProperty = AvaloniaProperty.Register<Axis, double>(nameof(AxisTickToLabelDistance), 4.0);

        /// <summary>
        /// Identifies the <see cref="AxisTitleDistance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AxisTitleDistanceProperty = AvaloniaProperty.Register<Axis, double>(nameof(AxisTitleDistance), 4.0);

        /// <summary>
        /// Identifies the <see cref="AxisDistance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AxisDistanceProperty = AvaloniaProperty.Register<Axis, double>(nameof(AxisDistance), 0.0);

        /// <summary>
        /// Identifies the <see cref="AxislineColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> AxislineColorProperty = AvaloniaProperty.Register<Axis, Color>(nameof(AxislineColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="AxislineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> AxislineStyleProperty = AvaloniaProperty.Register<Axis, LineStyle>(nameof(AxislineStyle), LineStyle.None);

        /// <summary>
        /// Identifies the <see cref="AxislineThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AxislineThicknessProperty = AvaloniaProperty.Register<Axis, double>(nameof(AxislineThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="ClipTitle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> ClipTitleProperty = AvaloniaProperty.Register<Axis, bool>(nameof(ClipTitle), true);

        /// <summary>
        /// Identifies the <see cref="EndPosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> EndPositionProperty = AvaloniaProperty.Register<Axis, double>(nameof(EndPosition), 1.0);

        /// <summary>
        /// Identifies the <see cref="ExtraGridlineColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> ExtraGridlineColorProperty = AvaloniaProperty.Register<Axis, Color>(nameof(ExtraGridlineColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="ExtraGridlineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> ExtraGridlineStyleProperty = AvaloniaProperty.Register<Axis, LineStyle>(nameof(ExtraGridlineStyle), LineStyle.Solid);

        /// <summary>
        /// Identifies the <see cref="ExtraGridlineThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> ExtraGridlineThicknessProperty = AvaloniaProperty.Register<Axis, double>(nameof(ExtraGridlineThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="ExtraGridlines"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double[]> ExtraGridlinesProperty = AvaloniaProperty.Register<Axis, double[]>("ExtraGridLines", null);

        /// <summary>
        /// Identifies the <see cref="FilterFunction"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Func<double, bool>> FilterFunctionProperty = AvaloniaProperty.Register<Axis, Func<double, bool>>(nameof(FilterFunction), null);

        /// <summary>
        /// Identifies the <see cref="FilterMaxValue"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> FilterMaxValueProperty = AvaloniaProperty.Register<Axis, double>(nameof(FilterMaxValue), double.MaxValue);

        /// <summary>
        /// Identifies the <see cref="FilterMinValue"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> FilterMinValueProperty = AvaloniaProperty.Register<Axis, double>(nameof(FilterMinValue), double.MinValue);

        /// <summary>
        /// Identifies the <see cref="Font"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> FontProperty = AvaloniaProperty.Register<Axis, string>(nameof(Font), null);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> FontSizeProperty = AvaloniaProperty.Register<Axis, double>(nameof(FontSize), double.NaN);

        /// <summary>
        /// Identifies the <see cref="FontWeight"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<FontWeight> FontWeightProperty = AvaloniaProperty.Register<Axis, FontWeight>(nameof(FontWeight), FontWeight.Normal);

        /// <summary>
        /// Identifies the <see cref="IntervalLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> IntervalLengthProperty = AvaloniaProperty.Register<Axis, double>(nameof(IntervalLength), 60.0);

        /// <summary>
        /// Identifies the <see cref="IsAxisVisible"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> IsAxisVisibleProperty = AvaloniaProperty.Register<Axis, bool>(nameof(IsAxisVisible), true);

        /// <summary>
        /// Identifies the <see cref="IsPanEnabled"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> IsPanEnabledProperty = AvaloniaProperty.Register<Axis, bool>(nameof(IsPanEnabled), true);

        /// <summary>
        /// Identifies the <see cref="IsZoomEnabled"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> IsZoomEnabledProperty = AvaloniaProperty.Register<Axis, bool>(nameof(IsZoomEnabled), true);

        /// <summary>
        /// Identifies the <see cref="Key"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> KeyProperty = AvaloniaProperty.Register<Axis, string>(nameof(Key), null);

        /// <summary>
        /// Identifies the <see cref="LabelFormatter"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Func<double, string>> LabelFormatterProperty = AvaloniaProperty.Register<Axis, Func<double, string>>(nameof(LabelFormatter), null);

        /// <summary>
        /// Identifies the <see cref="Layer"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Axes.AxisLayer> LayerProperty = AvaloniaProperty.Register<Axis, Axes.AxisLayer>(nameof(Layer), Axes.AxisLayer.BelowSeries);

        /// <summary>
        /// Identifies the <see cref="MajorGridlineColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> MajorGridlineColorProperty = AvaloniaProperty.Register<Axis, Color>(nameof(MajorGridlineColor), Color.FromArgb(0x40, 0, 0, 0));

        /// <summary>
        /// Identifies the <see cref="MajorGridlineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> MajorGridlineStyleProperty = AvaloniaProperty.Register<Axis, LineStyle>(nameof(MajorGridlineStyle), LineStyle.None);

        /// <summary>
        /// Identifies the <see cref="MajorGridlineThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MajorGridlineThicknessProperty = AvaloniaProperty.Register<Axis, double>(nameof(MajorGridlineThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="MajorStep"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MajorStepProperty = AvaloniaProperty.Register<Axis, double>(nameof(MajorStep), double.NaN);

        /// <summary>
        /// Identifies the <see cref="MajorTickSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MajorTickSizeProperty = AvaloniaProperty.Register<Axis, double>(nameof(MajorTickSize), 7.0);

        /// <summary>
        /// Identifies the <see cref="MaximumPadding"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumPaddingProperty = AvaloniaProperty.Register<Axis, double>(nameof(MaximumPadding), 0.01);

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumProperty = AvaloniaProperty.Register<Axis, double>(nameof(Maximum), double.NaN);

        /// <summary>
        /// Identifies the <see cref="MaximumRange"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumRangeProperty = AvaloniaProperty.Register<Axis, double>(nameof(MaximumRange), double.PositiveInfinity);

        /// <summary>
        /// Identifies the <see cref="MinimumPadding"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumPaddingProperty = AvaloniaProperty.Register<Axis, double>(nameof(MinimumPadding), 0.01);

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumProperty = AvaloniaProperty.Register<Axis, double>(nameof(Minimum), double.NaN);

        /// <summary>
        /// Identifies the <see cref="MinimumRange"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumRangeProperty = AvaloniaProperty.Register<Axis, double>(nameof(MinimumRange), 0.0);

        /// <summary>
        /// Identifies the <see cref="MinorGridlineColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> MinorGridlineColorProperty = AvaloniaProperty.Register<Axis, Color>(nameof(MinorGridlineColor), Color.FromArgb(0x20, 0, 0, 0));

        /// <summary>
        /// Identifies the <see cref="MinorGridlineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> MinorGridlineStyleProperty = AvaloniaProperty.Register<Axis, LineStyle>(nameof(MinorGridlineStyle), LineStyle.None);

        /// <summary>
        /// Identifies the <see cref="MinorGridlineThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinorGridlineThicknessProperty = AvaloniaProperty.Register<Axis, double>(nameof(MinorGridlineThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="MinorStep"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinorStepProperty = AvaloniaProperty.Register<Axis, double>(nameof(MinorStep), double.NaN);

        /// <summary>
        /// Identifies the <see cref="MinorTickSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinorTickSizeProperty = AvaloniaProperty.Register<Axis, double>(nameof(MinorTickSize), 4.0);

        /// <summary>
        /// Identifies the <see cref="PositionAtZeroCrossing"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> PositionAtZeroCrossingProperty = AvaloniaProperty.Register<Axis, bool>(nameof(PositionAtZeroCrossing), false);

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Axes.AxisPosition> PositionProperty = AvaloniaProperty.Register<Axis, Axes.AxisPosition>(nameof(Position), Axes.AxisPosition.Left);

        /// <summary>
        /// Identifies the <see cref="PositionTier"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<int> PositionTierProperty = AvaloniaProperty.Register<Axis, int>(nameof(PositionTier), 0);

        /// <summary>
        /// Identifies the <see cref="StartPosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StartPositionProperty = AvaloniaProperty.Register<Axis, double>(nameof(StartPosition), 0.0);

        /// <summary>
        /// Identifies the <see cref="StringFormat"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> StringFormatProperty = AvaloniaProperty.Register<Axis, string>(nameof(StringFormat), null);

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> TextColorProperty = AvaloniaProperty.Register<Axis, Color>(nameof(TextColor), MoreColors.Automatic);
        
        /// <summary>
        /// Identifies the <see cref="TickStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Axes.TickStyle> TickStyleProperty = AvaloniaProperty.Register<Axis, Axes.TickStyle>(nameof(TickStyle), Axes.TickStyle.Outside);

        /// <summary>
        /// Identifies the <see cref="TicklineColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> TicklineColorProperty = AvaloniaProperty.Register<Axis, Color>(nameof(TicklineColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="TitleClippingLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TitleClippingLengthProperty = AvaloniaProperty.Register<Axis, double>(nameof(TitleClippingLength), 0.9);

        /// <summary>
        /// Identifies the <see cref="TitleColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> TitleColorProperty = AvaloniaProperty.Register<Axis, Color>(nameof(TitleColor), OxyColors.Automatic.ToColor());

        /// <summary>
        /// Identifies the <see cref="TitleFont"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TitleFontProperty = AvaloniaProperty.Register<Axis, string>(nameof(TitleFont), null);

        /// <summary>
        /// Identifies the <see cref="TitleFontSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TitleFontSizeProperty = AvaloniaProperty.Register<Axis, double>(nameof(TitleFontSize), double.NaN);

        /// <summary>
        /// Identifies the <see cref="TitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<FontWeight> TitleFontWeightProperty = AvaloniaProperty.Register<Axis, FontWeight>(nameof(TitleFontWeight), FontWeight.Normal);

        /// <summary>
        /// Identifies the <see cref="TitleFormatString"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TitleFormatStringProperty = AvaloniaProperty.Register<Axis, string>(nameof(TitleFormatString), "{0} [{1}]");

        /// <summary>
        /// Identifies the <see cref="TitlePosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TitlePositionProperty = AvaloniaProperty.Register<Axis, double>(nameof(TitlePosition), 0.5);

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<Axis, string>(nameof(Title), null);

        /// <summary>
        /// Identifies the <see cref="Unit"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> UnitProperty = AvaloniaProperty.Register<Axis, string>(nameof(Unit), null);

        /// <summary>
        /// Identifies the <see cref="UseSuperExponentialFormat"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> UseSuperExponentialFormatProperty = AvaloniaProperty.Register<Axis, bool>(nameof(UseSuperExponentialFormat), false);

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
                return GetValue(AbsoluteMaximumProperty);
            }

            set
            {
                SetValue(AbsoluteMaximumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the absolute minimum. This is only used for the UI control. It will not be possible to zoom/pan beyond this limit.
        /// </summary>
        public double AbsoluteMinimum
        {
            get
            {
                return GetValue(AbsoluteMinimumProperty);
            }

            set
            {
                SetValue(AbsoluteMinimumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Angle.
        /// </summary>
        public double Angle
        {
            get
            {
                return GetValue(AngleProperty);
            }

            set
            {
                SetValue(AngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets AxisDistance.
        /// </summary>
        public double AxisDistance
        {
            get
            {
                return GetValue(AxisDistanceProperty);
            }

            set
            {
                SetValue(AxisDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets AxisTickToLabelDistance.
        /// </summary>
        public double AxisTickToLabelDistance
        {
            get
            {
                return GetValue(AxisTickToLabelDistanceProperty);
            }

            set
            {
                SetValue(AxisTickToLabelDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets AxisTitleDistance.
        /// </summary>
        public double AxisTitleDistance
        {
            get
            {
                return GetValue(AxisTitleDistanceProperty);
            }

            set
            {
                SetValue(AxisTitleDistanceProperty, value);
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
                return GetValue(AxislineColorProperty);
            }

            set
            {
                SetValue(AxislineColorProperty, value);
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
                return GetValue(AxislineStyleProperty);
            }

            set
            {
                SetValue(AxislineStyleProperty, value);
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
                return GetValue(AxislineThicknessProperty);
            }

            set
            {
                SetValue(AxislineThicknessProperty, value);
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
                return GetValue(ClipTitleProperty);
            }

            set
            {
                SetValue(ClipTitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets EndPosition.
        /// </summary>
        public double EndPosition
        {
            get
            {
                return GetValue(EndPositionProperty);
            }

            set
            {
                SetValue(EndPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridlineColor.
        /// </summary>
        public Color ExtraGridlineColor
        {
            get
            {
                return GetValue(ExtraGridlineColorProperty);
            }

            set
            {
                SetValue(ExtraGridlineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridlineStyle.
        /// </summary>
        public LineStyle ExtraGridlineStyle
        {
            get
            {
                return GetValue(ExtraGridlineStyleProperty);
            }

            set
            {
                SetValue(ExtraGridlineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridlineThickness.
        /// </summary>
        public double ExtraGridlineThickness
        {
            get
            {
                return GetValue(ExtraGridlineThicknessProperty);
            }

            set
            {
                SetValue(ExtraGridlineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ExtraGridLines.
        /// </summary>
        public double[] ExtraGridlines
        {
            get
            {
                return GetValue(ExtraGridlinesProperty);
            }

            set
            {
                SetValue(ExtraGridlinesProperty, value);
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
                return GetValue(FilterFunctionProperty);
            }

            set
            {
                SetValue(FilterFunctionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FilterMaxValue.
        /// </summary>
        public double FilterMaxValue
        {
            get
            {
                return GetValue(FilterMaxValueProperty);
            }

            set
            {
                SetValue(FilterMaxValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FilterMinValue.
        /// </summary>
        public double FilterMinValue
        {
            get
            {
                return GetValue(FilterMinValueProperty);
            }

            set
            {
                SetValue(FilterMinValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Font.
        /// </summary>
        public string Font
        {
            get
            {
                return GetValue(FontProperty);
            }

            set
            {
                SetValue(FontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FontSize.
        /// </summary>
        public double FontSize
        {
            get
            {
                return GetValue(FontSizeProperty);
            }

            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                return GetValue(FontWeightProperty);
            }

            set
            {
                SetValue(FontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the interval length.
        /// </summary>
        public double IntervalLength
        {
            get
            {
                return GetValue(IntervalLengthProperty);
            }

            set
            {
                SetValue(IntervalLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the axis is visible.
        /// </summary>
        public bool IsAxisVisible
        {
            get
            {
                return GetValue(IsAxisVisibleProperty);
            }

            set
            {
                SetValue(IsAxisVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether pan is enabled.
        /// </summary>
        public bool IsPanEnabled
        {
            get
            {
                return GetValue(IsPanEnabledProperty);
            }

            set
            {
                SetValue(IsPanEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether zoom is enabled.
        /// </summary>
        public bool IsZoomEnabled
        {
            get
            {
                return GetValue(IsZoomEnabledProperty);
            }

            set
            {
                SetValue(IsZoomEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the axis key.
        /// </summary>
        public string Key
        {
            get
            {
                return GetValue(KeyProperty);
            }

            set
            {
                SetValue(KeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label formatter.
        /// </summary>
        public Func<double, string> LabelFormatter
        {
            get
            {
                return GetValue(LabelFormatterProperty);
            }

            set
            {
                SetValue(LabelFormatterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        public Axes.AxisLayer Layer
        {
            get
            {
                return GetValue(LayerProperty);
            }

            set
            {
                SetValue(LayerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the major gridlines.
        /// </summary>
        public Color MajorGridlineColor
        {
            get
            {
                return GetValue(MajorGridlineColorProperty);
            }

            set
            {
                SetValue(MajorGridlineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the line style of the major gridlines.
        /// </summary>
        public LineStyle MajorGridlineStyle
        {
            get
            {
                return GetValue(MajorGridlineStyleProperty);
            }

            set
            {
                SetValue(MajorGridlineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MajorGridlineThickness.
        /// </summary>
        public double MajorGridlineThickness
        {
            get
            {
                return GetValue(MajorGridlineThicknessProperty);
            }

            set
            {
                SetValue(MajorGridlineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MajorStep.
        /// </summary>
        public double MajorStep
        {
            get
            {
                return GetValue(MajorStepProperty);
            }

            set
            {
                SetValue(MajorStepProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MajorTickSize.
        /// </summary>
        public double MajorTickSize
        {
            get
            {
                return GetValue(MajorTickSizeProperty);
            }

            set
            {
                SetValue(MajorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Maximum.
        /// </summary>
        public double Maximum
        {
            get
            {
                return GetValue(MaximumProperty);
            }

            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MaximumPadding.
        /// </summary>
        public double MaximumPadding
        {
            get
            {
                return GetValue(MaximumPaddingProperty);
            }

            set
            {
                SetValue(MaximumPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MaximumRange.
        /// </summary>
        public double MaximumRange
        {
            get
            {
                return GetValue(MaximumRangeProperty);
            }

            set
            {
                SetValue(MaximumRangeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Minimum.
        /// </summary>
        public double Minimum
        {
            get
            {
                return GetValue(MinimumProperty);
            }

            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinimumPadding.
        /// </summary>
        public double MinimumPadding
        {
            get
            {
                return GetValue(MinimumPaddingProperty);
            }

            set
            {
                SetValue(MinimumPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinimumRange.
        /// </summary>
        public double MinimumRange
        {
            get
            {
                return GetValue(MinimumRangeProperty);
            }

            set
            {
                SetValue(MinimumRangeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorGridlineColor.
        /// </summary>
        public Color MinorGridlineColor
        {
            get
            {
                return GetValue(MinorGridlineColorProperty);
            }

            set
            {
                SetValue(MinorGridlineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorGridlineStyle.
        /// </summary>
        public LineStyle MinorGridlineStyle
        {
            get
            {
                return GetValue(MinorGridlineStyleProperty);
            }

            set
            {
                SetValue(MinorGridlineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorGridlineThickness.
        /// </summary>
        public double MinorGridlineThickness
        {
            get
            {
                return GetValue(MinorGridlineThicknessProperty);
            }

            set
            {
                SetValue(MinorGridlineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorStep.
        /// </summary>
        public double MinorStep
        {
            get
            {
                return GetValue(MinorStepProperty);
            }

            set
            {
                SetValue(MinorStepProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorTickSize.
        /// </summary>
        public double MinorTickSize
        {
            get
            {
                return GetValue(MinorTickSizeProperty);
            }

            set
            {
                SetValue(MinorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public Axes.AxisPosition Position
        {
            get
            {
                return GetValue(PositionProperty);
            }

            set
            {
                SetValue(PositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether PositionAtZeroCrossing.
        /// </summary>
        public bool PositionAtZeroCrossing
        {
            get
            {
                return GetValue(PositionAtZeroCrossingProperty);
            }

            set
            {
                SetValue(PositionAtZeroCrossingProperty, value);
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
                return GetValue(PositionTierProperty);
            }

            set
            {
                SetValue(PositionTierProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        public double StartPosition
        {
            get
            {
                return GetValue(StartPositionProperty);
            }

            set
            {
                SetValue(StartPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the string format.
        /// </summary>
        public string StringFormat
        {
            get
            {
                return GetValue(StringFormatProperty);
            }

            set
            {
                SetValue(StringFormatProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text color
        /// </summary>
        public Color TextColor
        {
            get
            {
                return GetValue(TextColorProperty);
            }

            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tick style.
        /// </summary>
        public Axes.TickStyle TickStyle
        {
            get
            {
                return GetValue(TickStyleProperty);
            }

            set
            {
                SetValue(TickStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tick line color.
        /// </summary>
        public Color TicklineColor
        {
            get
            {
                return GetValue(TicklineColorProperty);
            }

            set
            {
                SetValue(TicklineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
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
                return GetValue(TitleClippingLengthProperty);
            }

            set
            {
                SetValue(TitleClippingLengthProperty, value);
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
                return GetValue(TitleColorProperty);
            }

            set
            {
                SetValue(TitleColorProperty, value);
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
                return GetValue(TitleFontProperty);
            }

            set
            {
                SetValue(TitleFontProperty, value);
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
                return GetValue(TitleFontSizeProperty);
            }

            set
            {
                SetValue(TitleFontSizeProperty, value);
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
                return GetValue(TitleFontWeightProperty);
            }

            set
            {
                SetValue(TitleFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TitleFormatString.
        /// </summary>
        public string TitleFormatString
        {
            get
            {
                return GetValue(TitleFormatStringProperty);
            }

            set
            {
                SetValue(TitleFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TitlePosition.
        /// </summary>
        public double TitlePosition
        {
            get
            {
                return GetValue(TitlePositionProperty);
            }

            set
            {
                SetValue(TitlePositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Unit.
        /// </summary>
        public string Unit
        {
            get
            {
                return GetValue(UnitProperty);
            }

            set
            {
                SetValue(UnitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UseSuperExponentialFormat.
        /// </summary>
        public bool UseSuperExponentialFormat
        {
            get
            {
                return GetValue(UseSuperExponentialFormatProperty);
            }

            set
            {
                SetValue(UseSuperExponentialFormatProperty, value);
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
        protected static void AppearanceChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Axis)d).OnVisualChanged();
        }

        /// <summary>
        /// The data changed.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The event args.</param>
        protected static void DataChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Axis)d).OnDataChanged();
        }

        /// <summary>
        /// The on data changed handler.
        /// </summary>
        protected void OnDataChanged()
        {
            var pc = Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        /// <summary>
        /// The on property changed handler.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.OwnerType == GetType())
            {
                var plot = Parent as IPlotView;
                if (plot != null)
                {
                    plot.InvalidatePlot();
                }
            }
        }

        /// <summary>
        /// Handles changed visuals.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = Parent as IPlotView;
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
            var a = InternalAxis;
            a.AbsoluteMaximum = AbsoluteMaximum;
            a.AbsoluteMinimum = AbsoluteMinimum;
            a.Angle = Angle;
            a.AxisDistance = AxisDistance;
            a.AxislineColor = AxislineColor.ToOxyColor();
            a.AxislineStyle = AxislineStyle;
            a.AxislineThickness = AxislineThickness;
            a.AxisTitleDistance = AxisTitleDistance;
            a.AxisTickToLabelDistance = AxisTickToLabelDistance;
            a.ClipTitle = ClipTitle;
            a.EndPosition = EndPosition;
            a.ExtraGridlineColor = ExtraGridlineColor.ToOxyColor();
            a.ExtraGridlineStyle = ExtraGridlineStyle;
            a.ExtraGridlineThickness = ExtraGridlineThickness;
            a.ExtraGridlines = ExtraGridlines;
            a.FilterFunction = FilterFunction;
            a.FilterMaxValue = FilterMaxValue;
            a.FilterMinValue = FilterMinValue;
            a.Font = Font;
            a.FontSize = FontSize;
            a.FontWeight = (int)FontWeight;
            a.IntervalLength = IntervalLength;
            a.IsPanEnabled = IsPanEnabled;
            a.IsAxisVisible = IsAxisVisible;
            a.IsZoomEnabled = IsZoomEnabled;
            a.Key = Key;
            a.Layer = Layer;
            a.MajorGridlineColor = MajorGridlineColor.ToOxyColor();
            a.MinorGridlineColor = MinorGridlineColor.ToOxyColor();
            a.MajorGridlineStyle = MajorGridlineStyle;
            a.MinorGridlineStyle = MinorGridlineStyle;
            a.MajorGridlineThickness = MajorGridlineThickness;
            a.MinorGridlineThickness = MinorGridlineThickness;
            a.MajorStep = MajorStep;
            a.MajorTickSize = MajorTickSize;
            a.MinorStep = MinorStep;
            a.MinorTickSize = MinorTickSize;
            a.Minimum = Minimum;
            a.Maximum = Maximum;
            a.MinimumRange = MinimumRange;
            a.MaximumRange = MaximumRange;
            a.MinimumPadding = MinimumPadding;
            a.MaximumPadding = MaximumPadding;
            a.Position = Position;
            a.PositionTier = PositionTier;
            a.PositionAtZeroCrossing = PositionAtZeroCrossing;
            a.StartPosition = StartPosition;
            a.StringFormat = StringFormat;
            a.TextColor = TextColor.ToOxyColor();
            a.TicklineColor = TicklineColor.ToOxyColor();
            a.TitleClippingLength = TitleClippingLength;
            a.TitleColor = TitleColor.ToOxyColor();
            a.TitleFont = TitleFont;
            a.TitleFontSize = TitleFontSize;
            a.TitleFontWeight = (int)TitleFontWeight;
            a.TitleFormatString = TitleFormatString;
            a.Title = Title;
            a.ToolTip = ToolTip.GetTip(this) != null ? ToolTip.GetTip(this).ToString() : null;
            a.TickStyle = TickStyle;
            a.TitlePosition = TitlePosition;
            a.Unit = Unit;
            a.UseSuperExponentialFormat = UseSuperExponentialFormat;
            a.LabelFormatter = LabelFormatter;
        }

        static Axis()
        {
            AbsoluteMaximumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AbsoluteMinimumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AngleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxisTickToLabelDistanceProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxisTitleDistanceProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxisDistanceProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxislineColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxislineStyleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxislineThicknessProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            ClipTitleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            EndPositionProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            ExtraGridlineColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            ExtraGridlineStyleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            ExtraGridlineThicknessProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            ExtraGridlinesProperty.Changed.AddClassHandler<Axis>(DataChanged);
            FilterFunctionProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            FilterMaxValueProperty.Changed.AddClassHandler<Axis>(DataChanged);
            FilterMinValueProperty.Changed.AddClassHandler<Axis>(DataChanged);
            FontProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            FontSizeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            FontWeightProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            IntervalLengthProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            IsAxisVisibleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            KeyProperty.Changed.AddClassHandler<Axis>(DataChanged);
            LabelFormatterProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            LayerProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorGridlineColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorGridlineStyleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorGridlineThicknessProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorStepProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorTickSizeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MaximumPaddingProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MaximumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MaximumRangeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinimumPaddingProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinimumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinimumRangeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorGridlineColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorGridlineStyleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorGridlineThicknessProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorStepProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorTickSizeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            PositionAtZeroCrossingProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            PositionProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            PositionTierProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            StartPositionProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            StringFormatProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TextColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TickStyleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TicklineColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitleClippingLengthProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitleColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitleFontProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitleFontSizeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitleFontWeightProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitleFormatStringProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitlePositionProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TitleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            UnitProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            UseSuperExponentialFormatProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
        }
    }
}