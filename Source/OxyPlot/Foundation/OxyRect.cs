namespace OxyPlot
{
    public struct OxyRect
    {
        private double bottom;
        private double left;
        private double right;
        private double top;

        public OxyRect(double left, double top, double width, double height)
        {
            this.left = left;
            this.top = top;
            this.right= left+width;
            this.bottom = top+height;
        }

        public double Top
        {
            get { return top; }
            set { top = value; }
        }

        public double Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        public double Left
        {
            get { return left; }
            set { left = value; }
        }

        public double Right
        {
            get { return right; }
            set { right = value; }
        }

        public double Width
        {
            get { return Right - Left; }
        }

        public double Height
        {
            get { return Bottom - Top; }
        }
    }
}