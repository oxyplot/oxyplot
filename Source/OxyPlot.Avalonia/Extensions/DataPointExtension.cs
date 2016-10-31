// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointExtension.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a markup extension for DataPoints.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Avalonia
{
    using OmniXaml;

    /// <summary>
    /// Provides a markup extension for <see cref="DataPoint" />s.
    /// </summary>
    public class DataPointExtension : MarkupExtension
    {
        /// <summary>
        /// The point
        /// </summary>
        private readonly DataPoint point;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointExtension"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public DataPointExtension(double x, double y)
        {
            point = new DataPoint(x, y);
        }

        public override object ProvideValue(MarkupExtensionContext markupExtensionContext)
        {
            return point;
        }
    }
}