// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackEase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.AnimationsDemo
{
    public class NoEase : IEasingFunction
    {
        public double Ease(double value)
        {
            return 1d;
        }
    }
}