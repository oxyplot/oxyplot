// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegatePlotCommand.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a controller command for the <see cref="IPlotView" /> implemented by a delegate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides a controller command for the <see cref="IPlotView" /> implemented by a delegate.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments.</typeparam>
    public class DelegatePlotCommand<T> : DelegateViewCommand<T>
        where T : OxyInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatePlotCommand{T}" /> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public DelegatePlotCommand(Action<IPlotView, IController, T> handler)
            : base((v, c, e) => handler((IPlotView)v, c, e))
        {
        }
    }
}