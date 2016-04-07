using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;

namespace OxyPlot.SharpDX
{
    public class GeometryRenderUnit : IRenderUnit
    {
        Geometry geometry;
        Brush fill;
        Brush stroke;
        StrokeStyle strokeStyle;
        float strokeWidth;

        public GeometryRenderUnit(Geometry geometry, Brush stroke, Brush fill, float strokeWidth, StrokeStyle strokeStyle)
        {
            this.geometry = geometry;
            this.fill = fill;
            this.stroke = stroke;
            this.strokeWidth = strokeWidth;
            this.strokeStyle = strokeStyle;

        }

        public void Render(RenderTarget renderTarget)
        {
            if (stroke != null)
            {
                if (strokeStyle != null)
                    renderTarget.DrawGeometry(geometry, stroke, strokeWidth, strokeStyle);
                else
                    renderTarget.DrawGeometry(geometry, stroke, strokeWidth);
            }

            if (fill != null)
                renderTarget.FillGeometry(geometry, fill);
        }

        public void Dispose()
        {
            if (strokeStyle != null)
                strokeStyle.Dispose();
            geometry.Dispose();

            fill = null;
            geometry = null;
            stroke = null;
            strokeStyle = null;
        }
    }
}
