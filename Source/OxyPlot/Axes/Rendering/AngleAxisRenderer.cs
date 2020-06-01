// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxisRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to render <see cref="AngleAxis" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Linq;

    /// <summary>
    /// Provides functionality to render <see cref="AngleAxis" />.
    /// </summary>
    public class AngleAxisRenderer : AxisRendererBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AngleAxisRenderer" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public AngleAxisRenderer(IRenderContext rc, PlotModel plot)
            : base(rc, plot)
        {
        }

        /// <summary>
        /// Renders the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="pass">The render pass.</param>
        /// <exception cref="System.InvalidOperationException">Magnitude axis not defined.</exception>
        public override void Render(Axis axis, int pass)
        {
            var angleAxis = (AngleAxis)axis;

            base.Render(axis, pass);

            var magnitudeAxis = this.Plot.DefaultMagnitudeAxis;

            if (magnitudeAxis == null)
            {
                throw new InvalidOperationException("Magnitude axis not defined.");
            }

            var scaledStartAngle = angleAxis.StartAngle / angleAxis.Scale;
            var scaledEndAngle = angleAxis.EndAngle / angleAxis.Scale;

            var axisLength = Math.Abs(scaledEndAngle - scaledStartAngle);
            var eps = axis.MinorStep * 1e-3;
            if (this.MinorPen != null)
            {
                var tickCount = Math.Abs((int)(axisLength / axis.ActualMinorStep));
                var screenPoints = this.MinorTickValues
                    .Take(tickCount + 1)
                    .Select(x => magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, x, axis));

                foreach (var screenPoint in screenPoints)
                {
                    this.RenderContext.DrawLine(magnitudeAxis.MidPoint.x, magnitudeAxis.MidPoint.y, screenPoint.x, screenPoint.y, this.MinorPen, axis.EdgeRenderingMode);
                }
            }

            var isFullCircle = Math.Abs(Math.Abs(Math.Max(angleAxis.EndAngle, angleAxis.StartAngle) - Math.Min(angleAxis.StartAngle, angleAxis.EndAngle)) - 360) < 1e-3;
            var majorTickCount = (int)(axisLength / axis.ActualMajorStep);
            if (!isFullCircle)
            {
                majorTickCount++;
            }

            if (this.MajorPen != null)
            {
                var screenPoints = this.MajorTickValues
                    .Take(majorTickCount)
                    .Select(x => magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, x, axis));

                foreach (var point in screenPoints)
                {
                    this.RenderContext.DrawLine(magnitudeAxis.MidPoint.x, magnitudeAxis.MidPoint.y, point.x, point.y, this.MajorPen, axis.EdgeRenderingMode);
                }
            }

            foreach (var value in this.MajorLabelValues.Take(majorTickCount))
            {
                var pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, value, axis);
                var angle = Math.Atan2(pt.y - magnitudeAxis.MidPoint.y, pt.x - magnitudeAxis.MidPoint.x);

                // add some margin
                pt.x += Math.Cos(angle) * axis.AxisTickToLabelDistance;
                pt.y += Math.Sin(angle) * axis.AxisTickToLabelDistance;

                // Convert to degrees
                angle *= 180 / Math.PI;

                var text = axis.FormatValue(value);

                var ha = HorizontalAlignment.Left;
                var va = VerticalAlignment.Middle;

                if (Math.Abs(Math.Abs(angle) - 90) < 10)
                {
                    ha = HorizontalAlignment.Center;
                    va = angle >= 90 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
                    angle = 0;
                }
                else if (angle > 90 || angle < -90)
                {
                    angle -= 180;
                    ha = HorizontalAlignment.Right;
                }

                this.RenderContext.DrawMathText(
                    pt, text, axis.ActualTextColor, axis.ActualFont, axis.ActualFontSize, axis.ActualFontWeight, angle, ha, va);
            }
        }
    }
}
