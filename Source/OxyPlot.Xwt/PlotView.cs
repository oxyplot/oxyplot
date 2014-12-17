// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xwt;
using Xwt.Drawing;

namespace OxyPlot.Xwt
{

    /// <summary>
    /// A Widget that displays a <see cref="PlotModel" />.
    /// </summary>
    [Serializable]
    public class PlotView : Canvas, IPlotView
	{

		/// <summary>
		/// The mouse hit tolerance.
		/// </summary>
		const double MouseHitTolerance = 10;

        /// <summary>
        /// The invalidate lock.
        /// </summary>
        readonly object invalidateLock = new object ();

        /// <summary>
        /// The model lock.
        /// </summary>
        readonly object modelLock = new object ();

        /// <summary>
        /// The render context.
        /// </summary>
        readonly GraphicsRenderContext renderContext;

        /// <summary>
        /// The current model (holding a reference to this plot view).
        /// </summary>
        [NonSerialized]
        PlotModel currentModel;

        /// <summary>
        /// The is model invalidated.
        /// </summary>
        bool isModelInvalidated;

        /// <summary>
        /// The model.
        /// </summary>
        PlotModel model;

        /// <summary>
        /// The update data flag.
        /// </summary>
        bool updateDataFlag = true;

        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        OxyRect? zoomRectangle;

        /// <summary>
        /// The default controller
        /// </summary>
        IPlotController defaultController;

		/// <summary>
		/// The tracker definitions.
		/// </summary>
		Dictionary<string, TrackerSettings> trackerDefinitions;

		/// <summary>
		/// The tracker lock.
		/// </summary>
		readonly object trackerLock = new object ();

		/// <summary>
		/// The tracker hit result.
		/// </summary>
		TrackerHitResult actualTrackerHitResult;

		/// <summary>
		/// The tracker popover.
		/// </summary>
		Popover trackerPopover;

