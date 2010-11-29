using System.Collections.Generic;

namespace OxyPlot
{
    public abstract class AxisRendererBase 
    {
        protected const double AXIS_LEGEND_DIST = 4; // distance from axis number to axis legend
        protected const double TICK_DIST = 4; // distance from axis tick to number

        protected OxyPen ExtraPen;
        protected OxyPen MajorPen;
        protected OxyPen MajorTickPen;
        protected ICollection<double> MajorTickValues;
        protected OxyPen MinorPen;
        protected OxyPen MinorTickPen;
        protected ICollection<double> MinorTickValues;
        protected OxyPen ZeroPen;

        protected readonly PlotModel Plot;
        protected readonly IRenderContext rc;

        public AxisRendererBase(IRenderContext rc, PlotModel plot)
        {
            this.Plot = plot;
            this.rc = rc;
        }

        public virtual void Render(AxisBase axis)
        {
            if (axis == null)
                return;

            axis.GetTickValues(out MajorTickValues, out MinorTickValues);
            CreatePens(axis);
        }

       

        protected void GetTickPositions(AxisBase axis, TickStyle glt, double ticksize, AxisPosition position, out double x0, out double x1)
        {
            x0 = 0;
            x1 = 0;
            bool isTopOrLeft = position == AxisPosition.Top || position == AxisPosition.Left;
            double sign =  isTopOrLeft ? -1 : 1;
            switch (glt)
            {
                case TickStyle.Crossing:
                    x0 = -ticksize*sign*0.75;
                    x1 = ticksize * sign * 0.75;
                    break;
                case TickStyle.Inside:
                    x0 = -ticksize*sign;
                    break;
                case TickStyle.Outside:
                    x1 = ticksize*sign;
                    break;
            }
        }

        protected void CreatePens(AxisBase axis)
        {
            MinorPen = OxyPen.Create(axis.MinorGridlineColor, axis.MinorGridlineThickness, axis.MinorGridlineStyle);
            MajorPen = OxyPen.Create(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            MinorTickPen = OxyPen.Create(axis.TicklineColor, axis.MinorGridlineThickness, LineStyle.Solid);
            MajorTickPen = OxyPen.Create(axis.TicklineColor, axis.MajorGridlineThickness, LineStyle.Solid);
            ZeroPen = OxyPen.Create(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            ExtraPen = OxyPen.Create(axis.ExtraGridlineColor, axis.ExtraGridlineThickness, axis.ExtraGridlineStyle);
        }

        protected bool IsWithin(double d, double min, double max)
        {
            if (d < min) return false;
            if (d > max) return false;
            return true;
        }
    }
}