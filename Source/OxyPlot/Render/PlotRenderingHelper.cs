using System;

namespace OxyPlot
{
    public class PlotRenderingHelper
    {
        private static readonly double LEGEND_PADDING = 8;

        protected readonly PlotModel plot;
        protected readonly IRenderContext rc;

        public PlotRenderingHelper(IRenderContext rc, PlotModel p)
        {
            this.rc = rc;
            plot = p;
        }

        public void RenderTitle(string title, string subtitle)
        {
            OxySize size1 = rc.MeasureText(title, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            OxySize size2 = rc.MeasureText(subtitle, plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight);
            double height = size1.Height + size2.Height;
            double dy = (plot.PlotMargins.Top - height) * 0.5;
            double dx = (plot.PlotArea.Left + plot.PlotArea.Right) * 0.5;

            if (!String.IsNullOrEmpty(title))
                rc.DrawMathText(
                    new ScreenPoint(dx, dy), title, plot.TextColor,
                    plot.TitleFont, plot.TitleFontSize, plot.TitleFontWeight,
                    0,
                    HorizontalTextAlign.Center, VerticalTextAlign.Top);
            if (!String.IsNullOrEmpty(subtitle))
                rc.DrawMathText(new ScreenPoint(dx, dy + size1.Height), subtitle, plot.TextColor,
                            plot.TitleFont, plot.SubtitleFontSize, plot.SubtitleFontWeight, 0,
                            HorizontalTextAlign.Center, VerticalTextAlign.Top);
        }



        public void RenderLegends()
        {
            double maxWidth = 0;
            double maxHeight = 0;
            double totalHeight = 0;

            // Measure
            foreach (var s in plot.Series)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                var oxySize = rc.MeasureMathText(s.Title, plot.LegendFont, plot.LegendFontSize, 500);
                if (oxySize.Width > maxWidth) maxWidth = oxySize.Width;
                if (oxySize.Height > maxHeight) maxHeight = oxySize.Height;
                totalHeight += oxySize.Height;
            }

            double lineLength = plot.LegendSymbolLength;

            // Arrange
            double x0 = double.NaN, x1 = double.NaN, y0 = double.NaN;

            //   padding          padding
            //          lineLength
            // y0       -----o----       seriesName
            //          x0               x1

            double sign = 1;
            if (plot.IsLegendOutsidePlotArea)
                sign = -1;

            // Horizontal alignment
            HorizontalTextAlign ha = HorizontalTextAlign.Left;
            switch (plot.LegendPosition)
            {
                case LegendPosition.TopRight:
                case LegendPosition.BottomRight:
                    x0 = plot.PlotArea.Right - LEGEND_PADDING * sign;
                    x1 = x0 - lineLength * sign - LEGEND_PADDING * sign;
                    ha = sign == 1 ? HorizontalTextAlign.Right : HorizontalTextAlign.Left;
                    break;
                case LegendPosition.TopLeft:
                case LegendPosition.BottomLeft:
                    x0 = plot.PlotArea.Left + LEGEND_PADDING * sign;
                    x1 = x0 + lineLength * sign + LEGEND_PADDING * sign;
                    ha = sign == 1 ? HorizontalTextAlign.Left : HorizontalTextAlign.Right;
                    break;
            }

            // Vertical alignment
            VerticalTextAlign va = VerticalTextAlign.Middle;
            switch (plot.LegendPosition)
            {
                case LegendPosition.TopRight:
                case LegendPosition.TopLeft:
                    y0 = plot.PlotArea.Top + LEGEND_PADDING + maxHeight / 2;
                    break;
                case LegendPosition.BottomRight:
                case LegendPosition.BottomLeft:
                    y0 = plot.PlotArea.Bottom - maxHeight + LEGEND_PADDING;
                    break;
            }

            foreach (var s in plot.Series)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                rc.DrawMathText(new ScreenPoint(x1, y0),
                            s.Title, plot.TextColor,
                            plot.LegendFont, plot.LegendFontSize, 500, 0,
                            ha, va);
                OxyRect rect = new OxyRect(x0 - lineLength, y0 - maxHeight / 2, lineLength, maxHeight);
                if (ha == HorizontalTextAlign.Left)
                    rect = new OxyRect(x0, y0 - maxHeight / 2, lineLength, maxHeight);

                s.RenderLegend(rc, rect);
                if (plot.LegendPosition == LegendPosition.TopLeft || plot.LegendPosition == LegendPosition.TopRight)
                    y0 += maxHeight;
                else
                    y0 -= maxHeight;
            }
        }
    }
}
