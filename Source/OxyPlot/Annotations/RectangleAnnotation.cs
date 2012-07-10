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
    /// Represents a rectangle annotation.
    /// </summary>
    public class RectangleAnnotation : Annotation
    {
        #region Constants and Fields

        /// <summary>
        ///   The rectangle transformed to screen coordinates.
        /// </summary>
        private OxyRect screenRectangle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonAnnotation"/> class. 
        /// </summary>
        public RectangleAnnotation()
        {
            this.Fill = OxyColors.LightBlue;
            this.MinimumX = double.NaN;
            this.MaximumX = double.NaN;
            this.MinimumY = double.NaN;
            this.MaximumY = double.NaN;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the fill color.
        /// </summary>
        /// <value> The fill. </value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the minimum X.
        /// </summary>
        /// <value>The minimum X.</value>
        public double MinimumX { get; set; }

        /// <summary>
        /// Gets or sets the maximum X.
        /// </summary>
        /// <value>The maximum X.</value>
        public double MaximumX { get; set; }

        /// <summary>
        /// Gets or sets the minimum Y.
        /// </summary>
        /// <value>The minimum Y.</value>
        public double MinimumY { get; set; }

        /// <summary>
        /// Gets or sets the maximum Y.
        /// </summary>
        /// <value>The maximum Y.</value>
        public double MaximumY { get; set; }

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

            double x0 = double.IsNaN(this.MinimumX) ? this.XAxis.ActualMinimum : this.MinimumX;
            double x1 = double.IsNaN(this.MaximumX) ? this.XAxis.ActualMaximum : this.MaximumX;
            double y0 = double.IsNaN(this.MinimumY) ? this.YAxis.ActualMinimum : this.MinimumY;
            double y1 = double.IsNaN(this.MaximumY) ? this.YAxis.ActualMaximum : this.MaximumY;

            screenRectangle = OxyRect.Create(this.Transform(x0, y0), this.Transform(x1, y1));

            // clip to the area defined by the axes
            var clipping = this.GetClippingRect();

            const double MinimumSegmentLength = 4;

            rc.DrawClippedRectangle(screenRectangle, clipping, this.Fill, null, 0);

            if (!string.IsNullOrEmpty(this.Text))
            {
                var textPosition = screenRectangle.Center;
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
            if (screenRectangle.Contains(point))
            {
                return new HitTestResult(point);
            }

            return null;
        }

        #endregion
    }
}