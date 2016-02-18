// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationFrame.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System;
    using System.Collections.Generic;

    public class AnimationFrame
    {
        private static readonly TimeSpan DefaultDuration = TimeSpan.FromMilliseconds(15);

        public AnimationFrame()
        {
            AnimationPoints = new List<AnimationPoint>();
            Duration = DefaultDuration;
        }

        public TimeSpan Duration { get; set; }

        public List<AnimationPoint> AnimationPoints { get; private set; }
    }
}