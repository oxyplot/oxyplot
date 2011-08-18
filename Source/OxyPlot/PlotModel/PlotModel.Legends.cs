using System;
using System.Linq;

namespace OxyPlot
{
    // LegendPosition (LegendPlacement=Outside)
    //
    // +               +-----------------------------------+                 +
    //                 |              Title                |
    //                 |            Subtitle               |
    //                 +-----------------------------------+
    //                 |TopLeft       TopCenter    TopRight|
    //                 +-----------------------------------+
    //                 |              Top axis             |
    // +----------+----+-----------------------------------+-----+-----------+
    // |LeftTop   |    |                                   |     |RightTop   |
    // |          |    |                                   |     |           |
    // |          |Left|                                   |Right|           |
    // |LeftMiddle|axis|              PlotArea             |axis |RightMiddle|
    // |          |    |                                   |     |           |
    // |          |    |                                   |     |           |
    // |LeftBottom|    |                                   |     |RightBottom|
    // +----------+----+-----------------------------------+-----+-----------+
    //                 |             Bottom axis           |
    //                 +-----------------------------------+
    //                 |BottomLeft BottomCenter BottomRight|
    // +               +-----------------------------------+                 +

    /// <summary>
    /// Partial PlotModel class - this file contains methods related to the series legends.
    /// </summary>
    partial class PlotModel
    {
        private OxySize RenderLegends(IRenderContext rc, OxyRect rect, bool measureOnly = false)
        {
            // Render background and border around legend
            if (!measureOnly && rect.Width > 0 && rect.Height > 0)
                rc.DrawRectangleAsPolygon(rect, LegendBackground, LegendBorder, LegendBorderThickness);

            double availableWidth = rect.Width;
            double availableHeight = rect.Height;

            double x = LegendPadding;
            double top = LegendPadding;

            var size = new OxySize();
            if (!String.IsNullOrEmpty(LegendTitle))
            {
                OxySize titleSize;
                if (measureOnly)
                    titleSize = rc.MeasureMathText(LegendTitle, LegendTitleFont ?? DefaultFont, LegendTitleFontSize, LegendTitleFontWeight);
                else
                    titleSize = rc.DrawMathText(new ScreenPoint(rect.Left + x, rect.Top + top),
                                LegendTitle, TextColor,
                                LegendTitleFont ?? DefaultFont, LegendTitleFontSize, LegendTitleFontWeight, 0, HorizontalTextAlign.Left,
                                VerticalTextAlign.Top, true);
                top += titleSize.Height;
                size.Width = x + titleSize.Width + LegendPadding;
                size.Height = top + titleSize.Height;
            }

            double y = top;

            double lineHeight = 0;
            double rowWidth = 0;

            var items = LegendItemOrder == LegendItemOrder.Reverse ? Series.Reverse() : Series;

            foreach (var s in items)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                var textSize = rc.MeasureMathText(s.Title, LegendFont ?? DefaultFont, LegendFontSize, LegendFontWeight);
                double itemWidth = LegendSymbolLength + LegendSymbolMargin + textSize.Width;
                double itemHeight = textSize.Height;

                if (LegendOrientation == LegendOrientation.Horizontal)
                {
                    // Add spacing between items
                    if (x > LegendPadding) x += LegendItemSpacing;

                    if (x + itemWidth > availableWidth - LegendPadding)
                    {
                        // new line
                        x = LegendPadding;
                        y += lineHeight;
                        lineHeight = 0;
                    }

                    if (textSize.Height > lineHeight)
                        lineHeight = textSize.Height;

                    if (!measureOnly)
                    {
                        var r = new OxyRect(rect.Left + x, rect.Top + y, textSize.Width, textSize.Height);
                        RenderLegend(rc, s, r);
                    }
                    x += itemWidth;
                    if (x > size.Width)
                        size.Width = x;
                    if (y + textSize.Height > size.Height)
                        size.Height = y + textSize.Height;
                }
                else
                {
                    if (y + itemHeight > availableHeight - LegendPadding)
                    {
                        y = top;
                        x += rowWidth + LegendColumnSpacing;
                        rowWidth = 0;
                    }

                    if (!measureOnly)
                    {
                        var r = new OxyRect(rect.Left + x, rect.Top + y, rect.Width - x - LegendPadding, textSize.Height);
                        RenderLegend(rc, s, r);
                    }

                    y += itemHeight;
                    if (itemWidth > rowWidth)
                        rowWidth = itemWidth;

                    if (x + itemWidth > size.Width)
                        size.Width = x + itemWidth;

                    if (y > size.Height)
                        size.Height = y;



                }


            }
            if (size.Width > 0)
                size.Width += LegendPadding;
            if (size.Height > 0)
                size.Height += LegendPadding;

            return size;
        }

