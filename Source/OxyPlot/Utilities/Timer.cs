// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a timer that works for all target platforms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Timer callback delegate.
    /// </summary>
    /// <param name="state">
    /// The state.
    /// </param>
    public delegate void TimerCallback(object state);

    /// <summary>
    /// Timer for WinRT since WinRT only provides the DispatcherTimer which cannot be used outside the UI thread.
    /// </summary>
    public class Timer : IDisposable
    {
        /// <summary>
        /// The timer callback.
        /// </summary>
        private readonly TimerCallback timerCallback;

        /// <summary>
        /// The timer state.
        /// </summary>
        private readonly object timerState;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        public Timer()
            : this(100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="interval">
        /// The interval in milliseconds.
        /// </param>
        public Timer(int interval)
        {
            this.Interval = interval;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public Timer(TimerCallback callback)
            : this(callback, null, Timeout.Infinite, Timeout.Infinite)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="interval">The interval.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="callback" /> is <c>null</c>.</exception>
        public Timer(TimerCallback callback, object state, int dueTime, int interval)
            : this(callback, state, TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(interval))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="interval">The interval.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="callback" /> is <c>null</c>.</exception>
        public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan interval)
        {
            this.timerCallback = callback;
            this.timerState = state;

            this.Change(dueTime, interval);
        }
 
        /// <summary>
        /// Occurs when the interval elapses.
        /// </summary>
        public event EventHandler<EventArgs> Elapsed;
 
        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval. The default is 100 milliseconds.</value>
        public int Interval { get; set; }
   
        /// <summary>
        /// Changes the specified interval.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="interval">The interval.</param>
        public void Change(int dueTime, int interval)
        {
            this.Change(TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Changes the specified interval.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="interval">The interval.</param>
        public void Change(TimeSpan dueTime, TimeSpan interval)
        {
            this.Stop();

            Debug.WriteLine("Changing timer (dueTime: {0}, interval: {1})", dueTime, interval);

            this.cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = this.cancellationTokenSource.Token;

            this.Interval = (int)interval.TotalMilliseconds;

            if (dueTime < TimeSpan.Zero)
            {
                // Never invoke initial one
            }
            else if (dueTime == TimeSpan.Zero)
            {
                // Invoke immediately
                this.TimerElapsed();

                this.Start(cancellationToken);
            }
            else
            {
                // Invoke after due time
                Task.Delay(dueTime, cancellationToken).ContinueWith(this.ContinueTimer, cancellationToken, cancellationToken);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Stop();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        private void Start(CancellationToken cancellationToken)
        {
            if (this.Interval <= 0)
            {
                // Never start a timer
                return;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Task.Delay(this.Interval, cancellationToken).ContinueWith(this.ContinueTimer, cancellationToken, cancellationToken);
        }

        /// <summary>
        /// Continues the timer.
        /// </summary>
        /// <param name="t">The task.</param>
        /// <param name="state">The state which must be the cancellation token.</param>
        private void ContinueTimer(Task t, object state)
        {
            var cancellationToken = (CancellationToken)state;
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.TimerElapsed();

            if (!cancellationToken.IsCancellationRequested)
            {
                this.Start(cancellationToken);
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        private void Stop()
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Called when the interval elapses.
        /// </summary>
        private void TimerElapsed()
        {
            var elapsed = this.Elapsed;
            if (elapsed != null)
            {
                elapsed(this, EventArgs.Empty);
            }

            if (this.timerCallback != null)
            {
                this.timerCallback(this.timerState);
            }
        }
    }
}