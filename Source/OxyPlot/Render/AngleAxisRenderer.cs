namespace OxyPlot
{
    using System;

    public class AngleAxisRendererBase : AxisRendererBase
    {
        public AngleAxisRendererBase(IRenderContext rc, PlotModel plot)
            : base(rc, plot)
        {
        }

        public override void Render(AxisBase axis)
        {
            base.Render(axis);

            var magnitudeAxis = Plot.DefaultMagnitudeAxis;

            if (axis.RelatedAxis != null)
            {
                magnitudeAxis = axis.RelatedAxis;
            }
            double eps = axis.MinorStep * 1e-3;

            if (axis.ShowMinorTicks)
            {
                // GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

                foreach (double xValue in MinorTickValues)
                {
                    if (xValue < axis.ActualMinimum - eps || xValue > axis.ActualMaximum + eps)
                    {
                        continue;
                    }

                    if (MajorTickValues.Contains(xValue))
                    {
                        continue;
                    }

                    var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);

                    if (MinorPen != null)
                    {
                        rc.DrawLine(axis.MidPoint.x, axis.MidPoint.y, pt.x, pt.y, MinorPen, false);
                    }

                    // RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            // GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            foreach (double xValue in MajorTickValues)
            {
                if (xValue < axis.ActualMinimum - eps || xValue > axis.ActualMaximum + eps)
                {
                    continue;
                }

                var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);

                if (MajorPen != null)
                {
                    rc.DrawLine(axis.MidPoint.x, axis.MidPoint.y, pt.x, pt.y, MajorPen, false);
                }

            }

            foreach (var value in MajorLabelValues)
            {
                // skip the last value (overlapping with the first)
                if (value > axis.ActualMaximum - eps) continue;

                var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, value, axis);
                double angle = Math.Atan2(pt.y - axis.MidPoint.y, pt.x - axis.MidPoint.x) ;
                
                // add some margin
                pt.x += Math.Cos(angle) * axis.AxisTickToLabelDistance;
                pt.y += Math.Sin(angle) * axis.AxisTickToLabelDistance;
                
                // Convert to degrees
                angle *= 180 / Math.PI;
                
                string text = axis.FormatValue(value);

                var ha = HorizontalTextAlign.Left;
                var va = VerticalTextAlign.Middle;

                if (Math.Abs(Math.Abs(angle) - 90) < 10)
                {
                    ha = HorizontalTextAlign.Center;
                    va = angle > 90 ? VerticalTextAlign.Top : VerticalTextAlign.Bottom;
                    angle = 0;
                }
                else if (angle > 90 || angle < -90)
                {
                    angle -= 180;
                    ha = HorizontalTextAlign.Right;
                }
                rc.DrawMathText(pt, text, Plot.TextColor,
                                     axis.ActualFont, axis.FontSize, axis.FontWeight,
                                     angle, ha, va, false);
            }
        }
    }
}