        private void RenderLegend(IRenderContext rc, ISeries s, OxyRect rect)
        {
            double x = rect.Left;
            switch (LegendItemAlignment)
            {
                case HorizontalTextAlign.Center:
                    x = (rect.Left + rect.Right) / 2;
                    if (LegendSymbolPlacement == LegendSymbolPlacement.Left)
                        x -= (LegendSymbolLength + LegendSymbolMargin) / 2;
                    else
                        x -= (LegendSymbolLength + LegendSymbolMargin) / 2;
                    break;
                case HorizontalTextAlign.Right:
                    x = rect.Right;
                    //   if (LegendSymbolPlacement == LegendSymbolPlacement.Right)
                    x -= LegendSymbolLength + LegendSymbolMargin;
                    break;
            }
            if (LegendSymbolPlacement == LegendSymbolPlacement.Left)
                x += LegendSymbolLength + LegendSymbolMargin;

            var textSize = rc.DrawMathText(new ScreenPoint(x, rect.Top), s.Title, TextColor,
                                               LegendFont ?? DefaultFont, LegendFontSize, LegendFontWeight, 0,
                                               LegendItemAlignment, VerticalTextAlign.Top, true);
            double x0 = x;
            switch (LegendItemAlignment)
            {
                case HorizontalTextAlign.Center:
                    x0 = x - textSize.Width / 2;
                    break;
                case HorizontalTextAlign.Right:
                    x0 = x - textSize.Width;
                    break;
            }

            var symbolRect = new OxyRect(LegendSymbolPlacement == LegendSymbolPlacement.Right ? x0 + textSize.Width + LegendSymbolMargin : x0 - LegendSymbolMargin - LegendSymbolLength, rect.Top,
                LegendSymbolLength, textSize.Height);

            s.RenderLegend(rc, symbolRect);
        }

        /// <summary>
        /// Gets the rectangle of the legend box.
        /// </summary>
        /// <param name="legendSize">Size of the legend box.</param>
        /// <returns></returns>
        private OxyRect GetLegendRectangle(OxySize legendSize)
        {
            double top = 0;
            double left = 0;
            if (LegendPlacement == LegendPlacement.Outside)
            {
                switch (LegendPosition)
                {
                    case LegendPosition.LeftTop:
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.LeftBottom:
                        left = PlotAndAxisArea.Left - legendSize.Width - LegendMargin;
                        break;
                    case LegendPosition.RightTop:
                    case LegendPosition.RightMiddle:
                    case LegendPosition.RightBottom:
                        left = PlotAndAxisArea.Right + LegendMargin;
                        break;
                    case LegendPosition.TopLeft:
                    case LegendPosition.TopCenter:
                    case LegendPosition.TopRight:
                        top = PlotAndAxisArea.Top - legendSize.Height - LegendMargin;
                        break;
                    case LegendPosition.BottomLeft:
                    case LegendPosition.BottomCenter:
                    case LegendPosition.BottomRight:
                        top = PlotAndAxisArea.Bottom + LegendMargin;
                        break;
                }
                switch (LegendPosition)
                {
                    case LegendPosition.TopLeft:
                    case LegendPosition.BottomLeft:
                        left = PlotArea.Left;
                        break;
                    case LegendPosition.TopRight:
                    case LegendPosition.BottomRight:
                        left = PlotArea.Right - legendSize.Width;
                        break;
                    case LegendPosition.LeftTop:
                    case LegendPosition.RightTop:
                        top = PlotArea.Top;
                        break;
                    case LegendPosition.LeftBottom:
                    case LegendPosition.RightBottom:
                        top = PlotArea.Bottom - legendSize.Height;
                        break;
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.RightMiddle:
                        top = (PlotArea.Top + PlotArea.Bottom - legendSize.Height) * 0.5;
                        break;
                    case LegendPosition.TopCenter:
                    case LegendPosition.BottomCenter:
                        left = (PlotArea.Left + PlotArea.Right - legendSize.Width) * 0.5;
                        break;
                }
            }
            else
            {
                switch (LegendPosition)
                {
                    case LegendPosition.LeftTop:
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.LeftBottom:
                        left = PlotArea.Left + LegendMargin;
                        break;
                    case LegendPosition.RightTop:
                    case LegendPosition.RightMiddle:
                    case LegendPosition.RightBottom:
                        left = PlotArea.Right - legendSize.Width - LegendMargin;
                        break;
                    case LegendPosition.TopLeft:
                    case LegendPosition.TopCenter:
                    case LegendPosition.TopRight:
                        top = PlotArea.Top + LegendMargin;
                        break;
                    case LegendPosition.BottomLeft:
                    case LegendPosition.BottomCenter:
                    case LegendPosition.BottomRight:
                        top = PlotArea.Bottom - legendSize.Height - LegendMargin;
                        break;
                }
                switch (LegendPosition)
                {
                    case LegendPosition.TopLeft:
                    case LegendPosition.BottomLeft:
                        left = PlotArea.Left + LegendMargin;
                        break;
                    case LegendPosition.TopRight:
                    case LegendPosition.BottomRight:
                        left = PlotArea.Right - legendSize.Width - LegendMargin;
                        break;
                    case LegendPosition.LeftTop:
                    case LegendPosition.RightTop:
                        top = PlotArea.Top + LegendMargin;
                        break;
                    case LegendPosition.LeftBottom:
                    case LegendPosition.RightBottom:
                        top = PlotArea.Bottom - legendSize.Height - LegendMargin;
                        break;

                    case LegendPosition.LeftMiddle:
                    case LegendPosition.RightMiddle:
                        top = (PlotArea.Top + PlotArea.Bottom - legendSize.Height) * 0.5;
                        break;
                    case LegendPosition.TopCenter:
                    case LegendPosition.BottomCenter:
                        left = (PlotArea.Left + PlotArea.Right - legendSize.Width) * 0.5;
                        break;

                }

            }
            return new OxyRect(left, top, legendSize.Width, legendSize.Height);
        }
    }
}
