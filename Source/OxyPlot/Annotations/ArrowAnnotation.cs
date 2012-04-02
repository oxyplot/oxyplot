// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrowAnnotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an arrow annotation.
    /// </summary>
    public class ArrowAnnotation : Annotation
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrowAnnotation"/> class. 
        /// </summary>
        public ArrowAnnotation()
        {
            this.HeadLength = 10;
            this.HeadWidth = 3;
            this.Color = OxyColors.Blue;
            this.StrokeThickness = 2;
            this.LineStyle = LineStyle.Solid;
            this.LineJoin = OxyPenLineJoin.Miter;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the arrow direction.
        /// </summary>
        /// <remarks>
        /// Setting this property overrides the StartPoint property.
        /// </remarks>
        public ScreenVector ArrowDirection { get; set; }

        /// <summary>
        ///   Gets or sets the color of the arrow.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        public DataPoint EndPoint { get; set; }

        /// <summary>
        ///   Gets or sets the length of the head (relative to thickness).
        /// </summary>
        /// <value> The length of the head. </value>
        public double HeadLength { get; set; }

        /// <summary>
        ///   Gets or sets the width of the head (relative to thickness).
        /// </summary>
        /// <value> The width of the head. </value>
        public double HeadWidth { get; set; }

        /// <summary>
        ///   Gets or sets the line join type.
        /// </summary>
        /// <value> The line join type. </value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value> The line style. </value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets StartPoint.
        /// </summary>
        /// <remarks>
        /// This property is overridden by the ArrowDirection property, if set.
        /// </remarks>
        public DataPoint StartPoint { get; set; }

        /// <summary>
        ///   Gets or sets the stroke thickness.
        /// </summary>
        /// <value> The stroke thickness. </value>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the 'veeness' of the head (relative to thickness).
        /// </summary>
        /// <value> The 'veeness'. </value>
        public double Veeness { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the arrow annotation.
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

            ScreenPoint startPoint;
            var endPoint = this.XAxis.Transform(this.EndPoint.X, this.EndPoint.Y, this.YAxis);

            if (!this.ArrowDirection.x.IsZero() || !this.ArrowDirection.y.IsZero())
            {
                startPoint = endPoint - this.ArrowDirection;
            }
            else
            {
                startPoint = this.XAxis.Transform(this.StartPoint.X, this.StartPoint.Y, this.YAxis);
            }

            var d = endPoint - startPoint;
            d.Normalize();
            var n = new ScreenVector(d.Y, -d.X);

            var p1 = endPoint - d * this.HeadLength * this.StrokeThickness;
            var p2 = p1 + n * this.HeadWidth * this.StrokeThickness;
            var p3 = p1 - n * this.HeadWidth * this.StrokeThickness;
            var p4 = p1 + d * this.Veeness * this.StrokeThickness;

            OxyRect clippingRect = this.GetClippingRect();
            const double MinimumSegmentLength = 4;

            rc.DrawClippedLine(
                new[] { startPoint, p4 },
                clippingRect,
                MinimumSegmentLength * MinimumSegmentLength,
                this.Color,
                this.StrokeThickness,
                this.LineStyle,
                this.LineJoin,
                false);

            rc.DrawClippedPolygon(
                new[] { p3, endPoint, p2, p4 },
                clippingRect,
                MinimumSegmentLength * MinimumSegmentLength,
                this.Color,
                null);

            var ha = d.X < 0 ? HorizontalTextAlign.Left : HorizontalTextAlign.Right;
            var va = d.Y < 0 ? VerticalTextAlign.Top : VerticalTextAlign.Bottom;

            var textPoint = startPoint;
            rc.DrawClippedText(
                clippingRect,
                textPoint,
                this.Text,
                this.ActualTextColor,
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                0,
                ha,
                va);
        }

        #endregion
    }
}