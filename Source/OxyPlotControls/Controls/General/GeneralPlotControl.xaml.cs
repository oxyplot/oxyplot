using OxyPlot;
using OxyPlotControls.Controls.Base;

namespace OxyPlotControls.Controls.General;

/// <summary>
/// Interaction logic for GeneralPlotControl.xaml
/// Provides UI for editing general PlotModel properties including title, subtitle, plot area, and background.
/// </summary>
public partial class GeneralPlotControl : PlotControlBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralPlotControl"/> class.
    /// </summary>
    public GeneralPlotControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Called when the Model property changes.
    /// </summary>
    /// <param name="oldValue">The old PlotModel.</param>
    /// <param name="newValue">The new PlotModel.</param>
    protected override void OnModelChanged(PlotModel? oldValue, PlotModel? newValue)
    {
        base.OnModelChanged(oldValue, newValue);

        // Future: Could add property change listeners here if needed
        // For now, bindings handle all updates automatically
    }
}
