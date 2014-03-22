// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisRendererBase.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides an abstract base class for axis renderers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides an abstract base class for axis renderers.
    /// </summary>
    public abstract class AxisRendererBase
    {
        /// <summary>
        /// The plot.
        /// </summary>
        private readonly PlotModel plot;

        /// <summary>
        /// The render context.
        /// </summary>
        private readonly IRenderContext rc;

        /// <summary>
        /// The major label values
        /// </summary>
        private IList<double> majorLabelValues;

        /// <summary>
        /// The major tick values
        /// </summary>
        private IList<double> majorTickValues;

        /// <summary>
        /// The minor tick values
        /// </summary>
        private IList<double> minorTickValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisRendererBase"/> class.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="plot">
        /// The plot.
        /// </param>
        protected AxisRendererBase(IRenderContext rc, PlotModel plot)
        {
            this.plot = plot;
            this.rc = rc;
        }

        /// <summary>
        /// Gets the plot.
        /// </summary>
        /// <value>
        /// The plot.
        /// </value>
        protected PlotModel Plot
        {
            get
            {
                return this.plot;
            }
        }

        /// <summary>
        /// Gets the render context.
        /// </summary>
        /// <value>
        /// The render context.
        /// </value>
        protected IRenderContext RenderContext
        {
            get
            {
                return this.rc;
            }
        }

        /// <summary>
        /// Gets or sets the axis lines pen.
        /// </summary>
        protected OxyPen AxislinePen { get; set; }

        /// <summary>
        /// Gets or sets the extra grid lines pen.
        /// </summary>
        protected OxyPen ExtraPen { get; set; }

        /// <summary>
        /// Gets or sets the major label values.
        /// </summary>
        protected IList<double> MajorLabelValues
        {
            get
            {
                return this.majorLabelValues;
            }

            set
            {
                this.majorLabelValues = value;
            }
        }

        /// <summary>
        /// Gets or sets the major grid lines pen.
        /// </summary>
        protected OxyPen MajorPen { get; set; }

        /// <summary>
        /// Gets or sets the major tick pen.
        /// </summary>
        protected OxyPen MajorTickPen { get; set; }

        /// <summary>
        /// Gets or sets the major tick values.
        /// </summary>
        protected IList<double> MajorTickValues
        {
            get
            {
                return this.majorTickValues;
            }

            set
            {
                this.majorTickValues = value;
            }
        }

        /// <summary>
        /// Gets or sets the minor grid lines pen.
        /// </summary>
        protected OxyPen MinorPen { get; set; }

        /// <summary>
        /// Gets or sets the minor tick pen.
        /// </summary>
        protected OxyPen MinorTickPen { get; set; }

        /// <summary>
        /// Gets or sets the minor tick values.
        /// </summary>
        protected IList<double> MinorTickValues
        {
            get
            {
                return this.minorTickValues;
            }

            set
            {
                this.minorTickValues = value;
            }
        }

        /// <summary>
        /// Gets or sets the zero grid line pen.
        /// </summary>
        protected OxyPen ZeroPen { get; set; }

        /// <summary>
        /// Renders the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="pass">The pass.</param>
        public virtual void Render(Axis axis, int pass)
        {
            if (axis == null)
            {
                return;
            }

            axis.GetTickValues(out this.majorLabelValues, out this.majorTickValues, out this.minorTickValues);
            this.CreatePens(axis);
        }

        /// <summary>
        /// Creates the pens.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        protected virtual void CreatePens(Axis axis)
        {
            this.MinorPen = OxyPen.Create(axis.MinorGridlineColor, axis.MinorGridlineThickness, axis.MinorGridlineStyle);
            this.MajorPen = OxyPen.Create(axis.MajorGridlineColor, axis.MajorGridlineThickness, axis.MajorGridlineStyle);
            this.MinorTickPen = OxyPen.Create(axis.TicklineColor, axis.MinorGridlineThickness);
            this.MajorTickPen = OxyPen.Create(axis.TicklineColor, axis.MajorGridlineThickness);
            this.ZeroPen = OxyPen.Create(axis.TicklineColor, axis.MajorGridlineThickness);
            this.ExtraPen = OxyPen.Create(axis.ExtraGridlineColor, axis.ExtraGridlineThickness, axis.ExtraGridlineStyle);
            this.AxislinePen = OxyPen.Create(axis.AxislineColor, axis.AxislineThickness, axis.AxislineStyle);
        }

        /// <summary>
        /// Gets the tick positions.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="tickStyle">
        /// The tick style.
        /// </param>
        /// <param name="tickSize">
        /// The tick size.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="x0">
        /// The x 0.
        /// </param>
        /// <param name="x1">
        /// The x 1.
        /// </param>
        protected virtual void GetTickPositions(Axis axis, TickStyle tickStyle, double tickSize, AxisPosition position, out double x0, out double x1)
        {
            x0 = 0;
            x1 = 0;
            bool isTopOrLeft = position == AxisPosition.Top || position == AxisPosition.Left;
            double sign = isTopOrLeft ? -1 : 1;
            switch (tickStyle)
            {
                case TickStyle.Crossing:
                    x0 = -tickSize * sign * 0.75;
                    x1 = tickSize * sign * 0.75;
                    break;
                case TickStyle.Inside:
                    x0 = -tickSize * sign;
                    break;
                case TickStyle.Outside:
                    x1 = tickSize * sign;
                    break;
            }
        }

        /// <summary>
        /// Determines whether the specified value is within the specified range.
        /// </summary>
        /// <param name="d">The value to check.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <returns>
        ///  <c>true</c> if the specified value is within the range; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsWithin(double d, double min, double max)
        {
            if (d < min)
            {
                return false;
            }

            if (d > max)
            {
                return false;
            }

            return true;
        }
    }
}