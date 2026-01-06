using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using OxyPlot;
using OxyPlot.Series;

namespace OxyPlotControls
{
    /// <summary>
    /// A control for editing box plot series properties including box style, whiskers, and outliers.
    /// </summary>
    public partial class BoxPlotSeriesControl : UserControl
    {
        #region Constants

        /// <summary>
        /// The XML tag used for box plot series properties in serialization.
        /// </summary>
        public static readonly string BoxPlotSeriesPropertiesTag = "BoxPlotSeries";

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the list of available marker types (excluding Custom).
        /// </summary>
        public static List<MarkerType> MarkerTypeOptions
        {
            get
            {
                var types = new List<MarkerType>((MarkerType[])Enum.GetValues(typeof(MarkerType)));
                types.Remove(MarkerType.Custom);
                return types;
            }
        }

        /// <summary>
        /// Gets the list of available line style options.
        /// </summary>
        public static List<DoubleCollection> LineStyleOptions => GenericControls.LineStyleSelectorControl.LineStyleOptions;

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series),
            typeof(BoxPlotSeries),
            typeof(BoxPlotSeriesControl),
            new PropertyMetadata(null, OnSeriesChanged));

        /// <summary>
        /// Gets or sets the box plot series whose properties are being edited.
        /// </summary>
        public BoxPlotSeries? Series
        {
            get => (BoxPlotSeries?)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle),
            typeof(Style),
            typeof(BoxPlotSeriesControl));

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
        /// Initializes a new instance of the <see cref="BoxPlotSeriesControl"/> class.
        /// </summary>
        public BoxPlotSeriesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the Series property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BoxPlotSeriesControl control && e.NewValue != null)
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
    }

    /// <summary>
    /// Converts box plot series fill color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class BoxPlotSeriesFillConverter : IMultiValueConverter
    {
        private BoxPlotSeries? _series;

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
            _series = values[1] as BoxPlotSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var fillColor = _series.Fill;
                return new SolidColorBrush(Color.FromArgb(fillColor.A, fillColor.R, fillColor.G, fillColor.B));
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

            if (OxyColor.ColorDifference(oxyCol, _series.Fill) == 0)
            {
                var fillColor = _series.Fill;
                return new object[] { Color.FromArgb(fillColor.A, fillColor.R, fillColor.G, fillColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }
}
