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

    using OxyPlot.Series;

    public static partial class AnimationExtensions
    {
        private static readonly IEasingFunction DefaultEasingFunction = new NoEase();

        static AnimationExtensions()
        {
            DefaultAnimationDelay = 0;
            DefaultAnimationDuration = 750;
            DefaultAnimationFrameDuration = 10;
        }

        public static int DefaultAnimationDelay { get; set; }

        public static int DefaultAnimationDuration { get; set; }

        public static int DefaultAnimationFrameDuration { get; set; }

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

        private static void InsertDelayAnimationFrame(this List<AnimationFrame> animationFrames, TimeSpan delay)
        {
            if (animationFrames.Count < 2)
            {
                return;
            }

            if (delay <= TimeSpan.Zero)
            {
                return;
            }

            var animationFrame = new AnimationFrame
            {
                Duration = delay
            };

            animationFrames.Insert(1, animationFrame);
        }

        private static List<IAnimatablePoint> GetAnimatablePoints(this DataPointSeries series)
        {
            var points = new List<IAnimatablePoint>();

            var itemsSource = series.ItemsSource;
            if (itemsSource != null)
            {
                points.AddRange(from x in itemsSource.Cast<object>()
                                where x is IAnimatablePoint
                                select (IAnimatablePoint)x);
            }

            return points;
        }
    }
}