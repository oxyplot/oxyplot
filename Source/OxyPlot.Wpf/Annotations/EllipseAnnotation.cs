// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EllipseAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.EllipseAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.EllipseAnnotation
    /// </summary>
    public class EllipseAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="X"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(EllipseAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(EllipseAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// Initializes static members of the <see cref="EllipseAnnotation"/> class.
        /// </summary>
        static EllipseAnnotation()
        {
            TextColorProperty.OverrideMetadata(typeof(EllipseAnnotation), new FrameworkPropertyMetadata(MoreColors.Automatic, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseAnnotation" /> class.
        /// </summary>
        public EllipseAnnotation()
        {
            this.InternalAnnotation = new Annotations.EllipseAnnotation();
        }

        /// <summary>
        /// Gets or sets the  X.
        /// </summary>
        public double X
        {
            get
            {
                return (double)this.GetValue(XProperty);
            }

            set
            {
                this.SetValue(XProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the  Y.
        /// </summary>
        public double Y
        {
            get
            {
                return (double)this.GetValue(YProperty);
            }

            set
            {
                this.SetValue(YProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>The annotation.</returns>
        public override Annotations.Annotation CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAnnotation;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.EllipseAnnotation)this.InternalAnnotation;

            a.X = this.X;
            a.Width = this.Width;
            a.Y = this.Y;
            a.Height = this.Height;
        }
    }
}