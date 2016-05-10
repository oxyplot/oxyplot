// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.Rendering.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Renders the plot with the specified rendering context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public partial class PlotModel
    {
        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        void IPlotModel.Render(IRenderContext rc, double width, double height)
        {
            this.RenderOverride(rc, width, height);
        }

        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected virtual void RenderOverride(IRenderContext rc, double width, double height)
        {
            lock (this.SyncRoot)
            {
                try
                {
                    if (this.lastPlotException != null)
                    {
                        // There was an exception during plot model update. 
                        // This could happen when OxyPlot is given invalid input data. 
                        // The client application should be responsible for handling this.
                        // If the client application submitted invalid data, show the exception message and stack trace.
                        var errorMessage = string.Format(
                                "An exception of type {0} was thrown when updating the plot model.\r\n{1}",
                                this.lastPlotException.GetType(),
                                this.lastPlotException.GetBaseException().StackTrace);
                        this.RenderErrorMessage(rc, string.Format("OxyPlot exception: {0}", this.lastPlotException.Message), errorMessage);
                        return;
                    }

                    if (this.RenderingDecorator != null)
                    {
                        rc = this.RenderingDecorator(rc);
                    }

                    this.Width = width;
                    this.Height = height;

                    this.ActualPlotMargins =
                        new OxyThickness(
                            double.IsNaN(this.PlotMargins.Left) ? 0 : this.PlotMargins.Left,
                            double.IsNaN(this.PlotMargins.Top) ? 0 : this.PlotMargins.Top,
                            double.IsNaN(this.PlotMargins.Right) ? 0 : this.PlotMargins.Right,
                            double.IsNaN(this.PlotMargins.Bottom) ? 0 : this.PlotMargins.Bottom);

                    this.EnsureLegendProperties();

                    while (true)
                    {
                        this.UpdatePlotArea(rc);
                        this.UpdateAxisTransforms();
                        this.UpdateIntervals();

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

                    foreach (var a in this.Axes)
                    {
                        a.ResetCurrentValues();
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
                catch (Exception exception)
                {
                    // An exception was raised during rendering. This should not happen...
                    var errorMessage = string.Format(
                            "An exception of type {0} was thrown when rendering the plot model.\r\n{1}",
                            exception.GetType(),
                            exception.GetBaseException().StackTrace);
                    this.lastPlotException = exception;
                    this.RenderErrorMessage(rc, string.Format("OxyPlot exception: {0}", exception.Message), errorMessage);
                }
                finally
                {
                    // Clean up unused images
                    rc.CleanUp();
                }
            }
        }

        /// <summary>
        /// Increases margin size if needed, do it on the specified border.
        /// </summary>
        /// <param name="currentMargin">The current margin.</param>
        /// <param name="minBorderSize">Minimum size of the border.</param>
        /// <param name="borderPosition">The border position.</param>
        private static void EnsureMarginIsBigEnough(ref OxyThickness currentMargin, double minBorderSize, AxisPosition borderPosition)
        {
            switch (borderPosition)
            {
                case AxisPosition.Bottom:
                    currentMargin = new OxyThickness(currentMargin.Left, currentMargin.Top, currentMargin.Right, Math.Max(currentMargin.Bottom, minBorderSize));
                    break;

                case AxisPosition.Left:
                    currentMargin = new OxyThickness(Math.Max(currentMargin.Left, minBorderSize), currentMargin.Top, currentMargin.Right, currentMargin.Bottom);
                    break;

                case AxisPosition.Right:
                    currentMargin = new OxyThickness(currentMargin.Left, currentMargin.Top, Math.Max(currentMargin.Right, minBorderSize), currentMargin.Bottom);
                    break;

                case AxisPosition.Top:
                    currentMargin = new OxyThickness(currentMargin.Left, Math.Max(currentMargin.Top, minBorderSize), currentMargin.Right, currentMargin.Bottom);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Calculates the maximum size of the specified axes.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="axesOfPositionTier">The axes of position tier.</param>
        /// <returns>The maximum size.</returns>
        private static double MaxSizeOfPositionTier(IRenderContext rc, IEnumerable<Axis> axesOfPositionTier)
        {
            double maxSizeOfPositionTier = 0;
            foreach (var axis in axesOfPositionTier)
            {
                var size = axis.Measure(rc);
                if (axis.IsVertical())
                {
                    if (size.Width > maxSizeOfPositionTier)
                    {
                        maxSizeOfPositionTier = size.Width;
                    }
                }
                else
                {
                    // caution: this includes AngleAxis because Position=None
                    if (size.Height > maxSizeOfPositionTier)
                    {
                        maxSizeOfPositionTier = size.Height;
                    }
                }
            }

            return maxSizeOfPositionTier;
        }

        /// <summary>
        /// Renders the specified error message.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="title">The title.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="fontSize">The font size. The default value is 12.</param>
        private void RenderErrorMessage(IRenderContext rc, string title, string errorMessage, double fontSize = 12)
        {
            var p0 = new ScreenPoint(10, 10);
            rc.DrawText(p0, title, this.TextColor, fontWeight: FontWeights.Bold, fontSize: fontSize);
            rc.DrawMultilineText(p0 + new ScreenVector(0, fontSize * 1.5), errorMessage, this.TextColor, fontSize: fontSize, dy: fontSize * 1.25);
        }

        /// <summary>
        /// Determines whether the plot margin for the specified axis position is auto-sized.
        /// </summary>
        /// <param name="position">The axis position.</param>
        /// <returns><c>true</c> if it is auto-sized.</returns>
        private bool IsPlotMarginAutoSized(AxisPosition position)
        {
            switch (position)
            {
                case AxisPosition.Left:
                    return double.IsNaN(this.PlotMargins.Left);
                case AxisPosition.Right:
                    return double.IsNaN(this.PlotMargins.Right);
                case AxisPosition.Top:
                    return double.IsNaN(this.PlotMargins.Top);
                case AxisPosition.Bottom:
                    return double.IsNaN(this.PlotMargins.Bottom);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Adjusts the plot margins.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <returns><c>true</c> if the margins were adjusted.</returns>
        private bool AdjustPlotMargins(IRenderContext rc)
        {
            var currentMargin = this.ActualPlotMargins;

            for (var position = AxisPosition.Left; position <= AxisPosition.Bottom; position++)
            {
                var axesOfPosition = this.Axes.Where(a => a.IsAxisVisible && a.Position == position).ToList();
                var requiredSize = this.AdjustAxesPositions(rc, axesOfPosition);

                if (!this.IsPlotMarginAutoSized(position))
                {
                    continue;
                }

                EnsureMarginIsBigEnough(ref currentMargin, requiredSize, position);
            }

            // Special case for AngleAxis which is all around the plot
            var angularAxes = this.Axes.Where(a => a.IsAxisVisible).OfType<AngleAxis>().Cast<Axis>().ToList();

            if (angularAxes.Any())
            {
                var requiredSize = this.AdjustAxesPositions(rc, angularAxes);

                for (var position = AxisPosition.Left; position <= AxisPosition.Bottom; position++)
                {
                    if (!this.IsPlotMarginAutoSized(position))
                    {
                        continue;
                    }

                    EnsureMarginIsBigEnough(ref currentMargin, requiredSize, position);
                }
            }

            if (currentMargin.Equals(this.ActualPlotMargins))
            {
                return false;
            }

            this.ActualPlotMargins = currentMargin;
            return true;
        }

        /// <summary>
        /// Adjust the positions of parallel axes, returns total size
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="parallelAxes">The parallel axes.</param>
        /// <returns>The maximum value of the position tier??</returns>
        private double AdjustAxesPositions(IRenderContext rc, IList<Axis> parallelAxes)
        {
            double maxValueOfPositionTier = 0;

            foreach (var positionTier in parallelAxes.Select(a => a.PositionTier).Distinct().OrderBy(l => l))
            {
                var axesOfPositionTier = parallelAxes.Where(a => a.PositionTier == positionTier).ToList();
                var maxSizeOfPositionTier = MaxSizeOfPositionTier(rc, axesOfPositionTier);
                var minValueOfPositionTier = maxValueOfPositionTier;

                if (Math.Abs(maxValueOfPositionTier) > 1e-5)
                {
                    maxValueOfPositionTier += this.AxisTierDistance;
                }

                maxValueOfPositionTier += maxSizeOfPositionTier;

                foreach (var axis in axesOfPositionTier)
                {
                    axis.PositionTierSize = maxSizeOfPositionTier;
                    axis.PositionTierMinShift = minValueOfPositionTier;
                    axis.PositionTierMaxShift = maxValueOfPositionTier;
                }
            }

            return maxValueOfPositionTier;
        }

        /// <summary>
        /// Measures the size of the title and subtitle.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <returns>Size of the titles.</returns>
        private OxySize MeasureTitles(IRenderContext rc)
        {
            var titleSize = rc.MeasureText(this.Title, this.ActualTitleFont, this.TitleFontSize, this.TitleFontWeight);
            var subtitleSize = rc.MeasureText(this.Subtitle, this.SubtitleFont ?? this.ActualSubtitleFont, this.SubtitleFontSize, this.SubtitleFontWeight);
            double height = titleSize.Height + subtitleSize.Height;
            double width = Math.Max(titleSize.Width, subtitleSize.Width);
            return new OxySize(width, height);
        }

        /// <summary>
        /// Renders the annotations.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="layer">The layer.</param>
        private void RenderAnnotations(IRenderContext rc, AnnotationLayer layer)
        {
            foreach (var a in this.Annotations.Where(a => a.Layer == layer))
            {
                rc.SetToolTip(a.ToolTip);
                a.Render(rc);
            }

            rc.SetToolTip(null);
        }

        /// <summary>
        /// Renders the axes.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="layer">The layer.</param>
        private void RenderAxes(IRenderContext rc, AxisLayer layer)
        {
            // render pass 0
            foreach (var a in this.Axes.Where(a => a.IsAxisVisible && a.Layer == layer))
            {
                rc.SetToolTip(a.ToolTip);
                a.Render(rc, 0);
            }

            // render pass 1
            foreach (var a in this.Axes.Where(a => a.IsAxisVisible && a.Layer == layer))
            {
                rc.SetToolTip(a.ToolTip);
                a.Render(rc, 1);
            }

            rc.SetToolTip(null);
        }

        /// <summary>
        /// Renders the series backgrounds.
        /// </summary>
        /// <param name="rc">The render context.</param>
        private void RenderBackgrounds(IRenderContext rc)
        {
            // Render the main background of the plot area (only if there are axes)
            // The border is rendered by DrawRectangleAsPolygon to ensure that it is pixel aligned with the tick marks.
            if (this.Axes.Count > 0 && this.PlotAreaBackground.IsVisible())
            {
                rc.DrawRectangleAsPolygon(this.PlotArea, this.PlotAreaBackground, OxyColors.Undefined, 0);
            }

            foreach (var s in this.Series.Where(s => s.IsVisible && s is XYAxisSeries && s.Background.IsVisible()).Cast<XYAxisSeries>())
            {
                rc.DrawRectangle(s.GetScreenRectangle(), s.Background, OxyColors.Undefined, 0);
            }
        }

        /// <summary>
        /// Renders the border around the plot area.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <remarks>The border will only by rendered if there are axes in the plot.</remarks>
        private void RenderBox(IRenderContext rc)
        {
            // The border is rendered by DrawRectangleAsPolygon to ensure that it is pixel aligned with the tick marks (cannot use DrawRectangle here).
            if (this.Axes.Count > 0)
            {
                rc.DrawRectangleAsPolygon(this.PlotArea, OxyColors.Undefined, this.PlotAreaBorderColor, this.PlotAreaBorderThickness);
            }
        }

        /// <summary>
        /// Renders the series.
        /// </summary>
        /// <param name="rc">The render context.</param>
        private void RenderSeries(IRenderContext rc)
        {
            foreach (var s in this.Series.Where(s => s.IsVisible))
            {
                rc.SetToolTip(s.ToolTip);
                s.Render(rc);
            }

            rc.SetToolTip(null);
        }

        /// <summary>
        /// Renders the title and subtitle.
        /// </summary>
        /// <param name="rc">The render context.</param>
        private void RenderTitle(IRenderContext rc)
        {
            var titleSize = rc.MeasureText(this.Title, this.ActualTitleFont, this.TitleFontSize, this.TitleFontWeight);

            double x = (this.TitleArea.Left + this.TitleArea.Right) * 0.5;
            double y = this.TitleArea.Top;

            if (!string.IsNullOrEmpty(this.Title))
            {
                rc.SetToolTip(this.TitleToolTip);

                rc.DrawMathText(
                    new ScreenPoint(x, y),
                    this.Title,
                    this.TitleColor.GetActualColor(this.TextColor),
                    this.ActualTitleFont,
                    this.TitleFontSize,
                    this.TitleFontWeight,
                    0,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top);
                y += titleSize.Height;

                rc.SetToolTip(null);
            }

            if (!string.IsNullOrEmpty(this.Subtitle))
            {
                rc.DrawMathText(
                    new ScreenPoint(x, y),
                    this.Subtitle,
                    this.SubtitleColor.GetActualColor(this.TextColor),
                    this.ActualSubtitleFont,
                    this.SubtitleFontSize,
                    this.SubtitleFontWeight,
                    0,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top);
            }
        }

        /// <summary>
        /// Calculates the plot area (subtract padding, title size and outside legends)
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        private void UpdatePlotArea(IRenderContext rc)
        {
            var plotArea = new OxyRect(
                this.Padding.Left,
                this.Padding.Top,
                Math.Max(0, this.Width - this.Padding.Left - this.Padding.Right),
                Math.Max(0, this.Height - this.Padding.Top - this.Padding.Bottom));

            var titleSize = this.MeasureTitles(rc);

            if (titleSize.Height > 0)
            {
                var titleHeight = titleSize.Height + this.TitlePadding;
                plotArea = new OxyRect(plotArea.Left, plotArea.Top + titleHeight, plotArea.Width, Math.Max(0, plotArea.Height - titleHeight));
            }

            plotArea = plotArea.Deflate(this.ActualPlotMargins);

            // Find the available size for the legend box
            var availableLegendWidth = plotArea.Width;
            var availableLegendHeight = double.IsNaN(this.LegendMaxHeight) ?
                plotArea.Height : Math.Min(plotArea.Height, this.LegendMaxHeight);
            if (this.LegendPlacement == LegendPlacement.Inside)
            {
                availableLegendWidth -= this.LegendMargin * 2;
                availableLegendHeight -= this.LegendMargin * 2;
            }

            // Calculate the size of the legend box
            var legendSize = this.MeasureLegends(rc, new OxySize(Math.Max(0, availableLegendWidth), Math.Max(0, availableLegendHeight)));

            // Adjust the plot area after the size of the legend box has been calculated
            if (this.IsLegendVisible && this.LegendPlacement == LegendPlacement.Outside)
            {
                switch (this.LegendPosition)
                {
                    case LegendPosition.LeftTop:
                    case LegendPosition.LeftMiddle:
                    case LegendPosition.LeftBottom:
                        plotArea = new OxyRect(plotArea.Left + legendSize.Width + this.LegendMargin, plotArea.Top, Math.Max(0, plotArea.Width - (legendSize.Width + this.LegendMargin)), plotArea.Height);
                        break;
                    case LegendPosition.RightTop:
                    case LegendPosition.RightMiddle:
                    case LegendPosition.RightBottom:
                        plotArea = new OxyRect(plotArea.Left, plotArea.Top, Math.Max(0, plotArea.Width - (legendSize.Width + this.LegendMargin)), plotArea.Height);
                        break;
                    case LegendPosition.TopLeft:
                    case LegendPosition.TopCenter:
                    case LegendPosition.TopRight:
                        plotArea = new OxyRect(plotArea.Left, plotArea.Top + legendSize.Height + this.LegendMargin, plotArea.Width, Math.Max(0, plotArea.Height - (legendSize.Height + this.LegendMargin)));
                        break;
                    case LegendPosition.BottomLeft:
                    case LegendPosition.BottomCenter:
                    case LegendPosition.BottomRight:
                        plotArea = new OxyRect(plotArea.Left, plotArea.Top, plotArea.Width, Math.Max(0, plotArea.Height - (legendSize.Height + this.LegendMargin)));
                        break;
                }
            }

            // Ensure the plot area is valid
            if (plotArea.Height < 0)
            {
                plotArea = new OxyRect(plotArea.Left, plotArea.Top, plotArea.Width, 1);
            }

            if (plotArea.Width < 0)
            {
                plotArea = new OxyRect(plotArea.Left, plotArea.Top, 1, plotArea.Height);
            }

            this.PlotArea = plotArea;
            this.PlotAndAxisArea = plotArea.Inflate(this.ActualPlotMargins);

            switch (this.TitleHorizontalAlignment)
            {
                case TitleHorizontalAlignment.CenteredWithinView:
                    this.TitleArea = new OxyRect(
                        0,
                        this.Padding.Top,
                        this.Width,
                        titleSize.Height + (this.TitlePadding * 2));
                    break;
                default:
                    this.TitleArea = new OxyRect(
                        this.PlotArea.Left,
                        this.Padding.Top,
                        this.PlotArea.Width,
                        titleSize.Height + (this.TitlePadding * 2));
                    break;
            }

            this.LegendArea = this.GetLegendRectangle(legendSize);
        }
    }
}