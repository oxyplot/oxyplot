// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyShakeGesture.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a shake input gesture.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a shake input gesture.
    /// </summary>
    /// <remarks>The input gesture can be bound to a command in a <see cref="PlotController" />. The shake gesture applies primarily to mobile devices.</remarks>
    public class OxyShakeGesture : OxyInputGesture
    {
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
        public override bool Equals(OxyInputGesture other)
        {
            var sg = other as OxyShakeGesture;
            return sg != null;
        }
    }
}