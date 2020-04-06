// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatrixSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a series that visualizes the structure of a matrix.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Provides a series that visualizes the structure of a matrix.
    /// </summary>
    public class MatrixSeries : XYAxisSeries
    {
        /// <summary>
        /// The image
        /// </summary>
        private OxyImage image;

        /// <summary>
        /// The matrix
        /// </summary>
        private double[,] matrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixSeries" /> class.
        /// </summary>
        public MatrixSeries()
        {
            this.GridInterval = 1;
            this.ShowDiagonal = false;
            this.MinimumGridLineDistance = 4;
            this.GridColor = OxyColors.LightGray;
            this.BorderColor = OxyColors.Gray;
            this.NotZeroColor = OxyColors.Black;
            this.ZeroTolerance = 0;
            this.TrackerFormatString = "{0}\r\n[{1},{2}] = {3}";
        }

        /// <summary>
        /// Gets or sets the matrix.
        /// </summary>
        public double[,] Matrix
        {
            get
            {
                return this.matrix;
            }

            set
            {
                this.image = null;
                this.matrix = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval between the grid lines (the grid is hidden if value is 0).
        /// </summary>
        public int GridInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the diagonal.
        /// </summary>
        /// <value><c>true</c> if the diagonal should be shown; otherwise, <c>false</c>.</value>
        public bool ShowDiagonal { get; set; }

        /// <summary>
        /// Gets or sets the minimum grid line distance.
        /// </summary>
        public double MinimumGridLineDistance { get; set; }

        /// <summary>
        /// Gets or sets the color of the grid.
        /// </summary>
        /// <value>The color of the grid.</value>
        public OxyColor GridColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the matrix.
        /// </summary>
        /// <value>The color of the border.</value>
        public OxyColor BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the not zero elements of the matrix.
        /// </summary>
        public OxyColor NotZeroColor { get; set; }

        /// <summary>
        /// Gets or sets the zero tolerance (inclusive).
        /// </summary>
        /// <value>The zero tolerance.</value>
        public double ZeroTolerance { get; set; }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            var dp = this.InverseTransform(point);
            int i = (int)dp.Y;
            int j = (int)dp.X;

            if (i >= 0 && i < this.matrix.GetLength(0) && j >= 0 && j < this.matrix.GetLength(1))
            {
                var value = this.matrix[i, j];
                return new TrackerHitResult
                {
                    Series = this,
                    DataPoint = dp,
                    Position = point,
                    Item = null,
                    Index = -1,
                    Text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, null, this.Title, i, j, value)
                };
            }

            return null;
        }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.Matrix == null)
            {
                return;
            }

            int m = this.Matrix.GetLength(0);
            int n = this.Matrix.GetLength(1);
            var p0 = this.Transform(0, 0);
            var p1 = this.Transform(n, m);

            // note matrix index [i,j] maps to image index [j,i]
            if (this.image == null)
            {
                var pixels = new OxyColor[n, m];
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        pixels[j, i] = Math.Abs(this.Matrix[i, j]) <= this.ZeroTolerance ? OxyColors.Transparent : this.NotZeroColor;
                    }
                }

                this.image = OxyImage.Create(pixels, ImageFormat.Png);
            }

            var clip = this.GetClippingRect();
            var x0 = Math.Min(p0.X, p1.X);
            var y0 = Math.Min(p0.Y, p1.Y);
            var w = Math.Abs(p0.X - p1.X);
            var h = Math.Abs(p0.Y - p1.Y);
            rc.DrawClippedImage(clip, this.image, x0, y0, w, h, 1, false);

            var points = new List<ScreenPoint>();
            if (this.GridInterval > 0)
            {
                var p2 = this.Transform(this.GridInterval, this.GridInterval);
                if (Math.Abs(p2.Y - p0.Y) > this.MinimumGridLineDistance)
                {
                    for (int i = 1; i < n; i += this.GridInterval)
                    {
                        points.Add(this.Transform(0, i));
                        points.Add(this.Transform(n, i));
                    }
                }

                if (Math.Abs(p2.X - p0.X) > this.MinimumGridLineDistance)
                {
                    for (int j = 1; j < m; j += this.GridInterval)
                    {
                        points.Add(this.Transform(j, 0));
                        points.Add(this.Transform(j, m));
                    }
                }
            }

            if (this.ShowDiagonal)
            {
                points.Add(this.Transform(0, 0));
                points.Add(this.Transform(n, m));
            }

            rc.DrawClippedLineSegments(clip, points, this.GridColor, 1, this.EdgeRenderingMode, null, LineJoin.Miter);

            if (this.BorderColor.IsVisible())
            {
                var borderPoints = new[]
                    {
                        this.Transform(0, 0),
                        this.Transform(m, 0),
                        this.Transform(0, n),
                        this.Transform(m, n),
                        this.Transform(0, 0),
                        this.Transform(0, n),
                        this.Transform(m, 0),
                        this.Transform(m, n)
                    };

                rc.DrawClippedLineSegments(clip, borderPoints, this.BorderColor, 1, this.EdgeRenderingMode, null, LineJoin.Miter);
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            if (this.Matrix == null)
            {
                return;
            }

            this.MinX = 0;
            this.MaxX = this.Matrix.GetLength(1);
            this.MinY = 0;
            this.MaxY = this.Matrix.GetLength(0);
        }
    }
}
