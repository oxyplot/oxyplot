namespace OxyPlot
{
    public struct OxySize
    {
        public static OxySize Empty = new OxySize(0, 0);

        public OxySize(double width, double height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public double Width { get; set; }
        public double Height { get; set; }
    }
}