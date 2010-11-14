using System;

namespace OxyPlot
{
    public class PlotRenderer
    {
        protected readonly PlotModel plot;
        protected readonly IRenderContext rc;
        protected Rectangle plotBounds;

        public PlotRenderer(IRenderContext rc, PlotModel p)
        {
            this.rc = rc;
            plot = p;
            plotBounds = new Rectangle
                             {
                                 Left = p.MarginLeft,
                                 Right = rc.Width - p.MarginRight,
                                 Top = p.MarginTop,
                                 Bottom = rc.Height - p.MarginBottom
                             };
        }

        public void RenderTitle(string title, string subtitle)
        {
            Size size1 = rc.MeasureText(title, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            Size size2 = rc.MeasureText(subtitle, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            double height = size1.Height + size2.Height;
            double dy = (plot.MarginTop - height) * 0.5;
            double dx = (plotBounds.Left + plotBounds.Right) * 0.5;

            if (!String.IsNullOrEmpty(title))
                rc.DrawText(
                    new Point(dx, dy), title, plot.TextColor,
                    plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight,
                    0,
                    HorizontalTextAlign.Center, VerticalTextAlign.Top);
            if (!String.IsNullOrEmpty(subtitle))
                rc.DrawText(new Point(dx, dy + size1.Height), subtitle, plot.TextColor,
                            plot.TitleFont, plot.SubtitleFontSize, plot.SubtitleFontWeight, 0,
                            HorizontalTextAlign.Center, VerticalTextAlign.Top);
        }
    }
}