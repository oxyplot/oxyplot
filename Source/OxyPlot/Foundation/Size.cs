namespace OxyPlot
{
    public struct Size
    {
        public double Width { get; set; }
        public double Height { get; set; }
        
        public static Size Empty = new Size(0,0);

        public Size(double width, double height)
            : this()
        {
            this.Width = width;
            this.Height = height;
        }
    }
}