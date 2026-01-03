using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using OxyPlot;
using OxyPlotControls.Controls.General;
using OxyPlotControls.Controls.Legend;
using OxyPlotControls.Controls.Selectors;

namespace OxyPlotControls;

/// <summary>
/// Master properties control for OxyPlot.
/// Provides unified interface to edit General/Legend/Axes/Series/Annotations properties.
/// Uses lazy loading to improve initialization performance.
/// </summary>
public partial class OxyplotPropertiesControl : UserControl
{
    #region Lazy-loaded Controls

    private GeneralPlotControl? _generalControl;
    private LegendControl? _legendControl;
    private AxesControl? _axesControl;
    private SeriesSelectorControl? _seriesControl;
    private AnnotationSelectorControl? _annotationsControl;

    #endregion

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

    public static readonly DependencyProperty ShowCloseButtonProperty =
        DependencyProperty.Register(
            nameof(ShowCloseButton),
            typeof(bool),
            typeof(OxyplotPropertiesControl),
            new PropertyMetadata(true));

    /// <summary>
    /// Gets or sets whether to show the close button.
    /// </summary>
    public bool ShowCloseButton
    {
        get => (bool)GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    #endregion

    #region Events

    /// <summary>
    /// Raised when the close button is clicked.
    /// </summary>
    public event EventHandler<EventArgs>? ClosePropertiesCalled;

    private void ClosePropertiesButton_Click(object sender, RoutedEventArgs e)
    {
        ClosePropertiesCalled?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Section Navigation with Lazy Loading

    private void PropertySectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PropertySectionComboBox.SelectedItem is not ComboBoxItem item) return;

        var section = item.Tag as string;

        // Hide all loaded controls
        if (_generalControl != null) _generalControl.Visibility = Visibility.Collapsed;
        if (_legendControl != null) _legendControl.Visibility = Visibility.Collapsed;
        if (_axesControl != null) _axesControl.Visibility = Visibility.Collapsed;
        if (_seriesControl != null) _seriesControl.Visibility = Visibility.Collapsed;
        if (_annotationsControl != null) _annotationsControl.Visibility = Visibility.Collapsed;

        // Show/create selected section (lazy loading)
        switch (section)
        {
            case "General":
                if (_generalControl == null)
                {
                    _generalControl = new GeneralPlotControl { MinWidth = 200, MinHeight = 200 };
                    BindingOperations.SetBinding(_generalControl, GeneralPlotControl.ModelProperty,
                        new Binding(nameof(Model)) { Source = this });
                    PropertyContentGrid.Children.Add(_generalControl);
                }
                _generalControl.Visibility = Visibility.Visible;
                break;

            case "Legend":
                if (_legendControl == null)
                {
                    _legendControl = new LegendControl { MinWidth = 200, MinHeight = 200 };
                    BindingOperations.SetBinding(_legendControl, LegendControl.ModelProperty,
                        new Binding(nameof(Model)) { Source = this });
                    PropertyContentGrid.Children.Add(_legendControl);
                }
                _legendControl.Visibility = Visibility.Visible;
                break;

            case "Axes":
                if (_axesControl == null)
                {
                    _axesControl = new AxesControl { MinWidth = 200, MinHeight = 200 };
                    BindingOperations.SetBinding(_axesControl, AxesControl.ModelProperty,
                        new Binding(nameof(Model)) { Source = this });
                    PropertyContentGrid.Children.Add(_axesControl);
                }
                _axesControl.Visibility = Visibility.Visible;
                break;

            case "Series":
                if (_seriesControl == null)
                {
                    _seriesControl = new SeriesSelectorControl { MinWidth = 200, MinHeight = 200 };
                    BindingOperations.SetBinding(_seriesControl, SeriesSelectorControl.ModelProperty,
                        new Binding(nameof(Model)) { Source = this });
                    PropertyContentGrid.Children.Add(_seriesControl);
                }
                _seriesControl.Visibility = Visibility.Visible;
                break;

            case "Annotations":
                if (_annotationsControl == null)
                {
                    _annotationsControl = new AnnotationSelectorControl { MinWidth = 200, MinHeight = 200 };
                    BindingOperations.SetBinding(_annotationsControl, AnnotationSelectorControl.ModelProperty,
                        new Binding(nameof(Model)) { Source = this });
                    PropertyContentGrid.Children.Add(_annotationsControl);
                }
                _annotationsControl.Visibility = Visibility.Visible;
                break;
        }
    }

    /// <summary>
    /// Navigates to a specific property expander, optionally selecting an object.
    /// </summary>
    /// <param name="expander">The property expander to navigate to.</param>
    /// <param name="selectedObject">Optional object to select (axis, series, or annotation).</param>
    public void ExpandProperty(PropertyExpander expander, object? selectedObject = null)
    {
        var name = expander.ToString();

        if (name.StartsWith("General_"))
        {
            PropertySectionComboBox.SelectedIndex = 0;
        }
        else if (name.StartsWith("Legend_"))
        {
            PropertySectionComboBox.SelectedIndex = 1;
        }
        else if (name.StartsWith("Axes_"))
        {
            PropertySectionComboBox.SelectedIndex = 2;
            // TODO: Select the specific axis and expand the appropriate section
        }
        else if (name.StartsWith("Series_"))
        {
            PropertySectionComboBox.SelectedIndex = 3;
            // TODO: Select the specific series and expand the appropriate section
        }
        else if (name.StartsWith("Annotations_"))
        {
            PropertySectionComboBox.SelectedIndex = 4;
            // TODO: Select the specific annotation and expand the appropriate section
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

/// <summary>
/// Enum defining all expandable property sections.
/// Matches the VB PropertyEXP enum for backward compatibility.
/// </summary>
public enum PropertyExpander
{
    // General section
    General_PlotTitle,
    General_PlotSubtitle,
    General_PlotArea,
    General_PlotBackground,

    // Legend section
    Legend_Title,
    Legend_Items,
    Legend_Area,
    Legend_Position,

    // Axes section
    Axes_Options,
    Axes_Display,
    Axes_Title,
    Axes_Labels,
    Axes_MajorGridLines,
    Axes_MinorGridLines,
    Axes_TickOptions,

    // Series section
    Series_General,
    Series_Display,
    Series_Markers,
    Series_BoxAndWhiskers,
    Series_ErrorBarSettings,

    // Annotations section
    Annotations_Text,
    Annotations_Display
}
