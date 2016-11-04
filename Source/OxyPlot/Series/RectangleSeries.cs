namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Axes;

    /// <summary>
    /// Represents a series that can be bound to a collection of <see cref="DataRect"/>.
    /// </summary>
    public class RectangleSeries : DataRectSeries
    {
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
        /// Gets or sets the x-coordinate of the elements at index [0,*] in the data set.
        /// </summary>
        /// <value>
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Center"/>, the value defines the mid point of the element at index [0,*] in the data set.
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Edge"/>, the value defines the coordinate of the left edge of the element at index [0,*] in the data set.
        /// </value>
        public double X0 { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the mid point for the elements at index [m-1,*] in the data set.
        /// </summary>
        /// <value>
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Center"/>, the value defines the mid point of the element at index [m-1,*] in the data set.
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Edge"/>, the value defines the coordinate of the right edge of the element at index [m-1,*] in the data set.
        /// </value>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the mid point for the elements at index [*,0] in the data set.
        /// </summary>
        /// <value>
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Center"/>, the value defines the mid point of the element at index [*,0] in the data set.
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Edge"/>, the value defines the coordinate of the bottom edge of the element at index [*,0] in the data set.
        /// </value>
        public double Y0 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the mid point for the elements at index [*,n-1] in the data set.
        /// </summary>
        /// <value>
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Center"/>, the value defines the mid point of the element at index [*,n-1] in the data set.
        /// If <see cref="CoordinateDefinition" /> equals <see cref="HeatMapCoordinateDefinition.Edge"/>, the value defines the coordinate of the top edge of the element at index [*,n-1] in the data set.
        /// </value>
        public double Y1 { get; set; }

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
        /// Gets or sets the coordinate definition. The default value is <see cref="HeatMapCoordinateDefinition.Center" />.
        /// </summary>
        /// <value>The coordinate definition.</value>
        public HeatMapCoordinateDefinition CoordinateDefinition { get; set; }

        /// <summary>
        /// Gets or sets the render method. The default value is <see cref="HeatMapRenderMethod.Bitmap" />.
        /// </summary>
        /// <value>The render method.</value>
        public HeatMapRenderMethod RenderMethod { get; set; }

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
        /// Transforms data space coordinates to orientated screen space coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The transformed point.</returns>
        public new ScreenPoint Transform(double x, double y)
        {
            return this.Orientate(base.Transform(x, y));
        }

        /// <summary>
        /// Transforms data space coordinates to orientated screen space coordinates.
        /// </summary>
        /// <param name="point">The point to transform.</param>
        /// <returns>The transformed point.</returns>
        public new ScreenPoint Transform(DataPoint point)
        {
            return this.Orientate(base.Transform(point));
        }

        /// <summary>
        /// Transforms orientated screen space coordinates to data space coordinates.
        /// </summary>
        /// <param name="point">The point to inverse transform.</param>
        /// <returns>The inverse transformed point.</returns>
        public new DataPoint InverseTransform(ScreenPoint point)
        {
            return base.InverseTransform(this.Orientate(point));
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualRects = this.ActualRects;

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            this.RenderRects(rc, clippingRect, actualRects);

            rc.ResetClip();
        }

        /// <summary>
        /// Renders the points as line, broken line and markers.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="rects">The rects to render.</param>
        protected void RenderRects(IRenderContext rc, OxyRect clippingRect, ICollection<DataRect> rects)
        {
            foreach (DataRect dataRect in rects)
            {
                var rectcolor = this.ColorAxis.GetColor(dataRect.value);

                // transform the data points to screen points
                var s00 = this.Transform(dataRect.A.X, dataRect.A.Y);
                var s11 = this.Transform(dataRect.B.X, dataRect.B.Y);

                var pointa = this.Orientate(new ScreenPoint(s00.X, s00.Y)); // re-orientate
                var pointb = this.Orientate(new ScreenPoint(s11.X, s11.Y)); // re-orientate
                var rectrect = new OxyRect(pointa, pointb);

                rc.DrawClippedRectangle(clippingRect, rectrect, rectcolor, OxyColors.Undefined, 0);
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
            var colorAxisTitle = (colorAxis != null ? colorAxis.Title : null) ?? DefaultColorAxisTitle;

            if (this.ActualRects != null)
            {
                // iterate through the DataRects and return the first one that contains the point
                foreach (DataRect dataRect in this.ActualRects)
                {
                    if (dataRect.Contains(p))
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
                            null,
                            this.Title,
                            this.XAxis.Title ?? DefaultXAxisTitle,
                            this.XAxis.GetValue(p.X),
                            this.YAxis.Title ?? DefaultYAxisTitle,
                            this.YAxis.GetValue(p.Y),
                            colorAxisTitle,
                            dataRect.Value)
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

            this.ColorAxis =
                this.PlotModel.GetAxisOrDefault(this.ColorAxisKey, (Axis)this.PlotModel.DefaultColorAxis) as IColorAxis;
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series for the x and y dimensions only.
        /// </summary>
        protected internal void UpdateMaxMinXY()
        {
            if (this.ActualRects != null && this.ActualRects.Count > 0)
            {
                this.MinX = Math.Min(this.ActualRects.Min(r => r.A.X), this.ActualRects.Min(r => r.B.X));
                this.MaxX = Math.Max(this.ActualRects.Max(r => r.A.X), this.ActualRects.Max(r => r.B.X));
                this.MinY = Math.Min(this.ActualRects.Min(r => r.A.Y), this.ActualRects.Min(r => r.B.Y));
                this.MaxY = Math.Max(this.ActualRects.Max(r => r.A.Y), this.ActualRects.Max(r => r.B.Y));
                return;
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            this.UpdateMaxMinXY();

            if (this.ActualRects != null && this.ActualRects.Count > 0)
            {
                this.MinValue = this.ActualRects.Min(r => r.Value);
                this.MaxValue = this.ActualRects.Max(r => r.Value);
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
        /// Gets the label for the specified cell.
        /// </summary>
        /// <param name="v">The value of the cell.</param>
        /// <param name="i">The first index.</param>
        /// <param name="j">The second index.</param>
        /// <returns>The label string.</returns>
        protected virtual string GetLabel(double v, int i, int j)
        {
            return v.ToString(this.LabelFormatString, this.ActualCulture);
        }

        /// <summary>
        /// Transposes the ScreenPoint if the X axis is vertically orientated
        /// </summary>
        /// <param name="point">The <see cref="ScreenPoint" /> to orientate.</param>
        /// <returns>The oriented point.</returns>
        private ScreenPoint Orientate(ScreenPoint point)
        {
            if (this.XAxis.IsVertical())
            {
                point = new ScreenPoint(point.Y, point.X);
            }

            return point;
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