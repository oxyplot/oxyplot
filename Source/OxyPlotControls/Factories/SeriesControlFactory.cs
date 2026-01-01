using System;
using System.Collections.Generic;
using System.Windows.Controls;
using OxyPlot.Series;

namespace OxyPlotControls.Factories;

/// <summary>
/// Factory for creating series editor controls based on series type.
/// Uses a registry pattern to allow dynamic extension with new series types.
/// </summary>
/// <remarks>
/// This replaces the hard-coded type switch statements in the VB code,
/// making it easy to add new series types without modifying existing code.
/// </remarks>
public static class SeriesControlFactory
{
    private static readonly Dictionary<Type, Func<UserControl>> _controlFactories = new();
    private static bool _isInitialized = false;

    /// <summary>
    /// Registers a control factory for a specific series type.
    /// </summary>
    /// <typeparam name="TSeries">The series type.</typeparam>
    /// <param name="factory">Factory function that creates the control.</param>
    public static void Register<TSeries>(Func<UserControl> factory) where TSeries : Series
    {
        _controlFactories[typeof(TSeries)] = factory;
    }

    /// <summary>
    /// Creates an editor control for the specified series.
    /// </summary>
    /// <param name="series">The series to create an editor for.</param>
    /// <returns>A UserControl for editing the series, or a generic control if no specific editor exists.</returns>
    public static UserControl CreateControl(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);

        EnsureInitialized();

        var seriesType = series.GetType();

        // Try exact type match first
        if (_controlFactories.TryGetValue(seriesType, out var factory))
        {
            return factory();
        }

        // Try base types (for derived series types)
        foreach (var kvp in _controlFactories)
        {
            if (kvp.Key.IsAssignableFrom(seriesType))
            {
                return kvp.Value();
            }
        }

        // Return generic control as fallback
        // TODO: Implement GenericSeriesControl in Phase 3
        return new UserControl
        {
            Content = new TextBlock { Text = $"No editor for {seriesType.Name}" }
        };
    }

    /// <summary>
    /// Initializes the factory with default series control registrations.
    /// </summary>
    private static void EnsureInitialized()
    {
        if (_isInitialized) return;

        // Register series controls
        Register<LineSeries>(() => new Controls.Series.LineSeriesControl());
        Register<BarSeries>(() => new Controls.Series.BarSeriesControl());
        Register<ColumnSeries>(() => new Controls.Series.BarSeriesControl()); // Use BarSeries control for columns
        Register<ScatterSeries>(() => new Controls.Series.ScatterSeriesControl());
        Register<AreaSeries>(() => new Controls.Series.AreaSeriesControl());
        Register<PieSeries>(() => new Controls.Series.PieSeriesControl());

        _isInitialized = true;
    }

    /// <summary>
    /// Gets all registered series types.
    /// </summary>
    /// <returns>Collection of registered series types.</returns>
    public static IEnumerable<Type> GetRegisteredSeriesTypes()
    {
        EnsureInitialized();
        return _controlFactories.Keys;
    }

    /// <summary>
    /// Checks if a control is registered for the given series type.
    /// </summary>
    /// <param name="seriesType">The series type to check.</param>
    /// <returns>True if a control is registered, false otherwise.</returns>
    public static bool IsRegistered(Type seriesType)
    {
        ArgumentNullException.ThrowIfNull(seriesType);
        EnsureInitialized();
        return _controlFactories.ContainsKey(seriesType);
    }
}
