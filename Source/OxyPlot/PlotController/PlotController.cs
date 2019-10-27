// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotController.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an <see cref="IPlotController" /> with a default set of plot bindings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides an <see cref="IPlotController" /> with a default set of plot bindings.
    /// </summary>
    public class PlotController : ControllerBase, IPlotController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotController" /> class.
        /// </summary>
        public PlotController()
        {
            // Zoom rectangle bindings: MMB / control RMB / control+alt LMB
            this.BindMouseDown(OxyMouseButton.Middle, PlotCommands.ZoomRectangle);
            this.BindMouseDown(OxyMouseButton.Right, OxyModifierKeys.Control, PlotCommands.ZoomRectangle);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control | OxyModifierKeys.Alt, PlotCommands.ZoomRectangle);

            // Reset bindings: Same as zoom rectangle, but double click / A key
            this.BindMouseDown(OxyMouseButton.Middle, OxyModifierKeys.None, 2, PlotCommands.ResetAt);
            this.BindMouseDown(OxyMouseButton.Right, OxyModifierKeys.Control, 2, PlotCommands.ResetAt);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control | OxyModifierKeys.Alt, 2, PlotCommands.ResetAt);
            this.BindKeyDown(OxyKey.A, PlotCommands.Reset);
            this.BindKeyDown(OxyKey.C, OxyModifierKeys.Control | OxyModifierKeys.Alt, PlotCommands.CopyCode);
            this.BindKeyDown(OxyKey.Home, PlotCommands.Reset);
            this.BindCore(new OxyShakeGesture(), PlotCommands.Reset);

            // Pan bindings: RMB / alt LMB / Up/down/left/right keys (panning direction on axis is opposite of key as it is more intuitive)
            this.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Alt, PlotCommands.PanAt);
            this.BindKeyDown(OxyKey.Left, PlotCommands.PanLeft);
            this.BindKeyDown(OxyKey.Right, PlotCommands.PanRight);
            this.BindKeyDown(OxyKey.Up, PlotCommands.PanUp);
            this.BindKeyDown(OxyKey.Down, PlotCommands.PanDown);
            this.BindKeyDown(OxyKey.Left, OxyModifierKeys.Control, PlotCommands.PanLeftFine);
            this.BindKeyDown(OxyKey.Right, OxyModifierKeys.Control, PlotCommands.PanRightFine);
            this.BindKeyDown(OxyKey.Up, OxyModifierKeys.Control, PlotCommands.PanUpFine);
            this.BindKeyDown(OxyKey.Down, OxyModifierKeys.Control, PlotCommands.PanDownFine);

            this.BindTouchDown(PlotCommands.PanZoomByTouch);

            // Tracker bindings: LMB
            this.BindMouseDown(OxyMouseButton.Left, PlotCommands.SnapTrack);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control, PlotCommands.Track);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Shift, PlotCommands.PointsOnlyTrack);

            // Tracker bindings: Touch
            this.BindTouchDown(PlotCommands.SnapTrackTouch);

            // Zoom in/out binding: XB1 / XB2 / mouse wheels / +/- keys
            this.BindMouseDown(OxyMouseButton.XButton1, PlotCommands.ZoomInAt);
            this.BindMouseDown(OxyMouseButton.XButton2, PlotCommands.ZoomOutAt);
            this.BindMouseWheel(PlotCommands.ZoomWheel);
            this.BindMouseWheel(OxyModifierKeys.Control, PlotCommands.ZoomWheelFine);
            this.BindKeyDown(OxyKey.Add, PlotCommands.ZoomIn);
            this.BindKeyDown(OxyKey.Subtract, PlotCommands.ZoomOut);
            this.BindKeyDown(OxyKey.PageUp, PlotCommands.ZoomIn);
            this.BindKeyDown(OxyKey.PageDown, PlotCommands.ZoomOut);
            this.BindKeyDown(OxyKey.Add, OxyModifierKeys.Control, PlotCommands.ZoomInFine);
            this.BindKeyDown(OxyKey.Subtract, OxyModifierKeys.Control, PlotCommands.ZoomOutFine);
            this.BindKeyDown(OxyKey.PageUp, OxyModifierKeys.Control, PlotCommands.ZoomInFine);
            this.BindKeyDown(OxyKey.PageDown, OxyModifierKeys.Control, PlotCommands.ZoomOutFine);
        }
    }
}
