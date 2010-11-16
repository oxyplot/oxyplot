using System;

namespace OxyPlot
{
    public class PlotRenderer
    {
        protected readonly PlotModel plot;
        protected readonly IRenderContext rc;
        internal OxyRect plotBounds;
        protected ScreenPoint midPoint;

        public PlotRenderer(IRenderContext rc, PlotModel p)
        {
            this.rc = rc;
            plot = p;
            plotBounds = new OxyRect
                             {
                                 Left = p.MarginLeft,
                                 Right = rc.Width - p.MarginRight,
                                 Top = p.MarginTop,
                                 Bottom = rc.Height - p.MarginBottom
                             };
            midPoint = new ScreenPoint((plotBounds.Left + plotBounds.Right) * 0.5, (plotBounds.Top + plotBounds.Bottom) * 0.5);
        }

        public void RenderTitle(string title, string subtitle)
        {
            OxySize size1 = rc.MeasureText(title, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            OxySize size2 = rc.MeasureText(subtitle, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            double height = size1.Height + size2.Height;
            double dy = (plot.MarginTop - height) * 0.5;
            double dx = (plotBounds.Left + plotBounds.Right) * 0.5;

            if (!String.IsNullOrEmpty(title))
                rc.DrawText(
                    new ScreenPoint(dx, dy), title, plot.TextColor,
                    plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight,
                    0,
                    HorizontalTextAlign.Center, VerticalTextAlign.Top);
            if (!String.IsNullOrEmpty(subtitle))
                rc.DrawText(new ScreenPoint(dx, dy + size1.Height), subtitle, plot.TextColor,
                            plot.TitleFont, plot.SubtitleFontSize, plot.SubtitleFontWeight, 0,
                            HorizontalTextAlign.Center, VerticalTextAlign.Top);
        }

        public void RenderBorder()
        {
            var border = new[]
                             {
                                 new ScreenPoint(plotBounds.Left, plotBounds.Top), new ScreenPoint(plotBounds.Right, plotBounds.Top),
                                 new ScreenPoint(plotBounds.Right, plotBounds.Bottom), new ScreenPoint(plotBounds.Left, plotBounds.Bottom),
                                 new ScreenPoint(plotBounds.Left, plotBounds.Top)
                             };

            if (!Equals(plot.BorderColor, OxyColors.Transparent) && plot.BorderThickness > 0)
                rc.DrawPolygon(border, null, plot.BorderColor, plot.BorderThickness, null, true);
        }
    }
}