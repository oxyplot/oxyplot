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
            double animationFrameDurationInMs = 10)
        {
            if (duration == default(TimeSpan))
            {
                duration = TimeSpan.FromMilliseconds(300);
            }

            var animationDuration = TimeSpan.FromMilliseconds(animationFrameDurationInMs);
            var animationFrames = (int)(duration.TotalMilliseconds / animationFrameDurationInMs);

            var valuesToAnimate = new List<List<double>>();

            var items = series.ItemsSource.Cast<object>().ToList();

            foreach (var point in items)
            {
                var animatablePoint = point as IAnimatablePoint;
                if (animatablePoint != null)
                {
                    var pointMinimum = 0d;
                    if (minimumValue.HasValue)
                    {
                        pointMinimum = minimumValue.Value;
                    }

                    var delta = animatablePoint.FinalY - pointMinimum;
                    var animationValues = CalculateEaseValues(delta, animationFrames - 1, easingFunction, pointMinimum);
                    animationValues.Add(animatablePoint.Y);

                    valuesToAnimate.Add(animationValues);
                    animatablePoint.Y = animationValues.First();
                }
            }

            // First frame animation
            await Task.Delay(animationDuration);

            for (var i = 0; i < animationFrames; i++)
            {
                for (var j = 0; j < items.Count; j++)
                {
                    var animatablePoint = items[j] as IAnimatablePoint;
                    if (animatablePoint != null)
                    {
                        var animationInfo = valuesToAnimate[j];
                        animatablePoint.Y = animationInfo[i];
                    }
                }

                plotModel.InvalidatePlot(true);

                await Task.Delay(animationDuration);
            }
        }
    }
}