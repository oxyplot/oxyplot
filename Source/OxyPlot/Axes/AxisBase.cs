using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace OxyPlot
{
    /// <summary>
    /// The Axis is the base class for the OxyPlot axes.
    /// </summary>
    public abstract class AxisBase : IAxis
    {
        private ScreenPoint midPoint;
        public ScreenPoint MidPoint
        {
            get { return midPoint; }
            set { midPoint = value; }
        }

        private double offset;
        public double Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private double scale;
        public double Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        internal AxisPosition position;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisBase"/> class.
        /// </summary>
        protected AxisBase()
        {
            Position = AxisPosition.Left;
            IsVisible = true;

            Minimum = double.NaN;
            Maximum = double.NaN;
            MinorStep = double.NaN;
            MajorStep = double.NaN;

            MinimumPadding = 0.01;
            MaximumPadding = 0.01;

            TickStyle = TickStyle.Inside;
            TicklineColor = OxyColors.Black;
            MajorGridlineStyle = LineStyle.None;
            MinorGridlineStyle = LineStyle.None;
            MajorGridlineColor = OxyColor.FromArgb(0x40, 0, 0, 0);
            MinorGridlineColor = OxyColor.FromArgb(0x20, 0, 0, 0x00);
            MajorGridlineThickness = 1;
            MinorGridlineThickness = 1;

            ExtraGridlineStyle = LineStyle.Solid;
            ExtraGridlineColor = OxyColors.Black;
            ExtraGridlineThickness = 1;

            ShowMinorTicks = true;

            FontFamily = PlotModel.DefaultFont;
            FontSize = 12;
            FontWeight = 500;

            MinorTickSize = 4;
            MajorTickSize = 7;

            StartPosition = 0;
            EndPosition = 1;
            
            TitlePosition = 0.5;

            Angle = 0;

            IsZoomEnabled = true;
            IsPanEnabled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisBase"/> class.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="title">The title shown next to the axis.</param>
        protected AxisBase(AxisPosition pos, double minimum, double maximum, string title = null)
            : this()
        {
            Position = pos;
            Minimum = minimum;
            Maximum = maximum;
            Title = title;
        }

        /// <summary>
        /// Gets the screen coordinate of the Maximum point on the axis.
        /// </summary>
        /// <value>The screen max.</value>
        public ScreenPoint ScreenMax { get; set; }

        /// <summary>
        /// Gets the screen coordinate of the Minimum point on the axis.
        /// </summary>
        /// <value>The screen min.</value>
        public ScreenPoint ScreenMin { get; set; }

        /// <summary>
        /// Gets or sets the key of the axis.
        /// This can be used to find an axis if you have 
        /// defined mutiple axes in a plot.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the position of the axis.
        /// </summary>
        /// <value>The position.</value>
        public AxisPosition Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the axis should
        /// be positioned on the zero-crossing of the related axis.
        /// </summary>
        public bool PositionAtZeroCrossing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this axis is visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this axis is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the actual minimum value of the axis.
        /// </summary>
        /// <value>The actual minimum.</value>
        public double ActualMinimum { get; set; }

        /// <summary>
        /// Gets or sets the actual maximum value of the axis.
        /// If Maximum is not NaN, this value will be overridden.
        /// </summary>
        /// <value>The actual maximum.</value>
        public double ActualMaximum { get; set; }

        /// <summary>
        /// Gets or sets the actual minor step.
        /// </summary>
        /// <value>The actual minor step.</value>
        internal double ActualMinorStep { get; set; }

        /// <summary>
        /// Gets or sets the actual major step.
        /// </summary>
        /// <value>The actual major step.</value>
        internal double ActualMajorStep { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minor step 
        /// (the interval between small ticks without number).
        /// </summary>
        /// <value>The minor step.</value>
        public double MinorStep { get; set; }

        /// <summary>
        /// Gets or sets the major step.
        /// (the interval between large ticks with numbers).
        /// </summary>
        /// <value>The major step.</value>
        public double MajorStep { get; set; }

        /// <summary>
        /// Gets or sets the 'padding' fraction of the minimum value.
        /// A value of 0.01 gives 1% more space on the minimum end of the axis.
        /// This property is not used if the Minimum property is set.
        /// </summary>
        /// <value>The minimum padding.</value>
        public double MinimumPadding { get; set; }

        /// <summary>
        /// Gets or sets the 'padding' fraction of the maximum value.
        /// A value of 0.01 gives 1% more space on the maximum end of the axis.
        /// This property is not used if the Maximum property is set.
        /// </summary>
        /// <value>The maximum padding.</value>
        public double MaximumPadding { get; set; }

        /// <summary>
        /// Gets or sets the tick style.
        /// </summary>
        /// <value>The tick style.</value>
        public TickStyle TickStyle { get; set; }

        /// <summary>
        /// Gets or sets the size of the minor tick.
        /// </summary>
        /// <value>The size of the minor tick.</value>
        public double MinorTickSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the major tick.
        /// </summary>
        /// <value>The size of the major tick.</value>
        public double MajorTickSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the ticks.
        /// </summary>
        /// <value>The color of the tickline.</value>
        public OxyColor TicklineColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether minor ticks should be shown.
        /// </summary>
        public bool ShowMinorTicks { get; set; }

        /// <summary>
        /// Gets or sets the major gridline style.
        /// </summary>
        /// <value>The major gridline style.</value>
        public LineStyle MajorGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the minor gridline style.
        /// </summary>
        /// <value>The minor gridline style.</value>
        public LineStyle MinorGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the color of the major gridline.
        /// </summary>
        /// <value>The color of the major gridline.</value>
        public OxyColor MajorGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the minor gridline.
        /// </summary>
        /// <value>The color of the minor gridline.</value>
        public OxyColor MinorGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the major gridline thickness.
        /// </summary>
        /// <value>The major gridline thickness.</value>
        public double MajorGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the minor gridline thickness.
        /// </summary>
        /// <value>The minor gridline thickness.</value>
        public double MinorGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the values for extra gridlines.
        /// </summary>
        /// <value>The extra gridlines.</value>
        public double[] ExtraGridlines { get; set; }

        /// <summary>
        /// Gets or sets the extra gridlines linestyle.
        /// </summary>
        /// <value>The extra gridline style.</value>
        public LineStyle ExtraGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the color of the extra gridlines.
        /// </summary>
        /// <value>The color of the extra gridline.</value>
        public OxyColor ExtraGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the extra gridline thickness.
        /// </summary>
        /// <value>The extra gridline thickness.</value>
        public double ExtraGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the angle for the axis values.
        /// </summary>
        /// <value>The angle.</value>
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the string format used
        /// for formatting the axis values.
        /// </summary>
        /// <value>The string format.</value>
        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use superscript exponential format.
        /// This format will convert 1.5E+03 to 1.5·10^{3} and render the superscript properly
        /// If StringFormat is null, 1.0E+03 will be converted to 10^{3}
        /// </summary>
        public bool UseSuperExponentialFormat { get; set; }

        /// <summary>
        /// Gets or sets the actual string format
        /// being used.
        /// </summary>
        /// <value>The actual string format.</value>
        internal string ActualStringFormat { get; set; }

        /// <summary>
        /// Gets or sets the title of the axis.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the unit of the axis.
        /// </summary>
        /// <value>The unit.</value>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>The font weight.</value>
        public double FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the start position of the axis on the plot area.
        /// This is a fraction from 0(bottom/left) to 1(top/right).
        /// </summary>
        /// <value>The start position.</value>
        public double StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the end position of the axis on the plot area.
        /// This is a fraction from 0(bottom/left) to 1(top/right).
        /// </summary>
        /// <value>The end position.</value>
        public double EndPosition { get; set; }

        /// <summary>
        /// Gets or sets the related axis.
        /// This is used for polar coordinate systems where
        /// the angle and magnitude axes are related.
        /// </summary>
        /// <value>The related axis.</value>
        public AxisBase RelatedAxis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pan is enabled.
        /// </summary>
        public bool IsPanEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether zoom is enabled.
        /// </summary>
        public bool IsZoomEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this axis is reversed.
        /// It is reversed if StartPosition>EndPosition.
        /// </summary>
        public bool IsReversed
        {
            get { return StartPosition > EndPosition; }
        }

        /// <summary>
        /// Position of the title (0.5 is in the middle).
        /// </summary>
        public double TitlePosition {get;set;}

        #region IAxis Members

        public virtual void Render(IRenderContext rc, PlotModel model)
        {
            switch (Position)
            {
                case AxisPosition.Left:
                case AxisPosition.Right:
                case AxisPosition.Top:
                case AxisPosition.Bottom:
                    {
                        var r = new HorizontalAndVerticalAxisRenderer(rc, model);
                        r.Render(this);
                        break;
                    }
                case AxisPosition.Angle:
                    {
                        var r = new AngleAxisRendererBase(rc, model);
                        r.Render(this);
                        break;
                    }
                case AxisPosition.Magnitude:
                    {
                        var r = new MagnitudeAxisRendererBase(rc, model);
                        r.Render(this);
                        break;
                    }
            }
        }

        #endregion

        public bool IsHorizontal()
        {
            return position == AxisPosition.Top || position == AxisPosition.Bottom;
        }

        public bool IsVertical()
        {
            return position == AxisPosition.Left || position == AxisPosition.Right;
        }

        public bool IsPolar()
        {
            return position == AxisPosition.Magnitude || position == AxisPosition.Angle;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}({1}, {2}, {3}, {4})", GetType().Name, Position,
                                 ActualMinimum, ActualMaximum, ActualMajorStep);
        }

        public virtual void GetTickValues(out ICollection<double> majorValues, out ICollection<double> minorValues)
        {
            minorValues = CreateTickValues(ActualMinimum, ActualMaximum, ActualMinorStep);
            majorValues = CreateTickValues(ActualMinimum, ActualMaximum, ActualMajorStep);
        }

        private static readonly Func<double, double> Exponent = x => Math.Round(Math.Log(Math.Abs(x), 10));
        private static readonly Func<double, double> Mantissa = x => x / Math.Pow(10, Exponent(x));

        public virtual string FormatValue(double x)
        {
            if (UseSuperExponentialFormat)
            {
                // if (x == 1 || x == 10 || x == -1 || x == -10)
                //    return x.ToString();

                double exp = Exponent(x);
                double mantissa = Mantissa(x);
                String fmt;
                if (StringFormat == null)
                {
                    fmt = Math.Abs(mantissa - 1.0) < 1e-6 ? "10^{{{1:0}}}" : "{0}·10^{{{1:0}}}";
                }
                else
                {
                    fmt = "{0:" + StringFormat + "}·10^{{{1:0}}}";
                }
                return String.Format(CultureInfo.InvariantCulture, fmt, mantissa, exp);
            }
            return x.ToString(ActualStringFormat, CultureInfo.InvariantCulture);
        }

        internal static ICollection<double> CreateTickValues(double min, double max, double step)
        {
            if (max <= min)
                throw new InvalidOperationException("Axis: Maximum should be larger than minimum.");
            if (step <= 0)
                throw new InvalidOperationException("Axis: Step cannot be negative.");

            double x0 = Math.Round(min / step) * step;

            var values = new Collection<double>();

            // Limit the maximum number of iterations (in case of something wrong with the step size)
            int i = 0;
            const int maxit = 1000;
            double x = x0;

            while (x <= max + double.Epsilon && i < maxit)
            {
                x = x0 + i * step;
                i++;
                if (x >= min - double.Epsilon && x <= max + double.Epsilon)
                {
                    x = RemoveNoiseFromDoubleMath(x);
                    values.Add(x);
                }
            }
            return values;
        }

        public virtual void Pan(double dx)
        {
            if (!IsPanEnabled)
                return;
            Minimum = ActualMinimum + dx;
            Maximum = ActualMaximum + dx;
        }

        public virtual void ZoomAt(double factor, double x)
        {
            if (!IsZoomEnabled)
                return;
            double dx0 = (ActualMinimum - x) * scale;
            double dx1 = (ActualMaximum - x) * scale;
            scale *= factor;
            Minimum = dx0 / scale + x;
            Maximum = dx1 / scale + x;
        }

        public virtual void Zoom(double x0, double x1)
        {
            if (!IsZoomEnabled)
                return;
            Minimum = Math.Min(x0, x1);
            Maximum = Math.Max(x0, x1);
        }

        public virtual void Reset()
        {
            Minimum = double.NaN;
            Maximum = double.NaN;
        }

        /// <summary>
        /// Updates the minor/major step intervals if they are undefined.
        /// </summary>
        public virtual void UpdateIntervals(double dx, double dy)
        {
            double labelSize = GetLabelSize();
            double length = IsHorizontal() ? dx : dy;
            length *= Math.Abs(EndPosition - StartPosition);

            ActualMajorStep = !double.IsNaN(MajorStep) ? MajorStep : CalculateActualInterval(length, labelSize);

            if (!double.IsNaN(MinorStep))
                ActualMinorStep = MinorStep;
            else
                ActualMinorStep = ActualMajorStep / 5;


            if (double.IsNaN(ActualMinorStep))
                ActualMinorStep = 2;
            if (double.IsNaN(ActualMajorStep))
                ActualMajorStep = 10;

            ActualStringFormat = StringFormat;

            //if (ActualStringFormat==null)
            //{
            //    if (ActualMaximum > 1e6 || ActualMinimum < 1e-6)
            //        ActualStringFormat = "#.#e-0";
            //}
        }

        protected virtual double GetLabelSize()
        {
            // todo: this could be dependent on the stringformat 
            // and min/max numbers
            // could format the string for min and max...

            switch (position)
            {
                case AxisPosition.Top:
                case AxisPosition.Bottom:
                    return 60;
                case AxisPosition.Left:
                case AxisPosition.Right:
                    return 60;
                case AxisPosition.Angle:
                    return 50;
                case AxisPosition.Magnitude:
                    return 100;
                default:
                    return 50;
            }
        }

        protected virtual double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            return CalculateActualInterval2(availableSize, maxIntervalSize, ActualMaximum - ActualMinimum);
        }

        // alternative algorithm not in use
        private double CalculateActualInterval1(double availableSize, double maxIntervalSize)
        {
            const int minimumTags = 5;
            const int maximumTags = 20;
            var numberOfTags = (int)(availableSize / maxIntervalSize);
            double range = ActualMaximum - ActualMinimum;
            double interval = range / numberOfTags;
            const int k1 = 10;
            interval = Math.Log10(interval / k1);
            interval = Math.Ceiling(interval);
            interval = Math.Pow(10, interval) * k1;

            if (range / interval > maximumTags) interval *= 5;
            if (range / interval < minimumTags) interval *= 0.5;

            if (interval <= 0) interval = 1;
            return interval;
        }

        // ===
        // the following algorithm is from 
        // System.Windows.Controls.DataVisualization.Charting.LinearAxis.cs

        // (c) Copyright Microsoft Corporation.
        // This source is subject to the Microsoft Public License (Ms-PL).
        // Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
        // All other rights reserved.

        /// <summary>
        /// Returns the actual interval to use to determine which values are 
        /// displayed in the axis.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <param name="maxIntervalSize">The maximum interval size.</param>
        /// <returns>Actual interval to use to determine which values are 
        /// displayed in the axis.
        /// </returns>
        protected double CalculateActualInterval2(double availableSize, double maxIntervalSize, double range)
        {
            if (availableSize <= 0)
                return maxIntervalSize;

            Func<double, double> exponent = x => Math.Ceiling(Math.Log(x, 10));
            Func<double, double> mantissa = x => x / Math.Pow(10, exponent(x) - 1);

            // reduce intervals for horizontal axis.
            // double maxIntervals = Orientation == AxisOrientation.x ? MaximumAxisIntervalsPer200Pixels * 0.8 : MaximumAxisIntervalsPer200Pixels;
            // real maximum interval count
            double maxIntervalCount = availableSize / maxIntervalSize;

            range = Math.Abs(range);
            //double range = Math.Abs(actualMinimum - actualMaximum);
            double interval = Math.Pow(10, exponent(range));
            double tempInterval = interval;

            // decrease interval until interval count becomes less than maxIntervalCount
            while (true)
            {
                var m = (int)mantissa(tempInterval);
                if (m == 5)
                {
                    // reduce 5 to 2
                    tempInterval = RemoveNoiseFromDoubleMath(tempInterval / 2.5);
                }
                else if (m == 2 || m == 1 || m == 10)
                {
                    // reduce 2 to 1,10 to 5,1 to 0.5
                    tempInterval = RemoveNoiseFromDoubleMath(tempInterval / 2.0);
                }
                else
                {
                    tempInterval = RemoveNoiseFromDoubleMath(tempInterval / 2.0);
                }

                if (range / tempInterval > maxIntervalCount)
                {
                    break;
                }

                interval = tempInterval;
            }
            return interval;
        }

        /// <summary>
        /// Removes the noise from double math.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A double without a noise.</returns>
        internal static double RemoveNoiseFromDoubleMath(double value)
        {
            if (value == 0.0 || Math.Abs((Math.Log10(Math.Abs(value)))) < 27)
            {
                return (double)((decimal)value);
            }
            return Double.Parse(value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        }

        // ===

        public void Include(double p)
        {
            if (double.IsNaN(p) || double.IsInfinity(p))
                return;

            ActualMinimum = double.IsNaN(ActualMinimum) ? p : Math.Min(ActualMinimum, p);
            ActualMaximum = double.IsNaN(ActualMaximum) ? p : Math.Max(ActualMaximum, p);
        }

        /// <summary>
        /// Updates the actual max and min with the 'padding' values.
        /// </summary>
        public virtual void UpdateActualMaxMin()
        {
            double range = ActualMaximum - ActualMinimum;
            double zeroRange = ActualMaximum > 0 ? ActualMaximum : 1;

            if (!double.IsNaN(Maximum))
            {
                ActualMaximum = Maximum;
            }
            else
            {
                if (range < double.Epsilon)
                    ActualMaximum += zeroRange * 0.5;

                double x1 = PreTransform(ActualMaximum);
                double x0 = PreTransform(ActualMinimum);
                double dx = MaximumPadding * (x1 - x0);
                ActualMaximum = PostInverseTransform(x1 + dx);
            }

            if (!double.IsNaN(Minimum))
            {
                ActualMinimum = Minimum;
            }
            else
            {
                if (range < double.Epsilon)
                    ActualMinimum -= zeroRange * 0.5;

                double x1 = PreTransform(ActualMaximum);
                double x0 = PreTransform(ActualMinimum);
                double dx = MinimumPadding * (x1 - x0);
                ActualMinimum = PostInverseTransform(x0 - dx);
            }

            if (double.IsNaN(ActualMaximum))
            {
                ActualMaximum = 100;
            }
            if (double.IsNaN(ActualMinimum))
            {
                ActualMinimum = this is LogarithmicAxis ? 1 : 0;
            }
        }

        #region Transformations

        protected virtual double PreTransform(double x)
        {
            return x;
        }

        protected virtual double PostInverseTransform(double x)
        {
            return x;
        }

        /// <summary>
        /// Transforms the specified x and y coordinates to screen coordinates.
        /// The this object is always the x-axis, and the y-axis is given as
        /// an argument. This is neccessary to calculate screen coordinates from
        /// polar coordinates.
        /// </summary>
        public virtual ScreenPoint Transform(DataPoint dp, IAxis yAxis)
        {
            // todo: review architecture here, could this be solved in a better way?

            if (IsPolar())
            {
                double r = (dp.x - Offset) * scale;
                double th = yAxis != null ? (dp.y - yAxis.Offset) * yAxis.Scale : double.NaN;
                return new ScreenPoint(MidPoint.x + r * Math.Cos(th), MidPoint.y + r * Math.Sin(th));
            }

            if (yAxis == null)
                return new ScreenPoint();

            return new ScreenPoint(Transform(dp.x), yAxis.Transform(dp.y));
        }

        // todo: should find a better way to do this
        // this method seems to be a bottleneck for performance...
        public double Transform(double x)
        {
            return (PreTransform(x) - Offset) * scale;
        }

        /// <summary>
        /// Transforms a point from screen coordinates to a data point.
        /// The this. object must be an x-axis.
        /// </summary>
        /// <param name="x">The screen x.</param>
        /// <param name="y">The screen y.</param>
        /// <param name="yAxis">The y axis.</param>
        /// <returns></returns>
        public virtual DataPoint InverseTransform(double x, double y, IAxis yAxis)
        {
            if (IsPolar())
            {
                x -= MidPoint.x;
                y -= MidPoint.y;
                double th = Math.Atan2(y, x);
                double r = Math.Sqrt(x * x + y * y);
                x = r / scale + Offset;
                y = yAxis != null ? th / yAxis.Scale + yAxis.Offset : double.NaN;
                return new DataPoint(x, y);
            }

            return new DataPoint(InverseTransform(x), yAxis.InverseTransform(y));
        }

        public static ScreenPoint Transform(DataPoint p, IAxis xAxis, IAxis yAxis)
        {
            return xAxis.Transform(p, yAxis);
        }

        public static DataPoint InverseTransform(ScreenPoint p, IAxis xAxis, IAxis yAxis)
        {
            return InverseTransform(p.x, p.y, xAxis, yAxis);
        }

        public static DataPoint InverseTransform(double x, double y, IAxis xAxis, IAxis yAxis)
        {
            if (xAxis != null && xAxis.IsPolar())
            {
                x -= xAxis.MidPoint.x;
                y -= xAxis.MidPoint.y;
                double th = Math.Atan2(y, x);
                double r = Math.Sqrt(x * x + y * y);
                x = r / xAxis.Scale + xAxis.Offset;
                y = yAxis != null ? th / yAxis.Scale + yAxis.Offset : double.NaN;
                return new DataPoint(x, y);
            }

            return new DataPoint(xAxis != null ? xAxis.InverseTransform(x) : 0,
                                 yAxis != null ? yAxis.InverseTransform(y) : 0);
        }

        public double InverseTransform(double x)
        {
            return PostInverseTransform(x / scale + Offset);
        }

        /// <summary>
        /// Updates the scale and offset properties of the transform
        /// from the specified boundary rectangle.
        /// </summary>
        public void UpdateTransform(OxyRect bounds)
        {
            double x0 = bounds.Left;
            double x1 = bounds.Right;
            double y0 = bounds.Bottom;
            double y1 = bounds.Top;

            ScreenMin = new ScreenPoint(x0, y1);
            ScreenMax = new ScreenPoint(x1, y0);

            MidPoint = new ScreenPoint((x0 + x1) / 2, (y0 + y1) / 2);

            if (Position == AxisPosition.Angle)
            {
                scale = 2 * Math.PI / (ActualMaximum - ActualMinimum);
                Offset = ActualMinimum;
                return;
            }
            if (Position == AxisPosition.Magnitude)
            {
                ActualMinimum = 0;
                double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
                scale = 0.5 * r / (ActualMaximum - ActualMinimum);
                Offset = ActualMinimum;
                return;
            }

            double a0 = IsHorizontal() ? x0 : y0;
            double a1 = IsHorizontal() ? x1 : y1;

            double dx = a1 - a0;
            a1 = a0 + EndPosition * dx;
            a0 = a0 + StartPosition * dx;
            ScreenMin = new ScreenPoint(a0, a1);
            ScreenMax = new ScreenPoint(a1, a0);

            if (ActualMaximum - ActualMinimum < double.Epsilon)
                ActualMaximum = ActualMinimum + 1;

            double max = PreTransform(ActualMaximum);
            double min = PreTransform(ActualMinimum);

            double da = a0 - a1;
            if (Math.Abs(da) != 0)
                Offset = a0 / da * max - a1 / da * min;
            else
                Offset = 0;

            double range = max - min;
            if (Math.Abs(range) != 0)
                scale = (a1 - a0) / range;
            else
                scale = 1;
        }

        public void SetScale(double scale)
        {
            double sx1 = (ActualMaximum - Offset) * this.scale;
            double sx0 = (ActualMinimum - Offset) * this.scale;

            double sgn = Math.Sign(this.scale);
            double mid = (ActualMaximum + ActualMinimum) / 2;

            double dx = (Offset - mid) * this.scale;
            this.scale = sgn * scale;
            Offset = dx / this.scale + mid;
            ActualMaximum = sx1 / this.scale + Offset;
            ActualMinimum = sx0 / this.scale + Offset;
        }

        #endregion
    }
}