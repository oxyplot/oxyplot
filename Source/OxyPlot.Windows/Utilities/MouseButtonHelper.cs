// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseButtonHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Mouse button helper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Windows
{
    using System;

    using global::Windows.Foundation;

    /// <summary>
    /// Mouse button helper
    /// </summary>
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
        /// Determines whether the last click is a double click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="position">The position.</param>
        /// <returns>True if the click was a double click.</returns>
        internal static bool IsDoubleClick(object sender, Point position)
        {
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
        /// Calculates the distance.
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