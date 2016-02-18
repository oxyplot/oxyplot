// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BounceEase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace AnimationsDemo
{
    using System;

    public class BounceEase : IEasingFunction
    {
        public BounceEase()
        {
            this.Bounces = 3;
            this.Bounciness = 2d;
        }

        public int Bounces { get; set; }

        public double Bounciness { get; set; }

        public double Ease(double value)
        {
            double y1 = Math.Max(0.0, (double)this.Bounces);
            double num1 = this.Bounciness;
            if (num1 < 1.0 || num1 == 1d)
                num1 = 1001.0 / 1000.0;
            double num2 = Math.Pow(num1, y1);
            double num3 = 1.0 - num1;
            double num4 = (1.0 - num2) / num3 + num2 * 0.5;
            double y2 = Math.Floor(Math.Log(-(value * num4) * (1.0 - num1) + 1.0, num1));
            double y3 = y2 + 1.0;
            double num5 = (1.0 - Math.Pow(num1, y2)) / (num3 * num4);
            double num6 = (1.0 - Math.Pow(num1, y3)) / (num3 * num4);
            double num7 = (num5 + num6) * 0.5;
            double num8 = value - num7;
            double num9 = num7 - num5;
            double num10 = -Math.Pow(1.0 / num1, y1 - y2);
            double num11 = num9;
            double num12 = num11 * num11;
            return num10 / num12 * (num8 - num9) * (num8 + num9);
        }
    }
}