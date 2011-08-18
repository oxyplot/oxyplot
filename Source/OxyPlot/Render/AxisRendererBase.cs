using System.Collections.Generic;

namespace OxyPlot
{
    public abstract class AxisRendererBase
    {
        protected readonly PlotModel Plot;
        protected readonly IRenderContext rc;

        protected OxyPen ExtraPen;
        protected OxyPen MajorPen;
        protected OxyPen AxislinePen;
        protected OxyPen MajorTickPen;
        protected ICollection<double> MajorTickValues;
        protected ICollection<double> MinorTickValues;
        protected ICollection<double> MajorLabelValues;
        protected OxyPen MinorPen;
        protected OxyPen MinorTickPen;
        protected OxyPen ZeroPen;

        public AxisRendererBase(IRenderContext rc, PlotModel plot)
        {
            Plot = plot;
            this.rc = rc;
        }

        public virtual void Render(AxisBase axis)
        {
            if (axis == null)
            {
                return;
            }

            axis.GetTickValues(out MajorLabelValues, out MajorTickValues, out MinorTickValues);
            CreatePens(axis);
        }

        protected void GetTickPositions(AxisBase axis, TickStyle glt, double ticksize, AxisPosition position,
                                        out double x0, out double x1)
        {
            x0 = 0;
            x1 = 0;
            bool isTopOrLeft = position == AxisPosition.Top || position == AxisPosition.Left;
            double sign = isTopOrLeft ? -1 : 1;
            switch (glt)
            {
                case TickStyle.Crossing:
                    x0 = -ticksize * sign * 0.75;
                    x1 = ticksize * sign * 0.75;
                    break;
                case TickStyle.Inside:
                    x0 = -ticksize * sign;
                    break;
                case TickStyle.Outside:
                    x1 = ticksize * sign;
                    break;
            }
        }

        protected void CreatePens(AxisBase axis)
        {
            MinorPen = OxyPen.Create(axis.MinorGridlineColor, axis.MinorGridlineThickness, axis.MinorGridlineStyle);
            MajorPen = OxyPen.Create(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            MinorTickPen = OxyPen.Create(axis.TicklineColor, axis.MinorGridlineThickness, LineStyle.Solid);
            MajorTickPen = OxyPen.Create(axis.TicklineColor, axis.MajorGridlineThickness, LineStyle.Solid);
            ZeroPen = OxyPen.Create(axis.TicklineColor, axis.MajorGridlineThickness, LineStyle.Solid);
            ExtraPen = OxyPen.Create(axis.ExtraGridlineColor, axis.ExtraGridlineThickness, axis.ExtraGridlineStyle);
            AxislinePen = OxyPen.Create(axis.AxislineColor, axis.AxislineThickness, axis.AxislineStyle);
        }

        protected bool IsWithin(double d, double min, double max)
        {
            if (d < min)
            {
                return false;
            }

            if (d > max)
            {
                return false;
            }

            return true;
        }
    }
}