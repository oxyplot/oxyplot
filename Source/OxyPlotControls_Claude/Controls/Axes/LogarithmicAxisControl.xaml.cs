using System.Windows;
using OxyPlot.Axes;
using OxyPlotControls.Controls.Base;

namespace OxyPlotControls.Controls.Axes;

/// <summary>
/// Editor control for LogarithmicAxis properties.
/// Provides access to Base and PowerPadding properties specific to logarithmic axes.
/// </summary>
public partial class LogarithmicAxisControl : AxisControlBase
{
    public LogarithmicAxisControl()
    {
        InitializeComponent();

        // Update LogAxis when Axis changes
        var dpd = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(
            AxisProperty, typeof(AxisControlBase));
        dpd?.AddValueChanged(this, (s, e) => UpdateLogAxis());
    }

    /// <summary>
    /// Gets the axis as a LogarithmicAxis for binding to log-specific properties.
    /// </summary>
    public LogarithmicAxis? LogAxis
    {
        get => (LogarithmicAxis?)GetValue(LogAxisProperty);
        private set => SetValue(LogAxisPropertyKey, value);
    }

    /// <summary>
    /// Updates the LogAxis property when Axis changes.
    /// </summary>
    private void UpdateLogAxis()
    {
        LogAxis = Axis as LogarithmicAxis;
    }

    /// <summary>
    /// Dependency property for LogAxis (read-only, derived from Axis).
    /// </summary>
    private static readonly DependencyPropertyKey LogAxisPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(LogAxis),
            typeof(LogarithmicAxis),
            typeof(LogarithmicAxisControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty LogAxisProperty = LogAxisPropertyKey.DependencyProperty;
}
