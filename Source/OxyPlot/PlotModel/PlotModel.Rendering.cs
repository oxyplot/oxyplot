using System;
using System.Collections.Generic;
using System.Linq;

namespace OxyPlot
{
    /// <summary>
    /// Partial PlotModel class - this file contains rendering methods.
    /// </summary>
    partial class PlotModel
    {
        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public void Render(IRenderContext rc)
        {
            if (rc.Width <= 0 || rc.Height <= 0)
                return;
            ActualPlotMargins = PlotMargins;
            while (true)
            {
                EnsureLegendProperties();
                UpdatePlotArea(rc);
                UpdateAxisTransforms();
                if (!AutoAdjustPlotMargins) break;
                if (!AdjustPlotMargins(rc)) break;
            }

            RenderBackgrounds(rc);
            RenderAxes(rc, AxisLayer.BelowSeries);
            RenderAnnotations(rc, AnnotationLayer.BelowSeries);
            RenderSeries(rc);
            RenderAnnotations(rc, AnnotationLayer.OverSeries);
            RenderTitle(rc);
            RenderBox(rc);
            RenderAxes(rc, AxisLayer.AboveSeries);

            if (IsLegendVisible)
                RenderLegends(rc, LegendArea);
        }

        private bool AdjustPlotMargins(IRenderContext rc)
        {
            bool isAdjusted = false;
            double newLeft = ActualPlotMargins.Left;
            double newTop = ActualPlotMargins.Top;
            double newRight = ActualPlotMargins.Right;
            double newBottom = ActualPlotMargins.Bottom;
            foreach (var axis in Axes)
            {
                var size = axis.Measure(rc);
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        if (size.Width > newLeft)
                        {
                            newLeft = size.Width;
                            isAdjusted = true;
                        }
                        break;
                    case AxisPosition.Right:
                        if (size.Width > newRight)
                        {
                            newRight = size.Width;
                            isAdjusted = true;
                        }
                        break;
                    case AxisPosition.Top:
                        if (size.Height > newTop)
                        {
                            newTop = size.Height;
                            isAdjusted = true;
                        }
                        break;
                    case AxisPosition.Bottom:
                        if (size.Height > newBottom)
                        {
                            newBottom = size.Height;
                            isAdjusted = true;
                        }
                        break;
                }
            }
            if (isAdjusted)
                ActualPlotMargins = new OxyThickness(newLeft, newTop, newRight, newBottom);
            return isAdjusted;
        }


        /// <summary>
        /// Makes the legend properties safe.
        /// If Legend is positioned left or right, force it to vertical orientation
        /// </summary>
        private void EnsureLegendProperties()
        {
            switch (LegendPosition)
            {
                case LegendPosition.LeftTop:
                case LegendPosition.LeftMiddle:
                case LegendPosition.LeftBottom:
                case LegendPosition.RightTop:
                case LegendPosition.RightMiddle:
                case LegendPosition.RightBottom:
                    if (LegendOrientation == LegendOrientation.Horizontal)
                        LegendOrientation = LegendOrientation.Vertical;
                    break;
            }
        }

        /// <summary>
        /// Measures the size of the Title and Subtitle.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <returns>Size of the titles.</returns>
        private OxySize MeasureTitles(IRenderContext rc)
        {
            var size1 = rc.MeasureText(Title, ActualTitleFont, TitleFontSize, TitleFontWeight);
            var size2 = rc.MeasureText(Subtitle, SubtitleFont ?? ActualTitleFont, TitleFontSize, TitleFontWeight);
            double height = size1.Height + size2.Height;
            double width = Math.Max(size1.Width, size2.Width);
            return new OxySize(width, height);
        }

        /// <summary>
        /// Calculates the plot area (subtract padding, title size and outside legends)
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        private void UpdatePlotArea(IRenderContext rc)
        {
            var tmp = new OxyRect(Padding.Left, Padding.Top, rc.Width - Padding.Left - Padding.Right, rc.Height - Padding.Top - Padding.Bottom);

            var titleSize = this.MeasureTitles(rc);

            if (titleSize.Height > 0)
            {
                double titleHeight = titleSize.Height + TitlePadding;
                tmp.Height -= titleHeight;
                tmp.Top += titleHeight;
            }

            tmp.Top += ActualPlotMargins.Top;
            tmp.Height -= ActualPlotMargins.Top;

            tmp.Height -= ActualPlotMargins.Bottom;

            tmp.Left += ActualPlotMargins.Left;
            tmp.Width -= ActualPlotMargins.Left;

            tmp.Width -= ActualPlotMargins.Right;

            var legendSize = RenderLegends(rc, tmp, true);
            if (IsLegendVisible && LegendPlacement == LegendPlacement.Outside)
            {
                switch (LegendPosition)
                {
                    case LegendPosition.LeftTop:
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.LeftBottom:
                        tmp.Left += legendSize.Width + LegendMargin;
                        tmp.Width -= legendSize.Width + LegendMargin;
                        break;
                    case LegendPosition.RightTop:
                    case LegendPosition.RightMiddle:
                    case LegendPosition.RightBottom:
                        tmp.Width -= legendSize.Width + LegendMargin;
                        break;
                    case LegendPosition.TopLeft:
                    case LegendPosition.TopCenter:
                    case LegendPosition.TopRight:
                        tmp.Top += legendSize.Height + LegendMargin;
                        tmp.Height -= legendSize.Height + LegendMargin;
                        break;
                    case LegendPosition.BottomLeft:
                    case LegendPosition.BottomCenter:
                    case LegendPosition.BottomRight:
                        tmp.Height -= legendSize.Height + LegendMargin;
                        break;
                }
            }
            if (tmp.Height < 0)
                tmp.Bottom = tmp.Top + 1;
            if (tmp.Width < 0)
                tmp.Right = tmp.Left + 1;

            PlotArea = tmp;
            PlotAndAxisArea = new OxyRect(tmp.Left - ActualPlotMargins.Left, tmp.Top - ActualPlotMargins.Top, tmp.Width + ActualPlotMargins.Left + ActualPlotMargins.Right, tmp.Height + ActualPlotMargins.Top + ActualPlotMargins.Bottom);
            TitleArea = new OxyRect(PlotArea.Left, Padding.Top, PlotArea.Width, titleSize.Height + TitlePadding * 2);
            LegendArea = GetLegendRectangle(legendSize);
        }

