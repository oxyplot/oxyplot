using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OxyPlot.Axes
{
    /// <summary>
    /// Creates a Gumbel (Extreme Value Type I) probability scaled axis.
    /// </summary>
    public class GumbelProbabilityAxis : Axis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "GumbelProbabilityAxis" /> class.
        /// </summary>
        public GumbelProbabilityAxis(){}

        private double _epsilon = 1E-16;

        /// <summary>
        /// Gets and sets the maximum axis value.
        /// </summary>
        public new double Maximum
        {
            get
            {
                return base.Maximum;
            }
            set
            {
                if (double.IsNaN(value))
                {
                    base.Maximum = 0.999;
                }
                else if (value > 0.0 && value < 1.0)
                {
                    base.Maximum = value;
                }
                else
                {
                    base.Maximum = 0.999;
                }
            }
        }

        /// <summary>
        /// Gets and sets the minimum axis value.
        /// </summary>
        public new double Minimum
        {
            get
            {
                return base.Minimum;
            }
            set
            {
                if (double.IsNaN(value))
                {
                    base.Minimum = 0.0000001;
                }
                else if (value > 0.0 && value < 1.0)
                {
                    base.Minimum = value;
                }
                else
                {
                    base.Minimum = _epsilon;
                }
            }
        }

        /// <summary>
        /// The standard normal distribution used to create the scale. 
        /// </summary>
        private GumbelDistribution Gumbel = new GumbelDistribution();

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {

            var major = new List<double>
            {
                0.9,
                0.5,
                0.1
            };
            var minor = new List<double>
            {
                0.2,
                0.3,
                0.4,
                0.6,
                0.7,
                0.8,
            };

            if (ActualMinimum == ActualMaximum)
            {
                majorTickValues = major;
                minorTickValues = minor;
                majorLabelValues = major;
                return;
            }

            // Add small majors and minors
            int exp = 1;
            do
            {
                major.Add(0.1 / Math.Pow(10, exp));
                for (int i = 2; i <= 9; i++)
                    minor.Add(major.Last() * i);
                if (major.Last() <= ActualMinimum)
                    break;
                exp += 1;
            }
            while (true);
            // Add big majors and minors
            exp = 1;
            do
            {
                major.Add(1 - 0.1 / Math.Pow(10, exp));
                for (int i = 2; i <= 9; i++)
                    minor.Add(1 - 0.1 / Math.Pow(10, exp) * i);
                if (major.Last() >= ActualMaximum)
                    break;
                exp += 1;
            }
            while (true);

            majorTickValues = major.Where(o => o <= ActualMaximum && o >= ActualMinimum).ToList();
            minorTickValues = minor.Where(o => o <= ActualMaximum && o >= ActualMinimum).ToList();
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
            return false;
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
            return 1-Gumbel.CDF((sx / this.Scale) + this.Offset);
        }

        /// <summary>
        /// Transforms the specified coordinate to screen coordinates.
        /// </summary>
        /// <param name="x">The value.</param>
        /// <returns>The transformed value (screen coordinate).</returns>
        public override double Transform(double x)
        {
            if (double.IsNaN(x)) return double.NaN;
            if (x < _epsilon) x = _epsilon;
            if (x > 1 - _epsilon) x = 1 - _epsilon;
            // Inline the <see cref="PreTransform" /> method here.
            return (Gumbel.InverseCDF(1-x) - this.Offset) * this.Scale;
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
        /// Updates the <see cref="Axis.ActualMaximum" /> and <see cref="Axis.ActualMinimum" /> values.
        /// </summary>
        /// <remarks>
        /// If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum
        /// values will be used. If Maximum or Minimum have been set, these values will be used. Otherwise the maximum and minimum values
        /// of the series will be used, including the 'padding'.
        /// </remarks>
        internal override void UpdateActualMaxMin()
        {

            if (!double.IsNaN(this.ActualMinimum) || this.ActualMinimum <= _epsilon)
            {
                this.ActualMinimum = _epsilon;
            }

            if (this.ActualMinimum < _epsilon)
            {
                this.ActualMinimum = _epsilon;
            }

            if (!double.IsNaN(this.ActualMaximum) || this.ActualMaximum >= 0.999)
            {
                this.ActualMaximum = 0.999;
            }

            if (this.ActualMaximum <= this.ActualMinimum)
            {
                this.ActualMinimum = _epsilon;
                this.ActualMaximum = 0.999;
            }

            base.UpdateActualMaxMin();

        }

        /// <summary>
        /// Applies a transformation after the inverse transform of the value.
        /// </summary>
        /// <param name="x">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override double PostInverseTransform(double x)
        {
            return 1-Gumbel.CDF(x);
        }

        /// <summary>
        /// Applies a transformation before the transform the value.
        /// </summary>
        /// <param name="x">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override double PreTransform(double x)
        {
            if (double.IsNaN(x)) return double.NaN;
            if (x < _epsilon) x = _epsilon;
            if (x > 1 - _epsilon) x = 1 - _epsilon;
            return Gumbel.InverseCDF(1-x);
        }

        /// <summary>
        /// Coerces the actual maximum and minimum values.
        /// </summary>
        protected override void CoerceActualMaxMin()
        {
            if (double.IsNaN(this.ActualMinimum) || double.IsInfinity(this.ActualMinimum))
            {
                this.ActualMinimum = _epsilon;
            }

            if (double.IsNaN(this.ActualMaximum) || double.IsInfinity(this.ActualMaximum))
            {
                this.ActualMaximum = 0.999;
            }

            if (this.ActualMinimum < _epsilon)
            {
                this.ActualMinimum = _epsilon;
            }

            if (this.ActualMaximum <= this.ActualMinimum)
            {
                this.ActualMinimum = _epsilon;
                this.ActualMaximum = 0.999;
            }

            base.CoerceActualMaxMin();
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value to format.</param>
        /// <returns>The formatted value.</returns>
        protected override string FormatValueOverride(double x)
        {
            if (x < 0.001 || x > 0.999)
            { return x.ToString("0.###E+0", CultureInfo.InvariantCulture); }
            else if (x >= 0.001 || x <= 0.999)
            { return x.ToString("0.######", CultureInfo.InvariantCulture); }
            else
            { return x.ToString(); }
        }

    }
}
