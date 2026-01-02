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
    }

    /// <summary>
    /// Gets the axis as a LogarithmicAxis for binding to log-specific properties.
    /// </summary>
    public LogarithmicAxis? LogAxis => Axis as LogarithmicAxis;

    /// <summary>
    /// Notifies property changed when Axis changes to update LogAxis binding.
    /// </summary>
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == AxisProperty)
        {
            // Notify that LogAxis has also changed
            OnPropertyChanged(new DependencyPropertyChangedEventArgs(
                LogAxisProperty, e.OldValue as LogarithmicAxis, e.NewValue as LogarithmicAxis));
        }
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
