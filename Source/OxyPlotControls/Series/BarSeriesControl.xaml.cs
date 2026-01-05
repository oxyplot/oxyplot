using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot;
using static OxyPlotControls.OxyPlotSettingsSerializer;

namespace OxyPlotControls
{
    /// <summary>
    /// A control for editing bar series properties including fill color, stroke, and bar width.
    /// </summary>
    public partial class BarSeriesControl : UserControl
    {
        #region Constants

        /// <summary>
        /// The XML tag used for bar series properties in serialization.
        /// </summary>
        public static readonly string BarSeriesPropertiesTag = "BarSeries";

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series),
            typeof(OxyPlot.Wpf.BarSeriesBase),
            typeof(BarSeriesControl),
            new PropertyMetadata(null, OnSeriesChanged));

        /// <summary>
        /// Gets or sets the bar series whose properties are being edited.
        /// </summary>
        public OxyPlot.Wpf.BarSeriesBase Series
        {
            get => (OxyPlot.Wpf.BarSeriesBase)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle),
            typeof(Style),
            typeof(BarSeriesControl));

        /// <summary>
        /// Gets or sets the style to apply to expanders in this control.
        /// </summary>
        public Style ExpanderStyle
        {
            get => (Style)GetValue(ExpanderStyleProperty);
            set => SetValue(ExpanderStyleProperty, value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesControl"/> class.
        /// </summary>
        public BarSeriesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the Series property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BarSeriesControl control && e.NewValue != null)
            {
                // Force layout update to sync bindings
                control.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
                {
                    control.UpdateLayout();
                }));
            }
        }

        /// <summary>
        /// Closes all expanders in this control.
        /// </summary>
        public void CloseExpanders()
        {
            // LabelingEXP.IsExpanded = false;
            DisplayEXP.IsExpanded = false;
        }

        /// <summary>
        /// Expands the specified property section.
        /// </summary>
        /// <param name="expansionZone">The property section to expand.</param>
        public void Expand(OxyPlotPropertiesControl.PropertyEXP expansionZone)
        {
            switch (expansionZone)
            {
                case OxyPlotPropertiesControl.PropertyEXP.Series_General:
                    // LabelingEXP.IsExpanded = true;
                    DisplayEXP.IsExpanded = true;
                    break;
                case OxyPlotPropertiesControl.PropertyEXP.Series_Display:
                    DisplayEXP.IsExpanded = true;
                    break;
            }
        }

        #region Serialization

        /// <summary>
        /// Deserializes bar series properties from an XElement and applies them to a bar series.
        /// </summary>
        /// <param name="barSeries">The bar series to populate with properties.</param>
        /// <param name="element">The XElement containing bar series properties.</param>
        public static void XElementToBarSeriesProperties(OxyPlot.Wpf.BarSeries barSeries, XElement element)
        {
            // Early Exit
            if (barSeries == null) return;
            if (element.Name != BarSeriesPropertiesTag) return;

            // get enabled or not
            if (GetBooleanAttribute(element, "IsEnabled", out bool isEnabled)) barSeries.IsEnabled = isEnabled;

            // Set up converters
            var fontWeightConverter = new FontWeightConverter();
            var thicknessConverter = new ThicknessConverter();
            var oxycolorConverter = new OxyPlot.Wpf.OxyColorConverter();
            var brushConverter = new BrushConverter();

            // Labeling Properties
            var labelingElement = element.Element("Labeling");
            if (labelingElement != null)
            {
                if (labelingElement.Attribute("Title") != null)
                    barSeries.Title = labelingElement.Attribute("Title").Value;
                if (labelingElement.Attribute("LabelPlacement") != null)
                {
                    if (OxyPlotSettingsSerializer.GetEnumAttribute(labelingElement, "LabelPlacement", out OxyPlot.Series.LabelPlacement placement))
                        barSeries.LabelPlacement = placement;
                }
                if (labelingElement.Attribute("TextColor") != null)
                    barSeries.Foreground = (Brush)brushConverter.ConvertFromString(labelingElement.Attribute("TextColor").Value);
                if (labelingElement.Attribute("Font") != null)
                    barSeries.InternalSeries.Font = labelingElement.Attribute("Font").Value;
                if (labelingElement.Attribute("FontSize") != null)
                {
                    if (double.TryParse(labelingElement.Attribute("FontSize").Value, out double fontSize))
                        barSeries.FontSize = fontSize;
                }
                if (labelingElement.Attribute("FontWeight") != null)
                    barSeries.FontWeight = (FontWeight)fontWeightConverter.ConvertFromString(labelingElement.Attribute("FontWeight").Value);
                if (labelingElement.Attribute("Padding") != null)
                    barSeries.Padding = (Thickness)thicknessConverter.ConvertFromString(labelingElement.Attribute("Padding").Value);
                if (labelingElement.Attribute("RenderInLegend") != null)
                    barSeries.RenderInLegend = Convert.ToBoolean(labelingElement.Attribute("RenderInLegend").Value);
                if (labelingElement.Attribute("XAxisKey") != null)
                    barSeries.XAxisKey = labelingElement.Attribute("XAxisKey").Value;
                if (labelingElement.Attribute("YAxisKey") != null)
                    barSeries.YAxisKey = labelingElement.Attribute("YAxisKey").Value;
                if (labelingElement.Attribute("TrackerKey") != null)
                    barSeries.TrackerKey = labelingElement.Attribute("TrackerKey").Value;
                if (labelingElement.Attribute("TrackerFormat") != null)
                    barSeries.TrackerFormatString = labelingElement.Attribute("TrackerFormat").Value;
                if (labelingElement.Attribute("LabelFormat") != null)
                    barSeries.LabelFormatString = labelingElement.Attribute("LabelFormat").Value;
            }

            // Display Properties
            var displayElement = element.Element("Display");
            if (displayElement != null)
            {
                if (displayElement.Attribute("Background") != null)
                    barSeries.Background = (Brush)brushConverter.ConvertFromString(displayElement.Attribute("Background").Value);
                if (displayElement.Attribute("Color") != null)
                    barSeries.Color = (Color)ColorConverter.ConvertFromString(displayElement.Attribute("Color").Value);
                if (displayElement.Attribute("Fill") != null)
                    barSeries.FillColor = (Color)ColorConverter.ConvertFromString(displayElement.Attribute("Fill").Value);
                if (displayElement.Attribute("LineThickness") != null)
                {
                    if (double.TryParse(displayElement.Attribute("LineThickness").Value, out double strokeThickness))
                        barSeries.StrokeThickness = strokeThickness;
                }
            }
        }

        /// <summary>
        /// Serializes bar series properties to an XElement.
        /// </summary>
        /// <param name="barSeries">The bar series to serialize.</param>
        /// <returns>An XElement containing the bar series properties.</returns>
        public static XElement BarSeriesPropertiesToXElement(OxyPlot.Wpf.BarSeries barSeries)
        {
            var properties = new XElement(BarSeriesPropertiesTag);
            properties.SetAttributeValue("IsEnabled", barSeries.IsEnabled.ToString());

            // Labeling properties
            var labelProps = new XElement("Labeling");
            labelProps.SetAttributeValue("Title", barSeries.Title);
            labelProps.SetAttributeValue("LabelPlacement", barSeries.LabelPlacement.ToString());
            labelProps.SetAttributeValue("TextColor", barSeries.Foreground.ToString());
            labelProps.SetAttributeValue("Font", barSeries.FontFamily);
            labelProps.SetAttributeValue("FontSize", barSeries.FontSize);
            labelProps.SetAttributeValue("FontWeight", barSeries.FontWeight.ToString());
            labelProps.SetAttributeValue("Padding", barSeries.Padding.ToString());
            labelProps.SetAttributeValue("RenderInLegend", barSeries.RenderInLegend);
            labelProps.SetAttributeValue("XAxisKey", barSeries.XAxisKey);
            labelProps.SetAttributeValue("YAxisKey", barSeries.YAxisKey);
            labelProps.SetAttributeValue("TrackerKey", barSeries.TrackerKey);
            labelProps.SetAttributeValue("TrackerFormat", barSeries.TrackerFormatString);
            labelProps.SetAttributeValue("LabelFormat", barSeries.LabelFormatString);
            properties.Add(labelProps);

            var displayProps = new XElement("Display");
            displayProps.SetAttributeValue("Background", barSeries.Background);
            displayProps.SetAttributeValue("Color", barSeries.Foreground);
            displayProps.SetAttributeValue("Fill", barSeries.FillColor);
            displayProps.SetAttributeValue("LineThickness", barSeries.StrokeThickness);
            properties.Add(displayProps);

            return properties;
        }

        #endregion
    }

    /// <summary>
    /// Converts bar series fill color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class BarSeriesFillConverter : IMultiValueConverter
    {
        private OxyPlot.Series.BarSeriesBase _series;

        /// <summary>
        /// Converts a fill color and series to a SolidColorBrush.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series (this should only be set on convert with one-way binding)
            if (values[1] == null) return new SolidColorBrush(c);
            _series = ((OxyPlot.Wpf.BarSeriesBase)values[1]).InternalSeries as OxyPlot.Series.BarSeriesBase;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualFillColor;
                return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to fill color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            // Get color value
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            var c = ((SolidColorBrush)value).Color;
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            if (OxyColor.ColorDifference(oxyCol, _series.ActualFillColor) == 0)
            {
                var actualColor = _series.ActualFillColor;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }
}
