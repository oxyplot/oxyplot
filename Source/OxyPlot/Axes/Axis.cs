// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Axis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot.Series;
    using OxyPlot.Utilities;

    /// <summary>
    /// Provides an abstract base class for axes.
    /// </summary>
    public abstract class Axis : PlotElement
    {
        /// <summary>
        /// Exponent function.
        /// </summary>
        protected static readonly Func<double, double> Exponent = x => Math.Floor(ThresholdRound(Math.Log(Math.Abs(x), 10)));

        /// <summary>
        /// Mantissa function.
        /// </summary>
        protected static readonly Func<double, double> Mantissa = x => ThresholdRound(x / Math.Pow(10, Exponent(x)));

        /// <summary>
        /// Rounds a value if the difference between the rounded value and the original value is less than 1e-6.
        /// </summary>
        protected static readonly Func<double, double> ThresholdRound = x => Math.Abs(Math.Round(x) - x) < 1e-6 ? Math.Round(x) : x;

        /// <summary>
        /// The offset.
        /// </summary>
        private double offset;

        /// <summary>
        /// The scale.
        /// </summary>
        private double scale;

        /// <summary>
        /// The position of the axis.
        /// </summary>
        private AxisPosition position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis" /> class.
        /// </summary>
        protected Axis()
        {
            this.Position = AxisPosition.Left;
            this.PositionTier = 0;
            this.IsAxisVisible = true;
            this.Layer = AxisLayer.BelowSeries;

            this.ViewMaximum = double.NaN;
            this.ViewMinimum = double.NaN;

            this.AbsoluteMaximum = double.MaxValue;
            this.AbsoluteMinimum = double.MinValue;

            this.Minimum = double.NaN;
            this.Maximum = double.NaN;
            this.MinorStep = double.NaN;
            this.MajorStep = double.NaN;
            this.MinimumMinorStep = 0;
            this.MinimumMajorStep = 0;

            this.MinimumPadding = 0.01;
            this.MaximumPadding = 0.01;
            this.MinimumRange = 0;
            this.MaximumRange = double.PositiveInfinity;

            this.TickStyle = TickStyle.Outside;
            this.TicklineColor = OxyColors.Black;
            this.MinorTicklineColor = OxyColors.Automatic;

            this.AxislineStyle = LineStyle.None;
            this.AxislineColor = OxyColors.Black;
            this.AxislineThickness = 1.0;

            this.MajorGridlineStyle = LineStyle.None;
            this.MajorGridlineColor = OxyColor.FromArgb(0x40, 0, 0, 0);
            this.MajorGridlineThickness = 1;

            this.MinorGridlineStyle = LineStyle.None;
            this.MinorGridlineColor = OxyColor.FromArgb(0x20, 0, 0, 0x00);
            this.MinorGridlineThickness = 1;

            this.ExtraGridlineStyle = LineStyle.Solid;
            this.ExtraGridlineColor = OxyColors.Black;
            this.ExtraGridlineThickness = 1;

            this.MinorTickSize = 4;
            this.MajorTickSize = 7;

            this.StartPosition = 0;
            this.EndPosition = 1;

            this.TitlePosition = 0.5;
            this.TitleFormatString = "{0} [{1}]";
            this.TitleClippingLength = 0.9;
            this.TitleColor = OxyColors.Automatic;
            this.TitleFontSize = double.NaN;
            this.TitleFontWeight = FontWeights.Normal;
            this.ClipTitle = true;

            this.Angle = 0;

            this.IsZoomEnabled = true;
            this.IsPanEnabled = true;

            this.FilterMinValue = double.MinValue;
            this.FilterMaxValue = double.MaxValue;
            this.FilterFunction = null;

            this.IntervalLength = 60;

            this.AxisDistance = 0;
            this.AxisTitleDistance = 4;
            this.AxisTickToLabelDistance = 4;

            this.DataMaximum = double.NaN;
            this.DataMinimum = double.NaN;
        }

        /// <summary>
        /// Occurs when the axis has been changed (by zooming, panning or resetting).
        /// </summary>
        [Obsolete("May be removed in v4.0 (#111)")]
        public event EventHandler<AxisChangedEventArgs> AxisChanged;

        /// <summary>
        /// Occurs when the transform changed (size or axis range was changed).
        /// </summary>
        [Obsolete("May be removed in v4.0 (#111)")]
        public event EventHandler TransformChanged;

        /// <summary>
        /// Gets or sets the absolute maximum. This is only used for the UI control. It will not be possible to zoom/pan beyond this limit. The default value is <c>double.MaxValue</c>.
        /// </summary>
        public double AbsoluteMaximum { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum. This is only used for the UI control. It will not be possible to zoom/pan beyond this limit. The default value is <c>double.MinValue</c>.
        /// </summary>
        public double AbsoluteMinimum { get; set; }

        /// <summary>
        /// Gets or sets the actual major step.
        /// </summary>
        public double ActualMajorStep { get; protected set; }

        /// <summary>
        /// Gets or sets the actual maximum value of the axis.
        /// </summary>
        /// <remarks>If <see cref="ViewMaximum" /> is not <c>NaN</c>, this value will be defined by <see cref="ViewMaximum" />.
        /// Otherwise, if <see cref="Maximum" /> is not <c>NaN</c>, this value will be defined by <see cref="Maximum" />.
        /// Otherwise, this value will be defined by the maximum (+padding) of the data.</remarks>
        public double ActualMaximum { get; protected set; }

        /// <summary>
        /// Gets or sets the actual minimum value of the axis.
        /// </summary>
        /// <remarks>If <see cref="ViewMinimum" /> is not <c>NaN</c>, this value will be defined by <see cref="ViewMinimum" />.
        /// Otherwise, if <see cref="Minimum" /> is not <c>NaN</c>, this value will be defined by <see cref="Minimum" />.
        /// Otherwise this value will be defined by the minimum (+padding) of the data.</remarks>
        public double ActualMinimum { get; protected set; }

        /// <summary>
        /// Gets or sets the actual minor step.
        /// </summary>
        public double ActualMinorStep { get; protected set; }

        /// <summary>
        /// Gets or sets the actual string format being used.
        /// </summary>
        public string ActualStringFormat { get; protected set; }

        /// <summary>
        /// Gets the actual title of the axis.
        /// </summary>
        /// <remarks>If the <see cref="Unit" /> property is set, the <see cref="TitleFormatString" /> property is used to format the actual title.</remarks>
        public string ActualTitle
        {
            get
            {
                if (this.Unit != null)
                {
                    return string.Format(this.TitleFormatString, this.Title, this.Unit);
                }

                return this.Title;
            }
        }

        /// <summary>
        /// Gets or sets the orientation angle (degrees) for the axis labels. The default value is <c>0</c>.
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the distance from the end of the tick lines to the labels. The default value is <c>4</c>.
        /// </summary>
        public double AxisTickToLabelDistance { get; set; }

        /// <summary>
        /// Gets or sets the minimum distance from the axis labels to the axis title. The default value is <c>4</c>.
        /// </summary>
        public double AxisTitleDistance { get; set; }

        /// <summary>
        /// Gets or sets the distance between the plot area and the axis. The default value is <c>0</c>.
        /// </summary>
        public double AxisDistance { get; set; }

        /// <summary>
        /// Gets or sets the color of the axis line. The default value is <see cref="OxyColors.Black" />.
        /// </summary>
        public OxyColor AxislineColor { get; set; }

        /// <summary>
        /// Gets or sets the line style of the axis line. The default value is <see cref="LineStyle.None" />.
        /// </summary>
        public LineStyle AxislineStyle { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the axis line. The default value is <c>1</c>.
        /// </summary>
        public double AxislineThickness { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clip the axis title. The default value is <c>true</c>.
        /// </summary>
        public bool ClipTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to crop gridlines with perpendicular axes Start/EndPositions. The default value is <c>false</c>.
        /// </summary>
        public bool CropGridlines { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the data displayed on this axis.
        /// </summary>
        public double DataMaximum { get; protected set; }

        /// <summary>
        /// Gets or sets the minimum value of the data displayed on this axis.
        /// </summary>
        public double DataMinimum { get; protected set; }

        /// <summary>
        /// Gets or sets the end position of the axis on the plot area. The default value is <c>1</c>.
        /// </summary>
        /// <remarks>The position is defined by a fraction in the range from <c>0</c> to <c>1</c>, where <c>0</c> is at the bottom/left
        /// and <c>1</c> is at the top/right. </remarks>
        public double EndPosition { get; set; }

        /// <summary>
        /// Gets or sets the color of the extra gridlines. The default value is <see cref="OxyColors.Black"/>.
        /// </summary>
        public OxyColor ExtraGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the line style of the extra gridlines. The default value is <see cref="LineStyle.Solid"/>.
        /// </summary>
        public LineStyle ExtraGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the extra gridlines. The default value is <c>1</c>.
        /// </summary>
        public double ExtraGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the values for the extra gridlines. The default value is <c>null</c>.
        /// </summary>
        public double[] ExtraGridlines { get; set; }

        /// <summary>
        /// Gets or sets the filter function. The default value is <c>null</c>.
        /// </summary>
        public Func<double, bool> FilterFunction { get; set; }

        /// <summary>
        /// Gets or sets the maximum value that can be shown using this axis. Values greater or equal to this value will not be shown. The default value is <c>double.MaxValue</c>.
        /// </summary>
        public double FilterMaxValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value that can be shown using this axis. Values smaller or equal to this value will not be shown. The default value is <c>double.MinValue</c>.
        /// </summary>
        public double FilterMinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum length (screen space) of the intervals. The available length of the axis will be divided by this length to get the approximate number of major intervals on the axis. The default value is <c>60</c>.
        /// </summary>
        public double IntervalLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this axis is visible. The default value is <c>true</c>.
        /// </summary>
        public bool IsAxisVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether panning is enabled. The default value is <c>true</c>.
        /// </summary>
        public bool IsPanEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this axis is reversed. It is reversed if <see cref="StartPosition" /> &gt; <see cref="EndPosition" />.
        /// </summary>
        public bool IsReversed
        {
            get
            {
                return this.StartPosition > this.EndPosition;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether zooming is enabled. The default value is <c>true</c>.
        /// </summary>
        public bool IsZoomEnabled { get; set; }

        /// <summary>
        /// Gets or sets the key of the axis. This can be used to specify an axis if you have defined multiple axes in a plot. The default value is <c>null</c>.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the formatting function for the labels. The default value is <c>null</c>.
        /// </summary>
        /// <remarks>This function can be used instead of overriding the <see cref="FormatValue" /> method.</remarks>
        public Func<double, string> LabelFormatter { get; set; }

        /// <summary>
        /// Gets or sets the layer of the axis. The default value is <see cref="AxisLayer.BelowSeries"/>.
        /// </summary>
        public AxisLayer Layer { get; set; }

        /// <summary>
        /// Gets or sets the color of the major gridlines. The default value is <c>#40000000</c>.
        /// </summary>
        public OxyColor MajorGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the line style of the major gridlines. The default value is <see cref="LineStyle.None"/>.
        /// </summary>
        public LineStyle MajorGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the major gridlines. The default value is <c>1</c>.
        /// </summary>
        public double MajorGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the interval between major ticks. The default value is <c>double.NaN</c>.
        /// </summary>
        public double MajorStep { get; set; }

        /// <summary>
        /// Gets or sets the size of the major ticks. The default value is <c>7</c>.
        /// </summary>
        public double MajorTickSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the axis. The default value is <c>double.NaN</c>.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the 'padding' fraction of the maximum value. The default value is <c>0.01</c>.
        /// </summary>
        /// <remarks> A value of 0.01 gives 1% more space on the maximum end of the axis. This property is not used if the <see cref="Maximum" /> property is set.</remarks>
        public double MaximumPadding { get; set; }

        /// <summary>
        /// Gets or sets the maximum range of the axis. Setting this property ensures that <c>ActualMaximum-ActualMinimum &lt; MaximumRange</c>. The default value is <c>double.PositiveInfinity</c>.
        /// </summary>
        public double MaximumRange { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the axis. The default value is <c>double.NaN</c>.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for the interval between major ticks. The default value is <c>0</c>.
        /// </summary>
        public double MinimumMajorStep { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for the interval between minor ticks. The default value is <c>0</c>.
        /// </summary>
        public double MinimumMinorStep { get; set; }

        /// <summary>
        /// Gets or sets the 'padding' fraction of the minimum value. The default value is <c>0.01</c>.
        /// </summary>
        /// <remarks>A value of 0.01 gives 1% more space on the minimum end of the axis. This property is not used if the <see cref="Minimum" /> property is set.</remarks>
        public double MinimumPadding { get; set; }

        /// <summary>
        /// Gets or sets the minimum range of the axis. Setting this property ensures that <c>ActualMaximum-ActualMinimum > MinimumRange</c>. The default value is <c>0</c>.
        /// </summary>
        public double MinimumRange { get; set; }

        /// <summary>
        /// Gets or sets the color of the minor gridlines. The default value is <c>#20000000</c>.
        /// </summary>
        public OxyColor MinorGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the line style of the minor gridlines. The default value is <see cref="LineStyle.None"/>.
        /// </summary>
        public LineStyle MinorGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the minor gridlines. The default value is <c>1</c>.
        /// </summary>
        public double MinorGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the interval between minor ticks. The default value is <c>double.NaN</c>.
        /// </summary>
        public double MinorStep { get; set; }

        /// <summary>
        /// Gets or sets the color of the minor ticks. The default value is <see cref="OxyColors.Automatic"/>.
        /// </summary>
        /// <remarks>If the value is <see cref="OxyColors.Automatic"/>, the value of
        /// <see cref="Axis.TicklineColor"/> will be used.</remarks>
        public OxyColor MinorTicklineColor { get; set; }

        /// <summary>
        /// Gets or sets the size of the minor ticks. The default value is <c>4</c>.
        /// </summary>
        public double MinorTickSize { get; set; }

        /// <summary>
        /// Gets the offset. This is used to transform between data and screen coordinates.
        /// </summary>
        public double Offset
        {
            get
            {
                return this.offset;
            }
        }

        /// <summary>
        /// Gets or sets the position of the axis. The default value is <see cref="AxisPosition.Left"/>.
        /// </summary>
        public AxisPosition Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the axis should be positioned at the zero-crossing of the related axis. The default value is <c>false</c>.
        /// </summary>
        public bool PositionAtZeroCrossing { get; set; }

        /// <summary>
        /// Gets or sets the position tier which defines in which tier the axis is displayed. The default value is <c>0</c>.
        /// </summary>
        /// <remarks>The bigger the value the further afar is the axis from the graph.</remarks>
        public int PositionTier { get; set; }

        /// <summary>
        /// Gets the scaling factor of the axis. This is used to transform between data and screen coordinates.
        /// </summary>
        public double Scale
        {
            get
            {
                return this.scale;
            }
        }

        /// <summary>
        /// Gets or sets the screen coordinate of the maximum end of the axis.
        /// </summary>
        public ScreenPoint ScreenMax { get; protected set; }

        /// <summary>
        /// Gets or sets the screen coordinate of the minimum end of the axis.
        /// </summary>
        public ScreenPoint ScreenMin { get; protected set; }

        /// <summary>
        /// Gets or sets the start position of the axis on the plot area. The default value is <c>0</c>.
        /// </summary>
        /// <remarks>The position is defined by a fraction in the range from <c>0</c> to <c>1</c>, where <c>0</c> is at the bottom/left
        /// and <c>1</c> is at the top/right. </remarks>
        public double StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the string format used for formatting the axis values. The default value is <c>null</c>.
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or sets the tick style for major and minor ticks. The default value is <see cref="OxyPlot.Axes.TickStyle.Outside"/>.
        /// </summary>
        public TickStyle TickStyle { get; set; }

        /// <summary>
        /// Gets or sets the color of the major and minor ticks. The default value is <see cref="OxyColors.Black"/>.
        /// </summary>
        public OxyColor TicklineColor { get; set; }

        /// <summary>
        /// Gets or sets the title of the axis. The default value is <c>null</c>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the length of the title clipping rectangle (fraction of the available length of the axis). The default value is <c>0.9</c>.
        /// </summary>
        public double TitleClippingLength { get; set; }

        /// <summary>
        /// Gets or sets the color of the title. The default value is <see cref="OxyColors.Automatic"/>.
        /// </summary>
        /// <remarks>If the value is <c>null</c>, the <see cref="PlotModel.TextColor" /> will be used.</remarks>
        public OxyColor TitleColor { get; set; }

        /// <summary>
        /// Gets or sets the title font. The default value is <c>null</c>.
        /// </summary>
        public string TitleFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the title font. The default value is <c>double.NaN</c>.
        /// </summary>
        public double TitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the weight of the title font. The default value is <see cref="FontWeights.Normal"/>.
        /// </summary>
        public double TitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the format string used for formatting the title and unit when <see cref="Unit" /> is defined. 
        /// The default value is "{0} [{1}]", where {0} refers to the <see cref="Title" /> and {1} refers to the <see cref="Unit" />.
        /// </summary>
        /// <remarks>If <see cref="Unit" /> is <c>null</c>, the actual title is defined by <see cref="Title" /> only.</remarks>
        public string TitleFormatString { get; set; }

        /// <summary>
        /// Gets or sets the position of the title. The default value is <c>0.5</c>.
        /// </summary>
        /// <remarks>The position is defined by a fraction in the range <c>0</c> to <c>1</c>.</remarks>
        public double TitlePosition { get; set; }

        /// <summary>
        /// Gets or sets the unit of the axis. The default value is <c>null</c>.
        /// </summary>
        /// <remarks>The <see cref="TitleFormatString" /> is used to format the title including this unit.</remarks>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use superscript exponential format. The default value is <c>false</c>.
        /// </summary>
        /// <remarks>
        /// This format will convert 1.5E+03 to 1.5·10^{3} and render the superscript properly.
        /// If <see cref="StringFormat" /> is <c>null</c>, 1.0E+03 will be converted to 10^{3}, otherwise it will use the format string for the mantissa.
        /// </remarks>
        public bool UseSuperExponentialFormat { get; set; }

        /// <summary>
        /// Gets or sets the desired margins such that the axis text ticks will not be clipped.
        /// The actual margins may be smaller or larger than the desired margins if they are set manually.
        /// </summary>
        public OxyThickness DesiredMargin { get; protected set; }

        /// <summary>
        /// Gets or sets the position tier max shift.
        /// </summary>
        internal double PositionTierMaxShift { get; set; }

        /// <summary>
        /// Gets or sets the position tier min shift.
        /// </summary>
        internal double PositionTierMinShift { get; set; }

        /// <summary>
        /// Gets or sets the size of the position tier.
        /// </summary>
        internal double PositionTierSize { get; set; }

        /// <summary>
        /// Gets the actual color of the title.
        /// </summary>
        protected internal OxyColor ActualTitleColor
        {
            get
            {
                return this.TitleColor.GetActualColor(this.PlotModel.TextColor);
            }
        }

        /// <summary>
        /// Gets the actual title font.
        /// </summary>
        protected internal string ActualTitleFont
        {
            get
            {
                return this.TitleFont ?? this.PlotModel.DefaultFont;
            }
        }

        /// <summary>
        /// Gets the actual size of the title font.
        /// </summary>
        protected internal double ActualTitleFontSize
        {
            get
            {
                return !double.IsNaN(this.TitleFontSize) ? this.TitleFontSize : this.ActualFontSize;
            }
        }

        /// <summary>
        /// Gets the actual title font weight.
        /// </summary>
        protected internal double ActualTitleFontWeight
        {
            get
            {
                return !double.IsNaN(this.TitleFontWeight) ? this.TitleFontWeight : this.ActualFontWeight;
            }
        }

        /// <summary>
        /// Gets or sets the current view's maximum. This value is used when the user zooms or pans.
        /// </summary>
        /// <value>The view maximum.</value>
        protected double ViewMaximum { get; set; }

        /// <summary>
        /// Gets or sets the current view's minimum. This value is used when the user zooms or pans.
        /// </summary>
        /// <value>The view minimum.</value>
        protected double ViewMinimum { get; set; }

        /// <summary>
        /// Converts the value of the specified object to a double precision floating point number. DateTime objects are converted using DateTimeAxis.ToDouble and TimeSpan objects are converted using TimeSpanAxis.ToDouble
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The floating point number value.</returns>
        public static double ToDouble(object value)
        {
            if (value is DateTime)
            {
                return DateTimeAxis.ToDouble((DateTime)value);
            }

            if (value is TimeSpan)
            {
                return TimeSpanAxis.ToDouble((TimeSpan)value);
            }

            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Transforms the specified point from screen space to data space.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <param name="xaxis">The x axis.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns>The data point.</returns>
        public static DataPoint InverseTransform(ScreenPoint p, Axis xaxis, Axis yaxis)
        {
            return xaxis.InverseTransform(p.x, p.y, yaxis);
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value.</param>
        /// <returns>The formatted value.</returns>
        public string FormatValue(double x)
        {
            if (this.LabelFormatter != null)
            {
                return this.LabelFormatter(x);
            }

            return this.FormatValueOverride(x);
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public virtual void GetTickValues(
            out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            minorTickValues = this.CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMinorStep);
            majorTickValues = this.CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMajorStep);
            majorLabelValues = majorTickValues;

            minorTickValues = AxisUtilities.FilterRedundantMinorTicks(majorTickValues, minorTickValues);
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from a coordinate <see cref="double" /> value to the actual data type.
        /// </summary>
        /// <param name="x">The coordinate.</param>
        /// <returns>The converted value.</returns>
        /// <remarks>Examples: The <see cref="DateTimeAxis" /> returns the <see cref="DateTime" /> and <see cref="CategoryAxis" /> returns category strings.</remarks>
        public virtual object GetValue(double x)
        {
            return x;
        }

        /// <summary>
        /// Inverse transform the specified screen point.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="yaxis">The y-axis.</param>
        /// <returns>The data point.</returns>
        public virtual DataPoint InverseTransform(double x, double y, Axis yaxis)
        {
            return new DataPoint(this.InverseTransform(x), yaxis != null ? yaxis.InverseTransform(y) : 0);
        }

        /// <summary>
        /// Inverse transforms the specified screen coordinate. This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="sx">The screen coordinate.</param>
        /// <returns>The value.</returns>
        public virtual double InverseTransform(double sx)
        {
            return (sx / this.scale) + this.offset;
        }

        /// <summary>
        /// Determines whether the axis is horizontal.
        /// </summary>
        /// <returns><c>true</c> if the axis is horizontal; otherwise, <c>false</c> .</returns>
        public bool IsHorizontal()
        {
            return this.position == AxisPosition.Top || this.position == AxisPosition.Bottom;
        }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c> .</returns>
        public bool IsValidValue(double value)
        {
#pragma warning disable 1718
            // ReSharper disable EqualExpressionComparison
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return value == value &&
                value != 1.0 / 0.0 &&
                value != -1.0 / 0.0 &&
                value < this.FilterMaxValue &&
                value > this.FilterMinValue &&
                (this.FilterFunction == null || this.FilterFunction(value));
            // ReSharper restore CompareOfFloatsByEqualityOperator
            // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
        }

        /// <summary>
        /// Determines whether the axis is vertical.
        /// </summary>
        /// <returns><c>true</c> if the axis is vertical; otherwise, <c>false</c> .</returns>
        public bool IsVertical()
        {
            return this.position == AxisPosition.Left || this.position == AxisPosition.Right;
        }

        /// <summary>
        /// Determines whether the axis is used for X/Y values.
        /// </summary>
        /// <returns><c>true</c> if it is an XY axis; otherwise, <c>false</c> .</returns>
        public abstract bool IsXyAxis();

        /// <summary>
        /// Determines whether the axis is logarithmic.
        /// </summary>
        /// <returns><c>true</c> if it is a logarithmic axis; otherwise, <c>false</c> .</returns>
        public virtual bool IsLogarithmic()
        {
            return false;
        }

        /// <summary>
        /// Measures the size of the axis and updates <see cref="DesiredMargin"/> accordingly. This takes into account the axis title as well as tick labels
        /// potentially exceeding the axis range.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public virtual void Measure(IRenderContext rc)
        {
            if (this.Position == AxisPosition.None)
            {
                this.DesiredMargin = new OxyThickness(0);
                return;
            }

            this.GetTickValues(out var majorLabelValues, out _, out _);

            var maximumTextSize = new OxySize();
            foreach (var v in majorLabelValues)
            {
                var s = this.FormatValue(v);
                var size = rc.MeasureText(s, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle);
                maximumTextSize = maximumTextSize.Include(size);
            }

            var titleTextSize = rc.MeasureText(this.ActualTitle, this.ActualTitleFont, this.ActualTitleFontSize, this.ActualTitleFontWeight);

            var marginLeft = 0d;
            var marginTop = 0d;
            var marginRight = 0d;
            var marginBottom = 0d;

            var margin = this.TickStyle switch
            {
                TickStyle.Outside => this.MajorTickSize,
                TickStyle.Crossing => this.MajorTickSize * 0.75,
                _ => 0
            };

            margin += this.AxisDistance + this.AxisTickToLabelDistance;

            if (titleTextSize.Height > 0)
            {
                margin += this.AxisTitleDistance + titleTextSize.Height;
            }

            switch (this.Position)
            {
                case AxisPosition.Left:
                    marginLeft = margin + maximumTextSize.Width;
                    break;
                case AxisPosition.Right:
                    marginRight = margin + maximumTextSize.Width;
                    break;
                case AxisPosition.Top:
                    marginTop = margin + maximumTextSize.Height;
                    break;
                case AxisPosition.Bottom:
                    marginBottom = margin + maximumTextSize.Height;
                    break;
                case AxisPosition.All:
                    marginLeft = marginRight = margin + maximumTextSize.Width;
                    marginTop = marginBottom = margin + maximumTextSize.Height;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (this.IsPanEnabled || this.IsZoomEnabled)
            {
                var reachesMinPosition = Math.Min(this.StartPosition, this.EndPosition) < 0.01;
                var reachesMaxPosition = Math.Max(this.StartPosition, this.EndPosition) > 0.99;

                switch (this.Position)
                {
                    case AxisPosition.Left:
                    case AxisPosition.Right:
                        if (reachesMinPosition)
                        {
                            marginBottom = maximumTextSize.Height / 2;
                        }

                        if (reachesMaxPosition)
                        {
                            marginTop = maximumTextSize.Height / 2;
                        }

                        break;
                    case AxisPosition.Top:
                    case AxisPosition.Bottom:
                        if (reachesMinPosition)
                        {
                            marginLeft = maximumTextSize.Width / 2;
                        }

                        if (reachesMaxPosition)
                        {
                            marginRight = maximumTextSize.Width / 2;
                        }

                        break;
                }
            }
            else if (majorLabelValues.Count > 0)
            {
                var minLabel = majorLabelValues.Min();
                var maxLabel = majorLabelValues.Max();

                var minLabelText = this.FormatValue(minLabel);
                var maxLabelText = this.FormatValue(maxLabel);

                var minLabelSize = rc.MeasureText(minLabelText, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle);
                var maxLabelSize = rc.MeasureText(maxLabelText, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle);

                var minLabelPosition = this.Transform(minLabel);
                var maxLabelPosition = this.Transform(maxLabel);

                if (minLabelPosition > maxLabelPosition)
                {
                    Helpers.Swap(ref minLabelPosition, ref maxLabelPosition);
                    Helpers.Swap(ref minLabelSize, ref maxLabelSize);
                }

                switch (this.Position)
                {
                    case AxisPosition.Left:
                    case AxisPosition.Right:
                        var screenMinY = Math.Min(this.ScreenMin.Y, this.ScreenMax.Y);
                        var screenMaxY = Math.Max(this.ScreenMin.Y, this.ScreenMax.Y);

                        marginTop = Math.Max(0, screenMinY - minLabelPosition + (minLabelSize.Height / 2));
                        marginBottom = Math.Max(0, maxLabelPosition - screenMaxY + (maxLabelSize.Height / 2));
                        break;
                    case AxisPosition.Top:
                    case AxisPosition.Bottom:
                        var screenMinX = Math.Min(this.ScreenMin.X, this.ScreenMax.X);
                        var screenMaxX = Math.Max(this.ScreenMin.X, this.ScreenMax.X);

                        marginLeft = Math.Max(0, screenMinX - minLabelPosition + (minLabelSize.Width / 2));
                        marginRight = Math.Max(0, maxLabelPosition - screenMaxX + (maxLabelSize.Width / 2));
                        break;
                }
            }

            this.DesiredMargin = new OxyThickness(marginLeft, marginTop, marginRight, marginBottom);
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="ppt">The previous point (screen coordinates).</param>
        /// <param name="cpt">The current point (screen coordinates).</param>
        public virtual void Pan(ScreenPoint ppt, ScreenPoint cpt)
        {
            if (!this.IsPanEnabled)
            {
                return;
            }

            bool isHorizontal = this.IsHorizontal();

            double dsx = isHorizontal ? cpt.X - ppt.X : cpt.Y - ppt.Y;
            this.Pan(dsx);
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public virtual void Pan(double delta)
        {
            if (!this.IsPanEnabled)
            {
                return;
            }

            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

            double dx = delta / this.Scale;

            double newMinimum = this.ActualMinimum - dx;
            double newMaximum = this.ActualMaximum - dx;
            if (newMinimum < this.AbsoluteMinimum)
            {
                newMinimum = this.AbsoluteMinimum;
                newMaximum = Math.Min(newMinimum + this.ActualMaximum - this.ActualMinimum, this.AbsoluteMaximum);
            }

            if (newMaximum > this.AbsoluteMaximum)
            {
                newMaximum = this.AbsoluteMaximum;
                newMinimum = Math.Max(newMaximum - (this.ActualMaximum - this.ActualMinimum), this.AbsoluteMinimum);
            }

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;
            this.UpdateActualMaxMin();

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan, deltaMinimum, deltaMaximum));
        }

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pass">The pass.</param>
        public virtual void Render(IRenderContext rc, int pass)
        {
            if (this.Position == AxisPosition.None)
            {
                return;
            }

            var r = new HorizontalAndVerticalAxisRenderer(rc, this.PlotModel);
            r.Render(this, pass);
        }

        /// <summary>
        /// Resets the user's modification (zooming/panning) to minimum and maximum of this axis.
        /// </summary>
        public virtual void Reset()
        {
            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

            this.ViewMinimum = double.NaN;
            this.ViewMaximum = double.NaN;
            this.UpdateActualMaxMin();

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Reset, deltaMinimum, deltaMaximum));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                this.ActualCulture,
                "{0}({1}, {2}, {3}, {4})",
                this.GetType().Name,
                this.Position,
                this.ActualMinimum,
                this.ActualMaximum,
                this.ActualMajorStep);
        }

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="x">The x value (for the current axis).</param>
        /// <param name="y">The y value.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns>The transformed point.</returns>
        public virtual ScreenPoint Transform(double x, double y, Axis yaxis)
        {
            if (yaxis == null)
            {
                throw new NullReferenceException("Y axis should not be null when transforming.");
            }

            return new ScreenPoint(this.Transform(x), yaxis.Transform(y));
        }

        /// <summary>
        /// Transforms the specified coordinate to screen coordinates. This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="x">The value.</param>
        /// <returns>The transformed value (screen coordinate).</returns>
        public virtual double Transform(double x)
        {
#if DEBUG
            // check if the screen coordinate is very big, this could cause issues
            // only do this in DEBUG builds, as it affects performance
            var s = (x - this.offset) * this.scale;
            if (s * s > 1e12)
            {
                throw new InvalidOperationException($"Invalid transform (screen coordinate={s}). This could cause issues with the presentation framework.");
            }

            return s;
#else
            return (x - this.offset) * this.scale;
#endif
        }

        /// <summary>
        /// Zoom to the specified scale.
        /// </summary>
        /// <param name="newScale">The new scale.</param>
        public virtual void Zoom(double newScale)
        {
            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

            double sx1 = this.Transform(this.ActualMaximum);
            double sx0 = this.Transform(this.ActualMinimum);

            double sgn = Math.Sign(this.scale);
            double mid = (this.ActualMaximum + this.ActualMinimum) / 2;

            double dx = (this.offset - mid) * this.scale;
            var newOffset = (dx / (sgn * newScale)) + mid;
            this.SetTransform(sgn * newScale, newOffset);

            double newMaximum = this.InverseTransform(sx1);
            double newMinimum = this.InverseTransform(sx0);

            if (newMinimum < this.AbsoluteMinimum && newMaximum > this.AbsoluteMaximum)
            {
                newMinimum = this.AbsoluteMinimum;
                newMaximum = this.AbsoluteMaximum;
            }
            else
            {
                if (newMinimum < this.AbsoluteMinimum)
                {
                    double d = newMaximum - newMinimum;
                    newMinimum = this.AbsoluteMinimum;
                    newMaximum = this.AbsoluteMinimum + d;
                    if (newMaximum > this.AbsoluteMaximum)
                    {
                        newMaximum = this.AbsoluteMaximum;
                    }
                }
                else if (newMaximum > this.AbsoluteMaximum)
                {
                    double d = newMaximum - newMinimum;
                    newMaximum = this.AbsoluteMaximum;
                    newMinimum = this.AbsoluteMaximum - d;
                    if (newMinimum < this.AbsoluteMinimum)
                    {
                        newMinimum = this.AbsoluteMinimum;
                    }
                }
            }

            this.ViewMaximum = newMaximum;
            this.ViewMinimum = newMinimum;
            this.UpdateActualMaxMin();

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, deltaMinimum, deltaMaximum));
        }

        /// <summary>
        /// Zooms the axis to the range [x0,x1].
        /// </summary>
        /// <param name="x0">The new minimum.</param>
        /// <param name="x1">The new maximum.</param>
        public virtual void Zoom(double x0, double x1)
        {
            if (!this.IsZoomEnabled)
            {
                return;
            }

            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

            double newMinimum = Math.Max(Math.Min(x0, x1), this.AbsoluteMinimum);
            double newMaximum = Math.Min(Math.Max(x0, x1), this.AbsoluteMaximum);

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;
            this.UpdateActualMaxMin();

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, deltaMinimum, deltaMaximum));
        }

        /// <summary>
        /// Zooms the axis at the specified coordinate.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        /// <param name="x">The coordinate to zoom at.</param>
        public virtual void ZoomAt(double factor, double x)
        {
            if (!this.IsZoomEnabled)
            {
                return;
            }

            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

            double dx0 = (this.ActualMinimum - x) * this.scale;
            double dx1 = (this.ActualMaximum - x) * this.scale;
            this.scale *= factor;

            double newMinimum = (dx0 / this.scale) + x;
            double newMaximum = (dx1 / this.scale) + x;

            if (newMaximum - newMinimum > this.MaximumRange)
            {
                var mid = (newMinimum + newMaximum) * 0.5;
                newMaximum = mid + (this.MaximumRange * 0.5);
                newMinimum = mid - (this.MaximumRange * 0.5);
            }

            if (newMaximum - newMinimum < this.MinimumRange)
            {
                var mid = (newMinimum + newMaximum) * 0.5;
                newMaximum = mid + (this.MinimumRange * 0.5);
                newMinimum = mid - (this.MinimumRange * 0.5);
            }

            newMinimum = Math.Max(newMinimum, this.AbsoluteMinimum);
            newMaximum = Math.Min(newMaximum, this.AbsoluteMaximum);

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;
            this.UpdateActualMaxMin();

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, deltaMinimum, deltaMaximum));
        }

        /// <summary>
        /// Zooms the axis with the specified zoom factor at the center of the axis.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        public virtual void ZoomAtCenter(double factor)
        {
            double sx = (this.Transform(this.ActualMaximum) + this.Transform(this.ActualMinimum)) * 0.5;
            var x = this.InverseTransform(sx);
            this.ZoomAt(factor, x);
        }

        /// <summary>
        /// Modifies the data range of the axis [DataMinimum,DataMaximum] to includes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void Include(double value)
        {
            if (!this.IsValidValue(value))
            {
                return;
            }

            this.DataMinimum = double.IsNaN(this.DataMinimum) ? value : Math.Min(this.DataMinimum, value);
            this.DataMaximum = double.IsNaN(this.DataMaximum) ? value : Math.Max(this.DataMaximum, value);
        }

        /// <summary>
        /// Resets the <see cref="DataMaximum" /> and <see cref="DataMinimum" /> values.
        /// </summary>
        internal virtual void ResetDataMaxMin()
        {
            this.DataMaximum = this.DataMinimum = this.ActualMaximum = this.ActualMinimum = double.NaN;
        }

        /// <summary>
        /// Updates the <see cref="ActualMaximum" /> and <see cref="ActualMinimum" /> values.
        /// </summary>
        /// <remarks>If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum
        /// values will be used. If Maximum or Minimum have been set, these values will be used. Otherwise the maximum and minimum values
        /// of the series will be used, including the 'padding'.</remarks>
        internal virtual void UpdateActualMaxMin()
        {
            if (!double.IsNaN(this.ViewMaximum))
            {
                // The user has zoomed/panned the axis, use the ViewMaximum value.
                this.ActualMaximum = this.ViewMaximum;
            }
            else if (!double.IsNaN(this.Maximum))
            {
                // The Maximum value has been set
                this.ActualMaximum = this.Maximum;
            }
            else
            {
                // Calculate the actual maximum, including padding
                this.ActualMaximum = this.CalculateActualMaximum();
            }

            if (!double.IsNaN(this.ViewMinimum))
            {
                this.ActualMinimum = this.ViewMinimum;
            }
            else if (!double.IsNaN(this.Minimum))
            {
                this.ActualMinimum = this.Minimum;
            }
            else
            {
                this.ActualMinimum = this.CalculateActualMinimum();
            }

            this.CoerceActualMaxMin();
        }

        /// <summary>
        /// Updates the actual minor and major step intervals.
        /// </summary>
        /// <param name="plotArea">The plot area rectangle.</param>
        internal virtual void UpdateIntervals(OxyRect plotArea)
        {
            double labelSize = this.IntervalLength;
            double length = this.IsHorizontal() ? plotArea.Width : plotArea.Height;
            length *= Math.Abs(this.EndPosition - this.StartPosition);

            this.ActualMajorStep = !double.IsNaN(this.MajorStep)
                                       ? this.MajorStep
                                       : this.CalculateActualInterval(length, labelSize);

            this.ActualMinorStep = !double.IsNaN(this.MinorStep)
                                       ? this.MinorStep
                                       : this.CalculateMinorInterval(this.ActualMajorStep);

            if (double.IsNaN(this.ActualMinorStep))
            {
                this.ActualMinorStep = 2;
            }

            if (double.IsNaN(this.ActualMajorStep))
            {
                this.ActualMajorStep = 10;
            }

            this.ActualMinorStep = Math.Max(this.ActualMinorStep, this.MinimumMinorStep);
            this.ActualMajorStep = Math.Max(this.ActualMajorStep, this.MinimumMajorStep);

            this.ActualStringFormat = this.StringFormat ?? this.GetDefaultStringFormat();
        }

        /// <summary>
        /// Updates the scale and offset properties of the transform from the specified boundary rectangle.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        internal virtual void UpdateTransform(OxyRect bounds)
        {
            double x0 = bounds.Left;
            double x1 = bounds.Right;
            double y0 = bounds.Bottom;
            double y1 = bounds.Top;

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            double a0 = this.IsHorizontal() ? x0 : y0;
            double a1 = this.IsHorizontal() ? x1 : y1;

            double dx = a1 - a0;
            a1 = a0 + (this.EndPosition * dx);
            a0 = a0 + (this.StartPosition * dx);
            this.ScreenMin = new ScreenPoint(a0, a1);
            this.ScreenMax = new ScreenPoint(a1, a0);

            if (this.ActualMaximum - this.ActualMinimum < double.Epsilon)
            {
                this.ActualMaximum = this.ActualMinimum + 1;
            }

            double max = this.PreTransform(this.ActualMaximum);
            double min = this.PreTransform(this.ActualMinimum);

            double da = a0 - a1;
            double newOffset, newScale;
            if (Math.Abs(da) > double.Epsilon)
            {
                newOffset = (a0 / da * max) - (a1 / da * min);
            }
            else
            {
                newOffset = 0;
            }

            double range = max - min;
            if (Math.Abs(range) > double.Epsilon)
            {
                newScale = (a1 - a0) / range;
            }
            else
            {
                newScale = 1;
            }

            this.SetTransform(newScale, newOffset);
        }

        /// <summary>
        /// Gets the default format string.
        /// </summary>
        /// <returns>A format string.</returns>
        /// <remarks>This format string is used if the StringFormat is not set.</remarks>
        protected virtual string GetDefaultStringFormat()
        {
            return "g6";
        }

        /// <summary>
        /// Applies a transformation after the inverse transform of the value.
        /// </summary>
        /// <param name="x">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        /// <remarks>If this method is overridden, the <see cref="InverseTransform(double)" /> method must also be overridden.
        /// See <see cref="LogarithmicAxis" /> for examples on how to implement this.</remarks>
        protected virtual double PostInverseTransform(double x)
        {
            return x;
        }

        /// <summary>
        /// Applies a transformation before the transform the value.
        /// </summary>
        /// <param name="x">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        /// <remarks>If this method is overridden, the <see cref="Transform(double)" /> method must also be overridden.
        /// See <see cref="LogarithmicAxis" /> for examples on how to implement this.</remarks>
        protected virtual double PreTransform(double x)
        {
            return x;
        }

        /// <summary>
        /// Calculates the minor interval.
        /// </summary>
        /// <param name="majorInterval">The major interval.</param>
        /// <returns>The minor interval.</returns>
        protected virtual double CalculateMinorInterval(double majorInterval)
        {
            return AxisUtilities.CalculateMinorInterval(majorInterval);
        }

        /// <summary>
        /// Creates tick values at the specified interval.
        /// </summary>
        /// <param name="from">The start value.</param>
        /// <param name="to">The end value.</param>
        /// <param name="step">The interval.</param>
        /// <param name="maxTicks">The maximum number of ticks (optional). The default value is 1000.</param>
        /// <returns>A sequence of values.</returns>
        /// <exception cref="System.ArgumentException">Step cannot be zero or negative.;step</exception>
        protected virtual IList<double> CreateTickValues(double from, double to, double step, int maxTicks = 1000)
        {
            return AxisUtilities.CreateTickValues(from, to, step, maxTicks);
        }

        /// <summary>
        /// Coerces the actual maximum and minimum values.
        /// </summary>
        protected virtual void CoerceActualMaxMin()
        {
            // Coerce actual minimum
            if (double.IsNaN(this.ActualMinimum) || double.IsInfinity(this.ActualMinimum))
            {
                this.ActualMinimum = 0;
            }

            // Coerce actual maximum
            if (double.IsNaN(this.ActualMaximum) || double.IsInfinity(this.ActualMaximum))
            {
                this.ActualMaximum = 100;
            }

            if (this.AbsoluteMaximum - this.AbsoluteMinimum < this.MinimumRange)
            {
                throw new InvalidOperationException("MinimumRange should be larger than AbsoluteMaximum-AbsoluteMinimum.");
            }

            // Coerce the minimum range
            if (this.ActualMaximum - this.ActualMinimum < this.MinimumRange)
            {
                if (this.ActualMinimum + this.MinimumRange < this.AbsoluteMaximum)
                {
                    var average = (this.ActualMaximum + this.ActualMinimum) * 0.5;
                    var delta = this.MinimumRange / 2;
                    this.ActualMinimum = average - delta;
                    this.ActualMaximum = average + delta;

                    if (this.ActualMinimum < this.AbsoluteMinimum)
                    {
                        var diff = this.AbsoluteMinimum - this.ActualMinimum;
                        this.ActualMinimum = this.AbsoluteMinimum;
                        this.ActualMaximum += diff;
                    }

                    if (this.ActualMaximum > this.AbsoluteMaximum)
                    {
                        var diff = this.AbsoluteMaximum - this.ActualMaximum;
                        this.ActualMaximum = this.AbsoluteMaximum;
                        this.ActualMinimum += diff;
                    }
                }
                else
                {
                    if (this.AbsoluteMaximum - this.MinimumRange > this.AbsoluteMinimum)
                    {
                        this.ActualMinimum = this.AbsoluteMaximum - this.MinimumRange;
                        this.ActualMaximum = this.AbsoluteMaximum;
                    }
                    else
                    {
                        this.ActualMaximum = this.AbsoluteMaximum;
                        this.ActualMinimum = this.AbsoluteMinimum;
                    }
                }
            }

            // Coerce the maximum range
            if (this.ActualMaximum - this.ActualMinimum > this.MaximumRange)
            {
                if (this.ActualMinimum + this.MaximumRange < this.AbsoluteMaximum)
                {
                    var average = (this.ActualMaximum + this.ActualMinimum) * 0.5;
                    var delta = this.MaximumRange / 2;
                    this.ActualMinimum = average - delta;
                    this.ActualMaximum = average + delta;

                    if (this.ActualMinimum < this.AbsoluteMinimum)
                    {
                        var diff = this.AbsoluteMinimum - this.ActualMinimum;
                        this.ActualMinimum = this.AbsoluteMinimum;
                        this.ActualMaximum += diff;
                    }

                    if (this.ActualMaximum > this.AbsoluteMaximum)
                    {
                        var diff = this.AbsoluteMaximum - this.ActualMaximum;
                        this.ActualMaximum = this.AbsoluteMaximum;
                        this.ActualMinimum += diff;
                    }
                }
                else
                {
                    if (this.AbsoluteMaximum - this.MaximumRange > this.AbsoluteMinimum)
                    {
                        this.ActualMinimum = this.AbsoluteMaximum - this.MaximumRange;
                        this.ActualMaximum = this.AbsoluteMaximum;
                    }
                    else
                    {
                        this.ActualMaximum = this.AbsoluteMaximum;
                        this.ActualMinimum = this.AbsoluteMinimum;
                    }
                }
            }

            // Coerce the absolute maximum/minimum
            if (this.AbsoluteMaximum <= this.AbsoluteMinimum)
            {
                throw new InvalidOperationException("AbsoluteMaximum should be larger than AbsoluteMinimum.");
            }

            if (this.ActualMaximum <= this.ActualMinimum)
            {
                this.ActualMaximum = this.ActualMinimum + 100;
            }

            if (this.ActualMinimum < this.AbsoluteMinimum)
            {
                this.ActualMinimum = this.AbsoluteMinimum;
            }

            if (this.ActualMinimum > this.AbsoluteMaximum)
            {
                this.ActualMinimum = this.AbsoluteMaximum;
            }

            if (this.ActualMaximum < this.AbsoluteMinimum)
            {
                this.ActualMaximum = this.AbsoluteMinimum;
            }

            if (this.ActualMaximum > this.AbsoluteMaximum)
            {
                this.ActualMaximum = this.AbsoluteMaximum;
            }
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value to format.</param>
        /// <returns>The formatted value.</returns>
        protected virtual string FormatValueOverride(double x)
        {
            // The "SuperExponentialFormat" renders the number with superscript exponents. E.g. 10^2
            if (this.UseSuperExponentialFormat && !x.Equals(0))
            {
                double exp = Exponent(x);
                double mantissa = Mantissa(x);
                string fmt;
                if (this.StringFormat == null)
                {
                    fmt = Math.Abs(mantissa - 1.0) < 1e-6 ? "10^{{{1:0}}}" : "{0}·10^{{{1:0}}}";
                }
                else
                {
                    fmt = "{0:" + this.StringFormat + "}·10^{{{1:0}}}";
                }

                return string.Format(this.ActualCulture, fmt, mantissa, exp);
            }

            string format = string.Concat("{0:", this.ActualStringFormat ?? this.StringFormat ?? string.Empty, "}");
            return string.Format(this.ActualCulture, format, x);
        }

        /// <summary>
        /// Calculates the actual maximum value of the axis, including the <see cref="MaximumPadding" />.
        /// </summary>
        /// <returns>The new actual maximum value of the axis.</returns>
        /// <remarks>
        /// Must be called before <see cref="CalculateActualMinimum" />
        /// </remarks>
        protected virtual double CalculateActualMaximum()
        {
            var actualMaximum = this.DataMaximum;
            double range = this.DataMaximum - this.DataMinimum;

            if (range < double.Epsilon)
            {
                double zeroRange = this.DataMaximum > 0 ? this.DataMaximum : 1;
                actualMaximum += zeroRange * 0.5;
            }

            if (!double.IsNaN(this.DataMinimum) && !double.IsNaN(actualMaximum))
            {
                double x1 = this.PreTransform(actualMaximum);
                double x0 = this.PreTransform(this.DataMinimum);
                double dx = this.MaximumPadding * (x1 - x0);
                return this.PostInverseTransform(x1 + dx);
            }

            return actualMaximum;
        }

        /// <summary>
        /// Calculates the actual minimum value of the axis, including the <see cref="MinimumPadding" />.
        /// </summary>
        /// <returns>The new actual minimum value of the axis.</returns>
        /// <remarks>
        /// Must be called after <see cref="CalculateActualMaximum" />
        /// </remarks>
        protected virtual double CalculateActualMinimum()
        {
            var actualMinimum = this.DataMinimum;
            double range = this.DataMaximum - this.DataMinimum;

            if (range < double.Epsilon)
            {
                double zeroRange = this.DataMaximum > 0 ? this.DataMaximum : 1;
                actualMinimum -= zeroRange * 0.5;
            }

            if (!double.IsNaN(this.ActualMaximum))
            {
                double x1 = this.PreTransform(this.ActualMaximum);
                double x0 = this.PreTransform(actualMinimum);
                double existingPadding = this.MaximumPadding;
                double dx = this.MinimumPadding * ((x1 - x0) / (1.0 + existingPadding));
                return this.PostInverseTransform(x0 - dx);
            }

            return actualMinimum;
        }

        /// <summary>
        /// Sets the transform.
        /// </summary>
        /// <param name="newScale">The new scale.</param>
        /// <param name="newOffset">The new offset.</param>
        protected void SetTransform(double newScale, double newOffset)
        {
            this.scale = newScale;
            this.offset = newOffset;
            this.OnTransformChanged(new EventArgs());
        }

        /// <summary>
        /// Calculates the actual interval.
        /// </summary>
        /// <param name="availableSize">Size of the available area.</param>
        /// <param name="maxIntervalSize">Maximum length of the intervals.</param>
        /// <returns>The calculate actual interval.</returns>
        protected virtual double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            return this.CalculateActualInterval(availableSize, maxIntervalSize, this.ActualMaximum - this.ActualMinimum);
        }

        /// <summary>
        /// Returns the actual interval to use to determine which values are displayed in the axis.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <param name="maxIntervalSize">The maximum interval size.</param>
        /// <param name="range">The range.</param>
        /// <returns>Actual interval to use to determine which values are displayed in the axis.</returns>
        protected double CalculateActualInterval(double availableSize, double maxIntervalSize, double range)
        {
            if (availableSize <= 0)
            {
                return maxIntervalSize;
            }

            if (Math.Abs(maxIntervalSize) < double.Epsilon)
            {
                throw new ArgumentException("Maximum interval size cannot be zero.", "maxIntervalSize");
            }

            if (Math.Abs(range) < double.Epsilon)
            {
                throw new ArgumentException("Range cannot be zero.", "range");
            }

            Func<double, double> exponent = x => Math.Ceiling(Math.Log(x, 10));
            Func<double, double> mantissa = x => x / Math.Pow(10, exponent(x) - 1);

            // reduce intervals for horizontal axis.
            // double maxIntervals = Orientation == AxisOrientation.x ? MaximumAxisIntervalsPer200Pixels * 0.8 : MaximumAxisIntervalsPer200Pixels;
            // real maximum interval count
            double maxIntervalCount = availableSize / maxIntervalSize;

            range = Math.Abs(range);
            double interval = Math.Pow(10, exponent(range));
            double intervalCandidate = interval;

            // Function to remove 'double precision noise'
            // TODO: can this be improved
            Func<double, double> removeNoise = x => double.Parse(x.ToString("e14"));

            // decrease interval until interval count becomes less than maxIntervalCount
            while (true)
            {
                var m = (int)mantissa(intervalCandidate);
                if (m == 5)
                {
                    // reduce 5 to 2
                    intervalCandidate = removeNoise(intervalCandidate / 2.5);
                }
                else if (m == 2 || m == 1 || m == 10)
                {
                    // reduce 2 to 1, 10 to 5, 1 to 0.5
                    intervalCandidate = removeNoise(intervalCandidate / 2.0);
                }
                else
                {
                    intervalCandidate = removeNoise(intervalCandidate / 2.0);
                }

                if (range / intervalCandidate > maxIntervalCount)
                {
                    break;
                }

                if (double.IsNaN(intervalCandidate) || double.IsInfinity(intervalCandidate))
                {
                    break;
                }

                interval = intervalCandidate;
            }

            return interval;
        }

        /// <summary>
        /// Raises the <see cref="AxisChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="OxyPlot.Axes.AxisChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnAxisChanged(AxisChangedEventArgs args)
        {
            this.UpdateActualMaxMin();

            var handler = this.AxisChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="TransformChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnTransformChanged(EventArgs args)
        {
            var handler = this.TransformChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}
