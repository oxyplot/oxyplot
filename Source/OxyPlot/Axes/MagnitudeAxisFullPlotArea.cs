// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagnitudeAxisFullPlotArea.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a magnitude axis for polar plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MagnitudeAxisFullPlotArea : MagnitudeAxis
    {
        private double _midshiftH = 0;
        /// <summary>
        /// Portion to shift the center in horizontal direction relative to the plot area size (from -0.5 to +0.5 meaning +-50% of the width)
        /// </summary>
        public double MidshiftH
        {
            get { return _midshiftH; }
            set
            {
                _midshiftH = value;
                _midshiftH = Math.Max(_midshiftH, -0.5d);
                _midshiftH = Math.Min(_midshiftH, 0.5d);
            }
        }

        private double _midshiftV = 0d;
        /// <summary>
        /// Portion to shift the center in vertical direction relative to the plot area size (from -0.5 to +0.5 meaning +-50% of the height)
        /// </summary>
        public double MidshiftV
        {
            get { return _midshiftV; }
            set
            {
                _midshiftV = value;
                _midshiftV = Math.Max(_midshiftV, -0.5d);
                _midshiftV = Math.Min(_midshiftV, 0.5d);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagnitudeAxis" /> class.
        /// </summary>
        public MagnitudeAxisFullPlotArea()
        {
            this.Position = AxisPosition.None;
            this.IsPanEnabled = true;
            this.IsZoomEnabled = false;

            this.MajorGridlineStyle = LineStyle.Solid;
            this.MinorGridlineStyle = LineStyle.Solid;
        }

        ///// <summary>
        ///// Inverse transform the specified screen point.
        ///// </summary>
        ///// <param name="x">The x coordinate.</param>
        ///// <param name="y">The y coordinate.</param>
        ///// <param name="yaxis">The y-axis.</param>
        ///// <returns>The data point.</returns>
        //public override DataPoint InverseTransform(double x, double y, Axis yaxis)
        //{
        //    var angleAxis = yaxis as AngleAxis;
        //    if (angleAxis == null)
        //    {
        //        throw new InvalidOperationException("Polar angle axis not defined!");
        //    }

        //    x -= this.MidPoint.x;
        //    y -= this.MidPoint.y;
        //    y *= -1;
        //    double th = Math.Atan2(y, x);
        //    double r = Math.Sqrt((x * x) + (y * y));
        //    x = (r / this.Scale) + this.Offset;
        //    y = (th / angleAxis.Scale) + angleAxis.Offset;
        //    return new DataPoint(x, y);
        //}

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pass">The rendering pass.</param>
        public override void Render(IRenderContext rc, int pass)
        {
            var r = new MagnitudeAxisFullPlotAreaRenderer(rc, this.PlotModel);
            r.Render(this, pass);
        }

        ///// <summary>
        ///// Transforms the specified point to screen coordinates.
        ///// </summary>
        ///// <param name="x">The x value (for the current axis).</param>
        ///// <param name="y">The y value.</param>
        ///// <param name="yaxis">The y axis.</param>
        ///// <returns>The transformed point.</returns>
        //public override ScreenPoint Transform(double x, double y, Axis yaxis)
        //{
        //    var angleAxis = yaxis as AngleAxis;
        //    if (angleAxis == null)
        //    {
        //        throw new InvalidOperationException("Polar angle axis not defined!");
        //    }

        //    var r = (x - this.Offset) * this.Scale;
        //    var theta = (y - angleAxis.Offset) * angleAxis.Scale;

        //    return new ScreenPoint(this.MidPoint.x + (r * Math.Cos(theta / 180 * Math.PI)), this.MidPoint.y - (r * Math.Sin(theta / 180 * Math.PI)));
        //}

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

            this.MidPoint = new ScreenPoint((x0 + x1) / 2 + this.MidshiftH * bounds.Width, (y0 + y1) / 2 + this.MidshiftV * bounds.Height); //new ScreenPoint((x0 + x1) / 2, (y0 + y1) / 2);
            //this.ShiftedMidPoint = new ScreenPoint((x0 + x1) / 2 + this.MidshiftH * bounds.Width, (y0 + y1) / 2 + this.MidshiftV * bounds.Height);

            // this.ActualMinimum = 0;
            double r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));
            this.SetTransform(0.5 * r / (this.ActualMaximum - this.ActualMinimum), this.ActualMinimum);
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="ppt">The previous point (screen coordinates).</param>
        /// <param name="cpt">The current point (screen coordinates).</param>
        public override void Pan(ScreenPoint ppt, ScreenPoint cpt)
        {
            if (!this.IsPanEnabled)
            {
                return;
            }

            bool isHorizontal = this.IsHorizontal();

            double dsx = cpt.X - ppt.X;
            double dsy = cpt.Y - ppt.Y;

            double dsxp = dsx / this.PlotModel.PlotArea.Width;
            double dsyp = dsy / this.PlotModel.PlotArea.Height;

            this.MidshiftH += dsxp;
            this.MidshiftV += dsyp;

            //double d = isHorizontal ? cpt.X - ppt.X : cpt.Y - ppt.Y;
            //this.Pan(d);

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan, 0, 0));
        }

        /// <summary>
        /// Zooms the axis at the specified coordinate.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        /// <param name="x">The coordinate to zoom at.</param>
        public override void ZoomAt(double factor, double x)
        {
            if (!this.IsZoomEnabled)
            {
                return;
            }

            base.ZoomAt(factor, 0);
            return;
        }
    }
}
