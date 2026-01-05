using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot;
using Wpf = OxyPlot.Wpf;

namespace OxyPlotControls
{
    /// <summary>
    /// A user control that provides UI for editing legend properties of an OxyPlot chart,
    /// including title, items, area styling, and position settings.
    /// </summary>
    public partial class LegendControl : UserControl
    {
        /// <summary>
        /// The XML tag name used for serializing legend properties.
        /// </summary>
        public static readonly string LegendPropertiesTag = "Legend";

        /// <summary>
        /// Identifies the <see cref="Plot"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotProperty = DependencyProperty.Register(
            nameof(Plot), typeof(Wpf.Plot), typeof(LegendControl),
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
        /// Gets the available legend orientation options.
        /// </summary>
        public static List<LegendOrientation> OrientationOptions { get; } = new List<LegendOrientation>((LegendOrientation[])Enum.GetValues(typeof(LegendOrientation)));

        /// <summary>
        /// Gets the available legend item order options.
        /// </summary>
        public static List<LegendItemOrder> ItemOrderOptions { get; } = new List<LegendItemOrder>((LegendItemOrder[])Enum.GetValues(typeof(LegendItemOrder)));

        /// <summary>
        /// Gets the available legend placement options.
        /// </summary>
        public static List<LegendPlacement> PlacementOptions { get; } = new List<LegendPlacement>((LegendPlacement[])Enum.GetValues(typeof(LegendPlacement)));

        /// <summary>
        /// Gets the available legend position options.
        /// </summary>
        public static List<LegendPosition> PositionOptions { get; } = new List<LegendPosition>((LegendPosition[])Enum.GetValues(typeof(LegendPosition)));

