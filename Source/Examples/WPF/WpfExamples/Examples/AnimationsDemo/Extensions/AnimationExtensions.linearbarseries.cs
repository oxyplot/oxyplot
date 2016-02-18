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
        public static async void AnimateSeries(
            this PlotModel plotModel,
            LinearBarSeries series,
            IEasingFunction easingFunction,
            double? minimumValue = null,
            TimeSpan duration = default(TimeSpan),
            int animationFrameDurationInMs = 10)
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

            var horizontalDuration = duration.TotalMilliseconds / 100 * 70;
            var verticalDuration = duration.TotalMilliseconds / 100 * 30;

            var horizontalMsPerPoint = horizontalDuration / points.Count;

            var animationFrameCount = (int)(duration.TotalMilliseconds / animationFrameDurationInMs);
            var animationFrameDuration = TimeSpan.FromMilliseconds(animationFrameDurationInMs);

            for (var i = 0; i < animationFrameCount; i++)
            {
                var animationFrame = new AnimationFrame
                {
                    Duration = animationFrameDuration
                };

                var currentTime = i * animationFrameDurationInMs;

                var lastVisibleHorizontalPoint = currentTime / horizontalMsPerPoint;
                var firstHorizontalPointThatStillNeedsAnimation = (currentTime - verticalDuration) / horizontalMsPerPoint;

                for (var j = 0; j < points.Count; j++)
                {
                    var point = points[j];

                    var x = point.X;
                    var y = 0d;

                    if (j <= lastVisibleHorizontalPoint)
                    {
                        if (j < firstHorizontalPointThatStillNeedsAnimation)
                        {
                            y = point.Y;
                        }
                        else
                        {
                            // This bar is part of the current animation, calculate the Y relative to 30 % of the time based on the current index
                            var startTime = j * horizontalMsPerPoint;
                            var endTime = startTime + verticalDuration;

                            var totalTimeDelta = endTime - startTime;
                            var offset = currentTime - startTime;

                            // Calculate the % that offset is based on totalTimeDelta
                            var percentage = offset * 100 / totalTimeDelta;

                            // Calculate point to animate from
                            var maxY = point.FinalY;
                            var minY = 0d;
                            if (minimumValue.HasValue)
                            {
                                minY = minimumValue.Value;
                            }

                            var deltaY = maxY - minY;

                            // We need to ease against percentage (between 0 and 1)
                            var ease = easingFunction.Ease(percentage / 100);
                            var currentDeltaY = deltaY * ease;

                            y = minY + currentDeltaY;
                        }
                    }

                    animationFrame.AnimationPoints.Add(new AnimationPoint
                    {
                        X = x,
                        Y = y
                    });
                }

                animationFrames.Add(animationFrame);
            }

            plotModel.AnimateSeries(series, animationFrames);
        }
    }
}