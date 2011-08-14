using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OxyPlot
{
    /// <summary>
    /// The BarSeries is used to create clustered or stacked bar charts.
    /// A bar chart or bar graph is a chart with rectangular bars with lengths proportional to the values that they represent. 
    /// The bars can be plotted vertically or horizontally.
    /// http://en.wikipedia.org/wiki/Bar_chart
    /// 
    /// The BarSeries requires a CategoryAxis. 
    /// The Values collection must contain the same number of elements as the number of categories in the CategoryAxis.
    /// You can define a ItemsSource and ValueField, or add the Values manually.
    /// 
    /// Use stacked bar charts with caution... http://lilt.ilstu.edu/gmklass/pos138/datadisplay/badchart.htm
    /// </summary>
    public class BarSeries : PlotSeriesBase
    {
        /// <summary>
        /// Gets or sets the width of the bar (fraction of available with).
        /// The default value is 0.5 (50%)
        /// </summary>
        /// <value>The width of the bar.</value>
        public double BarWidth { get; set; }

        /// <summary>
        ///   Gets or sets the color of the interior of the bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// The values for the bars of this BarSeries.
        /// </summary>
        internal IList<double> InternalValues;

        /// <summary>
        /// The actual rectangles for the bars.
        /// </summary>
        internal IList<OxyRect> ActualBarRectangles;

        public BarSeries()
        {
            InternalValues = new Collection<double>();
            StrokeColor = null;
            StrokeThickness = 0;
            BarWidth = 0.5;
            TrackerFormatString = "{0} {1}: {2}";
        }

        /// <summary>
        ///   Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        ///   Gets or sets the value field.
        /// </summary>
        public string ValueField { get; set; }

        /// <summary>
        ///   Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IList<double> Values
        {
            get { return InternalValues; }
            set { InternalValues = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this bar series is stacked.
        /// </summary>
        public bool IsStacked { get; set; }

        public CategoryAxis GetCategoryAxis()
        {
            return XAxis as CategoryAxis ?? YAxis as CategoryAxis;
        }

        public IAxis GetValueAxis()
        {
            return XAxis is CategoryAxis ? YAxis : XAxis;
        }

        public bool IsVertical()
        {
            return GetCategoryAxis() == XAxis;
        }

        #region ISeries Members

        /// <summary>
        ///   Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (Values.Count == 0)
            {
                return;
            }

            var clippingRect = GetClippingRect();

            var categoryAxis = XAxis as CategoryAxis ?? YAxis as CategoryAxis;
            if (categoryAxis == null)
            {
                throw new InvalidOperationException("No category axis defined.");
            }

            bool isVertical = categoryAxis == XAxis;
            var valueAxis = isVertical ? YAxis : XAxis;
            if (valueAxis == null)
            {
                throw new InvalidOperationException("No value axis defined.");
            }

            double dx = categoryAxis.BarOffset - BarWidth * 0.5;

            int i = 0;
            ActualBarRectangles = new List<OxyRect>();

            foreach (var v in Values)
            {
                if (!IsValidPoint(v, valueAxis))
                {
                    continue;
                }

                double baseValue = IsStacked ? categoryAxis.BaseValue[i] : double.NaN;
                if (double.IsNaN(baseValue))
                    baseValue = 0;

                double topValue = IsStacked ? baseValue + v : v;
                int nSeries = IsStacked ? 1 : categoryAxis.AttachedSeriesCount;
                OxyRect rect;
                if (isVertical)
                {
                    var p0 = XAxis.Transform(new DataPoint(i + dx, baseValue), YAxis);
                    var p1 = XAxis.Transform(new DataPoint(i + dx + BarWidth / nSeries, topValue), YAxis);

                    p0.X = (int)p0.X;
                    p0.Y = (int)p0.Y;
                    p1.X = (int)p1.X;
                    p1.Y = (int)p1.Y;

                    if (!double.IsNaN(categoryAxis.BaseValueScreen[i]))
                        if (IsStacked)
                            p0.Y = categoryAxis.BaseValueScreen[i];
                        else
                            p0.X = categoryAxis.BaseValueScreen[i];


                    rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                    if (IsStacked)
                    {
                        categoryAxis.BaseValue[i] = topValue;
                        categoryAxis.BaseValueScreen[i] = p1.Y;
                    }
                    else
                    {
                        categoryAxis.BaseValueScreen[i] = p1.X;
                    }
                }
                else
                {
                    var p0 = XAxis.Transform(new DataPoint(baseValue, i + dx), YAxis);
                    var p1 = XAxis.Transform(new DataPoint(topValue, i + dx + BarWidth / nSeries), YAxis);

                    p0.X = (int)p0.X;
                    p0.Y = (int)p0.Y;
                    p1.X = (int)p1.X;
                    p1.Y = (int)p1.Y;

                    if (!double.IsNaN(categoryAxis.BaseValueScreen[i]))
                        if (IsStacked)
                            p0.X = categoryAxis.BaseValueScreen[i];
                        else
                            p0.Y = categoryAxis.BaseValueScreen[i];

                    rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                    if (IsStacked)
                    {
                        categoryAxis.BaseValue[i] = topValue;
                        categoryAxis.BaseValueScreen[i] = p1.X;
                    }
                    else
                    {
                        categoryAxis.BaseValueScreen[i] = p1.Y;
                    }
                }
                ActualBarRectangles.Add(rect);

                // rc.DrawClippedRectangle(rect, clippingRect, FillColor, StrokeColor, StrokeThickness);
                rc.DrawClippedRectangleAsPolygon(rect, clippingRect, FillColor, StrokeColor, StrokeThickness);

                i++;
            }
            if (!IsStacked)
            {
                categoryAxis.BarOffset += BarWidth / categoryAxis.AttachedSeriesCount;
            }
        }

        public virtual bool IsValidPoint(double v, IAxis yAxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }

        /// <summary>
        ///   Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The rect.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double height = legendBox.Bottom - legendBox.Top;
            rc.DrawRectangleAsPolygon(new OxyRect(xmid - 0.25 * height, legendBox.Top + height * 0.1, 0.5 * height, height * 0.8), FillColor, StrokeColor, StrokeThickness);
        }

        #endregion

        public override void SetDefaultValues(PlotModel model)
        {
            if (FillColor == null)
            {
                FillColor = model.GetDefaultColor();
            }
        }

        public override void UpdateData()
        {
            if (ItemsSource == null)
                return;
            InternalValues.Clear();
            ReflectionHelper.FillValues(ItemsSource, ValueField, InternalValues);
        }

        /// <summary>
        ///   Updates the maximum/minimum value on the value axis from the bar values.
        /// </summary>
        public override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (InternalValues == null || InternalValues.Count == 0)
            {
                return;
            }

            var ca = XAxis as CategoryAxis ?? YAxis as CategoryAxis;
            if (ca == null)
                throw new Exception("CategoryAxis not defined.");

            bool isVertical = ca == XAxis;

            var valueAxis = isVertical ? YAxis : XAxis;
            if (valueAxis == null)
            {
                throw new InvalidOperationException("No value axis defined.");
            }

            double minValue = InternalValues[0];
            double maxValue = minValue;

            int i = 0;
            foreach (var v in Values)
            {
                double baseValue = 0;
                if (ca.BaseValue != null)
                {
                    baseValue = ca.BaseValue[i];
                }
                if (IsStacked)
                {
                    // Add to the max/min value on the category axis for stacked bars
                    ca.MaxValue[i] = Math.Max(ca.MaxValue[i], ca.MaxValue[i] + v);
                    ca.MinValue[i] = Math.Min(ca.MinValue[i], ca.MinValue[i] + v);
                    minValue = Math.Min(minValue, ca.MinValue[i]);
                    maxValue = Math.Max(maxValue, ca.MaxValue[i]);
                }

                minValue = Math.Min(minValue, baseValue);
                maxValue = Math.Max(maxValue, baseValue);

                minValue = Math.Min(minValue, v);
                maxValue = Math.Max(maxValue, v);
                i++;
            }

            if (isVertical)
            {
                valueAxis.Include(minValue);
                valueAxis.Include(maxValue);
                MinY = minValue;
                MaxY = maxValue;
            }
            else
            {
                valueAxis.Include(minValue);
                valueAxis.Include(maxValue);
                MinX = minValue;
                MaxX = maxValue;
            }
        }

        /// <summary>
        ///   Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            int i = 0;
            foreach (var r in ActualBarRectangles)
            {
                if (point.X >= r.Left && point.X <= r.Right && point.Y >= r.Top && point.Y <= r.Bottom)
                {
                    var sp = point; // new ScreenPoint((r.Left + r.Right) / 2, r.Top);
                    var dp = new DataPoint(i, InternalValues[i]);
                    var categoryAxis = GetCategoryAxis();
                    var text = String.Format(TrackerFormatString, Title, categoryAxis.FormatValueForTracker(i), InternalValues[i]);
                    return new TrackerHitResult(this, dp, sp, GetItem(i), text);
                }
                i++;
            }
            return null;
        }

        private object GetItem(int i)
        {
            return null;
        }

        /// <summary>
        ///   Gets the value from the specified X.
        /// </summary>
        /// <param name = "x">The x.</param>
        /// <returns></returns>
        public double? GetValueFromX(double x)
        {
            return null;
        }
    }
}