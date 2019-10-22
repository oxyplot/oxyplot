// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinFormsToolTip.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Wrapper around WinForms' ToolTip class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Wrapper around WinForms' <see cref="ToolTip"/> class.
    /// </summary>
    public class WinFormsToolTipView : IToolTipView
    {
        /// <summary>
        /// The associated <see cref="PlotView"/> on which the tooltip is shown.
        /// </summary>
        private PlotView pv;

        /// <summary>
        /// The <see cref="Task"/> for the initial delay of the tooltip.
        /// </summary>
        private Task firstToolTipTask;

        /// <summary>
        /// The <see cref="Task"/> for the minimum delay between tooltip showings.
        /// </summary>
        private Task secondToolTipTask;

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
        /// Initializes a new instance of the <see cref="WinFormsToolTipView"/> class.
        /// It also associates it with the given <see cref="PlotView"/>.
        /// </summary>
        /// <param name="v">The WinForms-based <see cref="PlotView"/> instance to which to associate the tooltip.</param>
        public WinFormsToolTipView(PlotView v)
        {
            this.pv = v;

            this.NativeToolTip = new ToolTip();
        }

        /// <summary>
        /// Gets or sets the native WinForms <see cref="ToolTip"/> object.
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
                    this.NativeToolTip.InitialDelay :
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
                    this.NativeToolTip.AutoPopDelay :
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
                    this.NativeToolTip.ReshowDelay :
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
            this.NativeToolTip.Hide(this.pv);
        }

        /// <summary>
        /// Shows the tooltip if it is the case.
        /// </summary>
        public void Show(CancellationToken ct)
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
            this.NativeToolTip.Dispose();
        }

        /// <summary>
        /// Internal asynchronous method for showing the ToolTip.
        /// </summary>
        /// <param name="value">The string to show as a tooltip.</param>
        /// <param name="ct">The cancellation token for when the user moves the cursor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ShowToolTip(string value, CancellationToken ct)
        {
            // necessary hiding for when the user moves the mouse from over a plot element to another element without empty space between them:
            this.NativeToolTip.Hide(this.pv);

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

            Point pos = this.pv.PointToClient(Control.MousePosition);
            pos.Y += Cursor.Current.Size.Height;

            this.NativeToolTip.Show(value, this.pv, pos, this.ShowDuration);

            _ = this.HideToolTip(ct);
        }

        /// <summary>
        /// Internal asynchronous method for hiding the ToolTip.
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

            this.NativeToolTip.Hide(this.pv);
        }
    }
}
