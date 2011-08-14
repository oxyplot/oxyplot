using System;
using System.Diagnostics;

namespace OxyPlot
{
    public struct OxyRect
    {
        private double height;
        private double left;
        private double top;
        private double width;

        public OxyRect(double left, double top, double width, double height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            Debug.Assert(width >= 0 && height >= 0);
        }

        public static OxyRect Create(double x0, double y0, double x1, double y1)
        {
            return new OxyRect(Math.Min(x0,x1), Math.Min(y0,y1),Math.Abs(x1-x0),Math.Abs(y1-y0)); 
        }

        public double Top
        {
            get { return top; }
            set { top = value; }
        }

        public double Bottom
        {
            get { return top + height; }
            set { height = value - top; }
        }

        public double Left
        {
            get { return left; }
            set { left = value; }
        }

        public double Right
        {
            get { return left + width; }
            set { width = value - left; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}