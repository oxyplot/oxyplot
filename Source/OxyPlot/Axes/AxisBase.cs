namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    /// <summary>
    /// The AxisBase is the base class for the OxyPlot axes.
    /// </summary>
    public abstract class AxisBase : IAxis
    {
        #region Constants and Fields

        /// <summary>
        /// Exponent function.
        /// </summary>
        private static readonly Func<double, double> Exponent = x => Math.Round(Math.Log(Math.Abs(x), 10));

        /// <summary>
        /// Mantissa function.
        /// http://en.wikipedia.org/wiki/Mantissa
        /// </summary>
        private static readonly Func<double, double> Mantissa = x => x / Math.Pow(10, Exponent(x));

        private AxisPosition position;

        private double scale;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisBase"/> class.
        /// </summary>
        protected AxisBase()
        {
            this.Position = AxisPosition.Left;
            this.IsAxisVisible = true;

            this.AbsoluteMaximum = double.MaxValue;
            this.AbsoluteMinimum = double.MinValue;

            this.ViewMaximum = double.NaN;
            this.ViewMinimum = double.NaN;

            this.Minimum = double.NaN;
            this.Maximum = double.NaN;
            this.MinorStep = double.NaN;
            this.MajorStep = double.NaN;

            this.MinimumPadding = 0.01;
            this.MaximumPadding = 0.01;
            this.MinimumRange = 0;

            this.TickStyle = TickStyle.Inside;
            this.TicklineColor = OxyColors.Black;
            this.MajorGridlineStyle = LineStyle.None;
            this.MinorGridlineStyle = LineStyle.None;
            this.MajorGridlineColor = OxyColor.FromArgb(0x40, 0, 0, 0);
            this.MinorGridlineColor = OxyColor.FromArgb(0x20, 0, 0, 0x00);
            this.MajorGridlineThickness = 1;
            this.MinorGridlineThickness = 1;

            this.ExtraGridlineStyle = LineStyle.Solid;
            this.ExtraGridlineColor = OxyColors.Black;
            this.ExtraGridlineThickness = 1;

            this.ShowMinorTicks = true;

            this.Font = null;
            this.FontSize = 12;
            this.FontWeight = FontWeights.Normal;

            this.MinorTickSize = 4;
            this.MajorTickSize = 7;

            this.StartPosition = 0;
            this.EndPosition = 1;

            this.TitlePosition = 0.5;
            this.TitleFormatString = "{0} [{1}]";

            this.Angle = 0;

            this.IsZoomEnabled = true;
            this.IsPanEnabled = true;

            this.FilterMinValue = double.MinValue;
            this.FilterMaxValue = double.MaxValue;
            this.FilterFunction = null;
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
            this.Position = pos;
            this.Minimum = minimum;
            this.Maximum = maximum;

            this.AbsoluteMaximum = double.NaN;
            this.AbsoluteMinimum = double.NaN;

            this.Title = title;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// This event occurs after the axis has been changed (by zooming, panning or resetting).
        /// </summary>
        public event EventHandler<AxisChangedEventArgs> AxisChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the absolute maximum. This is only used for the UI control.
        /// It will not be possible to zoom/pan beyond this limit.
        /// </summary>
        /// <value>The absolute maximum.</value>
        public double AbsoluteMaximum { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum. This is only used for the UI control.
        /// It will not be possible to zoom/pan beyond this limit.
        /// </summary>
        /// <value>The absolute minimum.</value>
        public double AbsoluteMinimum { get; set; }

        /// <summary>
        /// Gets or sets the actual major step.
        /// </summary>
        public double ActualMajorStep { get; protected set; }

        /// <summary>
        /// Gets or sets the actual maximum value of the axis.
        /// If Maximum is not NaN, this value will be defined by Maximum.
        /// If ViewMaximum is not NaN, this value will be defined by ViewMaximum.
        /// Otherwise this value will be defined by the maximum (+padding) of the data.
        /// </summary>
        public double ActualMaximum { get; protected set; }

        /// <summary>
        /// Gets or sets the actual minimum value of the axis.
        /// If Minimum is not NaN, this value will be defined by Minimum.
        /// If ViewMinimum is not NaN, this value will be defined by ViewMinimum.
        /// Otherwise this value will be defined by the minimum (+padding) of the data.
        /// </summary>
        public double ActualMinimum { get; protected set; }

        /// <summary>
        /// Gets or sets the actual minor step.
        /// </summary>
        public double ActualMinorStep { get; protected set; }

        /// <summary>
        /// Gets the actual string format being used.
        /// </summary>
        public string ActualStringFormat { get; protected set; }

        /// <summary>
        /// Gets the actual title (including Unit if Unit is set).
        /// </summary>
        /// <value>The actual title.</value>
        public string ActualTitle
        {
            get
            {
                if (this.Unit != null)
                {
                    return String.Format(this.TitleFormatString, this.Title, this.Unit);
                }
                return this.Title;
            }
        }

        /// <summary>
        /// Gets or sets the angle for the axis values.
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the end position of the axis on the plot area.
        /// This is a fraction from 0(bottom/left) to 1(top/right).
        /// </summary>
        public double EndPosition { get; set; }

        /// <summary>
        /// Gets or sets the color of the extra gridlines.
        /// </summary>
        public OxyColor ExtraGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the extra gridlines linestyle.
        /// </summary>
        public LineStyle ExtraGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the extra gridline thickness.
        /// </summary>
        public double ExtraGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the values for extra gridlines.
        /// </summary>
        public double[] ExtraGridlines { get; set; }

        /// <summary>
        /// Gets or sets the filter function. 
        /// </summary>
        /// <value>The filter function.</value>
        public Func<double, bool> FilterFunction { get; set; }

        /// <summary>
        /// Gets or sets the maximum value that can be shown using this axis.
        /// Values greater or equal to this value will not be shown.
        /// </summary>
        /// <value>The filter max value.</value>
        public double FilterMaxValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value that can be shown using this axis.
        /// Values smaller or equal to this value will not be shown.
        /// </summary>
        /// <value>The filter min value.</value>
        public double FilterMinValue { get; set; }

        /// <summary>
        /// Gets or sets the font name.
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        /// Gets the actual font.
        /// </summary>
        public string ActualFont
        {
            get
            {
                return Font ?? PlotModel.DefaultFont;
            }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public double FontWeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this axis is visible.
        /// </summary>
        public bool IsAxisVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pan is enabled.
        /// </summary>
        public bool IsPanEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this axis is reversed.
        /// It is reversed if StartPosition>EndPosition.
        /// </summary>
        public bool IsReversed
        {
            get
            {
                return this.StartPosition > this.EndPosition;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether zoom is enabled.
        /// </summary>
        public bool IsZoomEnabled { get; set; }

        /// <summary>
        /// Gets or sets the key of the axis.
        /// This can be used to find an axis if you have 
        /// defined mutiple axes in a plot.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the color of the major gridline.
        /// </summary>
        public OxyColor MajorGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the major gridline style.
        /// </summary>
        public LineStyle MajorGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the major gridline thickness.
        /// </summary>
        public double MajorGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the major step.
        /// (the interval between large ticks with numbers).
        /// </summary>
        public double MajorStep { get; set; }

        /// <summary>
        /// Gets or sets the size of the major tick.
        /// </summary>
        public double MajorTickSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the axis.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the 'padding' fraction of the maximum value.
        /// A value of 0.01 gives 1% more space on the maximum end of the axis.
        /// This property is not used if the Maximum property is set.
        /// </summary>
        public double MaximumPadding { get; set; }

        /// <summary>
        /// Gets or sets the mid point coordinates of the axis.
        /// </summary>
        /// <value>
        /// The mid point.
        /// </value>
        public ScreenPoint MidPoint { get; protected set; }

        /// <summary>
        /// Gets or sets the minimum value of the axis.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the 'padding' fraction of the minimum value.
        /// A value of 0.01 gives 1% more space on the minimum end of the axis.
        /// This property is not used if the Minimum property is set.
        /// </summary>
        public double MinimumPadding { get; set; }

        /// <summary>
        /// Gets or sets the minimum range of the axis.
        /// Setting this property ensures that ActualMaximum-ActualMinimum > MinimumRange.
        /// </summary>
        public double MinimumRange { get; set; }

        /// <summary>
        /// Gets or sets the color of the minor gridline.
        /// </summary>
        public OxyColor MinorGridlineColor { get; set; }

        /// <summary>
        /// Gets or sets the minor gridline style.
        /// </summary>
        public LineStyle MinorGridlineStyle { get; set; }

        /// <summary>
        /// Gets or sets the minor gridline thickness.
        /// </summary>
        public double MinorGridlineThickness { get; set; }

        /// <summary>
        /// Gets or sets the minor step 
        /// (the interval between small ticks without number).
        /// </summary>
        public double MinorStep { get; set; }

        /// <summary>
        /// Gets or sets the size of the minor tick.
        /// </summary>
        public double MinorTickSize { get; set; }

        public double Offset { get; protected set; }

        /// <summary>
        /// Gets or sets the position of the axis.
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
        /// Gets or sets a value indicating whether the axis should
        /// be positioned on the zero-crossing of the related axis.
        /// </summary>
        public bool PositionAtZeroCrossing { get; set; }

        /// <summary>
        /// Gets or sets the related axis.
        /// This is used for polar coordinate systems where
        /// the angle and magnitude axes are related.
        /// </summary>
        public AxisBase RelatedAxis { get; set; }

        public double Scale
        {
            get
            {
                return this.scale;
            }
            protected set
            {
                this.scale = value;
            }
        }

        /// <summary>
        /// Gets the screen coordinate of the Maximum point on the axis.
        /// </summary>
        public ScreenPoint ScreenMax { get; protected set; }

        /// <summary>
        /// Gets the screen coordinate of the Minimum point on the axis.
        /// </summary>
        public ScreenPoint ScreenMin { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether minor ticks should be shown.
        /// </summary>
        public bool ShowMinorTicks { get; set; }

        /// <summary>
        /// Gets or sets the start position of the axis on the plot area.
        /// This is a fraction from 0(bottom/left) to 1(top/right).
        /// </summary>
        public double StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the string format used
        /// for formatting the axis values.
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or sets the tick style.
        /// </summary>
        public TickStyle TickStyle { get; set; }

        /// <summary>
        /// Gets or sets the color of the ticks.
        /// </summary>
        public OxyColor TicklineColor { get; set; }

        /// <summary>
        /// Gets or sets the title of the axis.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the format string used for formatting the title and unit when unit is defined.
        /// If unit is null, only Title is used.
        /// The default value is "{0} [{1}]", where {0} uses the Title and {1} uses the Unit.
        /// </summary>
        public string TitleFormatString { get; set; }

        /// <summary>
        /// Position of the title (0.5 is in the middle).
        /// </summary>
        public double TitlePosition { get; set; }

        /// <summary>
        /// Gets or sets the unit of the axis.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use superscript exponential format.
        /// This format will convert 1.5E+03 to 1.5·10^{3} and render the superscript properly
        /// If StringFormat is null, 1.0E+03 will be converted to 10^{3}
        /// </summary>
        public bool UseSuperExponentialFormat { get; set; }

        #endregion

        #region Properties

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

        #endregion

        #region Public Methods

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

            return new DataPoint(
                xAxis != null ? xAxis.InverseTransform(x) : 0, yAxis != null ? yAxis.InverseTransform(y) : 0);
        }

        public static ScreenPoint Transform(DataPoint p, IAxis xAxis, IAxis yAxis)
        {
            return xAxis.Transform(p, yAxis);
        }

        public virtual string FormatValue(double x)
        {
            // The "SuperExponentialFormat" renders the number with superscript exponents. E.g. 10^2
            if (this.UseSuperExponentialFormat)
            {
                // if (x == 1 || x == 10 || x == -1 || x == -10)
                //    return x.ToString();

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
                return String.Format(CultureInfo.InvariantCulture, fmt, mantissa, exp);
            }
            string format = this.ActualStringFormat ?? this.StringFormat ?? String.Empty;
            return x.ToString(format, CultureInfo.InvariantCulture);
        }

        public virtual string FormatValueForTracker(double x)
        {
            return x.ToNiceString();
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public virtual void GetTickValues(
            out ICollection<double> majorLabelValues,
            out ICollection<double> majorTickValues,
            out ICollection<double> minorTickValues)
        {
            minorTickValues = CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMinorStep);
            majorTickValues = CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMajorStep);
            majorLabelValues = majorTickValues;
        }

        public virtual object GetValue(double x)
        {
            return x;
        }

        //private static double Truncate(double value, double precision)
        //{
        //    return Math.Truncate(value * precision) / precision;
        //}

        public void Include(double value)
        {
            if (!this.IsValidValue(value))
            {
                return;
            }

            this.ActualMinimum = double.IsNaN(this.ActualMinimum) ? value : Math.Min(this.ActualMinimum, value);
            this.ActualMaximum = double.IsNaN(this.ActualMaximum) ? value : Math.Max(this.ActualMaximum, value);
        }

        public double InverseTransform(double x)
        {
            return this.PostInverseTransform(x / this.scale + this.Offset);
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
            if (this.IsPolar())
            {
                x -= this.MidPoint.x;
                y -= this.MidPoint.y;
                double th = Math.Atan2(y, x);
                double r = Math.Sqrt(x * x + y * y);
                x = r / this.scale + this.Offset;
                y = yAxis != null ? th / yAxis.Scale + yAxis.Offset : double.NaN;
                return new DataPoint(x, y);
            }

            return new DataPoint(this.InverseTransform(x), yAxis.InverseTransform(y));
        }

        public bool IsHorizontal()
        {
            return this.position == AxisPosition.Top || this.position == AxisPosition.Bottom;
        }

        public bool IsPolar()
        {
            return this.position == AxisPosition.Magnitude || this.position == AxisPosition.Angle;
        }

        public virtual bool IsValidValue(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value < this.FilterMaxValue
                   && value > this.FilterMinValue && (this.FilterFunction == null || this.FilterFunction(value));
        }

        public bool IsVertical()
        {
            return this.position == AxisPosition.Left || this.position == AxisPosition.Right;
        }

        /// <summary>
        /// Pans the axis.
        /// </summary>
        /// <param name="x0">The previous screen coordinate.</param>
        /// <param name="x1">The current screen coordinate.</param>
        public virtual void Pan(double x0, double x1)
        {
            if (!this.IsPanEnabled)
            {
                return;
            }

            double newMinimum = this.ActualMinimum + x0 - x1;
            double newMaximum = this.ActualMaximum + x0 - x1;
            if (newMinimum < this.AbsoluteMinimum)
            {
                newMinimum = this.AbsoluteMinimum;
                newMaximum = newMinimum + this.ActualMaximum - this.ActualMinimum;
            }
            if (newMaximum > this.AbsoluteMaximum)
            {
                newMaximum = this.AbsoluteMaximum;
                newMinimum = newMaximum - (this.ActualMaximum - this.ActualMinimum);
            }

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan));
        }

        public virtual void Render(IRenderContext rc, PlotModel model)
        {
            switch (this.Position)
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

        public virtual void Reset()
        {
            this.ViewMinimum = double.NaN;
            this.ViewMaximum = double.NaN;
            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Reset));
        }

        /// <summary>
        /// Resets the actual max and min.
        /// </summary>
        public virtual void ResetActualMaxMin()
        {
            this.ActualMaximum = this.ActualMinimum = double.NaN;
        }

        public void SetScale(double newScale)
        {
            double sx1 = (this.ActualMaximum - this.Offset) * this.scale;
            double sx0 = (this.ActualMinimum - this.Offset) * this.scale;

            double sgn = Math.Sign(this.scale);
            double mid = (this.ActualMaximum + this.ActualMinimum) / 2;

            double dx = (this.Offset - mid) * this.scale;
            this.scale = sgn * newScale;
            this.Offset = dx / this.scale + mid;
            this.ActualMaximum = sx1 / this.scale + this.Offset;
            this.ActualMinimum = sx0 / this.scale + this.Offset;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "{0}({1}, {2}, {3}, {4})",
                this.GetType().Name,
                this.Position,
                this.ActualMinimum,
                this.ActualMaximum,
                this.ActualMajorStep);
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

            if (this.IsPolar())
            {
                double r = (dp.x - this.Offset) * this.scale;
                double th = yAxis != null ? (dp.y - yAxis.Offset) * yAxis.Scale : double.NaN;
                return new ScreenPoint(this.MidPoint.x + r * Math.Cos(th), this.MidPoint.y + r * Math.Sin(th));
            }

            if (yAxis == null)
            {
                return new ScreenPoint();
            }

            return new ScreenPoint(this.Transform(dp.x), yAxis.Transform(dp.y));
        }

        // todo: should find a better way to do this
        // this method seems to be a bottleneck for performance...
        public double Transform(double x)
        {
            return (this.PreTransform(x) - this.Offset) * this.scale;
        }

        /// <summary>
        /// Updates the actual max and min with the 'padding' values.
        /// </summary>
        public virtual void UpdateActualMaxMin()
        {
            double range = this.ActualMaximum - this.ActualMinimum;
            double zeroRange = this.ActualMaximum > 0 ? this.ActualMaximum : 1;

            if (!double.IsNaN(this.ViewMaximum))
            {
                this.ActualMaximum = this.ViewMaximum;
            }
            else if (!double.IsNaN(this.Maximum))
            {
                this.ActualMaximum = this.Maximum;
            }
            else
            {
                if (range < double.Epsilon)
                {
                    this.ActualMaximum += zeroRange * 0.5;
                }

                double x1 = this.PreTransform(this.ActualMaximum);
                double x0 = this.PreTransform(this.ActualMinimum);
                double dx = this.MaximumPadding * (x1 - x0);
                this.ActualMaximum = this.PostInverseTransform(x1 + dx);
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
                if (range < double.Epsilon)
                {
                    this.ActualMinimum -= zeroRange * 0.5;
                }

                double x1 = this.PreTransform(this.ActualMaximum);
                double x0 = this.PreTransform(this.ActualMinimum);
                double dx = this.MinimumPadding * (x1 - x0);
                this.ActualMinimum = this.PostInverseTransform(x0 - dx);
            }

            if (double.IsNaN(this.ActualMaximum))
            {
                this.ActualMaximum = 100;
            }
            if (double.IsNaN(this.ActualMinimum))
            {
                this.ActualMinimum = this is LogarithmicAxis ? 1 : 0;
            }

            this.EnsureRange();
        }

        public virtual void UpdateData(IEnumerable<ISeries> series)
        {
        }

        /// <summary>
        /// Updates the minor/major step intervals if they are undefined.
        /// </summary>
        public virtual void UpdateIntervals(OxyRect plotArea)
        {
            double labelSize = this.GetLabelSize();
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

            this.ActualStringFormat = this.StringFormat;

            //if (ActualStringFormat==null)
            //{
            //    if (ActualMaximum > 1e6 || ActualMinimum < 1e-6)
            //        ActualStringFormat = "#.#e-0";
            //}
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

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            this.MidPoint = new ScreenPoint((x0 + x1) / 2, (y0 + y1) / 2);

            if (this.Position == AxisPosition.Angle)
            {
                this.scale = 2 * Math.PI / (this.ActualMaximum - this.ActualMinimum);
                this.Offset = this.ActualMinimum;
                return;
            }
            if (this.Position == AxisPosition.Magnitude)
            {
                this.ActualMinimum = 0;
                double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
                this.scale = 0.5 * r / (this.ActualMaximum - this.ActualMinimum);
                this.Offset = this.ActualMinimum;
                return;
            }

            double a0 = this.IsHorizontal() ? x0 : y0;
            double a1 = this.IsHorizontal() ? x1 : y1;

            double dx = a1 - a0;
            a1 = a0 + this.EndPosition * dx;
            a0 = a0 + this.StartPosition * dx;
            this.ScreenMin = new ScreenPoint(a0, a1);
            this.ScreenMax = new ScreenPoint(a1, a0);

            if (this.ActualMaximum - this.ActualMinimum < double.Epsilon)
            {
                this.ActualMaximum = this.ActualMinimum + 1;
            }

            double max = this.PreTransform(this.ActualMaximum);
            double min = this.PreTransform(this.ActualMinimum);

            double da = a0 - a1;
            if (Math.Abs(da) != 0)
            {
                this.Offset = a0 / da * max - a1 / da * min;
            }
            else
            {
                this.Offset = 0;
            }

            double range = max - min;
            if (Math.Abs(range) != 0)
            {
                this.scale = (a1 - a0) / range;
            }
            else
            {
                this.scale = 1;
            }
        }

        public virtual void Zoom(double x0, double x1)
        {
            if (!this.IsZoomEnabled)
            {
                return;
            }

            this.ViewMinimum = Math.Max(Math.Min(x0, x1), this.AbsoluteMinimum);
            this.ViewMaximum = Math.Min(Math.Max(x0, x1), this.AbsoluteMaximum);
            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom));
        }

        public virtual void ZoomAt(double factor, double x)
        {
            if (!this.IsZoomEnabled)
            {
                return;
            }
            double dx0 = (this.ActualMinimum - x) * this.scale;
            double dx1 = (this.ActualMaximum - x) * this.scale;
            this.scale *= factor;

            this.ViewMinimum = Math.Max(dx0 / this.scale + x, this.AbsoluteMinimum);
            this.ViewMaximum = Math.Min(dx1 / this.scale + x, this.AbsoluteMaximum);

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates tick values at the specified interval.
        /// </summary>
        /// <param name="min">The minimum coordinate.</param>
        /// <param name="max">The maximum coordinate.</param>
        /// <param name="step">The interval.</param>
        /// <returns></returns>
        internal static ICollection<double> CreateTickValues(double min, double max, double step)
        {
            if (max <= min)
            {
                throw new ArgumentException("Axis: Maximum should be larger than minimum.", "max");
            }
            if (step <= 0)
            {
                throw new ArgumentException("Axis: Step cannot be zero or negative.", "step");
            }

            double x0 = Math.Round(min / step) * step;

            var values = new Collection<double>();

            // Limit the maximum number of iterations (in case something is wrong with the step size)
            int i = 0;
            const int maxIterations = 1000;
            double x = x0;

            while (x <= max + double.Epsilon && i < maxIterations)
            {
                x = x0 + (i * step);
                i++;
                if (x >= min - double.Epsilon && x <= max + double.Epsilon)
                {
                    x = RemoveNoiseFromDoubleMath(x);
                    values.Add(x);
                }
            }
            return values;
        }

        /// <summary>
        /// Removes the noise from double math.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A double without noise.</returns>
        internal static double RemoveNoiseFromDoubleMath(double value)
        {
            if (value == 0.0 || Math.Abs((Math.Log10(Math.Abs(value)))) < 27)
            {
                return (double)((decimal)value);
            }
            return Double.Parse(value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Calculates the actual interval.
        /// </summary>
        /// <param name="availableSize">Size of the available area.</param>
        /// <param name="maxIntervalSize">Maximum length of the intervals.</param>
        /// <returns></returns>
        protected virtual double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            return this.CalculateActualInterval(availableSize, maxIntervalSize, this.ActualMaximum - this.ActualMinimum);
        }

        // alternative algorithm not in use
        /*        private double CalculateActualIntervalOldAlgorithm(double availableSize, double maxIntervalSize)
                {
                    const int minimumTags = 5;
                    const int maximumTags = 20;
                    var numberOfTags = (int) (availableSize/maxIntervalSize);
                    double range = ActualMaximum - ActualMinimum;
                    double interval = range/numberOfTags;
                    const int k1 = 10;
                    interval = Math.Log10(interval/k1);
                    interval = Math.Ceiling(interval);
                    interval = Math.Pow(10, interval)*k1;

                    if (range/interval > maximumTags) interval *= 5;
                    if (range/interval < minimumTags) interval *= 0.5;

                    if (interval <= 0) interval = 1;
                    return interval;
                }*/

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
        /// <param name="range"></param>
        /// <returns>Actual interval to use to determine which values are 
        /// displayed in the axis.
        /// </returns>
        protected double CalculateActualInterval(double availableSize, double maxIntervalSize, double range)
        {
            if (availableSize <= 0)
            {
                return maxIntervalSize;
            }

            Func<double, double> exponent = x => Math.Ceiling(Math.Log(x, 10));
            Func<double, double> mantissa = x => x / Math.Pow(10, exponent(x) - 1);

            // reduce intervals for horizontal axis.
            // double maxIntervals = Orientation == AxisOrientation.x ? MaximumAxisIntervalsPer200Pixels * 0.8 : MaximumAxisIntervalsPer200Pixels;
            // real maximum interval count
            double maxIntervalCount = availableSize / maxIntervalSize;

            range = Math.Abs(range);
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
                    // reduce 2 to 1, 10 to 5, 1 to 0.5
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
                if (double.IsNaN(tempInterval) || double.IsInfinity(tempInterval))
                {
                    break;
                }
                interval = tempInterval;
            }
            return interval;
        }

        protected double CalculateMinorInterval(double majorInterval)
        {
            // if major interval is 100, the minor interval will be 20.
            return majorInterval / 5;

            // The following obsolete code divided major intervals into 4 minor intervals, unless the major interval's mantissa was 5.
            // e.g. Major interval 100 => minor interval 25.

            //Func<double, double> exponent = x => Math.Ceiling(Math.Log(x, 10));
            //Func<double, double> mantissa = x => x / Math.Pow(10, exponent(x) - 1);
            //var m = (int)mantissa(majorInterval);
            //switch (m)
            //{
            //    case 5:
            //        return majorInterval / 5;
            //    default:
            //        return majorInterval / 4;
            //}
        }

        protected virtual void EnsureRange()
        {
            double range = this.ActualMaximum - this.ActualMinimum;
            if (range < this.MinimumRange)
            {
                double avg = (this.ActualMaximum + this.ActualMinimum) * 0.5;
                this.ActualMinimum = avg - this.MinimumRange * 0.5;
                this.ActualMaximum = avg + this.MinimumRange * 0.5;
            }
        }

        /// <summary>
        /// Gets the size of the label.
        /// </summary>
        /// <returns></returns>
        protected virtual double GetLabelSize()
        {
            // todo: this could be dependent on the stringformat 
            // and min/max numbers
            // could format the string for min and max...

            switch (this.position)
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

        protected virtual void OnAxisChanged(AxisChangedEventArgs args)
        {
            EventHandler<AxisChangedEventArgs> handler = this.AxisChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual double PostInverseTransform(double x)
        {
            return x;
        }

        protected virtual double PreTransform(double x)
        {
            return x;
        }

        #endregion
    }
}