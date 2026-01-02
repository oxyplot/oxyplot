using System.Windows;
using System.Windows.Controls;
using OxyPlot;

namespace OxyPlotControls;

/// <summary>
/// Master properties control for OxyPlot.
/// Provides unified interface to edit General/Legend/Axes/Series/Annotations properties.
/// </summary>
public partial class OxyplotPropertiesControl : UserControl
{
    public OxyplotPropertiesControl()
    {
        InitializeComponent();
    }

    #region Dependency Properties

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            nameof(Model),
            typeof(PlotModel),
            typeof(OxyplotPropertiesControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Gets or sets the PlotModel to edit.
    /// </summary>
    public PlotModel? Model
    {
        get => (PlotModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    #endregion

    #region Section Navigation

    private void PropertySectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PropertySectionComboBox.SelectedItem is ComboBoxItem item)
        {
            var section = item.Tag as string;

            // Hide all sections
            GeneralControl.Visibility = Visibility.Collapsed;
            LegendControl.Visibility = Visibility.Collapsed;
            AxesControl.Visibility = Visibility.Collapsed;
            SeriesControl.Visibility = Visibility.Collapsed;
            AnnotationsControl.Visibility = Visibility.Collapsed;

            // Show selected section
            switch (section)
            {
                case "General":
                    GeneralControl.Visibility = Visibility.Visible;
                    break;
                case "Legend":
                    LegendControl.Visibility = Visibility.Visible;
                    break;
                case "Axes":
                    AxesControl.Visibility = Visibility.Visible;
                    break;
                case "Series":
                    SeriesControl.Visibility = Visibility.Visible;
                    break;
                case "Annotations":
                    AnnotationsControl.Visibility = Visibility.Visible;
                    break;
            }
        }
    }

    /// <summary>
    /// Navigates to the General Settings section.
    /// </summary>
    public void ShowGeneralSettings()
    {
        PropertySectionComboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Navigates to the Legend section.
    /// </summary>
    public void ShowLegend()
    {
        PropertySectionComboBox.SelectedIndex = 1;
    }

    /// <summary>
    /// Navigates to the Axes section.
    /// </summary>
    public void ShowAxes()
    {
        PropertySectionComboBox.SelectedIndex = 2;
    }

    /// <summary>
    /// Navigates to the Series section.
    /// </summary>
    public void ShowSeries()
    {
        PropertySectionComboBox.SelectedIndex = 3;
    }

    /// <summary>
    /// Navigates to the Annotations section.
    /// </summary>
    public void ShowAnnotations()
    {
        PropertySectionComboBox.SelectedIndex = 4;
    }

    #endregion
}
