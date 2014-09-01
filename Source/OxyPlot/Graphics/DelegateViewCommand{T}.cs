// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateViewCommand{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        private readonly Action<IView, IController, T> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateViewCommand{T}" /> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public DelegateViewCommand(Action<IView, IController, T> handler)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Executes the command on the specified plot.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="controller">The plot controller.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public void Execute(IView view, IController controller, T args)
        {
            this.handler(view, controller, args);
        }

        /// <summary>
        /// Executes the command on the specified plot.
        /// </summary>
        /// <param name="view">The plot view.</param>
        /// <param name="controller">The plot controller.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public void Execute(IView view, IController controller, OxyInputEventArgs args)
        {
            this.handler(view, controller, (T)args);
        }
    }
}