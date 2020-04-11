// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an error series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Represents an error series.
    /// </summary>
    public class ErrorSeries : XYAxisSeries
    {
        /// <summary>
        /// The list of error items.
        /// </summary>
        private readonly List<ErrorItem> points = new List<ErrorItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorSeries" /> class.
        /// </summary>
        public ErrorSeries()
        {
            this.Color = OxyColors.Black;
            this.StrokeThickness = 1;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets the list of points.
        /// </summary>
        /// <value>A list of <see cref="ErrorItem" />.</value>
        public List<ErrorItem> Points
        {
            get
            {
                return this.points;
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var points = this.Points;
            if (points.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();

            int n = points.Count;

            // Transform all points to screen coordinates
            var segments = new List<ScreenPoint>(n * 6);
            for (int i = 0; i < n; i++)
            {
                var sp = XAxis.Transform(points[i].X, points[i].Y, YAxis);
                var ei = points[i];
                double errorx = ei != null ? ei.XError * XAxis.Scale : 0;
                double errory = ei != null ? ei.YError * Math.Abs(YAxis.Scale) : 0;
                double d = 4;

                if (errorx > 0)
                {
                    var p0 = new ScreenPoint(sp.X - (errorx * 0.5), sp.Y);
                    var p1 = new ScreenPoint(sp.X + (errorx * 0.5), sp.Y);
                    segments.Add(p0);
                    segments.Add(p1);
                    segments.Add(new ScreenPoint(p0.X, p0.Y - d));
                    segments.Add(new ScreenPoint(p0.X, p0.Y + d));
                    segments.Add(new ScreenPoint(p1.X, p1.Y - d));
                    segments.Add(new ScreenPoint(p1.X, p1.Y + d));
                }

                if (errory > 0)
                {
                    var p0 = new ScreenPoint(sp.X, sp.Y - (errory * 0.5));
                    var p1 = new ScreenPoint(sp.X, sp.Y + (errory * 0.5));
                    segments.Add(p0);
                    segments.Add(p1);
                    segments.Add(new ScreenPoint(p0.X - d, p0.Y));
                    segments.Add(new ScreenPoint(p0.X + d, p0.Y));
                    segments.Add(new ScreenPoint(p1.X - d, p1.Y));
                    segments.Add(new ScreenPoint(p1.X + d, p1.Y));
                }
            }

            // clip the line segments with the clipping rectangle
            for (int i = 0; i + 1 < segments.Count; i += 2)
            {
                rc.DrawClippedLine(
                    clippingRect,
                    new[] { segments[i], segments[i + 1] },
                    2,
                    this.GetSelectableColor(this.Color),
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    null,
                    LineJoin.Bevel);
            }
        }

        /// <summary>
        /// Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The legend rectangle.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) * 0.5;
            double ymid = (legendBox.Top + legendBox.Bottom) * 0.5;
            var pts = new[]
                          {
                              new ScreenPoint(legendBox.Left, ymid),
                              new ScreenPoint(legendBox.Right, ymid),
                              new ScreenPoint(legendBox.Left, ymid - 2),
                              new ScreenPoint(legendBox.Left, ymid + 3),
                              new ScreenPoint(legendBox.Right, ymid - 2),
                              new ScreenPoint(legendBox.Right, ymid + 3),

                              new ScreenPoint(xmid, legendBox.Top),
                              new ScreenPoint(xmid, legendBox.Bottom),
                              new ScreenPoint(xmid - 2, legendBox.Top),
                              new ScreenPoint(xmid + 3, legendBox.Top),
                              new ScreenPoint(xmid - 2, legendBox.Bottom),
                              new ScreenPoint(xmid + 3, legendBox.Bottom)
                          };
            rc.DrawLineSegments(pts, this.GetSelectableColor(this.Color), this.StrokeThickness, this.EdgeRenderingMode, null, LineJoin.Miter);
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.points, p => p.X - p.XError, p => p.X + p.XError, p => p.Y - p.YError, p => p.Y + p.YError);
        }
    }
}
