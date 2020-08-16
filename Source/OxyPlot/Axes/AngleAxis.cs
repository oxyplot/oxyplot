// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an angular axis for polar plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an angular axis for polar plots.
    /// </summary>
    public class AngleAxis : LinearAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AngleAxis" /> class.
        /// </summary>
        public AngleAxis()
        {
            this.Position = AxisPosition.All;
            this.TickStyle = TickStyle.None;
            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;
            this.MajorGridlineStyle = LineStyle.Solid;
            this.MinorGridlineStyle = LineStyle.Solid;
            this.StartAngle = 0;
            this.EndAngle = 360;
        }

        /// <summary>
        /// Gets or sets the start angle (degrees).
        /// </summary>
        public double StartAngle { get; set; }

        /// <summary>
        /// Gets or sets the end angle (degrees).
        /// </summary>
        public double EndAngle { get; set; }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(
            out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            var minimum = this.StartAngle / this.Scale;
            var maximum = this.EndAngle / this.Scale;

            minorTickValues = this.CreateTickValues(minimum, maximum, this.ActualMinorStep);
            majorTickValues = this.CreateTickValues(minimum, maximum, this.ActualMajorStep);
            majorLabelValues = this.CreateTickValues(this.Minimum, this.Maximum, this.ActualMajorStep);

            minorTickValues = AxisUtilities.FilterRedundantMinorTicks(majorTickValues, minorTickValues);
        }

        /// <summary>
        /// Inverse transforms the specified screen point.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="yaxis">The y-axis.</param>
        /// <returns>The data point.</returns>
        /// <exception cref="System.InvalidOperationException">Angle axis should always be the y-axis.</exception>
        public override DataPoint InverseTransform(double x, double y, Axis yaxis)
        {
            throw new InvalidOperationException("Angle axis should always be the y-axis.");
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
        /// <param name="pass">The pass.</param>
        public override void Render(IRenderContext rc, int pass)
        {
            var r = new AngleAxisRenderer(rc, this.PlotModel);
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
            throw new InvalidOperationException("Angle axis should always be the y-axis.");
        }

        /// <summary>
        /// Updates the scale and offset properties of the transform from the specified boundary rectangle.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        internal override void UpdateTransform(OxyRect bounds)
        {
            var x0 = bounds.Left;
            var x1 = bounds.Right;
            var y0 = bounds.Bottom;
            var y1 = bounds.Top;

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            var newScale = (this.EndAngle - this.StartAngle) / (this.ActualMaximum - this.ActualMinimum);
            var newOffset = this.ActualMinimum - (this.StartAngle / newScale);
            this.SetTransform(newScale, newOffset);
        }
    }
}
