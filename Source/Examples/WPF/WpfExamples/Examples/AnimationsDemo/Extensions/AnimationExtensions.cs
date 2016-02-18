// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
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

    public static class AnimationExtensions
    {
        public static void AnimateSeries<TEase>(
            this PlotModel plotModel,
            ItemsSeries series,
            double? minimumValue = null,
            TimeSpan duration = default(TimeSpan),
            double animationFrameDurationInMs = 10) 
            where TEase : IEasingFunction, new()
        {
            var easingFunction = new TEase();

            AnimateSeries(plotModel, series, easingFunction, minimumValue, duration, animationFrameDurationInMs);
        }

        public static async void AnimateSeries(
            this PlotModel plotModel,
            ItemsSeries series,
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
                var measurePoint = point as Pnl;
                if (measurePoint != null)
                {
                    var pointMinimum = 0d;
                    if (minimumValue.HasValue)
                    {
                        pointMinimum = minimumValue.Value;
                    }

                    var delta = measurePoint.Value - pointMinimum;
                    var animationValues = CalculateEaseValues(delta, animationFrames - 1, easingFunction, pointMinimum);
                    animationValues.Add(measurePoint.Value);

                    valuesToAnimate.Add(animationValues);
                    measurePoint.Value = animationValues.First();
                }
            }

            // First frame animation
            await Task.Delay(animationDuration);

            for (var i = 0; i < animationFrames; i++)
            {
                for (var j = 0; j < items.Count; j++)
                {
                    var measurePoint = items[j] as Pnl;
                    if (measurePoint != null)
                    {
                        var animationInfo = valuesToAnimate[j];
                        measurePoint.Value = animationInfo[i];
                    }
                }

                plotModel.InvalidatePlot(true);

                await Task.Delay(animationDuration);
            }
        }

        private static List<double> CalculateEaseValues(double range, int count, IEasingFunction easingFunction, double baseValue = 0d)
        {
            var items = new List<double>();

            var easePhase = 1d / count;

            for (var i = 0; i < count; i++)
            {
                var valueToEase = easePhase * i;
                var easeValue = easingFunction.Ease(valueToEase);

                items.Add(baseValue + (easeValue * range));
            }

            return items;
        }
    }
}