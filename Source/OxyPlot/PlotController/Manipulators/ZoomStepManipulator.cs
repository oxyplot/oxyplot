// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomStepManipulator.cs" company="OxyPlot">
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
//   Provides a plot control manipulator for stepwise zoom functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides a plot control manipulator for stepwise zoom functionality.
    /// </summary>
    public class ZoomStepManipulator : MouseManipulator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomStepManipulator" /> class.
        /// </summary>
        /// <param name="plotControl">The plot control.</param>
        public ZoomStepManipulator(IPlotView plotControl)
            : base(plotControl)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether FineControl.
        /// </summary>
        public bool FineControl { get; set; }

        /// <summary>
        /// Gets or sets Step.
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);

            var current = this.InverseTransform(e.Position.X, e.Position.Y);

            double scale = this.Step;
            if (this.FineControl)
            {
                scale *= 3;
            }

            scale = 1 + scale;

            // make sure the zoom factor is not negative
            if (scale < 0.1)
            {
                scale = 0.1;
            }

            if (this.XAxis != null)
            {
                this.XAxis.ZoomAt(scale, current.X);
            }

            if (this.YAxis != null)
            {
                this.YAxis.ZoomAt(scale, current.Y);
            }

            this.PlotControl.InvalidatePlot(false);
        }
    }
}