using System;
using System.Collections.Generic;

namespace OxyPlot
{
    public class AxisRenderer : PlotRenderer
    {
        private const double AXIS_LEGEND_DIST = 4; // distance from axis number to axis legend
        private const double TICK_DIST = 8; // distance from axis tick to number

        public AxisRenderer(IRenderContext rc, PlotModel p)
            : base(rc, p)
        {
        }


        ICollection<double> minorValues;
        ICollection<double> majorValues;

        public void Render(Axis axis)
        {
            if (axis == null)
                return;

            axis.GetTickValues(out majorValues, out minorValues);

            CreatePens(axis);

            if (axis.IsHorizontal)
            {
                RenderHorizontalAxis(axis, plot.DefaultYAxis);
            }
            if (axis.IsVertical)
            {
                RenderVerticalAxis(axis, plot.DefaultXAxis);
            }
            if (axis.Position == AxisPosition.Angle)
            {
                RenderAngleAxis(axis, plot.DefaultMagnitudeAxis);
            }
            if (axis.Position == AxisPosition.Magnitude)
            {
                RenderMagnitudeAxis(axis, plot.DefaultAngleAxis);
            }
        }

        private void RenderMagnitudeAxis(Axis axis, Axis angleAxis)
        {
            if (axis.RelatedAxis != null)
                angleAxis = axis.RelatedAxis;

            if (axis.ShowMinorTicks)
            {
                //  GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

                foreach (double xValue in minorValues)
                {
                    if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                    {
                        continue;
                    }

                    if (majorValues.Contains(xValue))
                    {
                        continue;
                    }

                    var pts = new List<ScreenPoint>();
                    for (double th = angleAxis.ActualMinimum; th <= angleAxis.ActualMaximum; th += angleAxis.MinorStep * 0.1)
                    {
                        pts.Add(axis.Transform(xValue, th, angleAxis));
                    }

                    if (minorPen != null)
                    {
                        rc.DrawLine(pts, minorPen.Color, minorPen.Thickness, minorPen.DashArray);
                    }
                    // RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            //  GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            foreach (double xValue in majorValues)
            {
                if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                {
                    continue;
                }

                var pts = new List<ScreenPoint>();
                for (double th = angleAxis.ActualMinimum; th <= angleAxis.ActualMaximum; th += angleAxis.MinorStep * 0.1)
                {
                    pts.Add(axis.Transform(xValue, th, angleAxis));
                }

                if (majorPen != null)
                {
                    rc.DrawLine(pts, majorPen.Color, majorPen.Thickness, majorPen.DashArray);
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

        private void RenderAngleAxis(Axis axis, Axis magnitudeAxis)
        {
            if (axis.RelatedAxis != null)
                magnitudeAxis = axis.RelatedAxis;

            if (axis.ShowMinorTicks)
            {
                //  GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

                foreach (double xValue in minorValues)
                {
                    if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                    {
                        continue;
                    }

                    if (majorValues.Contains(xValue))
                    {
                        continue;
                    }

                    var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);

                    if (minorPen != null)
                    {
                        RenderLine(axis.MidPoint.X, axis.MidPoint.Y, pt.x, pt.y, minorPen,false);
                    }
                    // RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            //  GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            foreach (double xValue in majorValues)
            {
                if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                {
                    continue;
                }

                var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);

                if (majorPen != null)
                {
                    RenderLine(axis.MidPoint.X, axis.MidPoint.Y, pt.x, pt.y, majorPen,false);
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

        private void RenderLine(double x0, double y0, double x1, double y1, OxyPen pen, bool aliased = true)
        {
            if (pen == null)
                return;

            rc.DrawLine(new[]
                            {
                                new ScreenPoint(x0, y0),
                                new ScreenPoint(x1, y1)
                            }, pen.Color, pen.Thickness, pen.DashArray, aliased);
        }


        private void GetVerticalTickPositions(Axis axis, TickStyle glt, double ticksize,
                                              out double y0, out double y1)
        {
            y0 = 0;
            y1 = 0;
            bool istop = axis.Position == AxisPosition.Top;
            double topsign = istop ? -1 : 1;
            switch (glt)
            {
                case TickStyle.Crossing:
                    y0 = -ticksize * topsign;
                    y1 = ticksize * topsign;
                    break;
                case TickStyle.Inside:
                    y0 = -ticksize * topsign;
                    break;
                case TickStyle.Outside:
                    y1 = ticksize * topsign;
                    break;
            }
        }

        private void GetHorizontalTickPositions(Axis axis, TickStyle glt, double ticksize, out double x0,
                                                out double x1)
        {
            x0 = 0;
            x1 = 0;
            bool isLeft = axis.Position == AxisPosition.Left;
            double leftSign = isLeft ? -1 : 1;
            switch (glt)
            {
                case TickStyle.Crossing:
                    x0 = -ticksize * leftSign;
                    x1 = ticksize * leftSign;
                    break;
                case TickStyle.Inside:
                    x0 = -ticksize * leftSign;
                    break;
                case TickStyle.Outside:
                    x1 = ticksize * leftSign;
                    break;
            }
        }

        private OxyPen minorPen;
        private OxyPen majorPen;
        private OxyPen minorTickPen;
        private OxyPen majorTickPen;
        private OxyPen zeroPen;
        private OxyPen extraPen;

        public void CreatePens(Axis axis)
        {
            minorPen = CreatePen(axis.MinorGridlineColor, axis.MinorGridlineThickness, axis.MinorGridlineStyle);
            majorPen = CreatePen(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            minorTickPen = CreatePen(axis.TicklineColor, axis.MinorGridlineThickness, LineStyle.Solid);
            majorTickPen = CreatePen(axis.TicklineColor, axis.MajorGridlineThickness, LineStyle.Solid);
            zeroPen = CreatePen(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            extraPen = CreatePen(axis.ExtraGridlineColor, axis.ExtraGridlineThickness, axis.ExtraGridlineStyle);
        }

        private void RenderHorizontalAxis(Axis axis, Axis perpendicularAxis)
        {

            double y = plotBounds.Bottom;
            switch (axis.Position)
            {
                case AxisPosition.Top:
                    y = plotBounds.Top;
                    break;
                case AxisPosition.Bottom:
                    y = plotBounds.Bottom;
                    break;
            }
            if (axis.PositionAtZeroCrossing)
            {
                y = perpendicularAxis.TransformX(0);
            }

            double y0, y1;

            if (axis.ShowMinorTicks)
            {
                GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

                foreach (double xValue in minorValues)
                {
                    if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                    {
                        continue;
                    }

                    if (majorValues.Contains(xValue))
                    {
                        continue;
                    }

                    double x = axis.TransformX(xValue);
                    if (minorPen != null)
                    {
                        RenderLine(x, plotBounds.Top, x, plotBounds.Bottom, minorPen);
                    }
                    RenderLine(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            double maxh = 0;
            bool istop = axis.Position == AxisPosition.Top;
            foreach (double xValue in majorValues)
            {
                if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                {
                    continue;
                }

                double x = axis.TransformX(xValue);

                if (majorPen != null)
                {
                    RenderLine(x, plotBounds.Top, x, plotBounds.Bottom, majorPen);
                }
                RenderLine(x, y + y0, x, y + y1, majorTickPen);

                if (xValue == 0 && axis.PositionAtZeroCrossing)
                    continue;

                var pt = new ScreenPoint(x, istop ? y + y1 - TICK_DIST : y + y1 + TICK_DIST);
                string text = axis.FormatValue(xValue);
                double h = rc.MeasureText(text, axis.FontFamily, axis.FontSize, axis.FontWeight).Height;

                rc.DrawText(pt, text, plot.TextColor,
                            axis.FontFamily, axis.FontSize, axis.FontWeight,
                            axis.Angle,
                            HorizontalTextAlign.Center, istop ? VerticalTextAlign.Bottom : VerticalTextAlign.Top);

                maxh = Math.Max(maxh, h);
            }

            if (axis.PositionAtZeroCrossing)
            {
                double x = axis.TransformX(0);
                RenderLine(x, plotBounds.Top, x, plotBounds.Bottom, zeroPen);
            }

            if (axis.ExtraGridlines != null)
                foreach (double x in axis.ExtraGridlines)
                {
                    double sx = axis.TransformX(x);
                    RenderLine(sx, plotBounds.Top, sx, plotBounds.Bottom, extraPen);
                }

            RenderLine(plotBounds.Left, y, plotBounds.Right, y, majorPen);

            double legendX = axis.TransformX((axis.ActualMinimum + axis.ActualMaximum) / 2);
            var halign = HorizontalTextAlign.Center;
            var valign = VerticalTextAlign.Bottom;

            if (axis.PositionAtZeroCrossing)
            {
                legendX = perpendicularAxis.TransformX(perpendicularAxis.ActualMaximum);
                // halign = axis.IsReversed ? HorizontalTextAlign.Left : HorizontalTextAlign.Right;
            }
            y = rc.Height - AXIS_LEGEND_DIST;
            if (istop)
            {
                y = AXIS_LEGEND_DIST;
                valign = VerticalTextAlign.Top;
            }
            rc.DrawText(new ScreenPoint(legendX, y), axis.Title, plot.TextColor,
                        axis.FontFamily, axis.FontSize, axis.FontWeight, 0, halign, valign);
        }

        private OxyPen CreatePen(OxyColor c, double th, LineStyle ls)
        {
            if (ls == LineStyle.None || th == 0)
                return null;
            return new OxyPen(c, th, ls);
        }

        private void RenderVerticalAxis(Axis axis, Axis perpendicularAxis)
        {
            double x = plotBounds.Left;
            switch (axis.Position)
            {
                case AxisPosition.Left:
                    x = plotBounds.Left;
                    break;
                case AxisPosition.Right:
                    x = plotBounds.Right;
                    break;
            }
            if (axis.PositionAtZeroCrossing)
                x = perpendicularAxis.TransformX(0);

            double x0, x1;

            if (axis.ShowMinorTicks)
            {
                GetHorizontalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out x0, out x1);
                foreach (double yValue in minorValues)
                {
                    if (yValue < axis.ActualMinimum || yValue > axis.ActualMaximum)
                    {
                        continue;
                    }

                    if (majorValues.Contains(yValue))
                    {
                        continue;
                    }
                    double y = axis.TransformX(yValue);

                    if (minorPen != null)
                    {
                        RenderLine(plotBounds.Left, y, plotBounds.Right, y, minorPen);
                    }

                    RenderLine(x + x0, y, x + x1, y, minorTickPen);
                }
            }

            GetHorizontalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out x0, out x1);
            double maxw = 0;

            bool isleft = axis.Position == AxisPosition.Left;

            foreach (double yValue in majorValues)
            {
                if (yValue < axis.ActualMinimum || yValue > axis.ActualMaximum)
                    continue;

                double y = axis.TransformX(yValue);

                if (majorPen != null)
                {
                    RenderLine(plotBounds.Left, y, plotBounds.Right, y, majorPen);
                }

                RenderLine(x + x0, y, x + x1, y, majorTickPen);

                if (yValue == 0 && axis.PositionAtZeroCrossing)
                    continue;

                var pt = new ScreenPoint(isleft ? x + x1 - TICK_DIST : x + x1 + TICK_DIST, y);
                string text = axis.FormatValue(yValue);
                double w = rc.MeasureText(text, axis.FontFamily, axis.FontSize, axis.FontWeight).Height;
                rc.DrawText(pt, text, plot.TextColor,
                            axis.FontFamily, axis.FontSize, axis.FontWeight,
                            axis.Angle,
                            isleft ? HorizontalTextAlign.Right : HorizontalTextAlign.Left, VerticalTextAlign.Middle);
                maxw = Math.Max(maxw, w);
            }

            if (axis.PositionAtZeroCrossing)
            {
                double y = axis.TransformX(0);
                RenderLine(plotBounds.Left, y, plotBounds.Right, y, zeroPen);
            }

            if (axis.ExtraGridlines != null)
                foreach (double y in axis.ExtraGridlines)
                {
                    double sy = axis.TransformX(y);
                    RenderLine(plotBounds.Left, sy, plotBounds.Right, sy, extraPen);
                }

            RenderLine(x, plotBounds.Top, x, plotBounds.Bottom, majorPen);

            double ymid = axis.TransformX((axis.ActualMinimum + axis.ActualMaximum) / 2);

            HorizontalTextAlign halign = HorizontalTextAlign.Center;
            VerticalTextAlign valign = VerticalTextAlign.Top;

            if (axis.PositionAtZeroCrossing)
            {
                ymid = perpendicularAxis.TransformX(perpendicularAxis.ActualMaximum);
                // valign = axis.IsReversed ? VerticalTextAlign.Top : VerticalTextAlign.Bottom;
            }

            if (isleft)
            {
                x = AXIS_LEGEND_DIST;
            }
            else
            {
                x = rc.Width - AXIS_LEGEND_DIST;
                valign = VerticalTextAlign.Bottom;
            }

            rc.DrawText(new ScreenPoint(x, ymid), axis.Title, plot.TextColor,
                        axis.FontFamily, axis.FontSize, axis.FontWeight,
                        -90, halign, valign);
        }
    }
}