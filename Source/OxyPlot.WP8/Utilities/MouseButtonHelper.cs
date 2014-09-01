// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseButtonHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Mouse button helper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WP8
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Mouse button helper
    /// </summary>
    /// <remarks>See <a href="http://yinyangme.com/blog/post/The-simplest-way-to-detect-DoubleClick-in-Silverlight.aspx">The simplest way to detect DoubleClick in Silverlight</a>.</remarks>
    internal static class MouseButtonHelper
    {
        /// <summary>
        /// The double click speed.
        /// </summary>
        private const long DoubleClickSpeed = 500;

        /// <summary>
        /// The max move distance.
        /// </summary>
        private const double MaxMoveDistance = 10;

        /// <summary>
        /// The last click ticks.
        /// </summary>
        private static long lastClickTicks;

        /// <summary>
        /// The last position.
        /// </summary>
        private static Point lastPosition;

        /// <summary>
        /// The last sender.
        /// </summary>
        private static WeakReference lastSender;

        /// <summary>
        /// Determines if the click in the specified <see cref="MouseButtonEventArgs" /> is a double click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        /// <returns><c>true</c> if the click is a double click.</returns>
        internal static bool IsDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(null);
            long clickTicks = DateTime.Now.Ticks;
            long elapsedTicks = clickTicks - lastClickTicks;
            long elapsedTime = elapsedTicks / TimeSpan.TicksPerMillisecond;
            bool quickClick = elapsedTime <= DoubleClickSpeed;
            bool senderMatch = lastSender != null && sender.Equals(lastSender.Target);

            if (senderMatch && quickClick && position.Distance(lastPosition) <= MaxMoveDistance)
            {
                // Double click!
                lastClickTicks = 0;
                lastSender = null;
                return true;
            }

            // Not a double click
            lastClickTicks = clickTicks;
            lastPosition = position;
            if (!quickClick)
            {
                lastSender = new WeakReference(sender);
            }

            return false;
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="pointA">The point a.</param>
        /// <param name="pointB">The point b.</param>
        /// <returns>The distance.</returns>
        private static double Distance(this Point pointA, Point pointB)
        {
            double x = pointA.X - pointB.X;
            double y = pointA.Y - pointB.Y;
            return Math.Sqrt((x * x) + (y * y));
        }
    }
}