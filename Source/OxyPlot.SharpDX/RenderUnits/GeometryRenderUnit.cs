// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeometryRenderUnit.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a Geometry IRenderUnit implementation.
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
    /// Represents a Geometry IRenderUnit implementation.
    /// </summary>
    internal class GeometryRenderUnit : IRenderUnit
    {
        /// <summary>
        /// The geometry.
        /// </summary>
        private Geometry geometry;

        /// <summary>
        /// The bounds.
        /// </summary>
        private RectangleF bounds;

        /// <summary>
        /// The fill.
        /// </summary>
        private Brush fill;

        /// <summary>
        /// The stroke.
        /// </summary>
        private Brush stroke;

        /// <summary>
        /// The stroke style.
        /// </summary>
        private StrokeStyle strokeStyle;

        /// <summary>
        /// The stroke width.
        /// </summary>
        private float strokeWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryRenderUnit" /> class.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="strokeWidth">The stroke width.</param>
        /// <param name="strokeStyle">The stroke style.</param>
        public GeometryRenderUnit(Geometry geometry, Brush stroke, Brush fill, float strokeWidth, StrokeStyle strokeStyle)
        {         
            this.geometry = geometry;
            this.fill = fill;
            this.stroke = stroke;
            this.strokeWidth = strokeWidth;
            this.strokeStyle = strokeStyle;            
            var raw = geometry.GetBounds();
            this.bounds = new RectangleF(raw.Left, raw.Top, raw.Right - raw.Left, raw.Bottom - raw.Top);
        }

        /// <summary>
        /// Renders geometry represented by current instance to render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        public void Render(RenderTarget renderTarget)
        {
            if (this.stroke != null)
            {
                if (this.strokeStyle != null)
                {
                    renderTarget.DrawGeometry(this.geometry, this.stroke, this.strokeWidth, this.strokeStyle);
                }
                else
                {
                    renderTarget.DrawGeometry(this.geometry, this.stroke, this.strokeWidth);
                }
            }

            if (this.fill != null)
            {
                renderTarget.FillGeometry(this.geometry, this.fill);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.strokeStyle != null)
            {
                this.strokeStyle.Dispose();
            }

            this.geometry.Dispose();

            this.fill = null;
            this.geometry = null;
            this.stroke = null;
            this.strokeStyle = null;
        }

        /// <summary>
        /// Checks if current instance bounds intersects with viewport or not.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <returns>Return <c>True</c> if bounds intersects with viewport, otherwise <c>False</c>.</returns>
        public bool CheckBounds(RectangleF viewport)
        {
            return this.bounds.Intersects(viewport);
        }
    }
}
