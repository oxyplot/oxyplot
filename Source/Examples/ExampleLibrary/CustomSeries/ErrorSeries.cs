// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorSeries.cs" company="OxyPlot">
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
//   Represents an error series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;

    /// <summary>
    /// Represents an error series.
    /// </summary>
    public class ErrorSeries : DataPointSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorSeries"/> class.
        /// </summary>
        public ErrorSeries()
        {
            this.Color = OxyColors.Black;
            this.StrokeThickness = 1;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            var points = this.Points;
            if (points.Count == 0)
            {
                return;
            }

            if (this.XAxis == null || this.YAxis == null)
            {
                Trace("Axis not defined.");
                return;
            }

            var clippingRect = GetClippingRect();

            int n = points.Count;

            // Transform all points to screen coordinates
            var segments = new List<ScreenPoint>(n * 6);
            for (int i = 0; i < n; i++)
            {
                var sp = XAxis.Transform(points[i].X, points[i].Y, YAxis);
                var ei = points[i] as ErrorItem;
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
                    new[] { segments[i], segments[i + 1] },
                    clippingRect,
                    2,
                    this.GetSelectableColor(this.Color),
                    this.StrokeThickness,
                    LineStyle.Solid,
                    OxyPenLineJoin.Bevel,
                    true);
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
            rc.DrawLineSegments(pts, this.GetSelectableColor(this.Color), this.StrokeThickness, null, OxyPenLineJoin.Miter, true);
        }
    }
}