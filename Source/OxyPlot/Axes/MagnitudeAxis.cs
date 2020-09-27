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

            double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));

            var a0 = 0.0;
            var a1 = r * 0.5;

            double dx = a1 - a0;
            a1 = a0 + (this.EndPosition * dx);
            a0 = a0 + (this.StartPosition * dx);

            double marginSign = this.IsReversed ? -1.0 : 1.0;

            if (this.MinimumMargin > 0)
            {
                a0 += this.MinimumMargin * marginSign;
            }

            if (this.MaximumMargin > 0)
            {
                a1 -= this.MaximumMargin * marginSign;
            }

            if (this.MinimumDataMargin > 0)
            {
                a0 += this.MinimumDataMargin * marginSign;
            }

            if (this.MaximumDataMargin > 0)
            {
                a1 -= this.MaximumDataMargin * marginSign;
            }

            if (this.ActualMaximum - this.ActualMinimum < double.Epsilon)
            {
                this.ActualMaximum = this.ActualMinimum + 1;
            }

            double max = this.PreTransform(this.ActualMaximum);
            double min = this.PreTransform(this.ActualMinimum);

            double da = a0 - a1;
            double newOffset, newScale;
            if (Math.Abs(da) > double.Epsilon)
            {
                newOffset = (a0 / da * max) - (a1 / da * min);
            }
            else
            {
                newOffset = 0;
            }

            double range = max - min;
            if (Math.Abs(range) > double.Epsilon)
            {
                newScale = (a1 - a0) / range;
            }
            else
            {
                newScale = 1;
            }

            this.SetTransform(newScale, newOffset);

            if (this.MinimumDataMargin > 0)
            {
                this.ClipMinimum = this.InverseTransform(0.0);
            }
            else
            {
                this.ClipMinimum = this.ActualMinimum;
            }

            if (this.MaximumDataMargin > 0)
            {
                this.ClipMaximum = this.InverseTransform(r * 0.5);
            }
            else
            {
                this.ClipMaximum = this.ActualMaximum;
            }

            this.ActualMaximumAndMinimumChangedOverride();
        }
    }
}
