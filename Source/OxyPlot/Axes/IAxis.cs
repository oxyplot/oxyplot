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
    } ;

    public enum LegendPosition
    {
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
    } ;

    public interface IAxis
    {
        void Render(IRenderContext rc, PlotModel model);

        // todo... clean up interface/Axis base class
        //double UpdateTransform(double x0, double x1, double y0, double y1);
        //void UpdateIntervals(double length, double labelSize);
        //void Include(double p);
        //double Transform(double x, double y);
        //double InverseTransform(double x, double y);
        //void Pan(double dx);
        //void Reset();
        //void SetScale(double scale);
        //void ScaleAt(double factor, double x);
        //void Zoom(double x0, double x1);
    }
}