// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a two-color line series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a two-color line series.
    /// </summary>
    public class TwoColorLineSeries : LineSeries
    {
        /// <summary>
        /// The default second color.
        /// </summary>
        private OxyColor defaultColor2;

        /// <summary>
        /// Initializes a new instance of the <see cref = "TwoColorLineSeries" /> class.
        /// </summary>
        public TwoColorLineSeries()
        {
            this.Limit = 0.0;
            this.Color2 = OxyColors.Blue;
            this.LineStyle2 = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the color for the part of the line that is below the limit.
        /// </summary>
        public OxyColor Color2 { get; set; }

        /// <summary>
        /// Gets the actual second color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColor2
        {
            get { return this.Color2.GetActualColor(this.defaultColor2); }
        }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <remarks>The parts of the line that is below this limit will be rendered with Color2.
        /// The parts of the line that is above the limit will be rendered with Color.</remarks>
        public double Limit { get; set; }

        /// <summary>
        /// Gets or sets the dash array for the rendered line that is below the limit (overrides <see cref="LineStyle" />).
        /// </summary>
        /// <value>The dash array.</value>
        /// <remarks>If this is not <c>null</c> it overrides the <see cref="LineStyle" /> property.</remarks>
        public double[] Dashes2 { get; set; }

        /// <summary>
        /// Gets or sets the line style for the part of the line that is below the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle2 { get; set; }

        /// <summary>
        /// Gets the actual line style for the part of the line that is below the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle ActualLineStyle2
        {
            get
            {
                return this.LineStyle2 != LineStyle.Automatic ? this.LineStyle2 : LineStyle.Solid;
            }
        }

        /// <summary>
        /// Gets the actual dash array for the line that is below the limit.
        /// </summary>
        protected double[] ActualDashArray2
        {
            get
            {
                return this.Dashes2 ?? this.ActualLineStyle2.GetDashArray();
            }
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();

            if (this.Color2.IsAutomatic())
            {
                this.defaultColor2 = this.PlotModel.GetDefaultColor();
            }

            if (this.LineStyle2 == LineStyle.Automatic)
            {
                this.LineStyle2 = this.PlotModel.GetDefaultLineStyle();
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
            double bottom = clippingRect.Bottom;

            // todo: this does not work when y axis is reversed
            double y = this.YAxis.Transform(this.Limit);

            if (y < clippingRect.Top)
            {
                y = clippingRect.Top;
            }

            if (y > clippingRect.Bottom)
            {
                y = clippingRect.Bottom;
            }

            var dashArray = this.ActualDashArray;
            var dashArray2 = this.ActualDashArray2;

            clippingRect = new OxyRect(clippingRect.Left, clippingRect.Top, clippingRect.Width, y - clippingRect.Top);
            rc.DrawClippedLine(
                clippingRect,
                pointsToRender,
                this.MinimumSegmentLength * this.MinimumSegmentLength,
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                dashArray,
                this.LineJoin,
                false);
            clippingRect = new OxyRect(clippingRect.Left, y, clippingRect.Width, bottom - y);
            rc.DrawClippedLine(
                clippingRect,
                pointsToRender,
                this.MinimumSegmentLength * this.MinimumSegmentLength,
                this.GetSelectableColor(this.ActualColor2),
                this.StrokeThickness,
                dashArray2,
                this.LineJoin,
                false);
        }
    }
}