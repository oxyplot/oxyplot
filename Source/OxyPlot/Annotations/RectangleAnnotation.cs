// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an annotation that shows a rectangle.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Represents an annotation that shows a rectangle.
    /// </summary>
    public class RectangleAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// The rectangle transformed to screen coordinates.
        /// </summary>
        private OxyRect screenRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleAnnotation" /> class.
        /// </summary>
        public RectangleAnnotation()
        {
            this.MinimumX = double.MinValue;
            this.MaximumX = double.MaxValue;
            this.MinimumY = double.MinValue;
            this.MaximumY = double.MaxValue;
            this.TextRotation = 0;
        }

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

        /// <summary>
        /// Renders the polygon annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public override void Render(IRenderContext rc)
        {
            base.Render(rc);

            double x0 = double.IsNaN(this.MinimumX) || this.MinimumX.Equals(double.MinValue)
                            ? this.XAxis.ActualMinimum
                            : this.MinimumX;
            double x1 = double.IsNaN(this.MaximumX) || this.MaximumX.Equals(double.MaxValue)
                            ? this.XAxis.ActualMaximum
                            : this.MaximumX;
            double y0 = double.IsNaN(this.MinimumY) || this.MinimumY.Equals(double.MinValue)
                            ? this.YAxis.ActualMinimum
                            : this.MinimumY;
            double y1 = double.IsNaN(this.MaximumY) || this.MaximumY.Equals(double.MaxValue)
                            ? this.YAxis.ActualMaximum
                            : this.MaximumY;

            this.screenRectangle = new OxyRect(this.Transform(x0, y0), this.Transform(x1, y1));

            // clip to the area defined by the axes
            var clippingRectangle = this.GetClippingRect();

            rc.DrawClippedRectangle(
                clippingRectangle,
                this.screenRectangle,
                this.GetSelectableFillColor(this.Fill),
                this.GetSelectableColor(this.Stroke),
                this.StrokeThickness);

            if (!string.IsNullOrEmpty(this.Text))
            {
                var textPosition = this.GetActualTextPosition(() => this.screenRectangle.Center);
                rc.DrawClippedText(
                    clippingRectangle,
                    textPosition,
                    this.Text,
                    this.ActualTextColor,
                    this.ActualFont,
                    this.ActualFontSize,
                    this.ActualFontWeight,
                    this.TextRotation,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Middle);
            }
        }

        /// <summary>
        /// When overridden in a derived class, tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// The result of the hit test.
        /// </returns>
        protected override HitTestResult HitTestOverride(HitTestArguments args)
        {
            if (this.screenRectangle.Contains(args.Point))
            {
                return new HitTestResult(this, args.Point);
            }

            return null;
        }
    }
}