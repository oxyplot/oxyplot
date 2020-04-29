// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series that can be bound to a collection of HistogramItems representing bins from a Histogram.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a series that can be bound to a collection of <see cref="HistogramItem"/>.
    /// </summary>
    public class HistogramSeries : XYAxisSeries
    {
        /// <summary>
        /// The default tracker format string.
        /// </summary>
        public new const string DefaultTrackerFormatString = "Start: {5}\nEnd: {6}\nValue: {7}\nArea: {8}\nCount: {9}";

        /// <summary>
        /// The default fill color.
        /// </summary>
        private OxyColor defaultFillColor;

        /// <summary>
        /// The items originating from the items source.
        /// </summary>
        private List<HistogramItem> actualItems;

        /// <summary>
        /// Specifies if the <see cref="actualItems" /> list can be modified.
        /// </summary>
        private bool ownsActualItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramSeries" /> class.
        /// </summary>
        public HistogramSeries()
        {
            this.FillColor = OxyColors.Automatic;
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 0;
            this.TrackerFormatString = DefaultTrackerFormatString;
            this.LabelFormatString = null;
            this.LabelPlacement = LabelPlacement.Outside;
            this.ColorMapping = this.GetDefaultColor;
        }

        /// <summary>
        /// Gets or sets the color of the interior of the bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        /// Gets the actual fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualFillColor => this.FillColor.GetActualColor(this.defaultFillColor);

        /// <summary>
        /// Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets the minimum value of the dataset.
        /// </summary>
        public double MinValue { get; private set; }

        /// <summary>
        /// Gets the maximum value of the dataset.
        /// </summary>
        public double MaxValue { get; private set; }

        /// <summary>
        /// Gets or sets the format string for the cell labels. The default value is <c>0.00</c>.
        /// </summary>
        /// <value>The format string.</value>
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
        /// Gets or sets the delegate used to map from histogram item to color.
        /// </summary>
        public Func<HistogramItem, OxyColor> ColorMapping { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to map from <see cref="ItemsSeries.ItemsSource" /> to <see cref="HistogramSeries" />. The default is <c>null</c>.
        /// </summary>
        /// <value>The mapping.</value>
        /// <remarks>Example: series1.Mapping = item => new HistogramItem((double)item.BinStart, (double)item.BinStart + item.BinWidth, (double)item.Count / totalCount, item.Count).</remarks>
        public Func<object, HistogramItem> Mapping { get; set; }

        /// <summary>
        /// Gets the list of <see cref="HistogramItem" />.
        /// </summary>
        /// <value>A list of <see cref="HistogramItem" />. This list is used if <see cref="ItemsSeries.ItemsSource" /> is not set.</value>
        public List<HistogramItem> Items { get; } = new List<HistogramItem>();

        /// <summary>
        /// Gets the list of <see cref="HistogramItem" /> that should be rendered.
        /// </summary>
        /// <value>A list of <see cref="HistogramItem" />.</value>
        protected List<HistogramItem> ActualItems => this.ItemsSource != null ? this.actualItems : this.Items;

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualBins = this.ActualItems;

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            this.RenderBins(rc, clippingRect, actualBins);

            rc.ResetClip();
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            var p = this.InverseTransform(point);

            if (!this.IsPointInRange(p))
            {
                return null;
            }

            if (this.ActualItems == null)
            {
                return null;
            }

            // iterate through the HistogramItems and return the first one that contains the point
            for (var i = 0; i < this.ActualItems.Count; i++)
            {
                var item = this.ActualItems[i];
                if (item.Contains(p))
                {
                    var itemsSourceItem = this.GetItem(i);
                    return new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = p,
                        Position = point,
                        Item = itemsSourceItem,
                        Index = i,
                        Text = StringHelper.Format(
                            this.ActualCulture,
                            this.TrackerFormatString,
                            itemsSourceItem,
                            this.Title,
                            this.XAxis.Title ?? DefaultXAxisTitle,
                            this.XAxis.GetValue(p.X),
                            this.YAxis.Title ?? DefaultYAxisTitle,
                            this.YAxis.GetValue(p.Y),
                            item.RangeStart,
                            item.RangeEnd,
                            item.Value,
                            item.Area,
                            item.Count),
                    };
                }
            }

            // if no HistogramItems contain the point, return null
            return null;
        }

        /// <summary>
        /// Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The legend rectangle.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            var xmid = (legendBox.Left + legendBox.Right) / 2;
            var ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var height = (legendBox.Bottom - legendBox.Top) * 0.8;
            var width = height;
            rc.DrawRectangle(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.GetSelectableColor(this.ActualFillColor),
                this.StrokeColor,
                this.StrokeThickness,
                this.EdgeRenderingMode);
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

            this.UpdateActualItems();
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            if (this.FillColor.IsAutomatic())
            {
                this.defaultFillColor = this.PlotModel.GetDefaultColor();
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series for the x and y dimensions only.
        /// </summary>
        protected internal void UpdateMaxMinXY()
        {
            if (this.ActualItems != null && this.ActualItems.Count > 0)
            {
                this.MinX = Math.Min(this.ActualItems.Min(r => r.RangeStart), this.ActualItems.Min(r => r.RangeEnd));
                this.MaxX = Math.Max(this.ActualItems.Max(r => r.RangeStart), this.ActualItems.Max(r => r.RangeEnd));
                this.MinY = Math.Min(this.ActualItems.Min(r => 0), this.ActualItems.Min(r => r.Height));
                this.MaxY = Math.Max(this.ActualItems.Max(r => 0), this.ActualItems.Max(r => r.Height));
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            var allDataPoints = new List<DataPoint>();
            allDataPoints.AddRange(this.ActualItems.Select(item => new DataPoint(item.RangeStart, 0.0)));
            allDataPoints.AddRange(this.ActualItems.Select(item => new DataPoint(item.RangeEnd, item.Height)));
            this.InternalUpdateMaxMin(allDataPoints);

            this.UpdateMaxMinXY();

            if (this.ActualItems != null && this.ActualItems.Count > 0)
            {
                this.MinValue = this.ActualItems.Min(r => r.Value);
                this.MaxValue = this.ActualItems.Max(r => r.Value);
            }
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected override object GetItem(int i)
        {
            var items = this.ActualItems;
            if (this.ItemsSource == null && items != null && i < items.Count)
            {
                return items[i];
            }

            return base.GetItem(i);
        }

        /// <summary>
        /// Renders the points as line, broken line and markers.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="items">The Items to render.</param>
        protected void RenderBins(IRenderContext rc, OxyRect clippingRect, ICollection<HistogramItem> items)
        {
            foreach (var item in items)
            {
                var actualFillColor = this.GetItemFillColor(item);

                // transform the data points to screen points
                var p1 = this.Transform(item.RangeStart, 0);
                var p2 = this.Transform(item.RangeEnd, item.Height);

                var rectrect = new OxyRect(p1, p2);

                rc.DrawClippedRectangle(
                    clippingRect, 
                    rectrect, 
                    actualFillColor, 
                    this.StrokeColor, 
                    this.StrokeThickness, 
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));

                if (this.LabelFormatString != null)
                {
                    this.RenderLabel(rc, clippingRect, rectrect, item);
                }
            }
        }

        /// <summary>
        /// Gets the fill color of the given <see cref="HistogramItem"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The fill color of the item.</returns>
        protected OxyColor GetItemFillColor(HistogramItem item)
        {
            return item.Color.IsAutomatic() ? this.ColorMapping(item) : item.Color;
        }

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="rect">The column rectangle.</param>
        /// <param name="item">The item.</param>
        protected void RenderLabel(IRenderContext rc, OxyRect clippingRect, OxyRect rect, HistogramItem item)
        {
            var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, item, item.Value);
            DataPoint dp;
            VerticalAlignment va;
            var ha = HorizontalAlignment.Center;

            var midX = (item.RangeStart + item.RangeEnd) / 2;
            var sign = Math.Sign(item.Value);
            var dy = sign * this.LabelMargin;

            switch (this.LabelPlacement)
            {
                case LabelPlacement.Inside:
                    dp = new DataPoint(midX, item.Value);
                    va = (VerticalAlignment)(-sign);
                    break;
                case LabelPlacement.Middle:
                    var p1 = this.InverseTransform(rect.TopLeft);
                    var p2 = this.InverseTransform(rect.BottomRight);
                    dp = new DataPoint(midX, (p1.Y + p2.Y) / 2);
                    va = VerticalAlignment.Middle;
                    break;
                case LabelPlacement.Base:
                    dp = new DataPoint(midX, 0);
                    dy = -dy;
                    va = (VerticalAlignment)sign;
                    break;
                case LabelPlacement.Outside:
                    dp = new DataPoint(midX, item.Value);
                    dy = -dy;
                    va = (VerticalAlignment)sign;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.Orientate(ref ha, ref va);
            var sp = this.Transform(dp) + this.Orientate(new ScreenVector(0, dy));

            rc.DrawClippedText(
                clippingRect,
                sp,
                s,
                this.ActualTextColor,
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                0,
                ha,
                va);
        }

        /// <summary>
        /// Tests if a <see cref="DataPoint" /> is inside the histogram.
        /// </summary>
        /// <param name="p">The <see cref="DataPoint" /> to test.</param>
        /// <returns><c>True</c> if the point is inside the heat map.</returns>
        private bool IsPointInRange(DataPoint p)
        {
            this.UpdateMaxMinXY();

            return p.X >= this.MinX && p.X <= this.MaxX && p.Y >= this.MinY && p.Y <= this.MaxY;
        }

        /// <summary>
        /// Clears or creates the <see cref="actualItems"/> list.
        /// </summary>
        private void ClearActualItems()
        {
            if (!this.ownsActualItems || this.actualItems == null)
            {
                this.actualItems = new List<HistogramItem>();
            }
            else
            {
                this.actualItems.Clear();
            }

            this.ownsActualItems = true;
        }

        /// <summary>
        /// Gets the default color for a HistogramItem.
        /// </summary>
        /// <returns>The default color.</returns>
        private OxyColor GetDefaultColor(HistogramItem item)
        {
            return this.ActualFillColor;
        }

        /// <summary>
        /// Updates the points from the <see cref="ItemsSeries.ItemsSource" />.
        /// </summary>
        private void UpdateActualItems()
        {
            // Use the Mapping property to generate the points
            if (this.Mapping != null)
            {
                this.ClearActualItems();
                foreach (var item in this.ItemsSource)
                {
                    this.actualItems.Add(this.Mapping(item));
                }

                return;
            }

            if (this.ItemsSource is List<HistogramItem> sourceAsListOfHistogramItems)
            {
                this.actualItems = sourceAsListOfHistogramItems;
                this.ownsActualItems = false;
                return;
            }

            this.ClearActualItems();

            if (this.ItemsSource is IEnumerable<HistogramItem> sourceAsEnumerableHistogramItems)
            {
                this.actualItems.AddRange(sourceAsEnumerableHistogramItems);
            }
        }
    }
}
