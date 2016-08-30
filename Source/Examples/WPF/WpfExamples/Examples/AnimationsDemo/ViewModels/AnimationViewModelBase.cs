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

    public abstract class AnimationViewModelBase : IAnimationViewModel
    {
        protected AnimationViewModelBase()
        {
            this.EasingFunctions = (from type in typeof(CircleEase).Assembly.GetTypes()
                                    where type.GetInterfaces().Any(x => x == typeof(IEasingFunction)) && !type.IsAbstract
                                    orderby type.Name
                                    select type).ToList();
            this.SelectedEasingFunction = this.EasingFunctions.FirstOrDefault();

            if (!this.SupportsEasingFunction)
            {
                this.SelectedEasingFunction = typeof(NoEase);
            }

            this.AnimationDuration = 1000;
            this.AnimationFrameDuration = AnimationExtensions.DefaultAnimationFrameDuration;
            this.HorizontalPercentage = 70;
            this.VerticalPercentage = 30;

            this.PlotModel = new PlotModel();
        }

        public abstract bool SupportsEasingFunction { get; }

        public PlotModel PlotModel { get; private set; }

        public List<Type> EasingFunctions { get; private set; }

        public Type SelectedEasingFunction { get; set; }

        public double HorizontalPercentage { get; set; }

        public double VerticalPercentage { get; set; }

        public int AnimationDelay { get; set; }

        public int AnimationDuration { get; set; }

        public int AnimationFrameDuration { get; set; }

        public Task AnimateAsync()
        {
            var easingFunction = (IEasingFunction)Activator.CreateInstance(this.SelectedEasingFunction);

            var animationSettings = new AnimationSettings
            {
                EasingFunction = easingFunction,
                Duration = TimeSpan.FromMilliseconds(this.AnimationDuration),
                FrameDuration = TimeSpan.FromMilliseconds(this.AnimationFrameDuration),
                Delay = TimeSpan.FromMilliseconds(this.AnimationDelay),
                HorizontalPercentage = this.HorizontalPercentage,
                VerticalPercentage = this.VerticalPercentage
            };

            return this.AnimateAsync(animationSettings);
        }

        public abstract Task AnimateAsync(AnimationSettings animationSettings);
    }
}