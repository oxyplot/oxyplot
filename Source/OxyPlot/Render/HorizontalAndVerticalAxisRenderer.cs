// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HorizontalAndVerticalAxisRenderer.cs" company="OxyPlot">
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
//   Rendering helper class for horizontal and vertical axes (both linear and logarithmic)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Rendering helper class for horizontal and vertical axes (both linear and logarithmic)
    /// </summary>
    public class HorizontalAndVerticalAxisRenderer : AxisRendererBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalAndVerticalAxisRenderer"/> class.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="plot">
        /// The plot.
        /// </param>
        public HorizontalAndVerticalAxisRenderer(IRenderContext rc, PlotModel plot)
            : base(rc, plot)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        public override void Render(Axis axis)
        {
            base.Render(axis);

            double totalShift = axis.PositionTierMinShift;
            double tierSize = axis.PositionTierSize - this.Plot.AxisTierDistance;

            Axis perpendicularAxis = this.Plot.DefaultXAxis;
            bool isHorizontal = true;

            // store properties locally for performance
            double plotAreaLeft = this.Plot.PlotArea.Left;
            double plotAreaRight = this.Plot.PlotArea.Right;
            double plotAreaTop = this.Plot.PlotArea.Top;
            double plotAreaBottom = this.Plot.PlotArea.Bottom;
            double actualMinimum = axis.ActualMinimum;
            double actualMaximum = axis.ActualMaximum;

            // Axis position (x or y screen coordinate)
            double axisPosition = 0;
            double titlePosition = 0;

            switch (axis.Position)
            {
                case AxisPosition.Left:
                    axisPosition = plotAreaLeft - totalShift;
                    titlePosition = axisPosition - tierSize;
                    isHorizontal = false;
                    break;
                case AxisPosition.Right:
                    axisPosition = plotAreaRight + totalShift;
                    titlePosition = axisPosition + tierSize;
                    isHorizontal = false;
                    break;
                case AxisPosition.Top:
                    axisPosition = plotAreaTop - totalShift;
                    titlePosition = axisPosition - tierSize;
                    perpendicularAxis = this.Plot.DefaultYAxis;
                    break;
                case AxisPosition.Bottom:
                    axisPosition = plotAreaBottom + totalShift;
                    titlePosition = axisPosition + tierSize;
                    perpendicularAxis = this.Plot.DefaultYAxis;
                    break;
            }

            if (axis.PositionAtZeroCrossing)
            {
                axisPosition = perpendicularAxis.Transform(0);
            }

            double a0, a1;
            var minorSegments = new List<ScreenPoint>();
            var minorTickSegments = new List<ScreenPoint>();
            var majorSegments = new List<ScreenPoint>();
            var majorTickSegments = new List<ScreenPoint>();

            double eps = axis.ActualMinorStep * 1e-3;

            this.GetTickPositions(axis, axis.TickStyle, axis.MinorTickSize, axis.Position, out a0, out a1);

            foreach (double value in this.MinorTickValues)
            {
                if (value < actualMinimum - eps || value > actualMaximum + eps)
                {
                    continue;
                }

                if (this.MajorTickValues.Contains(value))
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
                    SnapTo(plotAreaLeft, ref transformedValue);
                    SnapTo(plotAreaRight, ref transformedValue);
                }
                else
                {
                    SnapTo(plotAreaTop, ref transformedValue);
                    SnapTo(plotAreaBottom, ref transformedValue);
                }

                // Draw the minor grid line
                if (this.MinorPen != null)
                {
                    if (isHorizontal)
                    {
                        minorSegments.Add(new ScreenPoint(transformedValue, plotAreaTop));
                        minorSegments.Add(new ScreenPoint(transformedValue, plotAreaBottom));
                    }
                    else
                    {
                        if (transformedValue < plotAreaTop || transformedValue > plotAreaBottom)
                        {
                        }

                        minorSegments.Add(new ScreenPoint(plotAreaLeft, transformedValue));
                        minorSegments.Add(new ScreenPoint(plotAreaRight, transformedValue));
                    }
                }

                // Draw the minor tick
                if (axis.TickStyle != TickStyle.None)
                {
                    if (isHorizontal)
                    {
                        minorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a0));
                        minorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a1));
                    }
                    else
                    {
                        minorTickSegments.Add(new ScreenPoint(axisPosition + a0, transformedValue));
                        minorTickSegments.Add(new ScreenPoint(axisPosition + a1, transformedValue));
                    }
                }
            }

            this.GetTickPositions(axis, axis.TickStyle, axis.MajorTickSize, axis.Position, out a0, out a1);

            foreach (double value in this.MajorTickValues)
            {
                if (value < actualMinimum - eps || value > actualMaximum + eps)
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
                    SnapTo(plotAreaLeft, ref transformedValue);
                    SnapTo(plotAreaRight, ref transformedValue);
                }
                else
                {
                    SnapTo(plotAreaTop, ref transformedValue);
                    SnapTo(plotAreaBottom, ref transformedValue);
                }

                if (this.MajorPen != null)
                {
                    if (isHorizontal)
                    {
                        majorSegments.Add(new ScreenPoint(transformedValue, plotAreaTop));
                        majorSegments.Add(new ScreenPoint(transformedValue, plotAreaBottom));
                    }
                    else
                    {
                        majorSegments.Add(new ScreenPoint(plotAreaLeft, transformedValue));
                        majorSegments.Add(new ScreenPoint(plotAreaRight, transformedValue));
                    }
                }

                if (axis.TickStyle != TickStyle.None)
                {
                    if (isHorizontal)
                    {
                        majorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a0));
                        majorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a1));
                    }
                    else
                    {
                        majorTickSegments.Add(new ScreenPoint(axisPosition + a0, transformedValue));
                        majorTickSegments.Add(new ScreenPoint(axisPosition + a1, transformedValue));
                    }
                }
            }

            // Render the axis labels (numbers or category names)
            foreach (double value in this.MajorLabelValues)
            {
                if (value < actualMinimum - eps || value > actualMaximum + eps)
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
                    SnapTo(plotAreaLeft, ref transformedValue);
                    SnapTo(plotAreaRight, ref transformedValue);
                }
                else
                {
                    SnapTo(plotAreaTop, ref transformedValue);
                    SnapTo(plotAreaBottom, ref transformedValue);
                }

                var pt = new ScreenPoint();
                var ha = HorizontalTextAlign.Right;
                var va = VerticalTextAlign.Middle;
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        pt = new ScreenPoint(axisPosition + a1 - axis.AxisTickToLabelDistance, transformedValue);
                        GetRotatedAlignments(
                            axis.Angle, HorizontalTextAlign.Right, VerticalTextAlign.Middle, out ha, out va);
                        break;
                    case AxisPosition.Right:
                        pt = new ScreenPoint(axisPosition + a1 + axis.AxisTickToLabelDistance, transformedValue);
                        GetRotatedAlignments(
                            axis.Angle, HorizontalTextAlign.Left, VerticalTextAlign.Middle, out ha, out va);
                        break;
                    case AxisPosition.Top:
                        pt = new ScreenPoint(transformedValue, axisPosition + a1 - axis.AxisTickToLabelDistance);
                        GetRotatedAlignments(
                            axis.Angle, HorizontalTextAlign.Center, VerticalTextAlign.Bottom, out ha, out va);
                        break;
                    case AxisPosition.Bottom:
                        pt = new ScreenPoint(transformedValue, axisPosition + a1 + axis.AxisTickToLabelDistance);
                        GetRotatedAlignments(
                            axis.Angle, HorizontalTextAlign.Center, VerticalTextAlign.Top, out ha, out va);
                        break;
                }

                string text = axis.FormatValue(value);
                this.rc.DrawMathText(
                    pt,
                    text,
                    axis.ActualTextColor,
                    axis.ActualFont,
                    axis.ActualFontSize,
                    axis.ActualFontWeight,
                    axis.Angle,
                    ha,
                    va,
                    false);
            }

            // Draw the zero crossing line
            if (axis.PositionAtZeroCrossing && this.ZeroPen != null)
            {
                double t0 = axis.Transform(0);
                if (isHorizontal)
                {
                    this.rc.DrawLine(t0, plotAreaTop, t0, plotAreaBottom, this.ZeroPen);
                }
                else
                {
                    this.rc.DrawLine(plotAreaLeft, t0, plotAreaRight, t0, this.ZeroPen);
                }
            }

            // Draw extra grid lines
            if (axis.ExtraGridlines != null && this.ExtraPen != null)
            {
                foreach (double value in axis.ExtraGridlines)
                {
                    if (!this.IsWithin(value, actualMinimum, actualMaximum))
                    {
                        continue;
                    }

                    double transformedValue = axis.Transform(value);
                    if (isHorizontal)
                    {
                        this.rc.DrawLine(transformedValue, plotAreaTop, transformedValue, plotAreaBottom, this.ExtraPen);
                    }
                    else
                    {
                        this.rc.DrawLine(plotAreaLeft, transformedValue, plotAreaRight, transformedValue, this.ExtraPen);
                    }
                }
            }

            // Draw the axis line (across the tick marks)
            if (isHorizontal)
            {
                this.rc.DrawLine(
                    axis.Transform(actualMinimum),
                    axisPosition,
                    axis.Transform(actualMaximum),
                    axisPosition,
                    this.AxislinePen);
            }
            else
            {
                this.rc.DrawLine(
                    axisPosition,
                    axis.Transform(actualMinimum),
                    axisPosition,
                    axis.Transform(actualMaximum),
                    this.AxislinePen);
            }

            // Draw the axis title
            if (!string.IsNullOrEmpty(axis.ActualTitle))
            {
                double ymid = isHorizontal
                                  ? Lerp(axis.ScreenMin.X, axis.ScreenMax.X, axis.TitlePosition)
                                  : Lerp(axis.ScreenMax.Y, axis.ScreenMin.Y, axis.TitlePosition);

                double angle = -90;
                var lpt = new ScreenPoint();

                var halign = HorizontalTextAlign.Center;
                var valign = VerticalTextAlign.Top;

                if (axis.PositionAtZeroCrossing)
                {
                    ymid = perpendicularAxis.Transform(perpendicularAxis.ActualMaximum);
                }

                OxySize? maxSize = null;

                if (axis.ClipTitle)
                {
                    // Calculate the title clipping dimensions
                    double screenLength = isHorizontal
                                              ? Math.Abs(axis.ScreenMax.X - axis.ScreenMin.X)
                                              : Math.Abs(axis.ScreenMax.Y - axis.ScreenMin.Y);

                    maxSize = new OxySize(screenLength * axis.TitleClippingLength, double.MaxValue);
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

                this.rc.SetToolTip(axis.ToolTip);
                this.rc.DrawText(
                    lpt,
                    axis.ActualTitle,
                    axis.ActualTitleColor,
                    axis.ActualTitleFont,
                    axis.ActualTitleFontSize,
                    axis.ActualTitleFontWeight,
                    angle,
                    halign,
                    valign,
                    maxSize);
                this.rc.SetToolTip(null);
            }

            // Draw all the line segments);
            if (this.MinorPen != null)
            {
                this.rc.DrawLineSegments(minorSegments, this.MinorPen);
            }

            if (this.MajorPen != null)
            {
                this.rc.DrawLineSegments(majorSegments, this.MajorPen);
            }

            if (this.MinorTickPen != null)
            {
                this.rc.DrawLineSegments(minorTickSegments, this.MinorTickPen);
            }

            if (this.MajorTickPen != null)
            {
                this.rc.DrawLineSegments(majorTickSegments, this.MajorTickPen);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the rotated alignments given the specified angle.
        /// </summary>
        /// <param name="angle">
        /// The angle.
        /// </param>
        /// <param name="defaultHorizontalAlignment">
        /// The default horizontal alignment.
        /// </param>
        /// <param name="defaultVerticalAlignment">
        /// The default vertical alignment.
        /// </param>
        /// <param name="ha">
        /// The rotated horizontal alignment.
        /// </param>
        /// <param name="va">
        /// The rotated vertical alignment.
        /// </param>
        private static void GetRotatedAlignments(
            double angle,
            HorizontalTextAlign defaultHorizontalAlignment,
            VerticalTextAlign defaultVerticalAlignment,
            out HorizontalTextAlign ha,
            out VerticalTextAlign va)
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
            }
        }

        /// <summary>
        /// Linear interpolation
        ///   http://en.wikipedia.org/wiki/Linear_interpolation
        /// </summary>
        /// <param name="x0">
        /// The x0.
        /// </param>
        /// <param name="x1">
        /// The x1.
        /// </param>
        /// <param name="f">
        /// The f.
        /// </param>
        /// <returns>
        /// The lerp.
        /// </returns>
        private static double Lerp(double x0, double x1, double f)
        {
            return (x0 * (1 - f)) + (x1 * f);
        }

        /// <summary>
        /// Snaps v to value if is within a distance of eps.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="v">
        /// The v.
        /// </param>
        /// <param name="eps">
        /// The eps.
        /// </param>
        private static void SnapTo(double value, ref double v, double eps = 0.5)
        {
            if (v > value - eps && v < value + eps)
            {
                v = value;
            }
        }

        #endregion
    }
}