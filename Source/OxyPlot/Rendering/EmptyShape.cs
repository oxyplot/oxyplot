using System;
using System.Collections.Generic;
using System.Text;

namespace OxyPlot
{
    /// <summary>
    /// Describes an empty shape
    /// </summary>
    public struct EmptyShape : IOxyRegion
    {
        /// <summary>
        /// A specified point will never be within an empty shape.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>false</returns>
        public bool Contains(double x, double y) { return false; }
    }
}
