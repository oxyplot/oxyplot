// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotation.cs" company="OxyPlot">
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
//   Represents an annotation that shows a polygon.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an annotation that shows a polygon.
    /// </summary>
    public class PolygonAnnotation : TextualAnnotation
    {
        /// <summary>
        /// The polygon points transformed to screen coordinates.
        /// </summary>
        private IList<ScreenPoint> screenPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonAnnotation" /> class.
        /// </summary>
        public PolygonAnnotation()
        {
            this.Color = OxyColors.Blue;
            this.Fill = OxyColors.LightBlue;
            this.StrokeThickness = 1;
            this.LineStyle = LineStyle.Solid;
            this.LineJoin = OxyPenLineJoin.Miter;
            this.Points = new List<DataPoint>();
        }

        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        /// <value>The fill.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>The points.</value>
        public List<DataPoint> Points { get; private set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Renders the polygon annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="model">The plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);
            if (this.Points == null)
            {
                return;
            }

            // transform to screen coordinates
            this.screenPoints = this.Points.Select(this.Transform).ToList();
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
                var textPosition = this.GetActualTextPosition(() => ScreenPointHelper.GetCentroid(this.screenPoints));

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
            if (this.screenPoints == null)
            {
                // Points not specified.
                return null;
            }

            return ScreenPointHelper.IsPointInPolygon(args.Point, this.screenPoints) ? new HitTestResult(this, args.Point) : null;
        }
    }
}