// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HorizontalAndVerticalAxisRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to render horizontal and vertical axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Provides functionality to render horizontal and vertical axes.
    /// </summary>
    public class HorizontalAndVerticalAxisRenderer : AxisRendererBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalAndVerticalAxisRenderer" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public HorizontalAndVerticalAxisRenderer(IRenderContext rc, PlotModel plot)
            : base(rc, plot)
        {
        }

        /// <summary>
        /// Renders the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="pass">The pass.</param>
        public override void Render(Axis axis, int pass)
        {
            base.Render(axis, pass);

            bool drawAxisLine = true;
            double totalShift = axis.AxisDistance + axis.PositionTierMinShift;
            double tierSize = axis.PositionTierSize - this.Plot.AxisTierDistance;

            // store properties locally for performance
            double plotAreaLeft = this.Plot.PlotArea.Left;
            double plotAreaRight = this.Plot.PlotArea.Right;
            double plotAreaTop = this.Plot.PlotArea.Top;
            double plotAreaBottom = this.Plot.PlotArea.Bottom;

            // Axis position (x or y screen coordinate)
            double axisPosition = 0;
            double titlePosition = 0;

            switch (axis.Position)
            {
                case AxisPosition.Left:
                    axisPosition = plotAreaLeft - totalShift;
                    break;
                case AxisPosition.Right:
                    axisPosition = plotAreaRight + totalShift;
                    break;
                case AxisPosition.Top:
                    axisPosition = plotAreaTop - totalShift;
                    break;
                case AxisPosition.Bottom:
                    axisPosition = plotAreaBottom + totalShift;
                    break;
            }

            if (axis.PositionAtZeroCrossing)
            {
                var perpendicularAxis = axis.IsHorizontal() ? this.Plot.DefaultYAxis : this.Plot.DefaultXAxis;

                // the axis should be positioned at the origin of the perpendicular axis
                axisPosition = perpendicularAxis.Transform(0);

                var p0 = axis.IsHorizontal()
                    ? perpendicularAxis.ScreenMin.X
                    : perpendicularAxis.ScreenMin.Y;
                var p1 = axis.IsHorizontal()
                    ? perpendicularAxis.ScreenMax.X
                    : perpendicularAxis.ScreenMax.Y;

                // find the min/max positions
                var min = Math.Min(p0, p1);
                var max = Math.Max(p0, p1);

                // also consider the plot area
                var areaMin = axis.IsHorizontal() ? plotAreaTop : plotAreaLeft;
                var areaMax = axis.IsHorizontal() ? plotAreaBottom : plotAreaRight;
                min = Math.Max(min, areaMin);
                max = Math.Min(max, areaMax);

                if (axisPosition < min)
                {
                    axisPosition = min;

                    var borderThickness = axis.IsHorizontal()
                        ? this.Plot.PlotAreaBorderThickness.Top
                        : this.Plot.PlotAreaBorderThickness.Left;
                    var borderPosition = axis.IsHorizontal()
                        ? this.Plot.PlotArea.Top
                        : this.Plot.PlotArea.Left;
                    if (axisPosition <= borderPosition && borderThickness > 0 && this.Plot.PlotAreaBorderColor.IsVisible())
                    {
                        // there is already a line here...
                        drawAxisLine = false;
                    }
                }

                if (axisPosition > max)
                {
                    axisPosition = max;

                    var borderThickness = axis.IsHorizontal()
                        ? this.Plot.PlotAreaBorderThickness.Bottom
                        : this.Plot.PlotAreaBorderThickness.Right;
                    var borderPosition = axis.IsHorizontal()
                        ? this.Plot.PlotArea.Bottom
                        : this.Plot.PlotArea.Right;
                    if (axisPosition >= borderPosition && borderThickness > 0 && this.Plot.PlotAreaBorderColor.IsVisible())
                    {
                        // there is already a line here...
                        drawAxisLine = false;
                    }
                }
            }

            switch (axis.Position)
            {
                case AxisPosition.Left:
                    titlePosition = axisPosition - tierSize;
                    break;
                case AxisPosition.Right:
                    titlePosition = axisPosition + tierSize;
                    break;
                case AxisPosition.Top:
                    titlePosition = axisPosition - tierSize;
                    break;
                case AxisPosition.Bottom:
                    titlePosition = axisPosition + tierSize;
                    break;
            }

            switch (axis.Position)
            {
                case AxisPosition.Left:
                case AxisPosition.Top:
                    titlePosition += axis.AxisDistance;
                    break;
                case AxisPosition.Right:
                case AxisPosition.Bottom:
                    titlePosition -= axis.AxisDistance;
                    break;
            }

            if (pass == 0)
            {
                this.RenderMinorItems(axis, axisPosition);
            }

            if (pass == 1)
            {
                this.RenderMajorItems(axis, axisPosition, titlePosition, drawAxisLine);
                this.RenderAxisTitle(axis, titlePosition);
            }
        }

        /// <summary>
        /// Interpolates linearly between two values.
        /// </summary>
        /// <param name="x0">The x0.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="f">The interpolation factor.</param>
        /// <returns>The interpolated value.</returns>
        protected static double Lerp(double x0, double x1, double f)
        {
            // http://en.wikipedia.org/wiki/Linear_interpolation
            return (x0 * (1 - f)) + (x1 * f);
        }

        /// <summary>
        /// Snaps v to value if it is within the specified distance.
        /// </summary>
        /// <param name="target">The target value.</param>
        /// <param name="v">The value to snap.</param>
        /// <param name="eps">The distance tolerance.</param>
        protected static void SnapTo(double target, ref double v, double eps = 0.5)
        {
            if (v > target - eps && v < target + eps)
            {
                v = target;
            }
        }

        /// <summary>
        /// Gets the axis title position, rotation and alignment.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="titlePosition">The title position.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <returns>The <see cref="ScreenPoint" />.</returns>
        protected virtual ScreenPoint GetAxisTitlePositionAndAlignment(
            Axis axis,
            double titlePosition,
            ref double angle,
            ref HorizontalAlignment halign,
            ref VerticalAlignment valign)
        {
            double middle = axis.IsHorizontal()
                                ? Lerp(axis.ScreenMin.X, axis.ScreenMax.X, axis.TitlePosition)
                                : Lerp(axis.ScreenMax.Y, axis.ScreenMin.Y, axis.TitlePosition);

            if (axis.PositionAtZeroCrossing)
            {
                middle = Lerp(axis.Transform(axis.ActualMaximum), axis.Transform(axis.ActualMinimum), axis.TitlePosition);
            }

            switch (axis.Position)
            {
                case AxisPosition.Left:
                    return new ScreenPoint(titlePosition, middle);
                case AxisPosition.Right:
                    valign = VerticalAlignment.Bottom;
                    return new ScreenPoint(titlePosition, middle);
                case AxisPosition.Top:
                    halign = HorizontalAlignment.Center;
                    valign = VerticalAlignment.Top;
                    angle = 0;
                    return new ScreenPoint(middle, titlePosition);
                case AxisPosition.Bottom:
                    halign = HorizontalAlignment.Center;
                    valign = VerticalAlignment.Bottom;
                    angle = 0;
                    return new ScreenPoint(middle, titlePosition);
                default:
                    throw new ArgumentOutOfRangeException("axis");
            }
        }

        /// <summary>
        /// Renders the axis title.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="titlePosition">The title position.</param>
        protected virtual void RenderAxisTitle(Axis axis, double titlePosition)
        {
            if (string.IsNullOrEmpty(axis.ActualTitle))
            {
                return;
            }

            bool isHorizontal = axis.IsHorizontal();

            OxySize? maxSize = null;

            if (axis.ClipTitle)
            {
                // Calculate the title clipping dimensions
                double screenLength = isHorizontal
                                          ? Math.Abs(axis.ScreenMax.X - axis.ScreenMin.X)
                                          : Math.Abs(axis.ScreenMax.Y - axis.ScreenMin.Y);

                maxSize = new OxySize(screenLength * axis.TitleClippingLength, double.MaxValue);
            }

            double angle = -90;

            var halign = HorizontalAlignment.Center;
            var valign = VerticalAlignment.Top;

            var lpt = this.GetAxisTitlePositionAndAlignment(axis, titlePosition, ref angle, ref halign, ref valign);

            this.RenderContext.DrawMathText(
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
        }

        /// <summary>
        /// Renders the major items.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="axisPosition">The axis position.</param>
        /// <param name="titlePosition">The title position.</param>
        /// <param name="drawAxisLine">Draw the axis line if set to <c>true</c>.</param>
        protected virtual void RenderMajorItems(Axis axis, double axisPosition, double titlePosition, bool drawAxisLine)
        {
            double eps = axis.ActualMinorStep * 1e-3;

            double actualMinimum = axis.ActualMinimum;
            double actualMaximum = axis.ActualMaximum;

            double plotAreaLeft = this.Plot.PlotArea.Left;
            double plotAreaRight = this.Plot.PlotArea.Right;
            double plotAreaTop = this.Plot.PlotArea.Top;
            double plotAreaBottom = this.Plot.PlotArea.Bottom;
            bool isHorizontal = axis.IsHorizontal();
            bool cropGridlines = axis.CropGridlines;

            double a0;
            double a1;
            var majorSegments = new List<ScreenPoint>();
            var majorTickSegments = new List<ScreenPoint>();
            this.GetTickPositions(axis, axis.TickStyle, axis.MajorTickSize, axis.Position, out a0, out a1);

            var perpendicularAxis = axis.IsHorizontal() ? this.Plot.DefaultYAxis : this.Plot.DefaultXAxis;
            var dontRenderZero = axis.PositionAtZeroCrossing && perpendicularAxis.PositionAtZeroCrossing;

            List<Axis> perpAxes = null;
            if (cropGridlines)
            {
                if (isHorizontal)
                {
                    perpAxes = this.Plot.Axes.Where(x => x.IsXyAxis() && x.IsVertical()).ToList();
                }
                else
                {
                    perpAxes = this.Plot.Axes.Where(x => x.IsXyAxis() && x.IsHorizontal()).ToList();
                }
            }

            foreach (double value in this.MajorTickValues)
            {
                if (dontRenderZero && Math.Abs(value) < eps)
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
                    this.AddSegments(
                        majorSegments, 
                        perpAxes, 
                        isHorizontal, 
                        cropGridlines, 
                        transformedValue, 
                        plotAreaLeft, 
                        plotAreaRight, 
                        plotAreaTop, 
                        plotAreaBottom);
                }

                if (axis.TickStyle != TickStyle.None && axis.MajorTickSize > 0)
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

                if (dontRenderZero && Math.Abs(value) < eps)
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
                var ha = HorizontalAlignment.Right;
                var va = VerticalAlignment.Middle;
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        pt = new ScreenPoint(axisPosition + a1 - axis.AxisTickToLabelDistance, transformedValue);
                        this.GetRotatedAlignments(axis.Angle, -90, out ha, out va);

                        break;
                    case AxisPosition.Right:
                        pt = new ScreenPoint(axisPosition + a1 + axis.AxisTickToLabelDistance, transformedValue);
                        this.GetRotatedAlignments(axis.Angle, 90, out ha, out va);

                        break;
                    case AxisPosition.Top:
                        pt = new ScreenPoint(transformedValue, axisPosition + a1 - axis.AxisTickToLabelDistance);
                        this.GetRotatedAlignments(axis.Angle, 0, out ha, out va);

                        break;
                    case AxisPosition.Bottom:
                        pt = new ScreenPoint(transformedValue, axisPosition + a1 + axis.AxisTickToLabelDistance);
                        this.GetRotatedAlignments(axis.Angle, -180, out ha, out va);

                        break;
                }

                string text = axis.FormatValue(value);
                this.RenderContext.DrawMathText(
                    pt,
                    text,
                    axis.ActualTextColor,
                    axis.ActualFont,
                    axis.ActualFontSize,
                    axis.ActualFontWeight,
                    axis.Angle,
                    ha,
                    va);
            }

            // Draw extra grid lines
            if (axis.ExtraGridlines != null && this.ExtraPen != null)
            {
                var extraSegments = new List<ScreenPoint>();

                foreach (double value in axis.ExtraGridlines)
                {
                    if (!this.IsWithin(value, actualMinimum, actualMaximum))
                    {
                        continue;
                    }

                    double transformedValue = axis.Transform(value);
                    this.AddSegments(
                        extraSegments,
                        perpAxes,
                        isHorizontal,
                        cropGridlines,
                        transformedValue,
                        plotAreaLeft,
                        plotAreaRight,
                        plotAreaTop,
                        plotAreaBottom);
                }

                this.RenderContext.DrawLineSegments(extraSegments, this.ExtraPen, axis.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
            }

            if (drawAxisLine)
            {
                // Draw the axis line (across the tick marks)
                if (isHorizontal)
                {
                    this.RenderContext.DrawLine(
                        axis.Transform(actualMinimum),
                        axisPosition,
                        axis.Transform(actualMaximum),
                        axisPosition,
                        this.AxislinePen,
                        axis.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
                }
                else
                {
                    this.RenderContext.DrawLine(
                        axisPosition,
                        axis.Transform(actualMinimum),
                        axisPosition,
                        axis.Transform(actualMaximum),
                        this.AxislinePen,
                        axis.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
                }
            }

            if (this.MajorPen != null)
            {
                this.RenderContext.DrawLineSegments(majorSegments, this.MajorPen, axis.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
            }

            if (this.MajorTickPen != null)
            {
                this.RenderContext.DrawLineSegments(majorTickSegments, this.MajorTickPen, axis.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
            }
        }

        /// <summary>
        /// Renders the minor items.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="axisPosition">The axis position.</param>
        protected virtual void RenderMinorItems(Axis axis, double axisPosition)
        {
            double eps = axis.ActualMinorStep * 1e-3;
            double actualMinimum = axis.ActualMinimum;
            double actualMaximum = axis.ActualMaximum;

            double plotAreaLeft = this.Plot.PlotArea.Left;
            double plotAreaRight = this.Plot.PlotArea.Right;
            double plotAreaTop = this.Plot.PlotArea.Top;
            double plotAreaBottom = this.Plot.PlotArea.Bottom;
            bool cropGridlines = axis.CropGridlines;
            bool isHorizontal = axis.IsHorizontal();

            double a0;
            double a1;
            var minorSegments = new List<ScreenPoint>();
            var minorTickSegments = new List<ScreenPoint>();

            List<Axis> perpAxes = null;
            if (cropGridlines)
            {
                if (isHorizontal)
                {
                    perpAxes = this.Plot.Axes.Where(x => x.IsXyAxis() && x.IsVertical()).ToList();
                }
                else
                {
                    perpAxes = this.Plot.Axes.Where(x => x.IsXyAxis() && x.IsHorizontal()).ToList();
                }
            }

            this.GetTickPositions(axis, axis.TickStyle, axis.MinorTickSize, axis.Position, out a0, out a1);

            foreach (double value in this.MinorTickValues)
            {
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
                    this.AddSegments(
                        minorSegments,
                        perpAxes,
                        isHorizontal,
                        cropGridlines,
                        transformedValue,
                        plotAreaLeft,
                        plotAreaRight,
                        plotAreaTop,
                        plotAreaBottom);
                }

                // Draw the minor tick
                if (axis.TickStyle != TickStyle.None && axis.MinorTickSize > 0)
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

            // Draw all the line segments);
            if (this.MinorPen != null)
            {
                this.RenderContext.DrawLineSegments(minorSegments, this.MinorPen, axis.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
            }

            if (this.MinorTickPen != null)
            {
                this.RenderContext.DrawLineSegments(minorTickSegments, this.MinorTickPen, axis.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
            }
        }

        /// <summary>
        /// Adds segments to <paramref name="segments"/> array. 
        /// If <paramref name="cropGridlines"/> is true, then lines will be cropped with <paramref name="perpAxes"/> lists axes.
        /// </summary>
        /// <param name="segments">The target segments.</param>
        /// <param name="perpAxes">Perpendicular axes list.</param>
        /// <param name="isHorizontal">True, if current axis is horizontal.</param>
        /// <param name="cropGridlines">True, if gridlines should be cropped.</param>
        /// <param name="transformedValue">Starting point position.</param>
        /// <param name="plotAreaLeft">Plot area left position.</param>
        /// <param name="plotAreaRight">Plot area right position.</param>
        /// <param name="plotAreaTop">Plot area top position.</param>
        /// <param name="plotAreaBottom">Plot area bottom position.</param>
        private void AddSegments(
            List<ScreenPoint> segments, 
            List<Axis> perpAxes,
            bool isHorizontal,
            bool cropGridlines,
            double transformedValue,
            double plotAreaLeft,
            double plotAreaRight,
            double plotAreaTop,
            double plotAreaBottom)
        {
            if (isHorizontal)
            {
                if (!cropGridlines)
                {
                    segments.Add(new ScreenPoint(transformedValue, plotAreaTop));
                    segments.Add(new ScreenPoint(transformedValue, plotAreaBottom));
                }
                else
                {
                    foreach (var perpAxis in perpAxes)
                    {
                        segments.Add(new ScreenPoint(transformedValue, perpAxis.Transform(perpAxis.ActualMinimum)));
                        segments.Add(new ScreenPoint(transformedValue, perpAxis.Transform(perpAxis.ActualMaximum)));
                    }
                }
            }
            else
            {
                if (!cropGridlines)
                {
                    segments.Add(new ScreenPoint(plotAreaLeft, transformedValue));
                    segments.Add(new ScreenPoint(plotAreaRight, transformedValue));
                }
                else
                {
                    foreach (var perpAxis in perpAxes)
                    {
                        segments.Add(new ScreenPoint(perpAxis.Transform(perpAxis.ActualMinimum), transformedValue));
                        segments.Add(new ScreenPoint(perpAxis.Transform(perpAxis.ActualMaximum), transformedValue));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the alignments given the specified rotation angle.
        /// </summary>
        /// <param name="boxAngle">The angle of a box to rotate (usually it is label angle).</param>
        /// <param name="axisAngle">
        /// The axis angle, the original angle belongs to. The Top axis should have 0, next angles are computed clockwise. 
        /// The angle should be in [-180, 180). (T, R, B, L) is (0, 90, -180, -90). 
        /// </param>
        /// <param name="ha">Horizontal alignment.</param>
        /// <param name="va">Vertical alignment.</param>
        /// <remarks>
        /// This method is supposed to compute the alignment of the labels that are put near axis. 
        /// Because such labels can have different angles, and the axis can have different angles as well,
        /// computing the alignment is not straightforward.
        /// </remarks>
        private void GetRotatedAlignments(
            double boxAngle,
            double axisAngle,
            out HorizontalAlignment ha,
            out VerticalAlignment va)
        {
            const double AngleTolerance = 10.0;

            Debug.Assert(new[] { 0.0, 90.0, -180.0, -90.0 }.Contains(axisAngle), "The axis angles should be one of 0, 90, -180, -90");

            // The axis angle if it would have been turned on 180 and leave it in [-180, 180)
            double flippedAxisAngle = ((axisAngle + 360.0) % 360.0) - 180.0;

            // When the box (assuming the axis and box have the same angle) box starts to turn clockwise near the axis
            // It leans on the right until it gets to 180 rotation, when it is started to lean on the left.
            // In real computation we need to compute this in relation with axisAngle
            // So if axisAngle <= boxAngle < (axisAngle + 180), we align Right, else - left.
            // The check looks inverted because flippedAxisAngle has the opposite sign.
            ha = boxAngle >= Math.Min(axisAngle, flippedAxisAngle) && boxAngle < Math.Max(axisAngle, flippedAxisAngle) ? HorizontalAlignment.Left : HorizontalAlignment.Right;

            // If axisAngle was < 0, we need to shift the previous computation on 180.
            if (axisAngle < 0)
            {
                ha = (HorizontalAlignment)((int)ha * -1);
            }

            va = VerticalAlignment.Middle;

            // If the angle almost the same as axisAngle (or axisAngle + 180) - set horizontal alignment to Center
            if (Math.Abs(boxAngle - flippedAxisAngle) < AngleTolerance || Math.Abs(boxAngle - axisAngle) < AngleTolerance)
            {
                ha = HorizontalAlignment.Center;
            }

            // And vertical alignment according to whether it is near to axisAngle or flippedAxisAngle
            if (Math.Abs(boxAngle - axisAngle) < AngleTolerance)
            {
                va = VerticalAlignment.Bottom;
            }

            if (Math.Abs(boxAngle - flippedAxisAngle) < AngleTolerance)
            {
                va = VerticalAlignment.Top;
            }
        }
    }
}
