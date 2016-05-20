// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to handle input events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides functionality to handle input events.
    /// </summary>
    public abstract class ControllerBase : IController
    {
        /// <summary>
        /// A synchronization object that is used when the actual model in the current view is <c>null</c>.
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBase" /> class.
        /// </summary>
        protected ControllerBase()
        {
            this.InputCommandBindings = new List<InputCommandBinding>();
            this.MouseDownManipulators = new List<ManipulatorBase<OxyMouseEventArgs>>();
            this.MouseHoverManipulators = new List<ManipulatorBase<OxyMouseEventArgs>>();
            this.TouchManipulators = new List<ManipulatorBase<OxyTouchEventArgs>>();
        }

        /// <summary>
        /// Gets the input bindings.
        /// </summary>
        /// <remarks>This collection is used to specify the customized input gestures (both key, mouse and touch).</remarks>
        public List<InputCommandBinding> InputCommandBindings { get; private set; }

        /// <summary>
        /// Gets the manipulators that are created by mouse down events. These manipulators are removed when the mouse button is released.
        /// </summary>
        protected IList<ManipulatorBase<OxyMouseEventArgs>> MouseDownManipulators { get; private set; }

        /// <summary>
        /// Gets the manipulators that are created by mouse enter events. These manipulators are removed when the mouse leaves the control.
        /// </summary>
        protected IList<ManipulatorBase<OxyMouseEventArgs>> MouseHoverManipulators { get; private set; }

        /// <summary>
        /// Gets the manipulators that are created by touch events. These manipulators are removed when the touch gesture is completed.
        /// </summary>
        protected IList<ManipulatorBase<OxyTouchEventArgs>> TouchManipulators { get; private set; }

        /// <summary>
        /// Handles the specified gesture.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="gesture">The gesture.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleGesture(IView view, OxyInputGesture gesture, OxyInputEventArgs args)
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
        public virtual bool HandleMouseDown(IView view, OxyMouseDownEventArgs args)
        {
            lock (this.GetSyncRoot(view))
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
        }

        /// <summary>
        /// Handles mouse enter events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseEnter(IView view, OxyMouseEventArgs args)
        {
            lock (this.GetSyncRoot(view))
            {
                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleMouseEnter(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                var command = this.GetCommand(new OxyMouseEnterGesture(args.ModifierKeys));
                return this.HandleCommand(command, view, args);
            }
        }

        /// <summary>
        /// Handles mouse leave events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseLeave(IView view, OxyMouseEventArgs args)
        {
            lock (this.GetSyncRoot(view))
            {
                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleMouseLeave(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                foreach (var m in this.MouseHoverManipulators.ToArray())
                {
                    m.Completed(args);
                    this.MouseHoverManipulators.Remove(m);
                }

                return args.Handled;
            }
        }

        /// <summary>
        /// Handles mouse move events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseMove(IView view, OxyMouseEventArgs args)
        {
            lock (this.GetSyncRoot(view))
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

                return args.Handled;
            }
        }

        /// <summary>
        /// Handles mouse up events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseUp(IView view, OxyMouseEventArgs args)
        {
            lock (this.GetSyncRoot(view))
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

                return args.Handled;
            }
        }

        /// <summary>
        /// Handles mouse wheel events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleMouseWheel(IView view, OxyMouseWheelEventArgs args)
        {
            lock (this.GetSyncRoot(view))
            {
                var command = this.GetCommand(new OxyMouseWheelGesture(args.ModifierKeys));
                return this.HandleCommand(command, view, args);
            }
        }

        /// <summary>
        /// Handles touch started events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleTouchStarted(IView view, OxyTouchEventArgs args)
        {
            lock (this.GetSyncRoot(view))
            {
                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleTouchStarted(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                var command = this.GetCommand(new OxyTouchGesture());
                return this.HandleCommand(command, view, args);
            }
        }

        /// <summary>
        /// Handles touch delta events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleTouchDelta(IView view, OxyTouchEventArgs args)
        {
            lock (this.GetSyncRoot(view))
            {
                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleTouchDelta(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                foreach (var m in this.TouchManipulators)
                {
                    m.Delta(args);
                }

                return args.Handled;
            }
        }

        /// <summary>
        /// Handles touch completed events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleTouchCompleted(IView view, OxyTouchEventArgs args)
        {
            lock (this.GetSyncRoot(view))
            {
                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleTouchCompleted(this, args);
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

                return args.Handled;
            }
        }

        /// <summary>
        /// Handles key down events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyKeyEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        public virtual bool HandleKeyDown(IView view, OxyKeyEventArgs args)
        {
            lock (this.GetSyncRoot(view))
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
        }

        /// <summary>
        /// Adds the specified mouse manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse down event arguments.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="manipulator">The manipulator to add.</param>
        /// <param name="args">The <see cref="OxyMouseDownEventArgs" /> instance containing the event data.</param>
        public virtual void AddMouseManipulator(
            IView view,
            ManipulatorBase<OxyMouseEventArgs> manipulator,
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
            IView view,
            ManipulatorBase<OxyMouseEventArgs> manipulator,
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
            IView view,
            ManipulatorBase<OxyTouchEventArgs> manipulator,
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
        public virtual void Bind(OxyMouseDownGesture gesture, IViewCommand<OxyMouseDownEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified mouse enter gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public virtual void Bind(OxyMouseEnterGesture gesture, IViewCommand<OxyMouseEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified mouse wheel gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public virtual void Bind(OxyMouseWheelGesture gesture, IViewCommand<OxyMouseWheelEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified touch gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public virtual void Bind(OxyTouchGesture gesture, IViewCommand<OxyTouchEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Binds the specified command to the specified key gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        public virtual void Bind(OxyKeyGesture gesture, IViewCommand<OxyKeyEventArgs> command)
        {
            this.BindCore(gesture, command);
        }

        /// <summary>
        /// Unbinds the specified gesture.
        /// </summary>
        /// <param name="gesture">The gesture to unbind.</param>
        public virtual void Unbind(OxyInputGesture gesture)
        {
            // ReSharper disable once RedundantNameQualifier
            foreach (var icb in this.InputCommandBindings.Where(icb => icb.Gesture.Equals(gesture)).ToArray())
            {
                this.InputCommandBindings.Remove(icb);
            }
        }

        /// <summary>
        /// Unbinds the specified command from all gestures.
        /// </summary>
        /// <param name="command">The command to unbind.</param>
        public virtual void Unbind(IViewCommand command)
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
        public virtual void UnbindAll()
        {
            this.InputCommandBindings.Clear();
        }

        /// <summary>
        /// Binds the specified command to the specified gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        /// <remarks>This method was created to avoid calling a virtual method in the constructor.</remarks>
        protected void BindCore(OxyInputGesture gesture, IViewCommand command)
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
        /// <returns>A command.</returns>
        protected virtual IViewCommand GetCommand(OxyInputGesture gesture)
        {
            var binding = this.InputCommandBindings.FirstOrDefault(b => b.Gesture.Equals(gesture));
            if (binding == null)
            {
                return null;
            }

            return binding.Command;
        }
        
        /// <summary>
        /// Handles a command triggered by an input gesture.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the command was handled.</returns>
        protected virtual bool HandleCommand(IViewCommand command, IView view, OxyInputEventArgs args)
        {
            if (command == null)
            {
                return false;
            }

            command.Execute(view, this, args);
            return args.Handled;
        }

        /// <summary>
        /// Gets the synchronization object for the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>An object that can be used to synchronize access to the actual model of the view.</returns>
        /// <remarks>This object is used to ensure that events are not handled when the model is being updated.</remarks>
        protected object GetSyncRoot(IView view)
        {
            return view.ActualModel != null ? view.ActualModel.SyncRoot : this.syncRoot;
        }
    }
}