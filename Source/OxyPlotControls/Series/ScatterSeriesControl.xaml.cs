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
    /// A control for editing scatter series properties including markers and error bars.
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
    public partial class ScatterSeriesControl : UserControl
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

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series),
            typeof(ScatterSeries),
            typeof(ScatterSeriesControl),
            new PropertyMetadata(null, OnSeriesChanged));

        /// <summary>
        /// Gets or sets the scatter series whose properties are being edited.
        /// </summary>
        public ScatterSeries? Series
        {
            get => (ScatterSeries?)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle),
            typeof(Style),
            typeof(ScatterSeriesControl));

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
        /// Initializes a new instance of the <see cref="ScatterSeriesControl"/> class.
        /// </summary>
        public ScatterSeriesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the Series property changes.
        /// Forces a layout update to ensure bindings are properly synchronized.
        /// </summary>
        private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScatterSeriesControl control && e.NewValue != null)
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
            LabelingEXP.IsExpanded = false;
            MarkersEXP.IsExpanded = false;
            ErrorBarSettingsEXP.IsExpanded = false;
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
                    LabelingEXP.IsExpanded = true;
                    break;
                case OxyPlotPropertiesControl.PropertyEXP.Series_Display:
                case OxyPlotPropertiesControl.PropertyEXP.Series_Markers:
                    MarkersEXP.IsExpanded = true;
                    break;
                case OxyPlotPropertiesControl.PropertyEXP.Series_ErrorBarSettings:
                    ErrorBarSettingsEXP.IsExpanded = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Converts scatter series marker fill color to/from a SolidColorBrush, handling automatic colors.
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
    public class ScatterSeriesMarkerFillConverter : IMultiValueConverter
    {
        private ScatterSeries? _series;

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

            // Get Series directly (core type)
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as ScatterSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualMarkerFillColor;
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

            if (OxyColor.ColorDifference(oxyCol, _series.ActualMarkerFillColor) == 0)
            {
                var actualColor = _series.ActualMarkerFillColor;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }

    /// <summary>
    /// Converts scatter series marker stroke color to/from a SolidColorBrush, handling automatic colors.
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
    public class ScatterSeriesMarkerStrokeConverter : IMultiValueConverter
    {
        private ScatterSeries? _series;

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

            // Get Series directly (core type)
            if (values[1] == null) return new SolidColorBrush(c);
            _series = values[1] as ScatterSeries;
            if (_series == null) return new SolidColorBrush(c);

            // Convert
            if (oxyCol.IsAutomatic())
            {
                var actualColor = _series.ActualMarkerFillColor;
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

            if (OxyColor.ColorDifference(oxyCol, _series.ActualMarkerFillColor) == 0)
            {
                var actualColor = _series.ActualMarkerFillColor;
                return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
            }

            return new object[] { c, _series };
        }
    }
}
