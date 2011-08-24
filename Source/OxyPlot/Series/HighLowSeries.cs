// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighLowSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Use the HighLowSeries to create time series High-Low plots.
    /// http://www.mathworks.com/help/toolbox/finance/highlowfts.html
    /// </summary>
    public class HighLowSeries : ItemsSeries
    {
        #region Constants and Fields

        /// <summary>
        /// High/low items
        /// </summary>
        protected IList<HighLowItem> items;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HighLowSeries"/> class.
        /// </summary>
        public HighLowSeries()
        {
            this.items = new List<HighLowItem>();
            this.TickLength = 4;
            this.StrokeThickness = 1;
            this.TrackerFormatString = "X: {1:0.00}\nHigh: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighLowSeries"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        public HighLowSeries(string title)
            : this()
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighLowSeries"/> class.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <param name="strokeThickness">
        /// The stroke thickness.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public HighLowSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : this()
        {
            this.Color = color;
            this.StrokeThickness = strokeThickness;
            this.Title = title;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the color of the curve.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the dashes array. 
        ///   If this is not null it overrides the LineStyle property.
        /// </summary>
        /// <value>The dashes.</value>
        public double[] Dashes { get; set; }

        /// <summary>
        ///   Gets or sets the data field for the Close value.
        /// </summary>
        public string DataFieldClose { get; set; }

        /// <summary>
        ///   Gets or sets the data field for the High value.
        /// </summary>
        public string DataFieldHigh { get; set; }

        /// <summary>
        ///   Gets or sets the data field for the Low value.
        /// </summary>
        public string DataFieldLow { get; set; }

        /// <summary>
        ///   Gets or sets the data field for the Open value.
        /// </summary>
        public string DataFieldOpen { get; set; }

        /// <summary>
        ///   Gets or sets the x data field (time).
        /// </summary>
        public string DataFieldX { get; set; }

        /// <summary>
        ///   Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        public IList<HighLowItem> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.items = value;
            }
        }

        /// <summary>
        ///   Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the mapping deleagte.
        ///   Example: series1.Mapping = item => new HighLowItem(((MyType)item).Time,((MyType)item).Value);
        /// </summary>
        /// <value>The mapping.</value>
        public Func<object, HighLowItem> Mapping { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the curve.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the length of the open/close ticks (screen coordinates).
        /// </summary>
        /// <value>The length of the open/close ticks.</value>
        public double TickLength { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolated">
        /// if set to <c>true</c> [interpolated].
        /// </param>
        /// <returns>
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolated)
        {
            if (interpolated)
            {
                return null;
            }

            double minimumDistance = double.MaxValue;
            var result = new TrackerHitResult(this, DataPoint.Undefined, ScreenPoint.Undefined, null, null);

            Action<DataPoint, HighLowItem> check = (p, item) =>
                {
                    ScreenPoint sp = AxisBase.Transform(p, this.XAxis, this.YAxis);
                    double dx = sp.x - point.x;
                    double dy = sp.y - point.y;
                    double d2 = dx * dx + dy * dy;

                    if (d2 < minimumDistance)
                    {
                        result.DataPoint = p;
                        result.Position = sp;
                        result.Item = item;
                        if (this.TrackerFormatString != null)
                        {
                            result.Text = string.Format(
                                CultureInfo.InvariantCulture, 
                                this.TrackerFormatString, 
                                this.Title, 
                                p.X, 
                                item.High, 
                                item.Low, 
                                item.Open, 
                                item.Close);
                        }

                        minimumDistance = d2;
                    }
                };
            foreach (HighLowItem item in this.items)
            {
                check(new DataPoint(item.X, item.High), item);
                check(new DataPoint(item.X, item.Low), item);
                check(new DataPoint(item.X, item.Open), item);
                check(new DataPoint(item.X, item.Close), item);
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
        /// <param name="pt">
        /// The point.
        /// </param>
        /// <param name="xAxis">
        /// The x axis.
        /// </param>
        /// <param name="yAxis">
        /// The y axis.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is valid point] [the specified pt]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsValidPoint(HighLowItem pt, IAxis xAxis, IAxis yAxis)
        {
            return !double.IsNaN(pt.X) && !double.IsInfinity(pt.X) && !double.IsNaN(pt.High)
                   && !double.IsInfinity(pt.High) && !double.IsNaN(pt.Low) && !double.IsInfinity(pt.Low);
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The owner plot model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            if (this.items.Count == 0)
            {
                return;
            }

            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis has not been defined.");

            OxyRect clippingRect = this.GetClippingRect();

            foreach (HighLowItem v in this.items)
            {
                if (!this.IsValidPoint(v, this.XAxis, this.YAxis))
                {
                    continue;
                }

                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    ScreenPoint high = this.XAxis.Transform(v.X, v.High, this.YAxis);
                    ScreenPoint low = this.XAxis.Transform(v.X, v.Low, this.YAxis);

                    rc.DrawClippedLine(
                        new[] { low, high }, 
                        clippingRect, 
                        0, 
                        this.Color, 
                        this.StrokeThickness, 
                        this.LineStyle, 
                        this.LineJoin, 
                        true);
                    if (!double.IsNaN(v.Open))
                    {
                        ScreenPoint open = this.XAxis.Transform(v.X, v.Open, this.YAxis);
                        ScreenPoint openTick = open;
                        openTick.X -= this.TickLength;
                        rc.DrawClippedLine(
                            new[] { open, openTick }, 
                            clippingRect, 
                            0, 
                            this.Color, 
                            this.StrokeThickness, 
                            this.LineStyle, 
                            this.LineJoin, 
                            true);
                    }

                    if (!double.IsNaN(v.Close))
                    {
                        ScreenPoint close = this.XAxis.Transform(v.X, v.Close, this.YAxis);
                        ScreenPoint closeTick = close;
                        closeTick.X += this.TickLength;
                        rc.DrawClippedLine(
                            new[] { close, closeTick }, 
                            clippingRect, 
                            0, 
                            this.Color, 
                            this.StrokeThickness, 
                            this.LineStyle, 
                            this.LineJoin, 
                            true);
                    }
                }
            }
        }

        /// <summary>
        /// Renders the legend symbol for the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="legendBox">
        /// The bounding rectangle of the legend box.
        /// </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double yOpen = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
            double yClose = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
            double[] dashArray = LineStyleHelper.GetDashArray(this.LineStyle);
            rc.DrawLine(
                new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) }, 
                this.Color, 
                this.StrokeThickness, 
                dashArray, 
                OxyPenLineJoin.Miter, 
                true);
            rc.DrawLine(
                new[] { new ScreenPoint(xmid - this.TickLength, yOpen), new ScreenPoint(xmid, yOpen) }, 
                this.Color, 
                this.StrokeThickness, 
                dashArray, 
                OxyPenLineJoin.Miter, 
                true);
            rc.DrawLine(
                new[] { new ScreenPoint(xmid + this.TickLength, yClose), new ScreenPoint(xmid, yClose) }, 
                this.Color, 
                this.StrokeThickness, 
                dashArray, 
                OxyPenLineJoin.Miter, 
                true);
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.Color == null)
            {
                this.LineStyle = model.GetDefaultLineStyle();
                this.Color = model.GetDefaultColor();
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
                foreach (object item in this.ItemsSource)
                {
                    this.items.Add(this.Mapping(item));
                }

                return;
            }

            // Using reflection 
            PropertyInfo piX = null;
            PropertyInfo piHigh = null;
            PropertyInfo piLow = null;
            PropertyInfo piOpen = null;
            PropertyInfo piClose = null;
            Type t = null;

            foreach (object o in this.ItemsSource)
            {
                if (piX == null || o.GetType() != t)
                {
                    t = o.GetType();
                    piX = t.GetProperty(this.DataFieldX);
                    piHigh = t.GetProperty(this.DataFieldHigh);
                    piLow = t.GetProperty(this.DataFieldLow);
                    piOpen = this.DataFieldOpen != null ? t.GetProperty(this.DataFieldOpen) : null;
                    piClose = this.DataFieldClose != null ? t.GetProperty(this.DataFieldClose) : null;
                    if (piX == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find data field {0} on type {1}", this.DataFieldX, t));
                    }

                    if (piHigh == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find data field {0} on type {1}", this.DataFieldHigh, t));
                    }

                    if (piLow == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find data field {0} on type {1}", this.DataFieldLow, t));
                    }
                }

                double x = this.ToDouble(piX.GetValue(o, null));
                double high = this.ToDouble(piHigh.GetValue(o, null));
                double low = this.ToDouble(piLow.GetValue(o, null));
                double open = piOpen != null ? this.ToDouble(piOpen.GetValue(o, null)) : double.NaN;
                double close = piClose != null ? this.ToDouble(piClose.GetValue(o, null)) : double.NaN;

                var pp = new HighLowItem(x, high, low, open, close);
                this.items.Add(pp);
            }
        }

        /// <summary>
        /// Updates the max/min values.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.items);
        }

        protected internal override void UpdateAxisMaxMin()
        {
            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);
            this.YAxis.Include(this.MinY);
            this.YAxis.Include(this.MaxY);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Updates the Max/Min limits from the specified point list.
        /// </summary>
        /// <param name="pts">
        /// The PTS.
        /// </param>
        protected void InternalUpdateMaxMin(IList<HighLowItem> pts)
        {
            if (pts == null || pts.Count == 0)
            {
                return;
            }

            double minx = this.MinX;
            double miny = this.MinY;
            double maxx = this.MaxX;
            double maxy = this.MaxY;

            foreach (HighLowItem pt in pts)
            {
                if (!this.IsValidPoint(pt, this.XAxis, this.YAxis))
                {
                    continue;
                }

                if (pt.X < minx || double.IsNaN(minx))
                {
                    minx = pt.X;
                }

                if (pt.X > maxx || double.IsNaN(maxx))
                {
                    maxx = pt.X;
                }

                if (pt.Low < miny || double.IsNaN(miny))
                {
                    miny = pt.Low;
                }

                if (pt.High > maxy || double.IsNaN(maxy))
                {
                    maxy = pt.High;
                }
            }

            this.MinX = minx;
            this.MinY = miny;
            this.MaxX = maxx;
            this.MaxY = maxy;
        }

        #endregion
    }
}