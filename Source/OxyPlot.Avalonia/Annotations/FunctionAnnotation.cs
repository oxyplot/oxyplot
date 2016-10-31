// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.PathAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Layout;
    using OxyPlot.Annotations;
    using System;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.PathAnnotation
    /// </summary>
    public class FunctionAnnotation : PathAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Type"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<FunctionAnnotationType> TypeProperty = AvaloniaProperty.Register<FunctionAnnotation, FunctionAnnotationType>(nameof(Type), FunctionAnnotationType.EquationX);

        /// <summary>
        /// Identifies the <see cref="Equation"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Func<double, double>> EquationProperty = AvaloniaProperty.Register<FunctionAnnotation, Func<double, double>>(nameof(Equation));

        /// <summary>
        /// Identifies the <see cref="Resolution"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<int> ResolutionProperty = AvaloniaProperty.Register<FunctionAnnotation, int>(nameof(Resolution), 400);

        /// <summary>
        /// Initializes static members of the <see cref="FunctionAnnotation"/> class.
        /// </summary>
        static FunctionAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<FunctionAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<FunctionAnnotation>(AppearanceChanged);
            TextHorizontalAlignmentProperty.OverrideDefaultValue<FunctionAnnotation>(HorizontalAlignment.Right);
            TextHorizontalAlignmentProperty.Changed.AddClassHandler<FunctionAnnotation>(AppearanceChanged);
            TextVerticalAlignmentProperty.OverrideDefaultValue<FunctionAnnotation>(VerticalAlignment.Top);
            TextVerticalAlignmentProperty.Changed.AddClassHandler<FunctionAnnotation>(AppearanceChanged);

            TypeProperty.Changed.AddClassHandler<FunctionAnnotation>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAnnotation" /> class.
        /// </summary>
        public FunctionAnnotation()
        {
            InternalAnnotation = new Annotations.FunctionAnnotation();
        }

        /// <summary>
        /// Gets or sets the equation.
        /// </summary>
        /// <value>The equation.</value>
        public Func<double, double> Equation
        {
            get
            {
                return GetValue(EquationProperty);
            }

            set
            {
                SetValue(EquationProperty, value);
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
                return GetValue(ResolutionProperty);
            }

            set
            {
                SetValue(ResolutionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Type.
        /// </summary>
        public FunctionAnnotationType Type
        {
            get
            {
                return GetValue(TypeProperty);
            }

            set
            {
                SetValue(TypeProperty, value);
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

            var a = (Annotations.FunctionAnnotation)InternalAnnotation;
            a.Type = Type;
            a.Equation = Equation;
            a.Resolution = Resolution;
        }
    }
}