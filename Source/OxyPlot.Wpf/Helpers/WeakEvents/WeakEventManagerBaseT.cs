// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeakEventManagerBaseT.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Provides a generic base class for weak event managers that handle static events.
    /// </summary>
    /// <typeparam name="TManager">The type of the manager.</typeparam>
    public abstract class WeakEventManagerBase<TManager> : WeakEventManager where TManager : WeakEventManagerBase<TManager>, new()
    {
        /// <summary>
        /// Gets the current manager.
        /// </summary>
        /// <value>The current manager.</value>
        private static TManager CurrentManager
        {
            get
            {
                Type managerType = typeof(TManager);
                var manager = (TManager)GetCurrentManager(managerType);
                if (manager == null)
                {
                    manager = new TManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        /// <summary>
        /// Adds the specified listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public static void AddListener(IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(null, listener);
        }

        /// <summary>
        /// Removes the specified listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public static void RemoveListener(IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(null, listener);
        }

        /// <summary>
        /// When overridden in a derived class, starts listening for the event being managed. After <see cref="M:System.Windows.WeakEventManager.StartListening(System.Object)"/>  is first called, the manager should be in the state of calling <see cref="M:System.Windows.WeakEventManager.DeliverEvent(System.Object,System.EventArgs)"/> or <see cref="M:System.Windows.WeakEventManager.DeliverEventToList(System.Object,System.EventArgs,System.Windows.WeakEventManager.ListenerList)"/> whenever the relevant event from the provided source is handled.
        /// </summary>
        /// <param name="source">The source to begin listening on.</param>
        protected sealed override void StartListening(object source)
        {
            this.StartListening();
        }

        /// <summary>
        /// When overridden in a derived class, stops listening on the provided source for the event being managed.
        /// </summary>
        /// <param name="source">The source to stop listening on.</param>
        protected sealed override void StopListening(object source)
        {
            this.StopListening();
        }

        /// <summary>
        /// Handlers the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Handler(object sender, EventArgs e)
        {
            this.DeliverEvent(null, e);
        }

        /// <summary>
        /// Starts the listening.
        /// </summary>
        protected abstract void StartListening();

        /// <summary>
        /// Stops the listening.
        /// </summary>
        protected abstract void StopListening();
    }
}
