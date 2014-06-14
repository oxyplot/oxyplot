// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseButtonHelper.cs" company="OxyPlot">
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