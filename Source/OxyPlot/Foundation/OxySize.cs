namespace OxyPlot
{
    public struct OxySize
    {
        public double Width { get; set; }
        public double Height { get; set; }
        
        public static OxySize Empty = new OxySize(0,0);

        public OxySize(double width, double height)
            : this()
        {
            this.Width = width;
            this.Height = height;
        }
    }
}