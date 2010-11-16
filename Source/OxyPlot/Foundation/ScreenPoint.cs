namespace OxyPlot
{
    public struct ScreenPoint
    {
        public static readonly ScreenPoint Undefined = new ScreenPoint(double.NaN, double.NaN);

        internal double x;
        internal double y;

        public ScreenPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public override string ToString()
        {
            return x + " " + y;
        }
    }
}