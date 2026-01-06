using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using OxyPlot;

namespace OxyPlotControls
{
    /// <summary>
    /// A comprehensive user control that provides a unified interface for editing all OxyPlot chart properties.
    /// This control uses lazy loading to improve initialization times by only creating child controls when they are first accessed.
    /// </summary>
    public partial class OxyPlotPropertiesControl : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="PlotModel"/> dependency property.
        /// </summary>
        public static DependencyProperty PlotModelProperty = DependencyProperty.Register(
            nameof(PlotModel), typeof(OxyPlot.PlotModel), typeof(OxyPlotPropertiesControl),
            new PropertyMetadata(null, InitializePlotModel));

        private static void InitializePlotModel(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (d.GetType() != typeof(OxyPlotPropertiesControl)) return;
            var thisControl = (OxyPlotPropertiesControl)d;

            if (e.NewValue == null) return;
            // Use fully qualified type name to avoid ambiguity with the PlotModel property
            if (e.NewValue is not OxyPlot.PlotModel) return;
        }

        /// <summary>
        /// Gets or sets the PlotModel that this control edits.
        /// </summary>
        public PlotModel PlotModel
        {
            get { return (PlotModel)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ShowCloseButton"/> dependency property.
        /// </summary>
        public static DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            nameof(ShowCloseButton), typeof(bool), typeof(OxyPlotPropertiesControl),
            new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets whether the close button is visible.
        /// </summary>
        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        /// <summary>
        /// Occurs when the close properties button is clicked.
        /// </summary>
        public event Action<OxyPlotPropertiesControl> ClosePropertiesCalled;

        private void SetDefaultStyles()
        {
            if (BackButtonStyle == null) BackButtonStyle = (Style)FindResource("CleanButtonStyle");
            if (PropertyControlComboBoxStyle == null) PropertyControlComboBoxStyle = (Style)FindResource("CleanComboBoxStyle");
            if (ExpanderStyle == null) ExpanderStyle = (Style)FindResource("ExcelExpanderStyle");
            if (TabItemStyle == null) TabItemStyle = (Style)FindResource("CustomTabItemStyle");
        }

        /// <summary>
        /// Identifies the <see cref="BackButtonStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty BackButtonStyleProperty = DependencyProperty.Register(
            nameof(BackButtonStyle), typeof(Style), typeof(OxyPlotPropertiesControl),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the style for the back/close button.
        /// </summary>
        public Style BackButtonStyle
        {
            get { return (Style)GetValue(BackButtonStyleProperty); }
            set { SetValue(BackButtonStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabItemStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty TabItemStyleProperty = DependencyProperty.Register(
            nameof(TabItemStyle), typeof(Style), typeof(OxyPlotPropertiesControl),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the style for tab items.
        /// </summary>
        public Style TabItemStyle
        {
            get { return (Style)GetValue(TabItemStyleProperty); }
            set { SetValue(TabItemStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle), typeof(Style), typeof(OxyPlotPropertiesControl),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the style for expander controls.
        /// </summary>
        public Style ExpanderStyle
        {
            get { return (Style)GetValue(ExpanderStyleProperty); }
            set { SetValue(ExpanderStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PropertyControlComboBoxStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty PropertyControlComboBoxStyleProperty = DependencyProperty.Register(
            nameof(PropertyControlComboBoxStyle), typeof(Style), typeof(OxyPlotPropertiesControl),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the style for the property control combo box.
        /// </summary>
        public Style PropertyControlComboBoxStyle
        {
            get { return (Style)GetValue(PropertyControlComboBoxStyleProperty); }
            set { SetValue(PropertyControlComboBoxStyleProperty, value); }
        }

        // Lazy loading to improve initialization times.
        private GeneralPlotControl _generalControls;
        private LegendControl _legendControls;
        private AxesControl _axesControls;
        private SeriesSelectorControl _seriesControls;
        private AnnotationSelectorControl _annotationsControls;

        private Binding _viewPortWidthBinding;
        private Binding _plotBinding;
        private Binding _expanderBinding;
        private Binding _tabItemStyleBinding;
        private Binding _comboboxStyleBinding;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlotPropertiesControl"/> class.
        /// </summary>
        public OxyPlotPropertiesControl()
        {
            GenericControls.PropertyDefaults.DefaultMaxPropertyWidth = 200;

            InitializeComponent();

            // Initialize bindings after InitializeComponent
            _viewPortWidthBinding = new Binding(nameof(ScrollViewer.ViewportWidth)) { Source = ControlScrollViewer };
            _plotBinding = new Binding(nameof(PlotModel)) { Source = this };
            _expanderBinding = new Binding(nameof(ExpanderStyle)) { Source = this };
            _tabItemStyleBinding = new Binding(nameof(TabItemStyle)) { Source = this };
            _comboboxStyleBinding = new Binding(nameof(PropertyControlComboBoxStyle)) { Source = this };

            SetDefaultStyles();

            // Re-trigger the selection change now that bindings are ready
            // (the initial SelectionChanged fired during InitializeComponent was skipped)
            PropertyControlComboBox_SelectionChanged(PropertyControlComboBox, null);
        }

        /// <summary>
        /// Selects the general settings properties from the control dropdown menu.
        /// </summary>
        public void SelectGeneralSettings()
        {
            if (PropertyControlComboBox != null) PropertyControlComboBox.SelectedIndex = 0;
        }

        private string ToEnumName<T>(int value) where T : struct
        {
            return ((T)(object)value).ToString();
        }

        /// <summary>
        /// Deletes an annotation from the plot.
        /// </summary>
        /// <param name="anno">The annotation to delete.</param>
        public void DeleteAnnotation(OxyPlot.Annotations.Annotation anno)
        {
            // Implementation placeholder
        }

        /// <summary>
        /// Expands a specific property section and optionally selects a specific object.
        /// </summary>
        /// <param name="prop">The property section to expand.</param>
        /// <param name="selectedObject">Optional object to select within the property section.</param>
        public void ExpandProperty(PropertyEXP prop, object selectedObject = null)
        {
            string str = ToEnumName<PropertyEXP>((int)prop);

            if (str.Contains("General_"))
            {
                PropertyControlComboBox.SelectedIndex = 0;
                switch (prop)
                {
                    case PropertyEXP.General_PlotTitle:
                        _generalControls.PlotTitleEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.General_PlotSubtitle:
                        _generalControls.PlotSubtitleEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.General_PlotArea:
                        _generalControls.PlotAreaEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.General_PlotBackground:
                        _generalControls.PlotBackgroundEXP.IsExpanded = true;
                        break;
                }
            }
            else if (str.Contains("Legend_"))
            {
                PropertyControlComboBox.SelectedIndex = 1;
                switch (prop)
                {
                    case PropertyEXP.Legend_Title:
                        _legendControls.LegendTitleEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Legend_Items:
                        _legendControls.LegendItemsEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Legend_Area:
                        _legendControls.LegendAreaEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Legend_Position:
                        _legendControls.LegendPositionEXP.IsExpanded = true;
                        break;
                }
            }
            else if (str.Contains("Axes_"))
            {
                if (selectedObject == null) return;
                PropertyControlComboBox.SelectedIndex = 2;
                _axesControls.AxesPropertyControlComboBox.SelectedItem = selectedObject;

                switch (prop)
                {
                    case PropertyEXP.Axes_Options:
                        _axesControls.AxisPropertiesControl.AxisTabControl.SelectedItem = _axesControls.AxisPropertiesControl.GeneralTab;
                        _axesControls.AxisPropertiesControl.GeneralEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Axes_Display:
                        _axesControls.AxisPropertiesControl.AxisTabControl.SelectedItem = _axesControls.AxisPropertiesControl.GeneralTab;
                        _axesControls.AxisPropertiesControl.DisplayEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Axes_Title:
                        _axesControls.AxisPropertiesControl.AxisTabControl.SelectedItem = _axesControls.AxisPropertiesControl.LabelsTab;
                        _axesControls.AxisPropertiesControl.TitleEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Axes_Labels:
                        _axesControls.AxisPropertiesControl.AxisTabControl.SelectedItem = _axesControls.AxisPropertiesControl.LabelsTab;
                        _axesControls.AxisPropertiesControl.LabelsEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Axes_MajorGridLines:
                        _axesControls.AxisPropertiesControl.AxisTabControl.SelectedItem = _axesControls.AxisPropertiesControl.GridLinesTab;
                        _axesControls.AxisPropertiesControl.MajorGridLinesEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Axes_MinorGridLines:
                        _axesControls.AxisPropertiesControl.AxisTabControl.SelectedItem = _axesControls.AxisPropertiesControl.GridLinesTab;
                        _axesControls.AxisPropertiesControl.MinorGridLinesEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Axes_TickOptions:
                        _axesControls.AxisPropertiesControl.AxisTabControl.SelectedItem = _axesControls.AxisPropertiesControl.GridLinesTab;
                        _axesControls.AxisPropertiesControl.TickOptionsEXP.IsExpanded = true;
                        break;
                }
            }
            else if (str.Contains("Series_"))
            {
                PropertyControlComboBox.SelectedIndex = 3;
                _seriesControls.SeriesPropertyControlComboBox.SelectedItem = selectedObject;
                _seriesControls.SeriesPropertiesControl.Expand(prop);
            }
            else if (str.Contains("Annotations_"))
            {
                PropertyControlComboBox.SelectedIndex = 4;
                _annotationsControls.AnnotationPropertyControlComboBox.SelectedItem = selectedObject;

                switch (prop)
                {
                    case PropertyEXP.Annotations_Text:
                        _annotationsControls.AnnotationPropertiesControl.TextEXP.IsExpanded = true;
                        break;
                    case PropertyEXP.Annotations_Display:
                        _annotationsControls.AnnotationPropertiesControl.DisplayOptionsEXP.IsExpanded = true;
                        break;
                }
            }
        }

        private void ClosePropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            ClosePropertiesCalled?.Invoke(this);
        }

        /// <summary>
        /// Enumeration of expandable property sections.
        /// </summary>
        public enum PropertyEXP
        {
            /// <summary>Plot title section.</summary>
            General_PlotTitle,
            /// <summary>Plot subtitle section.</summary>
            General_PlotSubtitle,
            /// <summary>Plot area section.</summary>
            General_PlotArea,
            /// <summary>Plot background section.</summary>
            General_PlotBackground,
            /// <summary>Legend title section.</summary>
            Legend_Title,
            /// <summary>Legend items section.</summary>
            Legend_Items,
            /// <summary>Legend area section.</summary>
            Legend_Area,
            /// <summary>Legend position section.</summary>
            Legend_Position,
            /// <summary>Axes options section.</summary>
            Axes_Options,
            /// <summary>Axes display section.</summary>
            Axes_Display,
            /// <summary>Axes title section.</summary>
            Axes_Title,
            /// <summary>Axes labels section.</summary>
            Axes_Labels,
            /// <summary>Major grid lines section.</summary>
            Axes_MajorGridLines,
            /// <summary>Minor grid lines section.</summary>
            Axes_MinorGridLines,
            /// <summary>Tick options section.</summary>
            Axes_TickOptions,
            /// <summary>Series general section.</summary>
            Series_General,
            /// <summary>Series display section.</summary>
            Series_Display,
            /// <summary>Series markers section.</summary>
            Series_Markers,
            /// <summary>Box and whiskers section.</summary>
            Series_BoxAndWhiskers,
            /// <summary>Error bar settings section.</summary>
            Series_ErrorBarSettings,
            /// <summary>Annotations text section.</summary>
            Annotations_Text,
            /// <summary>Annotations display section.</summary>
            Annotations_Display
        }

        private void PropertyControlComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PropertyControlsGrid == null) return;
            // Bindings are initialized after InitializeComponent, so they may be null during initial XAML load
            if (_viewPortWidthBinding == null) return;

            if (_generalControls != null) _generalControls.Visibility = Visibility.Collapsed;
            if (_legendControls != null) _legendControls.Visibility = Visibility.Collapsed;
            if (_axesControls != null) _axesControls.Visibility = Visibility.Collapsed;
            if (_seriesControls != null) _seriesControls.Visibility = Visibility.Collapsed;
            if (_annotationsControls != null) _annotationsControls.Visibility = Visibility.Collapsed;

            switch (PropertyControlComboBox.SelectedIndex)
            {
                case 0: // General
                    if (_generalControls == null)
                    {
                        _generalControls = new GeneralPlotControl { MinWidth = 200, MinHeight = 200 };
                        BindingOperations.SetBinding(_generalControls, WidthProperty, _viewPortWidthBinding);
                        BindingOperations.SetBinding(_generalControls, GeneralPlotControl.PlotModelProperty, _plotBinding);
                        BindingOperations.SetBinding(_generalControls, GeneralPlotControl.ExpanderStyleProperty, _expanderBinding);
                        PropertyControlsGrid.Children.Add(_generalControls);
                    }
                    _generalControls.Visibility = Visibility.Visible;
                    break;

                case 1: // Legend
                    if (_legendControls == null)
                    {
                        _legendControls = new LegendControl { MinWidth = 200, MinHeight = 200 };
                        BindingOperations.SetBinding(_legendControls, WidthProperty, _viewPortWidthBinding);
                        BindingOperations.SetBinding(_legendControls, LegendControl.PlotModelProperty, _plotBinding);
                        BindingOperations.SetBinding(_legendControls, LegendControl.ExpanderStyleProperty, _expanderBinding);
                        PropertyControlsGrid.Children.Add(_legendControls);
                    }
                    _legendControls.Visibility = Visibility.Visible;
                    break;

                case 2: // Axes
                    if (_axesControls == null)
                    {
                        _axesControls = new AxesControl { MinWidth = 200, MinHeight = 200 };
                        BindingOperations.SetBinding(_axesControls, WidthProperty, _viewPortWidthBinding);
                        BindingOperations.SetBinding(_axesControls, AxesControl.PlotModelProperty, _plotBinding);
                        BindingOperations.SetBinding(_axesControls, AxesControl.ExpanderStyleProperty, _expanderBinding);
                        BindingOperations.SetBinding(_axesControls, AxesControl.TabItemStyleProperty, _tabItemStyleBinding);
                        BindingOperations.SetBinding(_axesControls, AxesControl.ComboBoxStyleProperty, _comboboxStyleBinding);
                        PropertyControlsGrid.Children.Add(_axesControls);
                    }
                    _axesControls.Visibility = Visibility.Visible;
                    break;

                case 3: // Series
                    if (_seriesControls == null)
                    {
                        _seriesControls = new SeriesSelectorControl { MinWidth = 200, MinHeight = 200 };
                        BindingOperations.SetBinding(_seriesControls, WidthProperty, _viewPortWidthBinding);
                        BindingOperations.SetBinding(_seriesControls, SeriesSelectorControl.PlotModelProperty, _plotBinding);
                        BindingOperations.SetBinding(_seriesControls, SeriesSelectorControl.ExpanderStyleProperty, _expanderBinding);
                        BindingOperations.SetBinding(_seriesControls, SeriesSelectorControl.ComboBoxStyleProperty, _comboboxStyleBinding);
                        PropertyControlsGrid.Children.Add(_seriesControls);
                    }
                    _seriesControls.Visibility = Visibility.Visible;
                    break;

                case 4: // Annotations
                    if (_annotationsControls == null)
                    {
                        _annotationsControls = new AnnotationSelectorControl { MinWidth = 200, MinHeight = 200 };
                        BindingOperations.SetBinding(_annotationsControls, WidthProperty, _viewPortWidthBinding);
                        BindingOperations.SetBinding(_annotationsControls, AnnotationSelectorControl.PlotModelProperty, _plotBinding);
                        BindingOperations.SetBinding(_annotationsControls, AnnotationSelectorControl.ExpanderStyleProperty, _expanderBinding);
                        BindingOperations.SetBinding(_annotationsControls, AnnotationSelectorControl.ComboBoxStyleProperty, _comboboxStyleBinding);
                        PropertyControlsGrid.Children.Add(_annotationsControls);
                    }
                    _annotationsControls.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
