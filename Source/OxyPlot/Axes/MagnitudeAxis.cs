// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a magnitude axis for polar plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;

    /// <summary>
    /// Represents a magnitude axis for polar plots.
    /// </summary>
    public class MagnitudeAxis : LinearAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MagnitudeAxis" /> class.
        /// </summary>
        public MagnitudeAxis()
        {
            this.Position = AxisPosition.None;
            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;

            this.MajorGridlineStyle = LineStyle.Solid;
            this.MinorGridlineStyle = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the midpoint (screen coordinates) of the plot area. This is used by polar coordinate systems.
        /// </summary>
        internal ScreenPoint MidPoint { get; set; }

        /// <summary>
        /// Inverse transform the specified screen point.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="yaxis">The y-axis.</param>
        /// <returns>The data point.</returns>
        public override DataPoint InverseTransform(double x, double y, Axis yaxis)
        {
            var angleAxis = yaxis as AngleAxis;
            if (angleAxis == null)
            {
                throw new InvalidOperationException("Polar angle axis not defined!");
            }

            x -= this.MidPoint.x;
            y -= this.MidPoint.y;
            y *= -1;
            double th = Math.Atan2(y, x);
            double r = Math.Sqrt((x * x) + (y * y));
            x = (r / this.Scale) + this.Offset;
            y = (th / angleAxis.Scale) + angleAxis.Offset*Math.PI/180;
            return new DataPoint(x, y);
        }

        /// <summary>
        /// Determines whether the axis is used for X/Y values.
        /// </summary>
        /// <returns><c>true</c> if it is an XY axis; otherwise, <c>false</c> .</returns>
        public override bool IsXyAxis()
        {
            return false;
        }

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pass">The rendering pass.</param>
        public override void Render(IRenderContext rc, int pass)
        {
            var r = new MagnitudeAxisRenderer(rc, this.PlotModel);
            r.Render(this, pass);
        }

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="x">The x value (for the current axis).</param>
        /// <param name="y">The y value.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns>The transformed point.</returns>
        public override ScreenPoint Transform(double x, double y, Axis yaxis)
        {
            var angleAxis = yaxis as AngleAxis;
            if (angleAxis == null)
            {
                throw new InvalidOperationException("Polar angle axis not defined!");
            }

            var r = (x - this.Offset) * this.Scale;
            var theta = (y - angleAxis.Offset) * angleAxis.Scale;

            return new ScreenPoint(this.MidPoint.x + (r * Math.Cos(theta / 180 * Math.PI)), this.MidPoint.y - (r * Math.Sin(theta / 180 * Math.PI)));
        }

        /// <summary>
        /// Updates the scale and offset properties of the transform from the specified boundary rectangle.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        internal override void UpdateTransform(OxyRect bounds)
        {
            double x0 = bounds.Left;
            double x1 = bounds.Right;
            double y0 = bounds.Bottom;
            double y1 = bounds.Top;

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            this.MidPoint = new ScreenPoint((x0 + x1) / 2, (y0 + y1) / 2);

            // this.ActualMinimum = 0;
            double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
            this.SetTransform(0.5 * r / (this.ActualMaximum - this.ActualMinimum), this.ActualMinimum);
        }
    }
}