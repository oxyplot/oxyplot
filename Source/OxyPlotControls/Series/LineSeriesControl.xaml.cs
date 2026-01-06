/*
* NOTICE:
* The U.S. Army Corps of Engineers, Risk Management Center (USACE-RMC) makes no guarantees about
* the results, or appropriateness of outputs, obtained from this software.
*
* LIST OF CONDITIONS:
* Redistribution and use in source and binary forms, with or without modification, are permitted
* provided that the following conditions are met:
* ● Redistributions of source code must retain the above notice, this list of conditions, and the
* following disclaimer.
* ● Redistributions in binary form must reproduce the above notice, this list of conditions, and
* the following disclaimer in the documentation and/or other materials provided with the distribution.
* ● The names of the U.S. Government, the U.S. Army Corps of Engineers, the Institute for Water
* Resources, or the Risk Management Center may not be used to endorse or promote products derived
* from this software without specific prior written permission. Nor may the names of its contributors
* be used to endorse or promote products derived from this software without specific prior
* written permission.
*
* DISCLAIMER:
* THIS SOFTWARE IS PROVIDED BY THE U.S. ARMY CORPS OF ENGINEERS RISK MANAGEMENT CENTER
* (USACE-RMC) "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
* THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL USACE-RMC BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
* PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
* THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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
    /// A control for editing line series properties including line style, color, and markers.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     <b> Authors: </b>
    /// <list type="bullet">
    /// <item><description>
    ///     Haden Smith, USACE Risk Management Center, cole.h.smith@usace.army.mil
    /// </description></item>
    /// <item><description>
    ///     Woodrow Fields, USACE Risk Management Center, woodrow.l.fields@usace.army.mil
    /// </description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public partial class LineSeriesControl : UserControl
    {
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
        /// Gets the list of available line legend positions.
        /// </summary>
        public static List<LineLegendPosition> LineLegendPositionOptions { get; } =
            new List<LineLegendPosition>((LineLegendPosition[])Enum.GetValues(typeof(LineLegendPosition)));

        /// <summary>
        /// Gets the list of available line join options.
        /// </summary>
        public static List<LineJoin> LineJoinOptions { get; } =
            new List<LineJoin>((LineJoin[])Enum.GetValues(typeof(LineJoin)));

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
            typeof(LineSeries),
            typeof(LineSeriesControl),
            new PropertyMetadata(null, OnSeriesChanged));

        /// <summary>
        /// Gets or sets the line series whose properties are being edited.
        /// </summary>
        public LineSeries? Series
        {
            get => (LineSeries?)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle),
            typeof(Style),
            typeof(LineSeriesControl));

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
        /// Initializes a new instance of the <see cref="LineSeriesControl"/> class.
        /// </summary>
        public LineSeriesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the Series property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineSeriesControl control && e.NewValue != null)
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
            MarkersEXP.IsExpanded = false;
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
                case OxyPlotPropertiesControl.PropertyEXP.Series_Markers:
                    MarkersEXP.IsExpanded = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Converts line series color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class LineSeriesColorConverter : IMultiValueConverter
    {
        private LineSeries? _series;

        /// <summary>
        /// Converts a color and series to a SolidColorBrush.
        /// </summary>
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series directly (core type, not wrapper)
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as LineSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualColor;
                return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            // Get color value
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            var c = ((SolidColorBrush)value).Color;
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            if (OxyColor.ColorDifference(oxyCol, _series.ActualColor) == 0)
            {
                var actualColor = _series.ActualColor;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }

    /// <summary>
    /// Converts area series color2 to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class AreaSeriesColor2Converter : IMultiValueConverter
    {
        private AreaSeries? _series;

        /// <summary>
        /// Converts a color and series to a SolidColorBrush.
        /// </summary>
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series directly
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as AreaSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualColor2;
                return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            // Get color value
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            var c = ((SolidColorBrush)value).Color;
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            if (OxyColor.ColorDifference(oxyCol, _series.ActualColor2) == 0)
            {
                var actualColor = _series.ActualColor2;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }

    /// <summary>
    /// Converts area series fill color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class AreaSeriesFillConverter : IMultiValueConverter
    {
        private AreaSeries? _series;

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

            // Get Series directly
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as AreaSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualFill;
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

            if (OxyColor.ColorDifference(oxyCol, _series.ActualFill) == 0)
            {
                var actualColor = _series.ActualFill;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }

    /// <summary>
    /// Converts line series marker fill color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class LineSeriesMarkerFillConverter : IMultiValueConverter
    {
        private LineSeries? _series;

        /// <summary>
        /// Converts a marker fill color and series to a SolidColorBrush.
        /// </summary>
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series directly
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as LineSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualMarkerFill;
                return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to marker fill color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            // Get color value
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            var c = ((SolidColorBrush)value).Color;
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            if (OxyColor.ColorDifference(oxyCol, _series.ActualMarkerFill) == 0)
            {
                var actualColor = _series.ActualMarkerFill;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }

    /// <summary>
    /// Converts line series marker stroke color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class LineSeriesMarkerStrokeConverter : IMultiValueConverter
    {
        private LineSeries? _series;

        /// <summary>
        /// Converts a marker stroke color and series to a SolidColorBrush.
        /// </summary>
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series directly
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as LineSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualMarkerFill;
                return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to marker stroke color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            // Get color value
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null! };
            var c = ((SolidColorBrush)value).Color;
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            if (OxyColor.ColorDifference(oxyCol, _series.ActualMarkerFill) == 0)
            {
                var actualColor = _series.ActualMarkerFill;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }
}
