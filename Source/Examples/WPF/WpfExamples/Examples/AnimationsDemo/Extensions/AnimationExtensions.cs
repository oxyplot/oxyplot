// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System.Collections.Generic;

    public static partial class AnimationExtensions
    {
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