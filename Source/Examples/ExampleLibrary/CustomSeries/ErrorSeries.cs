using System;
using System.Collections.Generic;
using System.Diagnostics;
using OxyPlot;

namespace ExampleLibrary
{
    public class ErrorSeries : DataPointSeries
    {
        public List<double> XErrors { get; set; }
        public List<double> YErrors { get; set; }
        public OxyColor Color { get; set; }
        public double StrokeThickness { get; set; }
        public ErrorSeries()
        {
            XErrors = new List<double>();
            YErrors = new List<double>();
            Color = OxyColors.Black;
            StrokeThickness = 1;
        }

        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            if (points.Count == 0)
            {
                return;
            }

            Debug.Assert(XAxis != null && YAxis != null, "Axis has not been defined.");

            var clippingRect = GetClippingRect();

            int n = points.Count;

            // Transform all points to screen coordinates
            var segments = new List<ScreenPoint>(n * 6);
            for (int i = 0; i < n; i++)
            {
                var sp = XAxis.Transform(points[i], YAxis);
                double errx = XErrors[i] * XAxis.Scale;
                double erry = YErrors[i] * Math.Abs(YAxis.Scale);
                double d = 4;

                if (errx > 0)
                {
                    var p0 = new ScreenPoint(sp.X - errx * 0.5, sp.Y);
                    var p1 = new ScreenPoint(sp.X + errx * 0.5, sp.Y);
                    segments.Add(p0);
                    segments.Add(p1);
                    segments.Add(new ScreenPoint(p0.X, p0.Y - d));
                    segments.Add(new ScreenPoint(p0.X, p0.Y + d));
                    segments.Add(new ScreenPoint(p1.X, p1.Y - d));
                    segments.Add(new ScreenPoint(p1.X, p1.Y + d));
                }
                if (erry > 0)
                {
                    var p0 = new ScreenPoint(sp.X, sp.Y - erry * 0.5);
                    var p1 = new ScreenPoint(sp.X, sp.Y + erry * 0.5);
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
                rc.DrawClippedLine(new[] { segments[i], segments[i + 1] }, clippingRect, 2, Color, StrokeThickness, LineStyle.Solid, OxyPenLineJoin.Bevel, true);

        }

        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var pts = new[]
                          {
                              new ScreenPoint(legendBox.Left, ymid), 
                              new ScreenPoint(legendBox.Right, ymid)
                          };
            rc.DrawLine(pts, Color, StrokeThickness, null, OxyPenLineJoin.Miter, true);
        }

    }
}