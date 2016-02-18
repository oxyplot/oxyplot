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

            this.AnimationDuration = 250;
            
            this.PlotModel = new PlotModel();
        }

        public PlotModel PlotModel { get; private set; }

        public List<Type> EasingFunctions { get; private set; }

        public Type SelectedEasingFunction { get; set; }

        public int AnimationDuration { get; set; }

        public void Animate()
        {
            var easingFunction = (IEasingFunction)Activator.CreateInstance(this.SelectedEasingFunction);
            var timeSpan = TimeSpan.FromMilliseconds(this.AnimationDuration);

            this.Animate(easingFunction, timeSpan);
        }

        public abstract void Animate(IEasingFunction easingFunction, TimeSpan duration);
    }
}