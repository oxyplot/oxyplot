// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Placement of the labels.
    /// </summary>
    public enum LabelPlacement
    {
        /// <summary>
        /// Placed outside the bar.
        /// </summary>
        Outside,

        /// <summary>
        /// Placed inside the bar.
        /// </summary>
        Inside,

        /// <summary>
        /// Placed inside in the middle/center of the bar.
        /// </summary>
        Middle,

        /// <summary>
        /// Placed inside at the base of the bar.
        /// </summary>
        Base
    }

    /// <summary>
    /// The BarSeries is used to create clustered or stacked bar charts.
    /// </summary>
    /// <remarks>
    /// A bar chart or bar graph is a chart with rectangular bars with lengths proportional to the values that they represent. The bars can be plotted vertically or horizontally. http://en.wikipedia.org/wiki/Bar_chart The BarSeries requires a CategoryAxis. The Values collection must contain the same number of elements as the number of categories in the CategoryAxis. You can define a ItemsSource and ValueField, or add the Values manually. Use stacked bar charts with caution... http://lilt.ilstu.edu/gmklass/pos138/datadisplay/badchart.htm
    /// </remarks>
    public class BarSeries : XYAxisSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="BarSeries" /> class.
        /// </summary>
        public BarSeries()
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
        /// Gets or sets the label color.
        /// </summary>
        public OxyColor LabelColor { get; set; }

        /// <summary>
        ///   Gets or sets the label format string.
        /// </summary>
        /// <value> The label format string. </value>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        /// Gets or sets label placements.
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
        ///   Gets or sets the actual rectangles for the bars.
        /// </summary>
        internal IList<OxyRect> ActualBarRectangles { get; set; }

        /// <summary>
        ///   Gets or sets the values for the bars of this BarSeries.
        /// </summary>
        internal IList<double> InternalValues { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>
        /// The category axis. 
        /// </returns>
        public CategoryAxis GetCategoryAxis()
        {
            return this.XAxis as CategoryAxis ?? this.YAxis as CategoryAxis;
        }

        /// <summary>
        /// Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <param name="interpolate">
        /// The interpolate. 
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
        /// Gets the value axis.
        /// </summary>
        /// <returns>
        /// The value axis. 
        /// </returns>
        public Axis GetValueAxis()
        {
            return this.XAxis is CategoryAxis ? this.YAxis : this.XAxis;
        }

        /// <summary>
        /// Gets the value from the specified X.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <returns>
        /// The value. 
        /// </returns>
        public double? GetValueFromX(double x)
        {
            return null;
        }

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
        public virtual bool IsValidPoint(double v, Axis yaxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }

        /// <summary>
        /// Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context. 
        /// </param>
        /// <param name="model">
        /// The model. 
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.Values.Count == 0)
            {
                return;
            }

            OxyRect clippingRect = this.GetClippingRect();

            CategoryAxis categoryAxis = this.XAxis as CategoryAxis ?? this.YAxis as CategoryAxis;
            if (categoryAxis == null)
            {
                throw new InvalidOperationException("No category axis defined.");
            }

            bool isVertical = categoryAxis == this.XAxis;
            Axis valueAxis = isVertical ? this.YAxis : this.XAxis;
            if (valueAxis == null)
            {
                throw new InvalidOperationException("No value axis defined.");
            }

            double dx = categoryAxis.BarOffset - this.BarWidth * 0.5;

            int i = 0;
            this.ActualBarRectangles = new List<OxyRect>();

            foreach (double v in this.Values)
            {
                if (!this.IsValidPoint(v, valueAxis))
                {
                    continue;
                }

                double baseValue = double.NaN;
                if (this.IsStacked)
                {
                    baseValue = v < 0 ? categoryAxis.NegativeBaseValue[i] : categoryAxis.PositiveBaseValue[i];
                }

                if (double.IsNaN(baseValue))
                {
                    baseValue = this.BaseValue;
                }

                double topValue = this.IsStacked ? baseValue + v : v;
                int numberOfSeries = this.IsStacked ? 1 : categoryAxis.AttachedSeriesCount;
                OxyRect rect;
                if (isVertical)
                {
                    ScreenPoint p0 = this.XAxis.Transform(i + dx, baseValue, this.YAxis);
                    ScreenPoint p1 = this.XAxis.Transform(i + dx + this.BarWidth / numberOfSeries, topValue, this.YAxis);

                    p0.X = (int)p0.X;
                    p0.Y = (int)p0.Y;
                    p1.X = (int)p1.X;
                    p1.Y = (int)p1.Y;

                    if ((v >= 0 && !double.IsNaN(categoryAxis.PositiveBaseValueScreen[i]))
                        || (v < 0 && !double.IsNaN(categoryAxis.NegativeBaseValueScreen[i])))
                    {
                        if (this.IsStacked)
                        {
                            p0.Y = v < 0
                                       ? categoryAxis.NegativeBaseValueScreen[i]
                                       : categoryAxis.PositiveBaseValueScreen[i];
                        }
                        else
                        {
                            p0.X = categoryAxis.PositiveBaseValueScreen[i];
                        }
                    }

                    rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                    if (this.IsStacked)
                    {
                        if (v < 0)
                        {
                            categoryAxis.NegativeBaseValue[i] = topValue;
                            categoryAxis.NegativeBaseValueScreen[i] = p1.Y;
                        }
                        else
                        {
                            categoryAxis.PositiveBaseValue[i] = topValue;
                            categoryAxis.PositiveBaseValueScreen[i] = p1.Y;
                        }
                    }
                    else
                    {
                        categoryAxis.PositiveBaseValueScreen[i] = p1.X;
                    }
                }
                else
                {
                    ScreenPoint p0 = this.XAxis.Transform(baseValue, i + dx, this.YAxis);
                    ScreenPoint p1 = this.XAxis.Transform(topValue, i + dx + this.BarWidth / numberOfSeries, this.YAxis);

                    p0.X = (int)p0.X;
                    p0.Y = (int)p0.Y;
                    p1.X = (int)p1.X;
                    p1.Y = (int)p1.Y;

                    if ((v >= 0 && !double.IsNaN(categoryAxis.PositiveBaseValueScreen[i]))
                        || (v < 0 && !double.IsNaN(categoryAxis.NegativeBaseValueScreen[i])))
                    {
                        if (this.IsStacked)
                        {
                            p0.X = v < 0
                                       ? categoryAxis.NegativeBaseValueScreen[i]
                                       : categoryAxis.PositiveBaseValueScreen[i];
                        }
                        else
                        {
                            p0.Y = categoryAxis.PositiveBaseValueScreen[i];
                        }
                    }

                    rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                    if (this.IsStacked)
                    {
                        if (v < 0)
                        {
                            categoryAxis.NegativeBaseValue[i] = topValue;
                            categoryAxis.NegativeBaseValueScreen[i] = p1.X;
                        }
                        else
                        {
                            categoryAxis.PositiveBaseValue[i] = topValue;
                            categoryAxis.PositiveBaseValueScreen[i] = p1.X;
                        }
                    }
                    else
                    {
                        categoryAxis.PositiveBaseValueScreen[i] = p1.Y;
                    }
                }

                this.ActualBarRectangles.Add(rect);

                OxyColor actualFillColor = this.FillColor;
                if (v < 0 && this.NegativeFillColor != null)
                {
                    actualFillColor = this.NegativeFillColor;
                }

                // rc.DrawClippedRectangle(rect, clippingRect, actualFillColor, StrokeColor, StrokeThickness);
                rc.DrawClippedRectangleAsPolygon(
                    rect, clippingRect, actualFillColor, this.StrokeColor, this.StrokeThickness);

                if (this.LabelFormatString != null)
                {
                    var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, this.GetItem(i), v);
                    HorizontalTextAlign ha;
                    VerticalTextAlign va;
                    ScreenPoint pt;
                    if (isVertical)
                    {
                        ha = HorizontalTextAlign.Center;
                        va = VerticalTextAlign.Bottom;
                        switch (this.LabelPlacement)
                        {
                            case LabelPlacement.Inside:
                                pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Top + this.LabelMargin);
                                va = VerticalTextAlign.Top;
                                break;
                            case LabelPlacement.Middle:
                                pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Bottom + rect.Top) / 2);
                                va = VerticalTextAlign.Middle;
                                break;
                            case LabelPlacement.Base:
                                pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Bottom - this.LabelMargin);
                                break;
                            default: // outside
                                pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Top - this.LabelMargin);
                                break;
                        }
                    }
                    else
                    {
                        va = VerticalTextAlign.Middle;
                        switch (this.LabelPlacement)
                        {
                            case LabelPlacement.Inside:
                                pt = new ScreenPoint(rect.Right - this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                                ha = HorizontalTextAlign.Right;
                                break;
                            case LabelPlacement.Middle:
                                pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
                                ha = HorizontalTextAlign.Center;
                                break;
                            case LabelPlacement.Base:
                                pt = new ScreenPoint(rect.Left + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                                ha = HorizontalTextAlign.Left;
                                break;
                            default: // Outside
                                pt = new ScreenPoint(rect.Right + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                                ha = HorizontalTextAlign.Left;
                                break;
                        }
                    }

                    if (clippingRect.Contains(pt.X, pt.Y))
                    {
                        rc.DrawText(
                            pt,
                            s,
                            this.LabelColor ?? model.TextColor,
                            model.ActualLegendFont,
                            model.LegendFontSize,
                            FontWeights.Normal,
                            0,
                            ha,
                            va);
                    }
                }

                i++;
            }

            if (!this.IsStacked)
            {
                categoryAxis.BarOffset += this.BarWidth / categoryAxis.AttachedSeriesCount;
            }
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
                new OxyRect(xmid - 0.5 * width, ymid - 0.5 * height, width, height),
                this.FillColor,
                this.StrokeColor,
                this.StrokeThickness);
        }

        #endregion

        #region Methods

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
            CategoryAxis ca = this.XAxis as CategoryAxis ?? this.YAxis as CategoryAxis;
            if (ca == null)
            {
                throw new Exception("CategoryAxis not defined.");
            }

            bool isVertical = ca == this.XAxis;
            Axis valueAxis = isVertical ? this.YAxis : this.XAxis;
            if (isVertical)
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

            CategoryAxis ca = this.XAxis as CategoryAxis ?? this.YAxis as CategoryAxis;
            if (ca == null)
            {
                throw new Exception("CategoryAxis not defined.");
            }

            bool isVertical = ca == this.XAxis;

            Axis valueAxis = isVertical ? this.YAxis : this.XAxis;
            if (valueAxis == null)
            {
                throw new InvalidOperationException("No value axis defined.");
            }

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

            if (isVertical)
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
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">
        /// The index. 
        /// </param>
        /// <returns>
        /// The item. 
        /// </returns>
        private object GetItem(int i)
        {
            return this.GetItem(this.ItemsSource, i);
        }

        #endregion
    }
}