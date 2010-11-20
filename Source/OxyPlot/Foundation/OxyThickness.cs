namespace OxyPlot
{
    public struct OxyThickness
    {
        private double bottom;
        private double left;
        private double right;
        private double top;

        public OxyThickness(double thickness)
            : this(thickness, thickness, thickness, thickness)
        {
        }

        public OxyThickness(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
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