// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxisRenderer.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The magnitude axis renderer.
    /// </summary>
    public class MagnitudeAxisRenderer : AxisRendererBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MagnitudeAxisRenderer"/> class.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="plot">
        /// The plot.
        /// </param>
        public MagnitudeAxisRenderer(IRenderContext rc, PlotModel plot)
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

            var angleAxis = this.Plot.DefaultAngleAxis as Axis;
            if (axis.RelatedAxis != null)
            {
                angleAxis = axis.RelatedAxis;
            }

            if (angleAxis == null)
            {
                throw new NullReferenceException("Angle axis should not be null.");
            }

            if (axis.ShowMinorTicks)
            {
                // GetVerticalTickPositions(axis, axis.TickStyle, axis.MinorTickSize, out y0, out y1);

                foreach (double xValue in this.MinorTickValues)
                {
                    if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                    {
                        continue;
                    }

                    if (this.MajorTickValues.Contains(xValue))
                    {
                        continue;
                    }

                    var pts = new List<ScreenPoint>();
                    for (double th = angleAxis.ActualMinimum;
                         th <= angleAxis.ActualMaximum + angleAxis.MinorStep * 0.01;
                         th += angleAxis.MinorStep * 0.1)
                    {
                        pts.Add(axis.Transform(xValue, th, angleAxis));
                    }

                    if (this.MinorPen != null)
                    {
                        this.rc.DrawLine(pts, this.MinorPen.Color, this.MinorPen.Thickness, this.MinorPen.DashArray);
                    }

                    // RenderGridline(x, y + y0, x, y + y1, minorTickPen);
                }
            }

            // GetVerticalTickPositions(axis, axis.TickStyle, axis.MajorTickSize, out y0, out y1);

            foreach (double xValue in this.MajorTickValues)
            {
                if (xValue < axis.ActualMinimum || xValue > axis.ActualMaximum)
                {
                    continue;
                }

                var pts = new List<ScreenPoint>();
                for (double th = angleAxis.ActualMinimum;
                     th <= angleAxis.ActualMaximum + angleAxis.MinorStep * 0.01;
                     th += angleAxis.MinorStep * 0.1)
                {
                    pts.Add(axis.Transform(xValue, th, angleAxis));
                }

                if (this.MajorPen != null)
                {
                    this.rc.DrawLine(pts, this.MajorPen.Color, this.MajorPen.Thickness, this.MajorPen.DashArray);
                }

                // RenderGridline(x, y + y0, x, y + y1, majorTickPen);
                // var pt = new ScreenPoint(x, istop ? y + y1 - TICK_DIST : y + y1 + TICK_DIST);
                // string text = axis.FormatValue(xValue);
                // double h = rc.MeasureText(text, axis.Font, axis.FontSize, axis.FontWeight).Height;
                // rc.DrawText(pt, text, axis.LabelColor ?? plot.TextColor,
                // axis.Font, axis.FontSize, axis.FontWeight,
                // axis.Angle,
                // HorizontalTextAlign.Center, istop ? VerticalTextAlign.Bottom : VerticalTextAlign.Top);
                // maxh = Math.Max(maxh, h);
            }
        }

        #endregion
    }
}