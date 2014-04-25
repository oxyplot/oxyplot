// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EllipseAnnotation.cs" company="OxyPlot">
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
//   Represents an annotation that shows an ellipse.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Represents an annotation that shows an ellipse.
    /// </summary>
    public class EllipseAnnotation : TextualAnnotation
    {
        /// <summary>
        /// The rectangle transformed to screen coordinates.
        /// </summary>
        private OxyRect screenRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseAnnotation" /> class.
        /// </summary>
        public EllipseAnnotation()
        {
            this.Stroke = OxyColors.Black;
            this.Fill = OxyColors.LightBlue;
            this.Width = double.NaN;
            this.Height = double.NaN;
        }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        /// <value>The fill.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public OxyColor Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the center.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the center.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the ellipse.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the ellipse.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Renders the polygon annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="model">The plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            this.screenRectangle = new OxyRect(this.Transform(this.X - (this.Width / 2), this.Y - (this.Height / 2)), this.Transform(this.X + (this.Width / 2), this.Y + (this.Height / 2)));

            // clip to the area defined by the axes
            var clipping = this.GetClippingRect();

            rc.DrawClippedEllipse(clipping, this.screenRectangle, this.Fill, this.Stroke, this.StrokeThickness);

            if (!string.IsNullOrEmpty(this.Text))
            {
                var textPosition = this.GetActualTextPosition(() => this.screenRectangle.Center);
                rc.DrawClippedText(
                    clipping,
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
            if (this.screenRectangle.Contains(args.Point))
            {
                return new HitTestResult(args.Point);
            }

            return null;
        }
    }
}