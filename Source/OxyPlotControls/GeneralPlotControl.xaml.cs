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
using Wpf = OxyPlot.Wpf;

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
        /// Identifies the <see cref="Plot"/> dependency property.
        /// </summary>
        public static DependencyProperty PlotProperty = DependencyProperty.Register(
            nameof(Plot), typeof(Wpf.Plot), typeof(GeneralPlotControl),
            new PropertyMetadata(null, OnPlotChanged));

        /// <summary>
        /// Gets or sets the OxyPlot Plot control that this control edits.
        /// </summary>
        public Wpf.Plot Plot
        {
            get { return (Wpf.Plot)GetValue(PlotProperty); }
            set { SetValue(PlotProperty, value); }
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
        /// Called when the Plot property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnPlotChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
        /// <param name="plot">The OxyPlot Plot control whose properties will be serialized.</param>
        /// <returns>An XElement containing all serialized general plot properties.</returns>
        public static XElement GeneralPropertiesToXElement(Wpf.Plot plot)
        {
            var generalProperties = new XElement(GeneralPropertiesTag);
            generalProperties.SetAttributeValue(nameof(plot.IsEnabled), plot.IsEnabled.ToString());

            var weightConverter = new FontWeightConverter();

            // Title Properties
            var titleProperties = new XElement("Title");
            titleProperties.SetAttributeValue(nameof(plot.Title), plot.Title);
            titleProperties.SetAttributeValue(nameof(plot.TitleColor), plot.TitleColor.ToString());
            titleProperties.SetAttributeValue(nameof(plot.TitleFont), plot.TitleFont);
            titleProperties.SetAttributeValue(nameof(plot.TitleFontSize), plot.TitleFontSize.ToString("G17", CultureInfo.InvariantCulture));
            titleProperties.SetAttributeValue(nameof(plot.TitleFontWeight), weightConverter.ConvertToInvariantString(plot.TitleFontWeight));
            titleProperties.SetAttributeValue(nameof(plot.TitlePadding), plot.TitlePadding.ToString("G17", CultureInfo.InvariantCulture));
            generalProperties.Add(titleProperties);

            // SubTitle Properties
            var subTitleProperties = new XElement("Subtitle");
            subTitleProperties.SetAttributeValue(nameof(plot.Subtitle), plot.Subtitle);
            subTitleProperties.SetAttributeValue(nameof(plot.SubtitleColor), plot.SubtitleColor.ToString());
            subTitleProperties.SetAttributeValue(nameof(plot.SubtitleFont), plot.SubtitleFont);
            subTitleProperties.SetAttributeValue(nameof(plot.SubtitleFontSize), plot.SubtitleFontSize.ToString("G17", CultureInfo.InvariantCulture));
            subTitleProperties.SetAttributeValue(nameof(plot.SubtitleFontWeight), weightConverter.ConvertToInvariantString(plot.SubtitleFontWeight));
            generalProperties.Add(subTitleProperties);

            // Chart Area Properties
            var chartProperties = new XElement("Chart");
            var bc = new BrushConverter();
            var tc = new ThicknessConverter();
            chartProperties.SetAttributeValue(nameof(plot.Background), bc.ConvertToInvariantString(plot.Background));
            chartProperties.SetAttributeValue(nameof(plot.BorderBrush), bc.ConvertToInvariantString(plot.BorderBrush));
            chartProperties.SetAttributeValue(nameof(plot.BorderThickness), tc.ConvertToInvariantString(plot.BorderThickness));
            generalProperties.Add(chartProperties);

            // Plot Area Properties
            var plotAreaProperties = new XElement("Plot");
            plotAreaProperties.SetAttributeValue(nameof(plot.PlotAreaBackground), bc.ConvertToInvariantString(plot.PlotAreaBackground));
            plotAreaProperties.SetAttributeValue(nameof(plot.PlotAreaBorderColor), plot.PlotAreaBorderColor.ToString());
            plotAreaProperties.SetAttributeValue(nameof(plot.PlotAreaBorderThickness), tc.ConvertToInvariantString(plot.PlotAreaBorderThickness));
            generalProperties.Add(plotAreaProperties);

            return generalProperties;
        }

        /// <summary>
        /// Deserializes general plot properties from an XML element and applies them to the plot.
        /// </summary>
        /// <param name="plot">The OxyPlot Plot control to apply settings to.</param>
        /// <param name="element">The XElement containing serialized general plot properties.</param>
        public static void XElementToGeneralProperties(Wpf.Plot plot, XElement element)
        {
            // Early Exit
            if (plot == null) return;
            if (element.Name != GeneralPropertiesTag) return;

            // Get enabled or not
            bool isEnabled;
            OxyPlotSettingsSerializer.GetBooleanAttribute(element, nameof(plot.IsEnabled), out isEnabled);
            plot.IsEnabled = isEnabled;

            // Set up converters
            var weightConverter = new FontWeightConverter();
            var thicknessConverter = new ThicknessConverter();
            var brushConverter = new BrushConverter();

            // Title Properties
            var titleElement = element.Element("Title");
            if (titleElement != null)
            {
                string titleStr;
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(plot.Title), out titleStr)) plot.Title = titleStr;

                Color titleColor;
                if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, nameof(plot.TitleColor), out titleColor)) plot.TitleColor = titleColor;

                string titleFont;
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(plot.TitleFont), out titleFont)) plot.TitleFont = titleFont;

                double titleFontSize;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(plot.TitleFontSize), out titleFontSize)) plot.TitleFontSize = titleFontSize;

                FontWeight titleFontWeight;
                if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, nameof(plot.TitleFontWeight), weightConverter, out titleFontWeight)) plot.TitleFontWeight = titleFontWeight;

                double titlePadding;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(plot.TitlePadding), out titlePadding)) plot.TitlePadding = titlePadding;

                // Backward compatibility
                if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, "Color", out titleColor)) plot.TitleColor = titleColor;
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, "Font", out titleFont)) plot.TitleFont = titleFont;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, "Size", out titleFontSize)) plot.TitleFontSize = titleFontSize;
                if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, "Weight", weightConverter, out titleFontWeight)) plot.TitleFontWeight = titleFontWeight;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, "Padding", out titlePadding)) plot.TitlePadding = titlePadding;
            }

            // SubTitle Properties
            var subTitleElement = element.Element("Subtitle");
            if (subTitleElement != null)
            {
                string subtitle;
                if (OxyPlotSettingsSerializer.GetStringAttribute(subTitleElement, nameof(plot.Subtitle), out subtitle)) plot.Subtitle = subtitle;

                Color subtitleColor;
                if (OxyPlotSettingsSerializer.GetColorAttribute(subTitleElement, nameof(plot.SubtitleColor), out subtitleColor)) plot.SubtitleColor = subtitleColor;

                string subtitleFont;
                if (OxyPlotSettingsSerializer.GetStringAttribute(subTitleElement, nameof(plot.SubtitleFont), out subtitleFont)) plot.SubtitleFont = subtitleFont;

                double subtitleFontSize;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(subTitleElement, nameof(plot.SubtitleFontSize), out subtitleFontSize)) plot.SubtitleFontSize = subtitleFontSize;

                FontWeight subtitleFontWeight;
                if (OxyPlotSettingsSerializer.GetFontWeightAttribute(subTitleElement, nameof(plot.SubtitleFontWeight), weightConverter, out subtitleFontWeight)) plot.SubtitleFontWeight = subtitleFontWeight;

                // Backward compatibility
                if (OxyPlotSettingsSerializer.GetStringAttribute(subTitleElement, "Title", out subtitle)) plot.Subtitle = subtitle;
                if (OxyPlotSettingsSerializer.GetColorAttribute(subTitleElement, "Color", out subtitleColor)) plot.SubtitleColor = subtitleColor;
                if (OxyPlotSettingsSerializer.GetStringAttribute(subTitleElement, "Font", out subtitleFont)) plot.SubtitleFont = subtitleFont;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(subTitleElement, "Size", out subtitleFontSize)) plot.SubtitleFontSize = subtitleFontSize;
                if (OxyPlotSettingsSerializer.GetFontWeightAttribute(subTitleElement, "Weight", weightConverter, out subtitleFontWeight)) plot.SubtitleFontWeight = subtitleFontWeight;
            }

            // Chart Area Properties
            var chartElement = element.Element("Chart");
            if (chartElement != null)
            {
                Brush background;
                if (OxyPlotSettingsSerializer.GetBrushAttribute(chartElement, nameof(plot.Background), brushConverter, out background)) plot.Background = background;

                Brush borderBrush;
                if (OxyPlotSettingsSerializer.GetBrushAttribute(chartElement, nameof(plot.BorderBrush), brushConverter, out borderBrush)) plot.BorderBrush = borderBrush;

                Thickness borderThickness;
                if (OxyPlotSettingsSerializer.GetThicknessAttribute(chartElement, nameof(plot.BorderThickness), thicknessConverter, out borderThickness)) plot.BorderThickness = borderThickness;

                // Backward compatibility
                var backgroundElement = chartElement.Element("BackgroundBrush");
                if (backgroundElement != null)
                {
                    var firstElement = backgroundElement.Elements().GetEnumerator();
                    if (firstElement.MoveNext())
                    {
                        var bg = DeserializeFromXElement(firstElement.Current) as Brush;
                        if (bg != null) plot.Background = bg;
                    }
                }

                var borderElement = chartElement.Element("BorderBrush");
                if (borderElement != null)
                {
                    var firstElement = borderElement.Elements().GetEnumerator();
                    if (firstElement.MoveNext())
                    {
                        var bg = DeserializeFromXElement(firstElement.Current) as Brush;
                        if (bg != null) plot.BorderBrush = bg;
                    }
                }
            }

            // Plot Area Properties
            var plotAreaElement = element.Element("Plot");
            if (plotAreaElement != null)
            {
                Brush plotAreaBackground;
                if (OxyPlotSettingsSerializer.GetBrushAttribute(plotAreaElement, nameof(plot.PlotAreaBackground), brushConverter, out plotAreaBackground)) plot.PlotAreaBackground = plotAreaBackground;

                Color plotAreaBorderColor;
                if (OxyPlotSettingsSerializer.GetColorAttribute(plotAreaElement, nameof(plot.PlotAreaBorderColor), out plotAreaBorderColor)) plot.PlotAreaBorderColor = plotAreaBorderColor;

                Thickness plotAreaBorderThickness;
                if (OxyPlotSettingsSerializer.GetThicknessAttribute(plotAreaElement, nameof(plot.PlotAreaBorderThickness), thicknessConverter, out plotAreaBorderThickness)) plot.PlotAreaBorderThickness = plotAreaBorderThickness;

                // Backward compatibility
                var backgroundElement = plotAreaElement.Element("BackgroundBrush");
                if (backgroundElement != null)
                {
                    var firstElement = backgroundElement.Elements().GetEnumerator();
                    if (firstElement.MoveNext())
                    {
                        var bg = DeserializeFromXElement(firstElement.Current) as Brush;
                        if (bg != null) plot.PlotAreaBackground = bg;
                    }
                }

                if (OxyPlotSettingsSerializer.GetColorAttribute(plotAreaElement, "BorderColor", out plotAreaBorderColor)) plot.PlotAreaBorderColor = plotAreaBorderColor;
                if (OxyPlotSettingsSerializer.GetThicknessAttribute(plotAreaElement, "BorderThickness", thicknessConverter, out plotAreaBorderThickness)) plot.PlotAreaBorderThickness = plotAreaBorderThickness;
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
    /// </summary>
    public class OxyAutomaticColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OxyPlot color to a WPF SolidColorBrush, handling the automatic color case.
        /// </summary>
        /// <param name="value">The color value to convert.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>A SolidColorBrush representing the color, with automatic colors converted to black.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            if (value.GetType() != typeof(Color)) return null;
            Color c = (Color)value;
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);
            if (oxyCol.IsAutomatic()) return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to a Color.
        /// </summary>
        /// <param name="value">The brush to convert.</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <param name="culture">The culture to use for conversion.</param>
        /// <returns>The Color from the SolidColorBrush.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color;
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
