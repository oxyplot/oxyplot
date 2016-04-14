using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.DirectWrite;

namespace OxyPlot.SharpDX
{
    class TextRenderUnit : IRenderUnit
    {

        Matrix3x2 transform;
        TextLayout layout;
        Brush brush;
        RectangleF bounds;

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
                Left = topleft.X ,
                Right = bottomRight.X,
                Bottom = bottomRight.Y
            };
                


        }
        public void Dispose()
        {
            layout.Dispose();
            brush = null;
        }

        public void Render(RenderTarget renderTarget)
        {
            var currentTransform = renderTarget.Transform;
            renderTarget.Transform = transform * currentTransform;

            renderTarget.DrawTextLayout(new Vector2(), layout, brush);

            renderTarget.Transform = currentTransform;

        }

        public bool CheckBounds(RectangleF viewport)
        {
            return viewport.Intersects(bounds);
        }
    }
}
