// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a two-color line series.
    /// </summary>
    public class TwoColorLineSeries : LineSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TwoColorLineSeries" /> class.
        /// </summary>
        public TwoColorLineSeries()
        {
            this.Limit = 0.0;
            this.Color2 = OxyColors.Blue;
            this.LineStyle2 = LineStyle.Solid;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the color for the part of the line that is below the limit.
        /// </summary>
        public OxyColor Color2 { get; set; }

        /// <summary>
        ///   Gets or sets the limit.
        /// </summary>
        /// <remarks>
        ///   The parts of the line that is below this limit will be rendered with Color2.
        ///   The parts of the line that is above the limit will be rendered with Color.
        /// </remarks>
        public double Limit { get; set; }

        /// <summary>
        ///   Gets or sets the line style for the part of the line that is below the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle2 { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The set default values.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.Color2 == null)
            {
                this.LineStyle2 = model.GetDefaultLineStyle();
                this.Color2 = model.GetDefaultColor();
            }
        }

        /// <summary>
        /// Renders the smoothed line.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="clippingRect">
        /// The clipping rect.
        /// </param>
        /// <param name="pointsToRender">
        /// The points.
        /// </param>
        protected override void RenderSmoothedLine(IRenderContext rc, OxyRect clippingRect, IList<ScreenPoint> pointsToRender)
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

            clippingRect.Bottom = y;
            rc.DrawClippedLine(
                pointsToRender, 
                clippingRect, 
                this.MinimumSegmentLength * this.MinimumSegmentLength, 
                this.GetSelectableColor(this.Color), 
                this.StrokeThickness, 
                this.LineStyle, 
                this.LineJoin, 
                false);
            clippingRect.Top = y;
            clippingRect.Height = bottom - y;
            rc.DrawClippedLine(
                pointsToRender, 
                clippingRect, 
                this.MinimumSegmentLength * this.MinimumSegmentLength, 
                this.GetSelectableColor(this.Color2), 
                this.StrokeThickness, 
                this.LineStyle2, 
                this.LineJoin, 
                false);
        }

        #endregion
    }
}