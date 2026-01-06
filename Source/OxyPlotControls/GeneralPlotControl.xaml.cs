using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using OxyPlot;

namespace OxyPlotControls
{
    /// <summary>
    /// A user control that provides UI for editing general plot properties such as title, subtitle,
    /// plot area styling, and background settings for an OxyPlot chart.
    /// </summary>
    public partial class GeneralPlotControl : UserControl
    {
        /// <summary>
        /// The XML tag name used for serializing general plot properties.
        /// </summary>
        public static readonly string GeneralPropertiesTag = "General";

        /// <summary>
        /// Identifies the <see cref="PlotModel"/> dependency property.
        /// </summary>
        public static DependencyProperty PlotModelProperty = DependencyProperty.Register(
            nameof(PlotModel), typeof(PlotModel), typeof(GeneralPlotControl),
            new PropertyMetadata(null, OnPlotModelChanged));

        /// <summary>
        /// Gets or sets the PlotModel that this control edits.
        /// </summary>
        public PlotModel PlotModel
        {
            get { return (PlotModel)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle), typeof(Style), typeof(GeneralPlotControl));

        /// <summary>
        /// Gets or sets the style applied to expander controls within this control.
        /// </summary>
        public Style ExpanderStyle
        {
            get { return (Style)GetValue(ExpanderStyleProperty); }
            set { SetValue(ExpanderStyleProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPlotControl"/> class.
        /// </summary>
        public GeneralPlotControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the PlotModel property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnPlotModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GeneralPlotControl control && e.NewValue != null)
            {
                // Force layout update to sync bindings
                control.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
                {
                    control.UpdateLayout();
                }));
            }
        }

        /// <summary>
        /// Serializes general plot properties to an XML element for persistence.
        /// </summary>
        /// <param name="plotModel">The PlotModel whose properties will be serialized.</param>
        /// <returns>An XElement containing all serialized general plot properties.</returns>
        public static XElement GeneralPropertiesToXElement(PlotModel plotModel)
        {
            var generalProperties = new XElement(GeneralPropertiesTag);
            generalProperties.SetAttributeValue(nameof(plotModel.IsLegendVisible), plotModel.IsLegendVisible.ToString());

            // Title Properties
            var titleProperties = new XElement("Title");
            titleProperties.SetAttributeValue(nameof(plotModel.Title), plotModel.Title ?? "");
            titleProperties.SetAttributeValue(nameof(plotModel.TitleColor), OxyPlotSettingsSerializer.OxyColorToString(plotModel.TitleColor));
            titleProperties.SetAttributeValue(nameof(plotModel.TitleFont), plotModel.TitleFont ?? "");
            titleProperties.SetAttributeValue(nameof(plotModel.TitleFontSize), plotModel.TitleFontSize.ToString("G17", CultureInfo.InvariantCulture));
            titleProperties.SetAttributeValue(nameof(plotModel.TitleFontWeight), plotModel.TitleFontWeight.ToString("G17", CultureInfo.InvariantCulture));
            titleProperties.SetAttributeValue(nameof(plotModel.TitlePadding), plotModel.TitlePadding.ToString("G17", CultureInfo.InvariantCulture));
            generalProperties.Add(titleProperties);

            // Subtitle Properties
            var subTitleProperties = new XElement("Subtitle");
            subTitleProperties.SetAttributeValue(nameof(plotModel.Subtitle), plotModel.Subtitle ?? "");
            subTitleProperties.SetAttributeValue(nameof(plotModel.SubtitleColor), OxyPlotSettingsSerializer.OxyColorToString(plotModel.SubtitleColor));
            subTitleProperties.SetAttributeValue(nameof(plotModel.SubtitleFont), plotModel.SubtitleFont ?? "");
            subTitleProperties.SetAttributeValue(nameof(plotModel.SubtitleFontSize), plotModel.SubtitleFontSize.ToString("G17", CultureInfo.InvariantCulture));
            subTitleProperties.SetAttributeValue(nameof(plotModel.SubtitleFontWeight), plotModel.SubtitleFontWeight.ToString("G17", CultureInfo.InvariantCulture));
            generalProperties.Add(subTitleProperties);

            // Chart Area Properties (Background)
            var chartProperties = new XElement("Chart");
            chartProperties.SetAttributeValue(nameof(plotModel.Background), OxyPlotSettingsSerializer.OxyColorToString(plotModel.Background));
            generalProperties.Add(chartProperties);

            // Plot Area Properties
            var plotAreaProperties = new XElement("Plot");
            plotAreaProperties.SetAttributeValue(nameof(plotModel.PlotAreaBackground), OxyPlotSettingsSerializer.OxyColorToString(plotModel.PlotAreaBackground));
            plotAreaProperties.SetAttributeValue(nameof(plotModel.PlotAreaBorderColor), OxyPlotSettingsSerializer.OxyColorToString(plotModel.PlotAreaBorderColor));
            plotAreaProperties.SetAttributeValue(nameof(plotModel.PlotAreaBorderThickness), OxyThicknessToString(plotModel.PlotAreaBorderThickness));
            generalProperties.Add(plotAreaProperties);

            return generalProperties;
        }

