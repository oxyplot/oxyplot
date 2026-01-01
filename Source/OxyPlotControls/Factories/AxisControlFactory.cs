using System;
using System.Collections.Generic;
using System.Windows.Controls;
using OxyPlot.Axes;

namespace OxyPlotControls.Factories;

/// <summary>
/// Factory for creating axis editor controls based on axis type.
/// Uses a registry pattern to allow dynamic extension with new axis types.
/// </summary>
public static class AxisControlFactory
{
    private static readonly Dictionary<Type, Func<UserControl>> _controlFactories = new();
    private static bool _isInitialized = false;

    /// <summary>
    /// Registers a control factory for a specific axis type.
    /// </summary>
    /// <typeparam name="TAxis">The axis type.</typeparam>
    /// <param name="factory">Factory function that creates the control.</param>
    public static void Register<TAxis>(Func<UserControl> factory) where TAxis : Axis
    {
        _controlFactories[typeof(TAxis)] = factory;
    }

    /// <summary>
    /// Creates an editor control for the specified axis.
    /// </summary>
    /// <param name="axis">The axis to create an editor for.</param>
    /// <returns>A UserControl for editing the axis, or a generic control if no specific editor exists.</returns>
    public static UserControl CreateControl(Axis axis)
    {
        ArgumentNullException.ThrowIfNull(axis);

        EnsureInitialized();

        var axisType = axis.GetType();

        // Try exact type match first
        if (_controlFactories.TryGetValue(axisType, out var factory))
        {
            return factory();
        }

        // Try base types (for derived axis types)
        foreach (var kvp in _controlFactories)
        {
            if (kvp.Key.IsAssignableFrom(axisType))
            {
                return kvp.Value();
            }
        }

        // Return generic control as fallback
        // TODO: Implement GenericAxisControl in Phase 4
        return new UserControl
        {
            Content = new TextBlock { Text = $"No editor for {axisType.Name}" }
        };
    }

    /// <summary>
    /// Initializes the factory with default axis control registrations.
    /// </summary>
    private static void EnsureInitialized()
    {
        if (_isInitialized) return;

        // TODO: Register axis controls as they are implemented in Phase 4
        // Examples:
        // Register<LinearAxis>(() => new LinearAxisControl());
        // Register<LogarithmicAxis>(() => new LogarithmicAxisControl());
        // Register<DateTimeAxis>(() => new DateTimeAxisControl());
        // Register<CategoryAxis>(() => new CategoryAxisControl());
        // Register<TimeSpanAxis>(() => new TimeSpanAxisControl());
        // Register<NormalProbabilityAxis>(() => new NormalProbabilityAxisControl());
        // Register<GumbelProbabilityAxis>(() => new GumbelProbabilityAxisControl());
        // Register<LinearColorAxis>(() => new LinearColorAxisControl());
        // Register<RangeColorAxis>(() => new RangeColorAxisControl());
        // Register<AngleAxis>(() => new AngleAxisControl());
        // Register<MagnitudeAxis>(() => new MagnitudeAxisControl());
        // ... etc

        _isInitialized = true;
    }

    /// <summary>
    /// Gets all registered axis types.
    /// </summary>
    /// <returns>Collection of registered axis types.</returns>
    public static IEnumerable<Type> GetRegisteredAxisTypes()
    {
        EnsureInitialized();
        return _controlFactories.Keys;
    }

    /// <summary>
    /// Checks if a control is registered for the given axis type.
    /// </summary>
    /// <param name="axisType">The axis type to check.</param>
    /// <returns>True if a control is registered, false otherwise.</returns>
    public static bool IsRegistered(Type axisType)
    {
        ArgumentNullException.ThrowIfNull(axisType);
        EnsureInitialized();
        return _controlFactories.ContainsKey(axisType);
    }
}
