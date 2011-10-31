// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisBase.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Axis base class that is used by the linear and logarithmic axes.
    /// </summary>
    public abstract class AxisBase : Axis
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "AxisBase" /> class.
        /// </summary>
        protected AxisBase()
        {
            this.ViewMaximum = double.NaN;
            this.ViewMinimum = double.NaN;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisBase"/> class.
        /// </summary>
        /// <param name="pos">
        /// The position.
        /// </param>
        /// <param name="minimum">
        /// The minimum value.
        /// </param>
        /// <param name="maximum">
        /// The maximum value.
        /// </param>
        /// <param name="title">
        /// The title shown next to the axis.
        /// </param>
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

        #region Properties

        /// <summary>
        ///   Gets or sets the position tier max shift.
        /// </summary>
        /// <value>The position tier max shift.</value>
        internal double PositionTierMaxShift { get; set; }

        /// <summary>
        ///   Gets or sets the position tier min shift.
        /// </summary>
        /// <value>The position tier min shift.</value>
        internal double PositionTierMinShift { get; set; }

        /// <summary>
        ///   Gets or sets the size of the position tier.
        /// </summary>
        /// <value>The size of the position tier.</value>
        internal double PositionTierSize { get; set; }

        /// <summary>
        ///   Gets or sets the current view's maximum. This value is used when the user zooms or pans.
        /// </summary>
        /// <value>The view maximum.</value>
        protected double ViewMaximum { get; set; }

        /// <summary>
        ///   Gets or sets the current view's minimum. This value is used when the user zooms or pans.
        /// </summary>
        /// <value>The view minimum.</value>
        protected double ViewMinimum { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="p">
        /// The point.
        /// </param>
        /// <param name="xaxis">
        /// The x axis.
        /// </param>
        /// <param name="yaxis">
        /// The y axis.
        /// </param>
        /// <returns>
        /// The transformed point.
        /// </returns>
        public static ScreenPoint Transform(DataPoint p, IAxis xaxis, IAxis yaxis)
        {
            return xaxis.Transform(p.x, p.y, yaxis);
        }

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="p">
        /// The point.
        /// </param>
        /// <param name="xaxis">
        /// The x axis.
        /// </param>
        /// <param name="yaxis">
        /// The y axis.
        /// </param>
        /// <returns>
        /// The transformed point.
        /// </returns>
        public static ScreenPoint Transform(IDataPoint p, IAxis xaxis, IAxis yaxis)
        {
            return xaxis.Transform(p.X, p.Y, yaxis);
        }

        /// <summary>
        /// Coerces the actual maximum and minimum values.
        /// </summary>
        public virtual void CoerceActualMaxMin()
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

            if (this.ActualMaximum <= this.ActualMinimum)
            {
                this.ActualMaximum = this.ActualMinimum + 100;
            }

            // Coerce the minimum range
            double range = this.ActualMaximum - this.ActualMinimum;
            if (range < this.MinimumRange)
            {
                double avg = (this.ActualMaximum + this.ActualMinimum) * 0.5;
                this.ActualMinimum = avg - this.MinimumRange * 0.5;
                this.ActualMaximum = avg + this.MinimumRange * 0.5;
            }

            if (this.AbsoluteMaximum <= this.AbsoluteMinimum)
            {
                throw new InvalidOperationException("AbsoluteMaximum should be larger than AbsoluteMinimum.");
            }
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        public override string FormatValue(double x)
        {
            // The "SuperExponentialFormat" renders the number with superscript exponents. E.g. 10^2
            if (this.UseSuperExponentialFormat)
            {
                // if (x == 1 || x == 10 || x == -1 || x == -10)
                // return x.ToString();
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

            string format = this.ActualStringFormat ?? this.StringFormat ?? string.Empty;
            return x.ToString(format, this.ActualCulture);
        }

        /// <summary>
        /// Formats the value to be used by the tracker.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        public override string FormatValueForTracker(double x)
        {
            return x.ToNiceString();
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">
        /// The major label values.
        /// </param>
        /// <param name="majorTickValues">
        /// The major tick values.
        /// </param>
        /// <param name="minorTickValues">
        /// The minor tick values.
        /// </param>
        public virtual void GetTickValues(
            out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            minorTickValues = CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMinorStep);
            majorTickValues = CreateTickValues(this.ActualMinimum, this.ActualMaximum, this.ActualMajorStep);
            majorLabelValues = majorTickValues;
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if neccessary.
        ///   e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">
        /// The coordinate.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        public override object GetValue(double x)
        {
            return x;
        }

        /// <summary>
        /// Inverse transform the specified screen point.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <param name="yaxis">
        /// The y-axis.
        /// </param>
        /// <returns>
        /// The data point.
        /// </returns>
        public override DataPoint InverseTransform(double x, double y, IAxis yaxis)
        {
            return new DataPoint(this.InverseTransform(x), yaxis != null ? yaxis.InverseTransform(y) : 0);
        }

        /// <summary>
        /// Inverse transform the specified screen coordinate.
        ///   This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="sx">
        /// The screen coordinate.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        public override double InverseTransform(double sx)
        {
            return this.PostInverseTransform(sx / this.scale + this.Offset);
        }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified value is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValidValue(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value < this.FilterMaxValue
                   && value > this.FilterMinValue && (this.FilterFunction == null || this.FilterFunction(value));
        }

        /// <summary>
        /// Measures the size of the axis (maximum axis label width/height).
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <returns>
        /// The size of the axis.
        /// </returns>
        public override OxySize Measure(IRenderContext rc)
        {
            IList<double> majorTickValues;
            IList<double> minorTickValues;
            IList<double> majorLabelValues;

            this.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);

            var maximumTextSize = new OxySize();
            foreach (double v in majorLabelValues)
            {
                string s = this.FormatValue(v);
                OxySize size = rc.MeasureText(s, this.ActualFont, this.FontSize, this.FontWeight);
                if (size.Width > maximumTextSize.Width)
                {
                    maximumTextSize.Width = size.Width;
                }

                if (size.Height > maximumTextSize.Height)
                {
                    maximumTextSize.Height = size.Height;
                }
            }

            OxySize labelTextSize = rc.MeasureText(this.ActualTitle, this.ActualFont, this.FontSize, this.FontWeight);

            double width = 0;
            double height = 0;

            if (this.IsHorizontal())
            {
                switch (this.TickStyle)
                {
                    case TickStyle.Outside:
                        height += this.MajorTickSize;
                        break;
                    case TickStyle.Crossing:
                        height += this.MajorTickSize * 0.75;
                        break;
                }

                height += this.AxisTickToLabelDistance;
                height += maximumTextSize.Height;
                if (labelTextSize.Height > 0)
                {
                    height += this.AxisTitleDistance;
                    height += labelTextSize.Height;
                }
            }
            else
            {
                switch (this.TickStyle)
                {
                    case TickStyle.Outside:
                        width += this.MajorTickSize;
                        break;
                    case TickStyle.Crossing:
                        width += this.MajorTickSize * 0.75;
                        break;
                }

                width += this.AxisTickToLabelDistance;
                width += maximumTextSize.Width;
                if (labelTextSize.Height > 0)
                {
                    width += this.AxisTitleDistance;
                    width += labelTextSize.Height;
                }
            }

            return new OxySize(width, height);
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="ppt">
        /// The previous point (screen coordinates).
        /// </param>
        /// <param name="cpt">
        /// The current point (screen coordinates).
        /// </param>
        public override void Pan(ScreenPoint ppt, ScreenPoint cpt)
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
        /// <param name="delta">
        /// The delta.
        /// </param>
        public override void Pan(double delta)
        {
            if (!this.IsPanEnabled)
            {
                return;
            }

            double dx = delta / this.Scale;

            double newMinimum = this.ActualMinimum - dx;
            double newMaximum = this.ActualMaximum - dx;
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

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="axisLayer">
        /// The rendering order.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer)
        {
            if (this.Layer != axisLayer)
            {
                return;
            }

            var r = new HorizontalAndVerticalAxisRenderer(rc, model);
            r.Render(this);
        }

        /// <summary>
        /// Resets the user's modification (zooming/panning) to minmum and maximum of this axis.
        /// </summary>
        public override void Reset()
        {
            this.ViewMinimum = double.NaN;
            this.ViewMaximum = double.NaN;
            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Reset));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                "{0}({1}, {2}, {3}, {4})", 
                TypeHelper.GetTypeName(this.GetType()), 
                this.Position, 
                this.ActualMinimum, 
                this.ActualMaximum, 
                this.ActualMajorStep);
        }

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="x">
        /// The x value (for the current axis).
        /// </param>
        /// <param name="y">
        /// The y value.
        /// </param>
        /// <param name="yaxis">
        /// The y axis.
        /// </param>
        /// <returns>
        /// The transformed point.
        /// </returns>
        public override ScreenPoint Transform(double x, double y, IAxis yaxis)
        {
            if (yaxis == null)
            {
                throw new NullReferenceException("Y axis should not be null when transforming.");
            }

            return new ScreenPoint(this.Transform(x), yaxis.Transform(y));
        }

        /// <summary>
        /// Transforms the specified coordinate to screen coordinates.
        ///   This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The transformed value (screen coordinate).
        /// </returns>
        public override double Transform(double x)
        {
            return (x - this.offset) * this.scale;

            // return (this.PreTransform(x) - this.Offset) * this.Scale;
        }

        /// <summary>
        /// Zoom to the specified scale.
        /// </summary>
        /// <param name="newScale">
        /// The new scale.
        /// </param>
        public override void Zoom(double newScale)
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
        /// Zooms the axis to the range [x0,x1].
        /// </summary>
        /// <param name="x0">
        /// The new minimum.
        /// </param>
        /// <param name="x1">
        /// The new maximum.
        /// </param>
        public override void Zoom(double x0, double x1)
        {
            if (!this.IsZoomEnabled)
            {
                return;
            }

            this.ViewMinimum = Math.Max(Math.Min(x0, x1), this.AbsoluteMinimum);
            this.ViewMaximum = Math.Min(Math.Max(x0, x1), this.AbsoluteMaximum);
            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom));
        }

        /// <summary>
        /// Zooms the axis at the specified coordinate.
        /// </summary>
        /// <param name="factor">
        /// The zoom factor.
        /// </param>
        /// <param name="x">
        /// The coordinate to zoom at.
        /// </param>
        public override void ZoomAt(double factor, double x)
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
        /// Updates the actual maximum and minimum values.
        ///   If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum values will be used.
        ///   If Maximum or Minimum have been set, these values will be used.
        ///   Otherwise the maximum and minimum values of the series will be used, including the 'padding'.
        /// </summary>
        internal override void UpdateActualMaxMin()
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

                if (!double.IsNaN(this.ActualMaximum) && !double.IsNaN(this.ActualMaximum))
                {
                    double x1 = this.PreTransform(this.ActualMaximum);
                    double x0 = this.PreTransform(this.ActualMinimum);
                    double dx = this.MaximumPadding * (x1 - x0);
                    this.ActualMaximum = this.PostInverseTransform(x1 + dx);
                }
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

                if (!double.IsNaN(this.ActualMaximum) && !double.IsNaN(this.ActualMaximum))
                {
                    double x1 = this.PreTransform(this.ActualMaximum);
                    double x0 = this.PreTransform(this.ActualMinimum);
                    double dx = this.MinimumPadding * (x1 - x0);
                    this.ActualMinimum = this.PostInverseTransform(x0 - dx);
                }
            }

            this.CoerceActualMaxMin();
        }

        /// <summary>
        /// Updates the axis with information from the plot series.
        ///   This is used by the category axis that need to know the number of series using the axis.
        /// </summary>
        /// <param name="series">
        /// The series collection.
        /// </param>
        internal override void UpdateFromSeries(IEnumerable<Series> series)
        {
        }

        /// <summary>
        /// Updates the actual minor and major step intervals.
        /// </summary>
        /// <param name="plotArea">
        /// The plot area rectangle.
        /// </param>
        internal override void UpdateIntervals(OxyRect plotArea)
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

            this.ActualStringFormat = this.StringFormat;

            // if (ActualStringFormat==null)
            // {
            // if (ActualMaximum > 1e6 || ActualMinimum < 1e-6)
            // ActualStringFormat = "#.#e-0";
            // }
        }

        /// <summary>
        /// Updates the scale and offset properties of the transform
        ///   from the specified boundary rectangle.
        /// </summary>
        /// <param name="bounds">
        /// The bounds.
        /// </param>
        internal override void UpdateTransform(OxyRect bounds)
        {
            double x0 = bounds.Left;
            double x1 = bounds.Right;
            double y0 = bounds.Bottom;
            double y1 = bounds.Top;

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            // this.MidPoint = new ScreenPoint((x0 + x1) / 2, (y0 + y1) / 2);

            // if (this.Position == AxisPosition.Angle)
            // {
            // this.scale = 2 * Math.PI / (this.ActualMaximum - this.ActualMinimum);
            // this.Offset = this.ActualMinimum;
            // return;
            // }

            // if (this.Position == AxisPosition.Magnitude)
            // {
            // this.ActualMinimum = 0;
            // double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
            // this.scale = 0.5 * r / (this.ActualMaximum - this.ActualMinimum);
            // this.Offset = this.ActualMinimum;
            // return;
            // }
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
            if (Math.Abs(da) > double.Epsilon)
            {
                this.Offset = a0 / da * max - a1 / da * min;
            }
            else
            {
                this.Offset = 0;
            }

            double range = max - min;
            if (Math.Abs(range) > double.Epsilon)
            {
                this.scale = (a1 - a0) / range;
            }
            else
            {
                this.scale = 1;
            }
        }

        /// <summary>
        /// Creates tick values at the specified interval.
        /// </summary>
        /// <param name="min">
        /// The minimum coordinate.
        /// </param>
        /// <param name="max">
        /// The maximum coordinate.
        /// </param>
        /// <param name="step">
        /// The interval.
        /// </param>
        /// <returns>
        /// A list of tick values.
        /// </returns>
        protected static IList<double> CreateTickValues(double min, double max, double step)
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
            int n = Math.Max((int)((max - min) / step), 1);
            var values = new List<double>(n);

            // Limit the maximum number of iterations (in case something is wrong with the step size)
            int i = 0;
            const int MaxIterations = 1000;
            double x = x0;
            double eps = step * 1e-3;

            while (x <= max + eps && i < MaxIterations)
            {
                x = x0 + (i * step);
                i++;
                if (x >= min - eps && x <= max + eps)
                {
                    x = x.RemoveNoiseFromDoubleMath();
                    values.Add(x);
                }
            }

            return values;
        }

        /// <summary>
        /// Calculates the actual interval.
        /// </summary>
        /// <param name="availableSize">
        /// Size of the available area.
        /// </param>
        /// <param name="maxIntervalSize">
        /// Maximum length of the intervals.
        /// </param>
        /// <returns>
        /// The calculate actual interval.
        /// </returns>
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
        /// Returns the actual interval to use to determine which values are displayed in the axis.
        /// </summary>
        /// <param name="availableSize">
        /// The available size.
        /// </param>
        /// <param name="maxIntervalSize">
        /// The maximum interval size.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// Actual interval to use to determine which values are displayed in the axis.
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
                    tempInterval = (tempInterval / 2.5).RemoveNoiseFromDoubleMath();
                }
                else if (m == 2 || m == 1 || m == 10)
                {
                    // reduce 2 to 1, 10 to 5, 1 to 0.5
                    tempInterval = (tempInterval / 2.0).RemoveNoiseFromDoubleMath();
                }
                else
                {
                    tempInterval = (tempInterval / 2.0).RemoveNoiseFromDoubleMath();
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

        /// <summary>
        /// The calculate minor interval.
        /// </summary>
        /// <param name="majorInterval">
        /// The major interval.
        /// </param>
        /// <returns>
        /// The minor interval.
        /// </returns>
        protected double CalculateMinorInterval(double majorInterval)
        {
            // if major interval is 100, the minor interval will be 20.
            return majorInterval / 5;

            // The following obsolete code divided major intervals into 4 minor intervals, unless the major interval's mantissa was 5.
            // e.g. Major interval 100 => minor interval 25.

            // Func<double, double> exponent = x => Math.Ceiling(Math.Log(x, 10));
            // Func<double, double> mantissa = x => x / Math.Pow(10, exponent(x) - 1);
            // var m = (int)mantissa(majorInterval);
            // switch (m)
            // {
            // case 5:
            // return majorInterval / 5;
            // default:
            // return majorInterval / 4;
            // }
        }

        #endregion
    }
}