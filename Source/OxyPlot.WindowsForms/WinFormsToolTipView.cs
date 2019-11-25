// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinFormsToolTipView.cs" company="OxyPlot">
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
        /// Disposes the tooltip if possible.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Shows the tooltip.
        /// </summary>
        /// <param name="value">The string to show as tooltip.</param>
        public void ShowToolTip(string value)
        {
            Point pos = this.pv.PointToClient(Control.MousePosition);
            pos.Y += Cursor.Current.Size.Height;

            this.NativeToolTip.Show(value, this.pv, pos, this.ShowDuration);
        }

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        public void HideToolTip()
        {
            this.NativeToolTip.Hide(this.pv);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        protected virtual void Dispose(bool disposing)
        {
            this.NativeToolTip.Dispose();
        }
    }
}
