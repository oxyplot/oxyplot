// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatMapSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies how the heat map coordinates are defined.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    using OxyPlot.Axes;

    /// <summary>
    /// Specifies how the heat map coordinates are defined.
    /// </summary>
    public enum HeatMapCoordinateDefinition
    {
        /// <summary>
        /// The coordinates defines the center of the cells
        /// </summary>
        Center,

        /// <summary>
        /// The coordinates defines the edge of the cells
        /// </summary>
        Edge
    }

    /// <summary>
    /// Specifies how the heat map coordinates are defined.
    /// </summary>
    public enum HeatMapRenderMethod
    {
        /// <summary>
        /// The heat map is rendered as a bitmap
        /// </summary>
        Bitmap,

        /// <summary>
        /// The heat map is rendered as a collection of discrete rectangles
        /// </summary>
        Rectangles
    }

    /// <summary>
    /// Represents a heat map.
    /// </summary>
    public class HeatMapSeries : XYAxisSeries
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
        /// The hash code of the data when the image was updated.
        /// </summary>
        private int dataHash;

        /// <summary>
        /// The hash code of the color axis when the image was updated.
        /// </summary>
        private int colorAxisHash;

        /// <summary>
        /// The image
        /// </summary>
        private OxyImage image;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatMapSeries" /> class.
        /// </summary>
        public HeatMapSeries()
        {
            this.TrackerFormatString = DefaultTrackerFormatString;
            this.Interpolate = true;
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
        /// Gets or sets the data array.
        /// </summary>
        /// <remarks>Note that the indices of the data array refer to [x,y].
        /// The first dimension is along the x-axis.
        /// The second dimension is along the y-axis.
        /// Remember to call the <see cref="Invalidate" /> method if the contents of the <see cref="Data" /> array is changed.</remarks>
        public double[,] Data { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to interpolate when rendering. The default value is <c>true</c>.
        /// </summary>
        /// <remarks>This property is not supported on all platforms. Ignored (off) if <see cref="RenderMethod" /> is <see cref="HeatMapRenderMethod.Rectangles" />.</remarks>
        public bool Interpolate { get; set; }

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
        /// Invalidates the image that renders the heat map. The image will be regenerated the next time the <see cref="HeatMapSeries" /> is rendered.
        /// </summary>
        /// <remarks>Call <see cref="PlotModel.InvalidatePlot" /> to refresh the view.</remarks>
        public void Invalidate()
        {
            this.image = null;
        }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.Data == null)
            {
                this.image = null;
                return;
            }

            if (this.ColorAxis == null)
            {
                throw new InvalidOperationException("Color axis not specified.");
            }

            double left = this.X0;
            double right = this.X1;
            double bottom = this.Y0;
            double top = this.Y1;

            int m = this.Data.GetLength(0);
            int n = this.Data.GetLength(1);
            double dx = (this.X1 - this.X0) / (m - 1);
            double dy = (this.Y1 - this.Y0) / (n - 1);

            if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
            {
                if (this.XAxis.IsLogarithmic())
                {
                    double gx = Math.Log(this.X1 / this.X0) / (m - 1);
                    left *= Math.Exp(gx / -2);
                    right *= Math.Exp(gx / 2);
                }
                else
                {
                    left -= dx / 2;
                    right += dx / 2;
                }

                if (this.YAxis.IsLogarithmic())
                {
                    double gy = Math.Log(this.Y1 / this.Y0) / (n - 1);
                    bottom *= Math.Exp(gy / -2);
                    top *= Math.Exp(gy / 2);
                }
                else
                {
                    bottom -= dy / 2;
                    top += dy / 2;
                }
            }

            var s00 = this.Transform(left, bottom);
            var s11 = this.Transform(right, top);
            var rect = new OxyRect(s00, s11);

            bool needImage = this.RenderMethod == HeatMapRenderMethod.Bitmap;

            var currentDataHash = this.Data.GetHashCode();
            var currentColorAxisHash = this.ColorAxis.GetElementHashCode();
            if ((needImage && this.image == null) || currentDataHash != this.dataHash || currentColorAxisHash != this.colorAxisHash)
            {
                if (needImage)
                {
                    this.UpdateImage();
                }

                this.dataHash = currentDataHash;
                this.colorAxisHash = currentColorAxisHash;
            }

            var clip = this.GetClippingRect();
            if (needImage)
            {
                if (this.image != null)
                {
                    rc.DrawClippedImage(clip, this.image, rect.Left, rect.Top, rect.Width, rect.Height, 1, this.Interpolate);
                }
            }
            else
            {
                s00 = this.Orientate(s00); // disorientate
                s11 = this.Orientate(s11); // disorientate

                double sdx = (s11.X - s00.X) / m;
                double sdy = (s11.Y - s00.Y) / n;

                // draw lots of rectangles
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        var rectcolor = this.ColorAxis.GetColor(this.Data[i, j]);

                        var pointa = this.Orientate(new ScreenPoint(s00.X + (i * sdx), s00.Y + (j * sdy))); // re-orientate
                        var pointb = this.Orientate(new ScreenPoint(s00.X + ((i + 1) * sdx), s00.Y + ((j + 1) * sdy))); // re-orientate
                        var rectrect = new OxyRect(pointa, pointb);

                        rc.DrawClippedRectangle(clip, rectrect, rectcolor, OxyColors.Undefined, 0, this.EdgeRenderingMode);
                    }
                }
            }

            if (this.LabelFontSize > 0)
            {
                this.RenderLabels(rc, rect);
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
            if (!this.Interpolate)
            {
                // It makes no sense to interpolate the tracker when the plot is not interpolated.
                interpolate = false;
            }

            var p = this.InverseTransform(point);

            if (!this.IsPointInRange(p))
            {
                return null;
            }

            double i;
            double j;

            if (this.XAxis.IsLogarithmic())
            {
                double gx = Math.Log(this.X1 / this.X0) / (this.Data.GetLength(0) - 1);
                i = Math.Log(p.X / this.X0) / gx;
            }
            else
            {
                double dx = (this.X1 - this.X0) / (this.Data.GetLength(0) - 1);
                i = (p.X - this.X0) / dx;
            }

            if (this.YAxis.IsLogarithmic())
            {
                double gy = Math.Log(this.Y1 / this.Y0) / (this.Data.GetLength(1) - 1);
                j = Math.Log(p.Y / this.Y0) / gy;
            }
            else
            {
                double dy = (this.Y1 - this.Y0) / (this.Data.GetLength(1) - 1);
                j = (p.Y - this.Y0) / dy;
            }

            if (!interpolate)
            {
                i = Math.Round(i);
                j = Math.Round(j);

                double px;
                double py;

                if (this.XAxis.IsLogarithmic())
                {
                    double gx = Math.Log(this.X1 / this.X0) / (this.Data.GetLength(0) - 1);
                    px = this.X0 * Math.Exp(i * gx);
                }
                else
                {
                    double dx = (this.X1 - this.X0) / (this.Data.GetLength(0) - 1);
                    px = (i * dx) + this.X0;
                }

                if (this.YAxis.IsLogarithmic())
                {
                    double gy = Math.Log(this.Y1 / this.Y0) / (this.Data.GetLength(1) - 1);
                    py = this.Y0 * Math.Exp(j * gy);
                }
                else
                {
                    double dy = (this.Y1 - this.Y0) / (this.Data.GetLength(1) - 1);
                    py = (j * dy) + this.Y0;
                }

                p = new DataPoint(px, py);
                point = this.Transform(p);
            }

            // perform a second range check in index space to accomodate rounding
            if (i < -0.5 || i > this.Data.GetLength(0) - 0.5 || j < -0.5 || j > this.Data.GetLength(1) - 0.5)
            {
                return null;
            }

            var value = GetValue(this.Data, i, j);
            var colorAxis = this.ColorAxis as Axis;
            var colorAxisTitle = (colorAxis != null ? colorAxis.Title : null) ?? DefaultColorAxisTitle;

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
                value)
            };
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
            int m = this.Data.GetLength(0);
            int n = this.Data.GetLength(1);

            this.MinX = Math.Min(this.X0, this.X1);
            this.MaxX = Math.Max(this.X0, this.X1);

            this.MinY = Math.Min(this.Y0, this.Y1);
            this.MaxY = Math.Max(this.Y0, this.Y1);

            if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
            {
                if (this.XAxis.IsLogarithmic())
                {
                    double gx = Math.Log(this.MaxX / this.MinX) / (m - 1);
                    this.MinX *= Math.Exp(gx / -2);
                    this.MaxX *= Math.Exp(gx / 2);
                }
                else
                {
                    double dx = (this.MaxX - this.MinX) / (m - 1);
                    this.MinX -= dx / 2;
                    this.MaxX += dx / 2;
                }

                if (this.YAxis.IsLogarithmic())
                {
                    double gy = Math.Log(this.MaxY / this.MinY) / (n - 1);
                    this.MinY *= Math.Exp(gy / -2);
                    this.MaxY *= Math.Exp(gy / 2);
                }
                else
                {
                    double dy = (this.MaxY - this.MinY) / (n - 1);
                    this.MinY -= dy / 2;
                    this.MaxY += dy / 2;
                }
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            this.UpdateMaxMinXY();

            this.MinValue = this.Data.Min2D(true);
            this.MaxValue = this.Data.Max2D();
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
        /// Renders the labels.
        /// </summary>
        /// <param name="rc">The <see cref="IRenderContext" /></param>
        /// <param name="rect">The bounding rectangle for the data.</param>
        protected virtual void RenderLabels(IRenderContext rc, OxyRect rect)
        {
            var clip = this.GetClippingRect();
            int m = this.Data.GetLength(0);
            int n = this.Data.GetLength(1);
            double fontSize = (rect.Height / n) * this.LabelFontSize;

            double left = this.X0;
            double right = this.X1;
            double bottom = this.Y0;
            double top = this.Y1;

            var s00 = this.Orientate(this.Transform(left, bottom)); // disorientate
            var s11 = this.Orientate(this.Transform(right, top)); // disorientate

            double sdx = (s11.X - s00.X) / (m - 1);
            double sdy = (s11.Y - s00.Y) / (n - 1);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var point = this.Orientate(new ScreenPoint(s00.X + (i * sdx), s00.Y + (j * sdy))); // re-orientate
                    var v = GetValue(this.Data, i, j);
                    var color = this.ColorAxis.GetColor(v);
                    var hsv = color.ToHsv();
                    var textColor = hsv[2] > 0.6 ? OxyColors.Black : OxyColors.White;
                    var label = this.GetLabel(v, i, j);
                    rc.DrawClippedText(
                        clip,
                        point,
                        label,
                        textColor,
                        this.ActualFont,
                        fontSize,
                        500,
                        0,
                        HorizontalAlignment.Center,
                        VerticalAlignment.Middle);
                }
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
        /// Gets the interpolated value at the specified position in the data array (by bilinear interpolation).
        /// Where interpolation is impossible, return NaN, rather than a calculated nonsense value.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The first index.</param>
        /// <param name="j">The second index.</param>
        /// <returns>The interpolated value.</returns>
        private static double GetValue(double[,] data, double i, double j)
        {
            //// Note data[0, 0] is displayed in quadrant 1, not exactly at the origin, and that implies the invoker can produce negative coordinates.
            i = Math.Max(i, 0);
            j = Math.Max(j, 0);

            int i0 = (int)i;
            int i1 = i0 + 1 < data.GetLength(0) ? i0 + 1 : i0;

            int j0 = (int)j;
            int j1 = j0 + 1 < data.GetLength(1) ? j0 + 1 : j0;

            i = Math.Min(i, i1);
            j = Math.Min(j, j1);

            if ((i == i0) && (j == j0))
            {
                return data[i0, j0];
            }

            if ((i != i0) && (j == j0))
            {
                //// interpolate only by i

                if (double.IsNaN(data[i0, j0]) || double.IsNaN(data[i1, j0]))
                {
                    return double.NaN;
                }

                double ifraction = i - i0;
                if (i0 != i1)
                {
                    return (data[i0, j0] * (1 - ifraction)) + (data[i1, j0] * ifraction);
                }

                return data[i0, j0];
            }

            if ((i == i0) && (j != j0))
            {
                //// interpolate only by j

                if (double.IsNaN(data[i0, j0]) || double.IsNaN(data[i0, j1]))
                {
                    return double.NaN;
                }

                double jfraction = j - j0;
                if (j0 != j1)
                {
                    return (data[i0, j0] * (1 - jfraction)) + (data[i0, j1] * jfraction);
                }

                return data[i0, j0];
            }
            else
            {
                if (double.IsNaN(data[i0, j0]) || double.IsNaN(data[i1, j0]) || double.IsNaN(data[i0, j1]) || double.IsNaN(data[i1, j1]))
                {
                    return double.NaN;
                }

                double ifraction = i - i0;
                double jfraction = j - j0;
                double v0;
                double v1;
                if (i0 != i1)
                {
                    v0 = (data[i0, j0] * (1 - ifraction)) + (data[i1, j0] * ifraction);
                    v1 = (data[i0, j1] * (1 - ifraction)) + (data[i1, j1] * ifraction);
                }
                else
                {
                    v0 = data[i0, j0];
                    v1 = data[i0, j1];
                }

                if (j0 != j1)
                {
                    return (v0 * (1 - jfraction)) + (v1 * jfraction);
                }

                return v0;
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

        /// <summary>
        /// Updates the image.
        /// </summary>
        private void UpdateImage()
        {
            // determine if the provided data should be reversed in x-direction
            var reverseX = this.XAxis.Transform(this.X0) > this.XAxis.Transform(this.X1);

            // determine if the provided data should be reversed in y-direction
            var reverseY = this.YAxis.Transform(this.Y0) > this.YAxis.Transform(this.Y1);

            // determine if the data should be transposed
            var swapXY = this.IsTransposed();

            int m = this.Data.GetLength(0);
            int n = this.Data.GetLength(1);
            var buffer = swapXY ? new OxyColor[n, m] : new OxyColor[m, n];
            for (int i = 0; i < m; i++)
            {
                var ii = reverseX ? m - 1 - i : i;
                for (int j = 0; j < n; j++)
                {
                    var jj = reverseY ? n - 1 - j : j;
                    if (swapXY)
                    {
                        buffer[j, i] = this.ColorAxis.GetColor(this.Data[ii, jj]);
                    }
                    else
                    {
                        buffer[i, j] = this.ColorAxis.GetColor(this.Data[ii, jj]);
                    }
                }
            }

            this.image = OxyImage.Create(buffer, ImageFormat.Png);
        }
    }
}
