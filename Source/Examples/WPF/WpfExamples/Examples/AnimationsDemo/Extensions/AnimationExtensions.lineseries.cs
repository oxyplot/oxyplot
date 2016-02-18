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

    using OxyPlot;
    using OxyPlot.Series;

    public static partial class AnimationExtensions
    {
        public static async void AnimateSeries(
            this PlotModel plotModel,
            LineSeries series,
            IEasingFunction easingFunction,
            double? minimumValue = null,
            TimeSpan duration = default(TimeSpan),
            double animationFrameDurationInMs = 10)
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

            // This line might look a bit weird, but this way we can easily tweak the %
            // it takes to animate the horizontal lines (in case we want to animate markers in the future)
            var horizontalDuration = duration.TotalMilliseconds / 100 * 100;

            var animationFrameCount = (int)(duration.TotalMilliseconds / animationFrameDurationInMs);
            var animationFrameDuration = TimeSpan.FromMilliseconds(animationFrameDurationInMs);

            var minX = (from point in points orderby point.X select point.X).Min();
            var maxX = (from point in points orderby point.X select point.X).Max();
            var deltaX = maxX - minX;

            for (var i = 0; i < animationFrameCount; i++)
            {
                var animationFrame = new AnimationFrame
                {
                    Duration = animationFrameDuration
                };

                var currentTime = i * animationFrameDurationInMs;

                var percentage = 100d / animationFrameCount * i;
                var currentDeltaX = deltaX / 100d * percentage;
                var currentX = minX + currentDeltaX;

                // We need to ease against percentage (between 0 and 1), so the currentX is the x based on the time,
                // the actualX is the value we are going to assign (the eased value, which might be negative)
                var ease = easingFunction.Ease(percentage / 100);
                var actualX = minX + (currentDeltaX * ease);
                if (i == animationFrameCount - 1)
                {
                    actualX = maxX;
                }

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

                    // If we are back easing (so the x is further than our slowed down animation) or 
                    // we hit the next point index that is not yet visible, calculate x and y manually
                    if ((x > actualX) || (j == nextPointIndex))
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
                        x = actualX;
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

            await plotModel.AnimateSeriesAsync(series, animationFrames);
        }
    }
}