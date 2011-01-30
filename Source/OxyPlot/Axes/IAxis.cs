namespace OxyPlot
{
    public enum AxisPosition
    {
        // cartesian axes:
        Left,
        Right,
        Top,
        Bottom,

        // polar axes:
        Angle,
        Magnitude
    }

    public enum LegendPosition
    {
        /* todo: Auto,*/
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight
    }

    public enum TickStyle
    {
        Crossing,
        Inside,
        Outside,
        None
    }

    public interface IAxis
    {
        string Title { get; set; }
        string Key { get; set; }
        bool IsVisible { get; set; }
        
        AxisPosition Position { get; set; }
        
        double ActualMinimum { get; set; }
        double ActualMaximum { get; set; }

        double Scale { get; set; }
        double Offset { get; set; }
        
        ScreenPoint MidPoint { get; set; }
        ScreenPoint ScreenMin { get; set; }
        ScreenPoint ScreenMax { get; set; }
        
        bool IsHorizontal();
        bool IsVertical();
        bool IsPolar();
        
        void UpdateActualMaxMin();
        void Include(double value);
        void SetScale(double scale);
        void UpdateIntervals(double width, double height);
        string FormatValue(double x);
        void Render(IRenderContext rc, PlotModel model);
        
        void UpdateTransform(OxyRect plotArea);
        double Transform(double x);
        ScreenPoint Transform(DataPoint dp, IAxis yAxis);
        double InverseTransform(double sx);


        void Reset();
        void Pan(double dx);
        void Zoom(double x1, double x2);
        void ZoomAt(double factor, double x);
    }
}