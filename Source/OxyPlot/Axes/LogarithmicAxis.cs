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
    using System.Linq;

    /// <summary>
    /// Represents an axis with logarithmic scale.
    /// </summary>
    /// <see href="http://en.wikipedia.org/wiki/Logarithmic_scale"/>
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
        /// <see href="http://en.wikipedia.org/wiki/Logarithm"/>
        public double Base { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ActualMaximum and ActualMinimum values should be padded to the nearest power of the Base.
        /// </summary>
        public bool PowerPadding { get; set; }

        /// <summary>
        /// Gets or sets the actual logarithmic maximum value of the axis.
        /// </summary>
        protected double LogActualMaximum { get; set; }

        /// <summary>
        /// Gets or sets the actual logarithmic minimum value of the axis.
        /// </summary>
        protected double LogActualMinimum { get; set; }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            // For easier readability, the nomenclature of this function and all related functions assumes a base of 10, and therefore uses the
            // term "decade". However, the code supports all other bases as well.
            var logBandwidth = Math.Abs(this.LogActualMaximum - this.LogActualMinimum);
            var axisBandwidth = Math.Abs(this.IsVertical() ? this.ScreenMax.Y - this.ScreenMin.Y : this.ScreenMax.X - this.ScreenMin.X);

            var desiredNumberOfTicks = axisBandwidth / this.IntervalLength;
            var ticksPerDecade = desiredNumberOfTicks / logBandwidth;
            var logDesiredStepSize = 1.0 / Math.Floor(ticksPerDecade);

            var intBase = Convert.ToInt32(this.Base);

            if (ticksPerDecade < 0.75)
            {   // Major Ticks every few decades (increase in powers of 2), up to eight minor tick subdivisions
                var decadesPerMajorTick = (int)Math.Pow(2, Math.Ceiling(Math.Log(1 / ticksPerDecade, 2)));
                majorTickValues = this.DecadeTicks(decadesPerMajorTick);
                minorTickValues = this.DecadeTicks(Math.Ceiling(decadesPerMajorTick / 8.0));
            }
            else if (Math.Abs(this.Base - intBase) > 1e-10)
            {   // fractional Base, best guess: naively subdivide decades
                majorTickValues = this.DecadeTicks(logDesiredStepSize);
                minorTickValues = this.DecadeTicks(0.5 * logDesiredStepSize);
            }
            else if (ticksPerDecade < 2)
            {   // Major Ticks at every decade, Minor Ticks at fractions (not for fractional base)
                majorTickValues = this.DecadeTicks();
                minorTickValues = this.SubdividedDecadeTicks();
            }
            else if (ticksPerDecade > this.Base * 1.5)
            {   // Fall back to linearly distributed tick values
                base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            }
            else
            {
                // use subdivided decades as major candidates
                var logMajorCandidates = this.LogSubdividedDecadeTicks(false);

                if (logMajorCandidates.Count < 2)
                {   // this should usually not be the case, but if for some reason we should happen to have too few candidates, fall back to linear ticks
                    base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
                    return;
                }

                // check for large candidate intervals; if there are any, subdivide with minor ticks
                var logMinorCandidates = this.LogCalculateMinorCandidates(logMajorCandidates, logDesiredStepSize);

                // use all minor tick candidates that are in the axis range
                minorTickValues = this.PowList(logMinorCandidates, true);

                // find suitable candidates for every desired major step
                majorTickValues = this.AlignTicksToCandidates(logMajorCandidates, logDesiredStepSize);
            }

            majorLabelValues = majorTickValues;
            minorTickValues = AxisUtilities.FilterRedundantMinorTicks(majorTickValues, minorTickValues);
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

            var isHorizontal = this.IsHorizontal();

            var x0 = this.InverseTransform(isHorizontal ? ppt.X : ppt.Y);
            var x1 = this.InverseTransform(isHorizontal ? cpt.X : cpt.Y);

            if (Math.Abs(x1) < double.Epsilon)
            {
                return;
            }

            var oldMinimum = this.ActualMinimum;
            var oldMaximum = this.ActualMaximum;

            var dx = x0 / x1;

            var newMinimum = this.ActualMinimum * dx;
            var newMaximum = this.ActualMaximum * dx;
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

            var px = this.PreTransform(x);
            var dx0 = this.PreTransform(this.ActualMinimum) - px;
            var dx1 = this.PreTransform(this.ActualMaximum) - px;
            var newViewMinimum = this.PostInverseTransform((dx0 / factor) + px);
            var newViewMaximum = this.PostInverseTransform((dx1 / factor) + px);

            var newMinimum = Math.Max(newViewMinimum, this.AbsoluteMinimum);
            var newMaximum = Math.Min(newViewMaximum, this.AbsoluteMaximum);

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;
            this.UpdateActualMaxMin();

            var deltaMinimum = this.ActualMinimum - oldMinimum;
            var deltaMaximum = this.ActualMaximum - oldMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Zoom, deltaMinimum, deltaMaximum));
        }

        /// <summary>
        /// Raises all elements of a List to the power of <c>this.Base</c>.
        /// </summary>
        /// <param name="logInput">The input values.</param>
        /// <param name="clip">If true, discards all values that are not in the axis range.</param>
        /// <returns>A new IList containing the resulting values.</returns>
        internal IList<double> PowList(IList<double> logInput, bool clip = false)
        {
            return
                logInput.Where(item => !clip || !(item < this.LogActualMinimum))
                    .TakeWhile(item => !clip || !(item > this.LogActualMaximum))
                    .Select(item => Math.Pow(this.Base, item))
                    .ToList();
        }

        /// <summary>
        /// Applies the logarithm with <c>this.Base</c> to all elements of a List.
        /// </summary>
        /// <param name="input">The input values.</param>
        /// <param name="clip">If true, discards all values that are not in the axis range.</param>
        /// <returns>A new IList containing the resulting values.</returns>
        internal IList<double> LogList(IList<double> input, bool clip = false)
        {
            return
                input.Where(item => !clip || !(item < this.ActualMinimum))
                    .TakeWhile(item => !clip || !(item > this.ActualMaximum))
                    .Select(item => Math.Log(item, this.Base))
                    .ToList();
        }

        /// <summary>
        /// Calculates ticks of the decades in the axis range with a specified step size.
        /// </summary>
        /// <param name="step">The step size.</param>
        /// <returns>A new IList containing the decade ticks.</returns>
        internal IList<double> DecadeTicks(double step = 1)
        {
            return this.PowList(this.LogDecadeTicks(step));
        }

        /// <summary>
        /// Calculates logarithmic ticks of the decades in the axis range with a specified step size.
        /// </summary>
        /// <param name="step">The step size.</param>
        /// <returns>A new IList containing the logarithmic decade ticks.</returns>
        internal IList<double> LogDecadeTicks(double step = 1)
        {
            var ret = new List<double>();
            if (step > 0)
            {
                var last = double.NaN;
                for (var exponent = Math.Ceiling(this.LogActualMinimum); exponent <= this.LogActualMaximum; exponent += step)
                {
                    if (exponent <= last)
                    {
                        break;
                    }

                    last = exponent;
                    if (exponent >= this.LogActualMinimum)
                    {
                        ret.Add(exponent);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Calculates logarithmic ticks of all decades in the axis range and their subdivisions.
        /// </summary>
        /// <param name="clip">If true (default), the lowest and highest decade are clipped to the axis range.</param>
        /// <returns>A new IList containing the logarithmic decade ticks.</returns>
        internal IList<double> LogSubdividedDecadeTicks(bool clip = true)
        {
            return this.LogList(this.SubdividedDecadeTicks(clip));
        }

        /// <summary>
        /// Calculates ticks of all decades in the axis range and their subdivisions.
        /// </summary>
        /// <param name="clip">If true (default), the lowest and highest decade are clipped to the axis range.</param>
        /// <returns>A new IList containing the decade ticks.</returns>
        internal IList<double> SubdividedDecadeTicks(bool clip = true)
        {
            var ret = new List<double>();
            for (var exponent = (int)Math.Floor(this.LogActualMinimum);; exponent++)
            {
                if (exponent > this.LogActualMaximum)
                {
                    break;
                }

                var currentDecade = Math.Pow(this.Base, exponent);
                for (var mantissa = 1; mantissa < this.Base; mantissa++)
                {
                    var currentValue = currentDecade * mantissa;
                    if (clip && currentValue < this.ActualMinimum)
                    {
                        continue;
                    }

                    if (clip && currentValue > this.ActualMaximum)
                    {
                        break;
                    }

                    ret.Add(currentDecade * mantissa);
                }
            }

            return ret;
        }

        /// <summary>
        /// Chooses from a list of candidates so that the resulting List matches the <paramref name="logDesiredStepSize"/> as far as possible.
        /// </summary>
        /// <param name="logCandidates">The candidates.</param>
        /// <param name="logDesiredStepSize">The desired logarithmic step size.</param>
        /// <returns>A new IList containing the chosen candidates.</returns>
        internal IList<double> AlignTicksToCandidates(IList<double> logCandidates, double logDesiredStepSize)
        {
            return this.PowList(this.LogAlignTicksToCandidates(logCandidates, logDesiredStepSize));
        }

        /// <summary>
        /// Chooses from a list of candidates so that the resulting List matches the <paramref name="logDesiredStepSize"/> as far as possible.
        /// </summary>
        /// <param name="logCandidates">The candidates.</param>
        /// <param name="logDesiredStepSize">The desired logarithmic step size.</param>
        /// <returns>A new IList containing the chosen logarithmic candidates.</returns>
        internal IList<double> LogAlignTicksToCandidates(IList<double> logCandidates, double logDesiredStepSize)
        {
            var ret = new List<double>();

            var candidateOffset = 1;
            var logPreviousMajorTick = double.NaN;

            // loop through all desired steps and find a suitable candidate for each of them
            for (var d = Math.Floor(this.LogActualMinimum);; d += logDesiredStepSize)
            {
                if (d < this.LogActualMinimum - logDesiredStepSize)
                {
                    continue;
                }

                if (d > (this.LogActualMaximum + logDesiredStepSize))
                {
                    break;
                }

                // find closest candidate 
                while (candidateOffset < logCandidates.Count - 1 && logCandidates[candidateOffset] < d)
                {
                    candidateOffset++;
                }

                var logNewMajorTick =
                    Math.Abs(logCandidates[candidateOffset] - d) < Math.Abs(logCandidates[candidateOffset - 1] - d) ?
                    logCandidates[candidateOffset] :
                    logCandidates[candidateOffset - 1];

                // don't add duplicates
                if ((logNewMajorTick != logPreviousMajorTick) && (logNewMajorTick >= this.LogActualMinimum) && (logNewMajorTick <= this.LogActualMaximum))
                {
                    ret.Add(logNewMajorTick);
                }

                logPreviousMajorTick = logNewMajorTick;
            }

            return ret;
        }

        /// <summary>
        /// Calculates minor tick candidates for a given set of major candidates.
        /// </summary>
        /// <param name="logMajorCandidates">The major candidates.</param>
        /// <param name="logDesiredMajorStepSize">The desired major step size.</param>
        /// <returns>A new IList containing the minor candidates.</returns>
        internal IList<double> LogCalculateMinorCandidates(IList<double> logMajorCandidates, double logDesiredMajorStepSize)
        {
            var ret = new List<double>();

            for (var c = 1; c < logMajorCandidates.Count; c++)
            {
                var previous = logMajorCandidates[c - 1];
                var current = logMajorCandidates[c];

                if (current < this.LogActualMinimum)
                {
                    continue;
                }

                if (previous > this.LogActualMaximum)
                {
                    break;
                }

                var stepSizeRatio = (current - previous) / logDesiredMajorStepSize;
                if (stepSizeRatio > 2)
                {   // Step size is too large... subdivide with minor ticks
                    this.LogSubdivideInterval(ret, this.Base, previous, current);
                }

                ret.Add(current);
            }

            return ret;
        }

        /// <summary>
        /// Subdivides a logarithmic range into multiple, evenly-spaced (in linear scale!) ticks. The number of ticks and the tick intervals are adapted so 
        /// that the resulting steps are "nice" numbers.
        /// </summary>
        /// <param name="logTicks">The IList the computed steps will be added to.</param>
        /// <param name="steps">The minimum number of steps.</param>
        /// <param name="logFrom">The start of the range.</param>
        /// <param name="logTo">The end of the range.</param>
        internal void LogSubdivideInterval(IList<double> logTicks, double steps, double logFrom, double logTo)
        {
            var actualNumberOfSteps = 1;
            var intBase = Convert.ToInt32(this.Base);

            // first, determine actual number of steps that gives a "nice" step size
            if (steps < 2)
            {
                // No Subdivision
                return;
            }

            if (Math.Abs(this.Base - intBase) > this.Base * 1e-10)
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

            var from = Math.Pow(this.Base, logFrom);
            var to = Math.Pow(this.Base, logTo);

            // subdivide with the actual number of steps
            for (var c = 1; c < actualNumberOfSteps; c++)
            {
                var newTick = (double)c / actualNumberOfSteps;
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
                var logBase = Math.Log(this.Base);
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

            if (this.ActualMinimum <= 0)
            {
                this.ActualMinimum = 0.1;
            }

            this.LogActualMinimum = Math.Log(this.ActualMinimum, this.Base);
            this.LogActualMaximum = Math.Log(this.ActualMaximum, this.Base);
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

            return x <= 0 ? 0 : Math.Log(x);
        }

        /// <summary>
        /// Coerces the actual maximum and minimum values.
        /// </summary>
        protected override void CoerceActualMaxMin()
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
    }
}
