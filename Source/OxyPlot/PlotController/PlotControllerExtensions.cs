// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotControllerExtensions.cs" company="OxyPlot">
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
//   Provides binding extension methods for the <see cref="IPlotController"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides binding extension methods for the <see cref="IPlotController" />.
    /// </summary>
    public static class PlotControllerExtensions
    {
        /// <summary>
        /// Binds the specified key to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="key">The key.</param>
        /// <param name="command">A plot controller command that takes key event arguments.</param>
        public static void BindKeyDown(this IPlotController controller, OxyKey key, IPlotControllerCommand<OxyKeyEventArgs> command)
        {
            controller.Bind(new OxyKeyGesture(key), command);
        }

        /// <summary>
        /// Binds the specified modifier+key to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The key modifiers.</param>
        /// <param name="command">A plot controller command that takes key event arguments.</param>
        public static void BindKeyDown(this IPlotController controller, OxyKey key, OxyModifierKeys modifiers, IPlotControllerCommand<OxyKeyEventArgs> command)
        {
            controller.Bind(new OxyKeyGesture(key, modifiers), command);
        }

        /// <summary>
        /// Binds the specified mouse button to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="mouseButton">The mouse button.</param>
        /// <param name="command">A plot controller command that takes mouse event arguments.</param>
        public static void BindMouseDown(this IPlotController controller, OxyMouseButton mouseButton, IPlotControllerCommand<OxyMouseDownEventArgs> command)
        {
            controller.Bind(new OxyMouseDownGesture(mouseButton), command);
        }

        /// <summary>
        /// Binds the specified modifier+mouse button gesture to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="mouseButton">The mouse button.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <param name="command">A plot controller command that takes mouse event arguments.</param>
        public static void BindMouseDown(this IPlotController controller, OxyMouseButton mouseButton, OxyModifierKeys modifiers, IPlotControllerCommand<OxyMouseDownEventArgs> command)
        {
            controller.Bind(new OxyMouseDownGesture(mouseButton, modifiers), command);
        }

        /// <summary>
        /// Binds the specified modifiers+mouse button+click count gesture to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="mouseButton">The mouse button.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <param name="clickCount">The click count.</param>
        /// <param name="command">A plot controller command that takes mouse event arguments.</param>
        public static void BindMouseDown(this IPlotController controller, OxyMouseButton mouseButton, OxyModifierKeys modifiers, int clickCount, IPlotControllerCommand<OxyMouseDownEventArgs> command)
        {
            controller.Bind(new OxyMouseDownGesture(mouseButton, modifiers, clickCount), command);
        }

        /// <summary>
        /// Binds the touch down event to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="command">A plot controller command that takes touch event arguments.</param>
        public static void BindTouchDown(this IPlotController controller, IPlotControllerCommand<OxyTouchEventArgs> command)
        {
            controller.Bind(new OxyTouchGesture(), command);
        }

        /// <summary>
        /// Binds the mouse enter event to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="command">A plot controller command that takes mouse event arguments.</param>
        public static void BindMouseEnter(this IPlotController controller, IPlotControllerCommand<OxyMouseEventArgs> command)
        {
            controller.Bind(new OxyMouseEnterGesture(), command);
        }

        /// <summary>
        /// Binds the mouse wheel event to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="command">A plot controller command that takes mouse wheel event arguments.</param>
        public static void BindMouseWheel(this IPlotController controller, IPlotControllerCommand<OxyMouseWheelEventArgs> command)
        {
            controller.Bind(new OxyMouseWheelGesture(), command);
        }

        /// <summary>
        /// Binds the modifier+mouse wheel event to the specified command.
        /// </summary>
        /// <param name="controller">The plot controller.</param>
        /// <param name="modifiers">The modifier key(s).</param>
        /// <param name="command">A plot controller command that takes mouse wheel event arguments.</param>
        public static void BindMouseWheel(this IPlotController controller, OxyModifierKeys modifiers, IPlotControllerCommand<OxyMouseWheelEventArgs> command)
        {
            controller.Bind(new OxyMouseWheelGesture(modifiers), command);
        }
    }
}