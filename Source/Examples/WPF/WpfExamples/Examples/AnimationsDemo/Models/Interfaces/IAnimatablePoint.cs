// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnimatablePoint.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    public interface IAnimatablePoint
    {
        double FinalX { get; set; }

        double FinalY { get; set; }

        double X { get; set; }

        double Y { get; set; }
    }
}