// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnimationViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;
    using System.Threading.Tasks;

    public interface IAnimationViewModel
    {
        bool SupportsEasingFunction { get; }

        Task AnimateAsync();
        Task AnimateAsync(AnimationSettings animationSettings);
    }
}