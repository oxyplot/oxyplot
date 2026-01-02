using OxyPlot.Series;
using OxyPlotControls.Controls.Base;

namespace OxyPlotControls.Controls.Series;

/// <summary>
/// Interaction logic for LineSeriesControl.xaml
/// Provides UI for editing LineSeries properties.
/// </summary>
public partial class LineSeriesControl : SeriesControlBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LineSeriesControl"/> class.
    /// </summary>
    public LineSeriesControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Called when the Series property changes.
    /// </summary>
    protected override void OnSeriesChanged(OxyPlot.Series.Series? oldValue, OxyPlot.Series.Series? newValue)
    {
        base.OnSeriesChanged(oldValue, newValue);

        // Validate that the series is a LineSeries
        if (newValue is not null and not LineSeries)
        {
            throw new ArgumentException($"LineSeriesControl can only edit LineSeries, but received {newValue.GetType().Name}");
        }
    }
}
