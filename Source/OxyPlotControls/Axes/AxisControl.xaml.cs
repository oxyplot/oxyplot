using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot;
using Wpf = OxyPlot.Wpf;
using static OxyPlotControls.OxyPlotSettingsSerializer;

namespace OxyPlotControls
{
    /// <summary>
    /// Control for editing axis properties in an OxyPlot chart.
    /// Provides UI elements for configuring axis type, range, labels, gridlines, and tick marks.
    /// </summary>
    public partial class AxisControl : UserControl
    {
        private static readonly double Epsilon = 0.0000000000000001;

        /// <summary>
        /// XML element tag used for serializing axis properties.
        /// </summary>
        public static readonly string AxisPropertiesTag = "Axis";

        /// <summary>
        /// Gets the available line style options for axis styling.
        /// </summary>
        public static List<DoubleCollection> LineStyleOptions => GenericControls.LineStyleSelectorControl.LineStyleOptions;

        /// <summary>
        /// Gets the available axis position options.
        /// </summary>
        public static List<OxyPlot.Axes.AxisPosition> AxisPositionOptions { get; } =
            new List<OxyPlot.Axes.AxisPosition>((OxyPlot.Axes.AxisPosition[])Enum.GetValues(typeof(OxyPlot.Axes.AxisPosition)));

        /// <summary>
        /// Gets the available axis tick style options.
        /// </summary>
        public static List<OxyPlot.Axes.TickStyle> AxisTickStyleOptions { get; } =
            new List<OxyPlot.Axes.TickStyle>((OxyPlot.Axes.TickStyle[])Enum.GetValues(typeof(OxyPlot.Axes.TickStyle)));

        /// <summary>
        /// Gets the available axis layer options.
        /// </summary>
        public static List<OxyPlot.Axes.AxisLayer> AxisLayerOptions { get; } =
            new List<OxyPlot.Axes.AxisLayer>((OxyPlot.Axes.AxisLayer[])Enum.GetValues(typeof(OxyPlot.Axes.AxisLayer)));

        /// <summary>
        /// Identifies the Axis dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisProperty = DependencyProperty.Register(
            nameof(Axis), typeof(Wpf.Axis), typeof(AxisControl),
            new PropertyMetadata(null, InitializePlot));

        /// <summary>
        /// Gets or sets the axis being edited by this control.
        /// </summary>
        public Wpf.Axis Axis
        {
            get => (Wpf.Axis)GetValue(AxisProperty);
            set => SetValue(AxisProperty, value);
        }

        private Wpf.LinearAxis _oldLinearAxis;
        private Wpf.LogarithmicAxis _oldLogAxis;
        private bool _ignoreMaxMinChange = false;

        /// <summary>
        /// Occurs when the axis type is changed by the user.
        /// </summary>
        public event Action<Wpf.Axis, Wpf.Axis> AxisTypeChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisControl"/> class.
        /// </summary>
        public AxisControl()
        {
            InitializeComponent();
        }