		/// <summary>
		/// The tracker label.
		/// </summary>
		Label lblTrackerText = new Label ();

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Xwt.PlotView"/> class.
        /// </summary>
        public PlotView ()
        {
            renderContext = new GraphicsRenderContext ();
			trackerDefinitions = new Dictionary<string, TrackerSettings> ();
			DefaultTrackerSettings = new TrackerSettings ();
			ZoomRectangleColor = OxyColor.FromArgb (0x40, 0xFF, 0xFF, 0x00);
			ZoomRectangleBorderColor = OxyColors.Transparent;
			ZoomRectangleBorderWidth = 1.0;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public PlotModel Model {
            get {
                return model;
            }

            set {
                if (model != value) {
                    model = value;
                    OnModelChanged ();
                }
            }
        }

        /// <summary>
        /// Gets the actual <see cref="PlotModel" /> of the control.
        /// </summary>
        Model IView.ActualModel {
            get {
                if (model == null)
                    model = new PlotModel ();
                return model;
            }
        }

        /// <summary>
        /// Gets the actual <see cref="OxyPlot.Model" /> of the control.
        /// </summary>
        public PlotModel ActualModel {
            get {
                return Model;
            }
        }

        /// <summary>
        /// Gets the actual controller.
        /// </summary>
        /// <value>
        /// The actual <see cref="IController" />.
        /// </value>
        IController IView.ActualController {
            get {
                return ActualController;
            }
        }

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        public OxyRect ClientArea {
            get {
                return new OxyRect (0, 0, Bounds.Width, Bounds.Height);
            }
        }

        /// <summary>
        /// Gets the actual controller.
        /// </summary>
        /// <value>
        /// The actual <see cref="IController" />.
        /// </value>
        public IController ActualController {
            get {
                return Controller ?? (defaultController ?? (defaultController = new PlotController ()));
            }
        }

        /// <summary>
        /// Gets or sets the plot controller.
        /// </summary>
        /// <value>The controller.</value>
        public IPlotController Controller { get; set; }

		/// <summary>
		/// Gets the tracker definitions.
		/// </summary>
		/// <value>The tracker definitions mapping.</value>
		/// <remarks>The tracker definitions make it possible to show different trackers for different series.
		/// The dictionary key must match the <see cref="OxyPlot.Series.Series.TrackerKey" /> property. If
		/// the dictionary does not contain a matching key, the <see cref="DefaultTrackerSettings"/> tracker
		/// configuration is used.</remarks>
		public Dictionary<string, TrackerSettings> TrackerDefinitions {
			get { return trackerDefinitions; }
		}

		/// <summary>
		/// Gets or sets the default tracker settings.
		/// </summary>
		/// <value>The default tracker settings.</value>
		/// <remarks>The default thracker settings to be used, if <see cref="TrackerDefinitions"/> does not
		/// contain a definition for the matching <see cref="OxyPlot.Series.Series.TrackerKey" />.</remarks>
		public TrackerSettings DefaultTrackerSettings { get; set; }

		/// <summary>
		/// Gets or sets the color of the zoom rectangle.
		/// </summary>
		/// <value>The color of the zoom rectangle.</value>
		public OxyColor ZoomRectangleColor { get; set; }

		/// <summary>
		/// Gets or sets the color of the zoom rectangle border.
		/// </summary>
		/// <value>The color of the zoom rectangle border.</value>
		public OxyColor ZoomRectangleBorderColor { get; set; }

		/// <summary>
		/// Gets or sets the width of the zoom rectangle border.
		/// </summary>
		/// <value>The width of the zoom rectangle border.</value>
		public double ZoomRectangleBorderWidth { get; set; }

		bool showDynamicTooltips = true;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="OxyPlot.Xwt.PlotView"/> shows dynamic tooltips
		/// for plot elements with <see cref="OxyPlot.PlotElement.ToolTip"/> property set.
		/// </summary>
		/// <value><c>true</c> to show dynamic tooltips; otherwise, <c>false</c>.</value>
		public bool ShowDynamicTooltips {
			get {
				return showDynamicTooltips;
			}
			set {
				showDynamicTooltips = value;
			}
		}

        /// <summary>
        /// Invalidates the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
        public void InvalidatePlot (bool updateData = true)
        {
            lock (invalidateLock) {
                isModelInvalidated = true;
                updateDataFlag = updateDataFlag || updateData;
            }

            QueueDraw ();
        }

        /// <summary>
        /// Called when the Model property has been changed.
        /// </summary>
        void OnModelChanged ()
        {
            lock (modelLock) {
                if (currentModel != null) {
                    ((IPlotModel)currentModel).AttachPlotView (null);
                }

                if (Model != null) {
                    if (Model.PlotView != null) {
                        throw new InvalidOperationException (
                            "This PlotModel is already in use by some other plot view.");
                    }

                    ((IPlotModel)Model).AttachPlotView (this);
                    currentModel = Model;
                }
            }

            InvalidatePlot ();
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType (CursorType cursorType)
        {
            Cursor = cursorType.ToXwtCursorType ();
        }


        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">The data.</param>
        public virtual void ShowTracker (TrackerHitResult trackerHitResult)
		{
			if (trackerHitResult == null) {
				HideTracker ();
				return;
			}

			if (trackerPopover != null)
				HideTracker();

			lock (trackerLock) {
				actualTrackerHitResult = trackerHitResult;
				trackerPopover = new Popover (lblTrackerText);
				// TODO: background color, when supported by xwt
				lblTrackerText.Text = trackerHitResult.Text;

				var position = new Rectangle (trackerHitResult.Position.X, trackerHitResult.Position.Y, 1, 1);
				//TODO: horizontal flip, needs xwt fix (https://github.com/mono/xwt/pull/362)
				//if (trackerHitResult.Position.Y <= Bounds.Height / 2)
					trackerPopover.Show (Popover.Position.Top, this, position);
				//else
				//	trackerPopover.Show (Popover.Position.Bottom, this, position);
			}
			QueueDraw ();
        }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public virtual void HideTracker ()
		{
			lock (trackerLock) {
				actualTrackerHitResult = null;
				if (trackerPopover != null) {
					trackerPopover.Hide ();
					trackerPopover.Content = null;
					trackerPopover.Dispose ();
					trackerPopover = null;
				}
			}
			QueueDraw ();
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        public virtual void ShowZoomRectangle (OxyRect rectangle)
        {
            zoomRectangle = rectangle;
            QueueDraw ();
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public virtual void HideZoomRectangle ()
        {
            zoomRectangle = null;
            QueueDraw ();
        }

        /// <summary>
        /// Sets the clipboard text.
        /// </summary>
        /// <param name="text">The text.</param>
        void IPlotView.SetClipboardText (string text)
        {
            Clipboard.SetText (text);
        }

        /// <summary>
        /// Pans all axes.
        /// </summary>
        /// <param name="dx">Dx.</param>
        /// <param name="dy">Dy.</param>
        public void PanAllAxes (double dx, double dy)
        {
            if (ActualModel != null)
                ActualModel.PanAllAxes (dx, dy);

            InvalidatePlot (false);
        }

        /// <summary>
        /// Zooms all axes.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        public void ZoomAllAxes (double factor)
        {
            if (ActualModel != null)
                ActualModel.ZoomAllAxes (factor);

            InvalidatePlot (false);
        }

        /// <summary>
        /// Resets all axes.
        /// </summary>
        public void ResetAllAxes ()
        {
            if (ActualModel != null)
                ActualModel.ResetAllAxes ();

            InvalidatePlot (false);
        }

        /// <summary>
        /// Called when the mouse button is pressed.
        /// </summary>
        /// <param name="args">An instance that contains the event data.</param>
        protected override void OnButtonPressed (ButtonEventArgs args)
        {
            base.OnButtonPressed (args);
            if (args.Handled)
                return;
            args.Handled = ActualController.HandleMouseDown (this,
                                                    args.ToOxyMouseDownEventArgs ());
        }

        /// <summary>
        /// Called on mouse move events.
        /// </summary>
        /// <param name="args">An instance that contains the event data.</param>
        protected override void OnMouseMoved (MouseMovedEventArgs args)
        {
            base.OnMouseMoved (args);
            if (args.Handled)
                return;

			if (ShowDynamicTooltips) {
				string tooltip = null;
				var hitArgs = new HitTestArguments (new ScreenPoint (args.X, args.Y), MouseHitTolerance);
				foreach (var result in ActualModel.HitTest(hitArgs)) {
					var plotElement = result.Element as PlotElement;
					if (plotElement != null && !String.IsNullOrEmpty (plotElement.ToolTip)) {
						tooltip = String.IsNullOrEmpty (tooltip) ? plotElement.ToolTip : tooltip + Environment.NewLine + plotElement.ToolTip;
					}
				}
				TooltipText = tooltip;
			}

            args.Handled = ActualController.HandleMouseMove (this,
                                                    args.ToOxyMouseEventArgs ());
        }

        /// <summary>
        /// Called when the mouse button is released.
        /// </summary>
        /// <param name="args">An instance that contains the event data.</param>
        protected override void OnButtonReleased (ButtonEventArgs args)
        {
            base.OnButtonReleased (args);
            if (args.Handled)
                return;
            args.Handled = ActualController.HandleMouseUp (this,
                                                  args.ToOxyMouseUpEventArgs ());
        }

        /// <summary>
        /// Called when the mouse wheel is scrolled.
        /// </summary>
        /// <param name="args">An instance that contains the event data.</param>
        protected override void OnMouseScrolled (MouseScrolledEventArgs args)
        {
            base.OnMouseScrolled (args);
            if (args.Handled)
                return;
            args.Handled = ActualController.HandleMouseWheel (this,
                                                     args.ToOxyMouseWheelEventArgs ());
        }

        /// <summary>
        /// Called when the mouse enters the widget.
        /// </summary>
        /// <param name="args">An instance that contains the event data.</param>
        protected override void OnMouseEntered (EventArgs args)
        {
            base.OnMouseEntered (args);
            ActualController.HandleMouseEnter (this, new OxyMouseEventArgs ());
        }

        /// <summary>
        /// Called when the mouse leaves the widget.
        /// </summary>
        /// <param name="args">An instance that contains the event data.</param>
        protected override void OnMouseExited (EventArgs args)
        {
            base.OnMouseExited (args);
            ActualController.HandleMouseLeave (this, new OxyMouseEventArgs ());
        }

        /// <summary>
        /// Called on KeyPress event.
        /// </summary>
        /// <param name="args">An instance that contains the event data.</param>
        protected override void OnKeyPressed (KeyEventArgs args)
        {
            base.OnKeyPressed (args);
            if (args.Handled)
                return;
            args.Handled = ActualController.HandleKeyDown (this,
                                                  args.ToOxyKeyEventArgs ());
        }

        /// <summary>
        /// Called when the widget needs to be redrawn
        /// </summary>
        /// <param name="ctx">Drawing context</param>
        /// <param name="dirtyRect">Dirty rect.</param>
        protected override void OnDraw (Context ctx, Rectangle dirtyRect)
        {
            try {
                lock (invalidateLock) {
                    if (isModelInvalidated) {
                        if (model != null) {
                            ((IPlotModel)model).Update (updateDataFlag);
                            updateDataFlag = false;
                        }
                        isModelInvalidated = false;
                    }
                }

                renderContext.Context = ctx;
                if (model != null) {
                    if (!model.Background.IsUndefined ())
                        renderContext.DrawRectangle (Bounds.ToOxyRect (),
                                   model.Background,
                                   OxyColors.Undefined,
                                   0);

                    ((IPlotModel)model).Render (renderContext, Bounds.Width, Bounds.Height);
                }

                if (zoomRectangle.HasValue) {
                    renderContext.DrawRectangle (zoomRectangle.Value,
					                             ZoomRectangleColor,
					                             ZoomRectangleBorderColor,
					                             ZoomRectangleBorderWidth);
                }

				if (actualTrackerHitResult != null) {
					var extents = actualTrackerHitResult.LineExtents;
					if (Math.Abs (extents.Width) < double.Epsilon) {
						extents.Left = actualTrackerHitResult.XAxis.ScreenMin.X;
						extents.Right = actualTrackerHitResult.XAxis.ScreenMax.X;
					}
					if (Math.Abs (extents.Height) < double.Epsilon) {
						extents.Top = actualTrackerHitResult.YAxis.ScreenMin.Y;
						extents.Bottom = actualTrackerHitResult.YAxis.ScreenMax.Y;
					}

					var pos = actualTrackerHitResult.Position;

					var trackerSettings = DefaultTrackerSettings;
					if (actualTrackerHitResult.Series != null && !string.IsNullOrEmpty(actualTrackerHitResult.Series.TrackerKey))
						trackerSettings = trackerDefinitions[actualTrackerHitResult.Series.TrackerKey];

					if (trackerSettings.HorizontalLineVisible) {

						renderContext.DrawLine (
							new[] { new ScreenPoint(extents.Left, pos.Y), new ScreenPoint(extents.Right, pos.Y)},
							trackerSettings.HorizontalLineColor,
							trackerSettings.HorizontalLineWidth,
							trackerSettings.HorizontalLineActualDashArray,
							LineJoin.Miter,
							true);
					}
					if (trackerSettings.VerticalLineVisible) {
						renderContext.DrawLine (
							new[] { new ScreenPoint(pos.X, extents.Top), new ScreenPoint(pos.X, extents.Bottom)},
							trackerSettings.VerticalLineColor,
							trackerSettings.VerticalLineWidth,
							trackerSettings.VerticalLineActualDashArray,
							LineJoin.Miter,
							true);
					}
				}
            } catch (Exception paintException) {
                var trace = new StackTrace (paintException);
                Debug.WriteLine (paintException);
                Debug.WriteLine (trace);
            }
        }

        /// <summary>
        /// Renders the contents of the widget into an Image
        /// </summary>
        /// <returns> The new Xwt image.</returns>
        public Image ToXwtImage ()
        {
            return Toolkit.CurrentEngine.RenderWidget (this);
        }

        protected override void Dispose (bool disposing)
        {
			if (disposing) {
				renderContext.Dispose ();
				if (trackerPopover != null)
					trackerPopover.Dispose ();
			}
            base.Dispose (disposing);
        }
    }
}

