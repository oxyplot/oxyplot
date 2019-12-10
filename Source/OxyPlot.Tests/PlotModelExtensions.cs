// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for <see cref="IPlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    /// <summary>
    /// Provides extension methods for <see cref="IPlotModel" />.
    /// </summary>
    public static class PlotModelExtensions
    {
        /// <summary>
        /// Updates the specified plot model and renders to null.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width of the output surface.</param>
        /// <param name="height">The height the output surface.</param>
        /// <remarks>This method is useful to simulate rendering in the unit tests.</remarks>
        public static void UpdateAndRenderToNull(this IPlotModel model, double width, double height)
        {
            var rc = new NullRenderContext();
            model.Update(true);
            model.Render(rc, new OxyRect(0, 0, width, height));
        }
    }
}
