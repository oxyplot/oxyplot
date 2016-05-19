using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;

namespace OxyPlot.SharpDX
{
    class EllipseRenderUnit : IRenderUnit
    {
        RectangleF bounds;
        Ellipse ellipse;
        Brush stroke;
        Brush fill;
        float thickness;
        public EllipseRenderUnit(Ellipse ellipse, Brush stroke, Brush fill, float thickness)
        {
            this.ellipse = ellipse;
            this.bounds = new RectangleF(ellipse.Point.X - ellipse.RadiusX, ellipse.Point.Y - ellipse.RadiusY, ellipse.RadiusX * 2, ellipse.RadiusY * 2);
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
            if (stroke != null)
                renderTarget.DrawEllipse(ellipse, stroke, thickness);

            if (fill != null)
                renderTarget.FillEllipse(ellipse, fill);
        }

        public bool CheckBounds(RectangleF viewport)
        {
            return viewport.Intersects(bounds);
        }
    }
}