        private static void InitializePlot(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (d.GetType() != typeof(AxisControl)) return;
            var thisControl = (AxisControl)d;

            var oldAxis = e.OldValue as Wpf.Axis;
            if (oldAxis != null)
            {
                // Clean up old axis if needed
            }

            var newAxis = e.NewValue as Wpf.Axis;
            if (newAxis == null || thisControl.Content == null) return;

            var axisTypeComboBox = thisControl.AxisTypeSelector.InnerContent as ComboBox;
            if (axisTypeComboBox == null) return;

            bool isSupportedType = true;

            axisTypeComboBox.SelectionChanged -= thisControl.AxisTypeComboBox_SelectionChanged;

            // Hide axis specific properties and clear bindings
            thisControl.PowerPaddingControl.Visibility = Visibility.Collapsed;
            BindingOperations.ClearBinding(thisControl.PowerPaddingControl, GenericControls.BooleanPropertyControl.IsSelectedProperty);
            thisControl.GapWidthSelector.Visibility = Visibility.Collapsed;
            BindingOperations.ClearBinding(thisControl.GapWidthSelector, GenericControls.NumericPropertySelectorControl.SelectedNumberProperty);
            thisControl.AxisLabelsControl.Visibility = Visibility.Collapsed;
            BindingOperations.ClearBinding(thisControl.AxisLabelsControl, GenericControls.StringListPropertyControl.StringListProperty);
            thisControl.TickCenteredControl.Visibility = Visibility.Collapsed;
            BindingOperations.ClearBinding(thisControl.TickCenteredControl, GenericControls.BooleanPropertyControl.IsSelectedProperty);
            thisControl.DateAxisMinimum.Visibility = Visibility.Collapsed;
            thisControl.DateAxisMaximum.Visibility = Visibility.Collapsed;
            thisControl.AxisMinimum.Visibility = Visibility.Collapsed;
            thisControl.AxisMaximum.Visibility = Visibility.Collapsed;

            var axisType = newAxis.GetType();

            if (axisType == typeof(Wpf.LinearAxis))
            {
                axisTypeComboBox.SelectedIndex = 0;
                thisControl.AxisMinimum.Visibility = Visibility.Visible;
                thisControl.AxisMaximum.Visibility = Visibility.Visible;
                thisControl.AxisMinimum.DefaultNumber = double.NaN;
                thisControl.AxisMinimum.MinValue = double.MinValue;
                thisControl.AxisMinimum.MaxValue = double.MaxValue;
                thisControl.AxisMaximum.DefaultNumber = double.NaN;
                thisControl.AxisMaximum.MinValue = double.MinValue;
                thisControl.AxisMaximum.MaxValue = double.MaxValue;
                thisControl.LabelTypeSelector.Visibility = Visibility.Visible;
                thisControl.DecimalPlaces.Visibility = Visibility.Visible;
            }
            else if (axisType == typeof(Wpf.LogarithmicAxis))
            {
                axisTypeComboBox.SelectedIndex = 1;
                thisControl.AxisMinimum.Visibility = Visibility.Visible;
                thisControl.AxisMaximum.Visibility = Visibility.Visible;
                thisControl.AxisMinimum.DefaultNumber = double.NaN;
                thisControl.AxisMinimum.MinValue = Epsilon;
                thisControl.AxisMinimum.MaxValue = double.MaxValue;
                thisControl.AxisMaximum.DefaultNumber = double.NaN;
                thisControl.AxisMaximum.MinValue = Epsilon;
                thisControl.AxisMaximum.MaxValue = double.MaxValue;
                thisControl.LabelTypeSelector.Visibility = Visibility.Visible;
                thisControl.DecimalPlaces.Visibility = Visibility.Visible;
                thisControl.PowerPaddingControl.Visibility = Visibility.Visible;
                var powerPaddingBinding = new Binding(nameof(Wpf.LogarithmicAxis.PowerPadding))
                {
                    Source = newAxis,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay
                };
                BindingOperations.SetBinding(thisControl.PowerPaddingControl, GenericControls.BooleanPropertyControl.IsSelectedProperty, powerPaddingBinding);
            }
            else if (axisType == typeof(Wpf.NormalProbabilityAxis))
            {
                axisTypeComboBox.SelectedIndex = 2;
                thisControl.AxisMinimum.Visibility = Visibility.Visible;
                thisControl.AxisMaximum.Visibility = Visibility.Visible;
                thisControl.AxisMinimum.DefaultNumber = 0.0000001;
                thisControl.AxisMinimum.MinValue = Epsilon;
                thisControl.AxisMinimum.MaxValue = 1 - Epsilon;
                thisControl.AxisMaximum.DefaultNumber = 0.999;
                thisControl.AxisMaximum.MinValue = Epsilon;
                thisControl.AxisMaximum.MaxValue = 1 - Epsilon;
                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed;
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed;
            }
            else if (axisType == typeof(Wpf.GumbelProbabilityAxis))
            {
                axisTypeComboBox.SelectedIndex = 3;
                thisControl.AxisMinimum.Visibility = Visibility.Visible;
                thisControl.AxisMaximum.Visibility = Visibility.Visible;
                thisControl.AxisMinimum.DefaultNumber = 0.0000001;
                thisControl.AxisMinimum.MinValue = Epsilon;
                thisControl.AxisMinimum.MaxValue = 1 - Epsilon;
                thisControl.AxisMaximum.DefaultNumber = 0.99;
                thisControl.AxisMaximum.MinValue = Epsilon;
                thisControl.AxisMaximum.MaxValue = 1 - Epsilon;
                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed;
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed;
            }
            else if (axisType == typeof(Wpf.CategoryAxis))
            {
                thisControl.AxisTypeSelector.Visibility = Visibility.Collapsed;

                thisControl.GapWidthSelector.Visibility = Visibility.Visible;
                var gapWidthBinding = new Binding(nameof(Wpf.CategoryAxis.GapWidth)) { Source = newAxis };
                BindingOperations.SetBinding(thisControl.GapWidthSelector, GenericControls.NumericPropertySelectorControl.SelectedNumberProperty, gapWidthBinding);

                thisControl.AxisLabelsControl.Visibility = Visibility.Visible;
                var categoryAxis = (Wpf.CategoryAxis)newAxis;
                if (categoryAxis.ItemsSource != null && (categoryAxis.Labels == null || categoryAxis.Labels.Count == 0))
                {
                    var axisLabelsBinding = new Binding(nameof(Wpf.CategoryAxis.ItemsSource))
                    {
                        Source = newAxis,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay
                    };
                    BindingOperations.SetBinding(thisControl.AxisLabelsControl, GenericControls.StringListPropertyControl.StringListProperty, axisLabelsBinding);
                }
                else
                {
                    var axisLabelsBinding = new Binding(nameof(Wpf.CategoryAxis.Labels))
                    {
                        Source = newAxis,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay
                    };
                    BindingOperations.SetBinding(thisControl.AxisLabelsControl, GenericControls.StringListPropertyControl.StringListProperty, axisLabelsBinding);
                }

                thisControl.TickCenteredControl.Visibility = Visibility.Visible;
                var tickCenteredBinding = new Binding(nameof(Wpf.CategoryAxis.IsTickCentered)) { Source = newAxis };
                BindingOperations.SetBinding(thisControl.TickCenteredControl, GenericControls.BooleanPropertyControl.IsSelectedProperty, tickCenteredBinding);

                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed;
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed;
            }
            else if (axisType == typeof(Wpf.DateTimeAxis))
            {
                thisControl.DateAxisMinimum.Visibility = Visibility.Visible;
                thisControl.DateAxisMaximum.Visibility = Visibility.Visible;
                thisControl.AxisTypeSelector.Visibility = Visibility.Collapsed;
                thisControl.LabelTypeSelector.Visibility = Visibility.Collapsed;
                thisControl.DecimalPlaces.Visibility = Visibility.Collapsed;
            }
            else
            {
                isSupportedType = false;
            }

            BindingOperations.GetBindingExpression(thisControl.AxisMinimum, GenericControls.NumericAutoPropertyControl.NumberProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(thisControl.AxisMinimum, GenericControls.NumericAutoPropertyControl.NumberProperty)?.UpdateTarget();
            BindingOperations.GetBindingExpression(thisControl.AxisMaximum, GenericControls.NumericAutoPropertyControl.NumberProperty)?.UpdateSource();
            BindingOperations.GetBindingExpression(thisControl.AxisMaximum, GenericControls.NumericAutoPropertyControl.NumberProperty)?.UpdateTarget();

            axisTypeComboBox.SelectionChanged += thisControl.AxisTypeComboBox_SelectionChanged;

            if (isSupportedType)
            {
                axisTypeComboBox.IsEnabled = true;
                axisTypeComboBox.Visibility = Visibility.Visible;
            }
            else
            {
                axisTypeComboBox.SelectedIndex = -1;
                axisTypeComboBox.IsEnabled = false;
                axisTypeComboBox.Visibility = Visibility.Collapsed;
            }

            var labelTypeComboBox = thisControl.LabelTypeSelector.InnerContent as ComboBox;
            if (labelTypeComboBox == null) return;
            labelTypeComboBox.SelectionChanged -= thisControl.LabelType_SelectionChanged;
            if (newAxis.GetType() == typeof(Wpf.DateTimeAxis) || newAxis.GetType() == typeof(Wpf.CategoryAxis)) return;

            string stringFormatCategory = "";
            string stringFormatDecimal = "";
            if (newAxis.StringFormat != null && newAxis.StringFormat.Length > 0)
            {
                stringFormatCategory = newAxis.StringFormat.Substring(0, 1);
                if (stringFormatCategory == "C" || stringFormatCategory == "c")
                    labelTypeComboBox.SelectedIndex = 0;
                else if (stringFormatCategory == "G" || stringFormatCategory == "g")
                    labelTypeComboBox.SelectedIndex = 1;
                else if (stringFormatCategory == "N" || stringFormatCategory == "n")
                    labelTypeComboBox.SelectedIndex = 2;
                else if (stringFormatCategory == "P" || stringFormatCategory == "p")
                    labelTypeComboBox.SelectedIndex = 3;
                else if (stringFormatCategory == "E" || stringFormatCategory == "e")
                    labelTypeComboBox.SelectedIndex = 4;
                else
                {
                    labelTypeComboBox.SelectedIndex = 1;
                    stringFormatCategory = "G";
                }
            }
            else
            {
                labelTypeComboBox.SelectedIndex = 1;
                stringFormatCategory = "G";
            }

            if (newAxis.StringFormat != null && newAxis.StringFormat.Length > 1)
            {
                stringFormatDecimal = newAxis.StringFormat.Substring(1, newAxis.StringFormat.Length - 1);
                if (double.TryParse(stringFormatDecimal, out double decimals))
                {
                    thisControl.DecimalPlaces.Number = decimals;
                }
            }

            if (stringFormatDecimal == "") thisControl.DecimalPlaces.Number = double.NaN;
            thisControl._stringFormatCategory = stringFormatCategory;
            thisControl._stringFormatDecimals = stringFormatDecimal;
            labelTypeComboBox.SelectionChanged += thisControl.LabelType_SelectionChanged;

            // Force layout update to sync bindings after axis change
            thisControl.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
            {
                thisControl.UpdateLayout();
            }));
        }

        /// <summary>
        /// Identifies the TabItemStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty TabItemStyleProperty = DependencyProperty.Register(
            nameof(TabItemStyle), typeof(Style), typeof(AxisControl),
            new PropertyMetadata(new Style(typeof(TabItem))));

        /// <summary>
        /// Gets or sets the style applied to tab items in this control.
        /// </summary>
        public Style TabItemStyle
        {
            get => (Style)GetValue(TabItemStyleProperty);
            set => SetValue(TabItemStyleProperty, value);
        }

        /// <summary>
        /// Identifies the ExpanderStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle), typeof(Style), typeof(AxisControl),
            new PropertyMetadata(new Style(typeof(Expander))));

        /// <summary>
        /// Gets or sets the style applied to expanders in this control.
        /// </summary>
        public Style ExpanderStyle
        {
            get => (Style)GetValue(ExpanderStyleProperty);
            set => SetValue(ExpanderStyleProperty, value);
        }

        /// <summary>
        /// Serializes axis properties to an XElement for XML storage.
        /// </summary>
        /// <param name="axis">The axis to serialize.</param>
        /// <returns>An XElement containing the serialized axis properties.</returns>
        public static XElement AxisPropertiesToXElement(Wpf.Axis axis)
        {
            var axisProperties = new XElement(AxisPropertiesTag);
            axisProperties.SetAttributeValue("AxisType", axis.GetType().ToString());

            // General Properties
            var generalProperties = new XElement("General");
            generalProperties.SetAttributeValue(nameof(Wpf.Axis.Name), axis.Name ?? "");
            generalProperties.SetAttributeValue(nameof(axis.IsEnabled), axis.IsEnabled.ToString());
            generalProperties.SetAttributeValue(nameof(axis.IsAxisVisible), axis.IsAxisVisible.ToString());
            generalProperties.SetAttributeValue(nameof(axis.StartPosition), axis.StartPosition.ToString("G17", CultureInfo.InvariantCulture));
            generalProperties.SetAttributeValue(nameof(axis.EndPosition), axis.EndPosition.ToString("G17", CultureInfo.InvariantCulture));
            generalProperties.SetAttributeValue(nameof(axis.IsPanEnabled), axis.IsPanEnabled.ToString());
            generalProperties.SetAttributeValue(nameof(axis.IsZoomEnabled), axis.IsZoomEnabled.ToString());
            axisProperties.Add(generalProperties);

            // Numeric Properties
            var numericProperties = new XElement("Numbers");
            numericProperties.SetAttributeValue(nameof(axis.Maximum), axis.Maximum.ToString("G17", CultureInfo.InvariantCulture));
            numericProperties.SetAttributeValue(nameof(axis.Minimum), axis.Minimum.ToString("G17", CultureInfo.InvariantCulture));
            numericProperties.SetAttributeValue(nameof(axis.AbsoluteMaximum), axis.AbsoluteMaximum.ToString("G17", CultureInfo.InvariantCulture));
            numericProperties.SetAttributeValue(nameof(axis.AbsoluteMinimum), axis.AbsoluteMinimum.ToString("G17", CultureInfo.InvariantCulture));
            numericProperties.SetAttributeValue(nameof(axis.FilterMaxValue), axis.FilterMaxValue.ToString("G17", CultureInfo.InvariantCulture));
            numericProperties.SetAttributeValue(nameof(axis.FilterMinValue), axis.FilterMinValue.ToString("G17", CultureInfo.InvariantCulture));
            axisProperties.Add(numericProperties);

            // Style Properties
            var styleProperties = new XElement("Style");
            styleProperties.SetAttributeValue(nameof(axis.AxislineColor), axis.AxislineColor.ToString());
            styleProperties.SetAttributeValue(nameof(axis.AxislineStyle), axis.AxislineStyle.ToString());
            styleProperties.SetAttributeValue(nameof(axis.AxislineThickness), axis.AxislineThickness.ToString("G17", CultureInfo.InvariantCulture));
            axisProperties.Add(styleProperties);

            // Position Properties
            var positionProperties = new XElement("Position");
            positionProperties.SetAttributeValue(nameof(axis.AxisDistance), axis.AxisDistance.ToString("G17", CultureInfo.InvariantCulture));
            positionProperties.SetAttributeValue(nameof(axis.PositionAtZeroCrossing), axis.PositionAtZeroCrossing.ToString());
            positionProperties.SetAttributeValue(nameof(axis.Position), axis.Position.ToString());
            positionProperties.SetAttributeValue(nameof(axis.Key), axis.Key);
            positionProperties.SetAttributeValue(nameof(axis.PositionTier), axis.PositionTier);
            axisProperties.Add(positionProperties);

            // Title Properties
            var weightConverter = new FontWeightConverter();
            var titleProperties = new XElement("Title");
            titleProperties.SetAttributeValue(nameof(axis.Title), axis.Title);
            titleProperties.SetAttributeValue(nameof(axis.TitleColor), axis.TitleColor.ToString());
            titleProperties.SetAttributeValue(nameof(axis.TitleFont), axis.TitleFont);
            titleProperties.SetAttributeValue(nameof(axis.TitleFontSize), axis.TitleFontSize.ToString("G17", CultureInfo.InvariantCulture));
            titleProperties.SetAttributeValue(nameof(axis.TitleFontWeight), weightConverter.ConvertToInvariantString(axis.TitleFontWeight));
            titleProperties.SetAttributeValue(nameof(axis.AxisTitleDistance), axis.AxisTitleDistance.ToString("G17", CultureInfo.InvariantCulture));
            titleProperties.SetAttributeValue(nameof(axis.Unit), axis.Unit);
            axisProperties.Add(titleProperties);

            // Label Properties
            var labelProperties = new XElement("Labels");
            labelProperties.SetAttributeValue(nameof(axis.TextColor), axis.TextColor.ToString());
            labelProperties.SetAttributeValue(nameof(axis.Font), axis.Font);
            labelProperties.SetAttributeValue(nameof(axis.FontSize), axis.FontSize.ToString("G17", CultureInfo.InvariantCulture));
            labelProperties.SetAttributeValue(nameof(axis.FontWeight), weightConverter.ConvertToInvariantString(axis.FontWeight));
            labelProperties.SetAttributeValue(nameof(axis.Angle), axis.Angle.ToString("G17", CultureInfo.InvariantCulture));
            labelProperties.SetAttributeValue(nameof(axis.AxisTickToLabelDistance), axis.AxisTickToLabelDistance.ToString("G17", CultureInfo.InvariantCulture));
            labelProperties.SetAttributeValue(nameof(axis.StringFormat), axis.StringFormat);
            labelProperties.SetAttributeValue(nameof(axis.UseSuperExponentialFormat), axis.UseSuperExponentialFormat.ToString());
            axisProperties.Add(labelProperties);

            // Major Gridline Properties
            var majorGridlineProperties = new XElement("MajorGridlines");
            majorGridlineProperties.SetAttributeValue(nameof(axis.MajorGridlineColor), axis.MajorGridlineColor.ToString());
            majorGridlineProperties.SetAttributeValue(nameof(axis.MajorGridlineStyle), axis.MajorGridlineStyle.ToString());
            majorGridlineProperties.SetAttributeValue(nameof(axis.MajorGridlineThickness), axis.MajorGridlineThickness.ToString("G17", CultureInfo.InvariantCulture));
            majorGridlineProperties.SetAttributeValue(nameof(axis.MajorStep), axis.MajorStep.ToString("G17", CultureInfo.InvariantCulture));
            majorGridlineProperties.SetAttributeValue(nameof(axis.MajorTickSize), axis.MajorTickSize.ToString("G17", CultureInfo.InvariantCulture));
            axisProperties.Add(majorGridlineProperties);

            // Minor Gridline Properties
            var minorGridlineProperties = new XElement("MinorGridlines");
            minorGridlineProperties.SetAttributeValue(nameof(axis.MinorGridlineColor), axis.MinorGridlineColor.ToString());
            minorGridlineProperties.SetAttributeValue(nameof(axis.MinorGridlineStyle), axis.MinorGridlineStyle.ToString());
            minorGridlineProperties.SetAttributeValue(nameof(axis.MinorGridlineThickness), axis.MinorGridlineThickness.ToString("G17", CultureInfo.InvariantCulture));
            minorGridlineProperties.SetAttributeValue(nameof(axis.MinorStep), axis.MinorStep.ToString("G17", CultureInfo.InvariantCulture));
            minorGridlineProperties.SetAttributeValue(nameof(axis.MinorTickSize), axis.MinorTickSize.ToString("G17", CultureInfo.InvariantCulture));
            axisProperties.Add(minorGridlineProperties);

            // Tick Style Properties
            var tickStyleProperties = new XElement("Tick");
            tickStyleProperties.SetAttributeValue(nameof(axis.TickStyle), axis.TickStyle.ToString());
            tickStyleProperties.SetAttributeValue(nameof(axis.TicklineColor), axis.TicklineColor.ToString());
            axisProperties.Add(tickStyleProperties);

            // Concrete axis implementation properties
            var axisType = axis.GetType();
            if (axisType == typeof(Wpf.LinearAxis))
            {
                var linearAxis = (Wpf.LinearAxis)axis;
                var linearAxisProperties = new XElement("LinearAxis");
                linearAxisProperties.SetAttributeValue(nameof(linearAxis.FormatAsFractions), linearAxis.FormatAsFractions.ToString());
                axisProperties.Add(linearAxisProperties);
            }
            else if (axisType == typeof(Wpf.CategoryAxis))
            {
                var categoryAxis = (Wpf.CategoryAxis)axis;
                var categoryAxisProperties = new XElement("CategoryAxis");
                categoryAxisProperties.SetAttributeValue(nameof(categoryAxis.IsTickCentered), categoryAxis.IsTickCentered.ToString());
                categoryAxisProperties.SetAttributeValue(nameof(categoryAxis.GapWidth), categoryAxis.GapWidth.ToString("G17", CultureInfo.InvariantCulture));
                axisProperties.Add(categoryAxisProperties);
            }
            else if (axisType == typeof(Wpf.LogarithmicAxis))
            {
                var logAxis = axis as Wpf.LogarithmicAxis;
                var logAxisProperties = new XElement("LogarithmicAxis");
                logAxisProperties.SetAttributeValue(nameof(logAxis.Base), logAxis.Base.ToString("G17", CultureInfo.InvariantCulture));
                logAxisProperties.SetAttributeValue(nameof(logAxis.PowerPadding), logAxis.PowerPadding.ToString());
                axisProperties.Add(logAxisProperties);
            }
            else if (axisType == typeof(Wpf.DateTimeAxis))
            {
                var dateAxis = (Wpf.DateTimeAxis)axis;
                var dateAxisProperties = new XElement("DateTimeAxis");
                dateAxisProperties.SetAttributeValue(nameof(dateAxis.CalendarWeekRule), dateAxis.CalendarWeekRule.ToString());
                axisProperties.Add(dateAxisProperties);
            }

            return axisProperties;
        }

        /// <summary>
        /// Deserializes axis properties from an XElement.
        /// </summary>
        /// <param name="element">The XElement containing the axis properties.</param>
        /// <param name="targetAxis">Optional target axis to apply properties to.</param>
        /// <returns>A new or updated axis with the deserialized properties.</returns>
        public static Wpf.Axis XElementToAxisProperties(XElement element, Wpf.Axis targetAxis = null)
        {
            if (element.Name != AxisPropertiesTag) return null;

            var fontWeightConverter = new FontWeightConverter();
            Wpf.Axis axis;
            string axisType = "";
            if (element.Attribute("AxisType") != null) axisType = element.Attribute("AxisType").Value;

            if (targetAxis != null)
            {
                axis = targetAxis;
            }
            else
            {
                if (axisType == typeof(Wpf.LinearAxis).ToString())
                    axis = new Wpf.LinearAxis();
                else if (axisType == typeof(Wpf.CategoryAxis).ToString())
                    axis = new Wpf.CategoryAxis();
                else if (axisType == typeof(Wpf.LogarithmicAxis).ToString())
                    axis = new Wpf.LogarithmicAxis();
                else if (axisType == typeof(Wpf.DateTimeAxis).ToString())
                    axis = new Wpf.DateTimeAxis();
                else if (axisType == typeof(Wpf.AngleAxis).ToString())
                    axis = new Wpf.AngleAxis();
                else if (axisType == typeof(Wpf.LinearColorAxis).ToString())
                    axis = new Wpf.LinearColorAxis();
                else if (axisType == typeof(Wpf.MagnitudeAxis).ToString())
                    axis = new Wpf.MagnitudeAxis();
                else if (axisType == typeof(Wpf.TimeSpanAxis).ToString())
                    axis = new Wpf.TimeSpanAxis();
                else if (axisType == typeof(Wpf.NormalProbabilityAxis).ToString())
                    axis = new Wpf.NormalProbabilityAxis();
                else if (axisType == typeof(Wpf.GumbelProbabilityAxis).ToString())
                    axis = new Wpf.GumbelProbabilityAxis();
                else
                    axis = new Wpf.LinearAxis();
            }

            // General Properties
            var generalElement = element.Element("General");
            if (generalElement != null)
            {
                if (GetStringAttribute(generalElement, nameof(axis.Name), out var name)) axis.Name = name;
                if (GetBooleanAttribute(generalElement, nameof(axis.IsEnabled), out var isEnabled)) axis.IsEnabled = isEnabled;
                if (GetBooleanAttribute(generalElement, nameof(axis.IsAxisVisible), out var isAxisVisible)) axis.IsAxisVisible = isAxisVisible;
                if (GetDoubleAttribute(generalElement, nameof(axis.StartPosition), out var startPosition)) axis.StartPosition = startPosition;
                if (GetDoubleAttribute(generalElement, nameof(axis.EndPosition), out var endPosition)) axis.EndPosition = endPosition;
                if (GetBooleanAttribute(generalElement, nameof(axis.IsPanEnabled), out var isPanEnabled)) axis.IsPanEnabled = isPanEnabled;
                if (GetBooleanAttribute(generalElement, nameof(axis.IsZoomEnabled), out var isZoomEnabled)) axis.IsZoomEnabled = isZoomEnabled;

                // Backward compatibility
                if (GetBooleanAttribute(generalElement, "AxisVisible", out isAxisVisible)) axis.IsAxisVisible = isAxisVisible;
                if (GetBooleanAttribute(generalElement, "CanPan", out isPanEnabled)) axis.IsPanEnabled = isPanEnabled;
                if (GetBooleanAttribute(generalElement, "CanZoom", out isZoomEnabled)) axis.IsZoomEnabled = isZoomEnabled;
            }

            // Number Properties
            var numbersElement = element.Element("Numbers");
            if (numbersElement != null)
            {
                if (GetDoubleAttribute(numbersElement, nameof(axis.Maximum), out var maximum)) axis.Maximum = maximum;
                if (GetDoubleAttribute(numbersElement, nameof(axis.Minimum), out var minimum)) axis.Minimum = minimum;
                if (GetDoubleAttribute(numbersElement, nameof(axis.AbsoluteMaximum), out var absoluteMaximum)) axis.AbsoluteMaximum = absoluteMaximum;
                if (GetDoubleAttribute(numbersElement, nameof(axis.AbsoluteMinimum), out var absoluteMinimum)) axis.AbsoluteMinimum = absoluteMinimum;
                if (GetDoubleAttribute(numbersElement, nameof(axis.FilterMaxValue), out var filterMaxValue)) axis.FilterMaxValue = filterMaxValue;
                if (GetDoubleAttribute(numbersElement, nameof(axis.FilterMinValue), out var filterMinValue)) axis.FilterMinValue = filterMinValue;
            }

            // Style Properties
            var styleElement = element.Element("Style");
            if (styleElement != null)
            {
                if (GetColorAttribute(styleElement, nameof(axis.AxislineColor), out var axislineColor)) axis.AxislineColor = axislineColor;
                if (GetEnumAttribute(styleElement, nameof(axis.AxislineStyle), out LineStyle axislineStyle)) axis.AxislineStyle = axislineStyle;
                if (GetDoubleAttribute(styleElement, nameof(axis.AxislineThickness), out var axislineThickness)) axis.AxislineThickness = axislineThickness;

                // Backward compatibility
                if (GetColorAttribute(styleElement, "Color", out axislineColor)) axis.AxislineColor = axislineColor;
                if (GetEnumAttribute(styleElement, "Style", out axislineStyle)) axis.AxislineStyle = axislineStyle;
                if (GetDoubleAttribute(styleElement, "Thickness", out axislineThickness)) axis.AxislineThickness = axislineThickness;
            }

            // Position Properties
            var positionElement = element.Element("Position");
            if (positionElement != null)
            {
                if (GetDoubleAttribute(positionElement, nameof(axis.AxisDistance), out var axisDistance)) axis.AxisDistance = axisDistance;
                if (GetBooleanAttribute(positionElement, nameof(axis.PositionAtZeroCrossing), out var positionAtZeroCrossing)) axis.PositionAtZeroCrossing = positionAtZeroCrossing;
                if (GetEnumAttribute(positionElement, nameof(axis.Position), out OxyPlot.Axes.AxisPosition position)) axis.Position = position;
                if (GetStringAttribute(positionElement, nameof(axis.Key), out var key)) axis.Key = key;
                if (GetIntegerAttribute(positionElement, nameof(axis.PositionTier), out var positionTier)) axis.PositionTier = positionTier;

                // Backward compatibility
                if (GetDoubleAttribute(positionElement, "Distance", out axisDistance)) axis.AxisDistance = axisDistance;
                if (GetBooleanAttribute(positionElement, "ZeroCrossing", out positionAtZeroCrossing)) axis.PositionAtZeroCrossing = positionAtZeroCrossing;
                if (GetIntegerAttribute(positionElement, "Tier", out positionTier)) axis.PositionTier = positionTier;
            }

            // Title Properties
            var titleElement = element.Element("Title");
            if (titleElement != null)
            {
                if (GetStringAttribute(titleElement, nameof(axis.Title), out var title)) axis.Title = title;
                if (GetColorAttribute(titleElement, nameof(axis.TitleColor), out var titleColor)) axis.TitleColor = titleColor;
                if (GetStringAttribute(titleElement, nameof(axis.TitleFont), out var titleFont)) axis.TitleFont = titleFont;
                if (GetDoubleAttribute(titleElement, nameof(axis.TitleFontSize), out var titleFontSize)) axis.TitleFontSize = titleFontSize;
                if (GetFontWeightAttribute(titleElement, nameof(axis.TitleFontWeight), fontWeightConverter, out var titleFontWeight)) axis.TitleFontWeight = titleFontWeight;
                if (GetDoubleAttribute(titleElement, nameof(axis.AxisTitleDistance), out var axisTitleDistance)) axis.AxisTitleDistance = axisTitleDistance;
                if (GetStringAttribute(titleElement, nameof(axis.Unit), out var unit)) axis.Unit = unit;

                // Backward compatibility
                if (GetColorAttribute(titleElement, "Color", out titleColor)) axis.TitleColor = titleColor;
                if (GetStringAttribute(titleElement, "Font", out titleFont)) axis.TitleFont = titleFont;
                if (GetDoubleAttribute(titleElement, "Size", out titleFontSize)) axis.TitleFontSize = titleFontSize;
                if (GetFontWeightAttribute(titleElement, "Weight", fontWeightConverter, out titleFontWeight)) axis.TitleFontWeight = titleFontWeight;
                if (GetDoubleAttribute(titleElement, "Distance", out axisTitleDistance)) axis.AxisTitleDistance = axisTitleDistance;
            }

            // Label Properties
            var labelElement = element.Element("Labels");
            if (labelElement != null)
            {
                if (GetColorAttribute(labelElement, nameof(axis.TextColor), out var textColor)) axis.TextColor = textColor;
                if (GetStringAttribute(labelElement, nameof(axis.Font), out var font)) axis.Font = font;
                if (GetDoubleAttribute(labelElement, nameof(axis.FontSize), out var fontSize)) axis.FontSize = fontSize;
                if (GetFontWeightAttribute(labelElement, nameof(axis.FontWeight), fontWeightConverter, out var fontWeight)) axis.FontWeight = fontWeight;
                if (GetDoubleAttribute(labelElement, nameof(axis.Angle), out var angle)) axis.Angle = angle;
                if (GetDoubleAttribute(labelElement, nameof(axis.AxisTickToLabelDistance), out var axisTickToLabelDistance)) axis.AxisTickToLabelDistance = axisTickToLabelDistance;
                if (GetStringAttribute(labelElement, nameof(axis.StringFormat), out var stringFormat)) axis.StringFormat = stringFormat;
                if (GetBooleanAttribute(labelElement, nameof(axis.UseSuperExponentialFormat), out var useSuperExponentialFormat)) axis.UseSuperExponentialFormat = useSuperExponentialFormat;

                // Backward compatibility
                if (GetColorAttribute(labelElement, "Color", out textColor)) axis.TextColor = textColor;
                if (GetDoubleAttribute(labelElement, "Size", out fontSize)) axis.FontSize = fontSize;
                if (GetFontWeightAttribute(labelElement, "Weight", fontWeightConverter, out fontWeight)) axis.FontWeight = fontWeight;
                if (GetDoubleAttribute(labelElement, "TickDistance", out axisTickToLabelDistance)) axis.AxisTickToLabelDistance = axisTickToLabelDistance;
                if (GetBooleanAttribute(labelElement, "Superscript", out useSuperExponentialFormat)) axis.UseSuperExponentialFormat = useSuperExponentialFormat;
            }

            // Major Gridline Properties
            var majorGridlineElement = element.Element("MajorGridlines");
            if (majorGridlineElement != null)
            {
                if (GetColorAttribute(majorGridlineElement, nameof(axis.MajorGridlineColor), out var majorGridlineColor)) axis.MajorGridlineColor = majorGridlineColor;
                if (GetEnumAttribute(majorGridlineElement, nameof(axis.MajorGridlineStyle), out LineStyle majorGridlineStyle)) axis.MajorGridlineStyle = majorGridlineStyle;
                if (GetDoubleAttribute(majorGridlineElement, nameof(axis.MajorGridlineThickness), out var majorGridlineThickness)) axis.MajorGridlineThickness = majorGridlineThickness;
                if (GetDoubleAttribute(majorGridlineElement, nameof(axis.MajorStep), out var majorStep)) axis.MajorStep = majorStep;
                if (GetDoubleAttribute(majorGridlineElement, nameof(axis.MajorTickSize), out var majorTickSize)) axis.MajorTickSize = majorTickSize;

                // Backward compatibility
                if (GetColorAttribute(majorGridlineElement, "Color", out majorGridlineColor)) axis.MajorGridlineColor = majorGridlineColor;
                if (GetEnumAttribute(majorGridlineElement, "Style", out majorGridlineStyle)) axis.MajorGridlineStyle = majorGridlineStyle;
                if (GetDoubleAttribute(majorGridlineElement, "Thickness", out majorGridlineThickness)) axis.MajorGridlineThickness = majorGridlineThickness;
                if (GetDoubleAttribute(majorGridlineElement, "Step", out majorStep)) axis.MajorStep = majorStep;
                if (GetDoubleAttribute(majorGridlineElement, "TickSize", out majorTickSize)) axis.MajorTickSize = majorTickSize;
            }

            // Minor Gridline Properties
            var minorGridlineElement = element.Element("MinorGridlines");
            if (minorGridlineElement != null)
            {
                if (GetColorAttribute(minorGridlineElement, nameof(axis.MinorGridlineColor), out var minorGridlineColor)) axis.MinorGridlineColor = minorGridlineColor;
                if (GetEnumAttribute(minorGridlineElement, nameof(axis.MinorGridlineStyle), out LineStyle minorGridlineStyle)) axis.MinorGridlineStyle = minorGridlineStyle;
                if (GetDoubleAttribute(minorGridlineElement, nameof(axis.MinorGridlineThickness), out var minorGridlineThickness)) axis.MinorGridlineThickness = minorGridlineThickness;
                if (GetDoubleAttribute(minorGridlineElement, nameof(axis.MinorStep), out var minorStep)) axis.MinorStep = minorStep;
                if (GetDoubleAttribute(minorGridlineElement, nameof(axis.MinorTickSize), out var minorTickSize)) axis.MinorTickSize = minorTickSize;

                // Backward compatibility
                if (GetColorAttribute(minorGridlineElement, "Color", out minorGridlineColor)) axis.MinorGridlineColor = minorGridlineColor;
                if (GetEnumAttribute(minorGridlineElement, "Style", out minorGridlineStyle)) axis.MinorGridlineStyle = minorGridlineStyle;
                if (GetDoubleAttribute(minorGridlineElement, "Thickness", out minorGridlineThickness)) axis.MinorGridlineThickness = minorGridlineThickness;
                if (GetDoubleAttribute(minorGridlineElement, "Step", out minorStep)) axis.MinorStep = minorStep;
                if (GetDoubleAttribute(minorGridlineElement, "TickSize", out minorTickSize)) axis.MinorTickSize = minorTickSize;
            }

            // Tick Style Properties
            var tickStyleElement = element.Element("Tick");
            if (tickStyleElement != null)
            {
                if (GetColorAttribute(tickStyleElement, nameof(axis.TicklineColor), out var ticklineColor)) axis.TicklineColor = ticklineColor;
                if (GetEnumAttribute(tickStyleElement, nameof(axis.TickStyle), out OxyPlot.Axes.TickStyle tickStyle)) axis.TickStyle = tickStyle;

                // Backward compatibility
                if (GetColorAttribute(tickStyleElement, "Color", out ticklineColor)) axis.TicklineColor = ticklineColor;
                if (GetEnumAttribute(tickStyleElement, "Style", out tickStyle)) axis.TickStyle = tickStyle;
            }

            // Concrete axis implementation properties
            var currentAxisType = axis.GetType();
            if (currentAxisType == typeof(Wpf.LinearAxis))
            {
                var linearAxisElement = element.Element("LinearAxis");
                if (linearAxisElement != null)
                {
                    if (GetBooleanAttribute(linearAxisElement, nameof(Wpf.LinearAxis.FormatAsFractions), out var formatAsFractions))
                        ((Wpf.LinearAxis)axis).FormatAsFractions = formatAsFractions;
                    // Backward compatibility
                    if (GetBooleanAttribute(linearAxisElement, "FractionFormat", out formatAsFractions))
                        ((Wpf.LinearAxis)axis).FormatAsFractions = formatAsFractions;
                }
            }
            else if (currentAxisType == typeof(Wpf.CategoryAxis))
            {
                var categoryAxisElement = element.Element("CategoryAxis");
                if (categoryAxisElement != null)
                {
                    if (GetBooleanAttribute(categoryAxisElement, nameof(Wpf.CategoryAxis.IsTickCentered), out var isTickCentered))
                        ((Wpf.CategoryAxis)axis).IsTickCentered = isTickCentered;
                    if (GetDoubleAttribute(categoryAxisElement, nameof(Wpf.CategoryAxis.GapWidth), out var gapWidth))
                        ((Wpf.CategoryAxis)axis).GapWidth = gapWidth;

                    // Backward compatibility
                    if (GetBooleanAttribute(categoryAxisElement, "Centered", out isTickCentered))
                        ((Wpf.CategoryAxis)axis).IsTickCentered = isTickCentered;
                }
            }
            else if (currentAxisType == typeof(Wpf.LogarithmicAxis))
            {
                var logAxisElement = element.Element("LogarithmicAxis");
                if (logAxisElement != null)
                {
                    if (GetDoubleAttribute(logAxisElement, nameof(Wpf.LogarithmicAxis.Base), out var logBase))
                        ((Wpf.LogarithmicAxis)axis).Base = logBase;
                    if (GetBooleanAttribute(logAxisElement, nameof(Wpf.LogarithmicAxis.PowerPadding), out var powerPadding))
                        ((Wpf.LogarithmicAxis)axis).PowerPadding = powerPadding;

                    // Backward compatibility
                    if (GetDoubleAttribute(logAxisElement, "LogBase", out logBase))
                        ((Wpf.LogarithmicAxis)axis).Base = logBase;
                }
            }
            else if (currentAxisType == typeof(Wpf.DateTimeAxis))
            {
                var dateAxisElement = element.Element("DateTimeAxis");
                if (dateAxisElement != null)
                {
                    if (GetEnumAttribute(dateAxisElement, nameof(Wpf.DateTimeAxis.CalendarWeekRule), out CalendarWeekRule calendarWeekRule))
                        ((Wpf.DateTimeAxis)axis).CalendarWeekRule = calendarWeekRule;

                    // Backward compatibility
                    if (GetEnumAttribute(dateAxisElement, "CalendarWeek", out calendarWeekRule))
                        ((Wpf.DateTimeAxis)axis).CalendarWeekRule = calendarWeekRule;
                }
            }

            return axis;
        }

        /// <summary>
        /// Converts an axis to a logarithmic axis while preserving compatible properties.
        /// </summary>
        /// <param name="wpfAxis">The source axis to convert.</param>
        /// <param name="logBase">The logarithm base to use.</param>
        /// <param name="powerPadding">Whether to use power padding.</param>
        /// <returns>A new logarithmic axis with properties copied from the source.</returns>
        public static Wpf.LogarithmicAxis ConvertAxisToLogarithmicAxis(Wpf.Axis wpfAxis, double logBase = 10, bool powerPadding = true)
        {
            var newAxis = new Wpf.LogarithmicAxis { Base = logBase, PowerPadding = powerPadding };
            newAxis.FromAxisProperties(wpfAxis);
            if (newAxis.Minimum <= 0) newAxis.Minimum = Epsilon;
            newAxis.Maximum = wpfAxis.Maximum;
            newAxis.StartPosition = wpfAxis.StartPosition;
            newAxis.EndPosition = wpfAxis.EndPosition;
            return newAxis;
        }

        /// <summary>
        /// Converts an axis to a linear axis while preserving compatible properties.
        /// </summary>
        /// <param name="wpfAxis">The source axis to convert.</param>
        /// <param name="formatAsFractions">Whether to format values as fractions.</param>
        /// <param name="fractionUnits">The fraction unit value.</param>
        /// <param name="fractionSymbol">The symbol to use for fractions.</param>
        /// <returns>A new linear axis with properties copied from the source.</returns>
        public static Wpf.LinearAxis ConvertAxisToLinearAxis(Wpf.Axis wpfAxis, bool formatAsFractions = false, double fractionUnits = 1, string fractionSymbol = null)
        {
            var newAxis = new Wpf.LinearAxis
            {
                FormatAsFractions = formatAsFractions,
                FractionUnit = fractionUnits,
                FractionUnitSymbol = fractionSymbol
            };
            newAxis.FromAxisProperties(wpfAxis);
            newAxis.Minimum = wpfAxis.Minimum;
            newAxis.Maximum = wpfAxis.Maximum;
            newAxis.StartPosition = wpfAxis.StartPosition;
            newAxis.EndPosition = wpfAxis.EndPosition;
            return newAxis;
        }

        /// <summary>
        /// Converts an axis to a normal probability axis while preserving compatible properties.
        /// </summary>
        /// <param name="wpfAxis">The source axis to convert.</param>
        /// <returns>A new normal probability axis with properties copied from the source.</returns>
        public static Wpf.NormalProbabilityAxis ConvertAxisToNormalAxis(Wpf.Axis wpfAxis)
        {
            var newAxis = new Wpf.NormalProbabilityAxis();
            newAxis.FromAxisProperties(wpfAxis);
            if (newAxis.Minimum < 0.0000000000000001) newAxis.Minimum = 0.0000001;
            if (newAxis.Maximum > 0.999 || double.IsNaN(newAxis.Maximum)) newAxis.Maximum = 0.999;
            newAxis.StartPosition = wpfAxis.StartPosition;
            newAxis.EndPosition = wpfAxis.EndPosition;
            return newAxis;
        }

        /// <summary>
        /// Converts an axis to a Gumbel probability axis while preserving compatible properties.
        /// </summary>
        /// <param name="wpfAxis">The source axis to convert.</param>
        /// <returns>A new Gumbel probability axis with properties copied from the source.</returns>
        public static Wpf.GumbelProbabilityAxis ConvertAxisToGumbelAxis(Wpf.Axis wpfAxis)
        {
            var newAxis = new Wpf.GumbelProbabilityAxis();
            newAxis.FromAxisProperties(wpfAxis);
            if (newAxis.Minimum < 0.0000000000000001) newAxis.Minimum = 0.0000001;
            if (newAxis.Maximum > 0.99 || double.IsNaN(newAxis.Maximum)) newAxis.Maximum = 0.99;
            newAxis.StartPosition = wpfAxis.StartPosition;
            newAxis.EndPosition = wpfAxis.EndPosition;
            return newAxis;
        }

        /// <summary>
        /// Converts an axis to a date time axis while preserving compatible properties.
        /// </summary>
        /// <param name="wpfAxis">The source axis to convert.</param>
        /// <returns>A new date time axis with properties copied from the source.</returns>
        public static Wpf.DateTimeAxis ConvertAxisToDateTimeAxis(Wpf.Axis wpfAxis)
        {
            var newAxis = new Wpf.DateTimeAxis();
            newAxis.FromAxisProperties(wpfAxis);
            newAxis.Minimum = wpfAxis.Minimum;
            newAxis.Maximum = wpfAxis.Maximum;
            newAxis.StartPosition = wpfAxis.StartPosition;
            newAxis.EndPosition = wpfAxis.EndPosition;
            return newAxis;
        }

        private void AxisTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            var axisTypeComboBox = sender as ComboBox;
            if (axisTypeComboBox == null) return;
            if (axisTypeComboBox.SelectedIndex == -1) return;
            if (Axis == null) return;
            if (Axis.Parent == null) return;
            if (Axis.Parent.GetType() != typeof(Wpf.Plot)) return;

            var axisType = Axis.GetType();
            if (axisType == typeof(Wpf.LinearAxis))
                _oldLinearAxis = (Wpf.LinearAxis)Axis;
            else if (axisType == typeof(Wpf.LogarithmicAxis))
                _oldLogAxis = (Wpf.LogarithmicAxis)Axis;

            var thePlot = (Wpf.Plot)Axis.Parent;
            Wpf.Axis newAxis;

            switch (axisTypeComboBox.SelectedIndex)
            {
                case 0: // Linear
                    AxisMinimum.DefaultNumber = double.NaN;
                    AxisMinimum.MinValue = double.MinValue;
                    AxisMinimum.MaxValue = double.MaxValue;
                    AxisMaximum.DefaultNumber = double.NaN;
                    AxisMaximum.MinValue = double.MinValue;
                    AxisMaximum.MaxValue = double.MaxValue;

                    if (Axis.GetType() == typeof(Wpf.LinearAxis)) return;
                    newAxis = ConvertAxisToLinearAxis(Axis);

                    LabelTypeSelector.Visibility = Visibility.Visible;
                    DecimalPlaces.Visibility = Visibility.Visible;
                    break;

                case 1: // Logarithmic
                    AxisMinimum.DefaultNumber = double.NaN;
                    AxisMinimum.MinValue = Epsilon;
                    AxisMinimum.MaxValue = double.MaxValue;
                    AxisMaximum.DefaultNumber = double.NaN;
                    AxisMaximum.MinValue = Epsilon;
                    AxisMaximum.MaxValue = double.MaxValue;

                    if (Axis.GetType() == typeof(Wpf.LogarithmicAxis)) return;
                    newAxis = ConvertAxisToLogarithmicAxis(Axis);

                    LabelTypeSelector.Visibility = Visibility.Visible;
                    DecimalPlaces.Visibility = Visibility.Visible;
                    break;

                case 2: // Normal Probability
                    if (Axis.InternalAxis.DataMinimum < 0 || Axis.InternalAxis.DataMaximum > 1)
                    {
                        MessageBox.Show("Axis cannot be converted to a Normal probability axis because the data is not between 0 and 1.",
                            "Normal Probability Axis", MessageBoxButton.OK, MessageBoxImage.Error);
                        axisTypeComboBox.SelectedItem = e.RemovedItems[0];
                        e.Handled = true;
                        return;
                    }

                    AxisMinimum.DefaultNumber = 0.0000001;
                    AxisMinimum.MinValue = Epsilon;
                    AxisMinimum.MaxValue = 1 - Epsilon;
                    AxisMaximum.DefaultNumber = 0.999;
                    AxisMaximum.MinValue = Epsilon;
                    AxisMaximum.MaxValue = 1 - Epsilon;

                    if (Axis.GetType() == typeof(Wpf.NormalProbabilityAxis)) return;
                    newAxis = ConvertAxisToNormalAxis(Axis);

                    LabelTypeSelector.Visibility = Visibility.Collapsed;
                    DecimalPlaces.Visibility = Visibility.Collapsed;
                    break;

                case 3: // Gumbel Probability
                    if (Axis.InternalAxis.DataMinimum < 0 || Axis.InternalAxis.DataMaximum > 1)
                    {
                        MessageBox.Show("Axis cannot be converted to a Gumbel probability axis because the data is not between 0 and 1.",
                            "Gumbel Probability Axis", MessageBoxButton.OK, MessageBoxImage.Error);
                        axisTypeComboBox.SelectedItem = e.RemovedItems[0];
                        e.Handled = true;
                        return;
                    }

                    AxisMinimum.DefaultNumber = 0.0000001;
                    AxisMinimum.MinValue = Epsilon;
                    AxisMinimum.MaxValue = 1 - Epsilon;
                    AxisMaximum.DefaultNumber = 0.99;
                    AxisMaximum.MinValue = Epsilon;
                    AxisMaximum.MaxValue = 1 - Epsilon;

                    if (Axis.GetType() == typeof(Wpf.GumbelProbabilityAxis)) return;
                    newAxis = ConvertAxisToGumbelAxis(Axis);

                    LabelTypeSelector.Visibility = Visibility.Collapsed;
                    DecimalPlaces.Visibility = Visibility.Collapsed;
                    break;

                case 4: // Date Time
                    if (Axis.GetType() == typeof(Wpf.DateTimeAxis)) return;
                    newAxis = ConvertAxisToDateTimeAxis(Axis);
                    break;

                default:
                    return;
            }

            thePlot.Axes.Remove(Axis);
            thePlot.Axes.Add(newAxis);
            Axis = newAxis;

            AxisTypeChanged?.Invoke(Axis, newAxis);

            thePlot.InvalidatePlot();
        }

        /// <summary>
        /// Closes all expander controls in the axis control.
        /// </summary>
        public void CloseExpanders()
        {
            GeneralEXP.IsExpanded = false;
            LabelsEXP.IsExpanded = false;
            TitleEXP.IsExpanded = false;
            MajorGridLinesEXP.IsExpanded = false;
            MinorGridLinesEXP.IsExpanded = false;
            TickOptionsEXP.IsExpanded = false;
        }

        private void AxisMinimum_PreviewNumberChanged(object oldValue, object newValue, ref bool cancel)
        {
            if (_ignoreMaxMinChange) return;

            double newNumber;
            if (!double.TryParse(newValue.ToString(), out newNumber))
            {
                if (newValue.GetType() != typeof(double)) return;
                newNumber = (double)newValue;
            }

            if (newNumber >= AxisMaximum.Number)
            {
                double oldNumber = (double)oldValue;
                _ignoreMaxMinChange = true;
                AxisMinimum.Number = oldNumber;
                _ignoreMaxMinChange = false;
                cancel = true;
            }
        }

        private void AxisMaximum_PreviewNumberChanged(object oldValue, object newValue, ref bool cancel)
        {
            if (_ignoreMaxMinChange) return;

            double newNumber;
            if (!double.TryParse(newValue.ToString(), out newNumber))
            {
                if (newValue.GetType() != typeof(double)) return;
                newNumber = (double)newValue;
            }

            if (newNumber <= AxisMinimum.Number)
            {
                double oldNumber = (double)oldValue;
                _ignoreMaxMinChange = true;
                AxisMaximum.Number = oldNumber;
                _ignoreMaxMinChange = false;
                cancel = true;
            }
        }

        private string _stringFormatCategory = "";
        private string _stringFormatDecimals = "";

        private void LabelType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            if (comboBox.SelectedIndex == -1) return;
            if (Axis.Parent == null) return;
            if (Axis.Parent.GetType() != typeof(Wpf.Plot)) return;

            switch (comboBox.SelectedIndex)
            {
                case 0: // Currency
                    _stringFormatCategory = "C";
                    break;
                case 1: // General
                    _stringFormatCategory = "G";
                    break;
                case 2: // Number
                    _stringFormatCategory = "N";
                    break;
                case 3: // Percent
                    _stringFormatCategory = "P";
                    break;
                case 4: // Scientific
                    _stringFormatCategory = "E";
                    break;
            }
            Axis.StringFormat = _stringFormatCategory + _stringFormatDecimals;
        }

        private void DecimalPlaces_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GenericControls.NumericAutoPropertyControl.Number))
            {
                if (DecimalPlaces.Number.ToString() == "NaN")
                    _stringFormatDecimals = "";
                else
                    _stringFormatDecimals = DecimalPlaces.Number.ToString();

                Axis.StringFormat = _stringFormatCategory + _stringFormatDecimals;
            }
        }

        private void DecimalPlaces_PreviewNumberChanged(object oldValue, object newValue, ref bool cancel)
        {
            double newNumber;
            if (!double.TryParse(newValue.ToString(), out newNumber))
            {
                if (newValue.GetType() != typeof(double)) return;
                newNumber = (double)newValue;
            }
            DecimalPlaces.Number = Math.Floor(newNumber);
        }
    }

    /// <summary>
    /// Converter for determining if an axis is reversed based on start and end positions.
    /// </summary>
    public class ReverseAxisConverter : IMultiValueConverter
    {
        private Wpf.Axis _axis;

        /// <summary>
        /// Converts start position, end position, and axis to a boolean indicating if the axis is reversed.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double startPosition;
            double.TryParse(values[0].ToString(), out startPosition);
            double endPosition;
            double.TryParse(values[1].ToString(), out endPosition);

            if (values[2] == null)
            {
                _axis = null;
                return false;
            }
            _axis = values[2] as Wpf.Axis;

            return endPosition < startPosition;
        }

        /// <summary>
        /// Converts a reversed boolean back to start and end positions.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_axis == null) return new object[] { 0.0, 1.0, null };

            bool result;
            bool.TryParse(value.ToString(), out result);

            bool switchPositions = false;
            if (result == false)
                switchPositions = _axis.StartPosition > _axis.EndPosition;
            else
                switchPositions = _axis.StartPosition < _axis.EndPosition;

            if (switchPositions) return new object[] { _axis.EndPosition, _axis.StartPosition, _axis };

            return new object[] { _axis.StartPosition, _axis.EndPosition, _axis };
        }
    }

    /// <summary>
    /// Converts OxyPlot LineStyle to WPF DoubleCollection for dash arrays.
    /// Returns matching instances from LineStyleOptions for proper ComboBox selection.
    /// </summary>
    public class OxyLineStyleToDashArrayConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OxyPlot LineStyle to a WPF DoubleCollection dash array.
        /// Returns the matching instance from LineStyleOptions to ensure proper ComboBox selection.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            if (value.GetType() != typeof(OxyPlot.LineStyle)) return AxisControl.LineStyleOptions?.FirstOrDefault() ?? new DoubleCollection();

            var lineStyle = (OxyPlot.LineStyle)value;
            double[] dashArray;

            if (lineStyle == LineStyle.Solid)
            {
                dashArray = Array.Empty<double>();
            }
            else
            {
                dashArray = lineStyle.GetDashArray();
                if (dashArray == null)
                {
                    dashArray = new double[] { 0 };
                }
            }

            // Find matching instance from LineStyleOptions for proper ComboBox selection
            var options = AxisControl.LineStyleOptions;
            if (options != null)
            {
                foreach (var option in options)
                {
                    if (option.Count == dashArray.Length && option.SequenceEqual(dashArray))
                    {
                        return option;
                    }
                }
            }

            // Fallback: return new collection (won't select in ComboBox, but won't crash)
            return new DoubleCollection(dashArray);
        }

        /// <summary>
        /// Converts a WPF DoubleCollection dash array back to an OxyPlot LineStyle.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return OxyPlot.LineStyle.None;
            if (value.GetType() != typeof(DoubleCollection)) return OxyPlot.LineStyle.None;
            var dashArray = (DoubleCollection)value;

            if (dashArray.Count == 0) return OxyPlot.LineStyle.Solid;

            foreach (var style in (OxyPlot.LineStyle[])Enum.GetValues(typeof(OxyPlot.LineStyle)))
            {
                var oxyArray = style.GetDashArray();
                if (oxyArray != null && dashArray.SequenceEqual(oxyArray)) return style;
            }

            return OxyPlot.LineStyle.None;
        }
    }

    /// <summary>
    /// Converts empty strings to null values for binding purposes.
    /// </summary>
    public class EmptyStringToNullConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value to its string representation.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            return value.ToString();
        }

        /// <summary>
        /// Converts empty strings to null, otherwise returns the value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value.ToString() == "") return null;
            return value;
        }
    }

    /// <summary>
    /// Converts between tab control dimensions for vertical tab sizing.
    /// </summary>
    public class VerticalTabSizeConverter : IMultiValueConverter
    {
        /// <summary>
        /// Calculates the height for each tab based on the tab control size and item count.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var tabControl = (TabControl)values[0];
            double height = tabControl.ActualHeight / tabControl.Items.Count;
            if (height < 12) return 0;
            return height - 1;
        }

        /// <summary>
        /// Not implemented. Throws NotImplementedException.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts between OxyPlot date axis double values and DateTime objects.
    /// </summary>
    public class DateToNumberConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OxyPlot double date value to a DateTime.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(double)) return value;
            return OxyPlot.Axes.DateTimeAxis.ToDateTime((double)value);
        }

        /// <summary>
        /// Converts a DateTime to an OxyPlot double date value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(DateTime)) return value;

            var dt = (DateTime)value;
            if (dt.Equals(DateTime.MinValue)) return double.NaN;
            return OxyPlot.Axes.DateTimeAxis.ToDouble((DateTime)value);
        }
    }
}
