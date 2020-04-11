// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an annotation that shows text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an annotation that shows text.
    /// </summary>
    public class TextAnnotation : TextualAnnotation
    {
        /// <summary>
        /// The actual bounds of the text.
        /// </summary>
        private IList<ScreenPoint> actualBounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAnnotation" /> class.
        /// </summary>
        public TextAnnotation()
        {
            this.Stroke = OxyColors.Black;
            this.Background = OxyColors.Undefined;
            this.StrokeThickness = 1;
            this.TextVerticalAlignment = VerticalAlignment.Bottom;
            this.Padding = new OxyThickness(4);
        }

        /// <summary>
        /// Gets or sets the fill color of the background rectangle.
        /// </summary>
        /// <value>The background.</value>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets or sets the position offset (screen coordinates).
        /// </summary>
        /// <value>The offset.</value>
        public ScreenVector Offset { get; set; }

        /// <summary>
        /// Gets or sets the padding of the background rectangle.
        /// </summary>
        /// <value>The padding.</value>
        public OxyThickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the stroke color of the background rectangle.
        /// </summary>
        /// <value>The stroke color.</value>
        public OxyColor Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness of the background rectangle.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Renders the text annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public override void Render(IRenderContext rc)
        {
            base.Render(rc);

            var position = this.Transform(this.TextPosition) + this.Orientate(this.Offset);
            var clippingRectangle = this.GetClippingRect();
            var textSize = rc.MeasureText(this.Text, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);
            this.GetActualTextAlignment(out var ha, out var va);

            rc.SetClip(clippingRectangle);
            this.actualBounds = GetTextBounds(position, textSize, this.Padding, this.TextRotation, ha, va);

            if ((this.TextRotation % 90).Equals(0))
            {
                var actualRect = new OxyRect(this.actualBounds[0], this.actualBounds[2]);
                rc.DrawRectangle(actualRect, this.Background, this.Stroke, this.StrokeThickness, this.EdgeRenderingMode);
            }
            else
            {
                rc.DrawPolygon(this.actualBounds, this.Background, this.Stroke, this.StrokeThickness, this.EdgeRenderingMode);
            }


            rc.DrawMathText(
                position,
                this.Text,
                this.GetSelectableFillColor(this.ActualTextColor),
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                this.TextRotation,
                ha,
                va);

            rc.ResetClip();
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
            if (this.actualBounds == null)
            {
                return null;
            }

            // Todo: see if performance can be improved by checking rectangle (with rotation and alignment), not polygon
            return ScreenPointHelper.IsPointInPolygon(args.Point, this.actualBounds) ? new HitTestResult(this, args.Point) : null;
        }

        /// <summary>
        /// Gets the coordinates of the (rotated) background rectangle.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="padding">The padding.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        /// <returns>The background rectangle coordinates.</returns>
        private static IList<ScreenPoint> GetTextBounds(
            ScreenPoint position,
            OxySize size,
            OxyThickness padding,
            double rotation,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment)
        {
            double left, right, top, bottom;
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    left = -size.Width * 0.5;
                    right = -left;
                    break;
                case HorizontalAlignment.Right:
                    left = -size.Width;
                    right = 0;
                    break;
                default:
                    left = 0;
                    right = size.Width;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Middle:
                    top = -size.Height * 0.5;
                    bottom = -top;
                    break;
                case VerticalAlignment.Bottom:
                    top = -size.Height;
                    bottom = 0;
                    break;
                default:
                    top = 0;
                    bottom = size.Height;
                    break;
            }

            double cost = Math.Cos(rotation / 180 * Math.PI);
            double sint = Math.Sin(rotation / 180 * Math.PI);
            var u = new ScreenVector(cost, sint);
            var v = new ScreenVector(-sint, cost);
            var polygon = new ScreenPoint[4];
            polygon[0] = position + (u * (left - padding.Left)) + (v * (top - padding.Top));
            polygon[1] = position + (u * (right + padding.Right)) + (v * (top - padding.Top));
            polygon[2] = position + (u * (right + padding.Right)) + (v * (bottom + padding.Bottom));
            polygon[3] = position + (u * (left - padding.Left)) + (v * (bottom + padding.Bottom));
            return polygon;
        }
    }
}
