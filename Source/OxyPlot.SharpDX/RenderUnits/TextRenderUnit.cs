// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextRenderUnit.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a Text IRenderUnit implementation.
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
    using global::SharpDX.DirectWrite;

    /// <summary>
    /// Represents a Text IRenderUnit implementation.
    /// </summary>
    internal class TextRenderUnit : IRenderUnit
    {
        /// <summary>
        /// The text transform.
        /// </summary>
        private Matrix3x2 transform;

        /// <summary>
        /// The text layout.
        /// </summary>
        private TextLayout layout;

        /// <summary>
        /// The brush.
        /// </summary>
        private Brush brush;

        /// <summary>
        /// The bounds.
        /// </summary>
        private RectangleF bounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRenderUnit" /> class.
        /// </summary>
        /// <param name="textLayout">The text layout.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="transform">The text transform.</param>
        public TextRenderUnit(TextLayout textLayout, Brush brush, Matrix3x2 transform)
        {
            this.layout = textLayout;
            this.brush = brush;
            this.transform = transform;

            var topleft = Matrix3x2.TransformPoint(transform, new Vector2(0, 0));
            var bottomRight = Matrix3x2.TransformPoint(transform, new Vector2(textLayout.Metrics.Width, textLayout.Metrics.Height));

            this.bounds = new RectangleF
            {
                Top = topleft.Y,
                Left = topleft.X,
                Right = bottomRight.X,
                Bottom = bottomRight.Y
            };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.layout.Dispose();
            this.brush = null;
        }

        /// <summary>
        /// Renders text represented by current instance to render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        public void Render(RenderTarget renderTarget)
        {
            var currentTransform = renderTarget.Transform;
            renderTarget.Transform = this.transform * currentTransform;

            renderTarget.DrawTextLayout(new Vector2(), this.layout, this.brush);

            renderTarget.Transform = currentTransform;
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
