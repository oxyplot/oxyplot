// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShapeAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for shape annotations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// Provides an abstract base class for shape annotations.
    /// </summary>
    public abstract class ShapeAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Fill"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> FillProperty = AvaloniaProperty.Register<ShapeAnnotation, Color>(nameof(Fill), Colors.LightBlue);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> StrokeProperty = AvaloniaProperty.Register<ShapeAnnotation, Color>(nameof(Stroke), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<ShapeAnnotation, double>(nameof(StrokeThickness), 0.0);

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        public Color Fill
        {
            get
            {
                return GetValue(FillProperty);
            }

            set
            {
                SetValue(FillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public Color Stroke
        {
            get
            {
                return GetValue(StrokeProperty);
            }

            set
            {
                SetValue(StrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return GetValue(StrokeThicknessProperty);
            }

            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.ShapeAnnotation)InternalAnnotation;
            a.Fill = Fill.ToOxyColor();
            a.Stroke = Stroke.ToOxyColor();
            a.StrokeThickness = StrokeThickness;
        }

        static ShapeAnnotation()
        {
            FillProperty.Changed.AddClassHandler<ShapeAnnotation>(AppearanceChanged);
            StrokeProperty.Changed.AddClassHandler<ShapeAnnotation>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<ShapeAnnotation>(AppearanceChanged);
        }
    }
}