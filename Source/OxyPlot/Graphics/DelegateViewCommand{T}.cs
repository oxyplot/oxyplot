// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateViewCommand{T}.cs" company="OxyPlot">
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
//   Provides a <see cref="IViewCommand" /> implemented by a delegate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides a <see cref="IViewCommand" /> implemented by a delegate.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments.</typeparam>
    public class DelegateViewCommand<T> : IViewCommand<T>
        where T : OxyInputEventArgs
    {
        /// <summary>
        /// The handler
        /// </summary>
        private readonly Action<IGraphicsView, IGraphicsController, T> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateViewCommand{T}" /> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public DelegateViewCommand(Action<IGraphicsView, IGraphicsController, T> handler)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Executes the command on the specified plot.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="controller">The plot controller.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public void Execute(IGraphicsView view, IGraphicsController controller, T args)
        {
            this.handler(view, controller, args);
        }

        /// <summary>
        /// Executes the command on the specified plot.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="controller">The plot controller.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public void Execute(IGraphicsView view, IGraphicsController controller, OxyInputEventArgs args)
        {
            this.handler(view, controller, (T)args);
        }
    }
}