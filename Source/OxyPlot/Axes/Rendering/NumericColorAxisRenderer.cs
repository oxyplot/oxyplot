// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericColorAxisRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes.Rendering
{
    using OxyPlot.Utilities;
    using System;

    /// <summary>
    /// Provides functionality to render numeric color axes.
    /// </summary>
    public class NumericColorAxisRenderer<T> : ColorAxisRenderer<T> where T : Axis, INumericColorAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericColorAxisRenderer{T}" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public NumericColorAxisRenderer(IRenderContext rc, PlotModel plot) : base(rc, plot)
        {
        }

        /// <inheritdoc />
        protected override void RenderColorBlock(T axis)
        {
            if (axis.Palette == null)
            {
                throw new InvalidOperationException("No Palette defined for color axis.");
            }

            if (axis.RenderAsImage)
            {
                var axisLength = axis.Transform(axis.ClipMaximum) - axis.Transform(axis.ClipMinimum);
                var reverse = axisLength < 0;
                axisLength = Math.Abs(axisLength);

                if (axis.IsHorizontal())
                {
                    var colorAxisImage = this.GenerateColorAxisImage(axis, reverse);
                    this.RenderContext.DrawImage(colorAxisImage, this.left, this.top, axisLength, this.size, 1, true);
                }
                else
                {
                    var colorAxisImage = this.GenerateColorAxisImage(axis, reverse);
                    this.RenderContext.DrawImage(colorAxisImage, this.left, this.top, this.size, axisLength, 1, true);
                }
            }
            else
            {
                for (var i = 0; i < axis.Palette.Colors.Count; i++)
                {
                    var ylow = this.Transform(axis, axis.GetLowValue(i));
                    var yhigh = this.Transform(axis, axis.GetHighValue(i));
                    this.DrawColorRect(axis, ylow, yhigh, axis.Palette.Colors[i]);
                }

                double highLowLength = 10;
                if (axis.IsHorizontal())
                {
                    highLowLength *= -1;
                }

                if (!axis.LowColor.IsUndefined())
                {
                    this.DrawColorRect(axis, this.minScreenPosition, this.minScreenPosition + highLowLength, axis.LowColor);
                }

                if (!axis.HighColor.IsUndefined())
                {
                    this.DrawColorRect(axis, this.maxScreenPosition, this.maxScreenPosition - highLowLength, axis.HighColor);
                }
            }
        }

        /// <summary>
        /// Generates the image used to render the color axis.
        /// </summary>
        /// <param name="axis">The color axis.</param>
        /// <param name="reverse">Reverse the colors if set to <c>true</c>.</param>
        /// <returns>An <see cref="OxyImage" /> used to render the color axis.</returns>
        private OxyImage GenerateColorAxisImage(T axis, bool reverse)
        {
            var n = axis.Palette.Colors.Count;
            var buffer = axis.IsHorizontal() ? new OxyColor[n, 1] : new OxyColor[1, n];
            for (var i = 0; i < n; i++)
            {
                var color = axis.Palette.Colors[i];
                var i2 = reverse ? n - 1 - i : i;
                if (axis.IsHorizontal())
                {
                    buffer[i2, 0] = color;
                }
                else
                {
                    buffer[0, i2] = color;
                }
            }

            return OxyImage.Create(buffer, ImageFormat.Png);
        }

        /// <summary>
        /// Transforms a value to a screen position. We don't use the regular Transform functions of the axis here, as the color block should always be drawn with linear scaling.
        /// </summary>
        /// <param name="axis">The color axis.</param>
        /// <param name="value">The value.</param>
        /// <returns>The transformed value.</returns>
        private double Transform(T axis, double value)
        {
            return Helpers.LinearInterpolation(axis.ClipMinimum, this.minScreenPosition, axis.ClipMaximum, this.maxScreenPosition, value);
        }
    }
}
