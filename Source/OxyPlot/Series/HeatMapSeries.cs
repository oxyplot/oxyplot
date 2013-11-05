// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatMapSeries.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The heat map series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
    /// The heat map series.
    /// </summary>
    /// <remarks>
    /// Does not work with Silverlight. Silverlight does not support bitmaps, only PNG and JPG.
    /// </remarks>
    public class HeatMapSeries : XYAxisSeries
    {
        /// <summary>
        /// The hash code of the current data.
        /// </summary>
        private int dataHash;

        /// <summary>
        /// The image
        /// </summary>
        private OxyImage image;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatMapSeries"/> class.
        /// </summary>
        public HeatMapSeries()
        {
            this.Interpolate = true;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the left column mid point.
        /// </summary>
        public double X0 { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the right column mid point.
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the top row mid point.
        /// </summary>
        public double Y0 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the bottom row mid point.
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// Gets or sets the data array.
        /// </summary>
        /// <remarks>
        /// Note that the indices of the data array refer to [x,y].
        /// </remarks>
        public double[,] Data { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to interpolate when rendering.
        /// </summary>
        /// <remarks>
        /// This property is not supported on all platforms.
        /// </remarks>
        public bool Interpolate { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the dataset.
        /// </summary>
        public double MinValue { get; protected set; }

        /// <summary>
        /// Gets or sets the maximum value of the dataset.
        /// </summary>
        public double MaxValue { get; protected set; }

        /// <summary>
        /// Gets or sets the color axis.
        /// </summary>
        /// <value>
        /// The color axis.
        /// </value>
        public ColorAxis ColorAxis { get; protected set; }

        /// <summary>
        /// Gets or sets the color axis key.
        /// </summary>
        /// <value> The color axis key. </value>
        public string ColorAxisKey { get; set; }

        /// <summary>
        /// Gets or sets the coordinate definition. The default value is Center.
        /// </summary>
        /// <value>
        /// The coordinate definition.
        /// </value>
        public HeatMapCoordinateDefinition CoordinateDefinition { get; set; }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.Data == null)
            {
                this.image = null;
                return;
            }

            double left = this.X0;
            double right = this.X1;
            double bottom = this.Y0;
            double top = this.Y1;

            if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
            {
                int m = this.Data.GetLength(0);
                int n = this.Data.GetLength(1);

                double dx = (this.X1 - this.X0) / (m - 1);
                double dy = (this.Y1 - this.Y0) / (n - 1);

                left -= dx / 2;
                right += dx / 2;
                bottom -= dy / 2;
                top += dy / 2;
            }

            var s00 = this.Transform(left, bottom);
            var s11 = this.Transform(right, top);
            var rect = OxyRect.Create(s00, s11);

            if (this.image == null || this.Data.GetHashCode() != this.dataHash)
            {
                this.UpdateImage();
                this.dataHash = this.Data.GetHashCode();
            }

            if (this.image != null)
            {
                var clip = this.GetClippingRect();
                rc.DrawClippedImage(clip, this.image, rect.Left, rect.Top, rect.Width, rect.Height, 1, this.Interpolate);
            }
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// Interpolate the series if this flag is set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (!this.Interpolate)
            {
                // it make no sense to interpolate the tracker
                // when the plot is not interpolated
                interpolate = false;
            }

            var p = this.InverseTransform(point);

            if (!this.IsPointInRange(p))
            {
                return null;
            }

            double dx = (this.X1 - this.X0) / (this.Data.GetLength(0) - 1);
            double dy = (this.Y1 - this.Y0) / (this.Data.GetLength(1) - 1);

            double i = (p.X - this.X0) / dx;
            double j = (p.Y - this.Y0) / dy;

            if (!interpolate)
            {
                i = Math.Round(i);
                j = Math.Round(j);
                p = new DataPoint((i * dx) + this.X0, (j * dy) + this.Y0);
                point = this.Transform(p);
            }

            var v = GetValue(this.Data, i, j);

            var formatString = this.TrackerFormatString;
            if (string.IsNullOrEmpty(this.TrackerFormatString))
            {
                formatString = "{2},{4}: {5}";
            }

            var text = this.Format(formatString, null, this.Title, this.XAxis.Title, p.X, this.YAxis.Title, p.Y, v);
            return new TrackerHitResult(this, p, point, null, -1, text);
        }

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal override void EnsureAxes()
        {
            base.EnsureAxes();

            this.ColorAxis =
                this.PlotModel.GetAxisOrDefault(this.ColorAxisKey, this.PlotModel.DefaultColorAxis) as ColorAxis;
        }

        /// <summary>
        /// Updates the max/minimum values.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            int m = this.Data.GetLength(0);
            int n = this.Data.GetLength(1);

            this.MinX = Math.Min(this.X0, this.X1);
            this.MaxX = Math.Max(this.X0, this.X1);

            this.MinY = Math.Min(this.Y0, this.Y1);
            this.MaxY = Math.Max(this.Y0, this.Y1);

            if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
            {
                double dx = Math.Abs(this.X1 - this.X0) / (m - 1);
                double dy = Math.Abs(this.Y1 - this.Y0) / (n - 1);
                this.MinX -= dx / 2;
                this.MaxX += dx / 2;
                this.MinY -= dy / 2;
                this.MaxY += dy / 2;
            }

            this.MinValue = this.GetData().Min();
            this.MaxValue = this.GetData().Max();

            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);

            this.YAxis.Include(this.MinY);
            this.YAxis.Include(this.MaxY);

            this.ColorAxis.Include(this.MinValue);
            this.ColorAxis.Include(this.MaxValue);
        }

        /// <summary>
        /// Gets the data as a sequence (LINQ-friendly).
        /// </summary>
        /// <returns>The sequence of data.</returns>
        protected IEnumerable<double> GetData()
        {
            int m = this.Data.GetLength(0);
            int n = this.Data.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    yield return this.Data[i, j];
                }
            }
        }

        /// <summary>
        /// Gets the interpolated value at the specified position in the data array (by bilinear interpolation).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The first index.</param>
        /// <param name="j">The second index.</param>
        /// <returns>The interpolated value.</returns>
        private static double GetValue(double[,] data, double i, double j)
        {
            var i0 = (int)i;
            int i1 = i0 + 1 < data.GetLength(0) ? i0 + 1 : i0;
            double ix = i - i0;
            var j0 = (int)j;
            int j1 = j0 + 1 < data.GetLength(1) ? j0 + 1 : j0;
            double jx = j - j0;
            var v0 = (data[i0, j0] * (1 - ix)) + (data[i1, j0] * ix);
            var v1 = (data[i0, j1] * (1 - ix)) + (data[i1, j1] * ix);
            return (v0 * (1 - jx)) + (v1 * jx);
        }

        /// <summary>
        /// Tests if a <see cref="DataPoint"/> is inside the heat map
        /// </summary>
        /// <param name="p">The <see cref="DataPoint"/> to test.</param>
        /// <returns><c>True</c> if the point is inside the heat map.</returns>
        private bool IsPointInRange(DataPoint p)
        {
            double left = this.X0;
            double right = this.X1;
            double bottom = this.Y0;
            double top = this.Y1;

            if (this.CoordinateDefinition == HeatMapCoordinateDefinition.Center)
            {
                double dx = (this.X1 - this.X0) / (this.Data.GetLength(0) - 1);
                double dy = (this.Y1 - this.Y0) / (this.Data.GetLength(1) - 1);

                left -= dx / 2;
                right += dx / 2;
                bottom -= dy / 2;
                top += dy / 2;
            }

            return p.X >= left && p.X <= right && p.Y >= bottom && p.Y <= top;
        }

        /// <summary>
        /// Updates the image.
        /// </summary>
        private void UpdateImage()
        {
            int m = this.Data.GetLength(0);
            int n = this.Data.GetLength(1);
            var buffer = new OxyColor[n, m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    buffer[j, i] = this.ColorAxis.GetColor(this.Data[i, j]);
                }
            }

            this.image = OxyImage.Create(buffer, ImageFormat.Png);
        }
    }
}