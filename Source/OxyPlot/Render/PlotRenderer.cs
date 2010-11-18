using System;

namespace OxyPlot
{
    public class PlotRenderer
    {
        protected readonly PlotModel plot;
        protected readonly IRenderContext rc;

        public PlotRenderer(IRenderContext rc, PlotModel p)
        {
            this.rc = rc;
            plot = p;
        }

        public void RenderTitle(string title, string subtitle)
        {
            OxySize size1 = rc.MeasureText(title, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            OxySize size2 = rc.MeasureText(subtitle, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            double height = size1.Height + size2.Height;
            double dy = (plot.MarginTop - height) * 0.5;
            double dx = (plot.bounds.Left + plot.bounds.Right) * 0.5;

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

        public void RenderRect(OxyRect bounds, OxyColor fill, OxyColor borderColor, double borderThickness)
        {
            var border = new[]
                             {
                                 new ScreenPoint(bounds.Left, bounds.Top), new ScreenPoint(bounds.Right, bounds.Top),
                                 new ScreenPoint(bounds.Right, bounds.Bottom), new ScreenPoint(bounds.Left, bounds.Bottom),
                                 new ScreenPoint(bounds.Left, bounds.Top)
                             };

            rc.DrawPolygon(border, fill, borderColor, borderThickness, null, true);
        }

        private static readonly double LEGEND_PADDING = 8;

        public void RenderLegends()
        {
            double maxWidth = 0;
            double maxHeight = 0;
            double totalHeight = 0;

            foreach (var s in plot.Series)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                var oxySize = rc.MeasureText(s.Title, plot.LegendFont, plot.LegendFontSize);
                if (oxySize.Width > maxWidth) maxWidth = oxySize.Width;
                if (oxySize.Height > maxHeight) maxHeight = oxySize.Height;
                totalHeight += oxySize.Height;
            }

            double length = plot.LegendLineLength;
            double x0 = plot.bounds.Right - LEGEND_PADDING;
            double y0 = plot.bounds.Top + LEGEND_PADDING + maxHeight / 2;

            foreach (var s in plot.Series)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                rc.DrawText(new ScreenPoint(x0 - length - LEGEND_PADDING, y0),
                            s.Title, plot.TextColor,
                            plot.LegendFont, plot.LegendFontSize, 500, 0,
                            HorizontalTextAlign.Right, VerticalTextAlign.Middle);
                var rect = new OxyRect(x0 - length, y0 - maxHeight / 2, length, maxHeight);
                s.RenderLegend(rc, rect);
                y0 += maxHeight;
            }
        }
    }
}