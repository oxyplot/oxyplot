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
        /// Initializes a new instance of the <see cref="TouchManipulator" /> class.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        public TouchManipulator(IPlotView plotView)
            : base(plotView)
        {
            SetHandledForPanOrZoom = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <c>e.Handled</c> should be set to <c>true</c>
        /// in case pan or zoom is enabled.
        /// </summary>
        protected bool SetHandledForPanOrZoom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether panning is enabled.
        /// </summary>
        private bool IsPanEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether zooming is enabled.
        /// </summary>
        private bool IsZoomEnabled { get; set; }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public override void Completed(OxyTouchEventArgs e)
        {
            base.Completed(e);

            if (this.SetHandledForPanOrZoom)
            {
                e.Handled |= this.IsPanEnabled || this.IsZoomEnabled;
            }
        }

        /// <summary>
        /// Occurs when a touch delta event is handled.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyTouchEventArgs e)
        {
            base.Delta(e);
            if (!this.IsPanEnabled && !this.IsZoomEnabled)
            {
                return;
            }

            var newPosition = e.Position;
            var previousPosition = newPosition - e.DeltaTranslation;

            if (this.XAxis != null)
            {
                this.XAxis.Pan(previousPosition, newPosition);
            }

            if (this.YAxis != null)
            {
                this.YAxis.Pan(previousPosition, newPosition);
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
            e.Handled = true;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyTouchEventArgs e)
        {
            this.AssignAxes(e.Position);
            base.Started(e);

            if (this.SetHandledForPanOrZoom)
            {
                this.IsPanEnabled = (this.XAxis != null && this.XAxis.IsPanEnabled)
                                    || (this.YAxis != null && this.YAxis.IsPanEnabled);

                this.IsZoomEnabled = (this.XAxis != null && this.XAxis.IsZoomEnabled)
                                     || (this.YAxis != null && this.YAxis.IsZoomEnabled);

                e.Handled |= this.IsPanEnabled || this.IsZoomEnabled;
            }
        }
    }
}