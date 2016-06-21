using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxyPlot.SharpDX
{
    internal interface IRenderUnit :IDisposable
    {
        void Render(RenderTarget renderTarget);
        bool CheckBounds(RectangleF viewport);
    }
}
