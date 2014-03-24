// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionAnnotation.cs" company="OxyPlot">
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
//   Represents a line annotation defined
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a line annotation defined
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
                double y = ActualMinimumY;

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

            return points.Select(p => this.Transform(p)).ToList();
        }
    }
}