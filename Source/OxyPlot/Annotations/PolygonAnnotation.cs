// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a polygon annotation.
    /// </summary>
    public class PolygonAnnotation : Annotation
    {
        #region Constants and Fields

        /// <summary>
        ///   The polygon points transformed to screen coordinates.
        /// </summary>
        private IList<ScreenPoint> screenPoints;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonAnnotation"/> class. 
        /// </summary>
        public PolygonAnnotation()
        {
            this.Color = OxyColors.Blue;
            this.Fill = OxyColors.LightBlue;
            this.StrokeThickness = 1;
            this.LineStyle = LineStyle.Solid;
            this.LineJoin = OxyPenLineJoin.Miter;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the color of the line.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the fill color.
        /// </summary>
        /// <value> The fill. </value>
        public OxyColor Fill { get; set; }

        /// <summary>
        ///   Gets or sets the line join.
        /// </summary>
        /// <value> The line join. </value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value> The line style. </value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the points.
        /// </summary>
        /// <value> The points. </value>
        public IEnumerable<DataPoint> Points { get; set; }

        /// <summary>
        ///   Gets or sets the stroke thickness.
        /// </summary>
        /// <value> The stroke thickness. </value>
        public double StrokeThickness { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the polygon annotation.
        /// </summary>
        /// <param name="rc">
        /// The render context. 
        /// </param>
        /// <param name="model">
        /// The plot model. 
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);
            if (this.Points == null)
            {
                return;
            }

            // transform to screen coordinates
            this.screenPoints = this.Points.Select(p => this.Transform(p)).ToList();
            if (this.screenPoints.Count == 0)
            {
                return;
            }

            // clip to the area defined by the axes
            var clipping = this.GetClippingRect();

            const double MinimumSegmentLength = 4;

            rc.DrawClippedPolygon(
                this.screenPoints, 
                clipping, 
                MinimumSegmentLength * MinimumSegmentLength, 
                this.GetSelectableFillColor(this.Fill), 
                this.GetSelectableColor(this.Color), 
                this.StrokeThickness, 
                this.LineStyle, 
                this.LineJoin);

            if (!string.IsNullOrEmpty(this.Text))
            {
                var textPosition = ScreenPointHelper.GetCentroid(this.screenPoints);

                rc.DrawClippedText(
                    clipping, 
                    textPosition, 
                    this.Text, 
                    this.ActualTextColor, 
                    this.ActualFont, 
                    this.ActualFontSize, 
                    this.ActualFontWeight, 
                    0, 
                    HorizontalTextAlign.Center, 
                    VerticalTextAlign.Middle);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <param name="tolerance">
        /// The tolerance. 
        /// </param>
        /// <returns>
        /// A hit test result. 
        /// </returns>
        protected internal override HitTestResult HitTest(ScreenPoint point, double tolerance)
        {
            if (ScreenPointHelper.IsPointInPolygon(point, this.screenPoints))
            {
                return new HitTestResult(point);
            }

            return null;
        }

        #endregion
    }
}