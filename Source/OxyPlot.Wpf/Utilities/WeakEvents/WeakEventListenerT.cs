// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeakEventListenerT.cs" company="OxyPlot">
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
//   Provides a weak event listener that pass the events of the specified event manager to the specified event handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Provides a weak event listener that pass the events of the specified event manager to the specified event handler.
    /// </summary>
    /// <typeparam name="TEventManager">The type of the event manager.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
    public class WeakEventListener<TEventManager, TEventArgs> : IWeakEventListener where TEventArgs : EventArgs
    {
        /// <summary>
        /// The real event handler.
        /// </summary>
        private readonly EventHandler<TEventArgs> realHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventListener&lt;TEventManager, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public WeakEventListener(EventHandler<TEventArgs> handler)
        {
            this.realHandler = handler;
        }

        /// <summary>
        /// Receives events from the centralized event manager.
        /// </summary>
        /// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager"/> calling this method.</param>
        /// <param name="sender">Object that originated the event.</param>
        /// <param name="e">Event data.</param>
        /// <returns>
        /// true if the listener handled the event. It is considered an error by the <see cref="T:System.Windows.WeakEventManager"/> handling in WPF to register a listener for an event that the listener does not handle. Regardless, the method should return false if it receives an event that it does not recognize or handle.
        /// </returns>
        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(TEventManager))
            {
                this.realHandler(sender, e as TEventArgs);
            }
            else
            {
                return false;       // unrecognized event
            }

            return true;
        }
    }
}