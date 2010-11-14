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

        public void Render(IAxis axis)
        {
            var ra = axis as RangeAxis;
            if (ra != null)
                Render(ra);
        }
        public void Render(RangeAxis axis)
        {
            if (axis.IsHorizontal)
            {
                RenderHorizontalAxis(axis, plot.DefaultYAxis);
            }
            else
            {
                RenderVerticalAxis(axis, plot.DefaultXAxis);
            }
        }

        private void RenderGridline(double x0, double y0, double x1, double y1, Pen pen)
        {
            rc.DrawLine(new[]
                            {
                                new Point(x0, y0),
                                new Point(x1, y1)
                            }, pen.Color, pen.Thickness, pen.DashArray);
        }


        private void GetVerticalTickPositions(IAxis axis, TickType glt, double ticksize,
                                              out double y0, out double y1)
        {
            y0 = 0;
            y1 = 0;
            bool istop = axis.Position == AxisPosition.Top;
            double topsign = istop ? -1 : 1;
            switch (glt)
            {
                case TickType.Short:
                    y0 = -ticksize * topsign;
                    y1 = ticksize * topsign;
                    break;
                case TickType.ShortInside:
                    y0 = -ticksize * topsign;
                    break;
                case TickType.ShortOutside:
                    y1 = ticksize * topsign;
                    break;
            }
        }

        private void GetHorizontalTickPositions(IAxis axis, TickType glt, double ticksize, out double x0,
                                                out double x1)
        {
            x0 = 0;
            x1 = 0;
            bool isLeft = axis.Position == AxisPosition.Left;
            double leftSign = isLeft ? -1 : 1;
            switch (glt)
            {
                case TickType.Short:
                    x0 = -ticksize * leftSign;
                    x1 = ticksize * leftSign;
                    break;
                case TickType.ShortInside:
                    x0 = -ticksize * leftSign;
                    break;
                case TickType.ShortOutside:
                    x1 = ticksize * leftSign;
                    break;
            }
        }

        private void RenderHorizontalAxis(RangeAxis axis, IAxis perpendicularAxis)
        {
            var minorPen = new Pen(axis.MinorGridlineColor, axis.MinorGridlineThickness, axis.MinorGridlineStyle);
            var minorTickPen = new Pen(axis.MinorTicklineColor, axis.MinorGridlineThickness, LineStyle.Solid);
            var majorPen = new Pen(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            var majorTickPen = new Pen(axis.MajorTicklineColor, axis.MajorGridlineThickness, LineStyle.Solid);
            var zeroPen = new Pen(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            var extraPen = new Pen(axis.ExtraGridlineColor, axis.ExtraGridlineThickness, axis.ExtraGridlineStyle);

            ICollection<double> minorValues;
            ICollection<double> majorValues;
            axis.GetTickValues(out majorValues, out minorValues);

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
                y = perpendicularAxis.Transform(0);

            double y0, y1;

            if (axis.ShowMinorTicks)
            {
                GetVerticalTickPositions(axis, axis.MinorTickType, axis.MinorTickSize, out y0, out y1);

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

                    double x = axis.Transform(xValue);
                    if (axis.MinorGridlines)
                    {
                        RenderGridline(x, plotBounds.Top, x, plotBounds.Bottom, minorPen);
                    }
                    RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            GetVerticalTickPositions(axis, axis.MajorTickType, axis.MajorTickSize, out y0, out y1);

            double maxh = 0;
            bool istop = axis.Position == AxisPosition.Top;
            foreach (double xValue in majorValues)
            {
                if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                {
                    continue;
                }

                double x = axis.Transform(xValue);

                if (axis.MajorGridlines)
                {
                    RenderGridline(x, plotBounds.Top, x, plotBounds.Bottom, majorPen);
                }
                RenderGridline(x, y + y0, x, y + y1, majorTickPen);

                if (xValue == 0 && axis.PositionAtZeroCrossing)
                    continue;

                var pt = new Point(x, istop ? y + y1 - TICK_DIST : y + y1 + TICK_DIST);
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
                double x = axis.Transform(0);
                RenderGridline(x, plotBounds.Top, x, plotBounds.Bottom, zeroPen);
            }

            if (axis.ExtraGridlines != null)
                foreach (double x in axis.ExtraGridlines)
                {
                    double sx = axis.Transform(x);
                    RenderGridline(sx, plotBounds.Top, sx, plotBounds.Bottom, extraPen);
                }

            RenderGridline(plotBounds.Left, y, plotBounds.Right, y, majorPen);

            double legendX = axis.Transform((axis.ActualMinimum + axis.ActualMaximum) / 2);
            var halign = HorizontalTextAlign.Center;
            var valign = VerticalTextAlign.Bottom;

            if (axis.PositionAtZeroCrossing)
            {
                legendX = axis.Transform(axis.ActualMaximum);
                // halign = axis.IsReversed ? HorizontalTextAlign.Left : HorizontalTextAlign.Right;
            }
            y = rc.Height - AXIS_LEGEND_DIST;
            if (istop)
            {
                y = AXIS_LEGEND_DIST;
                valign = VerticalTextAlign.Top;
            }
            rc.DrawText(new Point(legendX, y), axis.Title, plot.TextColor,
                        axis.FontFamily, axis.FontSize, axis.FontWeight, 0, halign, valign);
        }

        private void RenderVerticalAxis(RangeAxis axis, IAxis perpendicularAxis)
        {
            var minorPen = new Pen(axis.MinorGridlineColor, axis.MinorGridlineThickness, axis.MinorGridlineStyle);
            var minorTickPen = new Pen(axis.MinorTicklineColor, axis.MinorGridlineThickness, LineStyle.Solid);
            var majorPen = new Pen(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            var majorTickPen = new Pen(axis.MajorTicklineColor, axis.MajorGridlineThickness, LineStyle.Solid);
            var zeroPen = new Pen(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            var extraPen = new Pen(axis.ExtraGridlineColor, axis.ExtraGridlineThickness, axis.ExtraGridlineStyle);

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
                x = perpendicularAxis.Transform(0);

            ICollection<double> minorValues;
            ICollection<double> majorValues;
            axis.GetTickValues(out majorValues, out minorValues);

            double x0, x1;

            if (axis.ShowMinorTicks)
            {
                GetHorizontalTickPositions(axis, axis.MinorTickType, axis.MinorTickSize, out x0, out x1);
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
                    double y = axis.Transform(yValue);

                    if (axis.MinorGridlines)
                    {
                        RenderGridline(plotBounds.Left, y, plotBounds.Right, y, minorPen);
                    }

                    RenderGridline(x + x0, y, x + x1, y, minorTickPen);
                }
            }

            GetHorizontalTickPositions(axis, axis.MajorTickType, axis.MajorTickSize, out x0, out x1);
            double maxw = 0;

            bool isleft = axis.Position == AxisPosition.Left;

            foreach (double yValue in majorValues)
            {
                if (yValue < axis.ActualMinimum || yValue > axis.ActualMaximum)
                    continue;

                double y = axis.Transform(yValue);

                if (axis.MajorGridlines)
                {
                    RenderGridline(plotBounds.Left, y, plotBounds.Right, y, majorPen);
                }

                RenderGridline(x + x0, y, x + x1, y, majorTickPen);

                if (yValue == 0 && axis.PositionAtZeroCrossing)
                    continue;

                var pt = new Point(isleft ? x + x1 - TICK_DIST : x + x1 + TICK_DIST, y);
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
                double y = axis.Transform(0);
                RenderGridline(plotBounds.Left, y, plotBounds.Right, y, zeroPen);
            }

            if (axis.ExtraGridlines != null)
                foreach (double y in axis.ExtraGridlines)
                {
                    double sy = axis.Transform(y);
                    RenderGridline(plotBounds.Left, sy, plotBounds.Right, sy, extraPen);
                }

            RenderGridline(x, plotBounds.Top, x, plotBounds.Bottom, majorPen);

            double ymid = axis.Transform((axis.ActualMinimum + axis.ActualMaximum) / 2);

            HorizontalTextAlign halign = HorizontalTextAlign.Center;
            VerticalTextAlign valign = VerticalTextAlign.Top;

            if (axis.PositionAtZeroCrossing)
            {
                ymid = axis.Transform(axis.ActualMaximum);
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

            rc.DrawText(new Point(x, ymid), axis.Title, plot.TextColor,
                        axis.FontFamily, axis.FontSize, axis.FontWeight,
                        -90, halign, valign);
        }
    }
}