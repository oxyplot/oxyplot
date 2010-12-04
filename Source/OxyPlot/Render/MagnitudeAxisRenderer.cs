using System.Collections.Generic;

namespace OxyPlot
{
    public class MagnitudeAxisRendererBase : AxisRendererBase
    {
        public MagnitudeAxisRendererBase(IRenderContext rc, PlotModel plot)
            : base(rc, plot)
        {
        }

        public override void Render(AxisBase axis)
        {
            base.Render(axis);

            var angleAxis = Plot.DefaultAngleAxis as AxisBase;
            if (axis.RelatedAxis != null)
            {
                angleAxis = axis.RelatedAxis;
            }

            if (axis.ShowMinorTicks)
            {
                // GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

                foreach (double xValue in MinorTickValues)
                {
                    if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                    {
                        continue;
                    }

                    if (MajorTickValues.Contains(xValue))
                    {
                        continue;
                    }

                    var pts = new List<ScreenPoint>();
                    for (double th = angleAxis.ActualMinimum;
                         th <= angleAxis.ActualMaximum;
                         th += angleAxis.MinorStep * 0.1)
                    {
                        pts.Add(axis.Transform(new DataPoint(xValue, th), angleAxis));
                    }

                    if (MinorPen != null)
                    {
                        rc.DrawLine(pts, MinorPen.Color, MinorPen.Thickness, MinorPen.DashArray);
                    }

                    // RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            // GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            foreach (double xValue in MajorTickValues)
            {
                if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                {
                    continue;
                }

                var pts = new List<ScreenPoint>();
                for (double th = angleAxis.ActualMinimum; th <= angleAxis.ActualMaximum; th += angleAxis.MinorStep * 0.1)
                {
                    pts.Add(axis.Transform(new DataPoint(xValue, th), angleAxis));
                }

                if (MajorPen != null)
                {
                    rc.DrawLine(pts, MajorPen.Color, MajorPen.Thickness, MajorPen.DashArray);
                }

                // RenderGridline(x, y + y0, x, y + y1, majorTickPen);


                // var pt = new ScreenPoint(x, istop ? y + y1 - TICK_DIST : y + y1 + TICK_DIST);
                // string text = axis.FormatValue(xValue);
                // double h = rc.MeasureText(text, axis.FontFamily, axis.FontSize, axis.FontWeight).Height;

                // rc.DrawText(pt, text, plot.TextColor,
                // axis.FontFamily, axis.FontSize, axis.FontWeight,
                // axis.Angle,
                // HorizontalTextAlign.Center, istop ? VerticalTextAlign.Bottom : VerticalTextAlign.Top);
                // maxh = Math.Max(maxh, h);
            }
        }
    }
}