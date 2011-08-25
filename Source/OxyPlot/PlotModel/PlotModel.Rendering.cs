// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.Rendering.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Partial PlotModel class - this file contains rendering methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Linq;

    /// <summary>
    /// Partial PlotModel class - this file contains rendering methods.
    /// </summary>
    partial class PlotModel
    {
        #region Public Methods

        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        public void Render(IRenderContext rc)
        {
            if (rc.Width <= 0 || rc.Height <= 0)
            {
                return;
            }

            this.ActualPlotMargins = this.PlotMargins;
            while (true)
            {
                this.EnsureLegendProperties();
                this.UpdatePlotArea(rc);
                this.UpdateAxisTransforms();
                if (!this.AutoAdjustPlotMargins)
                {
                    break;
                }

                if (!this.AdjustPlotMargins(rc))
                {
                    break;
                }
            }

            this.RenderBackgrounds(rc);
            this.RenderAxes(rc, AxisLayer.BelowSeries);
            this.RenderAnnotations(rc, AnnotationLayer.BelowSeries);
            this.RenderSeries(rc);
            this.RenderAnnotations(rc, AnnotationLayer.OverSeries);
            this.RenderTitle(rc);
            this.RenderBox(rc);
            this.RenderAxes(rc, AxisLayer.AboveSeries);

            if (this.IsLegendVisible)
            {
                this.RenderLegends(rc, this.LegendArea);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The adjust plot margins.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <returns>
        /// The adjust plot margins.
        /// </returns>
        private bool AdjustPlotMargins(IRenderContext rc)
        {
            bool isAdjusted = false;
            double newLeft = this.ActualPlotMargins.Left;
            double newTop = this.ActualPlotMargins.Top;
            double newRight = this.ActualPlotMargins.Right;
            double newBottom = this.ActualPlotMargins.Bottom;
            foreach (Axis axis in this.Axes)
            {
                OxySize size = axis.Measure(rc);
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
            {
                this.ActualPlotMargins = new OxyThickness(newLeft, newTop, newRight, newBottom);
            }

            return isAdjusted;
        }

        /// <summary>
        /// Makes the legend properties safe.
        /// If Legend is positioned left or right, force it to vertical orientation
        /// </summary>
        private void EnsureLegendProperties()
        {
            switch (this.LegendPosition)
            {
                case LegendPosition.LeftTop:
                case LegendPosition.LeftMiddle:
                case LegendPosition.LeftBottom:
                case LegendPosition.RightTop:
                case LegendPosition.RightMiddle:
                case LegendPosition.RightBottom:
                    if (this.LegendOrientation == LegendOrientation.Horizontal)
                    {
                        this.LegendOrientation = LegendOrientation.Vertical;
                    }

                    break;
            }
        }

        /// <summary>
        /// Measures the size of the Title and Subtitle.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <returns>
        /// Size of the titles.
        /// </returns>
        private OxySize MeasureTitles(IRenderContext rc)
        {
            OxySize size1 = rc.MeasureText(this.Title, this.ActualTitleFont, this.TitleFontSize, this.TitleFontWeight);
            OxySize size2 = rc.MeasureText(
                this.Subtitle, this.SubtitleFont ?? this.ActualTitleFont, this.TitleFontSize, this.TitleFontWeight);
            double height = size1.Height + size2.Height;
            double width = Math.Max(size1.Width, size2.Width);
            return new OxySize(width, height);
        }

        /// <summary>
        /// The render annotations.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="layer">
        /// The layer.
        /// </param>
        private void RenderAnnotations(IRenderContext rc, AnnotationLayer layer)
        {
            foreach (Annotation a in this.Annotations.Where(a => a.Layer == layer))
            {
                a.Render(rc, this);
            }
        }

        /// <summary>
        /// Renders the axes.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="layer">
        /// The layer.
        /// </param>
        private void RenderAxes(IRenderContext rc, AxisLayer layer)
        {
            foreach (Axis a in this.Axes)
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
        /// <param name="rc">
        /// The rc.
        /// </param>
        private void RenderBackgrounds(IRenderContext rc)
        {
            // Render the background of the plot
            if (this.Background != null && rc.PaintBackground)
            {
                rc.DrawRectangle(new OxyRect(0, 0, rc.Width, rc.Height), this.Background, null, 0);
            }

            // Render the main background of the plot area (only if there are axes)
            // The border is rendered by DrawBox to ensure that it is pixel aligned with the tick marks (cannot use DrawRectangle here).
            if (this.Axes.Count > 0 && this.PlotAreaBackground != null)
            {
                rc.DrawRectangleAsPolygon(this.PlotArea, this.PlotAreaBackground, null, 0);
            }

            foreach (Series s in this.Series)
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
        /// Renders the border around the plot area.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        private void RenderBox(IRenderContext rc)
        {
            // Render the border around the plot (only if there are axes)
            // The border is rendered by DrawBox to ensure that it is pixel aligned with the tick marks (cannot use DrawRectangle here).
            if (this.Axes.Count > 0)
            {
                rc.DrawRectangleAsPolygon(this.PlotArea, null, this.PlotAreaBorderColor, this.PlotAreaBorderThickness);
            }
        }

        /// <summary>
        /// Renders the series.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        private void RenderSeries(IRenderContext rc)
        {
            // Update undefined colors
            this.ResetDefaultColor();
            foreach (Series s in this.Series)
            {
                s.SetDefaultValues(this);
            }

            foreach (Series s in this.Series)
            {
                s.Render(rc, this);
            }
        }

        /// <summary>
        /// Renders the title.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        private void RenderTitle(IRenderContext rc)
        {
            OxySize size1 = rc.MeasureText(this.Title, this.ActualTitleFont, this.TitleFontSize, this.TitleFontWeight);
            OxySize size2 = rc.MeasureText(
                this.Subtitle, this.SubtitleFont ?? this.ActualTitleFont, this.SubtitleFontSize, this.SubtitleFontWeight);

            // double height = size1.Height + size2.Height;
            // double dy = (TitleArea.Top+TitleArea.Bottom-height)*0.5;
            double dy = this.TitleArea.Top;
            double dx = (this.TitleArea.Left + this.TitleArea.Right) * 0.5;

            if (!string.IsNullOrEmpty(this.Title))
            {
                rc.DrawMathText(
                    new ScreenPoint(dx, dy), 
                    this.Title, 
                    this.TextColor, 
                    this.ActualTitleFont, 
                    this.TitleFontSize, 
                    this.TitleFontWeight, 
                    0, 
                    HorizontalTextAlign.Center, 
                    VerticalTextAlign.Top, 
                    false);
                dy += size1.Height;
            }

            if (!string.IsNullOrEmpty(this.Subtitle))
            {
                rc.DrawMathText(
                    new ScreenPoint(dx, dy), 
                    this.Subtitle, 
                    this.TextColor, 
                    this.SubtitleFont ?? this.ActualTitleFont, 
                    this.SubtitleFontSize, 
                    this.SubtitleFontWeight, 
                    0, 
                    HorizontalTextAlign.Center, 
                    VerticalTextAlign.Top, 
                    false);
            }
        }

        /// <summary>
        /// Calculates the plot area (subtract padding, title size and outside legends)
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        private void UpdatePlotArea(IRenderContext rc)
        {
            var tmp = new OxyRect(
                this.Padding.Left, 
                this.Padding.Top, 
                rc.Width - this.Padding.Left - this.Padding.Right, 
                rc.Height - this.Padding.Top - this.Padding.Bottom);

            OxySize titleSize = this.MeasureTitles(rc);

            if (titleSize.Height > 0)
            {
                double titleHeight = titleSize.Height + this.TitlePadding;
                tmp.Height -= titleHeight;
                tmp.Top += titleHeight;
            }

            tmp.Top += this.ActualPlotMargins.Top;
            tmp.Height -= this.ActualPlotMargins.Top;

            tmp.Height -= this.ActualPlotMargins.Bottom;

            tmp.Left += this.ActualPlotMargins.Left;
            tmp.Width -= this.ActualPlotMargins.Left;

            tmp.Width -= this.ActualPlotMargins.Right;

            OxySize legendSize = this.RenderLegends(rc, tmp, true);
            if (this.IsLegendVisible && this.LegendPlacement == LegendPlacement.Outside)
            {
                switch (this.LegendPosition)
                {
                    case LegendPosition.LeftTop:
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.LeftBottom:
                        tmp.Left += legendSize.Width + this.LegendMargin;
                        tmp.Width -= legendSize.Width + this.LegendMargin;
                        break;
                    case LegendPosition.RightTop:
                    case LegendPosition.RightMiddle:
                    case LegendPosition.RightBottom:
                        tmp.Width -= legendSize.Width + this.LegendMargin;
                        break;
                    case LegendPosition.TopLeft:
                    case LegendPosition.TopCenter:
                    case LegendPosition.TopRight:
                        tmp.Top += legendSize.Height + this.LegendMargin;
                        tmp.Height -= legendSize.Height + this.LegendMargin;
                        break;
                    case LegendPosition.BottomLeft:
                    case LegendPosition.BottomCenter:
                    case LegendPosition.BottomRight:
                        tmp.Height -= legendSize.Height + this.LegendMargin;
                        break;
                }
            }

            if (tmp.Height < 0)
            {
                tmp.Bottom = tmp.Top + 1;
            }

            if (tmp.Width < 0)
            {
                tmp.Right = tmp.Left + 1;
            }

            this.PlotArea = tmp;
            this.PlotAndAxisArea = new OxyRect(
                tmp.Left - this.ActualPlotMargins.Left, 
                tmp.Top - this.ActualPlotMargins.Top, 
                tmp.Width + this.ActualPlotMargins.Left + this.ActualPlotMargins.Right, 
                tmp.Height + this.ActualPlotMargins.Top + this.ActualPlotMargins.Bottom);
            this.TitleArea = new OxyRect(
                this.PlotArea.Left, this.Padding.Top, this.PlotArea.Width, titleSize.Height + this.TitlePadding * 2);
            this.LegendArea = this.GetLegendRectangle(legendSize);
        }

        #endregion
    }
}