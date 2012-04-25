// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.Rendering.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Partial PlotModel class - this file contains rendering methods.
    /// </summary>
    public partial class PlotModel
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
                this.UpdateIntervals();
                if (!this.AutoAdjustPlotMargins)
                {
                    break;
                }

                if (!this.AdjustPlotMargins(rc))
                {
                    break;
                }
            }

            if (this.PlotType == PlotType.Cartesian)
            {
                this.EnforceCartesianTransforms();
                this.UpdateIntervals();
            }

            this.RenderBackgrounds(rc);
            this.RenderAnnotations(rc, AnnotationLayer.BelowAxes);
            this.RenderAxes(rc, AxisLayer.BelowSeries);
            this.RenderAnnotations(rc, AnnotationLayer.BelowSeries);
            this.RenderSeries(rc);
            this.RenderAnnotations(rc, AnnotationLayer.AboveSeries);
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
        /// Calculates the max size of the specified axes.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="axesOfPositionTier">
        /// The axes of position tier.
        /// </param>
        /// <returns>
        /// The max size.
        /// </returns>
        private static double MaxSizeOfPositionTier(IRenderContext rc, IEnumerable<Axis> axesOfPositionTier)
        {
            double maxSizeOfPositionTier = 0;
            foreach (var axis in axesOfPositionTier)
            {
                OxySize size = axis.Measure(rc);
                if (axis.IsHorizontal())
                {
                    if (size.Height > maxSizeOfPositionTier)
                    {
                        maxSizeOfPositionTier = size.Height;
                    }
                }
                else
                {
                    if (size.Width > maxSizeOfPositionTier)
                    {
                        maxSizeOfPositionTier = size.Width;
                    }
                }
            }

            return maxSizeOfPositionTier;
        }

        /// <summary>
        /// Adjust the plot margins.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <returns>
        /// The adjust plot margins.
        /// </returns>
        private bool AdjustPlotMargins(IRenderContext rc)
        {
            bool isAdjusted = false;
            var newPlotMargins = new Dictionary<AxisPosition, double> 
                {
                    { AxisPosition.Left, this.ActualPlotMargins.Left }, 
                    { AxisPosition.Top, this.ActualPlotMargins.Top }, 
                    { AxisPosition.Right, this.ActualPlotMargins.Right }, 
                    { AxisPosition.Bottom, this.ActualPlotMargins.Bottom }
                };

            for (var position = AxisPosition.Left; position <= AxisPosition.Bottom; position++)
            {
                double maxValueOfPositionTier = 0;
                var axesOfPosition = this.Axes.Where(a => a.Position == position).ToList();
                foreach (var positionTier in axesOfPosition.Select(a => a.PositionTier).Distinct().OrderBy(l => l))
                {
                    var axesOfPositionTier = axesOfPosition.Where(a => a.PositionTier == positionTier).ToList();
                    double maxSizeOfPositionTier = MaxSizeOfPositionTier(rc, axesOfPositionTier);
                    double minValueOfPositionTier = maxValueOfPositionTier;

                    if (Math.Abs(maxValueOfPositionTier) > 1e-5)
                    {
                        maxValueOfPositionTier += this.AxisTierDistance;
                    }

                    maxValueOfPositionTier += maxSizeOfPositionTier;

                    foreach (Axis axis in axesOfPositionTier)
                    {
                        axis.PositionTierSize = maxSizeOfPositionTier;
                        axis.PositionTierMinShift = minValueOfPositionTier;
                        axis.PositionTierMaxShift = maxValueOfPositionTier;
                    }
                }

                if (maxValueOfPositionTier > newPlotMargins[position])
                {
                    newPlotMargins[position] = maxValueOfPositionTier;
                    isAdjusted = true;
                }
            }

            if (isAdjusted)
            {
                this.ActualPlotMargins = new OxyThickness(
                    newPlotMargins[AxisPosition.Left],
                    newPlotMargins[AxisPosition.Top],
                    newPlotMargins[AxisPosition.Right],
                    newPlotMargins[AxisPosition.Bottom]);
            }

            return isAdjusted;
        }

        /// <summary>
        /// Makes the legend properties safe.
        ///   If Legend is positioned left or right, force it to vertical orientation
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
                this.Subtitle, this.SubtitleFont ?? this.ActualSubtitleFont, this.SubtitleFontSize, this.SubtitleFontWeight);
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
            foreach (var a in this.Annotations.Where(a => a.Layer == layer))
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
            foreach (var a in this.Axes)
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

            foreach (var s in this.VisibleSeries)
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
            foreach (var s in this.VisibleSeries)
            {
                s.SetDefaultValues(this);
            }

            foreach (var s in this.VisibleSeries)
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
            rc.MeasureText(
                this.Subtitle, this.SubtitleFont ?? this.ActualSubtitleFont, this.SubtitleFontSize, this.SubtitleFontWeight);

            // double height = size1.Height + size2.Height;
            // double dy = (TitleArea.Top+TitleArea.Bottom-height)*0.5;
            double dy = this.TitleArea.Top;
            double dx = (this.TitleArea.Left + this.TitleArea.Right) * 0.5;

            if (!string.IsNullOrEmpty(this.Title))
            {
                rc.DrawMathText(
                    new ScreenPoint(dx, dy),
                    this.Title,
                    this.TitleColor ?? this.TextColor,
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
                    this.SubtitleColor ?? this.TextColor,
                    this.ActualSubtitleFont,
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
            this.TitleArea = new OxyRect(this.PlotArea.Left, this.Padding.Top, this.PlotArea.Width, titleSize.Height + (this.TitlePadding * 2));
            this.LegendArea = this.GetLegendRectangle(legendSize);
        }

        #endregion
    }
}