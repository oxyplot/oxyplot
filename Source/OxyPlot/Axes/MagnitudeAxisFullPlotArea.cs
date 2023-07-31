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

    /// <summary>
    /// Represents a magnitude axis that covers the whole plot area.
    /// </summary>
    public class MagnitudeAxisFullPlotArea : MagnitudeAxis
    {
        private double midshiftH = 0;
        private double midshiftV = 0d;

        /// <summary>
        /// Portion to shift the center in horizontal direction relative to the plot area size (from -0.5 to +0.5 meaning +-50% of the width)
        /// </summary>
        public double MidshiftH
        {
            get => this.midshiftH;
            set
            {
                this.midshiftH = value;
                this.midshiftH = Math.Max(this.midshiftH, -0.5d);
                this.midshiftH = Math.Min(this.midshiftH, 0.5d);
            }
        }

        /// <summary>
        /// Portion to shift the center in vertical direction relative to the plot area size (from -0.5 to +0.5 meaning +-50% of the height)
        /// </summary>
        public double MidshiftV
        {
            get => this.midshiftV;
            set
            {
                this.midshiftV = value;
                this.midshiftV = Math.Max(this.midshiftV, -0.5d);
                this.midshiftV = Math.Min(this.midshiftV, 0.5d);
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

        /// <inheritdoc/>
        public override void GetTickValues(out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            var axisRect = new OxyRect(this.ScreenMin, this.ScreenMax);

            var distanceTopLeft = axisRect.TopLeft.DistanceTo(this.MidPoint);
            var distanceTopRight = axisRect.TopRight.DistanceTo(this.MidPoint);
            var distanceBottomRight = axisRect.BottomRight.DistanceTo(this.MidPoint);
            var distanceBottomLeft = axisRect.BottomLeft.DistanceTo(this.MidPoint);

            var maxDistance = Math.Max(distanceTopLeft, distanceTopRight);
            maxDistance = Math.Max(maxDistance, distanceBottomRight);
            maxDistance = Math.Max(maxDistance, distanceBottomLeft);

            var actualMaximum = this.InverseTransform(maxDistance);

            majorTickValues = AxisUtilities.CreateTickValues(this.ActualMinimum, actualMaximum, this.ActualMajorStep);
            minorTickValues = AxisUtilities.CreateTickValues(this.ActualMinimum, actualMaximum, this.ActualMinorStep);
            minorTickValues = AxisUtilities.FilterRedundantMinorTicks(majorTickValues, minorTickValues);

            majorLabelValues = majorTickValues;
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

            this.MidPoint = new ScreenPoint((x0 + x1) / 2 + this.MidshiftH * bounds.Width, (y0 + y1) / 2 + this.MidshiftV * bounds.Height);

            var r = Math.Min(Math.Abs(x1 - x0), Math.Abs(y1 - y0));

            var a0 = 0.0;
            var a1 = r * 0.5;

            var dx = a1 - a0;
            a1 = a0 + this.EndPosition * dx;
            a0 += this.StartPosition * dx;

            var marginSign = this.IsReversed ? -1.0 : 1.0;

            if (this.MinimumDataMargin > 0)
            {
                a0 += this.MinimumDataMargin * marginSign;
            }

            if (this.MaximumDataMargin > 0)
            {
                a1 -= this.MaximumDataMargin * marginSign;
            }

            if (this.ActualMaximum - this.ActualMinimum <= 0)
            {
                this.ActualMaximum = this.ActualMinimum + 1;
            }

            var max = this.PreTransform(this.ActualMaximum);
            var min = this.PreTransform(this.ActualMinimum);

            var da = a0 - a1;
            double newOffset, newScale;
            if (Math.Abs(da) > double.Epsilon)
            {
                newOffset = a0 / da * max - a1 / da * min;
            }
            else
            {
                newOffset = 0;
            }

            var range = max - min;
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

            _ = this.IsHorizontal();

            var dsx = cpt.X - ppt.X;
            var dsy = cpt.Y - ppt.Y;

            var dsxp = dsx / this.PlotModel.PlotArea.Width;
            var dsyp = dsy / this.PlotModel.PlotArea.Height;

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
