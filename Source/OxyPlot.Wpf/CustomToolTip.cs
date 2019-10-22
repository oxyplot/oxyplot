// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomToolTip.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Wrapper around WPF's ToolTip class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;
    using OxyPlot.Wpf;

    /// <summary>
    /// Wrapper around WPF's <see cref="ToolTip"/> class.
    /// </summary>
    public class CustomToolTip : IToolTip
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
        /// The <see cref="Task"/> for the initial delay of the tooltip.
        /// </summary>
        private Task firstToolTipTask;

        /// <summary>
        /// The <see cref="Task"/> for the minimum delay between tooltip showings.
        /// </summary>
        private Task secondToolTipTask;

        /// <summary>
        /// The cancellation token source used to cancel the task that shows the tooltip after an initial delay,
        /// and the task that hides the tooltip after the show duration.
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// The associated <see cref="PlotBase"/> on which the tooltip is shown.
        /// </summary>
        private PlotBase pb;

        /// <summary>
        /// The storage for the <see cref="Text"/> property.
        /// </summary>
        private string lastToolTipString = null;

        /// <summary>
        /// Custom initial show delay storage.
        /// </summary>
        private int initialShowDelay = -1;

        /// <summary>
        /// Custom show duration storage.
        /// </summary>
        private int showDuration = -1;

        /// <summary>
        /// Custom between show delay storage.
        /// </summary>
        private int betweenShowDelay = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomToolTip"/> class.
        /// It also associates it with the given <see cref="PlotBase"/>.
        /// </summary>
        /// <param name="v">The WPF-based <see cref="PlotBase"/> instance to which to associate the tooltip.</param>
        public CustomToolTip(PlotBase v)
        {
            this.pb = v;
            this.pb.PreviewMouseMove += this.Pb_PreviewMouseMove;
            this.pb.MouseLeave += this.Pb_MouseLeave;
            this.pb.MouseEnter += this.Pb_MouseEnter;

            this.previouslyHoveredPlotElement = new ToolTippedPlotElement();
            this.currentlyHoveredPlotElement = new ToolTippedPlotElement();

            this.NativeToolTip = new ToolTip();
        }

        /// <summary>
        /// Gets or sets the native WPF <see cref="ToolTip"/> object.
        /// </summary>
        public ToolTip NativeToolTip { get; set; }

        /// <summary>
        /// Gets or sets the hit testing tolerance for usual <see cref="PlotElement"/>s (more precisely, excluding the plot title area).
        /// </summary>
        public double UsualPlotElementHitTestingTolerance { get; set; } = 10;

        /// <summary>
        /// Gets or sets the string representation of the tooltip.
        /// </summary>
        public string Text
        {
            get
            {
                return this.lastToolTipString;
            }

            set
            {
                this.lastToolTipString = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of time before a tooltip opens.
        /// </summary>
        public int InitialShowDelay
        {
            get
            {
                return this.initialShowDelay < 0 ?
                    ToolTipService.GetInitialShowDelay(this.pb) :
                    this.initialShowDelay;
            }

            set
            {
                this.initialShowDelay = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time that a tooltip remains visible.
        /// </summary>
        public int ShowDuration
        {
            get
            {
                return this.showDuration < 0 ?
                    ToolTipService.GetShowDuration(this.pb) :
                    this.showDuration;
            }

            set
            {
                this.showDuration = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum time between the display of two tooltips where the second tooltip appears without a delay.
        /// </summary>
        public int BetweenShowDelay
        {
            get
            {
                return this.betweenShowDelay < 0 ?
                    ToolTipService.GetBetweenShowDelay(this.pb) :
                    this.betweenShowDelay;
            }

            set
            {
                this.betweenShowDelay = value;
            }
        }

        /// <summary>
        /// Hides the tooltip if it is the case.
        /// </summary>
        public void Hide()
        {
            if (!this.previouslyHoveredPlotElement.IsEquivalentWith(this.currentlyHoveredPlotElement))
            {
                if (this.tokenSource != null)
                {
                    this.tokenSource.Cancel();
                    this.tokenSource.Dispose();
                    this.tokenSource = null;
                }

                this.Text = null;

                this.NativeToolTip.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        this.NativeToolTip.IsOpen = false;
                    }), System.Windows.Threading.DispatcherPriority.Send);
            }
        }

        /// <summary>
        /// Shows the tooltip if it is the case.
        /// </summary>
        public void Show()
        {
            if (this.Text != null &&
                !this.previouslyHoveredPlotElement.IsEquivalentWith(this.currentlyHoveredPlotElement))
            {
                if (this.tokenSource != null)
                {
                    this.tokenSource.Cancel();
                    this.tokenSource.Dispose();
                    this.tokenSource = null;
                }

                this.tokenSource = new CancellationTokenSource();
                this.firstToolTipTask = this.ShowToolTip(this.Text, this.tokenSource.Token);
            }
        }

        /// <summary>
        /// Disposes the tooltip if possible.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Runs a <see cref="Task"/> and ignores the cancellation exception.
        /// </summary>
        /// <param name="t">The <see cref="Task"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task CancelableTaskAsync(Task t)
        {
            try
            {
                await t;
            }
            catch (OperationCanceledException)
            {
                // nothing special to do
            }
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Internal asynchronous method for showing the tooltip.
        /// </summary>
        /// <param name="value">The string to show as a tooltip.</param>
        /// <param name="ct">The cancellation token for when the user moves the cursor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ShowToolTip(string value, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.NativeToolTip.Dispatcher.Invoke(new Action(() =>
            {
                this.NativeToolTip.IsOpen = false;
            }));

            if (ct.IsCancellationRequested)
            {
                return;
            }

            if (this.secondToolTipTask != null)
            {
                await CancelableTaskAsync(this.secondToolTipTask);
            }
            else
            {
                await CancelableTaskAsync(Task.Delay(this.InitialShowDelay, ct));
            }

            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.NativeToolTip.Content = value;

            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.NativeToolTip.Dispatcher.Invoke(new Action(() =>
            {
                this.NativeToolTip.IsOpen = true;
            }));

            _ = this.HideToolTip(ct);
        }

        /// <summary>
        /// Internal asynchronous method for hiding the tooltip.
        /// </summary>
        /// <param name="ct">The cancellation token for when the user moves the cursor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task HideToolTip(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.secondToolTipTask = Task.Delay(this.BetweenShowDelay);
            _ = this.secondToolTipTask.ContinueWith(new Action<Task>((t) =>
            {
                this.secondToolTipTask = null;
            }));

            if (ct.IsCancellationRequested)
            {
                return;
            }

            await CancelableTaskAsync(Task.Delay(this.ShowDuration, ct));

            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.NativeToolTip.Dispatcher.Invoke(new Action(() =>
            {
                this.NativeToolTip.IsOpen = false;
            }));
        }

        /// <summary>
        /// Returns true if the event is handled.
        /// </summary>
        /// <param name="sp">The point for hit-testing.</param>
        /// <returns>Whether there is a plot title and the plot title's area contains <paramref name="sp"/>.</returns>
        private bool HandleTitleToolTip(ScreenPoint sp)
        {
            bool v = this.pb.ActualModel.TitleArea.Contains(sp);

            if (v && this.pb.ActualModel.Title != null)
            {
                // these 2 lines must be before the third which calls the setter of Text
                this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                this.currentlyHoveredPlotElement = new ToolTippedPlotElement(true);

                // set the tooltip to be the tooltip of the plot title
                this.Text = this.pb.ActualModel.TitleToolTip;
                if (this.Text == null)
                {
                    this.Hide();
                }
                else
                {
                    this.Show();
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
        private bool HandlePlotElementsToolTip(ScreenPoint sp)
        {
            bool found = false;

            System.Collections.Generic.IEnumerable<HitTestResult> r =
                this.pb.ActualModel.HitTest(new HitTestArguments(sp, this.UsualPlotElementHitTestingTolerance));

            foreach (HitTestResult rtr in r)
            {
                if (rtr.Element != null)
                {
                    if (rtr.Element is PlotElement pe)
                    {
                        if (!this.previouslyHoveredPlotElement.IsEquivalentWith(pe))
                        {
                            // these 2 lines must be before the third which calls the setter of Text
                            this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                            this.currentlyHoveredPlotElement = new ToolTippedPlotElement(pe);

                            // show the tooltip
                            this.Text = pe.ToolTip;
                            if (this.Text == null)
                            {
                                this.Hide();
                            }
                            else
                            {
                                this.Show();
                            }
                        }

                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                this.currentlyHoveredPlotElement = new ToolTippedPlotElement();
            }

            return found;
        }

        /// <summary>
        /// Updates the custom tooltip system's tooltip.
        /// </summary>
        private void UpdateToolTip()
        {
            if (this.pb.ActualModel == null)
            {
                return;
            }

            ScreenPoint sp = Mouse.GetPosition(this.pb).ToScreenPoint();

            // do the hit-testing:
            bool handleTitle = this.HandleTitleToolTip(sp);
            bool handleOthers = false;

            if (!handleTitle)
            {
                handleOthers = this.HandlePlotElementsToolTip(sp);
            }

            if (!handleTitle && !handleOthers)
            {
                this.Hide();
            }
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="PlotBase"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Pb_MouseEnter(object sender, MouseEventArgs e)
        {
            this.UpdateToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="PlotBase"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Pb_MouseLeave(object sender, MouseEventArgs e)
        {
            this.UpdateToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="PlotBase"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Pb_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            this.UpdateToolTip();
        }
    }
}
