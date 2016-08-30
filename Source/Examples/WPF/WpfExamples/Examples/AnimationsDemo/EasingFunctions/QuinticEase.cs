// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuinticEase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    public class QuinticEase : IEasingFunction
    {
        public double Ease(double value)
        {
            double num = value;
            return num * num * value * value * value;
        }
    }
}