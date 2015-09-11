// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timeout.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a timer that works for all target platforms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The timeout class.
    /// </summary>
    public static class Timeout
    {
        /// <summary>
        /// A constant used to specify an infinite waiting period, for threading methods that accept an <c>int</c> parameter.
        /// </summary>
        public const int Infinite = -1;
    }
}