// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.Legends.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    ////                LegendPosition (LegendPlacement=Outside)
    ////
    //// +               +-----------------------------------+                 +
    ////                 |              Title                |
    ////                 |            Subtitle               |
    ////                 +-----------------------------------+
    ////                 |TopLeft       TopCenter    TopRight|
    ////                 +-----------------------------------+
    ////                 |              Top axis             |
    //// +----------+----+-----------------------------------+-----+-----------+
    //// |LeftTop   |    |                                   |     |RightTop   |
    //// |          |    |                                   |     |           |
    //// |          |Left|                                   |Right|           |
    //// |LeftMiddle|axis|              PlotArea             |axis |RightMiddle|
    //// |          |    |                                   |     |           |
    //// |          |    |                                   |     |           |
    //// |LeftBottom|    |                                   |     |RightBottom|
    //// +----------+----+-----------------------------------+-----+-----------+
    ////                 |             Bottom axis           |
    ////                 +-----------------------------------+
    ////                 |BottomLeft BottomCenter BottomRight|
    //// +               +-----------------------------------+                 +

    /// <summary>
    /// Partial PlotModel class - this file contains methods related to the series legends.
    /// </summary>
    public partial class PlotModel
    {
        #region Methods

        /// <summary>
        /// Gets the rectangle of the legend box.
        /// </summary>
        /// <param name="legendSize">Size of the legend box.</param>
        /// <returns>A rectangle.</returns>
        private OxyRect GetLegendRectangle(OxySize legendSize)
        {
            double top = 0;
            double left = 0;
            if (this.LegendPlacement == LegendPlacement.Outside)
            {
                switch (this.LegendPosition)
                {
                    case LegendPosition.LeftTop:
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.LeftBottom:
                        left = this.PlotAndAxisArea.Left - legendSize.Width - this.LegendMargin;
                        break;
                    case LegendPosition.RightTop:
                    case LegendPosition.RightMiddle:
                    case LegendPosition.RightBottom:
                        left = this.PlotAndAxisArea.Right + this.LegendMargin;
                        break;
                    case LegendPosition.TopLeft:
                    case LegendPosition.TopCenter:
                    case LegendPosition.TopRight:
                        top = this.PlotAndAxisArea.Top - legendSize.Height - this.LegendMargin;
                        break;
                    case LegendPosition.BottomLeft:
                    case LegendPosition.BottomCenter:
                    case LegendPosition.BottomRight:
                        top = this.PlotAndAxisArea.Bottom + this.LegendMargin;
                        break;
                }

                switch (this.LegendPosition)
                {
                    case LegendPosition.TopLeft:
                    case LegendPosition.BottomLeft:
                        left = this.PlotArea.Left;
                        break;
                    case LegendPosition.TopRight:
                    case LegendPosition.BottomRight:
                        left = this.PlotArea.Right - legendSize.Width;
                        break;
                    case LegendPosition.LeftTop:
                    case LegendPosition.RightTop:
                        top = this.PlotArea.Top;
                        break;
                    case LegendPosition.LeftBottom:
                    case LegendPosition.RightBottom:
                        top = this.PlotArea.Bottom - legendSize.Height;
                        break;
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.RightMiddle:
                        top = (this.PlotArea.Top + this.PlotArea.Bottom - legendSize.Height) * 0.5;
                        break;
                    case LegendPosition.TopCenter:
                    case LegendPosition.BottomCenter:
                        left = (this.PlotArea.Left + this.PlotArea.Right - legendSize.Width) * 0.5;
                        break;
                }
            }
            else
            {
                switch (this.LegendPosition)
                {
                    case LegendPosition.LeftTop:
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.LeftBottom:
                        left = this.PlotArea.Left + this.LegendMargin;
                        break;
                    case LegendPosition.RightTop:
                    case LegendPosition.RightMiddle:
                    case LegendPosition.RightBottom:
                        left = this.PlotArea.Right - legendSize.Width - this.LegendMargin;
                        break;
                    case LegendPosition.TopLeft:
                    case LegendPosition.TopCenter:
                    case LegendPosition.TopRight:
                        top = this.PlotArea.Top + this.LegendMargin;
                        break;
                    case LegendPosition.BottomLeft:
                    case LegendPosition.BottomCenter:
                    case LegendPosition.BottomRight:
                        top = this.PlotArea.Bottom - legendSize.Height - this.LegendMargin;
                        break;
                }

                switch (this.LegendPosition)
                {
                    case LegendPosition.TopLeft:
                    case LegendPosition.BottomLeft:
                        left = this.PlotArea.Left + this.LegendMargin;
                        break;
                    case LegendPosition.TopRight:
                    case LegendPosition.BottomRight:
                        left = this.PlotArea.Right - legendSize.Width - this.LegendMargin;
                        break;
                    case LegendPosition.LeftTop:
                    case LegendPosition.RightTop:
                        top = this.PlotArea.Top + this.LegendMargin;
                        break;
                    case LegendPosition.LeftBottom:
                    case LegendPosition.RightBottom:
                        top = this.PlotArea.Bottom - legendSize.Height - this.LegendMargin;
                        break;

                    case LegendPosition.LeftMiddle:
                    case LegendPosition.RightMiddle:
                        top = (this.PlotArea.Top + this.PlotArea.Bottom - legendSize.Height) * 0.5;
                        break;
                    case LegendPosition.TopCenter:
                    case LegendPosition.BottomCenter:
                        left = (this.PlotArea.Left + this.PlotArea.Right - legendSize.Width) * 0.5;
                        break;
                }
            }

            return new OxyRect(left, top, legendSize.Width, legendSize.Height);
        }

        /// <summary>
        /// The render legend.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="rect">
        /// The rect.
        /// </param>
        private void RenderLegend(IRenderContext rc, Series s, OxyRect rect)
        {
            double x = rect.Left;
            switch (this.LegendItemAlignment)
            {
                case HorizontalTextAlign.Center:
                    x = (rect.Left + rect.Right) / 2;
                    if (this.LegendSymbolPlacement == LegendSymbolPlacement.Left)
                    {
                        x -= (this.LegendSymbolLength + this.LegendSymbolMargin) / 2;
                    }
                    else
                    {
                        x -= (this.LegendSymbolLength + this.LegendSymbolMargin) / 2;
                    }

                    break;
                case HorizontalTextAlign.Right:
                    x = rect.Right;

                    // if (LegendSymbolPlacement == LegendSymbolPlacement.Right)
                    x -= this.LegendSymbolLength + this.LegendSymbolMargin;
                    break;
            }

            if (this.LegendSymbolPlacement == LegendSymbolPlacement.Left)
            {
                x += this.LegendSymbolLength + this.LegendSymbolMargin;
            }

            OxySize textSize = rc.DrawMathText(
                new ScreenPoint(x, rect.Top),
                s.Title,
                this.LegendTextColor ?? this.TextColor,
                this.LegendFont ?? this.DefaultFont,
                this.LegendFontSize,
                this.LegendFontWeight,
                0,
                this.LegendItemAlignment,
                VerticalTextAlign.Top,
                true);
            double x0 = x;
            switch (this.LegendItemAlignment)
            {
                case HorizontalTextAlign.Center:
                    x0 = x - (textSize.Width * 0.5);
                    break;
                case HorizontalTextAlign.Right:
                    x0 = x - textSize.Width;
                    break;
            }

            var symbolRect =
                new OxyRect(
                    this.LegendSymbolPlacement == LegendSymbolPlacement.Right
                        ? x0 + textSize.Width + this.LegendSymbolMargin
                        : x0 - this.LegendSymbolMargin - this.LegendSymbolLength,
                    rect.Top,
                    this.LegendSymbolLength,
                    textSize.Height);

            s.RenderLegend(rc, symbolRect);
        }

        /// <summary>
        /// The render legends.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="rect">
        /// The rect.
        /// </param>
        /// <param name="measureOnly">
        /// The measure only.
        /// </param>
        /// <returns>
        /// The size of the legends.
        /// </returns>
        private OxySize RenderLegends(IRenderContext rc, OxyRect rect, bool measureOnly = false)
        {
            // Render background and border around legend
            if (!measureOnly && rect.Width > 0 && rect.Height > 0)
            {
                rc.DrawRectangleAsPolygon(rect, this.LegendBackground, this.LegendBorder, this.LegendBorderThickness);
            }

            double availableWidth = rect.Width;
            double availableHeight = rect.Height;

            double x = this.LegendPadding;
            double top = this.LegendPadding;

            var size = new OxySize();
            if (!string.IsNullOrEmpty(this.LegendTitle))
            {
                OxySize titleSize;
                if (measureOnly)
                {
                    titleSize = rc.MeasureMathText(
                        this.LegendTitle,
                        this.LegendTitleFont ?? DefaultFont,
                        this.LegendTitleFontSize,
                        this.LegendTitleFontWeight);
                }
                else
                {
                    titleSize = rc.DrawMathText(
                        new ScreenPoint(rect.Left + x, rect.Top + top),
                        this.LegendTitle,
                        this.LegendTitleColor ?? this.TextColor,
                        this.LegendTitleFont ?? this.DefaultFont,
                        this.LegendTitleFontSize,
                        this.LegendTitleFontWeight,
                        0,
                        HorizontalTextAlign.Left,
                        VerticalTextAlign.Top,
                        true);
                }

                top += titleSize.Height;
                size.Width = x + titleSize.Width + this.LegendPadding;
                size.Height = top + titleSize.Height;
            }

            double y = top;

            double lineHeight = 0;
            double rowWidth = 0;

            IEnumerable<Series> items = this.LegendItemOrder == LegendItemOrder.Reverse
                                            ? this.VisibleSeries.Reverse()
                                            : this.VisibleSeries;

            foreach (var s in items)
            {
                if (string.IsNullOrEmpty(s.Title))
                {
                    continue;
                }

                OxySize textSize = rc.MeasureMathText(
                    s.Title, this.LegendFont ?? DefaultFont, this.LegendFontSize, this.LegendFontWeight);
                double itemWidth = this.LegendSymbolLength + this.LegendSymbolMargin + textSize.Width;
                double itemHeight = textSize.Height;

                if (this.LegendOrientation == LegendOrientation.Horizontal)
                {
                    // Add spacing between items
                    if (x > this.LegendPadding)
                    {
                        x += this.LegendItemSpacing;
                    }

                    if (x + itemWidth > availableWidth - this.LegendPadding)
                    {
                        // new line
                        x = this.LegendPadding;
                        y += lineHeight;
                        lineHeight = 0;
                    }

                    if (textSize.Height > lineHeight)
                    {
                        lineHeight = textSize.Height;
                    }

                    if (!measureOnly)
                    {
                        var r = new OxyRect(rect.Left + x, rect.Top + y, textSize.Width, textSize.Height);
                        this.RenderLegend(rc, s, r);
                    }

                    x += itemWidth;
                    if (x > size.Width)
                    {
                        size.Width = x;
                    }

                    if (y + textSize.Height > size.Height)
                    {
                        size.Height = y + textSize.Height;
                    }
                }
                else
                {
                    if (y + itemHeight > availableHeight - this.LegendPadding)
                    {
                        y = top;
                        x += rowWidth + this.LegendColumnSpacing;
                        rowWidth = 0;
                    }

                    if (!measureOnly)
                    {
                        var r = new OxyRect(
                            rect.Left + x, rect.Top + y, rect.Width - x - this.LegendPadding, textSize.Height);
                        this.RenderLegend(rc, s, r);
                    }

                    y += itemHeight;
                    if (itemWidth > rowWidth)
                    {
                        rowWidth = itemWidth;
                    }

                    if (x + itemWidth > size.Width)
                    {
                        size.Width = x + itemWidth;
                    }

                    if (y > size.Height)
                    {
                        size.Height = y;
                    }
                }
            }

            if (size.Width > 0)
            {
                size.Width += this.LegendPadding;
            }

            if (size.Height > 0)
            {
                size.Height += this.LegendPadding;
            }

            return size;
        }

        #endregion
    }
}