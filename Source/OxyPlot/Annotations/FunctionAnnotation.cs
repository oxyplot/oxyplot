// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an annotation that shows a function rendered as a path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an annotation that shows a function rendered as a path.
    /// </summary>
    public class FunctionAnnotation : PathAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAnnotation" /> class.
        /// </summary>
        public FunctionAnnotation()
        {
            this.Resolution = 400;
            this.Type = FunctionAnnotationType.EquationX;
        }

        /// <summary>
        /// Gets or sets the type of function. Can be either f(x) or f(y).
        /// </summary>
        /// <value>The type of the function.</value>
        public FunctionAnnotationType Type { get; set; }

        /// <summary>
        /// Gets or sets the y=f(x) equation when Type is Equation.
        /// </summary>
        public Func<double, double> Equation { get; set; }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public int Resolution { get; set; }

        /// <summary>
        /// Gets the screen points.
        /// </summary>
        /// <returns>The list of screen points defined by this function annotation.</returns>
        protected override IList<ScreenPoint> GetScreenPoints()
        {
            Func<double, double> fx = null;

            Func<double, double> fy = null;

            switch (this.Type)
            {
                case FunctionAnnotationType.EquationX:
                    fx = this.Equation;
                    break;
                case FunctionAnnotationType.EquationY:
                    fy = this.Equation;
                    break;
            }

            var points = new List<DataPoint>();

            if (fx != null)
            {
                double x = this.ActualMinimumX;

                // todo: the step size should be adaptive
                double dx = (this.ActualMaximumX - this.ActualMinimumX) / this.Resolution;
                while (true)
                {
                    points.Add(new DataPoint(x, fx(x)));
                    if (x > this.ActualMaximumX)
                    {
                        break;
                    }

                    x += dx;
                }
            }
            else if (fy != null)
            {
                double y = this.ActualMinimumY;

                // todo: the step size should be adaptive
                double dy = (this.ActualMaximumY - this.ActualMinimumY) / this.Resolution;
                while (true)
                {
                    points.Add(new DataPoint(fy(y), y));
                    if (y > this.ActualMaximumY)
                    {
                        break;
                    }

                    y += dy;
                }
            }

            return points.Select(this.Transform).ToList();
        }
    }
}