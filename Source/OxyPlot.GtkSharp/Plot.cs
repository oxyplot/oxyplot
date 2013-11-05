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

namespace OxyPlot.GtkSharp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Gtk;
    using Gdk;
    using Cairo;

    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Represents a control that displays a plot.
    /// </summary>
    [Serializable]
    public class Plot : DrawingArea, IPlotControl
    {
        /// <summary>
        /// The category for the properties of this control.
        /// </summary>
        private const string OxyPlotCategory = "OxyPlot";

        /// <summary>
        /// The invalidate lock.
        /// </summary>
        private readonly object invalidateLock = new object();

        /// <summary>
        /// The model lock.
        /// </summary>
        private readonly object modelLock = new object();

        /// <summary>
        /// The rendering lock.
        /// </summary>
        private readonly object renderingLock = new object();

        /// <summary>
        /// The render context.
        /// </summary>
        private readonly GraphicsRenderContext renderContext;

        /// <summary>
        /// The current model (holding a reference to this plot control).
        /// </summary>
        [NonSerialized]
        private PlotModel currentModel;

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
        [NonSerialized]
        private ManipulatorBase mouseManipulator;

        /// <summary>
        /// The update data flag.
        /// </summary>
        private bool updateDataFlag = true;

        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private OxyRect? zoomRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plot"/> class.
        /// </summary>
        public Plot()
        {
            this.renderContext = new GraphicsRenderContext();

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            this.DoubleBuffered = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            this.KeyboardPanHorizontalStep = 0.1;
            this.KeyboardPanVerticalStep = 0.1;
            this.PanCursor = new Cursor(CursorType.Hand1);
            this.ZoomRectangleCursor = new Cursor(CursorType.Sizing); //  Cursors.SizeNWSE;
            this.ZoomHorizontalCursor = new Cursor(CursorType.SbHDoubleArrow);
            this.ZoomVerticalCursor = new Cursor(CursorType.SbVDoubleArrow);
            this.AddEvents((int)(EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.KeyPressMask | EventMask.PointerMotionMask));
            this.CanFocus = true;
        }

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value> The actual model. </value>
        public PlotModel ActualModel
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Gets or sets the keyboard pan horizontal step.
        /// </summary>
        /// <value> The keyboard pan horizontal step. </value>
        [Category(OxyPlotCategory)]
        public double KeyboardPanHorizontalStep { get; set; }

        /// <summary>
        /// Gets or sets the keyboard pan vertical step.
        /// </summary>
        /// <value> The keyboard pan vertical step. </value>
        [Category(OxyPlotCategory)]
        public double KeyboardPanVerticalStep { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Category(OxyPlotCategory)]
        public PlotModel Model
        {
            get
            {
                return this.model;
            }

            set
            {
                if (this.model != value)
                {
                    this.model = value;
                    this.OnModelChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the pan cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor PanCursor { get; set; }

        /// <summary>
        /// Gets or sets the horizontal zoom cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor ZoomHorizontalCursor { get; set; }

        /// <summary>
        /// Gets or sets the rectangle zoom cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor ZoomRectangleCursor { get; set; }

        /// <summary>
        /// Gets or sets the vertical zoom cursor.
        /// </summary>
        [Category(OxyPlotCategory)]
        public Cursor ZoomVerticalCursor { get; set; }

        /// <summary>
        /// Gets the axes from a point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="xaxis">
        /// The x axis.
        /// </param>
        /// <param name="yaxis">
        /// The y axis.
        /// </param>
        public void GetAxesFromPoint(ScreenPoint point, out Axis xaxis, out Axis yaxis)
        {
            if (this.Model == null)
            {
                xaxis = null;
                yaxis = null;
                return;
            }

            this.Model.GetAxesFromPoint(point, out xaxis, out yaxis);
        }

        /// <summary>
        /// Gets the series from a point.
        /// </summary>
        /// <param name="point">
        /// The point (screen coordinates).
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <returns>
        /// The series.
        /// </returns>
        public Series GetSeriesFromPoint(ScreenPoint point, double limit)
        {
            if (this.Model == null)
            {
                return null;
            }

            return this.Model.GetSeriesFromPoint(point, limit);
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
            this.zoomRectangle = null;
            this.QueueDraw();
        }

        /// <summary>
        /// Invalidates the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
        public void InvalidatePlot(bool updateData)
        {
            lock (this.invalidateLock)
            {
                this.isModelInvalidated = true;
                this.updateDataFlag = this.updateDataFlag || updateData;
            }

            this.QueueDraw();
        }

        /// <summary>
        /// Called when the Model property has been changed.
        /// </summary>
        public void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.currentModel != null)
                {
                    this.currentModel.AttachPlotControl(null);
                }

                if (this.Model != null)
                {
                    if (this.Model.PlotControl != null)
                    {
                        throw new InvalidOperationException(
                            "This PlotModel is already in use by some other plot control.");
                    }

                    this.Model.AttachPlotControl(this);
                    this.currentModel = this.Model;
                }
            }

            this.InvalidatePlot(true);
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="previousPoint">The x0.</param>
        /// <param name="x1">The x1.</param>
        public void Pan(Axis axis, ScreenPoint previousPoint, ScreenPoint x1)
        {
            axis.Pan(previousPoint, x1);
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
        public void PanAll(double deltax, double deltay)
        {
            foreach (var a in this.ActualModel.Axes)
            {
                a.Pan(a.IsHorizontal() ? deltax : deltay);
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Refresh the plot immediately (blocking UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
        public void RefreshPlot(bool updateData)
        {
            lock (this.invalidateLock)
            {
                this.isModelInvalidated = true;
                this.updateDataFlag = this.updateDataFlag || updateData;
            }

            this.QueueDraw();
            this.GdkWindow.ProcessUpdates(false);
        }

        /// <summary>
        /// Resets the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        public void Reset(Axis axis)
        {
            axis.Reset();
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">
        /// The cursor type.
        /// </param>
        public void SetCursorType(OxyPlot.CursorType cursorType)
        {
            switch (cursorType)
            {
                case OxyPlot.CursorType.Pan:
                    base.GdkWindow.Cursor = this.PanCursor;
                    break;
                case OxyPlot.CursorType.ZoomRectangle:
                    base.GdkWindow.Cursor = this.ZoomRectangleCursor;
                    break;
                case OxyPlot.CursorType.ZoomHorizontal:
                    base.GdkWindow.Cursor = this.ZoomHorizontalCursor;
                    break;
                case OxyPlot.CursorType.ZoomVertical:
                    base.GdkWindow.Cursor = this.ZoomVerticalCursor;
                    break;
                default:
                    base.GdkWindow.Cursor = new Cursor(CursorType.Arrow);
                    break;
            }
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="data">The data.</param>
        public void ShowTracker(TrackerHitResult data)
        {
            // not implemented for GtkSharp
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect rectangle)
        {
            this.zoomRectangle = rectangle;
            this.QueueDraw();
        }

        /// <summary>
        /// Zooms the specified axis to the specified values.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="newMinimum">The new minimum value.</param>
        /// <param name="newMaximum">The new maximum value.</param>
        public void Zoom(Axis axis, double newMinimum, double newMaximum)
        {
            axis.Zoom(newMinimum, newMaximum);
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Resets all axes.
        /// </summary>
        public void ResetAllAxes()
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
        /// Zooms at the specified position.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="factor">The zoom factor.</param>
        /// <param name="x">The position to zoom at.</param>
        public void ZoomAt(Axis axis, double factor, double x = double.NaN)
        {
            if (double.IsNaN(x))
            {
                double sx = (axis.Transform(axis.ActualMaximum) + axis.Transform(axis.ActualMinimum)) * 0.5;
                x = axis.InverseTransform(sx);
            }

            axis.ZoomAt(factor, x);
            this.InvalidatePlot(false);
        }

        protected override bool OnButtonPressEvent(EventButton e)
        {
            if (this.mouseManipulator != null)
            {
                return true;
            }

            this.GrabFocus(); // .HasFocus = true;

            //  this.Capture = true; // TODO

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseDown(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            this.mouseManipulator = this.GetManipulator(e);

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Started(this.CreateManipulationEventArgs(e));
            }
            return true;
        }

        /// <summary>
        /// Called on mouse move event.
        /// </summary>
        /// <param name="e">An instance that contains the event data.</param>
        protected override bool OnMotionNotifyEvent(EventMotion e)
        {

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseMove(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Delta(this.CreateManipulationEventArgs(e));
            }
            return true;
        }

        /// <summary>
        /// Called on button release event.
        /// </summary>
        /// <param name="e">
        /// An instance that contains the event data.
        /// </param>
        protected override bool OnButtonReleaseEvent(EventButton e)
        {
            // this.Capture = false; // TODO
            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseUp(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Completed(this.CreateManipulationEventArgs(e));
            }

            this.mouseManipulator = null;
            return true;

        }

        /// <summary>
        /// Called on MouseWheel  event.
        /// </summary>
        /// <param name="e">
        /// An instance that contains the event data.
        /// </param>
        /* TODO */
        /*
        protected override void OnMouseWheel( MouseEventArgs e)
        {
            bool isControlDown = ModifierKeys == Keys.Control;
            var m = new ZoomStepManipulator(this, e.Delta * 0.001, isControlDown);
            m.Started(new ManipulationEventArgs(e.Location.ToScreenPoint()));
        }
         */

        /// <summary>
        /// Called on an expose (paint) event.
        /// </summary>
        /// <param name="e">
        /// An instance that contains the event data.
        /// </param>
        protected override bool OnExposeEvent(EventExpose e)
        {
			using (Cairo.Context g = Gdk.CairoHelper.Create(e.Window)) {
				try {
					lock (this.invalidateLock) {
						if (this.isModelInvalidated) {
							if (this.model != null) {
								this.model.Update (this.updateDataFlag);
								this.updateDataFlag = false;
							}

							this.isModelInvalidated = false;
						}
					}

					lock (this.renderingLock) {
						this.renderContext.SetGraphicsTarget(g);
						if (this.model != null) {
							int width;
							int height;
							this.GetSizeRequest (out width, out height);
							this.model.Render(this.renderContext, width, height);
						}

						if (this.zoomRectangle.HasValue) {
							// this.renderContext.DrawRectangle(zoomRectangle.Value, OxyColor.FromArgb(0x40, 0xFF, 0xFF, 0x00), OxyColors.Transparent, 1.0);
						}
					}
				} catch (Exception paintException) {
					var trace = new StackTrace (paintException);
					Debug.WriteLine (paintException);
					Debug.WriteLine (trace);
					//using (var font = new Font("Arial", 10))
					{
						//int width; int height;
						//this.GetSizeRequest(out width, out height);
						Debug.Assert (false, "OxyPlot paint exception: " + paintException.Message);
						//g.ResetTransform();
						//g.DrawString(, font, Brushes.Red, width / 2, height / 2, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
					}
            
				}
			}
            return true;
        }

        /// <summary>
        /// Called on KeyPress event.
        /// </summary>
        /// <param name="e">
        /// An instance that contains the event data.
        /// </param>
        protected override bool OnKeyPressEvent(EventKey e)
        {
            bool handled = false;
            if (e.Key == Gdk.Key.a)
            {
                this.ResetAllAxes();
            }

            bool control = (e.State & ModifierType.ControlMask) != 0;
            bool alt = (e.State & ModifierType.Mod1Mask) != 0;

            double deltax = 0;
            double deltay = 0;
            double zoom = 0;
            switch (e.Key)
            {
                case Gdk.Key.Up:
                    deltay = -1;
                    break;
                case Gdk.Key.Down:
                    deltay = 1;
                    break;
                case Gdk.Key.Left:
                    deltax = -1;
                    break;
                case Gdk.Key.Right:
                    deltax = 1;
                    break;
                case Gdk.Key.plus:
                case Gdk.Key.KP_Add:
                case Gdk.Key.Page_Up:
                    zoom = 1;
                    break;
                case Gdk.Key.minus:
                case Gdk.Key.KP_Subtract:
                case Gdk.Key.Page_Down:
                    zoom = -1;
                    break;
            }

            if ((deltax * deltax) + (deltay * deltay) > 0)
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

                handled = true;
            }

            if (Math.Abs(zoom) > 1e-8)
            {
                if (control)
                {
                    zoom *= 0.2;
                }

                this.ZoomAllAxes(1 + (zoom * 0.12));

                handled = true;
            }

            if (control && alt && this.ActualModel != null)
            {
                switch (e.Key)
                {
                    case Gdk.Key.r:
                        this.SetClipboardText(this.ActualModel.CreateTextReport());
                        handled = true;
                        break;
                    case Gdk.Key.c:
                        this.SetClipboardText(this.ActualModel.ToCode());
                        handled = true;
                        break;
                    case Gdk.Key.x:

                        // this.SetClipboardText(this.ActualModel.ToXml());
                        break;
                }
            }
            return handled;
        }

        /// <summary>
        /// Converts the changed button.
        /// </summary>
        /// <param name="e">
        /// The instance containing the event data.
        /// </param>
        /// <returns>
        /// The mouse button.
        /// </returns>
        private static OxyMouseButton ConvertChangedButton(EventButton e)
        {
            switch (e.Button)
            {
                case 1:
                    return OxyMouseButton.Left;
                case 2:
                    return OxyMouseButton.Middle;
                case 3:
                    return OxyMouseButton.Right;
                case 4:
                    return OxyMouseButton.XButton1;
                case 5:
                    return OxyMouseButton.XButton2;
            }

            return OxyMouseButton.Left;
        }


        /// <summary>
        /// Creates the mouse event arguments.
        /// </summary>
        /// <param name="e">
        /// The instance containing the event data.
        /// </param>
        /// <returns>
        /// Mouse event arguments.
        /// </returns>
        private OxyMouseEventArgs CreateMouseEventArgs(EventButton e)
        {
            return new OxyMouseEventArgs
            {
                ChangedButton = ConvertChangedButton(e),
                Position = new ScreenPoint(e.X, e.Y),
                IsShiftDown = (e.State & ModifierType.ShiftMask) != 0,
                IsControlDown = (e.State & ModifierType.ControlMask) != 0,
                IsAltDown = (e.State & ModifierType.Mod1Mask) != 0,
            };
        }

        private OxyMouseEventArgs CreateMouseEventArgs(EventMotion e)
        {
            return new OxyMouseEventArgs
            {
                ChangedButton = OxyMouseButton.None,
                Position = new ScreenPoint(e.X, e.Y),
                IsShiftDown = (e.State & ModifierType.ShiftMask) != 0,
                IsControlDown = (e.State & ModifierType.ControlMask) != 0,
                IsAltDown = (e.State & ModifierType.Mod1Mask) != 0,
            };
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
        private ManipulationEventArgs CreateManipulationEventArgs(EventButton e)
        {
            return new ManipulationEventArgs(new ScreenPoint(e.X, e.Y));
        }

        private ManipulationEventArgs CreateManipulationEventArgs(EventMotion e)
        {
            return new ManipulationEventArgs(new ScreenPoint(e.X, e.Y));
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
        private ManipulatorBase GetManipulator(EventButton e)
        {
            bool control = (e.State & ModifierType.ControlMask) != 0;
            bool shift = (e.State & ModifierType.ShiftMask) != 0;
            bool alt = (e.State & ModifierType.Mod1Mask) != 0;

            bool lmb = e.Button == 1;
            bool mmb = e.Button == 2;
            bool rmb = e.Button == 3;
            bool xb1 = e.Button == 4;
            bool xb2 = e.Button == 5;
            bool doubleClick = (e.Type == EventType.TwoButtonPress);
            //bool singleClick = (e.Type == EventType.ButtonPress);

            // MMB / control RMB / control+alt LMB
            if (mmb || (control && rmb) || (control && alt && lmb))
            {
                if (doubleClick)
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
            if (xb1 || xb2)
            {
                double d = xb1 ? 0.05 : -0.05;
                return new ZoomStepManipulator(this, d, control);
            }

            return null;
        }

        /// <summary>
        /// Sets the clipboard text.
        /// </summary>
        /// <param name="text">The text.</param>
        private void SetClipboardText(string text)
        {
            try
            {
                // todo: can't get the following solution to work
                // http://stackoverflow.com/questions/5707990/requested-clipboard-operation-did-not-succeed
                base.GetClipboard(Gdk.Selection.Clipboard).Text = text;
            }
            catch (ExternalException)
            {
                // Requested Clipboard operation did not succeed.
                //MessageBox.Show(this, ee.Message, "OxyPlot");
            }
        }
    }
}