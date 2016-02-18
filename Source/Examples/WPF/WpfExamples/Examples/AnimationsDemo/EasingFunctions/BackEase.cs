// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackEase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;

    public class BackEase : IEasingFunction
    {
        public BackEase()
        {
            this.Amplitude = 1d;
        }

        public double Amplitude { get; set; }

        public double Ease(double value)
        {
            var num = Math.Max(0.0, this.Amplitude);
            return Math.Pow(value, 3.0) - value * num * Math.Sin(Math.PI * value);
        }
    }
}