using System;
using System.Collections.Generic;
using System.Windows.Controls;
using OxyPlot.Annotations;

namespace OxyPlotControls.Factories;

/// <summary>
/// Factory for creating annotation editor controls based on annotation type.
/// Uses a registry pattern to allow dynamic extension with new annotation types.
/// </summary>
public static class AnnotationControlFactory
{
    private static readonly Dictionary<Type, Func<UserControl>> _controlFactories = new();
    private static bool _isInitialized = false;

    /// <summary>
    /// Registers a control factory for a specific annotation type.
    /// </summary>
    /// <typeparam name="TAnnotation">The annotation type.</typeparam>
    /// <param name="factory">Factory function that creates the control.</param>
    public static void Register<TAnnotation>(Func<UserControl> factory) where TAnnotation : Annotation
    {
        _controlFactories[typeof(TAnnotation)] = factory;
    }

    /// <summary>
    /// Creates an editor control for the specified annotation.
    /// </summary>
    /// <param name="annotation">The annotation to create an editor for.</param>
    /// <returns>A UserControl for editing the annotation, or a generic control if no specific editor exists.</returns>
    public static UserControl CreateControl(Annotation annotation)
    {
        ArgumentNullException.ThrowIfNull(annotation);

        EnsureInitialized();

        var annotationType = annotation.GetType();

        // Try exact type match first
        if (_controlFactories.TryGetValue(annotationType, out var factory))
        {
            return factory();
        }

        // Try base types (for derived annotation types)
        foreach (var kvp in _controlFactories)
        {
            if (kvp.Key.IsAssignableFrom(annotationType))
            {
                return kvp.Value();
            }
        }

        // Return generic control as fallback
        // TODO: Implement GenericAnnotationControl in Phase 5
        return new UserControl
        {
            Content = new TextBlock { Text = $"No editor for {annotationType.Name}" }
        };
    }

    /// <summary>
    /// Initializes the factory with default annotation control registrations.
    /// </summary>
    private static void EnsureInitialized()
    {
        if (_isInitialized) return;

        // TODO: Register annotation controls as they are implemented in Phase 5
        // Examples:
        // Register<LineAnnotation>(() => new LineAnnotationControl());
        // Register<ArrowAnnotation>(() => new ArrowAnnotationControl());
        // Register<TextAnnotation>(() => new TextAnnotationControl());
        // Register<PolygonAnnotation>(() => new PolygonAnnotationControl());
        // Register<ImageAnnotation>(() => new ImageAnnotationControl());
        // Register<FunctionAnnotation>(() => new FunctionAnnotationControl());
        // Register<PathAnnotation>(() => new PathAnnotationControl());
        // ... etc

        _isInitialized = true;
    }

    /// <summary>
    /// Gets all registered annotation types.
    /// </summary>
    /// <returns>Collection of registered annotation types.</returns>
    public static IEnumerable<Type> GetRegisteredAnnotationTypes()
    {
        EnsureInitialized();
        return _controlFactories.Keys;
    }

    /// <summary>
    /// Checks if a control is registered for the given annotation type.
    /// </summary>
    /// <param name="annotationType">The annotation type to check.</param>
    /// <returns>True if a control is registered, false otherwise.</returns>
    public static bool IsRegistered(Type annotationType)
    {
        ArgumentNullException.ThrowIfNull(annotationType);
        EnsureInitialized();
        return _controlFactories.ContainsKey(annotationType);
    }
}