        /// <summary>
        /// Gets the available legend symbol placement options.
        /// </summary>
        public static List<LegendSymbolPlacement> SymbolPlacementOptions { get; } = new List<LegendSymbolPlacement>((LegendSymbolPlacement[])Enum.GetValues(typeof(LegendSymbolPlacement)));

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle), typeof(Style), typeof(LegendControl));

        /// <summary>
        /// Gets or sets the style applied to expander controls within this control.
        /// </summary>
        public Style ExpanderStyle
        {
            get { return (Style)GetValue(ExpanderStyleProperty); }
            set { SetValue(ExpanderStyleProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendControl"/> class.
        /// </summary>
        public LegendControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the Plot property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnPlotChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LegendControl control && e.NewValue != null)
            {
                // Force layout update to sync bindings
                control.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
                {
                    control.UpdateLayout();
                }));
            }
        }

        /// <summary>
        /// Handles legend property ComboBox selection changes and invalidates the plot to refresh the display.
        /// </summary>
        private void LegendPropertyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the plot to refresh the legend display
            // Use true to force a full update including layout recalculation
            if (Plot != null)
            {
                Plot.InvalidatePlot(true);
            }
        }

        /// <summary>
        /// Serializes legend properties to an XML element for persistence.
        /// </summary>
        /// <param name="plot">The OxyPlot Plot control whose legend properties will be serialized.</param>
        /// <returns>An XElement containing all serialized legend properties.</returns>
        public static XElement LegendPropertiesToXElement(Wpf.Plot plot)
        {
            var legendProperties = new XElement(LegendPropertiesTag);

            var fwc = new FontWeightConverter();

            // Legend Area
            var legendAreaProperties = new XElement("Area");
            legendAreaProperties.SetAttributeValue(nameof(plot.IsLegendVisible), plot.IsLegendVisible);
            legendAreaProperties.SetAttributeValue(nameof(plot.LegendBackground), plot.LegendBackground.ToString());
            legendAreaProperties.SetAttributeValue(nameof(plot.LegendBorder), plot.LegendBorder.ToString());
            legendAreaProperties.SetAttributeValue(nameof(plot.LegendBorderThickness), plot.LegendBorderThickness.ToString("G17", CultureInfo.InvariantCulture));
            legendAreaProperties.SetAttributeValue(nameof(plot.LegendPadding), plot.LegendPadding.ToString("G17", CultureInfo.InvariantCulture));
            legendProperties.Add(legendAreaProperties);

            // Legend Position Properties
            var subTitleProperties = new XElement("Position");
            subTitleProperties.SetAttributeValue(nameof(plot.LegendPlacement), plot.LegendPlacement.ToString());
            subTitleProperties.SetAttributeValue(nameof(plot.LegendPosition), plot.LegendPosition.ToString());
            subTitleProperties.SetAttributeValue(nameof(plot.LegendOrientation), plot.LegendOrientation.ToString());
            legendProperties.Add(subTitleProperties);

            // Title Properties
            var titleProperties = new XElement("Title");
            titleProperties.SetAttributeValue(nameof(plot.LegendTitle), plot.LegendTitle);
            titleProperties.SetAttributeValue(nameof(plot.LegendTitleColor), plot.LegendTitleColor.ToString());
            titleProperties.SetAttributeValue(nameof(plot.LegendTitleFont), plot.LegendTitleFont);
            titleProperties.SetAttributeValue(nameof(plot.LegendTitleFontSize), plot.LegendTitleFontSize.ToString("G17", CultureInfo.InvariantCulture));
            titleProperties.SetAttributeValue(nameof(plot.LegendTitleFontWeight), fwc.ConvertToInvariantString(plot.LegendTitleFontWeight));
            legendProperties.Add(titleProperties);

            // Legend Item Properties
            var itemProperties = new XElement("Items");
            itemProperties.SetAttributeValue(nameof(plot.LegendTextColor), plot.LegendTextColor.ToString());
            itemProperties.SetAttributeValue(nameof(plot.LegendSymbolLength), plot.LegendSymbolLength.ToString("G17", CultureInfo.InvariantCulture));
            itemProperties.SetAttributeValue(nameof(plot.LegendSymbolMargin), plot.LegendSymbolMargin.ToString("G17", CultureInfo.InvariantCulture));
            itemProperties.SetAttributeValue(nameof(plot.LegendSymbolPlacement), plot.LegendSymbolPlacement.ToString());
            itemProperties.SetAttributeValue(nameof(plot.LegendColumnSpacing), plot.LegendColumnSpacing.ToString("G17", CultureInfo.InvariantCulture));
            itemProperties.SetAttributeValue(nameof(plot.LegendItemAlignment), plot.LegendItemAlignment.ToString());
            itemProperties.SetAttributeValue(nameof(plot.LegendItemOrder), plot.LegendItemOrder.ToString());
            itemProperties.SetAttributeValue(nameof(plot.LegendItemSpacing), plot.LegendItemSpacing.ToString("G17", CultureInfo.InvariantCulture));
            itemProperties.SetAttributeValue(nameof(plot.LegendLineSpacing), plot.LegendLineSpacing.ToString("G17", CultureInfo.InvariantCulture));
            legendProperties.Add(itemProperties);

            return legendProperties;
        }

        /// <summary>
        /// Deserializes legend properties from an XML element and applies them to the plot.
        /// </summary>
        /// <param name="plot">The OxyPlot Plot control to apply settings to.</param>
        /// <param name="element">The XElement containing serialized legend properties.</param>
        public static void XElementToLegendProperties(Wpf.Plot plot, XElement element)
        {
            // Early Exit
            if (plot == null) return;
            if (element.Name != LegendPropertiesTag) return;

            // Set up converters
            var fontWeightConverter = new FontWeightConverter();

            // Area Properties
            var areaElement = element.Element("Area");
            if (areaElement != null)
            {
                bool isLegendVisible;
                if (OxyPlotSettingsSerializer.GetBooleanAttribute(areaElement, nameof(plot.IsLegendVisible), out isLegendVisible)) plot.IsLegendVisible = isLegendVisible;

                Color legendBackground;
                if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, nameof(plot.LegendBackground), out legendBackground)) plot.LegendBackground = legendBackground;

                Color legendBorder;
                if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, nameof(plot.LegendBorder), out legendBorder)) plot.LegendBorder = legendBorder;

                double legendBorderThickness;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(areaElement, nameof(plot.LegendBorderThickness), out legendBorderThickness)) plot.LegendBorderThickness = legendBorderThickness;

                double legendPadding;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(areaElement, nameof(plot.LegendPadding), out legendPadding)) plot.LegendPadding = legendPadding;

                // Backward compatibility
                if (OxyPlotSettingsSerializer.GetBooleanAttribute(areaElement, "LegendVisible", out isLegendVisible)) plot.IsLegendVisible = isLegendVisible;
                if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, "BackgroundColor", out legendBackground)) plot.LegendBackground = legendBackground;
                if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, "BorderColor", out legendBorder)) plot.LegendBorder = legendBorder;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(areaElement, "BorderThickness", out legendBorderThickness)) plot.LegendBorderThickness = legendBorderThickness;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(areaElement, "Padding", out legendPadding)) plot.LegendPadding = legendPadding;
            }

            // Position Properties
            var positionElement = element.Element("Position");
            if (positionElement != null)
            {
                LegendPlacement legendPlacement;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, nameof(plot.LegendPlacement), out legendPlacement)) plot.LegendPlacement = legendPlacement;

                LegendPosition legendPosition;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, nameof(plot.LegendPosition), out legendPosition)) plot.LegendPosition = legendPosition;

                LegendOrientation legendOrientation;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, nameof(plot.LegendOrientation), out legendOrientation)) plot.LegendOrientation = legendOrientation;

                // Backward compatibility
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, "Placement", out legendPlacement)) plot.LegendPlacement = legendPlacement;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, "Position", out legendPosition)) plot.LegendPosition = legendPosition;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, "Orientation", out legendOrientation)) plot.LegendOrientation = legendOrientation;
            }

            // Title Properties
            var titleElement = element.Element("Title");
            if (titleElement != null)
            {
                string legendTitle;
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(plot.LegendTitle), out legendTitle)) plot.LegendTitle = legendTitle;

                Color legendTitleColor;
                if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, nameof(plot.LegendTitleColor), out legendTitleColor)) plot.LegendTitleColor = legendTitleColor;

                string legendTitleFont;
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(plot.LegendTitleFont), out legendTitleFont)) plot.LegendTitleFont = legendTitleFont;

                double legendTitleFontSize;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(plot.LegendTitleFontSize), out legendTitleFontSize)) plot.LegendTitleFontSize = legendTitleFontSize;

                FontWeight legendTitleFontWeight;
                if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, nameof(plot.LegendTitleFontWeight), fontWeightConverter, out legendTitleFontWeight)) plot.LegendTitleFontWeight = legendTitleFontWeight;

                // Backward compatibility
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, "Title", out legendTitle)) plot.LegendTitle = legendTitle;
                if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, "Color", out legendTitleColor)) plot.LegendTitleColor = legendTitleColor;
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, "Font", out legendTitleFont)) plot.LegendTitleFont = legendTitleFont;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, "Size", out legendTitleFontSize)) plot.LegendTitleFontSize = legendTitleFontSize;
                if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, "Weight", fontWeightConverter, out legendTitleFontWeight)) plot.LegendTitleFontWeight = legendTitleFontWeight;
            }

            // Legend Item Properties
            var itemsElement = element.Element("Items");
            if (itemsElement != null)
            {
                Color legendTextColor;
                if (OxyPlotSettingsSerializer.GetColorAttribute(itemsElement, nameof(plot.LegendTextColor), out legendTextColor)) plot.LegendTextColor = legendTextColor;

                double legendSymbolLength;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(plot.LegendSymbolLength), out legendSymbolLength)) plot.LegendSymbolLength = legendSymbolLength;

                double legendSymbolMargin;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(plot.LegendSymbolMargin), out legendSymbolMargin)) plot.LegendSymbolMargin = legendSymbolMargin;

                LegendSymbolPlacement legendSymbolPlacement;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, nameof(plot.LegendSymbolPlacement), out legendSymbolPlacement)) plot.LegendSymbolPlacement = legendSymbolPlacement;

                double legendColumnSpacing;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(plot.LegendColumnSpacing), out legendColumnSpacing)) plot.LegendColumnSpacing = legendColumnSpacing;

                System.Windows.HorizontalAlignment legendItemAlignment;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, nameof(plot.LegendItemAlignment), out legendItemAlignment)) plot.LegendItemAlignment = legendItemAlignment;

                LegendItemOrder legendItemOrder;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, nameof(plot.LegendItemOrder), out legendItemOrder)) plot.LegendItemOrder = legendItemOrder;

                double legendItemSpacing;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(plot.LegendItemSpacing), out legendItemSpacing)) plot.LegendItemSpacing = legendItemSpacing;

                double legendLineSpacing;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(plot.LegendLineSpacing), out legendLineSpacing)) plot.LegendLineSpacing = legendLineSpacing;

                // Backward compatibility
                if (OxyPlotSettingsSerializer.GetColorAttribute(itemsElement, "Color", out legendTextColor)) plot.LegendTextColor = legendTextColor;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, "SymbolLength", out legendSymbolLength)) plot.LegendSymbolLength = legendSymbolLength;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, "SymbolMargin", out legendSymbolMargin)) plot.LegendSymbolMargin = legendSymbolMargin;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, "SymbolPlacement", out legendSymbolPlacement)) plot.LegendSymbolPlacement = legendSymbolPlacement;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, "ColumnSpacing", out legendColumnSpacing)) plot.LegendColumnSpacing = legendColumnSpacing;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, "ItemAlignment", out legendItemAlignment)) plot.LegendItemAlignment = legendItemAlignment;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, "ItemOrder", out legendItemOrder)) plot.LegendItemOrder = legendItemOrder;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, "ItemSpacing", out legendItemSpacing)) plot.LegendItemSpacing = legendItemSpacing;
                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, "LineSpacing", out legendLineSpacing)) plot.LegendLineSpacing = legendLineSpacing;
            }
        }
    }
}
