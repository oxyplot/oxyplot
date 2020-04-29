// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IController.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality to interact with a graphics view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality to interact with a graphics view.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Handles mouse down events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseDown(IView view, OxyMouseDownEventArgs args);

        /// <summary>
        /// Handles mouse move events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseMove(IView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse up events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseUp(IView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse enter events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseEnter(IView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse leave events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseLeave(IView view, OxyMouseEventArgs args);

        /// <summary>
        /// Handles mouse wheel events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleMouseWheel(IView view, OxyMouseWheelEventArgs args);

        /// <summary>
        /// Handles touch started events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleTouchStarted(IView view, OxyTouchEventArgs args);

        /// <summary>
        /// Handles touch delta events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleTouchDelta(IView view, OxyTouchEventArgs args);

        /// <summary>
        /// Handles touch completed events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleTouchCompleted(IView view, OxyTouchEventArgs args);

        /// <summary>
        /// Handles key down events.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="args">The <see cref="OxyKeyEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleKeyDown(IView view, OxyKeyEventArgs args);

        /// <summary>
        /// Handles the specified gesture.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="gesture">The gesture.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        /// <returns><c>true</c> if the event was handled.</returns>
        bool HandleGesture(IView view, OxyInputGesture gesture, OxyInputEventArgs args);

        /// <summary>
        /// Adds the specified mouse manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="manipulator">The manipulator to add.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        void AddMouseManipulator(IView view, ManipulatorBase<OxyMouseEventArgs> manipulator, OxyMouseDownEventArgs args);

        /// <summary>
        /// Adds the specified mouse hover manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="manipulator">The manipulator.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        void AddHoverManipulator(IView view, ManipulatorBase<OxyMouseEventArgs> manipulator, OxyMouseEventArgs args);

        /// <summary>
        /// Adds the specified touch manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="manipulator">The manipulator.</param>
        /// <param name="args">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        void AddTouchManipulator(IView view, ManipulatorBase<OxyTouchEventArgs> manipulator, OxyTouchEventArgs args);

        /// <summary>
        /// Binds the specified command to the specified mouse down gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The mouse down gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyMouseDownGesture gesture, IViewCommand<OxyMouseDownEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified mouse enter gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The mouse enter gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyMouseEnterGesture gesture, IViewCommand<OxyMouseEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified mouse wheel gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The mouse wheel gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyMouseWheelGesture gesture, IViewCommand<OxyMouseWheelEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified touch gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The touch gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyTouchGesture gesture, IViewCommand<OxyTouchEventArgs> command);

        /// <summary>
        /// Binds the specified command to the specified key gesture. Removes old bindings to the gesture.
        /// </summary>
        /// <param name="gesture">The key gesture.</param>
        /// <param name="command">The command. If <c>null</c>, the binding will be removed.</param>
        void Bind(OxyKeyGesture gesture, IViewCommand<OxyKeyEventArgs> command);

        /// <summary>
        /// Unbinds the specified gesture.
        /// </summary>
        /// <param name="gesture">The gesture to unbind.</param>
        void Unbind(OxyInputGesture gesture);

        /// <summary>
        /// Unbinds the specified command from all gestures.
        /// </summary>
        /// <param name="command">The command to unbind.</param>
        void Unbind(IViewCommand command);

        /// <summary>
        /// Unbinds all commands.
        /// </summary>
        void UnbindAll();
    }
}