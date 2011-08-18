using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OxyPlot
{
    /// <summary>
    ///   Rendering helper class for horizontal and vertical axes (both linear and logarithmic)
    /// </summary>
    public class HorizontalAndVerticalAxisRenderer : AxisRendererBase
    {
        public HorizontalAndVerticalAxisRenderer(IRenderContext rc, PlotModel plot)
            : base(rc, plot)
        {
        }

        public override void Render(AxisBase axis)
        {
            base.Render(axis);

            var perpendicularAxis = Plot.DefaultXAxis;
            bool isHorizontal = true;

            // store properties locally for performance
            double ppl = Plot.PlotArea.Left;
            double ppr = Plot.PlotArea.Right;
            double ppt = Plot.PlotArea.Top;
            double ppb = Plot.PlotArea.Bottom;
            double actualMinimum = axis.ActualMinimum;
            double actualMaximum = axis.ActualMaximum;

            // Axis position (x or y screen coordinate)
            double apos = 0;
            double titlePosition = 0;


            switch (axis.Position)
            {
                case AxisPosition.Left:
                    apos = ppl;
                    titlePosition = Plot.PlotAndAxisArea.Left;
                    isHorizontal = false;
                    break;
                case AxisPosition.Right:
                    apos = ppr;
                    titlePosition = Plot.PlotAndAxisArea.Right;
                    isHorizontal = false;
                    break;
                case AxisPosition.Top:
                    apos = ppt;
                    titlePosition = Plot.PlotAndAxisArea.Top;
                    perpendicularAxis = Plot.DefaultYAxis;
                    break;
                case AxisPosition.Bottom:
                    apos = ppb;
                    titlePosition = Plot.PlotAndAxisArea.Bottom;
                    perpendicularAxis = Plot.DefaultYAxis;
                    break;
            }

            if (axis.PositionAtZeroCrossing)
            {
                apos = perpendicularAxis.Transform(0);
            }

            double a0 = 0, a1 = 0;
            var minorSegments = new List<ScreenPoint>();
            var minorTickSegments = new List<ScreenPoint>();
            var majorSegments = new List<ScreenPoint>();
            var majorTickSegments = new List<ScreenPoint>();
            
            double eps = axis.MinorStep * 1e-3;

            GetTickPositions(axis, axis.TickStyle, axis.MinorTickSize, axis.Position, out a0, out a1);

            foreach (double value in MinorTickValues)
            {
                if (value < actualMinimum-eps || value > actualMaximum+eps)
                {
                    continue;
                }

                if (MajorTickValues.Contains(value))
                {
                    continue;
                }

                if (axis.PositionAtZeroCrossing && Math.Abs(value) < eps)
                {
                    continue;
                }

                double transformedValue = axis.Transform(value);

                if (isHorizontal)
                {
                    SnapTo(ppl, ref transformedValue);
                    SnapTo(ppr, ref transformedValue);
                }
                else
                {
                    SnapTo(ppt, ref transformedValue);
                    SnapTo(ppb, ref transformedValue);
                }

                // Draw the minor grid line
                if (MinorPen != null)
                {
                    if (isHorizontal)
                    {
                        minorSegments.Add(new ScreenPoint(transformedValue, ppt));
                        minorSegments.Add(new ScreenPoint(transformedValue, ppb));
                    }
                    else
                    {
                        if (transformedValue < ppt || transformedValue > ppb)
                        {

                        }
                        minorSegments.Add(new ScreenPoint(ppl, transformedValue));
                        minorSegments.Add(new ScreenPoint(ppr, transformedValue));
                    }
                }

                // Draw the minor tick
                if (axis.TickStyle != TickStyle.None)
                {
                    if (isHorizontal)
                    {
                        minorTickSegments.Add(new ScreenPoint(transformedValue, apos + a0));
                        minorTickSegments.Add(new ScreenPoint(transformedValue, apos + a1));
                    }
                    else
                    {
                        minorTickSegments.Add(new ScreenPoint(apos + a0, transformedValue));
                        minorTickSegments.Add(new ScreenPoint(apos + a1, transformedValue));
                    }
                }
            }


            GetTickPositions(axis, axis.TickStyle, axis.MajorTickSize, axis.Position, out a0, out a1);

            foreach (double value in MajorTickValues)
            {
                if (value < actualMinimum-eps || value > actualMaximum+eps)
                {
                    continue;
                }

                if (axis.PositionAtZeroCrossing && Math.Abs(value) < eps)
                {
                    continue;
                }

                double transformedValue = axis.Transform(value);
                if (isHorizontal)
                {
                    SnapTo(ppl, ref transformedValue);
                    SnapTo(ppr, ref transformedValue);
                }
                else
                {
                    SnapTo(ppt, ref transformedValue);
                    SnapTo(ppb, ref transformedValue);
                }


                if (MajorPen != null)
                {
                    if (isHorizontal)
                    {
                        majorSegments.Add(new ScreenPoint(transformedValue, ppt));
                        majorSegments.Add(new ScreenPoint(transformedValue, ppb));
                    }
                    else
                    {
                        majorSegments.Add(new ScreenPoint(ppl, transformedValue));
                        majorSegments.Add(new ScreenPoint(ppr, transformedValue));
                    }
                }

                if (axis.TickStyle != TickStyle.None)
                {
                    if (isHorizontal)
                    {
                        majorTickSegments.Add(new ScreenPoint(transformedValue, apos + a0));
                        majorTickSegments.Add(new ScreenPoint(transformedValue, apos + a1));
                    }
                    else
                    {
                        majorTickSegments.Add(new ScreenPoint(apos + a0, transformedValue));
                        majorTickSegments.Add(new ScreenPoint(apos + a1, transformedValue));
                    }
                }

                if (value == 0 && axis.PositionAtZeroCrossing)
                {
                    continue;
                }

            }
          
            // Render the axis labels (numbers or category names)
            foreach (double value in MajorLabelValues)
            {
                if (value < actualMinimum-eps || value > actualMaximum+eps)
                {
                    continue;
                }

                if (axis.PositionAtZeroCrossing && Math.Abs(value) < eps)
                {
                    continue;
                }

                double transformedValue = axis.Transform(value);
                if (isHorizontal)
                {
                    SnapTo(ppl, ref transformedValue);
                    SnapTo(ppr, ref transformedValue);
                }
                else
                {
                    SnapTo(ppt, ref transformedValue);
                    SnapTo(ppb, ref transformedValue);
                }
                var pt = new ScreenPoint();
                var ha = HorizontalTextAlign.Right;
                var va = VerticalTextAlign.Middle;
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        pt = new ScreenPoint(apos + a1 - axis.AxisTickToLabelDistance, transformedValue);
                        GetRotatedAlignments(axis.Angle, HorizontalTextAlign.Right, VerticalTextAlign.Middle, out ha,
                                                    out va);
                        break;
                    case AxisPosition.Right:
                        pt = new ScreenPoint(apos + a1 + axis.AxisTickToLabelDistance, transformedValue);
                        GetRotatedAlignments(axis.Angle, HorizontalTextAlign.Left, VerticalTextAlign.Middle, out ha,
                                                    out va);
                        break;
                    case AxisPosition.Top:
                        pt = new ScreenPoint(transformedValue, apos + a1 - axis.AxisTickToLabelDistance);
                        GetRotatedAlignments(axis.Angle, HorizontalTextAlign.Center, VerticalTextAlign.Bottom,
                                                    out ha,
                                                    out va);
                        break;
                    case AxisPosition.Bottom:
                        pt = new ScreenPoint(transformedValue, apos + a1 + axis.AxisTickToLabelDistance);
                        GetRotatedAlignments(axis.Angle, HorizontalTextAlign.Center, VerticalTextAlign.Top, out ha,
                                                    out va);
                        break;
                }

                string text = axis.FormatValue(value);
                rc.DrawMathText(pt, text, Plot.TextColor,
                                     axis.ActualFont, axis.FontSize, axis.FontWeight,
                                     axis.Angle, ha, va, false);
            }

            // Draw the zero crossing line
            if (axis.PositionAtZeroCrossing && ZeroPen != null)
            {
                double t0 = axis.Transform(0);
                if (isHorizontal)
                {
                    rc.DrawLine(t0, ppt, t0, ppb, ZeroPen);
                }
                else
                {
                    rc.DrawLine(ppl, t0, ppr, t0, ZeroPen);
                }
            }

            // Draw extra grid lines
            if (axis.ExtraGridlines != null && ExtraPen != null)
            {
                foreach (double value in axis.ExtraGridlines)
                {
                    if (!IsWithin(value, actualMinimum, actualMaximum))
                    {
                        continue;
                    }

                    double transformedValue = axis.Transform(value);
                    if (isHorizontal)
                    {
                        rc.DrawLine(transformedValue, ppt, transformedValue, ppb, ExtraPen);
                    }
                    else
                    {
                        rc.DrawLine(ppl, transformedValue, ppr, transformedValue, ExtraPen);
                    }
                }
            }

            // Draw the axis line (across the tick marks)
            if (isHorizontal)
            {
                rc.DrawLine(axis.Transform(actualMinimum), apos, axis.Transform(actualMaximum), apos, AxislinePen);
            }
            else
            {
                rc.DrawLine(apos,axis.Transform(actualMinimum), apos, axis.Transform(actualMaximum), AxislinePen);
            }

            // Draw the axis title
            if (!String.IsNullOrEmpty(axis.ActualTitle))
            {
                double ymid = isHorizontal ? Lerp(axis.ScreenMin.X, axis.ScreenMax.X, axis.TitlePosition) : Lerp(axis.ScreenMax.Y, axis.ScreenMin.Y, axis.TitlePosition);

                double angle = -90;
                var lpt = new ScreenPoint();

                var halign = HorizontalTextAlign.Center;
                var valign = VerticalTextAlign.Top;

                if (axis.PositionAtZeroCrossing)
                {
                    ymid = perpendicularAxis.Transform(perpendicularAxis.ActualMaximum);
                }

                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        lpt = new ScreenPoint(titlePosition, ymid);
                        break;
                    case AxisPosition.Right:
                        lpt = new ScreenPoint(titlePosition, ymid);
                        valign = VerticalTextAlign.Bottom;
                        break;
                    case AxisPosition.Top:
                        lpt = new ScreenPoint(ymid, titlePosition);
                        halign = HorizontalTextAlign.Center;
                        valign = VerticalTextAlign.Top;
                        angle = 0;
                        break;
                    case AxisPosition.Bottom:
                        lpt = new ScreenPoint(ymid, titlePosition);
                        halign = HorizontalTextAlign.Center;
                        valign = VerticalTextAlign.Bottom;
                        angle = 0;
                        break;
                }

                rc.DrawText(lpt, axis.ActualTitle, Plot.TextColor,
                                axis.ActualFont, axis.FontSize, axis.FontWeight,
                                angle, halign, valign);
            }

            // Draw all the line segments);
            if (MinorPen != null)
                rc.DrawLineSegments(minorSegments, MinorPen);
            if (MajorPen != null)
                rc.DrawLineSegments(majorSegments, MajorPen);
            if (MinorTickPen != null)
                rc.DrawLineSegments(minorTickSegments, MinorTickPen);
            if (MajorTickPen != null)
                rc.DrawLineSegments(majorTickSegments, MajorTickPen);
        }

        /// <summary>
        /// Snaps v to value if is within a distance of eps.
        /// </summary>
        private static void SnapTo(double value, ref double v, double eps = 0.5)
        {
            if (v > value - eps && v < value + eps)
                v = value;
        }

        /// <summary>
        /// Linear interpolation
        /// http://en.wikipedia.org/wiki/Linear_interpolation
        /// </summary>
        /// <param name="x0">The x0.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        private static double Lerp(double x0, double x1, double f)
        {
            return x0 * (1 - f) + x1 * f;
        }


        /// <summary>
        ///   Gets the rotated alignments given the specified angle.
        /// </summary>
        /// <param name = "angle">The angle.</param>
        /// <param name = "defaultHorizontalAlignment">The default horizontal alignment.</param>
        /// <param name = "defaultVerticalAlignment">The default vertical alignment.</param>
        /// <param name = "ha">The rotated horizontal alignment.</param>
        /// <param name = "va">The rotated vertical alignment.</param>
        private static void GetRotatedAlignments(double angle, HorizontalTextAlign defaultHorizontalAlignment,
                                                              VerticalTextAlign defaultVerticalAlignment,
                                                              out HorizontalTextAlign ha, out VerticalTextAlign va)
        {
            ha = defaultHorizontalAlignment;
            va = defaultVerticalAlignment;

            Debug.Assert(angle <= 180 && angle >= -180, "Axis angle should be in the interval [-180,180] degrees.");

            if (angle > -45 && angle < 45)
            {
                return;
            }

            if (angle > 135 || angle < -135)
            {
                ha = (HorizontalTextAlign)(-(int)defaultHorizontalAlignment);
                va = (VerticalTextAlign)(-(int)defaultVerticalAlignment);
                return;
            }

            if (angle > 45)
            {
                ha = (HorizontalTextAlign)((int)defaultVerticalAlignment);
                va = (VerticalTextAlign)(-(int)defaultHorizontalAlignment);
                return;
            }

            if (angle < -45)
            {
                ha = (HorizontalTextAlign)(-(int)defaultVerticalAlignment);
                va = (VerticalTextAlign)((int)defaultHorizontalAlignment);
                return;
            }
        }
    }
}