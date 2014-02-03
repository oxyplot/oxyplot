// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a control that displays a plot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.XamarinIOS
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    
    /// <summary>
    /// Represents a control that displays a plot.
    /// </summary>
    [Serializable]
    public class Plot : IPlotControl
    {
        /// <summary>
        /// The is model invalidated.
        /// </summary>
        private bool isModelInvalidated;

        /// <summary>
        /// The model.
        /// </summary>
        private PlotModel model;

        /// <summary>
        /// The mouse manipulator.
        /// </summary>
        private ManipulatorBase mouseManipulator;

        /// <summary>
        /// The update data.
        /// </summary>
        private bool updateData = true;

        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private Rectangle zoomRectangle;
        private GraphicsRenderContext rc;

        /// <summary>
        /// Initializes a new instance of the <see cref = "Plot" /> class.
        /// </summary>
        public Plot()
        {
            this.DoubleBuffered = true;
            this.Model = new PlotModel();
            this.rc = new GraphicsRenderContext(); // e.ClipRectangle
        }

        /// <summary>
        /// Gets the actual model.
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
        /// Hides the tracker.
        /// </summary>
        public void HideTracker()
        {
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            this.zoomRectangle = Rectangle.Empty;
            this.Invalidate();
        }

        /// <summary>
        /// Invalidates the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
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
        /// Refresh the plot immediately (blocking UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
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
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">
        /// The cursor type.
        /// </param>
        public void SetCursor(OxyCursor cursor)
        {
            switch (cursorType)
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
        /// Shows the tracker.
        /// </summary>
        /// <param name="data">The data.</param>
        public void ShowTracker(TrackerHitResult data)
        {

        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect rectangle)
        {
            this.zoomRectangle = new Rectangle((int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Width, (int)rectangle.Height);
            this.Invalidate();
        }

        /// <summary>
        /// Resets all axes.
        /// </summary>
        public void ResetAllAxes()
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.ResetAllAxes();
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Pans all axes.
        /// </summary>
        /// <param name="deltax">
        /// The horizontal delta.
        /// </param>
        /// <param name="deltay">
        /// The vertical delta.
        /// </param>
        public void PanAllAxes(double deltax, double deltay)
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.PanAllAxes(deltax, deltay);
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
            if (this.ActualModel != null)
            {
                this.ActualModel.ZoomAllAxes(delta);
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.mouseManipulator != null)
            {
                return;
            }

            this.Focus();
            this.Capture = true;

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseDown(this, args);
                if (args.Handled)
                {
                    return;
                }
            }

            this.mouseManipulator = this.GetManipulator(e);

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Started(this.CreateManipulationEventArgs(e));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseMove(this, args);
                if (args.Handled)
                {
                    return;
                }
            }

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
            this.Capture = false;

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseUp(this, args);
                if (args.Handled)
                {
                    return;
                }
            }

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Completed(this.CreateManipulationEventArgs(e));
            }

            this.mouseManipulator = null;
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

            rc.SetTarget(e.Graphics);
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
        /// <param name="e">The event args.</param>
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
    }
}