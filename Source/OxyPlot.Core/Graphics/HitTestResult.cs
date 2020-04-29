// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HitTestResult.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a hit test result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a hit test result.
    /// </summary>
    public class HitTestResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HitTestResult" /> class.
        /// </summary>
        /// <param name="element">The element that was hit.</param>
        /// <param name="nearestHitPoint">The nearest hit point.</param>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        public HitTestResult(Element element, ScreenPoint nearestHitPoint, object item = null, double index = 0)
        {
            this.Element = element;
            this.NearestHitPoint = nearestHitPoint;
            this.Item = item;
            this.Index = index;
        }

        /// <summary>
        /// Gets the index of the hit (if available).
        /// </summary>
        /// <value>The index.</value>
        /// <remarks>If the hit was in the middle between point 1 and 2, index = 1.5.</remarks>
        public double Index { get; private set; }

        /// <summary>
        /// Gets the item of the hit (if available).
        /// </summary>
        /// <value>The item.</value>
        public object Item { get; private set; }

        /// <summary>
        /// Gets the element that was hit.
        /// </summary>
        /// <value>
        /// The element.
        /// </value>
        public Element Element { get; private set; }

        /// <summary>
        /// Gets the position of the nearest hit point.
        /// </summary>
        /// <value>The nearest hit point.</value>
        public ScreenPoint NearestHitPoint { get; private set; }
    }
}
