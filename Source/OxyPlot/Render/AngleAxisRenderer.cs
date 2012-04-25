// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxisRenderer.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// The angle axis renderer.
    /// </summary>
    public class AngleAxisRenderer : AxisRendererBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleAxisRenderer"/> class.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="plot">
        /// The plot.
        /// </param>
        public AngleAxisRenderer(IRenderContext rc, PlotModel plot)
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

            MagnitudeAxis magnitudeAxis = this.Plot.DefaultMagnitudeAxis;

            if (axis.RelatedAxis != null)
            {
                magnitudeAxis = axis.RelatedAxis as MagnitudeAxis;
            }

            if (magnitudeAxis == null)
            {
                throw new InvalidOperationException("Magnitude axis not defined.");
            }

            double eps = axis.MinorStep * 1e-3;

            if (axis.ShowMinorTicks)
            {
                // GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

                foreach (double xValue in this.MinorTickValues)
                {
                    if (xValue < axis.ActualMinimum - eps || xValue > axis.ActualMaximum + eps)
                    {
                        continue;
                    }

                    if (this.MajorTickValues.Contains(xValue))
                    {
                        continue;
                    }

                    ScreenPoint pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);

                    if (this.MinorPen != null)
                    {
                        this.rc.DrawLine(
                            magnitudeAxis.MidPoint.x, magnitudeAxis.MidPoint.y, pt.x, pt.y, this.MinorPen, false);
                    }

                    // RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            // GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            foreach (double xValue in this.MajorTickValues)
            {
                // skip the last value (overlapping with the first)
                if (xValue > axis.ActualMaximum - eps)
                {
                    continue;
                }

                if (xValue < axis.ActualMinimum - eps || xValue > axis.ActualMaximum + eps)
                {
                    continue;
                }

                ScreenPoint pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, xValue, axis);
                if (this.MajorPen != null)
                {
                    this.rc.DrawLine(
                        magnitudeAxis.MidPoint.x, magnitudeAxis.MidPoint.y, pt.x, pt.y, this.MajorPen, false);
                }
            }

            foreach (double value in this.MajorLabelValues)
            {
                // skip the last value (overlapping with the first)
                if (value > axis.ActualMaximum - eps)
                {
                    continue;
                }

                ScreenPoint pt = magnitudeAxis.Transform(magnitudeAxis.ActualMaximum, value, axis);
                double angle = Math.Atan2(pt.y - magnitudeAxis.MidPoint.y, pt.x - magnitudeAxis.MidPoint.x);

                // add some margin
                pt.x += Math.Cos(angle) * axis.AxisTickToLabelDistance;
                pt.y += Math.Sin(angle) * axis.AxisTickToLabelDistance;

                // Convert to degrees
                angle *= 180 / Math.PI;

                string text = axis.FormatValue(value);

                HorizontalTextAlign ha = HorizontalTextAlign.Left;
                VerticalTextAlign va = VerticalTextAlign.Middle;

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

                this.rc.DrawMathText(
                    pt, text, axis.ActualTextColor, axis.ActualFont, axis.ActualFontSize, axis.ActualFontWeight, angle, ha, va, false);
            }
        }

        #endregion
    }
}