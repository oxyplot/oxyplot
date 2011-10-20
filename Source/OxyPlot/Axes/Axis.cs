//-----------------------------------------------------------------------
// <copyright file="Axis.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Abstract base class for axes.
    /// </summary>
    /// <remarks>
    /// This class contains all properties from the IAxis interface.
    ///   This class contains methods that should only be accessible by the PlotModel.
    /// </remarks>
    [Serializable]
    public abstract class Axis : IAxis
    {
        #region Constants and Fields

        /// <summary>
        ///   The offset.
        /// </summary>
        internal double offset;

        /// <summary>
        ///   The scale.
        /// </summary>
        internal double scale;

        /// <summary>
        ///   Exponent function.
        /// </summary>
        protected static readonly Func<double, double> Exponent = x => Math.Round(Math.Log(Math.Abs(x), 10));

        /// <summary>
        ///   Mantissa function.
        ///   http://en.wikipedia.org/wiki/Mantissa
        /// </summary>
        protected static readonly Func<double, double> Mantissa = x => x / Math.Pow(10, Exponent(x));

        /// <summary>
        ///   The position.
        /// </summary>
        private AxisPosition position;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Axis" /> class.
        /// </summary>
        protected Axis()
        {
            this.Position = AxisPosition.Left;
            this.IsAxisVisible = true;
            this.Layer = AxisLayer.BelowSeries;

            this.AbsoluteMaximum = double.MaxValue;
            this.AbsoluteMinimum = double.MinValue;

            this.Minimum = double.NaN;
            this.Maximum = double.NaN;
            this.MinorStep = double.NaN;
            this.MajorStep = double.NaN;

            this.MinimumPadding = 0.01;
            this.MaximumPadding = 0.01;
            this.MinimumRange = 0;

            this.TickStyle = TickStyle.Outside;
            this.TicklineColor = OxyColors.Black;

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

            this.IntervalLength = 60;

            this.AxisTitleDistance = 4;
            this.AxisTickToLabelDistance = 4;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when the axis has been changed (by zooming, panning or resetting).
        /// </summary>
        public event EventHandler<AxisChangedEventArgs> AxisChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the absolute maximum. This is only used for the UI control.
        ///   It will not be possible to zoom/pan beyond this limit.
        /// </summary>
        /// <value>The absolute maximum.</value>
        public double AbsoluteMaximum { get; set; }

        /// <summary>
        ///   Gets or sets the absolute minimum. This is only used for the UI control.
        ///   It will not be possible to zoom/pan beyond this limit.
        /// </summary>
        /// <value>The absolute minimum.</value>
        public double AbsoluteMinimum { get; set; }

        /// <summary>
        ///   Gets the actual culture.
        /// </summary>
        /// <remarks>
        ///   The culture is defined in the parent PlotModel.
        /// </remarks>
        public CultureInfo ActualCulture
        {
            get
            {
                return this.PlotModel != null ? this.PlotModel.ActualCulture : CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        ///   Gets the actual font.
        /// </summary>
        public string ActualFont
        {
            get
            {
                return this.Font ?? PlotModel.DefaultFont;
            }
        }

        /// <summary>
        ///   Gets or sets the actual major step.
        /// </summary>
        public double ActualMajorStep { get; protected set; }

        /// <summary>
        ///   Gets or sets the actual maximum value of the axis.
        ///   If Maximum is not NaN, this value will be defined by Maximum.
        ///   If ViewMaximum is not NaN, this value will be defined by ViewMaximum.
        ///   Otherwise this value will be defined by the maximum (+padding) of the data.
        /// </summary>
        public double ActualMaximum { get; protected set; }

        /// <summary>
        ///   Gets or sets the actual minimum value of the axis.
        ///   If Minimum is not NaN, this value will be defined by Minimum.
        ///   If ViewMinimum is not NaN, this value will be defined by ViewMinimum.
        ///   Otherwise this value will be defined by the minimum (+padding) of the data.
        /// </summary>
        public double ActualMinimum { get; protected set; }

        /// <summary>
        ///   Gets or sets the actual minor step.
        /// </summary>
        public double ActualMinorStep { get; protected set; }

        /// <summary>
        ///   Gets or sets the actual string format being used.
        /// </summary>
        public string ActualStringFormat { get; protected set; }

        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        public string ToolTip { get; set; }

        /// <summary>
        ///   Gets the actual title (including Unit if Unit is set).
        /// </summary>
        /// <value>The actual title.</value>
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
        ///   Gets or sets the angle for the axis values.
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        ///   Gets or sets the distance from axis tick to number label.
        /// </summary>
        /// <value>The axis tick to label distance.</value>
        public double AxisTickToLabelDistance { get; set; }

        /// <summary>
        ///   Gets or sets the distance from axis number to axis title.
        /// </summary>
        /// <value>The axis title distance.</value>
        public double AxisTitleDistance { get; set; }

        /// <summary>
        ///   Gets or sets the color of the axis line.
        /// </summary>
        public OxyColor AxislineColor { get; set; }

        /// <summary>
        ///   Gets or sets the axis line.
        /// </summary>
        public LineStyle AxislineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the axis line.
        /// </summary>
        public double AxislineThickness { get; set; }

        /// <summary>
        ///   Gets or sets the end position of the axis on the plot area.
        ///   This is a fraction from 0(bottom/left) to 1(top/right).
        /// </summary>
        public double EndPosition { get; set; }

        /// <summary>
        ///   Gets or sets the color of the extra gridlines.
        /// </summary>
        public OxyColor ExtraGridlineColor { get; set; }

        /// <summary>
        ///   Gets or sets the extra gridlines linestyle.
        /// </summary>
        public LineStyle ExtraGridlineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the extra gridline thickness.
        /// </summary>
        public double ExtraGridlineThickness { get; set; }

        /// <summary>
        ///   Gets or sets the values for extra gridlines.
        /// </summary>
        public double[] ExtraGridlines { get; set; }

        /// <summary>
        ///   Gets or sets the filter function.
        /// </summary>
        /// <value>The filter function.</value>
        public Func<double, bool> FilterFunction { get; set; }

        /// <summary>
        ///   Gets or sets the maximum value that can be shown using this axis.
        ///   Values greater or equal to this value will not be shown.
        /// </summary>
        /// <value>The filter max value.</value>
        public double FilterMaxValue { get; set; }

        /// <summary>
        ///   Gets or sets the minimum value that can be shown using this axis.
        ///   Values smaller or equal to this value will not be shown.
        /// </summary>
        /// <value>The filter min value.</value>
        public double FilterMinValue { get; set; }

        /// <summary>
        ///   Gets or sets the font name.
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        ///   Gets or sets the size of the font.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        ///   Gets or sets the font weight.
        /// </summary>
        public double FontWeight { get; set; }

        /// <summary>
        ///   Gets or sets the length of the interval (screen length).
        ///   The available length of the axis will be divided by this length to get the approximate number of major intervals on the axis.
        ///   The default value is 60.
        /// </summary>
        public double IntervalLength { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this axis is visible.
        /// </summary>
        public bool IsAxisVisible { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether pan is enabled.
        /// </summary>
        public bool IsPanEnabled { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this axis is reversed.
        ///   It is reversed if StartPosition>EndPosition.
        /// </summary>
        public bool IsReversed
        {
            get
            {
                return this.StartPosition > this.EndPosition;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether zoom is enabled.
        /// </summary>
        public bool IsZoomEnabled { get; set; }

        /// <summary>
        ///   Gets or sets the key of the axis.
        ///   This can be used to find an axis if you have 
        ///   defined mutiple axes in a plot.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///   Gets or sets the layer.
        /// </summary>
        /// <value>The layer.</value>
        public AxisLayer Layer { get; set; }

        /// <summary>
        ///   Gets or sets the color of the major gridline.
        /// </summary>
        public OxyColor MajorGridlineColor { get; set; }

        /// <summary>
        ///   Gets or sets the major gridline style.
        /// </summary>
        public LineStyle MajorGridlineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the major gridline thickness.
        /// </summary>
        public double MajorGridlineThickness { get; set; }

        /// <summary>
        ///   Gets or sets the major step.
        ///   (the interval between large ticks with numbers).
        /// </summary>
        public double MajorStep { get; set; }

        /// <summary>
        ///   Gets or sets the size of the major tick.
        /// </summary>
        public double MajorTickSize { get; set; }

        /// <summary>
        ///   Gets or sets the maximum value of the axis.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        ///   Gets or sets the 'padding' fraction of the maximum value.
        ///   A value of 0.01 gives 1% more space on the maximum end of the axis.
        ///   This property is not used if the Maximum property is set.
        /// </summary>
        public double MaximumPadding { get; set; }

        /// <summary>
        ///   Gets or sets the minimum value of the axis.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        ///   Gets or sets the 'padding' fraction of the minimum value.
        ///   A value of 0.01 gives 1% more space on the minimum end of the axis.
        ///   This property is not used if the Minimum property is set.
        /// </summary>
        public double MinimumPadding { get; set; }

        /// <summary>
        ///   Gets or sets the minimum range of the axis.
        ///   Setting this property ensures that ActualMaximum-ActualMinimum > MinimumRange.
        /// </summary>
        public double MinimumRange { get; set; }

        /// <summary>
        ///   Gets or sets the color of the minor gridline.
        /// </summary>
        public OxyColor MinorGridlineColor { get; set; }

        /// <summary>
        ///   Gets or sets the minor gridline style.
        /// </summary>
        public LineStyle MinorGridlineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the minor gridline thickness.
        /// </summary>
        public double MinorGridlineThickness { get; set; }

        /// <summary>
        ///   Gets or sets the minor step 
        ///   (the interval between small ticks without number).
        /// </summary>
        public double MinorStep { get; set; }

        /// <summary>
        ///   Gets or sets the size of the minor tick.
        /// </summary>
        public double MinorTickSize { get; set; }

        /// <summary>
        ///   Gets or sets the offset.
        ///   This is used to transform between data and screen coordinates.
        /// </summary>
        public double Offset
        {
            get
            {
                return this.offset;
            }

            protected set
            {
                this.offset = value;
            }
        }

        /// <summary>
        ///   Gets the parent plot model.
        /// </summary>
        public PlotModel PlotModel { get; internal set; }

        /// <summary>
        ///   Gets or sets the position of the axis.
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
        ///   Gets or sets a value indicating whether the axis should
        ///   be positioned on the zero-crossing of the related axis.
        /// </summary>
        public bool PositionAtZeroCrossing { get; set; }

        /// <summary>
        ///   Gets or sets the related axis.
        ///   This is used for polar coordinate systems where
        ///   the angle and magnitude axes are related.
        /// </summary>
        public AxisBase RelatedAxis { get; set; }

        /// <summary>
        ///   Gets or sets the scaling factor of the axis.
        ///   This is used to transform between data and screen coordinates.
        /// </summary>
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
        ///   Gets or sets the screen coordinate of the Maximum point on the axis.
        /// </summary>
        public ScreenPoint ScreenMax { get; protected set; }

        /// <summary>
        ///   Gets or sets the screen coordinate of the Minimum point on the axis.
        /// </summary>
        public ScreenPoint ScreenMin { get; protected set; }

        /// <summary>
        ///   Gets or sets a value indicating whether minor ticks should be shown.
        /// </summary>
        public bool ShowMinorTicks { get; set; }

        /// <summary>
        ///   Gets or sets the start position of the axis on the plot area.
        ///   This is a fraction from 0(bottom/left) to 1(top/right).
        /// </summary>
        public double StartPosition { get; set; }

        /// <summary>
        ///   Gets or sets the string format used
        ///   for formatting the axis values.
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        ///   Gets or sets the tick style (both for major and minor ticks).
        /// </summary>
        public TickStyle TickStyle { get; set; }

        /// <summary>
        ///   Gets or sets the color of the ticks (both major and minor ticks).
        /// </summary>
        public OxyColor TicklineColor { get; set; }

        /// <summary>
        ///   Gets or sets the title of the axis.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets the format string used for formatting the title and unit when unit is defined.
        ///   If unit is null, only Title is used.
        ///   The default value is "{0} [{1}]", where {0} uses the Title and {1} uses the Unit.
        /// </summary>
        public string TitleFormatString { get; set; }

        /// <summary>
        ///   Gets or sets the position of the title (0.5 is in the middle).
        /// </summary>
        public double TitlePosition { get; set; }

        /// <summary>
        ///   Gets or sets the unit of the axis.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use superscript exponential format.
        ///   This format will convert 1.5E+03 to 1.5·10^{3} and render the superscript properly
        ///   If StringFormat is null, 1.0E+03 will be converted to 10^{3}
        /// </summary>
        public bool UseSuperExponentialFormat { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        public abstract string FormatValue(double x);

        /// <summary>
        /// Formats the value to be used by the tracker.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        public abstract string FormatValueForTracker(double x);

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
        public abstract object GetValue(double x);

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
        public abstract double InverseTransform(double sx);

        /// <summary>
        /// Inverse transform the specified screen point.
        /// </summary>
        /// <param name="sx">
        /// The x coordinate.
        /// </param>
        /// <param name="sy">
        /// The y coordinate.
        /// </param>
        /// <param name="yaxis">
        /// The y-axis.
        /// </param>
        /// <returns>
        /// The data point.
        /// </returns>
        public abstract DataPoint InverseTransform(double sx, double sy, IAxis yaxis);

        /// <summary>
        /// Determines whether this axis is horizontal.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this axis is horizontal; otherwise, <c>false</c>.
        /// </returns>
        public bool IsHorizontal()
        {
            return this.position == AxisPosition.Top || this.position == AxisPosition.Bottom;
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
        public abstract bool IsValidValue(double value);

        /// <summary>
        /// Determines whether this axis is vertical.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this axis is vertical; otherwise, <c>false</c>.
        /// </returns>
        public bool IsVertical()
        {
            return this.position == AxisPosition.Left || this.position == AxisPosition.Right;
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
        public abstract OxySize Measure(IRenderContext rc);

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="ppt">
        /// The previous point (screen coordinates).
        /// </param>
        /// <param name="cpt">
        /// The current point (screen coordinates).
        /// </param>
        public abstract void Pan(ScreenPoint ppt, ScreenPoint cpt);

        /// <summary>
        /// Pans the axis.
        /// </summary>
        /// <param name="delta">
        /// The delta (screen coordinates).
        /// </param>
        public abstract void Pan(double delta);

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
        public abstract void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer);

        /// <summary>
        /// Resets the user's modification (zooming/panning) to minmum and maximum of this axis.
        ///   This method will not refresh the plot.
        /// </summary>
        public abstract void Reset();

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
        public abstract double Transform(double x);

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
        public abstract ScreenPoint Transform(double x, double y, IAxis yaxis);

        /// <summary>
        /// Sets the scaling factor.
        /// </summary>
        /// <param name="scale">
        /// The new scale.
        /// </param>
        public abstract void Zoom(double scale);

        /// <summary>
        /// Zooms the axis to the range [x0,x1].
        /// </summary>
        /// <param name="x0">
        /// The new minimum.
        /// </param>
        /// <param name="x1">
        /// The new maximum.
        /// </param>
        public abstract void Zoom(double x0, double x1);

        /// <summary>
        /// Zooms the axis at the specified coordinate.
        /// </summary>
        /// <param name="factor">
        /// The zoom factor.
        /// </param>
        /// <param name="x">
        /// The coordinate to zoom at.
        /// </param>
        public abstract void ZoomAt(double factor, double x);

        #endregion

        #region Methods

        /// <summary>
        /// Modifies the range of the axis [ActualMinimum,ActualMaximum] to includes the specified value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        internal virtual void Include(double value)
        {
            if (!this.IsValidValue(value))
            {
                return;
            }

            this.ActualMinimum = double.IsNaN(this.ActualMinimum) ? value : Math.Min(this.ActualMinimum, value);
            this.ActualMaximum = double.IsNaN(this.ActualMaximum) ? value : Math.Max(this.ActualMaximum, value);
        }

        /// <summary>
        /// The post inverse transform.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The post inverse transform.
        /// </returns>
        internal virtual double PostInverseTransform(double x)
        {
            return x;
        }

        /// <summary>
        /// "Pretransform" the value.
        ///   This is used in logarithmic axis.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The pretransformed value.
        /// </returns>
        internal virtual double PreTransform(double x)
        {
            return x;
        }

        /// <summary>
        /// Resets the actual maximum and minimum.
        ///   This method will not refresh the plot.
        /// </summary>
        internal virtual void ResetActualMaxMin()
        {
            this.ActualMaximum = this.ActualMinimum = double.NaN;
        }

        /// <summary>
        /// Updates the actual maximum and minimum values.
        ///   If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum values will be used.
        ///   If Maximum or Minimum have been set, these values will be used.
        ///   Otherwise the maximum and minimum values of the series will be used, including the 'padding'.
        /// </summary>
        internal virtual void UpdateActualMaxMin()
        {
        }

        /// <summary>
        /// Updates the axis with information from the plot series.
        ///   This is used by the category axis that need to know the number of series using the axis.
        /// </summary>
        /// <param name="series">
        /// The series collection.
        /// </param>
        internal virtual void UpdateFromSeries(IEnumerable<Series> series)
        {
        }

        /// <summary>
        /// Updates the actual minor and major step intervals.
        /// </summary>
        /// <param name="plotArea">
        /// The plot area rectangle.
        /// </param>
        internal virtual void UpdateIntervals(OxyRect plotArea)
        {
        }

        /// <summary>
        /// Updates the scale and offset properties of the transform
        ///   from the specified boundary rectangle.
        /// </summary>
        /// <param name="plotArea">
        /// The plot area rectangle.
        /// </param>
        internal virtual void UpdateTransform(OxyRect plotArea)
        {
        }

        /// <summary>
        /// Raises the <see cref="E:AxisChanged"/> event.
        /// </summary>
        /// <param name="args">
        /// The <see cref="OxyPlot.AxisChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnAxisChanged(AxisChangedEventArgs args)
        {
            this.UpdateActualMaxMin();

            EventHandler<AxisChangedEventArgs> handler = this.AxisChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        #endregion
    }
}
