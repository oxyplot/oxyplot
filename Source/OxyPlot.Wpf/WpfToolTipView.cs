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
    public class WpfToolTipView : IToolTipView
    {
        /// <summary>
        /// The <see cref="Task"/> for the initial delay of the tooltip.
        /// </summary>
        private Task firstToolTipTask;

        /// <summary>
        /// The <see cref="Task"/> for the minimum delay between tooltip showings.
        /// </summary>
        private Task secondToolTipTask;

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
        /// Initializes a new instance of the <see cref="WpfToolTipView"/> class.
        /// It also associates it with the given <see cref="PlotBase"/>.
        /// </summary>
        /// <param name="v">The WPF-based <see cref="PlotBase"/> instance to which to associate the tooltip.</param>
        public WpfToolTipView(PlotBase v)
        {
            this.pb = v;

            this.NativeToolTip = new ToolTip();
        }

        /// <summary>
        /// Gets or sets the native WPF <see cref="ToolTip"/> object.
        /// </summary>
        public ToolTip NativeToolTip { get; set; }

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
            this.Text = null;
            this.NativeToolTip.Dispatcher.Invoke(
                new Action(() =>
                {
                    this.NativeToolTip.IsOpen = false;
                }), System.Windows.Threading.DispatcherPriority.Send);
        }

        /// <summary>
        /// Shows the tooltip if it is the case.
        /// </summary>
        public void ShowWithInitialDelay(CancellationToken ct)
        {
            this.firstToolTipTask = this.ShowToolTip(this.Text, ct);
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
    }
}
