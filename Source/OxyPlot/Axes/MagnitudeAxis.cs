//-----------------------------------------------------------------------
// <copyright file="MagnitudeAxis.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Polar plot magnitude axis.
    /// </summary>
    public class MagnitudeAxis : LinearAxis
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MagnitudeAxis" /> class.
        /// </summary>
        public MagnitudeAxis()
        {
            this.Position = AxisPosition.Bottom;
            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;

            this.MajorGridlineStyle = LineStyle.Solid;
            this.MinorGridlineStyle = LineStyle.Solid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagnitudeAxis"/> class.
        /// </summary>
        /// <param name="minimum">
        /// The minimum.
        /// </param>
        /// <param name="maximum">
        /// The maximum.
        /// </param>
        /// <param name="majorStep">
        /// The major step.
        /// </param>
        /// <param name="minorStep">
        /// The minor step.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public MagnitudeAxis(
            double minimum = double.NaN, 
            double maximum = double.NaN, 
            double majorStep = double.NaN, 
            double minorStep = double.NaN, 
            string title = null)
            : this()
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.MajorStep = majorStep;
            this.MinorStep = minorStep;
            this.Title = title;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the midpoint (screen coordinates) of the plot area.
        ///   This is used by polar coordinate systems.
        /// </summary>
        internal ScreenPoint MidPoint { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Inverse transform the specified screen point.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <param name="yaxis">
        /// The y-axis.
        /// </param>
        /// <returns>
        /// The data point.
        /// </returns>
        public override DataPoint InverseTransform(double x, double y, IAxis yaxis)
        {
            var angleAxis = yaxis as AngleAxis;
            if (angleAxis == null)
            {
                throw new InvalidOperationException("Polar angle axis not defined!");
            }

            x -= this.MidPoint.x;
            y -= this.MidPoint.y;
            double th = Math.Atan2(y, x);
            double r = Math.Sqrt(x * x + y * y);
            x = r / this.scale + this.offset;
            y = yaxis != null ? th / angleAxis.Scale + angleAxis.Offset : double.NaN;
            return new DataPoint(x, y);
        }

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="axisLayer">
        /// The rendering order.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer)
        {
            if (this.Layer != axisLayer)
            {
                return;
            }

            var r = new MagnitudeAxisRenderer(rc, model);
            r.Render(this);
        }

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="x">
        /// The x value (for the current axis).
        /// </param>
        /// <param name="y">
        /// The y value.
        /// </param>
        /// <param name="yaxis">
        /// The y axis.
        /// </param>
        /// <returns>
        /// The transformed point.
        /// </returns>
        public override ScreenPoint Transform(double x, double y, IAxis yaxis)
        {
            var angleAxis = yaxis as AngleAxis;
            if (angleAxis == null)
            {
                throw new InvalidOperationException("Polar angle axis not defined!");
            }

            double r = (x - this.Offset) * this.scale;
            double theta = yaxis != null ? (y - angleAxis.Offset) * angleAxis.Scale : double.NaN;

            return new ScreenPoint(this.MidPoint.x + r * Math.Cos(theta), this.MidPoint.y + r * Math.Sin(theta));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the scale and offset properties of the transform
        ///   from the specified boundary rectangle.
        /// </summary>
        /// <param name="bounds">
        /// The bounds.
        /// </param>
        internal override void UpdateTransform(OxyRect bounds)
        {
            double x0 = bounds.Left;
            double x1 = bounds.Right;
            double y0 = bounds.Bottom;
            double y1 = bounds.Top;

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            this.MidPoint = new ScreenPoint((x0 + x1) / 2, (y0 + y1) / 2);

            this.ActualMinimum = 0;
            double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
            this.scale = 0.5 * r / (this.ActualMaximum - this.ActualMinimum);
            this.Offset = this.ActualMinimum;
        }

        #endregion
    }
}
