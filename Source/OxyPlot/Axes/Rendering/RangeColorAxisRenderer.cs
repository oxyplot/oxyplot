// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeColorAxisRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes.Rendering
{
    /// <summary>
    /// Provides functionality to render range color axes.
    /// </summary>
    public class RangeColorAxisRenderer : ColorAxisRenderer<RangeColorAxis>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeColorAxisRenderer" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public RangeColorAxisRenderer(IRenderContext rc, PlotModel plot) : base(rc, plot)
        {
        }

        /// <inheritdoc />
        protected override void RenderColorBlock(RangeColorAxis axis)
        {
            var effectiveMaxY = axis.Transform(axis.IsReversed ? axis.ActualMinimum : axis.ActualMaximum);
            var effectiveMinY = axis.Transform(axis.IsReversed ? axis.ActualMaximum : axis.ActualMinimum);

            foreach (var range in axis.ranges)
            {
                var ylow = axis.Transform(range.LowerBound);
                var yhigh = axis.Transform(range.UpperBound);

                if (axis.IsHorizontal())
                {
                    if (ylow < effectiveMinY)
                    {
                        ylow = effectiveMinY;
                    }

                    if (yhigh > effectiveMaxY)
                    {
                        yhigh = effectiveMaxY;
                    }
                }
                else
                {
                    if (ylow > effectiveMinY)
                    {
                        ylow = effectiveMinY;
                    }

                    if (yhigh < effectiveMaxY)
                    {
                        yhigh = effectiveMaxY;
                    }
                }

                this.DrawColorRect(axis, ylow, yhigh, range.Color);
            }

            double highLowLength = 10;
            if (axis.IsHorizontal())
            {
                highLowLength *= -1;
            }

            if (!axis.LowColor.IsUndefined())
            {
                var ylow = axis.Transform(axis.ActualMinimum);
                this.DrawColorRect(axis, ylow, ylow + highLowLength, axis.LowColor);
            }

            if (!axis.HighColor.IsUndefined())
            {
                var yhigh = axis.Transform(axis.ActualMaximum);
                this.DrawColorRect(axis, yhigh, yhigh - highLowLength, axis.HighColor);
            }
        }
    }
}
