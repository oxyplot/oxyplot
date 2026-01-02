using System.Windows;
using System.Windows.Controls;
using OxyPlot.Series;

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
            typeof(Series),
            typeof(SeriesControlBase),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSeriesChanged));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the Series being edited by this control.
    /// </summary>
    public Series? Series
    {
        get => (Series?)GetValue(SeriesProperty);
        set => SetValue(SeriesProperty, value);
    }

    #endregion

    #region Property Change Handlers

    /// <summary>
    /// Called when the <see cref="Series"/> property changes.
    /// </summary>
    /// <param name="oldValue">The old Series value.</param>
    /// <param name="newValue">The new Series value.</param>
    protected virtual void OnSeriesChanged(Series? oldValue, Series? newValue)
    {
        // Override in derived classes to handle series changes
    }

    private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SeriesControlBase control)
        {
            control.OnSeriesChanged(e.OldValue as Series, e.NewValue as Series);
        }
    }

    #endregion
}
