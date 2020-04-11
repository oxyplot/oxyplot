namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Axes;

    /// <summary>
    /// Represents a series that can be bound to a collection of <see cref="RectangleItem"/>.
    /// </summary>
    public class RectangleSeries : XYAxisSeries
    {
        /// <summary>
        /// The items originating from the items source.
        /// </summary>
        private List<RectangleItem> actualItems;

        /// <summary>
        /// Specifies if the <see cref="actualItems" /> list can be modified.
        /// </summary>
        private bool ownsActualItems;

        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}\n{5}: {6}";

        /// <summary>
        /// The default color-axis title
        /// </summary>
        private const string DefaultColorAxisTitle = "Value";

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatMapSeries" /> class.
        /// </summary>
        public RectangleSeries()
        {
            this.TrackerFormatString = DefaultTrackerFormatString;
            this.LabelFormatString = "0.00";
            this.LabelFontSize = 0;
        }

        /// <summary>
        /// Gets the minimum value of the dataset.
        /// </summary>
        public double MinValue { get; private set; }

        /// <summary>
        /// Gets the maximum value of the dataset.
        /// </summary>
        public double MaxValue { get; private set; }

        /// <summary>
        /// Gets or sets the color axis.
        /// </summary>
        /// <value>The color axis.</value>
        public IColorAxis ColorAxis { get; protected set; }

        /// <summary>
        /// Gets or sets the color axis key.
        /// </summary>
        /// <value>The color axis key.</value>
        public string ColorAxisKey { get; set; }

        /// <summary>
        /// Gets or sets the format string for the cell labels. The default value is <c>0.00</c>.
        /// </summary>
        /// <value>The format string.</value>
        /// <remarks>The label format string is only used when <see cref="LabelFontSize" /> is greater than 0.</remarks>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the font size of the labels. The default value is <c>0</c> (labels not visible).
        /// </summary>
        /// <value>The font size relative to the cell height.</value>
        public double LabelFontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to map from <see cref="ItemsSeries.ItemsSource" /> to <see cref="RectangleItem" />. The default is <c>null</c>.
        /// </summary>
        /// <value>The mapping.</value>
        /// <remarks>Example: series1.Mapping = item => new RectangleItem(new DataPoint((MyType)item).Time1, ((MyType)item).Value1), new DataPoint((MyType)item).Time2, ((MyType)item).Value2));</remarks>
        public Func<object, RectangleItem> Mapping { get; set; }

        /// <summary>
        /// Gets the list of rectangles.
        /// </summary>
        /// <value>A list of <see cref="RectangleItem" />. This list is used if <see cref="ItemsSeries.ItemsSource" /> is not set.</value>
        public List<RectangleItem> Items { get; } = new List<RectangleItem>();

        /// <summary>
        /// Gets the list of rectangles that should be rendered.
        /// </summary>
        /// <value>A list of <see cref="RectangleItem" />.</value>
        protected List<RectangleItem> ActualItems => this.ItemsSource != null ? this.actualItems : this.Items;

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualRects = this.ActualItems;

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            this.RenderRectangles(rc, clippingRect, actualRects);

            rc.ResetClip();
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
        /// Clears or creates the <see cref="actualItems"/> list.
        /// </summary>
        private void ClearActualItems()
        {
            if (!this.ownsActualItems || this.actualItems == null)
            {
                this.actualItems = new List<RectangleItem>();
            }
            else
            {
                this.actualItems.Clear();
            }

            this.ownsActualItems = true;
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

            var sourceAsListOfDataRects = this.ItemsSource as List<RectangleItem>;
            if (sourceAsListOfDataRects != null)
            {
                this.actualItems = sourceAsListOfDataRects;
                this.ownsActualItems = false;
                return;
            }

            this.ClearActualItems();

            var sourceAsEnumerableDataRects = this.ItemsSource as IEnumerable<RectangleItem>;
            if (sourceAsEnumerableDataRects != null)
            {
                this.actualItems.AddRange(sourceAsEnumerableDataRects);
            }
        }

        /// <summary>
        /// Renders the points as line, broken line and markers.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="items">The Items to render.</param>
        protected void RenderRectangles(IRenderContext rc, OxyRect clippingRect, ICollection<RectangleItem> items)
        {
            foreach (var item in items)
            {
                var rectcolor = this.ColorAxis.GetColor(item.Value);

                // transform the data points to screen points
                var p1 = this.Transform(item.A.X, item.A.Y);
                var p2 = this.Transform(item.B.X, item.B.Y);

                var rectrect = new OxyRect(p1, p2);

                rc.DrawClippedRectangle(
                    clippingRect, 
                    rectrect, 
                    rectcolor, 
                    OxyColors.Undefined,
                    0, 
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));

                if (this.LabelFontSize > 0)
                {
                    rc.DrawClippedText(
                        clippingRect, 
                        rectrect.Center, 
                        item.Value.ToString(this.LabelFormatString), 
                        this.ActualTextColor, 
                        this.ActualFont, 
                        this.LabelFontSize, 
                        this.ActualFontWeight, 
                        0, 
                        HorizontalAlignment.Center, 
                        VerticalAlignment.Middle);
                }
            }
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

            var colorAxis = this.ColorAxis as Axis;
            var colorAxisTitle = colorAxis?.Title ?? DefaultColorAxisTitle;

            if (this.ActualItems != null)
            {
                // iterate through the DataRects and return the first one that contains the point
                foreach (var item in this.ActualItems)
                {
                    if (item.Contains(p))
                    {
                        return new TrackerHitResult
                        {
                            Series = this,
                            DataPoint = p,
                            Position = point,
                            Item = null,
                            Index = -1,
                            Text = StringHelper.Format(
                            this.ActualCulture,
                            this.TrackerFormatString,
                            item,
                            this.Title,
                            this.XAxis.Title ?? DefaultXAxisTitle,
                            this.XAxis.GetValue(p.X),
                            this.YAxis.Title ?? DefaultYAxisTitle,
                            this.YAxis.GetValue(p.Y),
                            colorAxisTitle,
                            item.Value)
                        };
                    }
                }
            }
            // if no DataRects contain the point, return null
            return null;
        }

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal override void EnsureAxes()
        {
            base.EnsureAxes();

            this.ColorAxis = this.ColorAxisKey != null ?
                             this.PlotModel.GetAxis(this.ColorAxisKey) as IColorAxis :
                             this.PlotModel.DefaultColorAxis as IColorAxis;
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series for the x and y dimensions only.
        /// </summary>
        protected internal void UpdateMaxMinXY()
        {
            if (this.ActualItems != null && this.ActualItems.Count > 0)
            {
                this.MinX = Math.Min(this.ActualItems.Min(r => r.A.X), this.ActualItems.Min(r => r.B.X));
                this.MaxX = Math.Max(this.ActualItems.Max(r => r.A.X), this.ActualItems.Max(r => r.B.X));
                this.MinY = Math.Min(this.ActualItems.Min(r => r.A.Y), this.ActualItems.Min(r => r.B.Y));
                this.MaxY = Math.Max(this.ActualItems.Max(r => r.A.Y), this.ActualItems.Max(r => r.B.Y));
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            var allDataPoints = new List<DataPoint>();
            allDataPoints.AddRange(this.ActualItems.Select(rect => rect.A));
            allDataPoints.AddRange(this.ActualItems.Select(rect => rect.B));
            // var allDataPoints = this.ActualItems.Select(rect => rect.A).Concat(this.ActualItems.Select(rect => rect.B));
            this.InternalUpdateMaxMin(allDataPoints);

            this.UpdateMaxMinXY();

            if (this.ActualItems != null && this.ActualItems.Count > 0)
            {
                this.MinValue = this.ActualItems.Min(r => r.Value);
                this.MaxValue = this.ActualItems.Max(r => r.Value);
            }
        }

        /// <summary>
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            base.UpdateAxisMaxMin();
            var colorAxis = this.ColorAxis as Axis;
            if (colorAxis != null)
            {
                colorAxis.Include(this.MinValue);
                colorAxis.Include(this.MaxValue);
            }
        }

        /// <summary>
        /// Tests if a <see cref="DataPoint" /> is inside the heat map
        /// </summary>
        /// <param name="p">The <see cref="DataPoint" /> to test.</param>
        /// <returns><c>True</c> if the point is inside the heat map.</returns>
        private bool IsPointInRange(DataPoint p)
        {
            this.UpdateMaxMinXY();

            return p.X >= this.MinX && p.X <= this.MaxX && p.Y >= this.MinY && p.Y <= this.MaxY;
        }
    }
}
