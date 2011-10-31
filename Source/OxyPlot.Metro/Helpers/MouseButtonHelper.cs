// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseButtonHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Metro
{
    using System;
    using Windows.Foundation;
    using Windows.UI.Xaml.Input;
#if !METRO

    /// <summary>
    /// Mouse button helper
    /// from http://yinyangme.com/blog/post/The-simplest-way-to-detect-DoubleClick-in-Silverlight.aspx
    /// </summary>
    internal static class MouseButtonHelper
    {
        #region Constants and Fields

        /// <summary>
        /// The k_ double click speed.
        /// </summary>
        private const long k_DoubleClickSpeed = 500;

        /// <summary>
        /// The k_ max move distance.
        /// </summary>
        private const double k_MaxMoveDistance = 10;

        /// <summary>
        /// The _ last click ticks.
        /// </summary>
        private static long _LastClickTicks;

        /// <summary>
        /// The _ last position.
        /// </summary>
        private static Point _LastPosition;

        /// <summary>
        /// The _ last sender.
        /// </summary>
        private static WeakReference _LastSender;

        #endregion

        #region Methods
        /// <summary>
        /// The is double click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The is double click.
        /// </returns>
        internal static bool IsDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Point position = e.GetPosition(null);
            long clickTicks = DateTime.Now.Ticks;
            long elapsedTicks = clickTicks - _LastClickTicks;
            long elapsedTime = elapsedTicks / TimeSpan.TicksPerMillisecond;
            bool quickClick = elapsedTime <= k_DoubleClickSpeed;
            bool senderMatch = _LastSender != null && sender.Equals(_LastSender.Target);

            if (senderMatch && quickClick && position.Distance(_LastPosition) <= k_MaxMoveDistance)
            {
                // Double click!
                _LastClickTicks = 0;
                _LastSender = null;
                return true;
            }

            // Not a double click
            _LastClickTicks = clickTicks;
            _LastPosition = position;
            if (!quickClick)
            {
                _LastSender = new WeakReference(sender);
            }

            return false;
        }
        /// <summary>
        /// The distance.
        /// </summary>
        /// <param name="pointA">
        /// The point a.
        /// </param>
        /// <param name="pointB">
        /// The point b.
        /// </param>
        /// <returns>
        /// The distance.
        /// </returns>
        private static double Distance(this Point pointA, Point pointB)
        {
            double x = pointA.X - pointB.X;
            double y = pointA.Y - pointB.Y;
            return Math.Sqrt(x * x + y * y);
        }

        #endregion
    }
#endif
}