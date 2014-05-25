// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlotModel.cs" company="OxyPlot">
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
//   Specifies functionality for the plot model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality for the plot model.
    /// </summary>
    public interface IPlotModel
    {
        /// <summary>
        /// Gets the color of the background of the plot.
        /// </summary>
        /// <value>The color.</value>
        OxyColor Background { get; }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="updateData">if set to <c>true</c> , all data collections will be updated.</param>
        void Update(bool updateData);

        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        void Render(IRenderContext rc, double width, double height);

        /// <summary>
        /// Attaches this model to the specified plot view.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        /// <remarks>Only one plot view can be attached to the plot model.
        /// The plot model contains data (e.g. axis scaling) that is only relevant to the current plot view.</remarks>
        void AttachPlotView(IPlotView plotView);
    }
}