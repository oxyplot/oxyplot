// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolTipController.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Controller for <see cref="IToolTipView"/>s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Controller for <see cref="IToolTipView"/>s.
    /// </summary>
    public class ToolTipController : IToolTipController
    {
        /// <summary>
        /// A reference to the previously hovered plot element wrapped by <see cref="ToolTippedPlotElement"/> and used in the tooltip system.
        /// </summary>
        private ToolTippedPlotElement previouslyHoveredPlotElement;

        /// <summary>
        /// A reference to the currently hovered plot element wrapped by <see cref="ToolTippedPlotElement"/> and used in the tooltip system.
        /// </summary>
        private ToolTippedPlotElement currentlyHoveredPlotElement;

        /// <summary>
        /// The cancellation token source used to cancel the task that shows the tooltip after an initial delay,
        /// and the task that hides the tooltip after the show duration.
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// The <see cref="Task"/> for the initial delay of the tooltip.
        /// </summary>
        private Task firstToolTipTask;

        /// <summary>
        /// The <see cref="Task"/> for the minimum delay between tooltip showings.
        /// </summary>
        private Task secondToolTipTask;

        /// <summary>
        /// The last shown tooltip string.
        /// </summary>
        private string lastShownToolTipString;

        /// <summary>
        /// Storage for the property <see cref="ToolTipView"/>.
        /// </summary>
        private IToolTipView currentToolTipView = null;

        /// <summary>
        /// Storage for the property <see cref="PlotModel"/>.
        /// </summary>
        private PlotModel currentPlotModel = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTipController"/> class.
        /// It also associates it with the given <see cref="IToolTipView"/>.
        /// </summary>
        /// <param name="plotModel">The plot model to be associated with this instance.</param>
        /// <param name="toolTipView">The tooltip view to be associated with this instance.</param>
        public ToolTipController(PlotModel plotModel, IToolTipView toolTipView)
        {
            this.ToolTipView = toolTipView;
            this.PlotModel = plotModel;

            this.previouslyHoveredPlotElement = ToolTippedPlotElement.FromPseudoElement();
            this.currentlyHoveredPlotElement = ToolTippedPlotElement.FromPseudoElement();

            this.PlotModel.MouseEnter += this.PlotModel_MouseEnter;
            this.PlotModel.MouseMove += this.PlotModel_MouseMove;
            this.PlotModel.MouseLeave += this.PlotModel_MouseLeave;
        }

        /// <summary>
        /// Gets or sets the associated tooltip view.
        /// </summary>
        public IToolTipView ToolTipView
        {
            get
            {
                return currentToolTipView;
            }

            set
            {
                if (currentToolTipView != value)
                {
                    IToolTipView old = currentToolTipView;
                    currentToolTipView = value;
                    HandleToolTipViewChanged(old, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the associated plot model.
        /// </summary>
        public PlotModel PlotModel
        {
            get
            {
                return currentPlotModel;
            }

            private set
            {
                if (currentPlotModel != value)
                {
                    currentPlotModel = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the hit testing tolerance for usual <see cref="PlotElement"/>s (more precisely, excluding the plot title area).
        /// </summary>
        public double UsualPlotElementHitTestingTolerance { get; set; } = 10;

        /// <summary>
        /// Does hit-testing and hides or hides/shows the tooltip if needed.
        /// </summary>
        /// <param name="sp">The screen point where the mouse cursor sits.</param>
        protected void UpdateToolTip(ScreenPoint sp)
        {
            if (this.PlotModel == null)
            {
                return;
            }

            // do the hit-testing:
            bool handleTitle = this.HandleTitleToolTip(sp);
            bool handleOthers = false;

            if (!handleTitle)
            {
                handleOthers = this.HandlePlotElementsToolTip(sp);
            }

            if (!handleTitle && !handleOthers)
            {
                this.HideToolTipChecked();
            }
        }

        /// <summary>
        /// Returns true if the event is handled.
        /// </summary>
        /// <param name="sp">The point for hit-testing.</param>
        /// <returns>Whether there is a plot title and the plot title's area contains <paramref name="sp"/>.</returns>
        protected bool HandleTitleToolTip(ScreenPoint sp)
        {
            if (this.PlotModel == null)
            {
                return false;
            }

            bool v = this.PlotModel.TitleArea.Contains(sp);

            if (v && this.PlotModel.Title != null)
            {
                // these 2 lines must be before the third which calls the setter of Text
                this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                this.currentlyHoveredPlotElement = ToolTippedPlotElement.FromPseudoElement(true);

                // show the tooltip if available
                this.lastShownToolTipString =
                    this.PlotModel.TitleToolTip is null ?
                    (this.PlotModel.TitleToolTipFormatter is null ?
                        null :
                        this.PlotModel.TitleToolTipFormatter(this.PlotModel)) :
                    this.PlotModel.TitleToolTip;
                if (this.lastShownToolTipString == null)
                {
                    this.HideToolTipChecked();
                }
                else
                {
                    this.ShowToolTipChecked();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the event is handled.
        /// </summary>
        /// <param name="sp">The point for hit-testing.</param>
        /// <returns>Whether there is a <see cref="PlotElement"/> that contains the point <paramref name="sp"/>.</returns>
        protected bool HandlePlotElementsToolTip(ScreenPoint sp)
        {
            if (this.PlotModel == null)
            {
                return false;
            }

            bool found = false;

            System.Collections.Generic.IEnumerable<HitTestResult> r =
                this.PlotModel.HitTest(new HitTestArguments(sp, this.UsualPlotElementHitTestingTolerance));

            foreach (HitTestResult rtr in r)
            {
                // if an element is found under the mouse cursor
                if (rtr.Element != null)
                {
                    // if it is a PlotElement (not just an UIElement)
                    if (rtr.Element is PlotElement pe)
                    {
                        // if the mouse was not over it previously
                        if (!this.currentlyHoveredPlotElement.IsEquivalentWith(pe))
                        {
                            // these 2 lines must be before the third which calls the setter of Text
                            this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                            this.currentlyHoveredPlotElement = ToolTippedPlotElement.FromElement(pe);

                            // show the tooltip if available
                            this.lastShownToolTipString =
                                pe.ToolTip is null ?
                                (pe.ToolTipFormatter is null ?
                                    null :
                                    pe.ToolTipFormatter(rtr)) :
                                pe.ToolTip;
                            if (this.lastShownToolTipString == null)
                            {
                                this.HideToolTipChecked();
                            }
                            else
                            {
                                this.ShowToolTipChecked();
                            }
                        }
                        else
                        {
                        }

                        found = true;
                        break;
                    }
                    else
                    {
                    }
                }
            }

            if (!found)
            {
                this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                this.currentlyHoveredPlotElement = ToolTippedPlotElement.FromPseudoElement();
            }

            return found;
        }

        /// <summary>
        /// Shows the tooltip if it is the case.
        /// </summary>
        protected void ShowToolTipChecked()
        {
            if (this.lastShownToolTipString != null &&
                !this.previouslyHoveredPlotElement.IsEquivalentWith(this.currentlyHoveredPlotElement))
            {
                if (this.tokenSource != null)
                {
                    this.tokenSource.Cancel();
                    this.tokenSource.Dispose();
                    this.tokenSource = null;
                }

                this.tokenSource = new CancellationTokenSource();
                this.ShowWithInitialDelayInternal(this.tokenSource.Token);
            }
        }

        /// <summary>
        /// Hides the tooltip if it is the case.
        /// </summary>
        protected void HideToolTipChecked()
        {
            if (!this.previouslyHoveredPlotElement.IsEquivalentWith(this.currentlyHoveredPlotElement))
            {
                if (this.tokenSource != null)
                {
                    this.tokenSource.Cancel();
                    this.tokenSource.Dispose();
                    this.tokenSource = null;
                }

                this.HideInternal();
            }
        }

        /// <summary>
        /// Hides the tooltip if it is the case.
        /// </summary>
        protected void HideInternal()
        {
            this.lastShownToolTipString = null;
            this.ToolTipView.HideToolTip();
        }

        /// <summary>
        /// Shows the tooltip after an initial delay.
        /// </summary>
        /// <param name="ct">The cancellation token which can be used to cancel the tooltip display or hiding.</param>
        protected void ShowWithInitialDelayInternal(CancellationToken ct)
        {
            this.firstToolTipTask = this.ShowToolTipBase(this.lastShownToolTipString, ct);
        }

        /// <summary>
        /// Internal asynchronous method for showing the tooltip.
        /// </summary>
        /// <param name="value">The string to show as a tooltip.</param>
        /// <param name="ct">The cancellation token for when the user moves the cursor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ShowToolTipBase(string value, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            // necessary hiding for when the user moves the mouse from over a plot element to another element without empty space between them:
            this.ToolTipView.HideToolTip();

            if (ct.IsCancellationRequested)
            {
                return;
            }

            if (this.secondToolTipTask != null)
            {
                await this.secondToolTipTask;
            }
            else
            {
                await Task.Delay(this.ToolTipView.InitialShowDelay, ct);
            }

            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.ToolTipView.ShowToolTip(value);

            _ = this.HideToolTipBase(ct);
        }

        /// <summary>
        /// Internal asynchronous method for hiding the tooltip after a delay.
        /// </summary>
        /// <param name="ct">The cancellation token for when the user moves the cursor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task HideToolTipBase(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.secondToolTipTask = Task.Delay(this.ToolTipView.BetweenShowDelay);
            _ = this.secondToolTipTask.ContinueWith(new Action<Task>((t) =>
            {
                this.secondToolTipTask = null;
            }));

            if (ct.IsCancellationRequested)
            {
                return;
            }

            await Task.Delay(this.ToolTipView.ShowDuration, ct);

            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.ToolTipView.HideToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="OxyPlot.PlotModel"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PlotModel_MouseLeave(object sender, OxyMouseEventArgs e)
        {
            this.UpdateToolTip(e.Position);
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="OxyPlot.PlotModel"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PlotModel_MouseEnter(object sender, OxyMouseEventArgs e)
        {
            this.UpdateToolTip(e.Position);
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="OxyPlot.PlotModel"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            this.UpdateToolTip(e.Position);
        }

        private void HandleToolTipViewChanged(IToolTipView oldView, IToolTipView newView)
        {
            if (oldView != null)
            {
                oldView.Dispose();
            }
        }
    }
}
