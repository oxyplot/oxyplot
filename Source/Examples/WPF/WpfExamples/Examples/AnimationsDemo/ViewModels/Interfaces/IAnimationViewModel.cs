// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnimationViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;

    public interface IAnimationViewModel
    {
        bool SupportsEasingFunction { get; }

        void Animate();
        void Animate(IEasingFunction easingFunction, TimeSpan duration, int animationFrameDuration);
    }
}