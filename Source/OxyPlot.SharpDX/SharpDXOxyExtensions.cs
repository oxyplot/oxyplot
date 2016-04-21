using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXLineJoin = SharpDX.Direct2D1.LineJoin;

namespace OxyPlot.SharpDX
{
    public static class SharpDXOxyExtensions
    {
        public static Vector2 ToVector2(this ScreenPoint point, bool aliased = false)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            if (aliased)
            {
                return new Vector2(0.5f + (int)point.X, 0.5f + (int)point.Y);
            }
            return new Vector2((float)point.X, (float)point.Y);
        }

        public static Ellipse ToEllipse(this OxyRect rect)
        {
            return new Ellipse(rect.Center.ToVector2(), (float)rect.Width / 2, (float)rect.Height / 2);
        }

        public static RectangleF ToRectangleF(this OxyRect rect)
        {
            return new RectangleF((float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

        public static DXLineJoin ToDXLineJoin(this LineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case LineJoin.Miter:
                    return DXLineJoin.Miter;
                case LineJoin.Round:
                    return DXLineJoin.Round;
                case LineJoin.Bevel:
                    return DXLineJoin.Bevel;
                default:
                    return DXLineJoin.MiterOrBevel;
            }

        }

        public static Color4 ToDXColor(this OxyColor color)
        {
            return new Color4(color.R * 1f / 255f, color.G * 1f / 255f, color.B * 1f / 255f, color.A * 1f / 255f);
        }
    }
}
