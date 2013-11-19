// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisBase.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The axis base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The axis base.
    /// </summary>
    public abstract class AxisBase : FrameworkElement, IAxis
    {
        /// <summary>
        /// The absolute maximum property.
        /// </summary>
        public static readonly DependencyProperty AbsoluteMaximumProperty =
            DependencyProperty.Register(
                "AbsoluteMaximum",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(double.MaxValue, VisualChanged));

        /// <summary>
        /// The absolute minimum property.
        /// </summary>
        public static readonly DependencyProperty AbsoluteMinimumProperty =
            DependencyProperty.Register(
                "AbsoluteMinimum",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(double.MinValue, VisualChanged));

        /// <summary>
        /// The angle property.
        /// </summary>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.0, VisualChanged));

        /// <summary>
        /// The axis tick to label distance property.
        /// </summary>
        public static readonly DependencyProperty AxisTickToLabelDistanceProperty =
            DependencyProperty.Register(
                "AxisTickToLabelDistance", typeof(double), typeof(AxisBase), new UIPropertyMetadata(4.0));

        /// <summary>
        /// The axis title distance property.
        /// </summary>
        public static readonly DependencyProperty AxisTitleDistanceProperty =
            DependencyProperty.Register(
                "AxisTitleDistance", typeof(double), typeof(AxisBase), new UIPropertyMetadata(4.0));

        /// <summary>
        /// The end position property.
        /// </summary>
        public static readonly DependencyProperty EndPositionProperty = DependencyProperty.Register(
            "EndPosition", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(1.0, VisualChanged));

        /// <summary>
        /// The extra gridline color property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlineColorProperty =
            DependencyProperty.Register(
                "ExtraGridlineColor",
                typeof(Color),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(Colors.Black, VisualChanged));

        /// <summary>
        /// The extra gridline style property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlineStyleProperty =
            DependencyProperty.Register(
                "ExtraGridlineStyle",
                typeof(LineStyle),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(LineStyle.Solid, VisualChanged));

        /// <summary>
        /// The extra gridline thickness property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlineThicknessProperty =
            DependencyProperty.Register(
                "ExtraGridlineThickness",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(1.0, VisualChanged));

        /// <summary>
        /// The extra gridlines property.
        /// </summary>
        public static readonly DependencyProperty ExtraGridlinesProperty = DependencyProperty.Register(
            "ExtraGridLines", typeof(double[]), typeof(AxisBase), new FrameworkPropertyMetadata(null, DataChanged));

        /// <summary>
        /// The filter max value property.
        /// </summary>
        public static readonly DependencyProperty FilterMaxValueProperty = DependencyProperty.Register(
            "FilterMaxValue",
            typeof(double),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(double.MaxValue, DataChanged));

        /// <summary>
        /// The filter min value property.
        /// </summary>
        public static readonly DependencyProperty FilterMinValueProperty = DependencyProperty.Register(
            "FilterMinValue",
            typeof(double),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(double.MinValue, DataChanged));

        /// <summary>
        /// The font property.
        /// </summary>
        public static readonly DependencyProperty FontProperty = DependencyProperty.Register(
            "Font", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        /// <summary>
        /// The font size property.
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(12.0, VisualChanged));

        /// <summary>
        /// The font weight property.
        /// </summary>
        public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
            "FontWeight",
            typeof(FontWeight),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(FontWeights.Normal, VisualChanged));

        /// <summary>
        /// The interval length property.
        /// </summary>
        public static readonly DependencyProperty IntervalLengthProperty = DependencyProperty.Register(
            "IntervalLength", typeof(double), typeof(AxisBase), new UIPropertyMetadata(60.0));

        /// <summary>
        /// The is axis visible property.
        /// </summary>
        public static readonly DependencyProperty IsAxisVisibleProperty = DependencyProperty.Register(
            "IsAxisVisible", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true, VisualChanged));

        /// <summary>
        /// The is pan enabled property.
        /// </summary>
        public static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register(
            "IsPanEnabled", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// The is zoom enabled property.
        /// </summary>
        public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register(
            "IsZoomEnabled", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// The key property.
        /// </summary>
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, DataChanged));

        /// <summary>
        /// The layer property.
        /// </summary>
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(
            "Layer", typeof(AxisLayer), typeof(AxisBase), new UIPropertyMetadata(AxisLayer.BelowSeries));

        /// <summary>
        /// The major gridline color property.
        /// </summary>
        public static readonly DependencyProperty MajorGridlineColorProperty =
            DependencyProperty.Register(
                "MajorGridlineColor",
                typeof(Color),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(Color.FromArgb(0x40, 0, 0, 0), VisualChanged));

        /// <summary>
        /// The major gridline style property.
        /// </summary>
        public static readonly DependencyProperty MajorGridlineStyleProperty =
            DependencyProperty.Register(
                "MajorGridlineStyle",
                typeof(LineStyle),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(LineStyle.None, VisualChanged));

        /// <summary>
        /// The major gridline thickness property.
        /// </summary>
        public static readonly DependencyProperty MajorGridlineThicknessProperty =
            DependencyProperty.Register(
                "MajorGridlineThickness",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(1.0, VisualChanged));

        /// <summary>
        /// The major step property.
        /// </summary>
        public static readonly DependencyProperty MajorStepProperty = DependencyProperty.Register(
            "MajorStep", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        /// <summary>
        /// The major tick size property.
        /// </summary>
        public static readonly DependencyProperty MajorTickSizeProperty = DependencyProperty.Register(
            "MajorTickSize", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(7.0, VisualChanged));

        /// <summary>
        /// The maximum padding property.
        /// </summary>
        public static readonly DependencyProperty MaximumPaddingProperty = DependencyProperty.Register(
            "MaximumPadding", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.01, VisualChanged));

        /// <summary>
        /// The maximum property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        /// <summary>
        /// The minimum padding property.
        /// </summary>
        public static readonly DependencyProperty MinimumPaddingProperty = DependencyProperty.Register(
            "MinimumPadding", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.01, VisualChanged));

        /// <summary>
        /// The minimum property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        /// <summary>
        /// The minimum range property.
        /// </summary>
        public static readonly DependencyProperty MinimumRangeProperty = DependencyProperty.Register(
            "MinimumRange", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.0, VisualChanged));

        /// <summary>
        /// The minor gridline color property.
        /// </summary>
        public static readonly DependencyProperty MinorGridlineColorProperty =
            DependencyProperty.Register(
                "MinorGridlineColor",
                typeof(Color),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(Color.FromArgb(0x20, 0, 0, 0), VisualChanged));

        /// <summary>
        /// The minor gridline style property.
        /// </summary>
        public static readonly DependencyProperty MinorGridlineStyleProperty =
            DependencyProperty.Register(
                "MinorGridlineStyle",
                typeof(LineStyle),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(LineStyle.None, VisualChanged));

        /// <summary>
        /// The minor gridline thickness property.
        /// </summary>
        public static readonly DependencyProperty MinorGridlineThicknessProperty =
            DependencyProperty.Register(
                "MinorGridlineThickness",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(1.0, VisualChanged));

        /// <summary>
        /// The minor step property.
        /// </summary>
        public static readonly DependencyProperty MinorStepProperty = DependencyProperty.Register(
            "MinorStep", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        /// <summary>
        /// The minor tick size property.
        /// </summary>
        public static readonly DependencyProperty MinorTickSizeProperty = DependencyProperty.Register(
            "MinorTickSize", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(4.0, VisualChanged));

        /// <summary>
        /// The position at zero crossing property.
        /// </summary>
        public static readonly DependencyProperty PositionAtZeroCrossingProperty =
            DependencyProperty.Register(
                "PositionAtZeroCrossing",
                typeof(bool),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(false, VisualChanged));

        /// <summary>
        /// The position property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof(AxisPosition),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(AxisPosition.Left, VisualChanged));

        /// <summary>
        /// The show minor ticks property.
        /// </summary>
        public static readonly DependencyProperty ShowMinorTicksProperty = DependencyProperty.Register(
            "ShowMinorTicks", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true, VisualChanged));

        /// <summary>
        /// The start position property.
        /// </summary>
        public static readonly DependencyProperty StartPositionProperty = DependencyProperty.Register(
            "StartPosition", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.0, VisualChanged));

        /// <summary>
        /// The string format property.
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            "StringFormat", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        /// <summary>
        /// The tick style property.
        /// </summary>
        public static readonly DependencyProperty TickStyleProperty = DependencyProperty.Register(
            "TickStyle",
            typeof(TickStyle),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(TickStyle.Inside, VisualChanged));

        /// <summary>
        /// The tickline color property.
        /// </summary>
        public static readonly DependencyProperty TicklineColorProperty = DependencyProperty.Register(
            "TicklineColor", typeof(Color), typeof(AxisBase), new FrameworkPropertyMetadata(Colors.Black, VisualChanged));

        /// <summary>
        /// The title format string property.
        /// </summary>
        public static readonly DependencyProperty TitleFormatStringProperty =
            DependencyProperty.Register(
                "TitleFormatString",
                typeof(string),
                typeof(AxisBase),
                new FrameworkPropertyMetadata("{0} [{1}]", VisualChanged));

        /// <summary>
        /// The title position property.
        /// </summary>
        public static readonly DependencyProperty TitlePositionProperty = DependencyProperty.Register(
            "TitlePosition", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.5, VisualChanged));

        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        /// <summary>
        /// The unit property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(
            "Unit", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        /// <summary>
        /// The use super exponential format property.
        /// </summary>
        public static readonly DependencyProperty UseSuperExponentialFormatProperty =
            DependencyProperty.Register(
                "UseSuperExponentialFormat",
                typeof(bool),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(false, VisualChanged));

        /// <summary>
        /// Internal axis
        /// </summary>
        protected OxyPlot.IAxis Axis;

        /// <summary>
        /// Gets or sets AbsoluteMaximum.
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
        /// Gets or sets AbsoluteMinimum.
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
        /// Gets or sets ExtraGridLines.
        /// </summary>
        public double[] ExtraGridLines
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
        /// Gets or sets FontWeight.
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
        /// Gets or sets IntervalLength.
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
        /// Gets or sets a value indicating whether IsAxisVisible.
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
        /// Gets or sets a value indicating whether IsPanEnabled.
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
        /// Gets or sets a value indicating whether IsZoomEnabled.
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
        /// Gets or sets Key.
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
        /// Gets or sets Layer.
        /// </summary>
        public AxisLayer Layer
        {
            get
            {
                return (AxisLayer)this.GetValue(LayerProperty);
            }

            set
            {
                this.SetValue(LayerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MajorGridlineColor.
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
        /// Gets or sets MajorGridlineStyle.
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
        public AxisPosition Position
        {
            get
            {
                return (AxisPosition)this.GetValue(PositionProperty);
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
        /// Gets or sets a value indicating whether ShowMinorTicks.
        /// </summary>
        public bool ShowMinorTicks
        {
            get
            {
                return (bool)this.GetValue(ShowMinorTicksProperty);
            }

            set
            {
                this.SetValue(ShowMinorTicksProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets StartPosition.
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
        /// Gets or sets StringFormat.
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
        /// Gets or sets TickStyle.
        /// </summary>
        public TickStyle TickStyle
        {
            get
            {
                return (TickStyle)this.GetValue(TickStyleProperty);
            }

            set
            {
                this.SetValue(TickStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TicklineColor.
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
        /// Gets or sets Title.
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
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract OxyPlot.IAxis CreateModel();

        /// <summary>
        /// The data changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AxisBase)d).OnDataChanged();
        }

        /// <summary>
        /// The visual changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AxisBase)d).OnVisualChanged();
        }

        /// <summary>
        /// The on data changed.
        /// </summary>
        protected void OnDataChanged()
        {
            // post event to  parent
            this.OnVisualChanged();
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.OwnerType == this.GetType())
            {
                var fpm = e.Property.GetMetadata(e.Property.OwnerType) as FrameworkPropertyMetadata;
                if (fpm != null && fpm.AffectsRender)
                {
                    var plot = this.Parent as Plot;
                    plot.InvalidatePlot();
                }
            }
        }

        /// <summary>
        /// The on visual changed.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected virtual void SynchronizeProperties()
        {
            var a = this.Axis as OxyPlot.AxisBase;
            a.AbsoluteMaximum = this.AbsoluteMaximum;
            a.AbsoluteMinimum = this.AbsoluteMinimum;
            a.Angle = this.Angle;
            a.EndPosition = this.EndPosition;
            a.ExtraGridlineColor = this.ExtraGridlineColor.ToOxyColor();
            a.ExtraGridlineStyle = this.ExtraGridlineStyle;
            a.ExtraGridlineThickness = this.ExtraGridlineThickness;
            a.ExtraGridlines = this.ExtraGridLines;
            a.FilterMaxValue = this.FilterMaxValue;
            a.FilterMinValue = this.FilterMinValue;
            a.Font = this.Font;
            a.FontSize = this.FontSize;
            a.FontWeight = this.FontWeight.ToOpenTypeWeight();
            a.IsPanEnabled = this.IsPanEnabled;
            a.IsAxisVisible = this.IsAxisVisible;
            a.IsZoomEnabled = this.IsZoomEnabled;
            a.Key = this.Key;
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
            a.MinimumPadding = this.MinimumPadding;
            a.MaximumPadding = this.MaximumPadding;
            a.Position = this.Position;
            a.PositionAtZeroCrossing = this.PositionAtZeroCrossing;
            a.ShowMinorTicks = this.ShowMinorTicks;
            a.StartPosition = this.StartPosition;
            a.StringFormat = this.StringFormat;
            a.TicklineColor = this.TicklineColor.ToOxyColor();
            a.TitleFormatString = this.TitleFormatString;
            a.Title = this.Title;
            a.TickStyle = this.TickStyle;
            a.TitlePosition = this.TitlePosition;
            a.Unit = this.Unit;
            a.UseSuperExponentialFormat = this.UseSuperExponentialFormat;
            a.AxisTitleDistance = this.AxisTitleDistance;
            a.AxisTickToLabelDistance = this.AxisTickToLabelDistance;
            a.IntervalLength = this.IntervalLength;
            a.Layer = this.Layer;
        }

    }
}