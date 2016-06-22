// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EllipseRenderUnit.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a Ellipse IRenderUnit implementation.
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
    /// Represents a Ellipse IRenderUnit implementation.
    /// </summary>
    internal class EllipseRenderUnit : IRenderUnit
    {
        /// <summary>
        /// The bounds.
        /// </summary>
        private RectangleF bounds;

        /// <summary>
        /// The ellipse.
        /// </summary>
        private Ellipse ellipse;

        /// <summary>
        /// The stroke.
        /// </summary>
        private Brush stroke;

        /// <summary>
        /// The fill.
        /// </summary>
        private Brush fill;

        /// <summary>
        /// The thickness;
        /// </summary>
        private float thickness;

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseRenderUnit" /> class.
        /// </summary>
        /// <param name="ellipse">The ellipse.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="thickness">The thickness.</param>
        public EllipseRenderUnit(Ellipse ellipse, Brush stroke, Brush fill, float thickness)
        {
            this.ellipse = ellipse;
            this.bounds = new RectangleF(ellipse.Point.X - ellipse.RadiusX, ellipse.Point.Y - ellipse.RadiusY, ellipse.RadiusX * 2, ellipse.RadiusY * 2);
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
        /// Renders ellipse represented by current instance to render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        public void Render(RenderTarget renderTarget)
        {
            if (this.stroke != null)
            {
                renderTarget.DrawEllipse(this.ellipse, this.stroke, this.thickness);
            }

            if (this.fill != null)
            {
                renderTarget.FillEllipse(this.ellipse, this.fill);
            }
        }

        /// <summary>
        /// Checks if current instance bounds intersects with viewport or not.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <returns>Return <c>True</c> if bounds intersects with viewport, otherwise <c>False</c>.</returns>
        public bool CheckBounds(RectangleF viewport)
        {
            return viewport.Intersects(this.bounds);
        }
    }
}
