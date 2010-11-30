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

        public static OxyPen Create(OxyColor color, double thickness, LineStyle lineStyle)
        {
            if (lineStyle == LineStyle.None || thickness == 0)
            {
                return null;
            }

            return new OxyPen(color, thickness, lineStyle);
        }
    }
}