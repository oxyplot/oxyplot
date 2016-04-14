using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;

namespace OxyPlot.SharpDX
{
    class ImageRenderUnit : IRenderUnit
    {
        Bitmap bitmap;
        RectangleF src;
        RectangleF dest;
        float opacity;
        BitmapInterpolationMode mode;


        public ImageRenderUnit(Bitmap bitmap,
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

        public void Dispose()
        {
            bitmap = null;
        }

        public void Render(RenderTarget renderTarget)
        {
            renderTarget.DrawBitmap(bitmap, dest, opacity, mode, src);
        }

        public bool CheckBounds(RectangleF viewport)
        {
            return viewport.Intersects(dest);
        }
    }
}
