// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryColorAxisRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes.Rendering
{
    /// <summary>
    /// Provides functionality to render category color axes.
    /// </summary>
    public class CategoryColorAxisRenderer : ColorAxisRenderer<CategoryColorAxis>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryColorAxisRenderer" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public CategoryColorAxisRenderer(IRenderContext rc, PlotModel plot) : base(rc, plot)
        {
        }

        /// <inheritdoc />
        protected override void RenderColorBlock(CategoryColorAxis axis)
        {
            axis.GetTickValues(out var majorLabelValues, out _, out _);

            for (var i = 0; i < axis.Palette.Colors.Count; i++)
            {
                var low = axis.Transform(axis.GetLowValue(i, majorLabelValues));
                var high = axis.Transform(axis.GetHighValue(i, majorLabelValues));
                this.DrawColorRect(axis, low, high, axis.Palette.Colors[i]);
            }
        }
    }
}
