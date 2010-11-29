namespace OxyPlot
{
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
                magnitudeAxis = axis.RelatedAxis;

            if (axis.ShowMinorTicks)
            {
                //  GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

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

                    var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);

                    if (MinorPen != null)
                    {
                        rc.DrawLine(axis.MidPoint.x, axis.MidPoint.y, pt.x, pt.y, MinorPen, false);
                    }
                    // RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            //  GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            foreach (double xValue in MajorTickValues)
            {
                if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                {
                    continue;
                }

                var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);

                if (MajorPen != null)
                {
                    rc.DrawLine(axis.MidPoint.x, axis.MidPoint.y, pt.x, pt.y, MajorPen, false);
                }
                // RenderGridline(x, y + y0, x, y + y1, majorTickPen);


                //var pt = new ScreenPoint(x, istop ? y + y1 - TICK_DIST : y + y1 + TICK_DIST);
                //string text = axis.FormatValue(xValue);
                //double h = rc.MeasureText(text, axis.FontFamily, axis.FontSize, axis.FontWeight).Height;

                //rc.DrawText(pt, text, plot.TextColor,
                //            axis.FontFamily, axis.FontSize, axis.FontWeight,
                //            axis.Angle,
                //            HorizontalTextAlign.Center, istop ? VerticalTextAlign.Bottom : VerticalTextAlign.Top);

                //maxh = Math.Max(maxh, h);
            }
        }

   }
}