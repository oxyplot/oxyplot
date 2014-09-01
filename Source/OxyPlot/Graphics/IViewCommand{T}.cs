// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewCommand{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality to execute a command on a view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality to execute a command on a view.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments.</typeparam>
    public interface IViewCommand<in T> : IViewCommand where T : OxyInputEventArgs
    {
        /// <summary>
        /// Executes the command on the specified plot.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="args">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        void Execute(IView view, IController controller, T args);
    }
}