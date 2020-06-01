// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxisFullPlotAreaRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to render <see cref="MagnitudeAxis" /> using the full plot area.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides functionality to render <see cref="MagnitudeAxis" /> using the full plot area.
    /// </summary>
    public class MagnitudeAxisFullPlotAreaRenderer : AxisRendererBase
    {
        /// <summary>
        /// constants to simplify angular calculations
        /// </summary>
        const double degree = 180.0d / Math.PI;
        const double rad = Math.PI / 180.0d;

        /// <summary>
        /// this constant limit the number of segments to draw a tick arc
        /// </summary>
        const double MaxSegments = 180.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagnitudeAxisFullPlotAreaRenderer" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public MagnitudeAxisFullPlotAreaRenderer(IRenderContext rc, PlotModel plot)
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
            var magnitudeAxis = this.Plot.DefaultMagnitudeAxis;

            if (angleAxis == null)
            {
                throw new NullReferenceException("Angle axis should not be null.");
            }

            angleAxis.UpdateActualMaxMin();

            double topdistance = Math.Abs(axis.PlotModel.PlotArea.Top - magnitudeAxis.MidPoint.Y);
            double bottomdistance = Math.Abs(axis.PlotModel.PlotArea.Bottom - magnitudeAxis.MidPoint.Y);
            double leftdistance = Math.Abs(axis.PlotModel.PlotArea.Left - magnitudeAxis.MidPoint.X);
            double rightdistance = Math.Abs(axis.PlotModel.PlotArea.Right - magnitudeAxis.MidPoint.X);
            OxyRect distancerect = axis.PlotModel.PlotArea.Offset(-magnitudeAxis.MidPoint.X, -magnitudeAxis.MidPoint.Y);

            double cornerangle_topright = -degree * Math.Atan2(distancerect.Top, distancerect.Right);
            double cornerangle_topleft = -degree * Math.Atan2(distancerect.Top, distancerect.Left);
            double cornerangle_bottomleft = 360-degree * Math.Atan2(distancerect.Bottom, distancerect.Left);
            double cornerangle_bottomright = 360-degree * Math.Atan2(distancerect.Bottom, distancerect.Right);

            // detect and filter dodgy values caused by zero values
            if (cornerangle_topleft < 0)
                cornerangle_topleft += 360;
            if (cornerangle_bottomleft > 360)
                cornerangle_bottomleft -= 360;

            double cornerdistance_topright = Math.Sqrt(Math.Pow(distancerect.Top, 2) + Math.Pow(distancerect.Right, 2));
            double cornerdistance_topleft = Math.Sqrt(Math.Pow(distancerect.Top, 2) + Math.Pow(distancerect.Left, 2));
            double cornerdistance_bottomleft = Math.Sqrt(Math.Pow(distancerect.Bottom, 2) + Math.Pow(distancerect.Left, 2));
            double cornerdistance_bottomright = Math.Sqrt(Math.Pow(distancerect.Bottom, 2) + Math.Pow(distancerect.Right, 2));

            if (pass == 0 && this.MinorPen != null)
            {
                OxyPen pen = this.MinorPen;

                foreach (var tickValue in this.MinorTickValues)
                {
                    //a circle consists - in this case - of 4 arcs
                    //the start and end of each arc has to be computed

                    double r = axis.Transform(tickValue);

                    //this works by putting the limits of the plotarea into the circular equation and solving it to gain t for each intersection

                    double startangle_0_90 = 0;
                    double endangle_0_90 = 90;
                    double startangle_90_180 = 90;
                    double endangle_90_180 = 180;
                    double startangle_180_270 = 180;
                    double endangle_180_270 = 270;
                    double startangle_270_360 = 270;
                    double endangle_270_360 = 360;

                    double rightportion = rightdistance / r;
                    double topportion = topdistance / r;
                    double leftportion = leftdistance / r;
                    double bottomportion = bottomdistance / r;

                    if (r > rightdistance)
                    {
                        //will hit the right bound
                        endangle_270_360 = 360 - degree * Math.Acos(rightportion);
                        startangle_0_90 = degree * Math.Acos(rightportion);
                    }
                    if (r > topdistance)
                    {
                        //will hit the top bound
                        endangle_0_90 = degree * Math.Asin(topportion);
                        startangle_90_180 = 180 - degree * Math.Asin(topportion);
                    }
                    if (r > leftdistance)
                    {
                        //will hit the left bound
                        endangle_90_180 = 180 - degree * Math.Acos(leftportion);
                        startangle_180_270 = 180 + degree * Math.Acos(leftportion);
                    }
                    if (r > bottomdistance)
                    {
                        //will hit the bottom bound
                        endangle_180_270 = 180 + degree * Math.Asin(bottomportion);
                        startangle_270_360 = 360 - degree * Math.Asin(bottomportion);
                    }

                    //Top right
                    if (r <= cornerdistance_topright)
                    {
                        if (startangle_0_90 < cornerangle_topright)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, (startangle_0_90 + angleAxis.Offset), (cornerangle_topright + angleAxis.Offset));
                        if (cornerangle_topright < endangle_0_90)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, (cornerangle_topright + angleAxis.Offset), (endangle_0_90 + angleAxis.Offset));
                    }

                    //Top left
                    if (r <= cornerdistance_topleft)
                    {
                        if (startangle_90_180 < cornerangle_topleft)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, startangle_90_180 + angleAxis.Offset, cornerangle_topleft + angleAxis.Offset);
                        if (cornerangle_topleft < endangle_90_180)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, cornerangle_topleft + angleAxis.Offset, endangle_90_180 + angleAxis.Offset);
                    }

                    //Bottom left
                    if (r <= cornerdistance_bottomleft)
                    {
                        if (startangle_180_270 < cornerangle_bottomleft)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, startangle_180_270 + angleAxis.Offset, cornerangle_bottomleft + angleAxis.Offset);
                        if (cornerangle_bottomleft < endangle_180_270)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, cornerangle_bottomleft + angleAxis.Offset, endangle_180_270 + angleAxis.Offset);
                    }

                    //Bottom right
                    if (r <= cornerdistance_bottomright)
                    {
                        if (startangle_270_360 < cornerangle_bottomright)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, startangle_270_360 + angleAxis.Offset, cornerangle_bottomright + angleAxis.Offset);
                        if (cornerangle_bottomright < endangle_270_360)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, cornerangle_bottomright + angleAxis.Offset, endangle_270_360 + angleAxis.Offset);
                    }
                }
            }

            if (pass == 0 && this.MajorPen != null)
            {
                OxyPen pen = this.MajorPen;
                foreach (var tickValue in this.MajorTickValues)
                {
                    //a circle consists - in this case - of 4 arcs
                    //the start and end of each arc has to be computed

                    double r = axis.Transform(tickValue);

                    //this works by putting the limits of the plotarea into the circular equation and solving it to gain t for each intersection

                    double startangle_0_90 = 0;
                    double endangle_0_90 = 90;
                    double startangle_90_180 = 90;
                    double endangle_90_180 = 180;
                    double startangle_180_270 = 180;
                    double endangle_180_270 = 270;
                    double startangle_270_360 = 270;
                    double endangle_270_360 = 360;

                    double rightportion = rightdistance / r;
                    double topportion = topdistance / r;
                    double leftportion = leftdistance / r;
                    double bottomportion = bottomdistance / r;

                    if (r > rightdistance)
                    {
                        //will hit the right bound
                        endangle_270_360 = 360 - degree * Math.Acos(rightportion);
                        startangle_0_90 = degree * Math.Acos(rightportion);
                    }
                    if (r > topdistance)
                    {
                        //will hit the top bound
                        endangle_0_90 = degree * Math.Asin(topportion);
                        startangle_90_180 = 180 - degree * Math.Asin(topportion);
                    }
                    if (r > leftdistance)
                    {
                        //will hit the left bound
                        endangle_90_180 = 180 - degree * Math.Acos(leftportion);
                        startangle_180_270 = 180 + degree * Math.Acos(leftportion);
                    }
                    if (r > bottomdistance)
                    {
                        //will hit the bottom bound
                        endangle_180_270 = 180 + degree * Math.Asin(bottomportion);
                        startangle_270_360 = 360 - degree * Math.Asin(bottomportion);
                    }

                    //Top right
                    if (r <= cornerdistance_topright)
                    {
                        if (startangle_0_90 < cornerangle_topright)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, (startangle_0_90 + angleAxis.Offset), (cornerangle_topright + angleAxis.Offset));
                        if (cornerangle_topright < endangle_0_90)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, (cornerangle_topright + angleAxis.Offset), (endangle_0_90 + angleAxis.Offset));
                    }

                    //Top left
                    if (r <= cornerdistance_topleft)
                    {
                        if (startangle_90_180 < cornerangle_topleft)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, startangle_90_180 + angleAxis.Offset, cornerangle_topleft + angleAxis.Offset);
                        if (cornerangle_topleft < endangle_90_180)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, cornerangle_topleft + angleAxis.Offset, endangle_90_180 + angleAxis.Offset);
                    }

                    //Bottom left
                    if (r <= cornerdistance_bottomleft)
                    {
                        if (startangle_180_270 < cornerangle_bottomleft)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, startangle_180_270 + angleAxis.Offset, cornerangle_bottomleft + angleAxis.Offset);
                        if (cornerangle_bottomleft < endangle_180_270)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, cornerangle_bottomleft + angleAxis.Offset, endangle_180_270 + angleAxis.Offset);
                    }

                    //Bottom right
                    if (r <= cornerdistance_bottomright)
                    {
                        if (startangle_270_360 < cornerangle_bottomright)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, startangle_270_360 + angleAxis.Offset, cornerangle_bottomright + angleAxis.Offset);
                        if (cornerangle_bottomright < endangle_270_360)
                            this.RenderTickArc(axis, angleAxis, tickValue, pen, cornerangle_bottomright + angleAxis.Offset, endangle_270_360 + angleAxis.Offset);
                    }
                }
            }

            if (pass == 1)
            {
                foreach (var tickValue in this.MajorLabelValues)
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
            //top
            if (actualAngle > 3 * Math.PI / 4 || actualAngle < -3 * Math.PI / 4)
            {
                ha = HorizontalAlignment.Center;
                va = VerticalAlignment.Top;
            }
            //right
            else if (actualAngle < -Math.PI / 4)
            {
                ha = HorizontalAlignment.Right;
                va = VerticalAlignment.Middle;
            }
            //left
            else if (actualAngle > Math.PI / 4)
            {
                ha = HorizontalAlignment.Left;
                va = VerticalAlignment.Middle;
            }
            //bottom
            else
            {
                ha = HorizontalAlignment.Center;
                va = VerticalAlignment.Bottom;
            }
        }

        /// <summary>
        /// Renders a tick by drawing an lot of segments
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="angleAxis">The angle axis.</param>
        /// <param name="x">The x-value.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="startAngle">The start angle.</param>
        /// <param name="endAngle">The end angle.</param>
        private void RenderTickArc(Axis axis, AngleAxis angleAxis, double x, OxyPen pen, double startAngle, double endAngle)
        {
            if(startAngle>endAngle)
            {

            }
            // caution: make sure angleAxis.UpdateActualMaxMin(); has been called

            // number of segment to draw a full circle
            // - decrease if you want get more speed
            // - increase if you want more detail
            // (making a public property of it would be a great idea)

            // compute the actual number of segments
            var segmentCount = (int)(MaxSegments * Math.Abs(endAngle - startAngle) / 360.0);
            if (angleAxis.FractionUnit == Math.PI || angleAxis.ActualMaximum == 2 * Math.PI)
            {
                segmentCount = (int)(MaxSegments * Math.Abs(endAngle - startAngle) / (2 * Math.PI));
                startAngle *= rad;
                endAngle *= rad;
            }

            segmentCount = Math.Max(segmentCount, 2);
            segmentCount = Math.Min(segmentCount, (int)MaxSegments);
            var angleStep = Math.Abs(endAngle - startAngle) / (segmentCount - 1);

            var points = new List<ScreenPoint>();

            for (var i = 0; i < segmentCount; i++)
            {
                var angle = startAngle + (i * angleStep);
                ScreenPoint toadd = axis.Transform(x, angle, angleAxis);
                points.Add(toadd);
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
            this.RenderContext.DrawClippedMathText(
                new OxyRect(axis.ScreenMin, axis.ScreenMax),
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
