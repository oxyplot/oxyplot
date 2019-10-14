namespace OxyPlot
{
    using OxyPlot.WindowsForms;
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class CustomToolTip : IToolTip
    {
        /// <summary>
        /// The associated PlotView on which the tooltip is shown.
        /// </summary>
        private PlotView pv;

        /// <summary>
        /// A reference to the previously hovered plot element wrapped by ToolTippedPlotElement and used in the tooltip system.
        /// </summary>
        private ToolTippedPlotElement previouslyHoveredPlotElement;

        /// <summary>
        /// A reference to the currently hovered plot element wrapped by ToolTippedPlotElement and used in the tooltip system.
        /// </summary>
        private ToolTippedPlotElement currentlyHoveredPlotElement;

        /// <summary>
        /// The cancellation token source used to cancel the task that shows the tooltip after an initial delay,
        /// and the task that hides the tooltip after the show duration.
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// The Task for the initial delay of the tooltip.
        /// </summary>
        private Task firstToolTipTask;

        /// <summary>
        /// The Task for the minimum delay between tooltip showings.
        /// </summary>
        private Task secondToolTipTask;

        /// <summary>
        /// The storage for the OxyToolTipString property.
        /// </summary>
        private string lastToolTipString = null;

        /// <summary>
        /// The native WinForms ToolTip object.
        /// </summary>
        public ToolTip NativeToolTip { get; set; }

        /// <summary>
        /// Constructs this IToolTip implementation and associates it with the given PlotView.
        /// </summary>
        /// <param name="v">The WinForms-based PlotView instance to which to associate the tooltip.</param>
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
        /// When the mouse enters, leaves or moves over the associated PlotView, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pv_MouseEnter(object sender, EventArgs e)
        {
            UpdateToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated PlotView, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pv_MouseLeave(object sender, EventArgs e)
        {
            UpdateToolTip();
        }

        /// <summary>
        /// When the mouse enters, leaves or moves over the associated PlotView, update the tooltip visibility and contents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pv_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateToolTip();
        }

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

                this.lastToolTipString = null;

                this.NativeToolTip.Hide(this.pv);
            }
        }

        public void Show()
        {
            if (this.previouslyHoveredPlotElement != this.currentlyHoveredPlotElement)
            {
                if (this.tokenSource != null)
                {
                    this.tokenSource.Cancel();
                    this.tokenSource.Dispose();
                    this.tokenSource = null;
                }

                this.tokenSource = new CancellationTokenSource();
                this.firstToolTipTask = ShowToolTip(Text, this.tokenSource.Token);
            }
        }

        /// <summary>
        /// The string representation of the ToolTip. In its setter there isn't any check of the value to be different than the previous value, and in the setter, if the value is null or empty string, the ToolTip is removed from the PlotView. The ToolTip shows up naturally if the mouse is over the PlotView, using the configuration in the PlotView's c-tor.
        /// </summary>
        public string Text
        {
            get
            {
                return lastToolTipString;
            }
            set
            {
                if (value == null)
                {
                    Hide();
                }
                else
                {
                    if (this.previouslyHoveredPlotElement != this.currentlyHoveredPlotElement)
                    {
                        this.lastToolTipString = value;
                        Show();
                    }
                }
            }
        }

        /// <summary>
        /// Disposes the tool tip if possible.
        /// </summary>
        public void Dispose()
        {
            this.NativeToolTip.Dispose();
        }

        /// <summary>
        /// Internal asynchronous method for showing the ToolTip.
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
        /// Internal asynchronous method for hiding the ToolTip.
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
                // these 2 lines must be before the third which calls the setter of OxyToolTipString
                this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                this.currentlyHoveredPlotElement = new ToolTippedPlotElement(true);

                // show the tooltip
                this.Text = this.pv.Model.TitleToolTip;

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

            // should we use other value than 5 in this line?
            System.Collections.Generic.IEnumerable<HitTestResult> r =
                this.pv.Model.HitTest(new HitTestArguments(sp, 5));

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
                            // these 2 lines must be before the third which calls the setter of OxyToolTipString
                            this.previouslyHoveredPlotElement = this.currentlyHoveredPlotElement;
                            this.currentlyHoveredPlotElement = new ToolTippedPlotElement(pe);

                            // show the tooltip
                            this.Text = pe.ToolTip;
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
        /// Does hit-testing and hides or shows the tooltip if needed.
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
                this.Text = null;
            }
        }
    }
}
