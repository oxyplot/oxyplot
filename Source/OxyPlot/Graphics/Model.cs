// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="OxyPlot">
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
//   Provides an abstract base class for graphics models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides an abstract base class for graphics models.
    /// </summary>
    public abstract class Model
    {
        /// <summary>
        /// The synchronization root object.
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="Model" />.
        /// </summary>
        /// <value>A synchronization object.</value>
        /// <remarks>This property can be used when modifying the <see cref="Model" /> on a separate thread (not the thread updating or rendering the model).</remarks>
        public object SyncRoot
        {
            get { return this.syncRoot; }
        }

        /// <summary>
        /// Handles the mouse down event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public abstract void HandleMouseDown(object sender, OxyMouseDownEventArgs args);

        /// <summary>
        /// Handles the mouse enter event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public abstract void HandleMouseEnter(object sender, OxyMouseEventArgs args);

        /// <summary>
        /// Handles the mouse leave event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public abstract void HandleMouseLeave(object sender, OxyMouseEventArgs args);

        /// <summary>
        /// Handles the mouse move event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public abstract void HandleMouseMove(object sender, OxyMouseEventArgs args);

        /// <summary>
        /// Handles the mouse up event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public abstract void HandleMouseUp(object sender, OxyMouseEventArgs args);

        /// <summary>
        /// Handles the touch started event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">A <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public abstract void HandleTouchStarted(object sender, OxyTouchEventArgs args);

        /// <summary>
        /// Handles the touch delta event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">A <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public abstract void HandleTouchDelta(object sender, OxyTouchEventArgs args);

        /// <summary>
        /// Handles the touch completed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">A <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public abstract void HandleTouchCompleted(object sender, OxyTouchEventArgs args);

        /// <summary>
        /// Handles key down events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="OxyKeyEventArgs" /> instance containing the event data.</param>
        public abstract void HandleKeyDown(object sender, OxyKeyEventArgs args);
    }
}