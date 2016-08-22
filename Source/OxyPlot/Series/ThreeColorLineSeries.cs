// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreeColorLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a three-color line series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a two-color line series.
    /// </summary>
    public class ThreeColorLineSeries : LineSeries
    {
        /// <summary>
        /// The default low color.
        /// </summary>
        private OxyColor defaultColorLo;

        /// <summary>
        /// The default hi color.
        /// </summary>
        private OxyColor defaultColorHi;

        /// <summary>
        /// Initializes a new instance of the <see cref = "ThreeColorLineSeries" /> class.
        /// </summary>
        public ThreeColorLineSeries()
        {
            this.LimitLo = -5.0;
            this.ColorLo = OxyColors.Blue;
            this.LineStyleLo = LineStyle.Solid;
            this.LimitHi = 5.0;
            this.ColorHi = OxyColors.Red;
            this.LineStyleHi = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the color for the part of the line that is below the limit.
        /// </summary>
        public OxyColor ColorLo { get; set; }

        /// <summary>
        /// Gets or sets the color for the part of the line that is above the limit.
        /// </summary>
        public OxyColor ColorHi { get; set; }

        /// <summary>
        /// Gets the actual hi color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColorLo
        {
            get { return this.ColorLo.GetActualColor(this.defaultColorLo); }
        }

        /// <summary>
        /// Gets the actual low color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColorHi
        {
            get { return this.ColorHi.GetActualColor(this.defaultColorHi); }
        }

        /// <summary>
        /// Gets or sets the high limit.
        /// </summary>
        /// <remarks>The parts of the line that is below this limit will be rendered with ColorHi.
        /// The parts of the line that is above the limit will be rendered with Color.</remarks>
        public double LimitHi { get; set; }

        /// <summary>
        /// Gets or sets the low limit.
        /// </summary>
        /// <remarks>The parts of the line that is below this limit will be rendered with ColorLo.
        /// The parts of the line that is above the limit will be rendered with Color.</remarks>
        public double LimitLo { get; set; }

        /// <summary>
        /// Gets or sets the dash array for the rendered line that is above the limit (overrides <see cref="LineStyle" />).
        /// </summary>
        /// <value>The dash array.</value>
        /// <remarks>If this is not <c>null</c> it overrides the <see cref="LineStyle" /> property.</remarks>
        public double[] DashesHi { get; set; }

        /// <summary>
        /// Gets or sets the dash array for the rendered line that is below the limit (overrides <see cref="LineStyle" />).
        /// </summary>
        /// <value>The dash array.</value>
        /// <remarks>If this is not <c>null</c> it overrides the <see cref="LineStyle" /> property.</remarks>
        public double[] DashesLo { get; set; }

        /// <summary>
        /// Gets or sets the line style for the part of the line that is above the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyleHi { get; set; }

        /// <summary>
        /// Gets or sets the line style for the part of the line that is below the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyleLo { get; set; }

        /// <summary>
        /// Gets the actual line style for the part of the line that is above the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle ActualLineStyleHi
        {
            get
            {
                return this.LineStyleHi != LineStyle.Automatic ? this.LineStyleHi : LineStyle.Solid;
            }
        }

        /// <summary>
        /// Gets the actual line style for the part of the line that is below the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle ActualLineStyleLo
        {
            get
            {
                return this.LineStyleLo != LineStyle.Automatic ? this.LineStyleLo : LineStyle.Solid;
            }
        }

        /// <summary>
        /// Gets the actual dash array for the line that is above the limit.
        /// </summary>
        protected double[] ActualDashArrayHi
        {
            get
            {
                return this.DashesHi ?? this.ActualLineStyleHi.GetDashArray();
            }
        }

        /// <summary>
        /// Gets the actual dash array for the line that is below the limit.
        /// </summary>
        protected double[] ActualDashArrayLo
        {
            get
            {
                return this.DashesLo ?? this.ActualLineStyleLo.GetDashArray();
            }
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();

            if (this.ColorLo.IsAutomatic())
            {
                this.defaultColorLo = this.PlotModel.GetDefaultColor();
            }

            if (this.LineStyleLo == LineStyle.Automatic)
            {
                this.LineStyleLo = this.PlotModel.GetDefaultLineStyle();
            }

            if (this.ColorHi.IsAutomatic())
            {
                this.defaultColorHi = this.PlotModel.GetDefaultColor();
            }

            if (this.LineStyleHi == LineStyle.Automatic)
            {
                this.LineStyleHi = this.PlotModel.GetDefaultLineStyle();
            }
        }

        /// <summary>
        /// Renders the smoothed line.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="pointsToRender">The points.</param>
        protected override void RenderLine(IRenderContext rc, OxyRect clippingRect, IList<ScreenPoint> pointsToRender)
        {
            var bottom = clippingRect.Bottom;
            var top = clippingRect.Top;

            // todo: this does not work when y axis is reversed
            var yLo = this.YAxis.Transform(this.LimitLo);
            var yHi = this.YAxis.Transform(this.LimitHi);

            if (yLo < clippingRect.Top)
            {
                yLo = clippingRect.Top;
            }

            if (yLo > clippingRect.Bottom)
            {
                yLo = clippingRect.Bottom;
            }

            if (yHi < clippingRect.Top)
            {
                yHi = clippingRect.Top;
            }

            if (yHi > clippingRect.Bottom)
            {
                yHi = clippingRect.Bottom;
            }

            clippingRect = new OxyRect(clippingRect.Left, yHi, clippingRect.Width, yLo - yHi);

            rc.DrawClippedLine(
                clippingRect,
                pointsToRender,
                this.MinimumSegmentLength * this.MinimumSegmentLength,
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                this.ActualDashArray,
                this.LineJoin,
                false);

            clippingRect = new OxyRect(clippingRect.Left, yLo, clippingRect.Width, bottom - yLo);

            rc.DrawClippedLine(
                clippingRect,
                pointsToRender,
                this.MinimumSegmentLength * this.MinimumSegmentLength,
                this.GetSelectableColor(this.ActualColorLo),
                this.StrokeThickness,
                this.ActualDashArrayLo,
                this.LineJoin,
                false);

            clippingRect = new OxyRect(clippingRect.Left, top, clippingRect.Width, yHi - top);

            rc.DrawClippedLine(
                clippingRect,
                pointsToRender,
                this.MinimumSegmentLength * this.MinimumSegmentLength,
                this.GetSelectableColor(this.ActualColorHi),
                this.StrokeThickness,
                this.ActualDashArrayHi,
                this.LineJoin,
                false);
        }
    }
}