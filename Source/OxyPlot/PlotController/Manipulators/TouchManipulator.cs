// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TouchManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        /// <param name="plotView">The plot view.</param>
        public TouchManipulator(IPlotView plotView)
            : base(plotView)
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

            this.PlotView.InvalidatePlot(false);

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