        private void RenderAnnotations(IRenderContext rc, AnnotationLayer layer)
        {
            foreach (var a in Annotations.Where(a => a.Layer == layer))
                a.Render(rc, this);
        }

        /// <summary>
        /// Renders the axes.
        /// </summary>
        /// <param name="rc">The rc.</param>
        private void RenderAxes(IRenderContext rc, AxisLayer layer)
        {
            foreach (var a in Axes)
            {
                if (a.IsAxisVisible)
                {
                    a.Render(rc, this, layer);
                }
            }
        }

        /// <summary>
        /// Renders the series backgrounds.
        /// </summary>
        /// <param name="rc">The rc.</param>
        private void RenderBackgrounds(IRenderContext rc)
        {
            // Render the background of the plot
            if (Background != null && rc.PaintBackground)
            {
                rc.DrawRectangle(new OxyRect(0, 0, rc.Width, rc.Height), Background, null, 0);
            }

            // Render the main background of the plot area (only if there are axes)
            // The border is rendered by DrawBox to ensure that it is pixel aligned with the tick marks (cannot use DrawRectangle here).
            if (Axes.Count > 0 && PlotAreaBackground != null)
            {
                rc.DrawRectangleAsPolygon(PlotArea, this.PlotAreaBackground, null, 0);
            }

            foreach (var s in Series)
            {
                var s2 = s as XYAxisSeries;
                if (s2 == null || s2.Background == null)
                {
                    continue;
                }
                rc.DrawRectangle(s2.GetScreenRectangle(), s2.Background, null, 0);
            }
        }

        /// <summary>
        /// Renders the title.
        /// </summary>
        /// <param name="rc">The rc.</param>
        private void RenderTitle(IRenderContext rc)
        {
            var size1 = rc.MeasureText(Title, ActualTitleFont, TitleFontSize, TitleFontWeight);
            var size2 = rc.MeasureText(Subtitle, SubtitleFont ?? ActualTitleFont, SubtitleFontSize, SubtitleFontWeight);

            // double height = size1.Height + size2.Height;
            // double dy = (TitleArea.Top+TitleArea.Bottom-height)*0.5;
            double dy = TitleArea.Top;
            double dx = (TitleArea.Left + TitleArea.Right) * 0.5;

            if (!string.IsNullOrEmpty(Title))
            {
                rc.DrawMathText(
                    new ScreenPoint(dx, dy), Title, TextColor,
                    ActualTitleFont, TitleFontSize, TitleFontWeight,
                    0,
                    HorizontalTextAlign.Center, VerticalTextAlign.Top, false);
                dy += size1.Height;
            }

            if (!string.IsNullOrEmpty(Subtitle))
            {
                rc.DrawMathText(new ScreenPoint(dx, dy), Subtitle, TextColor,
                                SubtitleFont ?? ActualTitleFont, SubtitleFontSize, SubtitleFontWeight, 0,
                                HorizontalTextAlign.Center, VerticalTextAlign.Top, false);
            }
        }

        /// <summary>
        /// Renders the border around the plot area.
        /// </summary>
        /// <param name="rc">The rc.</param>
        private void RenderBox(IRenderContext rc)
        {
            // Render the border around the plot (only if there are axes)
            // The border is rendered by DrawBox to ensure that it is pixel aligned with the tick marks (cannot use DrawRectangle here).
            if (Axes.Count > 0)
            {
                rc.DrawRectangleAsPolygon(PlotArea, null, this.PlotAreaBorderColor, this.PlotAreaBorderThickness);
            }
        }

        /// <summary>
        /// Renders the series.
        /// </summary>
        /// <param name="rc">The rc.</param>
        private void RenderSeries(IRenderContext rc)
        {
            // Update undefined colors
            ResetDefaultColor();
            foreach (var s in Series)
            {
                s.SetDefaultValues(this);
            }

            foreach (var s in Series)
            {
                s.Render(rc, this);
            }
        }

    }
}
