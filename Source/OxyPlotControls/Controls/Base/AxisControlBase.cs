using System.Windows;
using System.Windows.Controls;
using OxyPlot.Axes;

namespace OxyPlotControls.Controls.Base;

/// <summary>
/// Base class for axis editor controls.
/// </summary>
public abstract class AxisControlBase : UserControl
{
    public static readonly DependencyProperty AxisProperty =
        DependencyProperty.Register(
            nameof(Axis),
            typeof(Axis),
            typeof(AxisControlBase),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public Axis? Axis
    {
        get => (Axis?)GetValue(AxisProperty);
        set => SetValue(AxisProperty, value);
    }
}
