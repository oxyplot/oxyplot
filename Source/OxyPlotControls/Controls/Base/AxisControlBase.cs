using System.Windows;
using System.Windows.Controls;
using OxyPlot;
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

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            nameof(Model),
            typeof(PlotModel),
            typeof(AxisControlBase),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public Axis? Axis
    {
        get => (Axis?)GetValue(AxisProperty);
        set => SetValue(AxisProperty, value);
    }

    public PlotModel? Model
    {
        get => (PlotModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }
}
