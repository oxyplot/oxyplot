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

            majorTickValues = new List<double>();
            minorTickValues = new List<double>();

            double logMinimum = Math.Log(this.ActualMinimum, this.Base);
            double logMaximum = Math.Log(this.ActualMaximum, this.Base);

            int logSmallestDecade = (int)Math.Floor(logMinimum);
            int logLargestDecade = (int)Math.Ceiling(logMaximum);
            double smallestDecade = Math.Pow(this.Base, logSmallestDecade);
            double largestDecade = Math.Pow(this.Base, logLargestDecade);

            double logBandwidth = logMaximum - logMinimum;
            double axisBandwidth = this.IsVertical() ? this.ScreenMax.Y - this.ScreenMin.Y : this.ScreenMax.X - this.ScreenMin.X;
            double logAxisBandwidth = Math.Log(axisBandwidth, this.Base);

            double desiredNumberOfTicks = axisBandwidth / this.IntervalLength;
            double ticksPerDecade = desiredNumberOfTicks / logBandwidth;
            double desiredStepSize = 1.0 / Convert.ToInt32(ticksPerDecade);

            int intBase = Convert.ToInt32(this.Base);
            
            if (ticksPerDecade < 0.75)
            {   // Minor Ticks at every decade, Major Ticks every few decades
                int decadesPerMajorTick = Math.Max(2, Convert.ToInt32(1 / ticksPerDecade));
                for (int c = logSmallestDecade; true; c++)
                {
                    if (c < logMinimum)
                    {
                        continue;
                    }

                    if (c > logMaximum)
                    {
                        break;
                    }

                    double currentValue = Math.Pow(this.Base, c);
                    minorTickValues.Add(currentValue);

                    if (c % decadesPerMajorTick == 0)
                    {
                        majorTickValues.Add(currentValue);
                    }
                }
            }
            else if (Math.Abs(this.Base - intBase) > 1e-10)
            {   // fractional Base, best guess: naively subdivide decades
                for (double c = logSmallestDecade; true; c += desiredStepSize)
                {
                    if (c < logMinimum)
                    {
                        continue;
                    }

                    if (c > logMaximum)
                    {
                        break;
                    }

                    majorTickValues.Add(Math.Pow(this.Base, c));
                    minorTickValues.Add(Math.Pow(this.Base, c));

                    if (c + (0.5 * desiredStepSize) < logMaximum)
                    {
                        minorTickValues.Add(Math.Pow(this.Base, c + (0.5 * desiredStepSize)));
                    }
                }
            }
            else if (ticksPerDecade < 2)
            {   // Major Ticks at every decade, Minor Ticks at fractions (not for fractional base)
                for (int c = logSmallestDecade; true; c++)
                {
                    if (c < logMinimum)
                    {
                        continue;
                    }

                    if (c > logMaximum)
                    {
                        break;
                    }

                    double currentValue = Math.Pow(this.Base, c);
                    minorTickValues.Add(currentValue);
                    majorTickValues.Add(currentValue);

                    for (int c1 = 2; c1 < this.Base; c1++)
                    {
                        minorTickValues.Add(currentValue * c1);
                    }
                }
            }
            else if (ticksPerDecade > this.Base * 1000)
            {   // Fall back to linearly distributed tick values
                base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            }
            else
            {   // integer Base, subdivide decades

                List<double> logMajorCandidates = new List<double>();
                List<double> logMinorCandidates = new List<double>();
                
                // loop through all visible decades
                for (int c = logSmallestDecade; c <= logLargestDecade; c++)
                {
                    double logPreviousCandidate = c;

                    // loop through the subdivisions for the current decade
                    for (int c1 = 1; true; c1++)
                    {
                        double logCurrentCandidate = c + Math.Log(c1, intBase);

                        if (logPreviousCandidate > logMaximum)
                        {   // we are done with the last decade
                            break;
                        }

                        if (logCurrentCandidate > logMinimum)
                        {
                            if (logCurrentCandidate > c)
                            {   // this is not the first candidate in the current decade
                                double stepRatio = (logCurrentCandidate - logPreviousCandidate) / desiredStepSize;
                                if (stepRatio > 2)
                                {   // Step size is too large... subdivide with minor ticks
                                    this.LogSubdivide(logMinorCandidates, this.Base, logPreviousCandidate, logCurrentCandidate);
                                }
                            }

                            if (c1 == intBase)
                            {   // we are done with the current decade
                                break;
                            }

                            // add the subdivision as possible candidates for minor and major ticks
                            logMajorCandidates.Add(logCurrentCandidate);
                            logMinorCandidates.Add(logCurrentCandidate);
                        }

                        logPreviousCandidate = logCurrentCandidate;
                    }
                }

                if (logMajorCandidates.Count <= 1)
                {   // should not happen normally, but if we should happen to have too few candidates, fall back to linear ticks
                    base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
                    return;
                }

                int candidateOffset = 0;
                double previousMajorTick = double.NaN;

                // loop through the minor tick candidates and add them to the minorTickValues List
                foreach (double item in logMinorCandidates)
                {
                    if (item < logMinimum)
                    {
                        continue;
                    }

                    if (item > logMaximum)
                    {
                        break;
                    }

                    minorTickValues.Add(Math.Pow(this.Base, item));
                }

                // loop through all desired steps and find a suitable candidate
                for (double d = logSmallestDecade; true; d += desiredStepSize)
                {
                    if (d < logMinimum)
                    {
                        continue;
                    }

                    if (d > logMaximum)
                    {
                        break;
                    }

                    // find closest candidate and add it to MajorTicks
                    while (candidateOffset < logMajorCandidates.Count - 1 && logMajorCandidates[candidateOffset] < d)
                    {
                        candidateOffset++;
                    }

                    candidateOffset = Math.Max(candidateOffset, 1);
                    double logNewMajorTick = 
                        Math.Abs(logMajorCandidates[candidateOffset] - d) < Math.Abs(logMajorCandidates[candidateOffset - 1] - d) ?
                        logMajorCandidates[candidateOffset] :
                        logMajorCandidates[candidateOffset - 1];

                    // calculate the new tick
                    double newMajorTick = Math.Pow(this.Base, logNewMajorTick);

                    if (newMajorTick != previousMajorTick)
                    {
                        majorTickValues.Add(newMajorTick);
                    }

                    previousMajorTick = newMajorTick;
                }
            }

            majorLabelValues = majorTickValues;
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
        /// Determines whether the axis is logarithmic.
        /// </summary>
        /// <returns><c>true</c> if it is a logarithmic axis; otherwise, <c>false</c> .</returns>
        public override bool IsLogarithmic()
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

            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

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

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan, deltaMinimum, deltaMaximum));
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

            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

            double px = this.PreTransform(x);
            double dx0 = this.PreTransform(this.ActualMinimum) - px;
            double dx1 = this.PreTransform(this.ActualMaximum) - px;
            double newViewMinimum = this.PostInverseTransform((dx0 / factor) + px);
            double newViewMaximum = this.PostInverseTransform((dx1 / factor) + px);

            double newMinimum = Math.Max(newViewMinimum, this.AbsoluteMinimum);
            double newMaximum = Math.Min(newViewMaximum, this.AbsoluteMaximum);

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;
            this.UpdateActualMaxMin();

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, deltaMinimum, deltaMaximum));
        }

        /// <summary>
        /// Subdivides a logarithmic range into multiple, evenly-spaced (in linear space!) ticks. The number of ticks and the tick intervals are adapted so 
        /// that the resulting steps are "nice" numbers.
        /// </summary>
        /// <param name="logTicks">The IList the computed steps are added to.</param>
        /// <param name="steps">The minimum number of steps.</param>
        /// <param name="logFrom">The start of the range.</param>
        /// <param name="logTo">The end of the range.</param>
        internal void LogSubdivide(IList<double> logTicks, double steps, double logFrom, double logTo)
        {
            int actualNumberOfSteps = 1;
            int intBase = Convert.ToInt32(this.Base);

            if (steps < 2)
            {   // No Subdivision
                return;
            }
            else if (Math.Abs(this.Base - intBase) > this.Base * 1e-10)
            {   // fractional Base; just make a linear subdivision
                actualNumberOfSteps = Convert.ToInt32(steps);
            }
            else if ((intBase & (intBase - 1)) == 0)
            {   // base is a power of 2; use a power of 2 for the stepsize
                while (actualNumberOfSteps < steps)
                {
                    actualNumberOfSteps *= 2;
                }
            }
            else
            {   // integer base, no power of two

                // for bases != 10, first subdivide by the base
                if (intBase != 10)
                {
                    actualNumberOfSteps = intBase;
                }

                // follow 1-2-5-10 pattern
                while (true)
                {   
                    if (actualNumberOfSteps >= steps)
                    {
                        break;
                    }

                    actualNumberOfSteps *= 2;
                    
                    if (actualNumberOfSteps >= steps)
                    {   
                        break;
                    }

                    actualNumberOfSteps = Convert.ToInt32(actualNumberOfSteps * 2.5);
                   
                    if (actualNumberOfSteps >= steps)
                    {
                        break;
                    }

                    actualNumberOfSteps *= 2;
                }
            }

            double from = Math.Pow(this.Base, logFrom);
            double to = Math.Pow(this.Base, logTo);

            // subdivide with the actual number of steps
            for (int c = 1; c < actualNumberOfSteps; c++)
            {
                double newTick = (double)c / actualNumberOfSteps;
                newTick = Math.Log(from + ((to - from) * newTick), this.Base);

                logTicks.Add(newTick);
            }
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