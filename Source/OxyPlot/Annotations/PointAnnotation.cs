// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointAnnotation.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents an annotation that shows a point.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Represents an annotation that shows a point.
    /// </summary>
    public class PointAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// The position transformed to screen coordinates.
        /// </summary>
        private ScreenPoint screenPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointAnnotation" /> class.
        /// </summary>
        public PointAnnotation()
        {
            this.Size = 4;
            this.TextMargin = 2;
            this.Shape = MarkerType.Circle;
            this.TextVerticalAlignment = VerticalAlignment.Top;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the center.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the center.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the size of the rendered point.
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// Gets or sets the distance between the rendered point and the text.
        /// </summary>
        public double TextMargin { get; set; }

        /// <summary>
        /// Gets or sets the shape of the rendered point.
        /// </summary>
        /// <value>The shape.</value>
        public MarkerType Shape { get; set; }

        /// <summary>
        /// Renders the polygon annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="model">The plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            this.screenPosition = this.Transform(this.X, this.Y);

            // clip to the area defined by the axes
            var clippingRectangle = this.GetClippingRect();

            rc.DrawMarker(clippingRectangle, this.screenPosition, this.Shape, null, this.Size, this.Fill, this.Stroke, this.StrokeThickness);

            if (!string.IsNullOrEmpty(this.Text))
            {
                var dx = -(int)this.TextHorizontalAlignment * (this.Size + this.TextMargin);
                var dy = -(int)this.TextVerticalAlignment * (this.Size + this.TextMargin);
                var textPosition = this.screenPosition + new ScreenVector(dx, dy);
                rc.DrawClippedText(
                    clippingRectangle,
                    textPosition,
                    this.Text,
                    this.ActualTextColor,
                    this.ActualFont,
                    this.ActualFontSize,
                    this.ActualFontWeight,
                    this.TextRotation,
                    this.TextHorizontalAlignment,
                    this.TextVerticalAlignment);
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
            if (this.screenPosition.DistanceTo(args.Point) < this.Size)
            {
                return new HitTestResult(this, this.screenPosition);
            }

            return null;
        }
    }
}