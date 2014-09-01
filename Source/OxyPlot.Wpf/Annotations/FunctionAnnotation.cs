// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.PathAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;

    using OxyPlot.Annotations;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.PathAnnotation
    /// </summary>
    public class FunctionAnnotation : PathAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Type"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type",
            typeof(FunctionAnnotationType),
            typeof(FunctionAnnotation),
            new PropertyMetadata(FunctionAnnotationType.EquationX, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Equation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EquationProperty =
            DependencyProperty.Register("Equation", typeof(Func<double, double>), typeof(FunctionAnnotation), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Resolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResolutionProperty =
            DependencyProperty.Register("Resolution", typeof(int), typeof(FunctionAnnotation), new PropertyMetadata(400));

        /// <summary>
        /// Initializes static members of the <see cref="FunctionAnnotation"/> class.
        /// </summary>
        static FunctionAnnotation()
        {
            TextColorProperty.OverrideMetadata(typeof(FunctionAnnotation), new FrameworkPropertyMetadata(MoreColors.Automatic, AppearanceChanged));
            TextHorizontalAlignmentProperty.OverrideMetadata(typeof(FunctionAnnotation), new FrameworkPropertyMetadata(HorizontalAlignment.Right, AppearanceChanged));
            TextVerticalAlignmentProperty.OverrideMetadata(typeof(FunctionAnnotation), new FrameworkPropertyMetadata(VerticalAlignment.Top, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAnnotation" /> class.
        /// </summary>
        public FunctionAnnotation()
        {
            this.InternalAnnotation = new Annotations.FunctionAnnotation();
        }

        /// <summary>
        /// Gets or sets the equation.
        /// </summary>
        /// <value>The equation.</value>
        public Func<double, double> Equation
        {
            get
            {
                return (Func<double, double>)this.GetValue(EquationProperty);
            }

            set
            {
                this.SetValue(EquationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public int Resolution
        {
            get
            {
                return (int)this.GetValue(ResolutionProperty);
            }

            set
            {
                this.SetValue(ResolutionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Type.
        /// </summary>
        public FunctionAnnotationType Type
        {
            get
            {
                return (FunctionAnnotationType)this.GetValue(TypeProperty);
            }

            set
            {
                this.SetValue(TypeProperty, value);
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

            var a = (Annotations.FunctionAnnotation)this.InternalAnnotation;
            a.Type = this.Type;
            a.Equation = this.Equation;
            a.Resolution = this.Resolution;
        }
    }
}