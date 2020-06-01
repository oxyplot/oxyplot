// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxisFullPlotAreaRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to render with the plot area filled completely <see cref="AngleAxis" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Linq;

    /// <summary>
    /// Provides functionality to render <see cref="AngleAxis" /> using the full plot area.
    /// </summary>
    public class AngleAxisFullPlotAreaRenderer : AxisRendererBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AngleAxisFullPlotAreaRenderer" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public AngleAxisFullPlotAreaRenderer(IRenderContext rc, PlotModel plot)
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
                            .Select(x => this.TransformToClientRectangle(magnitudeAxis.ActualMaximum, x, axis, this.Plot.PlotArea, magnitudeAxis.MidPoint));

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
                                .Select(x => this.TransformToClientRectangle(magnitudeAxis.ActualMaximum, x, axis, this.Plot.PlotArea, magnitudeAxis.MidPoint));

                foreach (var point in screenPoints)
                {
                    this.RenderContext.DrawLine(magnitudeAxis.MidPoint.x, magnitudeAxis.MidPoint.y, point.x, point.y, this.MajorPen, axis.EdgeRenderingMode);
                }
            }

            //Text rendering
            foreach (var value in this.MajorLabelValues.Take(majorTickCount))
            {
                ScreenPoint pt = TransformToClientRectangle(magnitudeAxis.ActualMaximum, value, axis, this.Plot.PlotArea, magnitudeAxis.MidPoint);

                var angle = Math.Atan2(pt.y - magnitudeAxis.MidPoint.y, pt.x - magnitudeAxis.MidPoint.x);

                double degree = Math.PI / 180d;
                // Convert to degrees
                angle /= degree;

                var text = axis.FormatValue(value);

                var ha = HorizontalAlignment.Center;
                var va = VerticalAlignment.Middle;
                OxyRect plotrect = this.Plot.PlotArea;

                //check on which side of the plotarea it is
                //top
                if (pt.Y <= plotrect.Top)
                {
                    ha = HorizontalAlignment.Center;
                    va = VerticalAlignment.Top;
                    // add some margin
                    pt.y += axis.AxisTickToLabelDistance;
                }
                //bottom
                else if (pt.Y >= plotrect.Bottom)
                {
                    ha = HorizontalAlignment.Center;
                    va = VerticalAlignment.Bottom;
                    pt.y -= axis.AxisTickToLabelDistance;
                }
                //left
                else if (pt.X <= plotrect.Left)
                {
                    ha = HorizontalAlignment.Left;
                    va = VerticalAlignment.Middle;
                    pt.x += axis.AxisTickToLabelDistance;
                }
                //right
                else if (pt.X >= plotrect.Right)
                {
                    ha = HorizontalAlignment.Right;
                    va = VerticalAlignment.Middle;
                    pt.x -= axis.AxisTickToLabelDistance;
                }
                else
                {

                }

                if (Math.Abs(Math.Abs(angle) - 90) < 10)
                {
                    //ha = HorizontalAlignment.Center;
                    //va = angle >= 90 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
                    angle = 0;
                }
                else if (angle > 90 || angle < -90)
                {
                    angle -= 180;
                    //ha = HorizontalAlignment.Right;
                    //va = VerticalAlignment.Middle;
                }

                ScreenPoint outsideposition = pt;
                this.RenderContext.DrawMathText(
                    outsideposition, text, axis.ActualTextColor, axis.ActualFont, axis.ActualFontSize, axis.ActualFontWeight, 0, ha, va);
            }
        }

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="actualMaximum"></param>
        /// <param name="x"></param>
        /// <param name="axis"></param>
        /// <param name="plotArea"></param>
        /// <param name="midPoint"></param>
        /// <returns></returns>
        public ScreenPoint TransformToClientRectangle(double actualMaximum, double x, Axis axis, OxyRect plotArea, ScreenPoint midPoint)
        {
            ScreenPoint result = new ScreenPoint();
            //I think the key is to NOT compute the axis scaled value of the angle, BUT to just draw it from the Midpoint to the end of the PlotView
            //For each MinorTickValue, compute an intersection point within the client area
            double width_to_height = plotArea.Width / plotArea.Height;


            double theta = (x - axis.Offset) * axis.Scale;
            theta %= 360.0d;
            double theta_rad = theta / 180.0d * Math.PI;

            double _x = Math.Cos(theta_rad);
            //y is negative because it is from top to bottom
            double _y = -Math.Sin(theta_rad);
            //compute intersections with right or lefth side

            if (_x != 0)
            {
                double delta_x = 0;
                if (_x > 0)
                    delta_x = plotArea.Right - midPoint.X;
                else if (_x < 0)
                    delta_x = plotArea.Left - midPoint.X;

                double x_portion = delta_x / _x;
                double lineend_x = x_portion * _x;
                double lineend_y = x_portion * _y;
                if (lineend_y + midPoint.Y > plotArea.Bottom || lineend_y + midPoint.Y < plotArea.Top)
                {
                    double delta_y = 0;
                    if (_y > 0)
                        delta_y = plotArea.Bottom - midPoint.Y;
                    else
                        delta_y = plotArea.Top - midPoint.Y;

                    double y_portion = delta_y / _y;
                    lineend_x = y_portion * _x;
                    lineend_y = y_portion * _y;
                    result = new ScreenPoint((y_portion * _x) + midPoint.X, (y_portion * _y) + midPoint.Y);
                }
                else
                    result = new ScreenPoint(lineend_x + midPoint.X, lineend_y + midPoint.Y);
            }
            return result;
        }
    }
}
