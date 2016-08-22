// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageRenderUnit.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a Image IRenderUnit implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.SharpDX
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using global::SharpDX;
    using global::SharpDX.Direct2D1;

    /// <summary>
    /// Represents a Image IRenderUnit implementation.
    /// </summary>
    internal class ImageRenderUnit : IRenderUnit
    {
        /// <summary>
        /// The bitmap.
        /// </summary>
        private Bitmap bitmap;

        /// <summary>
        /// The source rectangle.
        /// </summary>
        private RectangleF src;

        /// <summary>
        /// The destination rectangle.
        /// </summary>
        private RectangleF dest;

        /// <summary>
        /// The opacity.
        /// </summary>
        private float opacity;

        /// <summary>
        /// The interpolation mode.
        /// </summary>
        private BitmapInterpolationMode mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRenderUnit" /> class.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="src">The source rectangle.</param>
        /// <param name="dest">The destination rectangle.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="mode">The interpolation mode.</param>
        public ImageRenderUnit(
            Bitmap bitmap,
            RectangleF src,
            RectangleF dest,
            float opacity,
            BitmapInterpolationMode mode)
        {
            this.bitmap = bitmap;
            this.src = src;
            this.dest = dest;
            this.opacity = opacity;
            this.mode = mode;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.bitmap = null;
        }

        /// <summary>
        /// Renders image represented by current instance to render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        public void Render(RenderTarget renderTarget)
        {
            renderTarget.DrawBitmap(this.bitmap, this.dest, this.opacity, this.mode, this.src);
        }

        /// <summary>
        /// Checks if current instance bounds intersects with viewport or not.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <returns>Return <c>True</c> if bounds intersects with viewport, otherwise <c>False</c>.</returns>
        public bool CheckBounds(RectangleF viewport)
        {
            return viewport.Intersects(this.dest);
        }
    }
}
