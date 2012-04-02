// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The BarSeriesBase provides common properties and methods for the BarSeries and ColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The BarSeriesBase provides common properties and methods for the BarSeries and ColumnSeries.
    /// </summary>
    public abstract class BarSeriesBase : XYAxisSeries
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesBase"/> class. 
        ///   Initializes a new instance of the <see cref="BarSeriesBase"/> class.
        /// </summary>
        protected BarSeriesBase()
        {
            this.InternalValues = new List<double>();
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 0;
            this.BarWidth = 0.5;
            this.TrackerFormatString = "{0}, {1}: {2}";
            this.LabelMargin = 2;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the width of the bars (as a fraction of the available width). The default value is 0.5 (50%)
        /// </summary>
        /// <value> The width of the bars. </value>
        public double BarWidth { get; set; }

        /// <summary>
        ///   Gets or sets the base value.
        /// </summary>
        /// <value> The base value. </value>
        public double BaseValue { get; set; }

        /// <summary>
        ///   Gets or sets the color of the interior of the bars.
        /// </summary>
        /// <value> The color. </value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this bar series is stacked.
        /// </summary>
        public bool IsStacked { get; set; }

        /// <summary>
        ///   Gets or sets the label format string.
        /// </summary>
        /// <value> The label format string. </value>
        public string LabelFormatString { get; set; }

        /// <summary>
        ///   Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        ///   Gets or sets label placements.
        /// </summary>
        public LabelPlacement LabelPlacement { get; set; }

        /// <summary>
        ///   Gets or sets the color of the interior of the bars when the value is negative.
        /// </summary>
        /// <value> The color. </value>
        public OxyColor NegativeFillColor { get; set; }

        /// <summary>
        ///   Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value> The color of the stroke. </value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value> The stroke thickness. </value>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the value field.
        /// </summary>
        public string ValueField { get; set; }

        /// <summary>
        ///   Gets or sets the values.
        /// </summary>
        /// <value> The values. </value>
        public IList<double> Values
        {
            get
            {
                return this.InternalValues;
            }

            set
            {
                this.InternalValues = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the values for the bars of this BarSeries.
        /// </summary>
        internal IList<double> InternalValues { get; private set; }

        /// <summary>
        ///   Gets or sets the actual rectangles for the bars.
        /// </summary>
        protected IList<OxyRect> ActualBarRectangles { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <param name="interpolate">
        /// interpolate if set to <c>true</c> . 
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit. 
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            int i = 0;
            foreach (var r in this.ActualBarRectangles)
            {
                if (point.X >= r.Left && point.X <= r.Right && point.Y >= r.Top && point.Y <= r.Bottom)
                {
                    ScreenPoint sp = point; // new ScreenPoint((r.Left + r.Right) / 2, r.Top);
                    var dp = new DataPoint(i, this.InternalValues[i]);
                    CategoryAxis categoryAxis = this.GetCategoryAxis();
                    string text = string.Format(
                        this.TrackerFormatString,
                        this.Title,
                        categoryAxis.FormatValueForTracker(i),
                        this.InternalValues[i]);
                    return new TrackerHitResult(this, dp, sp, this.GetItem(i), text);
                }

                i++;
            }

            return null;
        }

        /// <summary>
        /// Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context. 
        /// </param>
        /// <param name="legendBox">
        /// The legend rectangle. 
        /// </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            double height = (legendBox.Bottom - legendBox.Top) * 0.8;
            double width = height;
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.FillColor,
                this.StrokeColor,
                this.StrokeThickness);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">
        /// An axis. 
        /// </param>
        /// <returns>
        /// True if the axis is in use. 
        /// </returns>
        protected internal override bool IsUsing(Axis axis)
        {
            return this.XAxis == axis || this.YAxis == axis;
        }

        /// <summary>
        /// The set default values.
        /// </summary>
        /// <param name="model">
        /// The model. 
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.FillColor == null)
            {
                this.FillColor = model.GetDefaultColor();
            }
        }

        /// <summary>
        /// The update axis max min.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            var valueAxis = this.GetValueAxis();
            if (valueAxis.IsVertical())
            {
                valueAxis.Include(this.MinY);
                valueAxis.Include(this.MaxY);
            }
            else
            {
                valueAxis.Include(this.MinX);
                valueAxis.Include(this.MaxX);
            }
        }

        /// <summary>
        /// The update data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.InternalValues.Clear();
            ReflectionHelper.FillValues(this.ItemsSource, this.ValueField, this.InternalValues);
        }

        /// <summary>
        /// Updates the maximum/minimum value on the value axis from the bar values.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (this.InternalValues == null || this.InternalValues.Count == 0)
            {
                return;
            }

            CategoryAxis ca = this.GetCategoryAxis();
            var va = this.GetValueAxis();

            double minValue = this.BaseValue + this.InternalValues[0];
            double maxValue = minValue;

            int i = 0;
            foreach (double v in this.Values)
            {
                double baseValue = this.BaseValue;
                if (ca.PositiveBaseValue != null && i < ca.PositiveBaseValue.Length
                    && !double.IsNaN(ca.PositiveBaseValue[i]))
                {
                    baseValue = ca.PositiveBaseValue[i];
                }

                if (double.IsNaN(ca.MaxValue[i]))
                {
                    ca.MaxValue[i] = baseValue;
                }

                if (double.IsNaN(ca.MinValue[i]))
                {
                    ca.MinValue[i] = baseValue;
                }

                if (this.IsStacked)
                {
                    // Add to the max/min value on the category axis for stacked bars
                    ca.MaxValue[i] = Math.Max(ca.MaxValue[i], ca.MaxValue[i] + v);
                    ca.MinValue[i] = Math.Min(ca.MinValue[i], ca.MinValue[i] + v);
                    minValue = Math.Min(minValue, ca.MinValue[i]);
                    maxValue = Math.Max(maxValue, ca.MaxValue[i]);
                }
                else
                {
                    minValue = Math.Min(minValue, baseValue);
                    maxValue = Math.Max(maxValue, baseValue);

                    minValue = Math.Min(minValue, v);
                    maxValue = Math.Max(maxValue, v);
                }

                i++;
            }

            if (va.IsVertical())
            {
                this.MinY = minValue;
                this.MaxY = maxValue;
            }
            else
            {
                this.MinX = minValue;
                this.MaxX = maxValue;
            }
        }

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>
        /// The category axis. 
        /// </returns>
        protected abstract CategoryAxis GetCategoryAxis();

        /// <summary>
        /// Gets the value axis.
        /// </summary>
        /// <returns>
        /// The value axis. 
        /// </returns>
        protected abstract Axis GetValueAxis();

        /// <summary>
        /// Checks if the specified value is valid.
        /// </summary>
        /// <param name="v">
        /// The value. 
        /// </param>
        /// <param name="yaxis">
        /// The y axis. 
        /// </param>
        /// <returns>
        /// True if the value is valid. 
        /// </returns>
        protected virtual bool IsValidPoint(double v, Axis yaxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }

        #endregion
    }
}