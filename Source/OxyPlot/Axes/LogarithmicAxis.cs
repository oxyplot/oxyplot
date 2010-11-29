using System;
using System.Collections.Generic;

namespace OxyPlot
{
    public class LogarithmicAxis : AxisBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis"/> class.
        /// </summary>
        public LogarithmicAxis()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="title">The title.</param>
        public LogarithmicAxis(AxisPosition position, string title = null)
            : this()
        {
            this.Position = position;
            this.Title = title;
        }

        public override void GetTickValues(out ICollection<double> majorValues, out ICollection<double> minorValues)
        {
            if (ActualMinimum <= 0) ActualMinimum = 0.1;
            var e0 = (int)Math.Floor(Math.Log10(ActualMinimum));
            var e1 = (int)Math.Ceiling(Math.Log10(ActualMaximum));
            double d0 = Math.Pow(10, e0);
            double d1 = Math.Pow(10, e1);
            double d = d0;
            majorValues = new List<double>();
            minorValues = new List<double>();
            while (d <= d1 + double.Epsilon)
            {
                if (d >= ActualMinimum && d <= ActualMaximum)
                    majorValues.Add(d);
                for (int i = 1; i <= 9; i++)
                {
                    double d2 = d * (i + 1);
                    if (d2 > d1 + double.Epsilon) break;
                    if (d2 > ActualMaximum) break;
                    if (d2 > ActualMinimum)
                        minorValues.Add(d2);
                }
                d *= 10;
            }
        }

        protected override double PreTransform(double x)
        {
            if (x < 0)
                return -1;
            return Math.Log(x);
        }

        protected override double PostInverseTransform(double x)
        {
            return Math.Exp(x);
        }

        public override void Pan(double dx)
        {
            // base.Pan(dx);
            // TODO...
        }
        public override void ZoomAt(double factor, double x)
        {
            // base.ScaleAt(factor, x);
            // TODO...
        }
    }
}