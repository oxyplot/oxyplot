// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.Rendering.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Renders the plot with the specified rendering context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    /// <summary>
    /// Represents a plot.
    /// </summary>
    public partial class PlotModel
    {
        /// <summary>
        /// Renders the plot with the specified rendering context within the given rectangle.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="rect">The plot bounds.</param>
        void IPlotModel.Render(IRenderContext rc, OxyRect rect)
        {
            this.RenderOverride(rc, rect);
        }

        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="rect">The plot bounds.</param>
        protected virtual void RenderOverride(IRenderContext rc, OxyRect rect)
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

                    this.PlotBounds = rect;

                    this.ActualPlotMargins =
                        new OxyThickness(
                            double.IsNaN(this.PlotMargins.Left) ? 0 : this.PlotMargins.Left,
                            double.IsNaN(this.PlotMargins.Top) ? 0 : this.PlotMargins.Top,
                            double.IsNaN(this.PlotMargins.Right) ? 0 : this.PlotMargins.Right,
                            double.IsNaN(this.PlotMargins.Bottom) ? 0 : this.PlotMargins.Bottom);

                    foreach (var l in this.Legends)
                    {
                        l.EnsureLegendProperties();
                    }

                    for (var i = 0; i < 10; i++) // make we sure we don't loop infinitely
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
                        this.RenderLegends(rc);
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
        /// Adjusts the plot margins.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <returns><c>true</c> if the margins were adjusted.</returns>
        private bool AdjustPlotMargins(IRenderContext rc)
        {
            var visibleAxes = this.Axes.Where(axis => axis.IsAxisVisible).ToList();
            foreach (var axis in visibleAxes)
            {
                axis.Measure(rc);
            }

            var desiredMargin = new OxyThickness();

            void IncludeInMargin(double size, AxisPosition borderPosition)
            {
                desiredMargin = borderPosition switch
                {
                    AxisPosition.Bottom => new OxyThickness(desiredMargin.Left, desiredMargin.Top, desiredMargin.Right, Math.Max(desiredMargin.Bottom, size)),
                    AxisPosition.Left => new OxyThickness(Math.Max(desiredMargin.Left, size), desiredMargin.Top, desiredMargin.Right, desiredMargin.Bottom),
                    AxisPosition.Right => new OxyThickness(desiredMargin.Left, desiredMargin.Top, Math.Max(desiredMargin.Right, size), desiredMargin.Bottom),
                    AxisPosition.Top => new OxyThickness(desiredMargin.Left, Math.Max(desiredMargin.Top, size), desiredMargin.Right, desiredMargin.Bottom),
                    _ => desiredMargin,
                };
            }

            // include the value of the outermost position tier on each side ('normal' axes only)
            for (var position = AxisPosition.Left; position <= AxisPosition.Bottom; position++)
            {
                var axesOfPosition = visibleAxes.Where(a => a.Position == position);
                var requiredSize = this.AdjustAxesPositions(axesOfPosition);
                IncludeInMargin(requiredSize, position);
            }

            // include the desired margin of all visible axes (including polar axes)
            foreach (var axis in visibleAxes)
            {
                desiredMargin = desiredMargin.Include(axis.DesiredMargin);
            }

            var currentMargin = this.PlotMargins;
            currentMargin = new OxyThickness(
                double.IsNaN(currentMargin.Left) ? desiredMargin.Left : currentMargin.Left,
                double.IsNaN(currentMargin.Top) ? desiredMargin.Top : currentMargin.Top,
                double.IsNaN(currentMargin.Right) ? desiredMargin.Right : currentMargin.Right,
                double.IsNaN(currentMargin.Bottom) ? desiredMargin.Bottom : currentMargin.Bottom);

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
        /// <param name="parallelAxes">The parallel axes.</param>
        /// <returns>The maximum value of the position tier??</returns>
        private double AdjustAxesPositions(IEnumerable<Axis> parallelAxes)
        {
            double maxValueOfPositionTier = 0;

            static double GetSize(Axis axis)
            {
                return axis.Position switch
                {
                    AxisPosition.Left => axis.DesiredMargin.Left,
                    AxisPosition.Right => axis.DesiredMargin.Right,
                    AxisPosition.Top => axis.DesiredMargin.Top,
                    AxisPosition.Bottom => axis.DesiredMargin.Bottom,
                    _ => throw new InvalidOperationException(), // we don't do this for polar axes
                };
            }

            foreach (var tierGroup in parallelAxes.GroupBy(a => a.PositionTier).OrderBy(group => group.Key))
            {
                var axesOfPositionTier = tierGroup.ToList();
                var maxSizeOfPositionTier = axesOfPositionTier.Max(GetSize);

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

        private void RenderLegends(IRenderContext rc)
        {
            if (this.IsLegendVisible)
            {
                foreach (var l in this.Legends.Where(l => l.IsLegendVisible))
                {
                    rc.SetToolTip(l.ToolTip);
                    l.RenderLegends(rc);
                }
            }
        }

        /// <summary>
        /// Renders the series backgrounds.
        /// </summary>
        /// <param name="rc">The render context.</param>
        private void RenderBackgrounds(IRenderContext rc)
        {
            // Render the main background of the plot area (only if there are axes)
            if (this.Axes.Count > 0 && this.PlotAreaBackground.IsVisible())
            {
                rc.DrawRectangle(this.PlotArea, this.PlotAreaBackground, OxyColors.Undefined, 0, this.EdgeRenderingMode);
            }

            foreach (var s in this.Series.Where(s => s.IsVisible && s is XYAxisSeries && s.Background.IsVisible()).Cast<XYAxisSeries>())
            {
                rc.DrawRectangle(s.GetScreenRectangle(), s.Background, OxyColors.Undefined, 0, this.EdgeRenderingMode);
            }
        }

        /// <summary>
        /// Renders the border around the plot area.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <remarks>The border will only by rendered if there are axes in the plot.</remarks>
        private void RenderBox(IRenderContext rc)
        {
            if (this.Axes.Count > 0)
            {
                rc.DrawRectangle(this.PlotArea, this.PlotAreaBorderColor, this.PlotAreaBorderThickness, this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
            }
        }

        /// <summary>
        /// Renders the series.
        /// </summary>
        /// <param name="rc">The render context.</param>
        private void RenderSeries(IRenderContext rc)
        {
            foreach (var barSeriesManager in this.barSeriesManagers)
            {
                barSeriesManager.InitializeRender();
            }

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
            OxySize? maxSize = null;

            if (this.ClipTitle)
            {
                maxSize = new OxySize(this.TitleArea.Width * this.TitleClippingLength, double.MaxValue);
            }

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
                    VerticalAlignment.Top,
                    maxSize);
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
                    VerticalAlignment.Top,
                    maxSize);
            }
        }

        /// <summary>
        /// Calculates the plot area (subtract padding, title size and outside legends)
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        private void UpdatePlotArea(IRenderContext rc)
        {
            var plotArea = new OxyRect(
                this.PlotBounds.Left + this.Padding.Left,
                this.PlotBounds.Top + this.Padding.Top,
                Math.Max(0, this.Width - this.Padding.Left - this.Padding.Right),
                Math.Max(0, this.Height - this.Padding.Top - this.Padding.Bottom));

            var titleSize = this.MeasureTitles(rc);

            if (titleSize.Height > 0)
            {
                var titleHeight = titleSize.Height + this.TitlePadding;
                plotArea = new OxyRect(plotArea.Left, plotArea.Top + titleHeight, plotArea.Width, Math.Max(0, plotArea.Height - titleHeight));
            }

            plotArea = plotArea.Deflate(this.ActualPlotMargins);

            if (this.IsLegendVisible)
            {
                // Make space for legends

                OxySize maxLegendSize = new OxySize(0, 0);
                double legendMargin = 0;
                // first run Outside Left-Side legends
                foreach (var legend in this.Legends.Where(l =>
                    l.LegendPlacement == LegendPlacement.Outside && (l.IsLegendVisible &&
                    (l.LegendPosition == LegendPosition.LeftTop || l.LegendPosition == LegendPosition.LeftMiddle || l.LegendPosition == LegendPosition.LeftBottom))))
                {
                    // Find the available size for the legend box
                    var availableLegendWidth = plotArea.Width;
                    var availableLegendHeight = double.IsNaN(legend.LegendMaxHeight) ?
                        plotArea.Height : Math.Min(plotArea.Height, legend.LegendMaxHeight);

                    var lsiz = legend.GetLegendSize(rc, new OxySize(availableLegendWidth, availableLegendHeight));
                    legend.LegendSize = lsiz;
                    maxLegendSize = new OxySize(maxLegendSize.Width > lsiz.Width ? maxLegendSize.Width : lsiz.Width, maxLegendSize.Height > lsiz.Height ? maxLegendSize.Height : lsiz.Height);

                    if (legend.LegendMargin > legendMargin)
                        legendMargin = legend.LegendMargin;
                }

                // Adjust the plot area after the size of the legend has been calculated
                if (maxLegendSize.Width > 0 || maxLegendSize.Height > 0)
                {
                    plotArea = new OxyRect(plotArea.Left + maxLegendSize.Width + legendMargin, plotArea.Top, Math.Max(0, plotArea.Width - (maxLegendSize.Width + legendMargin)), plotArea.Height);
                }

                maxLegendSize = new OxySize(0, 0);
                legendMargin = 0;
                // second run Outside Right-Side legends
                foreach (var legend in this.Legends.Where(l =>
                    l.LegendPlacement == LegendPlacement.Outside && (l.IsLegendVisible &&
                    (l.LegendPosition == LegendPosition.RightTop || l.LegendPosition == LegendPosition.RightMiddle || l.LegendPosition == LegendPosition.RightBottom))))
                {
                    // Find the available size for the legend box
                    var availableLegendWidth = plotArea.Width;
                    var availableLegendHeight = double.IsNaN(legend.LegendMaxHeight) ?
                        plotArea.Height : Math.Min(plotArea.Height, legend.LegendMaxHeight);

                    var lsiz = legend.GetLegendSize(rc, new OxySize(availableLegendWidth, availableLegendHeight));
                    legend.LegendSize = lsiz;
                    maxLegendSize = new OxySize(maxLegendSize.Width > lsiz.Width ? maxLegendSize.Width : lsiz.Width, maxLegendSize.Height > lsiz.Height ? maxLegendSize.Height : lsiz.Height);

                    if (legend.LegendMargin > legendMargin)
                        legendMargin = legend.LegendMargin;
                }

                // Adjust the plot area after the size of the legend has been calculated
                if (maxLegendSize.Width > 0 || maxLegendSize.Height > 0)
                {
                    plotArea = new OxyRect(plotArea.Left, plotArea.Top, Math.Max(0, plotArea.Width - (maxLegendSize.Width + legendMargin)), plotArea.Height);
                }

                maxLegendSize = new OxySize(0, 0);
                legendMargin = 0;
                // third run Outside Top legends
                foreach (var legend in this.Legends.Where(l =>
                    l.LegendPlacement == LegendPlacement.Outside && (l.IsLegendVisible &&
                    (l.LegendPosition == LegendPosition.TopLeft || l.LegendPosition == LegendPosition.TopCenter || l.LegendPosition == LegendPosition.TopRight))))
                {
                    // Find the available size for the legend box
                    var availableLegendWidth = plotArea.Width;
                    var availableLegendHeight = double.IsNaN(legend.LegendMaxHeight) ?
                        plotArea.Height : Math.Min(plotArea.Height, legend.LegendMaxHeight);

                    var lsiz = legend.GetLegendSize(rc, new OxySize(availableLegendWidth, availableLegendHeight));
                    legend.LegendSize = lsiz;
                    maxLegendSize = new OxySize(maxLegendSize.Width > lsiz.Width ? maxLegendSize.Width : lsiz.Width, maxLegendSize.Height > lsiz.Height ? maxLegendSize.Height : lsiz.Height);

                    if (legend.LegendMargin > legendMargin)
                        legendMargin = legend.LegendMargin;
                }

                // Adjust the plot area after the size of the legend has been calculated
                if (maxLegendSize.Width > 0 || maxLegendSize.Height > 0)
                {
                    plotArea = new OxyRect(plotArea.Left, plotArea.Top + maxLegendSize.Height + legendMargin, plotArea.Width, Math.Max(0, plotArea.Height - (maxLegendSize.Height + legendMargin)));
                }

                maxLegendSize = new OxySize(0, 0);
                legendMargin = 0;
                // fourth run Outside Bottom legends
                foreach (var legend in this.Legends.Where(l =>
                    l.LegendPlacement == LegendPlacement.Outside && (l.IsLegendVisible &&
                    (l.LegendPosition == LegendPosition.BottomLeft || l.LegendPosition == LegendPosition.BottomCenter || l.LegendPosition == LegendPosition.BottomRight))))
                {
                    // Find the available size for the legend box
                    var availableLegendWidth = plotArea.Width;
                    var availableLegendHeight = double.IsNaN(legend.LegendMaxHeight) ?
                        plotArea.Height : Math.Min(plotArea.Height, legend.LegendMaxHeight);

                    var lsiz = legend.GetLegendSize(rc, new OxySize(availableLegendWidth, availableLegendHeight));
                    legend.LegendSize = lsiz;
                    maxLegendSize = new OxySize(maxLegendSize.Width > lsiz.Width ? maxLegendSize.Width : lsiz.Width, maxLegendSize.Height > lsiz.Height ? maxLegendSize.Height : lsiz.Height);

                    if (legend.LegendMargin > legendMargin)
                        legendMargin = legend.LegendMargin;
                }

                // Adjust the plot area after the size of the legend has been calculated
                if (maxLegendSize.Width > 0 || maxLegendSize.Height > 0)
                {
                    plotArea = new OxyRect(plotArea.Left, plotArea.Top, plotArea.Width, Math.Max(0, plotArea.Height - (maxLegendSize.Height + legendMargin)));
                }

                // Finally calculate size of inside legends
                foreach (var legend in this.Legends.Where(l => l.LegendPlacement == LegendPlacement.Inside && l.IsLegendVisible))
                {
                    // Find the available size for the legend box
                    var availableLegendWidth = plotArea.Width;
                    var availableLegendHeight = double.IsNaN(legend.LegendMaxHeight) ?
                        plotArea.Height : Math.Min(plotArea.Height, legend.LegendMaxHeight);

                    if (legend.LegendPlacement == LegendPlacement.Inside)
                    {
                        availableLegendWidth -= legend.LegendMargin * 2;
                        availableLegendHeight -= legend.LegendMargin * 2;
                    }

                    legend.LegendSize = legend.GetLegendSize(rc, new OxySize(availableLegendWidth, availableLegendHeight));
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
                        this.PlotBounds.Left,
                        this.PlotBounds.Top + this.Padding.Top,
                        this.Width,
                        titleSize.Height + (this.TitlePadding * 2));
                    break;
                default:
                    this.TitleArea = new OxyRect(
                        this.PlotArea.Left,
                        this.PlotBounds.Top + this.Padding.Top,
                        this.PlotArea.Width,
                        titleSize.Height + (this.TitlePadding * 2));
                    break;
            }

            // Calculate the legend area for each legend.
            foreach (var l in this.Legends)
            {
                l.LegendArea = l.GetLegendRectangle(l.LegendSize);
            }
        }
    }
}
