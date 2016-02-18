// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerEase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;

    public class PowerEase : IEasingFunction
    {
        public PowerEase()
        {
            this.Power = 2d;
        }

        public double Power { get; set; }

        public double Ease(double value)
        {
            double y = Math.Max(0.0, this.Power);
            return Math.Pow(value, y);
        }
    }
}