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

        public TextRenderUnit(TextLayout textLayout, Brush brush, Matrix3x2 transform)
        {
            layout = textLayout;
            this.brush = brush;
            this.transform = transform;

        }
        public void Dispose()
        {
            layout.Dispose();
            brush = null;
        }

        public void Render(RenderTarget renderTarget)
        {
            var currentTransform = renderTarget.Transform;
            renderTarget.Transform = transform;

            renderTarget.DrawTextLayout(new Vector2(), layout, brush);

            renderTarget.Transform = currentTransform;

        }
    }
}
