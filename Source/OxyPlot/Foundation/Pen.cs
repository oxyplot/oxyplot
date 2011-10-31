// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pen.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    public class Pen
    {
        public Pen(Color c, double th, LineStyle ls)
        {
            Color = c;
            Thickness = th;
            DashArray = LineStyleHelper.GetDashArray(ls);
        }

        public Color Color { get; set; }
        public double Thickness { get; set; }
        public double[] DashArray { get; set; }
    }
}