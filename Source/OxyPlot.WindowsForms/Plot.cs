//-----------------------------------------------------------------------
// <copyright file="Plot.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace Oxyplot.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using OxyPlot;

    /// <summary>
    /// Represents a control that displays a plot.
    /// </summary>
    [Serializable]
    public class Plot : Control, IPlotControl
    {
        #region Constants and Fields

        /// <summary>
        /// The pan action.
        /// </summary>
        private readonly PanAction panAction;

        /// <summary>
        /// The tracker action.
        /// </summary>
        private readonly TrackerAction trackerAction;

        /// <summary>
        /// The zoom action.
        /// </summary>
        private readonly ZoomAction zoomAction;

        /// <summary>
        /// The is model invalidated.
        /// </summary>
        private bool isModelInvalidated;

        /// <summary>
        /// The model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        /// The update data.
        /// </summary>
        private bool updateData = true;

        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private Rectangle zoomRectangle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Plot"/> class.
        /// </summary>
        public Plot()
        {
            this.DoubleBuffered = true;
            this.Model = new PlotModel();

            this.panAction = new PanAction(this);
            this.zoomAction = new ZoomAction(this);
            this.trackerAction = new TrackerAction(this);

            this.MouseActions = new List<OxyMouseAction> { this.panAction, this.zoomAction, this.trackerAction };
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public PlotModel ActualModel
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Gets or sets Model.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public PlotModel Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.model = value;
                this.InvalidatePlot(true);
            }
        }

        /// <summary>
        /// Gets MouseActions.
        /// </summary>
        public List<OxyMouseAction> MouseActions { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get axes from point.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="xaxis">
        /// The xaxis.
        /// </param>
        /// <param name="yaxis">
        /// The yaxis.
        /// </param>
        public void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis)
        {
            if (this.Model == null)
            {
                xaxis = null;
                yaxis = null;
                return;
            }

            this.Model.GetAxesFromPoint(pt, out xaxis, out yaxis);
        }

        /// <summary>
        /// The get series from point.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <returns>
        /// </returns>
        public ISeries GetSeriesFromPoint(ScreenPoint pt, double limit)
        {
            if (this.Model == null)
            {
                return null;
            }

            return this.Model.GetSeriesFromPoint(pt, limit);
        }

        /// <summary>
        /// The hide tracker.
        /// </summary>
        public void HideTracker()
        {
        }

        /// <summary>
        /// The hide zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            this.zoomRectangle = Rectangle.Empty;
            this.Invalidate();
        }

        /// <summary>
        /// The invalidate plot.
        /// </summary>
        /// <param name="updateData">
        /// The update data.
        /// </param>
        public void InvalidatePlot(bool updateData)
        {
            lock (this)
            {
                this.isModelInvalidated = true;
                this.updateData = this.updateData || updateData;
            }

            this.Invalidate();
        }

        /// <summary>
        /// The pan.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="x0">
        /// The x 0.
        /// </param>
        /// <param name="x1">
        /// The x 1.
        /// </param>
        public void Pan(IAxis axis, ScreenPoint x0, ScreenPoint x1)
        {
            axis.Pan(x0, x1);
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// The refresh plot.
        /// </summary>
        /// <param name="updateData">
        /// The update data.
        /// </param>
        public void RefreshPlot(bool updateData)
        {
            lock (this)
            {
                this.isModelInvalidated = true;
                this.updateData = this.updateData || updateData;
            }

            this.Refresh();
        }

        /// <summary>
        /// The reset.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        public void Reset(IAxis axis)
        {
            axis.Reset();
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// The show tracker.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        public void ShowTracker(TrackerHitResult data)
        {
            // not implemented for WindowsForms
        }

        /// <summary>
        /// The show zoom rectangle.
        /// </summary>
        /// <param name="r">
        /// The r.
        /// </param>
        public void ShowZoomRectangle(OxyRect r)
        {
            this.zoomRectangle = new Rectangle((int)r.Left, (int)r.Top, (int)r.Width, (int)r.Height);
            this.Invalidate();
        }

        /// <summary>
        /// The zoom.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        public void Zoom(IAxis axis, double p1, double p2)
        {
            axis.Zoom(p1, p2);
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// The zoom all.
        /// </summary>
        public void ZoomAll()
        {
            foreach (Axis a in this.Model.Axes)
            {
                a.Reset();
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// The zoom at.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="factor">
        /// The factor.
        /// </param>
        /// <param name="x">
        /// The x.
        /// </param>
        public void ZoomAt(IAxis axis, double factor, double x)
        {
            axis.ZoomAt(factor, x);
            this.InvalidatePlot(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on key down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.A)
            {
                this.ZoomAll();
            }

            bool control = (e.Modifiers & Keys.Control) == Keys.Control;
            bool alt = (e.Modifiers & Keys.Alt) == Keys.Alt;

            if (control && alt && this.ActualModel != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.R:
                        SetClipboardText(this.ActualModel.CreateTextReport());
                        break;
                    case Keys.C:
                        SetClipboardText(this.ActualModel.ToCode());
                        break;
                    case Keys.X:

                        // Clipboard.SetText(this.ToXml());
                        break;
                }
            }
        }

        private void SetClipboardText(string text)
        {
            try
            {
                // todo: can't get the following solution to work
                // http://stackoverflow.com/questions/5707990/requested-clipboard-operation-did-not-succeed

                Clipboard.SetText(text);
            }
            catch (ExternalException ee)
            {
                // Requested Clipboard operation did not succeed.
                MessageBox.Show(this, ee.Message, "OxyPlot");
            }
        }

        /// <summary>
        /// The on mouse down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.Focus();
            this.Capture = true;

            bool control = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;
            bool alt = ModifierKeys == Keys.Alt;

            OxyMouseButton button = OxyMouseButton.Left;
            if (e.Button == MouseButtons.Middle)
            {
                button = OxyMouseButton.Middle;
            }

            if (e.Button == MouseButtons.Right)
            {
                button = OxyMouseButton.Right;
            }

            if (e.Button == MouseButtons.XButton1)
            {
                button = OxyMouseButton.XButton1;
            }

            if (e.Button == MouseButtons.XButton2)
            {
                button = OxyMouseButton.XButton2;
            }

            var p = new ScreenPoint(e.X, e.Y);
            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseDown(p, button, e.Clicks, control, shift, alt);
            }
        }

        /// <summary>
        /// The on mouse move.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool control = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;
            bool alt = ModifierKeys == Keys.Alt;
            var p = new ScreenPoint(e.X, e.Y);
            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseMove(p, control, shift, alt);
            }
        }

        /// <summary>
        /// The on mouse up.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseUp();
            }

            this.Capture = false;
        }

        /// <summary>
        /// The on mouse wheel.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            bool control = ModifierKeys == Keys.Control;
            bool shift = ModifierKeys == Keys.Shift;
            bool alt = ModifierKeys == Keys.Alt;
            var p = new ScreenPoint(e.X, e.Y);
            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseWheel(p, e.Delta, control, shift, alt);
            }
        }

        /// <summary>
        /// The on paint.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            lock (this)
            {
                if (this.isModelInvalidated)
                {
                    if (this.model != null)
                    {
                        this.model.Update(this.updateData);
                        this.updateData = false;
                    }

                    this.isModelInvalidated = false;
                }
            }

            var rc = new GraphicsRenderContext(e.Graphics, this.Width, this.Height); // e.ClipRectangle
            if (this.model != null)
            {
                this.model.Render(rc);
            }

            if (this.zoomRectangle != Rectangle.Empty)
            {
                using (var zoomBrush = new SolidBrush(Color.FromArgb(0x40, 0xFF, 0xFF, 0x00)))
                using (var zoomPen = new Pen(Color.Black))
                {
                    zoomPen.DashPattern = new float[] { 3, 1 };
                    e.Graphics.FillRectangle(zoomBrush, this.zoomRectangle);
                    e.Graphics.DrawRectangle(zoomPen, this.zoomRectangle);
                }
            }
        }

        /// <summary>
        /// The on resize.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.InvalidatePlot(false);
        }

        #endregion
    }
}
