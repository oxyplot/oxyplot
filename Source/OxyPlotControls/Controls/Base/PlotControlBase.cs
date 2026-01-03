using System.Windows;
using System.Windows.Controls;
using OxyPlot;

namespace OxyPlotControls.Controls.Base;

/// <summary>
/// Base class for all OxyPlot property editor controls.
/// Provides common infrastructure for PlotModel binding and change notification.
/// </summary>
public class PlotControlBase : UserControl
{
    #region Dependency Properties

    /// <summary>
    /// Identifies the <see cref="Model"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            nameof(Model),
            typeof(PlotModel),
            typeof(PlotControlBase),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnModelChanged));

    /// <summary>
    /// Identifies the <see cref="ExpanderStyle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ExpanderStyleProperty =
        DependencyProperty.Register(
            nameof(ExpanderStyle),
            typeof(Style),
            typeof(PlotControlBase),
            new PropertyMetadata(null));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the PlotModel being edited by this control.
    /// </summary>
    public PlotModel? Model
    {
        get => (PlotModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    /// <summary>
    /// Gets or sets the style to apply to Expander controls within this control.
    /// </summary>
    public Style? ExpanderStyle
    {
        get => (Style?)GetValue(ExpanderStyleProperty);
        set => SetValue(ExpanderStyleProperty, value);
    }

    #endregion

    #region Property Change Handlers

    /// <summary>
    /// Called when the <see cref="Model"/> property changes.
    /// </summary>
    /// <param name="oldValue">The old PlotModel value.</param>
    /// <param name="newValue">The new PlotModel value.</param>
    protected virtual void OnModelChanged(PlotModel? oldValue, PlotModel? newValue)
    {
        // Override in derived classes to handle model changes
    }

    private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PlotControlBase control)
        {
            control.OnModelChanged(e.OldValue as PlotModel, e.NewValue as PlotModel);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Invalidates the plot to trigger a re-render.
    /// </summary>
    /// <param name="updateData">If true, data will be updated before rendering.</param>
    protected void InvalidatePlot(bool updateData = true)
    {
        Model?.InvalidatePlot(updateData);
    }

    #endregion
}
