using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;

namespace OxyPlot.SharpDX
{
    class RectangleRenderUnit : IRenderUnit
    {
        RectangleF rectangle;
        Brush stroke;
        Brush fill;
        float thickness;
        public RectangleRenderUnit(RectangleF rectangle, Brush stroke, Brush fill, float thickness)
        {
            this.rectangle = rectangle;
            this.stroke = stroke;
            this.fill = fill;
            this.thickness = thickness;


        }
        public void Dispose()
        {
            stroke = null;
            fill = null;
        }

        public void Render(RenderTarget renderTarget)
        {
            if (stroke!=null)
                renderTarget.DrawRectangle(rectangle,stroke, thickness);

            if (fill != null)
                renderTarget.FillRectangle(rectangle, fill);           
        }

        public bool CheckBounds(RectangleF viewport)
        {
            return viewport.Intersects(rectangle);
        }
    }
}
