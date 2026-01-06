using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Series;
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
            typeof(BarSeriesBase),
            typeof(BarSeriesControl),
            new PropertyMetadata(null, OnSeriesChanged));

        /// <summary>
        /// Gets or sets the bar series whose properties are being edited.
        /// </summary>
        public BarSeriesBase? Series
        {
            get => (BarSeriesBase?)GetValue(SeriesProperty);
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
        public Style? ExpanderStyle
        {
            get => (Style?)GetValue(ExpanderStyleProperty);
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
        public static void XElementToBarSeriesProperties(BarSeries barSeries, XElement element)
        {
            // Early Exit
            if (barSeries == null) return;
            if (element.Name != BarSeriesPropertiesTag) return;

            // Labeling Properties
            var labelingElement = element.Element("Labeling");
            if (labelingElement != null)
            {
                if (labelingElement.Attribute("Title") != null)
                    barSeries.Title = labelingElement.Attribute("Title")!.Value;
                if (labelingElement.Attribute("LabelPlacement") != null)
                {
                    if (GetEnumAttribute(labelingElement, "LabelPlacement", out LabelPlacement placement))
                        barSeries.LabelPlacement = placement;
                }
                if (labelingElement.Attribute("Font") != null)
                    barSeries.Font = labelingElement.Attribute("Font")!.Value;
                if (labelingElement.Attribute("FontSize") != null)
                {
                    if (double.TryParse(labelingElement.Attribute("FontSize")!.Value, out double fontSize))
                        barSeries.FontSize = fontSize;
                }
                if (labelingElement.Attribute("RenderInLegend") != null)
                    barSeries.RenderInLegend = Convert.ToBoolean(labelingElement.Attribute("RenderInLegend")!.Value);
                if (labelingElement.Attribute("XAxisKey") != null)
                    barSeries.XAxisKey = labelingElement.Attribute("XAxisKey")!.Value;
                if (labelingElement.Attribute("YAxisKey") != null)
                    barSeries.YAxisKey = labelingElement.Attribute("YAxisKey")!.Value;
                if (labelingElement.Attribute("TrackerKey") != null)
                    barSeries.TrackerKey = labelingElement.Attribute("TrackerKey")!.Value;
                if (labelingElement.Attribute("TrackerFormat") != null)
                    barSeries.TrackerFormatString = labelingElement.Attribute("TrackerFormat")!.Value;
                if (labelingElement.Attribute("LabelFormat") != null)
                    barSeries.LabelFormatString = labelingElement.Attribute("LabelFormat")!.Value;
            }

            // Display Properties
            var displayElement = element.Element("Display");
            if (displayElement != null)
            {
                if (GetColorAttribute(displayElement, "Fill", out var fill))
                    barSeries.FillColor = OxyColor.FromArgb(fill.A, fill.R, fill.G, fill.B);
                if (GetColorAttribute(displayElement, "Stroke", out var stroke))
                    barSeries.StrokeColor = OxyColor.FromArgb(stroke.A, stroke.R, stroke.G, stroke.B);
                if (displayElement.Attribute("LineThickness") != null)
                {
                    if (double.TryParse(displayElement.Attribute("LineThickness")!.Value, out double strokeThickness))
                        barSeries.StrokeThickness = strokeThickness;
                }
            }
        }

        /// <summary>
        /// Serializes bar series properties to an XElement.
        /// </summary>
        /// <param name="barSeries">The bar series to serialize.</param>
        /// <returns>An XElement containing the bar series properties.</returns>
        public static XElement BarSeriesPropertiesToXElement(BarSeries barSeries)
        {
            var properties = new XElement(BarSeriesPropertiesTag);

            // Labeling properties
            var labelProps = new XElement("Labeling");
            labelProps.SetAttributeValue("Title", barSeries.Title);
            labelProps.SetAttributeValue("LabelPlacement", barSeries.LabelPlacement.ToString());
            labelProps.SetAttributeValue("Font", barSeries.Font);
            labelProps.SetAttributeValue("FontSize", barSeries.FontSize);
            labelProps.SetAttributeValue("RenderInLegend", barSeries.RenderInLegend);
            labelProps.SetAttributeValue("XAxisKey", barSeries.XAxisKey);
            labelProps.SetAttributeValue("YAxisKey", barSeries.YAxisKey);
            labelProps.SetAttributeValue("TrackerKey", barSeries.TrackerKey);
            labelProps.SetAttributeValue("TrackerFormat", barSeries.TrackerFormatString);
            labelProps.SetAttributeValue("LabelFormat", barSeries.LabelFormatString);
            properties.Add(labelProps);

            var displayProps = new XElement("Display");
            displayProps.SetAttributeValue("Fill", barSeries.FillColor.ToString());
            displayProps.SetAttributeValue("Stroke", barSeries.StrokeColor.ToString());
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
        private BarSeriesBase? _series;

        /// <summary>
        /// Converts a fill color and series to a SolidColorBrush.
        /// </summary>
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series directly (core type)
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as BarSeriesBase;
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
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            // Get color value
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
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
