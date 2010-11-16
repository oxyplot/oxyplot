namespace OxyPlot
{
    public class OxyPen
    {
        public OxyPen(OxyColor c, double th, LineStyle ls)
        {
            Color = c;
            Thickness = th;
            DashArray = LineStyleHelper.GetDashArray(ls);
        }

        public OxyColor Color { get; set; }
        public double Thickness { get; set; }
        public double[] DashArray { get; set; }
    }
}