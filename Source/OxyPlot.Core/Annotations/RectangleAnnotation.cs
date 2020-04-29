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
    using System;

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
            this.MinimumX = double.NegativeInfinity;
            this.MaximumX = double.PositiveInfinity;
            this.MinimumY = double.NegativeInfinity;
            this.MaximumY = double.PositiveInfinity;
            this.TextRotation = 0;
            this.TextHorizontalAlignment = HorizontalAlignment.Center;
            this.TextVerticalAlignment = VerticalAlignment.Middle;
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
        /// Renders the rectangle annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public override void Render(IRenderContext rc)
        {
            base.Render(rc);

            var clippingRectangle = this.GetClippingRect();

            var p1 = this.InverseTransform(clippingRectangle.TopLeft);
            var p2 = this.InverseTransform(clippingRectangle.BottomRight);

            var x1 = double.IsNegativeInfinity(this.MinimumX) || double.IsNaN(this.MinimumX) ? Math.Min(p1.X, p2.X) : this.MinimumX;
            var x2 = double.IsPositiveInfinity(this.MaximumX) || double.IsNaN(this.MaximumX) ? Math.Max(p1.X, p2.X) : this.MaximumX;
            var y1 = double.IsNegativeInfinity(this.MinimumY) || double.IsNaN(this.MinimumY) ? Math.Min(p1.Y, p2.Y) : this.MinimumY;
            var y2 = double.IsPositiveInfinity(this.MaximumY) || double.IsNaN(this.MaximumY) ? Math.Max(p1.Y, p2.Y) : this.MaximumY;

            this.screenRectangle = new OxyRect(this.Transform(x1, y1), this.Transform(x2, y2));
            
            rc.DrawClippedRectangle(
                clippingRectangle,
                this.screenRectangle,
                this.GetSelectableFillColor(this.Fill),
                this.GetSelectableColor(this.Stroke),
                this.StrokeThickness,
                this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));

            if (string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            this.GetActualTextAlignment(out var ha, out var va);
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
                ha,
                va);
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
