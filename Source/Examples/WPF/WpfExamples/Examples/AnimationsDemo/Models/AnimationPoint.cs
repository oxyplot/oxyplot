// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationPoint.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AnimationsDemo
{
    using System.Diagnostics;

    [DebuggerDisplay("{X} / {Y} (IsVisible = {IsVisible})")]
    public class AnimationPoint
    {
        public AnimationPoint()
        {
            this.IsVisible = true;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public bool IsVisible { get; set; }
    }
}