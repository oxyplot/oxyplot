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
        /// <inheritdoc/>
        public bool Contains(double x, double y) { return false; }
    }
}
