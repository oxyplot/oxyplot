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
            RenderAxes(rc,AxisLayer.BelowSeries);
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
                ActualPlotMargins=new OxyThickness(newLeft,newTop,newRight,newBottom);
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
        /// Measures the size of the axes for the specified position.
        /// Currently this uses the PlotMargins property only, it is not auto-sizing
        /// </summary>
        /// <param name="rc">The rc.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private OxySize MeasureAxes(IRenderContext rc, AxisPosition position)
        {
            // todo: should not need this method

            OxySize size = new OxySize();
            switch (position)
            {
                case AxisPosition.Left:
                    size.Width = ActualPlotMargins.Left;
                    break;
                case AxisPosition.Right:
                    size.Width = ActualPlotMargins.Right;
                    break;
                case AxisPosition.Top:
                    size.Height = ActualPlotMargins.Top;
                    break;
                case AxisPosition.Bottom:
                    size.Height = ActualPlotMargins.Bottom;
                    break;
            }
            return size;
            
            //foreach (var axis in Axes)
            //{
            //    if (axis.Position != position) continue;
            //    return size;
            //}
            //return OxySize.Empty;

            // todo: measure the axes
            //foreach (var axis in Axes)
            //{
            //var size = MeasureAxis(axis, rc);
            //if (size.Height > maxSize.Height) maxSize.Height = size.Height;
            //if (size.Width > maxSize.Width) maxSize.Width = size.Width;
            //}
            // return size;
        }

        /// <summary>
        /// Measures the width/height of the axis.
        /// 1. Find the maximum size of the axis tick labels.
        /// 2. Add the size of the axis title and the tick lines.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="rc">The rendering context.</param>
        /// <returns></returns>
        //private OxySize MeasureAxis(AxisBase axis, IRenderContext rc)
        //{
        //    IList<double> majorTickValues;
        //    IList<double> minorTickValues;
        //    IList<double> majorLabelValues;
        //    // todo: must get the step size first...
        //    // this will not work
        //    axis.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
        //    OxySize maximumTextSize = new OxySize();
        //    foreach (var v in majorLabelValues)
        //    {
        //        var s = axis.FormatValue(v);
        //        var size = rc.MeasureText(s, axis.ActualFont, axis.FontSize, axis.FontWeight);
        //        if (size.Width > maximumTextSize.Width) maximumTextSize.Width = size.Width;
        //        if (size.Height > maximumTextSize.Height) maximumTextSize.Height = size.Height;
        //    }

        //    var labelTextSize = rc.MeasureText(axis.ActualTitle, axis.ActualFont, axis.FontSize, axis.FontWeight);

        //    double width = 0;
        //    double height = 0;

        //    if (axis.IsHorizontal())
        //    {
        //        switch (axis.TickStyle)
        //        {
        //            case TickStyle.Outside:
        //                height += axis.MajorTickSize;
        //                break;
        //            case TickStyle.Crossing:
        //                height += axis.MajorTickSize * 0.75;
        //                break;
        //        }
        //        height += axis.AxisTickToLabelDistance;
        //        height += maximumTextSize.Height;
        //        if (labelTextSize.Height > 0)
        //        {
        //            height += axis.AxisTitleDistance;
        //            height += labelTextSize.Height;
        //        }
        //    }
        //    else
        //    {
        //        switch (axis.TickStyle)
        //        {
        //            case TickStyle.Outside:
        //                width += axis.MajorTickSize;
        //                break;
        //            case TickStyle.Crossing:
        //                width += axis.MajorTickSize * 0.75;
        //                break;
        //        }
        //        width += axis.AxisTickToLabelDistance;
        //        width += maximumTextSize.Width;
        //        if (labelTextSize.Height > 0)
        //        {
        //            width += axis.AxisTitleDistance;
        //            width += labelTextSize.Height;
        //        }
        //    }
        //    return new OxySize(width, height);
        //}

        /// <summary>
        /// Calculates the plot area (subtract Padding, title size and outside legends)
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        private void UpdatePlotArea(IRenderContext rc)
        {
            var tmp = new OxyRect(Padding.Left, Padding.Top, rc.Width - Padding.Left - Padding.Right, rc.Height - Padding.Top - Padding.Bottom);

            var titleSize = MeasureTitles(rc);
            if (titleSize.Height > 0)
            {
                double titleHeight = titleSize.Height + TitlePadding;
                tmp.Height -= titleHeight;
                tmp.Top += titleHeight;
            }

            var topAxisSize = MeasureAxes(rc, AxisPosition.Top);
            tmp.Top += topAxisSize.Height;
            tmp.Height -= topAxisSize.Height;

            var bottomAxisSize = MeasureAxes(rc, AxisPosition.Bottom);
            tmp.Height -= bottomAxisSize.Height;

            var leftAxisSize = MeasureAxes(rc, AxisPosition.Left);
            // leftAxisSize.Width = 40;
            tmp.Left += leftAxisSize.Width;
            tmp.Width -= leftAxisSize.Width;

            var rightAxisSize = MeasureAxes(rc, AxisPosition.Right);
            tmp.Width -= rightAxisSize.Width;

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

            PlotArea = tmp;
            PlotAndAxisArea = new OxyRect(tmp.Left - leftAxisSize.Width, tmp.Top - topAxisSize.Height, tmp.Width + leftAxisSize.Width + rightAxisSize.Width, tmp.Height + topAxisSize.Height + bottomAxisSize.Height);
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
            // Render the main background of the plot (only if there are axes)
            // The border is rendered by DrawBox to ensure that it is pixel aligned with the tick marks (cannot use DrawRectangle here).
            if (Axes.Count > 0)
                rc.DrawRectangleAsPolygon(PlotArea, Background, null, 0);

            foreach (var s in Series)
            {
                var s2 = s as PlotSeriesBase;
                if (s2 == null || s2.Background == null)
                    continue;
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
            double height = size1.Height + size2.Height;
            // double dy = (TitleArea.Top+TitleArea.Bottom-height)*0.5;
            double dy = TitleArea.Top;
            double dx = (TitleArea.Left + TitleArea.Right) * 0.5;

            if (!String.IsNullOrEmpty(Title))
            {
                rc.DrawMathText(
                    new ScreenPoint(dx, dy), Title, TextColor,
                    ActualTitleFont, TitleFontSize, TitleFontWeight,
                    0,
                    HorizontalTextAlign.Center, VerticalTextAlign.Top, false);
                dy += size1.Height;
            }

            if (!String.IsNullOrEmpty(Subtitle))
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
                rc.DrawRectangleAsPolygon(PlotArea, null, BoxColor, BoxThickness);
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