        /// <summary>
        /// Converts an OxyThickness to a string for XML serialization.
        /// </summary>
        private static string OxyThicknessToString(OxyThickness thickness)
        {
            return $"{thickness.Left.ToString("G17", CultureInfo.InvariantCulture)}," +
                   $"{thickness.Top.ToString("G17", CultureInfo.InvariantCulture)}," +
                   $"{thickness.Right.ToString("G17", CultureInfo.InvariantCulture)}," +
                   $"{thickness.Bottom.ToString("G17", CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// Parses an OxyThickness from a string.
        /// </summary>
        private static bool TryParseOxyThickness(string value, out OxyThickness thickness)
        {
            thickness = new OxyThickness(0);
            if (string.IsNullOrEmpty(value)) return false;

            var parts = value.Split(',');
            if (parts.Length == 1 && double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double uniform))
            {
                thickness = new OxyThickness(uniform);
                return true;
            }
            if (parts.Length == 4 &&
                double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double left) &&
                double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double top) &&
                double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double right) &&
                double.TryParse(parts[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double bottom))
            {
                thickness = new OxyThickness(left, top, right, bottom);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deserializes general plot properties from an XML element and applies them to the plot model.
        /// </summary>
        /// <param name="plotModel">The PlotModel to apply settings to.</param>
        /// <param name="element">The XElement containing serialized general plot properties.</param>
        /// <param name="version">The serialization format version (1 for legacy, 2 for modern).</param>
        public static void XElementToGeneralProperties(PlotModel plotModel, XElement element, int version = 2)
        {
            // Early Exit
            if (plotModel == null) return;
            if (element.Name != GeneralPropertiesTag) return;

            // Get visibility (V2 uses IsLegendVisible, V1 used IsEnabled on the plot)
            if (OxyPlotSettingsSerializer.GetBooleanAttribute(element, nameof(plotModel.IsLegendVisible), out bool isLegendVisible))
                plotModel.IsLegendVisible = isLegendVisible;

            // For backward compatibility with V1
            var weightConverter = new FontWeightConverter();

            // Title Properties
            var titleElement = element.Element("Title");
            if (titleElement != null)
            {
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(plotModel.Title), out string titleStr))
                    plotModel.Title = titleStr;

                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(titleElement, nameof(plotModel.TitleColor), out OxyColor titleColor))
                    plotModel.TitleColor = titleColor;

                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(plotModel.TitleFont), out string titleFont))
                    plotModel.TitleFont = titleFont;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(plotModel.TitleFontSize), out double titleFontSize))
                    plotModel.TitleFontSize = titleFontSize;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(plotModel.TitleFontWeight), out double titleFontWeight))
                    plotModel.TitleFontWeight = titleFontWeight;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(plotModel.TitlePadding), out double titlePadding))
                    plotModel.TitlePadding = titlePadding;

                // V1 Backward compatibility - font weight was stored as WPF FontWeight string
                if (version == 1)
                {
                    if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, "TitleFontWeight", weightConverter, out FontWeight wpfWeight))
                        plotModel.TitleFontWeight = wpfWeight.ToOpenTypeWeight();
                    if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, "TitleColor", out Color wpfColor))
                        plotModel.TitleColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, "Color", out wpfColor))
                        plotModel.TitleColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, "Weight", weightConverter, out wpfWeight))
                        plotModel.TitleFontWeight = wpfWeight.ToOpenTypeWeight();
                }
            }

            // Subtitle Properties
            var subTitleElement = element.Element("Subtitle");
            if (subTitleElement != null)
            {
                if (OxyPlotSettingsSerializer.GetStringAttribute(subTitleElement, nameof(plotModel.Subtitle), out string subtitle))
                    plotModel.Subtitle = subtitle;

                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(subTitleElement, nameof(plotModel.SubtitleColor), out OxyColor subtitleColor))
                    plotModel.SubtitleColor = subtitleColor;

                if (OxyPlotSettingsSerializer.GetStringAttribute(subTitleElement, nameof(plotModel.SubtitleFont), out string subtitleFont))
                    plotModel.SubtitleFont = subtitleFont;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(subTitleElement, nameof(plotModel.SubtitleFontSize), out double subtitleFontSize))
                    plotModel.SubtitleFontSize = subtitleFontSize;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(subTitleElement, nameof(plotModel.SubtitleFontWeight), out double subtitleFontWeight))
                    plotModel.SubtitleFontWeight = subtitleFontWeight;

                // V1 Backward compatibility
                if (version == 1)
                {
                    if (OxyPlotSettingsSerializer.GetStringAttribute(subTitleElement, "Title", out subtitle))
                        plotModel.Subtitle = subtitle;
                    if (OxyPlotSettingsSerializer.GetColorAttribute(subTitleElement, "SubtitleColor", out Color wpfColor))
                        plotModel.SubtitleColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(subTitleElement, "Color", out wpfColor))
                        plotModel.SubtitleColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetFontWeightAttribute(subTitleElement, "SubtitleFontWeight", weightConverter, out FontWeight wpfWeight))
                        plotModel.SubtitleFontWeight = wpfWeight.ToOpenTypeWeight();
                    if (OxyPlotSettingsSerializer.GetFontWeightAttribute(subTitleElement, "Weight", weightConverter, out wpfWeight))
                        plotModel.SubtitleFontWeight = wpfWeight.ToOpenTypeWeight();
                }
            }

            // Chart Area Properties (Background)
            var chartElement = element.Element("Chart");
            if (chartElement != null)
            {
                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(chartElement, nameof(plotModel.Background), out OxyColor background))
                    plotModel.Background = background;

                // V1 Backward compatibility - Background was stored as WPF Brush
                if (version == 1)
                {
                    var brushConverter = new BrushConverter();
                    if (OxyPlotSettingsSerializer.GetBrushAttribute(chartElement, "Background", brushConverter, out Brush bgBrush) && bgBrush is SolidColorBrush scb)
                        plotModel.Background = OxyColor.FromArgb(scb.Color.A, scb.Color.R, scb.Color.G, scb.Color.B);
                }
            }

            // Plot Area Properties
            var plotAreaElement = element.Element("Plot");
            if (plotAreaElement != null)
            {
                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(plotAreaElement, nameof(plotModel.PlotAreaBackground), out OxyColor plotAreaBackground))
                    plotModel.PlotAreaBackground = plotAreaBackground;

                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(plotAreaElement, nameof(plotModel.PlotAreaBorderColor), out OxyColor plotAreaBorderColor))
                    plotModel.PlotAreaBorderColor = plotAreaBorderColor;

                if (OxyPlotSettingsSerializer.GetStringAttribute(plotAreaElement, nameof(plotModel.PlotAreaBorderThickness), out string thicknessStr) &&
                    TryParseOxyThickness(thicknessStr, out OxyThickness plotAreaBorderThickness))
                    plotModel.PlotAreaBorderThickness = plotAreaBorderThickness;

                // V1 Backward compatibility
                if (version == 1)
                {
                    var brushConverter = new BrushConverter();
                    var thicknessConverter = new ThicknessConverter();

                    if (OxyPlotSettingsSerializer.GetBrushAttribute(plotAreaElement, "PlotAreaBackground", brushConverter, out Brush paBrush) && paBrush is SolidColorBrush scb)
                        plotModel.PlotAreaBackground = OxyColor.FromArgb(scb.Color.A, scb.Color.R, scb.Color.G, scb.Color.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(plotAreaElement, "PlotAreaBorderColor", out Color wpfColor))
                        plotModel.PlotAreaBorderColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(plotAreaElement, "BorderColor", out wpfColor))
                        plotModel.PlotAreaBorderColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetThicknessAttribute(plotAreaElement, "PlotAreaBorderThickness", thicknessConverter, out Thickness wpfThickness))
                        plotModel.PlotAreaBorderThickness = new OxyThickness(wpfThickness.Left, wpfThickness.Top, wpfThickness.Right, wpfThickness.Bottom);
                    if (OxyPlotSettingsSerializer.GetThicknessAttribute(plotAreaElement, "BorderThickness", thicknessConverter, out wpfThickness))
                        plotModel.PlotAreaBorderThickness = new OxyThickness(wpfThickness.Left, wpfThickness.Top, wpfThickness.Right, wpfThickness.Bottom);
                }
            }
        }

        /// <summary>
        /// Deserializes a XAML element from an XElement.
        /// </summary>
        /// <param name="element">The XElement to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromXElement(XElement element)
        {
            var doc = new XmlDocument();
            doc.LoadXml(element.ToString());
            return XamlReader.Load(new XmlNodeReader(doc));
        }
    }

    /// <summary>
    /// A value converter that handles OxyPlot's automatic color representation.
    /// OxyPlot uses ARGB(0,0,0,1) to represent an automatic color, which this converter
    /// translates to black for display purposes.
    /// Supports both OxyColor and WPF Color as input.
    /// </summary>
    public class OxyAutomaticColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OxyColor or WPF Color to a WPF SolidColorBrush, handling the automatic color case.
        /// </summary>
        /// <param name="value">The color value to convert (OxyColor or WPF Color).</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>A SolidColorBrush representing the color, with automatic colors converted to black.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return value;

            // Handle OxyColor directly
            if (value is OxyColor oxyCol)
            {
                if (oxyCol.IsAutomatic() || oxyCol.IsUndefined())
                    return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                return new SolidColorBrush(Color.FromArgb(oxyCol.A, oxyCol.R, oxyCol.G, oxyCol.B));
            }

            // Handle WPF Color (for backward compatibility)
            if (value is Color c)
            {
                var oxyColor = OxyColor.FromArgb(c.A, c.R, c.G, c.B);
                if (oxyColor.IsAutomatic())
                    return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                return new SolidColorBrush(c);
            }

            return null;
        }

        /// <summary>
        /// Converts a SolidColorBrush back to an OxyColor.
        /// </summary>
        /// <param name="value">The brush to convert.</param>
        /// <param name="targetType">The target type (OxyColor or Color).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>An OxyColor or Color from the SolidColorBrush, depending on target type.</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not SolidColorBrush scb) return null;

            if (targetType == typeof(OxyColor))
                return OxyColor.FromArgb(scb.Color.A, scb.Color.R, scb.Color.G, scb.Color.B);

            return scb.Color;
        }
    }

    /// <summary>
    /// A value converter that converts OxyThickness to/from a single double value.
    /// Uses uniform thickness (all sides equal to the double value).
    /// </summary>
    public class OxyThicknessToDoubleConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OxyThickness to a double, using the Left value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OxyThickness thickness)
                return thickness.Left;
            return 0.0;
        }

        /// <summary>
        /// Converts a double to an OxyThickness with uniform values.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return new OxyThickness(d);
            return new OxyThickness(0);
        }
    }

    /// <summary>
    /// A value converter that provides a default font size when the value is NaN or invalid.
    /// Returns 12.0 as the default font size.
    /// </summary>
    public class OxyDefaultFontSizeConverter : IValueConverter
    {
        /// <summary>
        /// Converts a font size value, returning a default of 12.0 for NaN or invalid values.
        /// </summary>
        /// <param name="value">The font size value to convert.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>The font size, or 12.0 if the value is NaN, infinite, or invalid.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 12.0;
            if (value.GetType() != typeof(double)) return 12.0;
            double doubleVal = (double)value;
            if (double.IsNaN(doubleVal) || double.IsInfinity(doubleVal)) return 12.0;
            return doubleVal;
        }

        /// <summary>
        /// Converts a font size back, returning NaN if the value is the default 12.0.
        /// </summary>
        /// <param name="value">The font size value to convert back.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>The font size, or NaN if the value is null, invalid, or 12.0.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return double.NaN;
            if (value.GetType() != typeof(double)) return double.NaN;
            double doubleVal = (double)value;
            if (doubleVal == 12.0) return double.NaN;
            return doubleVal;
        }
    }

    /// <summary>
    /// A simple value converter that casts between Brush and SolidColorBrush types.
    /// </summary>
    public class SolidColorBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value to a SolidColorBrush.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>The value cast to SolidColorBrush, or null if the value is null.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (SolidColorBrush)value;
        }

        /// <summary>
        /// Converts a SolidColorBrush back to a Brush.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>The value cast to Brush, or null if the value is null.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (Brush)value;
        }
    }
}
