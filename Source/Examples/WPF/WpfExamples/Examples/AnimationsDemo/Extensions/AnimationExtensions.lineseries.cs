// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OxyPlot;
    using OxyPlot.Series;

    public static partial class AnimationExtensions
    {
        public static async Task AnimateSeriesAsync(
            this PlotModel plotModel,
            LineSeries series,
            AnimationSettings settings)
        {
            var points = series.GetAnimatablePoints();
            if (points.Count == 0)
            {
                return;
            }

            var duration = settings.Duration;
            if (duration == default(TimeSpan))
            {
                duration = TimeSpan.FromMilliseconds(DefaultAnimationDuration);
            }

            var easingFunction = settings.EasingFunction;
            if (easingFunction == null)
            {
                easingFunction = DefaultEasingFunction;
            }

            var animationFrameDurationInMs = (int)settings.FrameDuration.TotalMilliseconds;
            var minimumValue = settings.MinimumValue;

            var animationFrames = new List<AnimationFrame>();

            var horizontalDuration = duration.TotalMilliseconds / 100 * settings.HorizontalPercentage;
            var verticalDuration = duration.TotalMilliseconds / 100 * settings.VerticalPercentage;

            var animationFrameCount = (int)(duration.TotalMilliseconds / animationFrameDurationInMs);
            var animationFrameDuration = TimeSpan.FromMilliseconds(animationFrameDurationInMs);

            if (!minimumValue.HasValue)
            {
                minimumValue = 0d;

                var defaultYAxis = plotModel.DefaultYAxis;
                if (defaultYAxis != null)
                {
                    if (defaultYAxis.Minimum > 0d)
                    {
                        minimumValue = defaultYAxis.Minimum;
                    }
                }
            }

            var minX = (from point in points orderby point.X select point.X).Min();
            var maxX = (from point in points orderby point.X select point.X).Max();
            var deltaX = maxX - minX;

            for (var i = 0; i < animationFrameCount; i++)
            {
                var animationFrame = new AnimationFrame
                {
                    Duration = animationFrameDuration
                };

                var currentTime = animationFrameDurationInMs * i;
                var percentage = i * 100d / animationFrameCount;

                var horizontalPercentage = currentTime * 100d / horizontalDuration;
                if (horizontalPercentage > 100d)
                {
                    horizontalPercentage = 100d;
                }

                var currentDeltaX = deltaX / 100d * horizontalPercentage;
                var currentX = minX + currentDeltaX;

                // Get the last visible point. It should not be based on the index (can be really different), but on the X position
                // since we want to draw a smooth animation
                var lastVisibleHorizontalPoint = 0;
                for (int j = 0; j < points.Count; j++)
                {
                    if (points[j].FinalX > currentX)
                    {
                        break;
                    }

                    lastVisibleHorizontalPoint = j;
                }

                for (var j = 0; j < points.Count; j++)
                {
                    var point = points[j];

                    var isVisible = true;
                    var y = point.FinalY;
                    var x = point.FinalX;

                    var nextPointIndex = lastVisibleHorizontalPoint + 1;

                    if (j > nextPointIndex)
                    {
                        isVisible = false;
                    }

                    if (j >= nextPointIndex)
                    {
                        // Calculate the position the y line is in currently (based on x1 and x2)
                        var previousPoint = points[lastVisibleHorizontalPoint];
                        var nextPoint = points[nextPointIndex];

                        var previousX = previousPoint.FinalX;
                        var nextX = nextPoint.FinalX;

                        var totalDeltaX = nextX - previousX;
                        var subDeltaX = currentX - previousX;

                        var subPercentage = (subDeltaX * 100) / totalDeltaX;

                        var previousY = previousPoint.FinalY;
                        var nextY = nextPoint.FinalY;
                        var totalDeltaY = nextY - previousY;

                        var subDeltaY = totalDeltaY / 100 * subPercentage;

                        y = previousY + subDeltaY;
                        x = currentX;
                    }

                    // Calculate possible ease functions
                    if (j < nextPointIndex)
                    {
                        // We know how long an y animation takes. We only have to calculate if this start time of this x animation
                        // is longer than verticalDuration ago
                        var localDeltaX = point.FinalX - minX;
                        var localPercentageX = localDeltaX * 100d / deltaX;
                        var startTime = horizontalDuration / 100 * localPercentageX;
                        var endTime = startTime + verticalDuration;
                        if (endTime > currentTime)
                        {
                            var timeDeltaTotal = endTime - startTime;
                            var timeDeltaCurrent = currentTime - startTime;
                            var easePercentage = timeDeltaCurrent * 100d / timeDeltaTotal;

                            // This bar is part of the current animation, calculate the Y relative to 30 % of the time based on the current index
                            // Calculate the % that offset is based on totalTimeDelta

                            // Calculate point to animate from
                            var maxY = point.FinalY;
                            var minY = minimumValue.Value;
                            var deltaY = maxY - minY;

                            // We need to ease against percentage (between 0 and 1)
                            var ease = easingFunction.Ease(easePercentage / 100);
                            var currentDeltaY = deltaY * ease;

                            y = minY + currentDeltaY;
                        }
                    }

                    animationFrame.AnimationPoints.Add(new AnimationPoint
                    {
                        X = x,
                        Y = y,
                        IsVisible = isVisible
                    });
                }

                animationFrames.Add(animationFrame);
            }

            animationFrames.InsertDelayAnimationFrame(settings.Delay);

            await plotModel.AnimateSeriesAsync(series, animationFrames);
        }
    }
}