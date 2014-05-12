// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotCommands.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Defines common commands for the plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Defines common commands for the plots.
    /// </summary>
    public static class PlotCommands
    {
        /// <summary>
        /// Initializes static members of the <see cref="PlotCommands" /> class.
        /// </summary>
        static PlotCommands()
        {
            // commands that can be triggered from key events
            Reset = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandleReset(view));
            CopyTextReport = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => view.SetClipboardText(view.ActualModel.CreateTextReport()));
            CopyCode = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => view.SetClipboardText(view.ActualModel.ToCode()));

            // commands that can be triggered from mouse down events
            ResetAt = new DelegatePlotControllerCommand<OxyMouseEventArgs>((view, controller, args) => HandleReset(view));
            PanAt = new DelegatePlotControllerCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new PanManipulator(view), args));
            ZoomRectangle = new DelegatePlotControllerCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new ZoomRectangleManipulator(view), args));
            Track = new DelegatePlotControllerCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new TrackerManipulator(view) { Snap = false, PointsOnly = false }, args));
            SnapTrack = new DelegatePlotControllerCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new TrackerManipulator(view) { Snap = true, PointsOnly = false }, args));
            PointsOnlyTrack = new DelegatePlotControllerCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new TrackerManipulator(view) { Snap = false, PointsOnly = true }, args));
            ZoomWheel = new DelegatePlotControllerCommand<OxyMouseWheelEventArgs>((view, controller, args) => HandleZoomByWheel(view, args));
            ZoomWheelFine = new DelegatePlotControllerCommand<OxyMouseWheelEventArgs>((view, controller, args) => HandleZoomByWheel(view, args, 0.1));
            ZoomInAt = new DelegatePlotControllerCommand<OxyMouseEventArgs>((view, controller, args) => HandleZoomAt(view, args, 0.05));
            ZoomOutAt = new DelegatePlotControllerCommand<OxyMouseEventArgs>((view, controller, args) => HandleZoomAt(view, args, -0.05));

            // commands that can be triggered from mouse enter events
            HoverTrack = new DelegatePlotControllerCommand<OxyMouseEventArgs>((view, controller, args) => controller.AddHoverManipulator(view, new TrackerManipulator(view) { LockToInitialSeries = false, Snap = false, PointsOnly = false }, args));
            HoverSnapTrack = new DelegatePlotControllerCommand<OxyMouseEventArgs>((view, controller, args) => controller.AddHoverManipulator(view, new TrackerManipulator(view) { LockToInitialSeries = false, Snap = true, PointsOnly = false }, args));
            HoverPointsOnlyTrack = new DelegatePlotControllerCommand<OxyMouseEventArgs>((view, controller, args) => controller.AddHoverManipulator(view, new TrackerManipulator(view) { LockToInitialSeries = false, Snap = false, PointsOnly = true }, args));

            PanZoomByTouch = new DelegatePlotControllerCommand<OxyTouchEventArgs>((view, controller, args) => controller.AddTouchManipulator(view, new TouchManipulator(view), args));

            // commands that can be triggered from key events
            PanLeft = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, -0.1, 0));
            PanRight = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0.1, 0));
            PanUp = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, -0.1));
            PanDown = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, 0.1));
            PanLeftFine = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, -0.01, 0));
            PanRightFine = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0.01, 0));
            PanUpFine = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, -0.01));
            PanDownFine = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, 0.01));

            ZoomIn = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, 1));
            ZoomOut = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, -1));
            ZoomInFine = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, 0.1));
            ZoomOutFine = new DelegatePlotControllerCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, -0.1));
        }

        /// <summary>
        /// Gets the reset axes command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> Reset { get; private set; }

        /// <summary>
        /// Gets the reset axes command (for mouse events).
        /// </summary>
        public static IPlotControllerCommand<OxyMouseEventArgs> ResetAt { get; private set; }

        /// <summary>
        /// Gets the copy text report command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> CopyTextReport { get; private set; }

        /// <summary>
        /// Gets the copy code command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> CopyCode { get; private set; }

        /// <summary>
        /// Gets the pan/zoom touch command.
        /// </summary>
        public static IPlotControllerCommand<OxyTouchEventArgs> PanZoomByTouch { get; private set; }

        /// <summary>
        /// Gets the pan command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseDownEventArgs> PanAt { get; private set; }

        /// <summary>
        /// Gets the zoom rectangle command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseDownEventArgs> ZoomRectangle { get; private set; }

        /// <summary>
        /// Gets the zoom by mouse wheel command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseWheelEventArgs> ZoomWheel { get; private set; }

        /// <summary>
        /// Gets the fine-control zoom by mouse wheel command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseWheelEventArgs> ZoomWheelFine { get; private set; }

        /// <summary>
        /// Gets the tracker command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseDownEventArgs> Track { get; private set; }

        /// <summary>
        /// Gets the snap tracker command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseDownEventArgs> SnapTrack { get; private set; }

        /// <summary>
        /// Gets the points only tracker command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseDownEventArgs> PointsOnlyTrack { get; private set; }

        /// <summary>
        /// Gets the mouse hover tracker.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseEventArgs> HoverTrack { get; private set; }

        /// <summary>
        /// Gets the mouse hover snap tracker.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseEventArgs> HoverSnapTrack { get; private set; }

        /// <summary>
        /// Gets the mouse hover points only tracker.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseEventArgs> HoverPointsOnlyTrack { get; private set; }

        /// <summary>
        /// Gets the pan left command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanLeft { get; private set; }

        /// <summary>
        /// Gets the pan right command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanRight { get; private set; }

        /// <summary>
        /// Gets the pan up command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanUp { get; private set; }

        /// <summary>
        /// Gets the pan down command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanDown { get; private set; }

        /// <summary>
        /// Gets the fine control pan left command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanLeftFine { get; private set; }

        /// <summary>
        /// Gets the fine control pan right command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanRightFine { get; private set; }

        /// <summary>
        /// Gets the fine control pan up command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanUpFine { get; private set; }

        /// <summary>
        /// Gets the fine control pan down command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> PanDownFine { get; private set; }

        /// <summary>
        /// Gets the zoom in command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseEventArgs> ZoomInAt { get; private set; }

        /// <summary>
        /// Gets the zoom out command.
        /// </summary>
        public static IPlotControllerCommand<OxyMouseEventArgs> ZoomOutAt { get; private set; }

        /// <summary>
        /// Gets the zoom in command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> ZoomIn { get; private set; }

        /// <summary>
        /// Gets the zoom out command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> ZoomOut { get; private set; }

        /// <summary>
        /// Gets the fine control zoom in command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> ZoomInFine { get; private set; }

        /// <summary>
        /// Gets the fine control zoom out command.
        /// </summary>
        public static IPlotControllerCommand<OxyKeyEventArgs> ZoomOutFine { get; private set; }

        /// <summary>
        /// Handles the reset events.
        /// </summary>
        /// <param name="view">The view to reset.</param>
        private static void HandleReset(IPlotView view)
        {
            view.ActualModel.ResetAllAxes();
            view.InvalidatePlot(false);
        }

        /// <summary>
        /// Zooms the view by the specified factor at the position specified in the <see cref="OxyMouseEventArgs" />.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs" /> instance containing the event data.</param>
        /// <param name="delta">The zoom factor.</param>
        private static void HandleZoomAt(IPlotView view, OxyMouseEventArgs args, double delta)
        {
            var m = new ZoomStepManipulator(view) { Step = delta, FineControl = args.IsControlDown };
            m.Started(args);
        }

        /// <summary>
        /// Zooms the view by the mouse wheel delta in the specified <see cref="OxyKeyEventArgs" />.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs" /> instance containing the event data.</param>
        /// <param name="factor">The zoom speed factor. Default value is 1.</param>
        private static void HandleZoomByWheel(IPlotView view, OxyMouseWheelEventArgs args, double factor = 1)
        {
            var m = new ZoomStepManipulator(view) { Step = args.Delta * 0.001 * factor, FineControl = args.IsControlDown };
            m.Started(args);
        }

        /// <summary>
        /// Zooms the view by the key in the specified factor.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="delta">The zoom factor (positive zoom in, negative zoom out).</param>
        private static void HandleZoomCenter(IPlotView view, double delta)
        {
            view.ActualModel.ZoomAllAxes(1 + (delta * 0.12));
            view.InvalidatePlot(false);
        }

        /// <summary>
        /// Pans the view by the key in the specified vector.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dx">The horizontal delta (percentage of plot area width).</param>
        /// <param name="dy">The vertical delta (percentage of plot area height).</param>
        private static void HandlePan(IPlotView view, double dx, double dy)
        {
            dx *= view.ActualModel.PlotArea.Width;
            dy *= view.ActualModel.PlotArea.Height;
            view.ActualModel.PanAllAxes(dx, dy);
            view.InvalidatePlot(false);
        }
    }
}