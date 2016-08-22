// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircleEase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;

    using AnimationsDemo;

    public class CircleEase : IEasingFunction
    {
        public double Ease(double value)
        {
            value = Math.Max(0.0, Math.Min(1.0, value));

            var num1 = 1.0;
            var num2 = 1.0;
            var num3 = value;
            var num4 = num3 * num3;
            var num5 = Math.Sqrt(num2 - num4);

            return num1 - num5;
        }
    }
}