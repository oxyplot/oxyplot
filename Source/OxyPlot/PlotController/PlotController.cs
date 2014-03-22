// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotController.cs" company="OxyPlot">
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
//   Provides a plot controller where the input command bindings can be modified.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a plot controller where the input command bindings can be modified.
    /// </summary>
    public class PlotController : PlotControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotController" /> class.
        /// </summary>
        public PlotController()
        {
            this.InputCommandBindings = new List<InputCommandBinding>();

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
            this.BindKeyDown(OxyKey.R, OxyModifierKeys.Control | OxyModifierKeys.Alt, PlotCommands.CopyTextReport);
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

        /// <summary>
        /// Gets the input bindings.
        /// </summary>
        /// <remarks>
        /// This collection is used to specify the customized input gestures (both key, mouse and touch).
        /// </remarks>
        public List<InputCommandBinding> InputCommandBindings { get; private set; }

        /// <summary>
        /// Binds the specified command to the specified mouse gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public override void Bind(OxyMouseDownGesture gesture, IPlotControllerCommand<OxyMouseDownEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified mouse enter gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public override void Bind(OxyMouseEnterGesture gesture, IPlotControllerCommand<OxyMouseEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified mouse wheel gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public override void Bind(OxyMouseWheelGesture gesture, IPlotControllerCommand<OxyMouseWheelEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified touch gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public override void Bind(OxyTouchGesture gesture, IPlotControllerCommand<OxyTouchEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified key gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public override void Bind(OxyKeyGesture gesture, IPlotControllerCommand<OxyKeyEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Unbinds the specified gesture.
        /// </summary>
        /// <param name="gesture">The gesture to unbind.</param>
        public override void Unbind(OxyInputGesture gesture)
        {
            // ReSharper disable once RedundantNameQualifier
            foreach (var icb in this.InputCommandBindings.Where(icb => object.ReferenceEquals(icb.Gesture, gesture)).ToArray())
            {
                this.InputCommandBindings.Remove(icb);
            }
        }

        /// <summary>
        /// Unbinds the specified command from all gestures.
        /// </summary>
        /// <param name="command">The command to unbind.</param>
        public override void Unbind(IPlotControllerCommand command)
        {
            // ReSharper disable once RedundantNameQualifier
            foreach (var icb in this.InputCommandBindings.Where(icb => object.ReferenceEquals(icb.Command, command)).ToArray())
            {
                this.InputCommandBindings.Remove(icb);
            }
        }

        /// <summary>
        /// Unbinds all commands.
        /// </summary>
        public override void UnbindAll()
        {
            this.InputCommandBindings.Clear();
        }

        /// <summary>
        /// Binds the specified command to the specified gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        /// <remarks>
        /// This method was created to avoid calling a virtual method in the constructor.
        /// </remarks>
        protected void BindCore(OxyInputGesture gesture, IPlotControllerCommand command)
        {
            var current = this.InputCommandBindings.FirstOrDefault(icb => icb.Gesture.Equals(gesture));
            if (current != null)
            {
                this.InputCommandBindings.Remove(current);
            }

            if (command != null)
            {
                this.InputCommandBindings.Add(new InputCommandBinding(gesture, command));
            }
        }

        /// <summary>
        /// Gets the command for the specified <see cref="OxyInputGesture" />.
        /// </summary>
        /// <param name="gesture">The input gesture.</param>
        /// <returns>
        /// A command.
        /// </returns>
        protected override IPlotControllerCommand GetCommand(OxyInputGesture gesture)
        {
            var binding = this.InputCommandBindings.FirstOrDefault(b => b.Gesture.Equals(gesture));
            if (binding == null)
            {
                return null;
            }

            return binding.Command;
        }
    }
}