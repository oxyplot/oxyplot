using System.Windows;
using System.Windows.Controls;
using OxyPlot;

namespace OxyPlotControls.Controls.Base;

/// <summary>
/// Base class for series editor controls.
/// Provides common infrastructure for editing series properties.
/// </summary>
public abstract class SeriesControlBase : UserControl
{
    #region Dependency Properties

    /// <summary>
    /// Identifies the <see cref="Series"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SeriesProperty =
        DependencyProperty.Register(
            nameof(Series),
            typeof(OxyPlot.Series.Series),
            typeof(SeriesControlBase),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSeriesChanged));

    /// <summary>
    /// Identifies the <see cref="Model"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            nameof(Model),
            typeof(PlotModel),
            typeof(SeriesControlBase),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the Series being edited by this control.
    /// </summary>
    public OxyPlot.Series.Series? Series
    {
        get => (OxyPlot.Series.Series?)GetValue(SeriesProperty);
        set => SetValue(SeriesProperty, value);
    }

    /// <summary>
    /// Gets or sets the PlotModel that contains the series.
    /// </summary>
    public PlotModel? Model
    {
        get => (PlotModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    #endregion

    #region Property Change Handlers

    /// <summary>
    /// Called when the <see cref="Series"/> property changes.
    /// </summary>
    /// <param name="oldValue">The old Series value.</param>
    /// <param name="newValue">The new Series value.</param>
    protected virtual void OnSeriesChanged(OxyPlot.Series.Series? oldValue, OxyPlot.Series.Series? newValue)
    {
        // Override in derived classes to handle series changes
    }

    private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SeriesControlBase control)
        {
            control.OnSeriesChanged(e.OldValue as OxyPlot.Series.Series, e.NewValue as OxyPlot.Series.Series);
        }
    }

    #endregion
}
