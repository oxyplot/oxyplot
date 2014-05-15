// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TouchManipulator.cs" company="OxyPlot">
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
//   Provides a manipulator for panning and scaling by touch events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides a manipulator for panning and scaling by touch events.
    /// </summary>
    public class TouchManipulator : PlotManipulator<OxyTouchEventArgs>
    {
        /// <summary>
        /// The previous position
        /// </summary>
        private ScreenPoint previousPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchManipulator" /> class.
        /// </summary>
        /// <param name="plotControl">The plot control.</param>
        public TouchManipulator(IPlotView plotControl)
            : base(plotControl)
        {
        }

        /// <summary>
        /// Occurs when a touch delta event is handled.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyTouchEventArgs e)
        {
            base.Delta(e);

            var newPosition = this.previousPosition + e.DeltaTranslation;

            if (this.XAxis != null)
            {
                this.XAxis.Pan(this.previousPosition, newPosition);
            }

            if (this.YAxis != null)
            {
                this.YAxis.Pan(this.previousPosition, newPosition);
            }

            var current = this.InverseTransform(newPosition.X, newPosition.Y);

            if (this.XAxis != null)
            {
                this.XAxis.ZoomAt(e.DeltaScale.X, current.X);
            }

            if (this.YAxis != null)
            {
                this.YAxis.ZoomAt(e.DeltaScale.Y, current.Y);
            }

            this.PlotControl.InvalidatePlot(false);

            this.previousPosition = newPosition;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyTouchEventArgs e)
        {
            this.AssignAxes(e.Position);
            base.Started(e);
            this.previousPosition = e.Position;
        }
    }
}