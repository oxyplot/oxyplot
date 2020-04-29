// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighLowSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for high-low plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a series for high-low plots.
    /// </summary>
    /// <remarks>See <a href="http://www.mathworks.com/help/toolbox/finance/highlowfts.html">link</a></remarks>
    public class HighLowSeries : XYAxisSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\nHigh: {3:0.###}\nLow: {4:0.###}\nOpen: {5:0.###}\nClose: {6:0.###}";

        /// <summary>
        /// High/low items
        /// </summary>
        private readonly List<HighLowItem> items = new List<HighLowItem>();

        /// <summary>
        /// The default color.
        /// </summary>
        private OxyColor defaultColor;

        /// <summary>
        /// Initializes a new instance of the <see cref = "HighLowSeries" /> class.
        /// </summary>
        public HighLowSeries()
        {
            this.Color = OxyColors.Automatic;
            this.TickLength = 4;
            this.StrokeThickness = 1;
            this.TrackerFormatString = DefaultTrackerFormatString;
        }

        /// <summary>
        /// Gets or sets the color of the item.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets the actual color of the item.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColor
        {
            get { return this.Color.GetActualColor(this.defaultColor); }
        }

        /// <summary>
        /// Gets or sets the dashes array.
        /// If this is not <c>null</c> it overrides the LineStyle property.
        /// </summary>
        /// <value>The dashes.</value>
        public double[] Dashes { get; set; }

        /// <summary>
        /// Gets or sets the data field for the Close value.
        /// </summary>
        public string DataFieldClose { get; set; }

        /// <summary>
        /// Gets or sets the data field for the High value.
        /// </summary>
        public string DataFieldHigh { get; set; }

        /// <summary>
        /// Gets or sets the data field for the Low value.
        /// </summary>
        public string DataFieldLow { get; set; }

        /// <summary>
        /// Gets or sets the data field for the Open value.
        /// </summary>
        public string DataFieldOpen { get; set; }

        /// <summary>
        /// Gets or sets the x data field (time).
        /// </summary>
        public string DataFieldX { get; set; }

        /// <summary>
        /// Gets the items of the series.
        /// </summary>
        /// <value>The items.</value>
        public List<HighLowItem> Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public LineJoin LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the mapping delegate.
        /// </summary>
        /// <value>The mapping.</value>
        /// <remarks>Example: series1.Mapping = item => new HighLowItem(((MyType)item).Time,((MyType)item).Value);</remarks>
        public Func<object, HighLowItem> Mapping { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the curve.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the length of the open/close ticks (screen coordinates).
        /// </summary>
        /// <value>The length of the open/close ticks.</value>
        public double TickLength { get; set; }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.XAxis == null || this.YAxis == null)
            {
                return null;
            }

            if (interpolate)
            {
                return null;
            }

            double minimumDistance = double.MaxValue;

            TrackerHitResult result = null;
            Action<DataPoint, HighLowItem, int> check = (p, item, index) =>
                {
                    var sp = this.Transform(p);
                    double dx = sp.x - point.x;
                    double dy = sp.y - point.y;
                    double d2 = (dx * dx) + (dy * dy);

                    if (d2 < minimumDistance)
                    {
                        result = new TrackerHitResult
                        {
                            Series = this,
                            DataPoint = p,
                            Position = sp,
                            Item = item,
                            Index = index,
                            Text =
                                StringHelper.Format(
                                    this.ActualCulture,
                                    this.TrackerFormatString,
                                    item,
                                    this.Title,
                                    this.XAxis.Title ?? DefaultXAxisTitle,
                                    this.XAxis.GetValue(p.X),
                                    this.YAxis.GetValue(item.High),
                                    this.YAxis.GetValue(item.Low),
                                    this.YAxis.GetValue(item.Open),
                                    this.YAxis.GetValue(item.Close))
                        };

                        minimumDistance = d2;
                    }
                };
            int i = 0;
            foreach (var item in this.items)
            {
                check(new DataPoint(item.X, item.High), item, i);
                check(new DataPoint(item.X, item.Low), item, i);
                check(new DataPoint(item.X, item.Open), item, i);
                check(new DataPoint(item.X, item.Close), item, i++);
            }

            if (minimumDistance < double.MaxValue)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Determines whether the point is valid.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <param name="xaxis">The x axis.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns><c>true</c> if the specified point is valid; otherwise, <c>false</c>.</returns>
        public virtual bool IsValidItem(HighLowItem pt, Axis xaxis, Axis yaxis)
        {
            return !double.IsNaN(pt.X) && !double.IsInfinity(pt.X) && !double.IsNaN(pt.High)
                   && !double.IsInfinity(pt.High) && !double.IsNaN(pt.Low) && !double.IsInfinity(pt.Low);
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.items.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            var dashArray = this.LineStyle.GetDashArray();
            var actualColor = this.GetSelectableColor(this.ActualColor);
            foreach (var v in this.items)
            {
                if (!this.IsValidItem(v, this.XAxis, this.YAxis))
                {
                    continue;
                }

                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    var high = this.Transform(v.X, v.High);
                    var low = this.Transform(v.X, v.Low);

                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { low, high },
                        0,
                        actualColor,
                        this.StrokeThickness,
                        this.EdgeRenderingMode,
                        dashArray,
                        this.LineJoin);

                    var tickVector = this.Orientate(new ScreenVector(this.TickLength, 0));
                    if (!double.IsNaN(v.Open))
                    {
                        var open = this.Transform(v.X, v.Open);
                        var openTick = open - tickVector;
                        rc.DrawClippedLine(
                            clippingRect,
                            new[] { open, openTick },
                            0,
                            actualColor,
                            this.StrokeThickness,
                            this.EdgeRenderingMode,
                            dashArray,
                            this.LineJoin);
                    }

                    if (!double.IsNaN(v.Close))
                    {
                        var close = this.Transform(v.X, v.Close);
                        var closeTick = close + tickVector;
                        rc.DrawClippedLine(
                            clippingRect,
                            new[] { close, closeTick },
                            0,
                            actualColor,
                            this.StrokeThickness,
                            this.EdgeRenderingMode,
                            dashArray,
                            this.LineJoin);
                    }
                }
            }
        }

        /// <summary>
        /// Renders the legend symbol for the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double yopen = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.7);
            double yclose = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.3);
            double[] dashArray = this.LineStyle.GetDashArray();
            var color = this.GetSelectableColor(this.ActualColor);

            if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
            {
                rc.DrawLine(
                    new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) },
                    color,
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    LineJoin.Miter);
                rc.DrawLine(
                    new[] { new ScreenPoint(xmid - this.TickLength, yopen), new ScreenPoint(xmid, yopen) },
                    color,
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    LineJoin.Miter);
                rc.DrawLine(
                    new[] { new ScreenPoint(xmid + this.TickLength, yclose), new ScreenPoint(xmid, yclose) },
                    color,
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    LineJoin.Miter);
            }
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            if (this.Color.IsAutomatic())
            {
                this.LineStyle = this.PlotModel.GetDefaultLineStyle();
                this.defaultColor = this.PlotModel.GetDefaultColor();
            }
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.items.Clear();

            // Use the mapping to generate the points
            if (this.Mapping != null)
            {
                foreach (var item in this.ItemsSource)
                {
                    this.items.Add(this.Mapping(item));
                }

                return;
            }

            var sequenceOfHighLowItems = this.ItemsSource as IEnumerable<HighLowItem>;
            if (sequenceOfHighLowItems != null)
            {
                this.items.AddRange(sequenceOfHighLowItems);
                return;
            }

            var filler = new ListBuilder<HighLowItem>();
            filler.Add(this.DataFieldX, double.NaN);
            filler.Add(this.DataFieldHigh, double.NaN);
            filler.Add(this.DataFieldLow, double.NaN);
            filler.Add(this.DataFieldOpen, double.NaN);
            filler.Add(this.DataFieldClose, double.NaN);
            filler.FillT(this.items, this.ItemsSource, args => new HighLowItem(Axis.ToDouble(args[0]), Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3]), Convert.ToDouble(args[4])));
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.items, i => i.X, i => i.X, i => i.Low, i => i.High);
        }
    }
}
