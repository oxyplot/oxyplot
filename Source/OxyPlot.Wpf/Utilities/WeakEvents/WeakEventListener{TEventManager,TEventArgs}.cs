// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeakEventListener{TEventManager,TEventArgs}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        /// Initializes a new instance of the <see cref="WeakEventListener&lt;TEventManager, TEventArgs&gt;" /> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public WeakEventListener(EventHandler<TEventArgs> handler)
        {
            this.realHandler = handler;
        }

        /// <summary>
        /// Receives events from the centralized event manager.
        /// </summary>
        /// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method.</param>
        /// <param name="sender">Object that originated the event.</param>
        /// <param name="e">Event data.</param>
        /// <returns><c>true</c> if the listener handled the event. It is considered an error by the <see cref="T:System.Windows.WeakEventManager" /> handling in WPF to register a listener for an event that the listener does not handle. Regardless, the method should return <c>false</c> if it receives an event that it does not recognize or handle.</returns>
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