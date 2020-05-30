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
