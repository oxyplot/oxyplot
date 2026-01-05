using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Legends;

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
        /// Identifies the <see cref="PlotModel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotModelProperty = DependencyProperty.Register(
            nameof(PlotModel), typeof(PlotModel), typeof(LegendControl),
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
        /// Gets the first legend from the PlotModel, creating one if it doesn't exist.
        /// </summary>
        public Legend? CurrentLegend
        {
            get
            {
                if (PlotModel == null) return null;
                var legend = PlotModel.Legends.FirstOrDefault() as Legend;
                if (legend == null)
                {
                    legend = new Legend();
                    PlotModel.Legends.Add(legend);
                }
                return legend;
            }
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
        /// Called when the PlotModel property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnPlotModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
            PlotModel?.InvalidatePlot(true);
        }

        /// <summary>
        /// Serializes legend properties to an XML element for persistence.
        /// </summary>
        /// <param name="plotModel">The PlotModel whose legend properties will be serialized.</param>
        /// <returns>An XElement containing all serialized legend properties.</returns>
        public static XElement LegendPropertiesToXElement(PlotModel plotModel)
        {
            var legendProperties = new XElement(LegendPropertiesTag);

            // Get the first legend (or create defaults if none)
            var legend = plotModel.Legends.FirstOrDefault() as Legend;

            // Legend Area
            var legendAreaProperties = new XElement("Area");
            legendAreaProperties.SetAttributeValue(nameof(plotModel.IsLegendVisible), plotModel.IsLegendVisible);
            if (legend != null)
            {
                legendAreaProperties.SetAttributeValue(nameof(legend.LegendBackground), OxyPlotSettingsSerializer.OxyColorToString(legend.LegendBackground));
                legendAreaProperties.SetAttributeValue(nameof(legend.LegendBorder), OxyPlotSettingsSerializer.OxyColorToString(legend.LegendBorder));
                legendAreaProperties.SetAttributeValue(nameof(legend.LegendBorderThickness), legend.LegendBorderThickness.ToString("G17", CultureInfo.InvariantCulture));
                legendAreaProperties.SetAttributeValue(nameof(legend.LegendPadding), legend.LegendPadding.ToString("G17", CultureInfo.InvariantCulture));
            }
            legendProperties.Add(legendAreaProperties);

            // Legend Position Properties
            var positionProperties = new XElement("Position");
            if (legend != null)
            {
                positionProperties.SetAttributeValue(nameof(legend.LegendPlacement), legend.LegendPlacement.ToString());
                positionProperties.SetAttributeValue(nameof(legend.LegendPosition), legend.LegendPosition.ToString());
                positionProperties.SetAttributeValue(nameof(legend.LegendOrientation), legend.LegendOrientation.ToString());
            }
            legendProperties.Add(positionProperties);

            // Title Properties
            var titleProperties = new XElement("Title");
            if (legend != null)
            {
                titleProperties.SetAttributeValue(nameof(legend.LegendTitle), legend.LegendTitle ?? "");
                titleProperties.SetAttributeValue(nameof(legend.LegendTitleColor), OxyPlotSettingsSerializer.OxyColorToString(legend.LegendTitleColor));
                titleProperties.SetAttributeValue(nameof(legend.LegendTitleFont), legend.LegendTitleFont ?? "");
                titleProperties.SetAttributeValue(nameof(legend.LegendTitleFontSize), legend.LegendTitleFontSize.ToString("G17", CultureInfo.InvariantCulture));
                titleProperties.SetAttributeValue(nameof(legend.LegendTitleFontWeight), legend.LegendTitleFontWeight.ToString("G17", CultureInfo.InvariantCulture));
            }
            legendProperties.Add(titleProperties);

            // Legend Item Properties
            var itemProperties = new XElement("Items");
            if (legend != null)
            {
                itemProperties.SetAttributeValue(nameof(legend.LegendTextColor), OxyPlotSettingsSerializer.OxyColorToString(legend.LegendTextColor));
                itemProperties.SetAttributeValue(nameof(legend.LegendSymbolLength), legend.LegendSymbolLength.ToString("G17", CultureInfo.InvariantCulture));
                itemProperties.SetAttributeValue(nameof(legend.LegendSymbolMargin), legend.LegendSymbolMargin.ToString("G17", CultureInfo.InvariantCulture));
                itemProperties.SetAttributeValue(nameof(legend.LegendSymbolPlacement), legend.LegendSymbolPlacement.ToString());
                itemProperties.SetAttributeValue(nameof(legend.LegendColumnSpacing), legend.LegendColumnSpacing.ToString("G17", CultureInfo.InvariantCulture));
                itemProperties.SetAttributeValue(nameof(legend.LegendItemAlignment), legend.LegendItemAlignment.ToString());
                itemProperties.SetAttributeValue(nameof(legend.LegendItemOrder), legend.LegendItemOrder.ToString());
                itemProperties.SetAttributeValue(nameof(legend.LegendItemSpacing), legend.LegendItemSpacing.ToString("G17", CultureInfo.InvariantCulture));
                itemProperties.SetAttributeValue(nameof(legend.LegendLineSpacing), legend.LegendLineSpacing.ToString("G17", CultureInfo.InvariantCulture));
            }
            legendProperties.Add(itemProperties);

            return legendProperties;
        }

        /// <summary>
        /// Deserializes legend properties from an XML element and applies them to the plot model.
        /// </summary>
        /// <param name="plotModel">The PlotModel to apply settings to.</param>
        /// <param name="element">The XElement containing serialized legend properties.</param>
        /// <param name="version">The serialization format version (1 for legacy, 2 for modern).</param>
        public static void XElementToLegendProperties(PlotModel plotModel, XElement element, int version = 2)
        {
            // Early Exit
            if (plotModel == null) return;
            if (element.Name != LegendPropertiesTag) return;

            // Get or create the first legend
            var legend = plotModel.Legends.FirstOrDefault() as Legend;
            if (legend == null)
            {
                legend = new Legend();
                plotModel.Legends.Add(legend);
            }

            // Set up converters for V1 backward compatibility
            var fontWeightConverter = new FontWeightConverter();

            // Area Properties
            var areaElement = element.Element("Area");
            if (areaElement != null)
            {
                if (OxyPlotSettingsSerializer.GetBooleanAttribute(areaElement, nameof(plotModel.IsLegendVisible), out bool isLegendVisible))
                    plotModel.IsLegendVisible = isLegendVisible;

                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(areaElement, nameof(legend.LegendBackground), out OxyColor legendBackground))
                    legend.LegendBackground = legendBackground;

                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(areaElement, nameof(legend.LegendBorder), out OxyColor legendBorder))
                    legend.LegendBorder = legendBorder;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(areaElement, nameof(legend.LegendBorderThickness), out double legendBorderThickness))
                    legend.LegendBorderThickness = legendBorderThickness;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(areaElement, nameof(legend.LegendPadding), out double legendPadding))
                    legend.LegendPadding = legendPadding;

                // V1 Backward compatibility - colors were stored as WPF Color strings
                if (version == 1)
                {
                    if (OxyPlotSettingsSerializer.GetBooleanAttribute(areaElement, "LegendVisible", out isLegendVisible))
                        plotModel.IsLegendVisible = isLegendVisible;
                    if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, "LegendBackground", out Color wpfColor))
                        legend.LegendBackground = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, "BackgroundColor", out wpfColor))
                        legend.LegendBackground = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, "LegendBorder", out wpfColor))
                        legend.LegendBorder = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(areaElement, "BorderColor", out wpfColor))
                        legend.LegendBorder = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                }
            }

            // Position Properties
            var positionElement = element.Element("Position");
            if (positionElement != null)
            {
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, nameof(legend.LegendPlacement), out LegendPlacement legendPlacement))
                    legend.LegendPlacement = legendPlacement;

                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, nameof(legend.LegendPosition), out LegendPosition legendPosition))
                    legend.LegendPosition = legendPosition;

                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, nameof(legend.LegendOrientation), out LegendOrientation legendOrientation))
                    legend.LegendOrientation = legendOrientation;

                // Backward compatibility
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, "Placement", out legendPlacement))
                    legend.LegendPlacement = legendPlacement;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, "Position", out legendPosition))
                    legend.LegendPosition = legendPosition;
                if (OxyPlotSettingsSerializer.GetEnumAttribute(positionElement, "Orientation", out legendOrientation))
                    legend.LegendOrientation = legendOrientation;
            }

            // Title Properties
            var titleElement = element.Element("Title");
            if (titleElement != null)
            {
                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(legend.LegendTitle), out string legendTitle))
                    legend.LegendTitle = legendTitle;

                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(titleElement, nameof(legend.LegendTitleColor), out OxyColor legendTitleColor))
                    legend.LegendTitleColor = legendTitleColor;

                if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, nameof(legend.LegendTitleFont), out string legendTitleFont))
                    legend.LegendTitleFont = legendTitleFont;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(legend.LegendTitleFontSize), out double legendTitleFontSize))
                    legend.LegendTitleFontSize = legendTitleFontSize;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(titleElement, nameof(legend.LegendTitleFontWeight), out double legendTitleFontWeight))
                    legend.LegendTitleFontWeight = legendTitleFontWeight;

                // V1 Backward compatibility
                if (version == 1)
                {
                    if (OxyPlotSettingsSerializer.GetStringAttribute(titleElement, "Title", out legendTitle))
                        legend.LegendTitle = legendTitle;
                    if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, "LegendTitleColor", out Color wpfColor))
                        legend.LegendTitleColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(titleElement, "Color", out wpfColor))
                        legend.LegendTitleColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, "LegendTitleFontWeight", fontWeightConverter, out FontWeight wpfWeight))
                        legend.LegendTitleFontWeight = wpfWeight.ToOpenTypeWeight();
                    if (OxyPlotSettingsSerializer.GetFontWeightAttribute(titleElement, "Weight", fontWeightConverter, out wpfWeight))
                        legend.LegendTitleFontWeight = wpfWeight.ToOpenTypeWeight();
                }
            }

            // Legend Item Properties
            var itemsElement = element.Element("Items");
            if (itemsElement != null)
            {
                if (OxyPlotSettingsSerializer.GetOxyColorAttribute(itemsElement, nameof(legend.LegendTextColor), out OxyColor legendTextColor))
                    legend.LegendTextColor = legendTextColor;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(legend.LegendSymbolLength), out double legendSymbolLength))
                    legend.LegendSymbolLength = legendSymbolLength;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(legend.LegendSymbolMargin), out double legendSymbolMargin))
                    legend.LegendSymbolMargin = legendSymbolMargin;

                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, nameof(legend.LegendSymbolPlacement), out LegendSymbolPlacement legendSymbolPlacement))
                    legend.LegendSymbolPlacement = legendSymbolPlacement;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(legend.LegendColumnSpacing), out double legendColumnSpacing))
                    legend.LegendColumnSpacing = legendColumnSpacing;

                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, nameof(legend.LegendItemAlignment), out HorizontalAlignment legendItemAlignment))
                    legend.LegendItemAlignment = legendItemAlignment;

                if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, nameof(legend.LegendItemOrder), out LegendItemOrder legendItemOrder))
                    legend.LegendItemOrder = legendItemOrder;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(legend.LegendItemSpacing), out double legendItemSpacing))
                    legend.LegendItemSpacing = legendItemSpacing;

                if (OxyPlotSettingsSerializer.GetDoubleAttribute(itemsElement, nameof(legend.LegendLineSpacing), out double legendLineSpacing))
                    legend.LegendLineSpacing = legendLineSpacing;

                // V1 Backward compatibility
                if (version == 1)
                {
                    if (OxyPlotSettingsSerializer.GetColorAttribute(itemsElement, "LegendTextColor", out Color wpfColor))
                        legend.LegendTextColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    if (OxyPlotSettingsSerializer.GetColorAttribute(itemsElement, "Color", out wpfColor))
                        legend.LegendTextColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                    // WPF HorizontalAlignment maps to OxyPlot HorizontalAlignment
                    if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, "LegendItemAlignment", out System.Windows.HorizontalAlignment wpfAlignment))
                        legend.LegendItemAlignment = (HorizontalAlignment)(int)wpfAlignment;
                    if (OxyPlotSettingsSerializer.GetEnumAttribute(itemsElement, "ItemAlignment", out wpfAlignment))
                        legend.LegendItemAlignment = (HorizontalAlignment)(int)wpfAlignment;
                }
            }
        }
    }
}
