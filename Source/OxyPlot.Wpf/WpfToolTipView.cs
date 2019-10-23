// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomToolTip.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Wrapper around WPF's ToolTip class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Wrapper around WPF's <see cref="ToolTip"/> class.
    /// </summary>
    public class WpfToolTipView : IToolTipView
    {
        /// <summary>
        /// The associated <see cref="PlotBase"/> on which the tooltip is shown.
        /// </summary>
        private PlotBase pb;

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
        /// Hides the tooltip.
        /// </summary>
        public void HideToolTip()
        {
            this.NativeToolTip.Dispatcher.Invoke(
                new Action(() =>
                {
                    this.NativeToolTip.IsOpen = false;
                }), System.Windows.Threading.DispatcherPriority.Send);
        }

        /// <summary>
        /// Shows the tooltip.
        /// </summary>
        /// <param name="value">The string to show as tooltip.</param>
        public void ShowToolTip(string value)
        {
            this.NativeToolTip.Content = value;
            this.NativeToolTip.Dispatcher.Invoke(new Action(() =>
            {
                this.NativeToolTip.IsOpen = true;
            }));
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
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
