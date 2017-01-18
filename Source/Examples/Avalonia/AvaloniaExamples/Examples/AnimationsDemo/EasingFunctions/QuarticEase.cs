// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuadraticEase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AvaloniaExamples.Examples.AnimationsDemo
{
    public class QuarticEase : IEasingFunction
    {
        public double Ease(double value)
        {
            double num = value;
            return num * num * value * value;
        }
    }
}