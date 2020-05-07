// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderTarget.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp
{
    /// <summary>
    /// Defines a render target for <see cref="SkiaRenderContext"/>.
    /// </summary>
    public enum RenderTarget
    {
        /// <summary>
        /// Indicates that the <see cref="SkiaRenderContext"/> renders to a screen.
        /// </summary>
        /// <remarks>
        /// The render context may try to snap shapes to device pixels and will use sub-pixel text rendering.
        /// </remarks>
        Screen,

        /// <summary>
        /// Indicates that the <see cref="SkiaRenderContext"/> renders to a pixel graphic.
        /// </summary>
        /// <remarks>
        /// The render context may try to snap shapes to pixels, but will not use sub-pixel text rendering.
        /// </remarks>
        PixelGraphic,

        /// <summary>
        /// Indicates that the <see cref="SkiaRenderContext"/> renders to a vector graphic.
        /// </summary>
        /// <remarks>
        /// The render context will not use any rendering enhancements that are specific to pixel graphics.
        /// </remarks>
        VectorGraphic
    }
}
