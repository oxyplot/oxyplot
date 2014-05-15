// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegatePlotCommand.cs" company="OxyPlot">
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
//   Provides a controller command for the IPlotView implemented by a delegate.
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