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
            LinearBarSeries series,
            IEasingFunction easingFunction,
            double? minimumValue = null,
            TimeSpan duration = default(TimeSpan),
            int? animationFrameDurationInMs = null)
        {
            if (duration == default(TimeSpan))
            {
                duration = TimeSpan.FromMilliseconds(300);
            }

            var points = series.GetAnimatablePoints();
            if (points.Count == 0)
            {
                return;
            }

            var animationFrames = new List<AnimationFrame>();

            // Animation assumptions:
            // 
            //   - Total duration: 300ms (in this example)
            //   - At least animate each horizontal value separately
            //   - First animate from left => right (70 % of the time)
            //   - Second animate from center => end (30 % of the time)
            // 
            // |                                                  ^
            // |                                                  |
            // |               ___                                |
            // |               | |                                |  30 % of time
            // |   ___         | |                                |     90 ms
            // |   | |         | |                                |
            // |   | |   ___   | |                                |
            // |   | |   | |   | |                                |
            // |___|_|___|_|___|_|_____________________________   ^
            // 
            // <---------------- 70 % of time ---------------->
            //                     (210 ms)

            const double HorizontalPercentage = 70d;
            const double VerticalPercentage = 30d;

            var horizontalDuration = duration.TotalMilliseconds / 100 * HorizontalPercentage;
            var verticalDuration = duration.TotalMilliseconds / 100 * VerticalPercentage;

            if (!animationFrameDurationInMs.HasValue)
            {
                animationFrameDurationInMs = DefaultAnimationFrameDuration;
            }

            var animationFrameCount = (int)(duration.TotalMilliseconds / animationFrameDurationInMs);
            var animationFrameDuration = TimeSpan.FromMilliseconds(animationFrameDurationInMs.Value);

            var minX = (from point in points orderby point.X select point.X).Min();
            var maxX = (from point in points orderby point.X select point.X).Max();
            var deltaX = maxX - minX;

            for (var i = 0; i < animationFrameCount; i++)
            {
                var animationFrame = new AnimationFrame
                {
                    Duration = animationFrameDuration
                };

                var currentTime = animationFrameDurationInMs.Value * i;
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
                    var x = points[j].FinalX;
                    if (x > currentX)
                    {
                        break;
                    }

                    lastVisibleHorizontalPoint = j;
                }

                for (var j = 0; j < points.Count; j++)
                {
                    var point = points[j];

                    var isVisible = false;
                    var x = point.FinalX;
                    var y = 0d;

                    if (j <= lastVisibleHorizontalPoint)
                    {
                        isVisible = true;
                        y = point.FinalY;

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
                            var subPercentage = timeDeltaCurrent * 100d / timeDeltaTotal;

                            // This bar is part of the current animation, calculate the Y relative to 30 % of the time based on the current index
                            // Calculate the % that offset is based on totalTimeDelta

                            // Calculate point to animate from
                            var maxY = point.FinalY;
                            var minY = 0d;
                            if (minimumValue.HasValue)
                            {
                                minY = minimumValue.Value;
                            }

                            var deltaY = maxY - minY;

                            // We need to ease against percentage (between 0 and 1)
                            var ease = easingFunction.Ease(subPercentage / 100);
                            var currentDeltaY = deltaY * ease;

                            y = minY + currentDeltaY;
                        }
                    }

                    animationFrame.AnimationPoints.Add(new AnimationPoint
                    {
                        IsVisible = isVisible,
                        X = x,
                        Y = y
                    });
                }

                animationFrames.Add(animationFrame);
            }

            await plotModel.AnimateSeriesAsync(series, animationFrames);
        }
    }
}