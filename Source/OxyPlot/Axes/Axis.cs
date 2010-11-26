using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace OxyPlot
{
    /// <summary>
    /// The Axis is the base class for the OxyPlot axes.
    /// </summary>
    public abstract class Axis : IAxis
    {
        internal ScreenPoint MidPoint;
        internal double Offset;
        internal double Scale;
        internal AxisPosition position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        public Axis()
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

            MinorTickSize = 4;
            MajorTickSize = 7;

            StartPosition = 0;
            EndPosition = 1;

            Angle = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> class.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="title">The title shown next to the axis.</param>
        public Axis(AxisPosition pos, double minimum, double maximum, string title = null)
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
        public ScreenPoint ScreenMax { get; internal set; }

        /// <summary>
        /// Gets the screen coordinate of the Minimum point on the axis.
        /// </summary>
        /// <value>The screen min.</value>
        public ScreenPoint ScreenMin { get; internal set; }

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
        /// </summary>
        /// <value>The minimum padding.</value>
        public double MinimumPadding { get; set; }

        /// <summary>
        /// Gets or sets the 'padding' fraction of the maximum value.
        /// A value of 0.01 gives 1% more space on the maximum end of the axis.
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
        /// Gets or sets a value indicating whether to use the 'Super' exponential format.
        /// This format will convert 1.5E+03 to 1.5 10^{3} and render the superscript properly
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
        public Axis RelatedAxis { get; set; }

        /// <summary>
        /// Gets a value indicating whether this axis is reversed.
        /// It is reversed if StartPosition>EndPosition.
        /// </summary>
        public bool IsReversed
        {
            get { return StartPosition > EndPosition; }
        }

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
                        var r = new VerticalAxisRendererBase(rc, model);
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

        static Func<double, double> Exponent = x => Math.Round(Math.Log(Math.Abs(x), 10));
        static Func<double, double> Mantissa = x => x / Math.Pow(10, Exponent(x));

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
                    if (mantissa == 1.0)
                    {
                        fmt = "10^{{{1:0}}}";
                    }
                    else
                    {
                        fmt = "{0} · 10^{{{1:0}}}";
                    }
                }
                else
                {
                    fmt = "{0:" + StringFormat + "} · 10^{{{1:0}}}";
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

            double x = (int)Math.Round(min / step) * step;

            var values = new Collection<double>();
            // Maximum number of iterations (in case of very small step size)
            int it = 0;
            const int maxit = 1000;
            double epsilon = Math.Abs(max - min) * 1e-6;
            while (x <= max + epsilon && it++ < maxit)
            {
                if (x >= min - epsilon && x <= max + epsilon)
                {
                    x = RemoveNoiseFromDoubleMath(x);
                    values.Add(x);
                }
                x += step;
            }
            return values;
        }

        public virtual void Pan(double dx)
        {
            Minimum = ActualMinimum + dx;
            Maximum = ActualMaximum + dx;
        }

        public virtual void ScaleAt(double factor, double x)
        {
            double dx0 = (ActualMinimum - x) * Scale;
            double dx1 = (ActualMaximum - x) * Scale;
            Scale *= factor;
            Minimum = dx0 / Scale + x;
            Maximum = dx1 / Scale + x;
        }

        public virtual void Zoom(double x0, double x1)
        {
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
        public void UpdateIntervals(double dx, double dy)
        {
            double labelSize = GetLabelSize();
            double length = IsHorizontal() ? dx : dy;
            length *= Math.Abs(EndPosition - StartPosition);

            if (!double.IsNaN(MajorStep))
                ActualMajorStep = MajorStep;
            else
                ActualMajorStep = CalculateActualInterval(length, labelSize);

            if (!double.IsNaN(MinorStep))
                ActualMinorStep = MinorStep;
            else
                ActualMinorStep = ActualMajorStep / 5;


            if (double.IsNaN(ActualMinorStep))
                ActualMinorStep = 2;
            if (double.IsNaN(ActualMajorStep))
                ActualMajorStep = 10;

            ActualStringFormat = StringFormat;
        }

        private double GetLabelSize()
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
            return CalculateActualInterval2(availableSize, maxIntervalSize);
        }

        // alternative algorithm not in use
        private double CalculateActualInterval1(double availableSize, double maxIntervalSize)
        {
            int minTags = 5;
            int maxTags = 20;
            var numberOfTags = (int)(availableSize / maxIntervalSize);
            double range = ActualMaximum - ActualMinimum;
            double interval = range / numberOfTags;
            const int k1 = 10;
            interval = Math.Log10(interval / k1);
            interval = Math.Ceiling(interval);
            interval = Math.Pow(10, interval) * k1;

            if (range / interval > maxTags) interval *= 5;
            if (range / interval < minTags) interval *= 0.5;

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
        /// <returns>Actual interval to use to determine which values are 
        /// displayed in the axis.
        /// </returns>
        private double CalculateActualInterval2(double availableSize, double maxIntervalSize)
        {
            Func<double, double> Exponent = x => Math.Ceiling(Math.Log(x, 10));
            Func<double, double> Mantissa = x => x / Math.Pow(10, Exponent(x) - 1);

            // reduce intervals for horizontal axis.
            // double maxIntervals = Orientation == AxisOrientation.x ? MaximumAxisIntervalsPer200Pixels * 0.8 : MaximumAxisIntervalsPer200Pixels;
            // real maximum interval count
            double maxIntervalCount = availableSize / maxIntervalSize;

            double range = Math.Abs(ActualMinimum - ActualMaximum);
            double interval = Math.Pow(10, Exponent(range));
            double tempInterval = interval;

            // decrease interval until interval count becomes less than maxIntervalCount
            while (true)
            {
                var mantissa = (int)Mantissa(tempInterval);
                if (mantissa == 5)
                {
                    // reduce 5 to 2
                    tempInterval = RemoveNoiseFromDoubleMath(tempInterval / 2.5);
                }
                else if (mantissa == 2 || mantissa == 1 || mantissa == 10)
                {
                    // reduce 2 to 1,10 to 5,1 to 0.5
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

            if (double.IsNaN(ActualMinimum))
                ActualMinimum = p;
            else
                ActualMinimum = Math.Min(ActualMinimum, p);

            if (double.IsNaN(ActualMaximum))
                ActualMaximum = p;
            else
                ActualMaximum = Math.Max(ActualMaximum, p);
        }

        /// <summary>
        /// Updates the actual max and min with the 'padding' values.
        /// </summary>
        public void UpdateActualMaxMin()
        {
            if (!double.IsNaN(Maximum))
            {
                ActualMaximum = Maximum;
            }
            else
            {
                ActualMaximum += MaximumPadding * (ActualMaximum - ActualMinimum);
            }

            if (!double.IsNaN(Minimum))
            {
                ActualMinimum = Minimum;
            }
            else
            {
                ActualMinimum -= MinimumPadding * (ActualMaximum - ActualMinimum);
            }

            if (double.IsNaN(ActualMaximum))
            {
                ActualMaximum = 100;
            }
            if (double.IsNaN(ActualMinimum))
            {
                ActualMinimum = 0;
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
        /// The this object is always the xaxis, and the yaxis is given as
        /// an argument. This is neccessary to calculate screen coordinates from
        /// polar coordinates.
        /// </summary>
        public virtual ScreenPoint Transform(double x, double y, Axis yAxis)
        {
            // todo: review architecture here

            if (IsPolar())
            {
                double r = (x - Offset) * Scale;
                double th = yAxis != null ? (y - yAxis.Offset) * yAxis.Scale : double.NaN;
                return new ScreenPoint(MidPoint.x + r * Math.Cos(th), MidPoint.y + r * Math.Sin(th));
            }

            if (yAxis == null)
                return new ScreenPoint();

            return new ScreenPoint(TransformX(x), yAxis.TransformX(y));
        }

        // todo: should find a better way to do this
        // this method seems to be a bottleneck for performance...
        public double TransformX(double x)
        {
            return (PreTransform(x) - Offset) * Scale;
        }

        /// <summary>
        /// Transforms a point from screen coordinates to a data point.
        /// The this. object must be an x-axis.
        /// </summary>
        /// <param name="x">The screen x.</param>
        /// <param name="y">The screen y.</param>
        /// <param name="yAxis">The y axis.</param>
        /// <returns></returns>
        public virtual DataPoint InverseTransform(double x, double y, Axis yAxis)
        {
            if (IsPolar())
            {
                x -= MidPoint.x;
                y -= MidPoint.y;
                double th = Math.Atan2(y, x);
                double r = Math.Sqrt(x * x + y * y);
                x = r / Scale + Offset;
                y = yAxis != null ? th / yAxis.Scale + yAxis.Offset : double.NaN;
                return new DataPoint(x, y);
            }

            return new DataPoint(InverseTransformX(x), yAxis.InverseTransformX(y));
        }

        public double InverseTransformX(double x)
        {
            return PostInverseTransform(x / Scale + Offset);
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
                Scale = 2 * Math.PI / (ActualMaximum - ActualMinimum);
                Offset = ActualMinimum;
                return;
            }
            if (Position == AxisPosition.Magnitude)
            {
                ActualMinimum = 0;
                double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
                Scale = 0.5 * r / (ActualMaximum - ActualMinimum);
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

            const double eps = 1e-6;
            if (max - min < eps) max = min + 1;

            if (Math.Abs(a0 - a1) != 0)
                Offset = (a0 * max - min * a1) / (a0 - a1);
            else
                Offset = 0;

            Scale = (a1 - a0) / (max - min);
        }

        public void SetScale(double scale)
        {
            double sx1 = (ActualMaximum - Offset) * Scale;
            double sx0 = (ActualMinimum - Offset) * Scale;

            double sgn = Math.Sign(Scale);
            double mid = (ActualMaximum + ActualMinimum) / 2;

            double dx = (Offset - mid) * Scale;
            Scale = sgn * scale;
            Offset = dx / Scale + mid;
            ActualMaximum = sx1 / Scale + Offset;
            ActualMinimum = sx0 / Scale + Offset;
        }

        #endregion
    }
}