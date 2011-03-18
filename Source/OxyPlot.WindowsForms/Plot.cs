using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OxyPlot;

namespace Oxyplot.WindowsForms
{
    /// <summary>
    /// Represents a control that displays a plot.
    /// </summary>
    public sealed class Plot : Control, IPlot
    {
        public List<MouseAction> MouseActions { get; private set; }

        private readonly PanAction panAction;
        private readonly TrackerAction trackerAction;
        private readonly ZoomAction zoomAction;
        private Rectangle zoomRectangle;
        private bool isModelInvalidated;

        public Plot()
        {
            DoubleBuffered = true;
            Model = new PlotModel();

            panAction = new PanAction(this);
            zoomAction = new ZoomAction(this);
            trackerAction = new TrackerAction(this);

            MouseActions = new List<MouseAction> { panAction, zoomAction, trackerAction };
        }

        private PlotModel model;

        [Browsable(false), DefaultValue(null)]
        public PlotModel Model
        {
            get { return model; }
            set
            {
                model = value;
                Refresh();
            }
        }

        public override void Refresh()
        {
            if (model != null)
                model.UpdateData();
            base.Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            InvalidatePlot();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (isModelInvalidated)
            {
                Model.UpdateData();
                isModelInvalidated = false;
            }

            var rc = new GraphicsRenderContext(e.Graphics, Width, Height); // e.ClipRectangle
            if (model != null)
                model.Render(rc);
            if (zoomRectangle != Rectangle.Empty)
            {
                using (var zoomBrush = new SolidBrush(Color.FromArgb(0x40, 0xFF, 0xFF, 0x00)))
                using (var zoomPen = new Pen(Color.Black))
                {
                    zoomPen.DashPattern = new float[] { 3, 1 };
                    e.Graphics.FillRectangle(zoomBrush, zoomRectangle);
                    e.Graphics.DrawRectangle(zoomPen, zoomRectangle);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.A)
            {
                ZoomAll();
            }
        }

        public void ZoomAll()
        {
            foreach (var a in Model.Axes)
                a.Reset();
            Refresh();
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Focus();
            Capture = true;

            bool control = Control.ModifierKeys == Keys.Control;
            bool shift = Control.ModifierKeys == Keys.Shift;
            bool alt = Control.ModifierKeys == Keys.Alt;

            var button = OxyMouseButton.Left;
            if (e.Button == MouseButtons.Middle)
                button = OxyMouseButton.Middle;
            if (e.Button == MouseButtons.Right)
                button = OxyMouseButton.Right;
            if (e.Button == MouseButtons.XButton1)
                button = OxyMouseButton.XButton1;
            if (e.Button == MouseButtons.XButton2)
                button = OxyMouseButton.XButton2;

            var p = new ScreenPoint(e.X, e.Y);
            foreach (var a in MouseActions)
                a.OnMouseDown(p, button, e.Clicks, control, shift, alt);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool control = Control.ModifierKeys == Keys.Control;
            bool shift = Control.ModifierKeys == Keys.Shift;
            bool alt = Control.ModifierKeys == Keys.Alt;
            var p = new ScreenPoint(e.X, e.Y);
            foreach (var a in MouseActions)
                a.OnMouseMove(p, control, shift, alt);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            foreach (var a in MouseActions)
                a.OnMouseUp();
            Capture = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            bool control = Control.ModifierKeys == Keys.Control;
            bool shift = Control.ModifierKeys == Keys.Shift;
            bool alt = Control.ModifierKeys == Keys.Alt;
            var p = new ScreenPoint(e.X, e.Y);
            foreach (var a in MouseActions)
                a.OnMouseWheel(p, e.Delta, control, shift, alt);
        }

        public void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis)
        {
            Model.GetAxesFromPoint(pt, out xaxis, out yaxis);
        }

        public ISeries GetSeriesFromPoint(ScreenPoint pt, double limit)
        {
            return Model.GetSeriesFromPoint(pt, limit);
        }

        public void RefreshPlot()
        {
            Refresh();
        }

        public void InvalidatePlot()
        {
            isModelInvalidated = true;
            Invalidate();
        }

        public void UpdateAxisTransforms()
        {
            Model.UpdateAxisTransforms();
        }

        public void Pan(IAxis axis, double x0, double x1)
        {
            axis.Pan(x0,x1);
        }

        public void Reset(IAxis axis)
        {
            axis.Reset();
        }

        public void Zoom(IAxis axis, double p1, double p2)
        {
            axis.Zoom(p1, p2);
        }

        public void ZoomAt(IAxis axis, double factor, double x)
        {
            axis.ZoomAt(factor, x);
        }

        public OxyRect GetPlotArea()
        {
            return Model.PlotArea;
        }

        public void ShowTracker(ISeries s, DataPoint dp)
        {
        }

        public void HideTracker()
        {
        }

        public void ShowZoomRectangle(OxyRect r)
        {
            zoomRectangle = new Rectangle((int)r.Left, (int)r.Top, (int)r.Width, (int)r.Height);
            Invalidate();
        }

        public void HideZoomRectangle()
        {
            zoomRectangle = Rectangle.Empty;
            Invalidate();
        }
    }
}