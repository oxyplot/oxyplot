// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pnl.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;

    public class Pnl
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0:HH:mm} {1:0.0}", this.Time, this.Value);
        }
    }
}