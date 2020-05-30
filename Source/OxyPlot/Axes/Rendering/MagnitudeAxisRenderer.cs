// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxisRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to render <see cref="MagnitudeAxis" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides functionality to render <see cref="MagnitudeAxis" />.
    /// </summary>
    public class MagnitudeAxisRenderer : AxisRendererBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MagnitudeAxisRenderer" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public MagnitudeAxisRenderer(IRenderContext rc, PlotModel plot)
            : base(rc, plot)
        {
        }

        /// <summary>
        /// Renders the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="pass">The pass.</param>
        /// <exception cref="System.NullReferenceException">Angle axis should not be <c>null</c>.</exception>
        public override void Render(Axis axis, int pass)
        {
            base.Render(axis, pass);

            var angleAxis = this.Plot.DefaultAngleAxis;

            if (angleAxis == null)
            {
                throw new NullReferenceException("Angle axis should not be null.");
            }

            angleAxis.UpdateActualMaxMin();

            if (pass == 0 && this.ExtraPen != null)
            {
                var extraTicks = axis.ExtraGridlines;
                if (extraTicks != null)
                {
                    for (int i = 0; i < extraTicks.Length; i++)
                    {
                        this.RenderTick(axis, angleAxis, extraTicks[i], this.ExtraPen);
                    }
                }
            }

            if (pass == 0 && this.MinorPen != null)
            {
                foreach (var tickValue in this.MinorTickValues)
                {
                    this.RenderTick(axis, angleAxis, tickValue, this.MinorPen);
                }
            }

            if (pass == 0 && this.MajorPen != null)
            {
                foreach (var tickValue in this.MajorTickValues)
                {
                    this.RenderTick(axis, angleAxis, tickValue, this.MajorPen);
                }
            }

            if (pass == 1)
            {
                foreach (var tickValue in this.MajorTickValues)
                {
                    this.RenderTickText(axis, tickValue, angleAxis);
                }
            }
        }

        /// <summary>
        /// Returns the angle (in radian) of the axis line in screen coordinate
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="angleAxis">The angle axis.</param>
        /// <returns>The angle (in radians).</returns>
        private static double GetActualAngle(Axis axis, Axis angleAxis)
        {
            var a = axis.Transform(0, angleAxis.Angle, angleAxis);
            var b = axis.Transform(1, angleAxis.Angle, angleAxis);
            return Math.Atan2(b.y - a.y, b.x - a.x);
        }

        /// <summary>
        /// Choose the most appropriate alignment for tick text
        /// </summary>
        /// <param name="actualAngle">The actual angle.</param>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        private static void GetTickTextAligment(double actualAngle, out HorizontalAlignment ha, out VerticalAlignment va)
        {
            if (actualAngle > 3 * Math.PI / 4 || actualAngle < -3 * Math.PI / 4)
            {
                ha = HorizontalAlignment.Center;
                va = VerticalAlignment.Top;
            }
            else if (actualAngle < -Math.PI / 4)
            {
                ha = HorizontalAlignment.Right;
                va = VerticalAlignment.Middle;
            }
            else if (actualAngle > Math.PI / 4)
            {
                ha = HorizontalAlignment.Left;
                va = VerticalAlignment.Middle;
            }
            else
            {
                ha = HorizontalAlignment.Center;
                va = VerticalAlignment.Bottom;
            }
        }

        /// <summary>
        /// Renders a tick, chooses the best implementation
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="angleAxis">The angle axis.</param>
        /// <param name="x">The x-value.</param>
        /// <param name="pen">The pen.</param>
        private void RenderTick(Axis axis, AngleAxis angleAxis, double x, OxyPen pen)
        {
            var isFullCircle = Math.Abs(Math.Abs(angleAxis.EndAngle - angleAxis.StartAngle) - 360) < 1e-6;

            if (isFullCircle && pen.ActualDashArray == null)
            {
                this.RenderTickCircle(axis, angleAxis, x, pen);
            }
            else
            {
                this.RenderTickArc(axis, angleAxis, x, pen);
            }
        }

        /// <summary>
        /// Renders a tick by drawing an ellipse
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="angleAxis">The angle axis.</param>
        /// <param name="x">The x-value.</param>
        /// <param name="pen">The pen.</param>
        private void RenderTickCircle(Axis axis, Axis angleAxis, double x, OxyPen pen)
        {
            var zero = angleAxis.Offset;
            var center = axis.Transform(axis.ActualMinimum, zero, angleAxis);
            var right = axis.Transform(x, zero, angleAxis).X;
            var radius = right - center.X;
            var width = radius * 2;
            var left = right - width;
            var top = center.Y - radius;
            var height = width;

            this.RenderContext.DrawEllipse(new OxyRect(left, top, width, height), OxyColors.Undefined, pen.Color, pen.Thickness, axis.EdgeRenderingMode);
        }

        /// <summary>
        /// Renders a tick by drawing an lot of segments
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="angleAxis">The angle axis.</param>
        /// <param name="x">The x-value.</param>
        /// <param name="pen">The pen.</param>
        private void RenderTickArc(Axis axis, AngleAxis angleAxis, double x, OxyPen pen)
        {
            // caution: make sure angleAxis.UpdateActualMaxMin(); has been called
            var minAngle = angleAxis.ActualMinimum;
            var maxAngle = angleAxis.ActualMaximum;

            // number of segment to draw a full circle
            // - decrease if you want get more speed
            // - increase if you want more detail
            // (making a public property of it would be a great idea)
            const double MaxSegments = 90.0;

            // compute the actual number of segments
            var segmentCount = (int)(MaxSegments * Math.Abs(angleAxis.EndAngle - angleAxis.StartAngle) / 360.0);

            var angleStep = (maxAngle - minAngle) / (segmentCount - 1);

            var points = new List<ScreenPoint>();

            for (var i = 0; i < segmentCount; i++)
            {
                var angle = minAngle + (i * angleStep);
                points.Add(axis.Transform(x, angle, angleAxis));
            }

            this.RenderContext.DrawLine(points, pen.Color, pen.Thickness, axis.EdgeRenderingMode, pen.ActualDashArray);
        }

        /// <summary>
        /// Renders major tick text
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="x">The x-value.</param>
        /// <param name="angleAxis">The angle axis.</param>
        private void RenderTickText(Axis axis, double x, Axis angleAxis)
        {
            var actualAngle = GetActualAngle(axis, angleAxis);
            var dx = axis.AxisTickToLabelDistance * Math.Sin(actualAngle);
            var dy = -axis.AxisTickToLabelDistance * Math.Cos(actualAngle);

            HorizontalAlignment ha;
            VerticalAlignment va;
            GetTickTextAligment(actualAngle, out ha, out va);

            var pt = axis.Transform(x, angleAxis.Angle, angleAxis);
            pt = new ScreenPoint(pt.X + dx, pt.Y + dy);

            string text = axis.FormatValue(x);
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
    }
}
