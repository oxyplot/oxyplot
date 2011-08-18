using System;
using System.Collections.Generic;
using System.Linq;

namespace OxyPlot
{
    /// <summary>
    /// Logarithmic axis.
    /// http://en.wikipedia.org/wiki/Logarithmic_scale
    /// </summary>
    public class LogarithmicAxis : AxisBase
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "LogarithmicAxis" /> class.
        /// </summary>
        public LogarithmicAxis()
        {
            PowerPadding = true;
            Base = 10;
            FilterMinValue = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis"/> class.
        /// </summary>
        /// <param name="pos">The pos.</param>
        /// <param name="title">The title.</param>
        public LogarithmicAxis(AxisPosition pos, string title)
            : this()
        {
            Position = pos;
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="title">The title.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public LogarithmicAxis(AxisPosition position, double minimum = double.NaN, double maximum = double.NaN,
                               string title = null)
            : this()
        {
            Position = position;
            Title = title;
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ActualMaximum and ActualMinimum values should be padded to the nearest power of the Base.
        /// </summary>
        public bool PowerPadding { get; set; }

        /// <summary>
        /// Gets or sets the logarithmic base (normally 10).
        /// http://en.wikipedia.org/wiki/Logarithm
        /// </summary>
        /// <value>The logarithmic base.</value>
        public double Base { get; set; }

        public override void UpdateActualMaxMin()
        {
            if (PowerPadding)
            {
                double logBase = Math.Log(Base);
                var e0 = (int)Math.Floor(Math.Log(ActualMinimum) / logBase);
                var e1 = (int)Math.Ceiling(Math.Log(ActualMaximum) / logBase);
                if (!double.IsNaN(ActualMinimum)) 
                    ActualMinimum = RemoveNoiseFromDoubleMath(Math.Exp(e0 * logBase));
                if (!double.IsNaN(ActualMaximum))
                    ActualMaximum = RemoveNoiseFromDoubleMath(Math.Exp(e1 * logBase));
            }

            base.UpdateActualMaxMin();
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(out ICollection<double> majorLabelValues, out ICollection<double> majorTickValues, out ICollection<double> minorTickValues)
        {
            if (ActualMinimum <= 0)
            {
                ActualMinimum = 0.1;
            }

            double logBase = Math.Log(Base);
            var e0 = (int)Math.Floor(Math.Log(ActualMinimum) / logBase);
            var e1 = (int)Math.Ceiling(Math.Log(ActualMaximum) / logBase);
            double d0 = RemoveNoiseFromDoubleMath(Math.Exp(e0 * logBase));
            double d1 = RemoveNoiseFromDoubleMath(Math.Exp(e1 * logBase));
            double d = d0;
            majorTickValues = new List<double>();
            minorTickValues = new List<double>();

            while (d <= d1 + double.Epsilon)
            {
                d = RemoveNoiseFromDoubleMath(d);
                if (d >= ActualMinimum && d <= ActualMaximum)
                {
                    majorTickValues.Add(d);
                }

                for (int i = 1; i < Base; i++)
                {
                    double d2 = d * (i + 1);
                    if (d2 > d1 + double.Epsilon)
                    {
                        break;
                    }

                    if (d2 > ActualMaximum)
                    {
                        break;
                    }

                    if (d2 >= ActualMinimum && d2 <= ActualMaximum)
                    {
                        minorTickValues.Add(d2);
                    }
                }

                d *= Base;
            }

            //if (majorValues.Count == 1)
            //{
            //    double split = majorValues.First();
            //    minorValues = CreateTickValues(ActualMinimum, split, ActualMinorStep);
            //    majorValues = CreateTickValues(ActualMinimum, split, ActualMajorStep);
            //    ICollection<double> minorValues2 = CreateTickValues(split, ActualMaximum, ActualMinorStep);
            //    ICollection<double> majorValues2 = CreateTickValues(split, ActualMaximum, ActualMajorStep);
            //    foreach (double v in minorValues2)
            //        if (v > split) minorValues.Add(v);
            //    foreach (double v in majorValues2)
            //        if (v > split) majorValues.Add(v);
            //    return;
            //}

            if (majorTickValues.Count < 2)
            {
                base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            }
            else
            {
                majorLabelValues = majorTickValues;
            }
        }

        protected override double PreTransform(double x)
        {
            if (x < 0)
            {
                return -1;
            }

            return Math.Log(x);
        }

        protected override double PostInverseTransform(double x)
        {
            return Math.Exp(x);
        }

        public override void Pan(double x0, double x1)
        {
            if (!IsPanEnabled)
                return;
            if (x1 == 0)
                return;
            double dx = x0 / x1;

            double newMinimum = ActualMinimum * dx;
            double newMaximum = ActualMaximum * dx;
            if (newMinimum < AbsoluteMinimum)
            {
                newMinimum = AbsoluteMinimum;
                newMaximum = newMinimum * ActualMaximum / ActualMinimum;
            }
            if (newMaximum > AbsoluteMaximum)
            {
                newMaximum = AbsoluteMaximum;
                newMinimum = newMaximum * ActualMaximum / ActualMinimum;
            }

            ViewMinimum = newMinimum;
            ViewMaximum = newMaximum;

            OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan));
        }

        public override void ZoomAt(double factor, double x)
        {
            if (!IsZoomEnabled)
                return;
            
            double px = PreTransform(x);
            double dx0 = PreTransform(ActualMinimum) - px;
            double dx1 = PreTransform(ActualMaximum) - px;
            double newViewMinimum = PostInverseTransform(dx0 / factor + px);
            double newViewMaximum = PostInverseTransform(dx1 / factor + px);
            
            ViewMinimum = Math.Max(newViewMinimum, AbsoluteMinimum);
            ViewMaximum = Math.Min(newViewMaximum, AbsoluteMaximum);
        }
    }
}