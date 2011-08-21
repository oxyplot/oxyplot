// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicAxis.cs" company="OxyPlot">
//   See http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Logarithmic axis.
    /// http://en.wikipedia.org/wiki/Logarithmic_scale
    /// </summary>
    public class LogarithmicAxis : AxisBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LogarithmicAxis" /> class.
        /// </summary>
        public LogarithmicAxis()
        {
            this.PowerPadding = true;
            this.Base = 10;
            this.FilterMinValue = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis"/> class.
        /// </summary>
        /// <param name="pos">
        /// The pos.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public LogarithmicAxis(AxisPosition pos, string title)
            : this()
        {
            this.Position = pos;
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogarithmicAxis"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="minimum">
        /// The minimum.
        /// </param>
        /// <param name="maximum">
        /// The maximum.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public LogarithmicAxis(
            AxisPosition position, double minimum = double.NaN, double maximum = double.NaN, string title = null)
            : this()
        {
            this.Position = position;
            this.Title = title;
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the logarithmic base (normally 10).
        /// http://en.wikipedia.org/wiki/Logarithm
        /// </summary>
        /// <value>The logarithmic base.</value>
        public double Base { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ActualMaximum and ActualMinimum values should be padded to the nearest power of the Base.
        /// </summary>
        public bool PowerPadding { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">
        /// The major label values.
        /// </param>
        /// <param name="majorTickValues">
        /// The major tick values.
        /// </param>
        /// <param name="minorTickValues">
        /// The minor tick values.
        /// </param>
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

            while (d <= d1 + double.Epsilon)
            {
                // d = RemoveNoiseFromDoubleMath(d);
                if (d >= this.ActualMinimum && d <= this.ActualMaximum)
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
        /// Pans the axis.
        /// </summary>
        /// <param name="x0">
        /// The previous screen coordinate.
        /// </param>
        /// <param name="x1">
        /// The current screen coordinate.
        /// </param>
        public override void Pan(double x0, double x1)
        {
            if (!this.IsPanEnabled)
            {
                return;
            }

            if (x1 == 0)
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
                newMinimum = newMaximum * this.ActualMaximum / this.ActualMinimum;
            }

            this.ViewMinimum = newMinimum;
            this.ViewMaximum = newMaximum;

            this.OnAxisChanged(new AxisChangedEventArgs(AxisChangeTypes.Pan));
        }

        /// <summary>
        /// Transforms the specified coordinate to screen coordinates.
        /// This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The transformed value (screen coordinate).
        /// </returns>
        public override double Transform(double x)
        {
            Debug.Assert(x > 0, "X should be positive.");
            if (x < 0)
            {
                return -1;
            }

            return (Math.Log(x) - this.offset) * this.scale;
        }

        /// <summary>
        /// Updates the actual maximum and minimum values.
        /// If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum values will be used.
        /// If Maximum or Minimum have been set, these values will be used.
        /// Otherwise the maximum and minimum values of the series will be used, including the 'padding'.
        /// </summary>
        public override void UpdateActualMaxMin()
        {
            if (this.PowerPadding)
            {
                double logBase = Math.Log(this.Base);
                var e0 = (int)Math.Floor(Math.Log(this.ActualMinimum) / logBase);
                var e1 = (int)Math.Ceiling(Math.Log(this.ActualMaximum) / logBase);
                if (!double.IsNaN(this.ActualMinimum))
                {
                    this.ActualMinimum = RemoveNoiseFromDoubleMath(Math.Exp(e0 * logBase));
                }

                if (!double.IsNaN(this.ActualMaximum))
                {
                    this.ActualMaximum = RemoveNoiseFromDoubleMath(Math.Exp(e1 * logBase));
                }
            }

            base.UpdateActualMaxMin();
        }

        /// <summary>
        /// Zooms the axis at the specified coordinate.
        /// </summary>
        /// <param name="factor">
        /// The zoom factor.
        /// </param>
        /// <param name="x">
        /// The coordinate to zoom at.
        /// </param>
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
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the 'post inverse transform' to the value.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The transform value.
        /// </returns>
        protected override double PostInverseTransform(double x)
        {
            return Math.Exp(x);
        }

        /// <summary>
        /// "Pretransform" the value.
        /// This is used in logarithmic axis.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The pretransformed value.
        /// </returns>
        protected override double PreTransform(double x)
        {
            Debug.Assert(x > 0, "X should be positive.");

            if (x <= 0)
            {
                return 0;
            }

            return Math.Log(x);
        }

        #endregion
    }
}