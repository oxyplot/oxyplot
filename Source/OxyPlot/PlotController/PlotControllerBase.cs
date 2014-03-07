// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotControllerBase.cs" company="OxyPlot">
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
//   Provides functionality to interact with the plot view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides functionality to interact with the plot view.
    /// </summary>
    public abstract class PlotControllerBase : IPlotController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotControllerBase"/> class.
        /// </summary>
        protected PlotControllerBase()
        {
            this.MouseDownManipulators = new List<MouseManipulator>();
            this.MouseHoverManipulators = new List<MouseManipulator>();
            this.TouchManipulators = new List<TouchManipulator>();
        }

        /// <summary>
        /// Gets the manipulators that are created by mouse down events. These manipulators are removed when the mouse button is released.
        /// </summary>
        protected IList<MouseManipulator> MouseDownManipulators { get; private set; }

        /// <summary>
        /// Gets the manipulators that are created by mouse enter events. These manipulators are removed when the mouse leaves the control.
        /// </summary>
        protected IList<MouseManipulator> MouseHoverManipulators { get; private set; }

        /// <summary>
        /// Gets the manipulators that are created by touch events. These manipulators are removed when the touch gesture is completed.
        /// </summary>
        protected IList<TouchManipulator> TouchManipulators { get; private set; }

        /// <summary>
        /// Handles the specified gesture.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="gesture">The gesture.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        /// <returns>
        ///   <c>true</c> if the event was handled.
        /// </returns>
        public bool HandleGesture(IPlotControl view, OxyInputGesture gesture, OxyInputEventArgs args)
        {
            var command = this.GetCommand(gesture);
            return this.HandleCommand(command, view, args);
        }

        /// <summary>
        /// Handles mouse down events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseDown(IPlotControl view, OxyMouseDownEventArgs args)
        {
            if (view.ActualModel != null)
            {
                view.ActualModel.HandleMouseDown(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            var command = this.GetCommand(new OxyMouseDownGesture(args.ChangedButton, args.ModifierKeys, args.ClickCount));
            return this.HandleCommand(command, view, args);
        }

        /// <summary>
        /// Handles mouse enter events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseEnter(IPlotControl view, OxyMouseEventArgs args)
        {
            var command = this.GetCommand(new OxyMouseEnterGesture(args.ModifierKeys));
            return this.HandleCommand(command, view, args);
        }

        /// <summary>
        /// Handles mouse leave events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseLeave(IPlotControl view, OxyMouseEventArgs args)
        {
            foreach (var m in this.MouseHoverManipulators.ToArray())
            {
                m.Completed(args);
                this.MouseHoverManipulators.Remove(m);
            }

            return true;
        }

        /// <summary>
        /// Handles mouse move events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseMove(IPlotControl view, OxyMouseEventArgs args)
        {
            if (view.ActualModel != null)
            {
                view.ActualModel.HandleMouseMove(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            foreach (var m in this.MouseDownManipulators)
            {
                m.Delta(args);
            }

            foreach (var m in this.MouseHoverManipulators)
            {
                m.Delta(args);
            }

            return true;
        }

        /// <summary>
        /// Handles mouse up events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseUp(IPlotControl view, OxyMouseEventArgs args)
        {
            if (view.ActualModel != null)
            {
                view.ActualModel.HandleMouseUp(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            foreach (var m in this.MouseDownManipulators.ToArray())
            {
                m.Completed(args);
                this.MouseDownManipulators.Remove(m);
            }

            return true;
        }

        /// <summary>
        /// Handles mouse wheel events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseWheel(IPlotControl view, OxyMouseWheelEventArgs args)
        {
            var command = this.GetCommand(new OxyMouseWheelGesture(args.ModifierKeys));
            return this.HandleCommand(command, view, args);
        }

        /// <summary>
        /// Handles touch started events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public bool HandleTouchStarted(IPlotControl view, OxyTouchEventArgs args)
        {
            if (view.ActualModel != null)
            {
                // view.ActualModel.HandleTouchStarted(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            var command = this.GetCommand(new OxyTouchGesture());
            return this.HandleCommand(command, view, args);
        }

        /// <summary>
        /// Handles touch delta events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public bool HandleTouchDelta(IPlotControl view, OxyTouchEventArgs args)
        {
            if (view.ActualModel != null)
            {
                ////  view.ActualModel.HandleTouchDelta(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            foreach (var m in this.TouchManipulators)
            {
                m.Delta(args);
            }

            return true;
        }

        /// <summary>
        /// Handles touch completed events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public bool HandleTouchCompleted(IPlotControl view, OxyTouchEventArgs args)
        {
            if (view.ActualModel != null)
            {
                ////   view.ActualModel.HandleTouchCompleted(this, args);
                if (args.Handled)
                {
                    return true;
                }
            }

            foreach (var m in this.TouchManipulators.ToArray())
            {
                m.Completed(args);
                this.TouchManipulators.Remove(m);
            }

            return true;
        }

        /// <summary>
        /// Handles key down events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyKeyEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleKeyDown(IPlotControl view, OxyKeyEventArgs args)
        {
            if (view.ActualModel == null)
            {
                return false;
            }

            view.ActualModel.HandleKeyDown(this, args);
            if (args.Handled)
            {
                return true;
            }

            var command = this.GetCommand(new OxyKeyGesture(args.Key, args.ModifierKeys));
            return this.HandleCommand(command, view, args);
        }

        /// <summary>
        /// Adds the specified mouse manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse down event arguments.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="manipulator">The manipulator to add.</param>
        /// <param name="args">The <see cref="OxyMouseDownEventArgs"/> instance containing the event data.</param>
        public virtual void AddMouseManipulator(
            IPlotControl view,
            MouseManipulator manipulator,
            OxyMouseDownEventArgs args)
        {
            this.MouseDownManipulators.Add(manipulator);
            manipulator.Started(args);
        }

        /// <summary>
        /// Adds the specified mouse hover manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="manipulator">The manipulator.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        public virtual void AddHoverManipulator(
            IPlotControl view,
            MouseManipulator manipulator,
            OxyMouseEventArgs args)
        {
            this.MouseHoverManipulators.Add(manipulator);
            manipulator.Started(args);
        }

        /// <summary>
        /// Adds the specified mouse hover manipulator and invokes the <see cref="TouchManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="manipulator">The manipulator.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        public virtual void AddTouchManipulator(
            IPlotControl view,
            TouchManipulator manipulator,
            OxyTouchEventArgs args)
        {
            this.TouchManipulators.Add(manipulator);
            manipulator.Started(args);
        }

        /// <summary>
        /// Binds the specified command to the specified mouse gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public abstract void Bind(OxyMouseDownGesture gesture, IPlotControllerCommand<OxyMouseDownEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified mouse enter gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public abstract void Bind(OxyMouseEnterGesture gesture, IPlotControllerCommand<OxyMouseEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified mouse wheel gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public abstract void Bind(OxyMouseWheelGesture gesture, IPlotControllerCommand<OxyMouseWheelEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified touch gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public abstract void Bind(OxyTouchGesture gesture, IPlotControllerCommand<OxyTouchEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified key gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public abstract void Bind(OxyKeyGesture gesture, IPlotControllerCommand<OxyKeyEventArgs> command);

        /// <summary>
        /// Unbinds the specified gesture.
        /// </summary>
        /// <param name="gesture">The gesture to unbind.</param>
        public abstract void Unbind(OxyInputGesture gesture);

        /// <summary>
        /// Unbinds the specified command from all gestures.
        /// </summary>
        /// <param name="command">The command to unbind.</param>
        public abstract void Unbind(IPlotControllerCommand command);

        /// <summary>
        /// Unbinds all commands.
        /// </summary>
        public abstract void UnbindAll();

        /// <summary>
        /// Handles a command triggered by an input gesture.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the command was handled.</returns>
        protected virtual bool HandleCommand(IPlotControllerCommand command, IPlotControl view, OxyInputEventArgs args)
        {
            if (command == null)
            {
                return false;
            }

            command.Execute(view, this, args);

            args.Handled = true;
            return true;
        }

        /// <summary>
        /// Gets the command for the specified <see cref="OxyInputGesture"/>.
        /// </summary>
        /// <param name="gesture">The input gesture.</param>
        /// <returns>A command.</returns>
        protected abstract IPlotControllerCommand GetCommand(OxyInputGesture gesture);
    }
}
