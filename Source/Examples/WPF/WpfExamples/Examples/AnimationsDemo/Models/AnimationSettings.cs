// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationFrame.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;

    public class AnimationSettings
    {
        public AnimationSettings()
        {
            this.HorizontalPercentage = 70;
            this.VerticalPercentage = 30;

            this.Duration = TimeSpan.FromMilliseconds(750);
            this.AnimationFrameDurationInMs = AnimationExtensions.DefaultAnimationFrameDuration;
        }

        public double? MinimumValue { get; set; }

        public double HorizontalPercentage { get; set; }

        public double VerticalPercentage { get; set; }

        public TimeSpan Duration { get; set; }

        public IEasingFunction EasingFunction { get; set; }

        public int AnimationFrameDurationInMs { get; set; }
    }
}