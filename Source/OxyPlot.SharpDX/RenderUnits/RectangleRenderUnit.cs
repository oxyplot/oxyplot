// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleRenderUnit.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a Rectangle IRenderUnit implementation.
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
    /// Represents a Rectangle IRenderUnit implementation.
    /// </summary>
    internal class RectangleRenderUnit : IRenderUnit
    {
        /// <summary>
        /// The rectangle.
        /// </summary>
        private RectangleF rectangle;

        /// <summary>
        /// The stroke.
        /// </summary>
        private Brush stroke;

        /// <summary>
        /// The fill.
        /// </summary>
        private Brush fill;

        /// <summary>
        /// The thickness.
        /// </summary>
        private float thickness;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleRenderUnit" /> class.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="thickness">The thickness.</param>
        public RectangleRenderUnit(RectangleF rectangle, Brush stroke, Brush fill, float thickness)
        {
            this.rectangle = rectangle;
            this.stroke = stroke;
            this.fill = fill;
            this.thickness = thickness;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.stroke = null;
            this.fill = null;
        }

        /// <summary>
        /// Renders rectangle represented by current instance to render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        public void Render(RenderTarget renderTarget)
        {
            if (this.stroke != null)
            {
                renderTarget.DrawRectangle(this.rectangle, this.stroke, this.thickness);
            }

            if (this.fill != null)
            {
                renderTarget.FillRectangle(this.rectangle, this.fill);
            }        
        }

        /// <summary>
        /// Checks if current instance bounds intersects with viewport or not.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <returns>Return <c>True</c> if bounds intersects with viewport, otherwise <c>False</c>.</returns>
        public bool CheckBounds(RectangleF viewport)
        {
            return viewport.Intersects(this.rectangle);
        }
    }
}
