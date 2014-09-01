// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an axis with logarithmic scale.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Represents an axis with logarithmic scale.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Logarithmic_scale.</remarks>
    public class LogarithmicAxis : Axis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "LogarithmicAxis" /> class.
        /// </summary>
        public LogarithmicAxis()
        {
            this.PowerPadding = true;
            this.Base = 10;
            this.FilterMinValue = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis" /> class.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="title">The title.</param>
        [Obsolete]
        public LogarithmicAxis(AxisPosition pos, string title)
            : this()
        {
            this.Position = pos;
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis" /> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="title">The title.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        [Obsolete]
        public LogarithmicAxis(AxisPosition position, string title = null, double minimum = double.NaN, double maximum = double.NaN)
            : this()
        {
            this.Position = position;
            this.Title = title;
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        /// <summary>
        /// Gets or sets the logarithmic base (normally 10).
        /// </summary>
        /// <value>The logarithmic base.</value>
        /// <remarks>See http://en.wikipedia.org/wiki/Logarithm.</remarks>
        public double Base { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ActualMaximum and ActualMinimum values should be padded to the nearest power of the Base.
        /// </summary>
        public bool PowerPadding { get; set; }

        /// <summary>
        /// Coerces the actual maximum and minimum values.
        /// </summary>
        public override void CoerceActualMaxMin()
        {
            if (double.IsNaN(this.ActualMinimum) || double.IsInfinity(this.ActualMinimum))
            {
                this.ActualMinimum = 1;
            }

            if (this.ActualMinimum <= 0)
            {
                this.ActualMinimum = 1;
            }

            if (this.ActualMaximum <= this.ActualMinimum)
            {
                this.ActualMaximum = this.ActualMinimum * 100;
            }

            base.CoerceActualMaxMin();
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(
            out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            if (this.ActualMinimum <= 0)
            {
                this.ActualMinimum = 0.1;
            }

            double logBase = Math.Log(this.Base);
            var e0 = (int)Math.Floor(Math.Log(this.ActualMinimum) / logBase);
            var e1 = (int)Math.Ceiling(Math.Log(this.ActualMaximum) / logBase);

            // find the min & max values for the specified base
            // round to max 10 digits
            double p0 = Math.Pow(this.Base, e0);
            double p1 = Math.Pow(this.Base, e1);
            double d0 = Math.Round(p0, 10);
            double d1 = Math.Round(p1, 10);
            if (d0 <= 0)
            {
                d0 = p0;
            }

            double d = d0;
            majorTickValues = new List<double>();
            minorTickValues = new List<double>();

            double epsMin = this.ActualMinimum * 1e-6;
            double epsMax = this.ActualMaximum * 1e-6;

            while (d <= d1 + epsMax)
            {
                // d = RemoveNoiseFromDoubleMath(d);
                if (d >= this.ActualMinimum - epsMin && d <= this.ActualMaximum + epsMax)
                {
                    majorTickValues.Add(d);
                }

                for (int i = 1; i < this.Base; i++)
                {
                    double d2 = d * (i + 1);
                    if (d2 > d1 + double.Epsilon)
                    {
                        break;
                    }

                    if (d2 > this.ActualMaximum)
                    {
                        break;
                    }

                    if (d2 >= this.ActualMinimum && d2 <= this.ActualMaximum)
                    {
                        minorTickValues.Add(d2);
                    }
                }

                d *= this.Base;
                if (double.IsInfinity(d))
                {
                    break;
                }

                if (d < double.Epsilon)
                {
                    break;
                }

                if (double.IsNaN(d))
                {
                    break;
                }
            }

            if (majorTickValues.Count < 2)
            {
                base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            }
            else
            {
                majorLabelValues = majorTickValues;
            }
        }

        /// <summary>
        /// Determines whether the axis is used for X/Y values.
        /// </summary>
        /// <returns><c>true</c> if it is an XY axis; otherwise, <c>false</c> .</returns>
        public override bool IsXyAxis()
        {
            return true;
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="ppt">The previous point (screen coordinates).</param>
        /// <param name="cpt">The current point (screen coordinates).</param>
        public override void Pan(ScreenPoint ppt, ScreenPoint cpt)
        {
            if (!this.IsPanEnabled)
            {
                return;
            }

            bool isHorizontal = this.IsHorizontal();

            double x0 = this.InverseTransform(isHorizontal ? ppt.X : ppt.Y);
            double x1 = this.InverseTransform(isHorizontal ? cpt.X : cpt.Y);

            if (Math.Abs(x1) < double.Epsilon)
            {
                return;
            }

            double dx = x0 / x1;

            double newMinimum = this.ActualMinimum * dx;
            double newMaximum = this.ActualMaximum * dx;
            if (newMinimum < this.AbsoluteMinimum)
            {
                newMinimum = this.AbsoluteMinimum;
                newMaximum = newMinimum * this.ActualMaximum / this.ActualMinimum;
            }

            if (newMaximum > this.AbsoluteMaximum)
            {
                newMaximum = this.AbsoluteMaximum;
                newMinimum = newMaximum * this.ActualMinimum / this.ActualMaximum;
            }

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan));
        }

        /// <summary>
        /// Inverse transforms the specified screen coordinate. This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="sx">The screen coordinate.</param>
        /// <returns>The value.</returns>
        public override double InverseTransform(double sx)
        {
            // Inline the <see cref="PostInverseTransform" /> method here.
            return Math.Exp((sx / this.Scale) + this.Offset);
        }

        /// <summary>
        /// Transforms the specified coordinate to screen coordinates.
        /// </summary>
        /// <param name="x">The value.</param>
        /// <returns>The transformed value (screen coordinate).</returns>
        public override double Transform(double x)
        {
            if (x <= 0)
            {
                return -1;
            }

            // Inline the <see cref="PreTransform" /> method here.
            return (Math.Log(x) - this.Offset) * this.Scale;
        }

        /// <summary>
        /// Zooms the axis at the specified coordinate.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        /// <param name="x">The coordinate to zoom at.</param>
        public override void ZoomAt(double factor, double x)
        {
            if (!this.IsZoomEnabled)
            {
                return;
            }

            double px = this.PreTransform(x);
            double dx0 = this.PreTransform(this.ActualMinimum) - px;
            double dx1 = this.PreTransform(this.ActualMaximum) - px;
            double newViewMinimum = this.PostInverseTransform((dx0 / factor) + px);
            double newViewMaximum = this.PostInverseTransform((dx1 / factor) + px);

            this.ViewMinimum = Math.Max(newViewMinimum, this.AbsoluteMinimum);
            this.ViewMaximum = Math.Min(newViewMaximum, this.AbsoluteMaximum);
            this.UpdateActualMaxMin();

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom));
        }

        /// <summary>
        /// Updates the <see cref="Axis.ActualMaximum" /> and <see cref="Axis.ActualMinimum" /> values.
        /// </summary>
        /// <remarks>
        /// If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum
        /// values will be used. If Maximum or Minimum have been set, these values will be used. Otherwise the maximum and minimum values
        /// of the series will be used, including the 'padding'.
        /// </remarks>
        internal override void UpdateActualMaxMin()
        {
            if (this.PowerPadding)
            {
                double logBase = Math.Log(this.Base);
                var e0 = (int)Math.Floor(Math.Log(this.ActualMinimum) / logBase);
                var e1 = (int)Math.Ceiling(Math.Log(this.ActualMaximum) / logBase);
                if (!double.IsNaN(this.ActualMinimum))
                {
                    this.ActualMinimum = Math.Round(Math.Exp(e0 * logBase), 14);
                }

                if (!double.IsNaN(this.ActualMaximum))
                {
                    this.ActualMaximum = Math.Round(Math.Exp(e1 * logBase), 14);
                }
            }

            base.UpdateActualMaxMin();
        }

        /// <summary>
        /// Applies a transformation after the inverse transform of the value. This is used in logarithmic axis.
        /// </summary>
        /// <param name="x">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override double PostInverseTransform(double x)
        {
            return Math.Exp(x);
        }

        /// <summary>
        /// Applies a transformation before the transform the value. This is used in logarithmic axis.
        /// </summary>
        /// <param name="x">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override double PreTransform(double x)
        {
            Debug.Assert(x > 0, "Value should be positive.");

            if (x <= 0)
            {
                return 0;
            }

            return Math.Log(x);
        }
    }
}