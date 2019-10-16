// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomToolTip.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Wrapper around WinForms' ToolTip class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using OxyPlot.WindowsForms;
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Wrapper around WinForms' <see cref="ToolTip"/> class.
    /// </summary>
    public class CustomToolTip : IToolTip
    {
        /// <summary>
        /// The associated <see cref="PlotView"/> on which the tooltip is shown.
        /// </summary>
        private PlotView pv;

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
        /// The storage for the <see cref="Text"/> property.
        /// </summary>
        private string lastToolTipString = null;

        /// <summary>
        /// The native WinForms <see cref="ToolTip"/> object.
        /// </summary>
        public ToolTip NativeToolTip { get; set; }

        /// <summary>
        /// Hit testing tolerance for usual <see cref="PlotElement"/>s (more precisely, excluding the plot title area).
        /// </summary>
        public double UsualPlotElementHitTestingTolerance { get; set; } = 10;

        /// <summary>
        /// Constructs this <see cref="IToolTip"/> implementation and associates it with the given <see cref="PlotView"/>.
        /// </summary>
        /// <param name="v">The WinForms-based <see cref="PlotView"/> instance to which to associate the tooltip.</param>
        public CustomToolTip(PlotView v)
        {
            this.pv = v;
            this.pv.MouseMove += Pv_MouseMove;
            this.pv.MouseLeave += Pv_MouseLeave;
            this.pv.MouseEnter += Pv_MouseEnter;

            this.previouslyHoveredPlotElement = new ToolTippedPlotElement();
            this.currentlyHoveredPlotElement = new ToolTippedPlotElement();

            this.NativeToolTip = new ToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="PlotView"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pv_MouseEnter(object sender, EventArgs e)
        {
            UpdateToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="PlotView"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pv_MouseLeave(object sender, EventArgs e)
        {
            UpdateToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated <see cref="PlotView"/>, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pv_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateToolTip();
        }

        /// <summary>
        /// Hides the tooltip if it is the case.
        /// </summary>
        public void Hide()
        {
            if (this.previouslyHoveredPlotElement != this.currentlyHoveredPlotElement)
            {
                if (this.tokenSource != null)
                {
                    this.tokenSource.Cancel();
                    this.tokenSource.Dispose();
                    this.tokenSource = null;
                }

                this.Text = null;

                this.NativeToolTip.Hide(this.pv);
            }
        }

        /// <summary>
        /// Shows the tooltip if it is the case.
        /// </summary>
        public void Show()
        {
            if (this.Text != null &&
                this.previouslyHoveredPlotElement != this.currentlyHoveredPlotElement)
            {
                if (this.tokenSource != null)
                {
                    this.tokenSource.Cancel();
                    this.tokenSource.Dispose();
                    this.tokenSource = null;
                }

                this.tokenSource = new CancellationTokenSource();
                this.firstToolTipTask = ShowToolTip(this.Text, this.tokenSource.Token);
            }
        }

        /// <summary>
        /// The string representation of the tooltip.
        /// </summary>
        public string Text
        {
            get
            {
                return lastToolTipString;
            }
            set
            {
                this.lastToolTipString = value;
            }
        }

        /// <summary>
        /// Disposes the tooltip if possible.
        /// </summary>
        public void Dispose()
        {
            this.NativeToolTip.Dispose();
        }

        /// <summary>
        /// Internal asynchronous method for showing the tooltip.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task ShowToolTip(string value, CancellationToken ct)
        {
            if (this.secondToolTipTask != null)
            {
                await this.secondToolTipTask;
            }

            if (ct.IsCancellationRequested)
            {
                return;
            }

            // necessary hiding for when the user moves the mouse from over a plot element to another element without empty space between them:
            this.NativeToolTip.Hide(this.pv);

            if (ct.IsCancellationRequested)
            {
                return;
            }

            await Task.Delay(this.NativeToolTip.InitialDelay, ct);

            if (ct.IsCancellationRequested)
            {
                return;
            }

            Point pos = this.pv.PointToClient(Control.MousePosition);
            pos.Y += Cursor.Current.Size.Height;

            // Without the -2000, the duration of the tooltip is too long (because of the animation, probably)
            this.NativeToolTip.Show(value, this.pv, pos, Math.Max(0, this.NativeToolTip.AutoPopDelay - 2000));

            if (ct.IsCancellationRequested)
            {
                return;
            }

            _ = HideToolTip(ct);
        }

        /// <summary>
        /// Internal asynchronous method for hiding the tooltip.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task HideToolTip(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            int betweenShowDelay = this.NativeToolTip.ReshowDelay;

            this.secondToolTipTask = Task.Delay(betweenShowDelay);
            _ = this.secondToolTipTask.ContinueWith(new Action<Task>((t) =>
            {
                this.secondToolTipTask = null;
            }));

            if (ct.IsCancellationRequested)
            {
                return;
            }

            int showDuration = this.NativeToolTip.AutoPopDelay;

            if (ct.IsCancellationRequested)
            {
                return;
            }

            await Task.Delay(showDuration, ct);

            if (ct.IsCancellationRequested)
            {
                return;
            }

            this.NativeToolTip.Hide(this.pv);

            if (ct.IsCancellationRequested)
            {
                return;
            }
        }

        /// <summary>
        /// Returns true if the event is handled.
        /// </summary>
        /// <returns></returns>
        protected bool HandleTitleToolTip(ScreenPoint sp)
        {
            if (this.pv.Model == null)
            {
                return false;
            }

            bool v = this.pv.Model.TitleArea.Contains(sp);

            if (v && this.pv.Model.Title != null)
            {
                // these 2 lines must be before the third which calls the setter of Text
                this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                this.currentlyHoveredPlotElement = new ToolTippedPlotElement(true);

                // show the tooltip
                this.Text = this.pv.Model.TitleToolTip;
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
        /// <returns></returns>
        protected bool HandlePlotElementsToolTip(ScreenPoint sp)
        {
            if (this.pv.Model == null)
            {
                return false;
            }

            bool found = false;

            System.Collections.Generic.IEnumerable<HitTestResult> r =
                this.pv.Model.HitTest(new HitTestArguments(sp, UsualPlotElementHitTestingTolerance));

            foreach (HitTestResult rtr in r)
            {
                // if an element is found under the mouse cursor
                if (rtr.Element != null)
                {
                    // if it is a PlotElement (not just an UIElement)
                    if (rtr.Element is PlotElement pe)
                    {
                        // if the mouse was not over it previously
                        if (pe != this.currentlyHoveredPlotElement)
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
                this.currentlyHoveredPlotElement = new ToolTippedPlotElement();
            }

            return found;
        }

        /// <summary>
        /// Does hit-testing and hides or hides/shows the tooltip if needed.
        /// </summary>
        protected void UpdateToolTip()
        {
            if (this.pv.ActualModel == null)
            {
                return;
            }

            ScreenPoint sp = this.pv.PointToClient(Control.MousePosition).ToScreenPoint();


            bool handleTitle = HandleTitleToolTip(sp);
            bool handleOthers = false;

            if (!handleTitle)
            {
                handleOthers = HandlePlotElementsToolTip(sp);
            }

            if (!handleTitle && !handleOthers)
            {
                this.Hide();
            }
        }
    }
}
