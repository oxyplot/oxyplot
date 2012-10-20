// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseButtonHelper.cs" company="OxyPlot">
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
//   Mouse button helper
//   from http://yinyangme.com/blog/post/The-simplest-way-to-detect-DoubleClick-in-Silverlight.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Silverlight
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Mouse button helper
    /// from http://yinyangme.com/blog/post/The-simplest-way-to-detect-DoubleClick-in-Silverlight.aspx
    /// </summary>
    internal static class MouseButtonHelper
    {
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

    }
}