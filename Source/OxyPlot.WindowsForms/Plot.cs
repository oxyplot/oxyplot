// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// Represents a control that displays a plot.
    /// </summary>
    [Serializable]
    public class Plot : Control, IPlotControl
    {
        #region Constants and Fields

        /// <summary>
        ///   The is model invalidated.
        /// </summary>
        private bool isModelInvalidated;

        /// <summary>
        ///   The model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        ///   The mouse manipulator.
        /// </summary>
        private ManipulatorBase mouseManipulator;

        /// <summary>
        ///   The update data.
        /// </summary>
        private bool updateData = true;

        /// <summary>
        ///   The zoom rectangle.
        /// </summary>
        private Rectangle zoomRectangle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Plot" /> class.
        /// </summary>
        public Plot()
        {
            this.DoubleBuffered = true;
            this.Model = new PlotModel();
            this.KeyboardPanHorizontalStep = 0.1;
            this.KeyboardPanVerticalStep = 0.1;
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
        ///   Gets or sets the keyboard pan horizontal step.
        /// </summary>
        /// <value>The keyboard pan horizontal step.</value>
        public double KeyboardPanHorizontalStep { get; set; }

        /// <summary>
        ///   Gets or sets the keyboard pan vertical step.
        /// </summary>
        /// <value>The keyboard pan vertical step.</value>
        public double KeyboardPanVerticalStep { get; set; }

        /// <summary>
        ///   Gets or sets Model.
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the axes from a point.
        /// </summary>
        /// <param name="pt">
        /// The point.
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
        /// Get the series from a point.
        /// </summary>
        /// <param name="pt">
        /// The ppint.
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
        /// Pans all axes.
        /// </summary>
        /// <param name="deltax">
        /// The deltax.
        /// </param>
        /// <param name="deltay">
        /// The deltay.
        /// </param>
        public void PanAll(double deltax, double deltay)
        {
            foreach (IAxis a in this.ActualModel.Axes)
            {
                a.Pan(a.IsHorizontal() ? deltax : deltay);
            }

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
        /// Sets the cursor.
        /// </summary>
        /// <param name="cursor">
        /// The cursor.
        /// </param>
        public void SetCursor(OxyCursor cursor)
        {
            switch (cursor)
            {
                case OxyCursor.Arrow:
                    this.Cursor = Cursors.Arrow;
                    break;
                case OxyCursor.Cross:
                    this.Cursor = Cursors.Cross;
                    break;
                case OxyCursor.SizeAll:
                    this.Cursor = Cursors.SizeAll;
                    break;
                case OxyCursor.SizeNWSE:
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case OxyCursor.None:
                    this.Cursor = null;
                    break;
            }
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
            foreach (var a in this.Model.Axes)
            {
                a.Reset();
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Zooms all axes.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void ZoomAllAxes(double delta)
        {
            foreach (var a in this.ActualModel.Axes)
            {
                this.ZoomAt(a, delta);
            }

            this.RefreshPlot(false);
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
        public void ZoomAt(IAxis axis, double factor, double x = double.NaN)
        {
            if (double.IsNaN(x))
            {
                double sx = (axis.Transform(axis.ActualMaximum) + axis.Transform(axis.ActualMinimum)) * 0.5;
                x = axis.InverseTransform(sx);
            }

            axis.ZoomAt(factor, x);
            this.InvalidatePlot(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.PreviewKeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PreviewKeyDownEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.KeyCode == Keys.A)
            {
                this.ZoomAll();
            }

            bool control = (e.Modifiers & Keys.Control) == Keys.Control;
            bool alt = (e.Modifiers & Keys.Alt) == Keys.Alt;

            double deltax = 0;
            double deltay = 0;
            double zoom = 0;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    deltay = -1;
                    break;
                case Keys.Down:
                    deltay = 1;
                    break;
                case Keys.Left:
                    deltax = -1;
                    break;
                case Keys.Right:
                    deltax = 1;
                    break;
                case Keys.Add:
                case Keys.Oemplus:
                case Keys.PageUp:
                    zoom = 1;
                    break;
                case Keys.Subtract:
                case Keys.OemMinus:
                case Keys.PageDown:
                    zoom = -1;
                    break;
            }

            if (deltax * deltax + deltay * deltay > 0)
            {
                deltax = deltax * this.ActualModel.PlotArea.Width * this.KeyboardPanHorizontalStep;
                deltay = deltay * this.ActualModel.PlotArea.Height * this.KeyboardPanVerticalStep;

                // small steps if the user is pressing control
                if (control)
                {
                    deltax *= 0.2;
                    deltay *= 0.2;
                }

                this.PanAll(deltax, deltay);
                // e.Handled = true;
            }

            if (Math.Abs(zoom) > 1e-8)
            {
                if (control)
                {
                    zoom *= 0.2;
                }

                this.ZoomAllAxes(1 + zoom * 0.12);
                // e.Handled = true;
            }

            if (control && alt && this.ActualModel != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.R:
                        this.SetClipboardText(this.ActualModel.CreateTextReport());
                        break;
                    case Keys.C:
                        this.SetClipboardText(this.ActualModel.ToCode());
                        break;
                    case Keys.X:

                        // this.SetClipboardText(this.ActualModel.ToXml());
                        break;
                }
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

            if (this.mouseManipulator != null)
            {
                return;
            }

            this.Focus();
            this.Capture = true;
            this.mouseManipulator = this.GetManipulator(e);

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Started(this.CreateManipulationEventArgs(e));
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
            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Delta(this.CreateManipulationEventArgs(e));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Completed(this.CreateManipulationEventArgs(e));
            }

            this.mouseManipulator = null;

            this.Capture = false;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            bool isControlDown = ModifierKeys == Keys.Control;
            var m = new ZoomStepManipulator(this, e.Delta * 0.001, isControlDown);
            m.Started(new ManipulationEventArgs(e.Location.ToScreenPoint()));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.
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
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Creates the manipulation event args.
        /// </summary>
        /// <param name="e">
        /// The MouseEventArgs instance containing the event data.
        /// </param>
        /// <returns>
        /// A manipulation event args object.
        /// </returns>
        private ManipulationEventArgs CreateManipulationEventArgs(MouseEventArgs e)
        {
            return new ManipulationEventArgs(e.Location.ToScreenPoint());
        }

        /// <summary>
        /// Gets the manipulator for the current mouse button and modifier keys.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
        /// <returns>
        /// A manipulator or null if no gesture was recognized.
        /// </returns>
        private ManipulatorBase GetManipulator(MouseEventArgs e)
        {
            bool control = (ModifierKeys & Keys.Control) == Keys.Control;
            bool shift = (ModifierKeys & Keys.Shift) == Keys.Shift;
            bool alt = (ModifierKeys & Keys.Alt) == Keys.Alt;

            bool lmb = e.Button == MouseButtons.Left;
            bool rmb = e.Button == MouseButtons.Right;
            bool mmb = e.Button == MouseButtons.Middle;
            bool x1b = e.Button == MouseButtons.XButton1;
            bool x2b = e.Button == MouseButtons.XButton2;

            // MMB / control RMB / control+alt LMB
            if (mmb || (control && rmb) || (control && alt && lmb))
            {
                if (e.Clicks == 2)
                {
                    return new ResetManipulator(this);
                }

                return new ZoomRectangleManipulator(this);
            }

            // Right mouse button / alt+left mouse button
            if (rmb || (lmb && alt))
            {
                return new PanManipulator(this);
            }

            // Left mouse button
            if (lmb)
            {
                return new TrackerManipulator(this) { Snap = !control, PointsOnly = shift };
            }

            // XButtons are zoom-stepping
            if (x1b || x2b)
            {
                double d = x1b ? 0.05 : -0.05;
                return new ZoomStepManipulator(this, d, control);
            }

            return null;
        }

        /// <summary>
        /// The set clipboard text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
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

        #endregion
    }
}