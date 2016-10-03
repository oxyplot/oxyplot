// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EllipseAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.EllipseAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.EllipseAnnotation
    /// </summary>
    public class EllipseAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="X"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> XProperty = AvaloniaProperty.Register<EllipseAnnotation, double>(nameof(X), 0.0);

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> YProperty = AvaloniaProperty.Register<EllipseAnnotation, double>(nameof(Y), 0.0);

        /// <summary>
        /// Initializes static members of the <see cref="EllipseAnnotation"/> class.
        /// </summary>
        static EllipseAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<EllipseAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<EllipseAnnotation>(AppearanceChanged);

            XProperty.Changed.AddClassHandler<EllipseAnnotation>(DataChanged);
            YProperty.Changed.AddClassHandler<EllipseAnnotation>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseAnnotation" /> class.
        /// </summary>
        public EllipseAnnotation()
        {
            InternalAnnotation = new Annotations.EllipseAnnotation();
        }

        /// <summary>
        /// Gets or sets the  X.
        /// </summary>
        public double X
        {
            get
            {
                return GetValue(XProperty);
            }

            set
            {
                SetValue(XProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the  Y.
        /// </summary>
        public double Y
        {
            get
            {
                return GetValue(YProperty);
            }

            set
            {
                SetValue(YProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>The annotation.</returns>
        public override Annotations.Annotation CreateModel()
        {
            SynchronizeProperties();
            return InternalAnnotation;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.EllipseAnnotation)InternalAnnotation;

            a.X = X;
            a.Width = Width;
            a.Y = Y;
            a.Height = Height;
        }
    }
}