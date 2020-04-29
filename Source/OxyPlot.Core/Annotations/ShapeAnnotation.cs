// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShapeAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for shape annotations, such as <see cref="PointAnnotation" />, <see cref="EllipseAnnotation" />, <see cref="PolygonAnnotation" /> and <see cref="RectangleAnnotation" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    /// <summary>
    /// Provides an abstract base class for shape annotations, such as <see cref="PointAnnotation" />, <see cref="EllipseAnnotation" />, <see cref="PolygonAnnotation" /> and <see cref="RectangleAnnotation" />.
    /// </summary>
    public abstract class ShapeAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeAnnotation"/> class.
        /// </summary>
        protected ShapeAnnotation()
        {
            this.Stroke = OxyColors.Black;
            this.Fill = OxyColors.LightBlue;
        }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        /// <value>The fill color.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public OxyColor Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness { get; set; }
    }
}