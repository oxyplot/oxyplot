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

using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Axes;

namespace OxyPlotControls
{
    /// <summary>
    /// A user control that provides a selector and editor for OxyPlot axes.
    /// Allows users to select an axis from a dropdown and edit its properties.
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
    public partial class AxesControl : UserControl
    {
        /// <summary>
        /// The XML tag name used for serializing axes properties.
        /// </summary>
        public static readonly string AxesPropertiesTag = "Axes";

        /// <summary>
        /// Identifies the <see cref="PlotModel"/> dependency property.
        /// </summary>
        public static DependencyProperty PlotModelProperty = DependencyProperty.Register(
            nameof(PlotModel), typeof(PlotModel), typeof(AxesControl),
            new PropertyMetadata(null, InitializePlotModel));

        /// <summary>
        /// Gets or sets the PlotModel that contains the axes.
        /// </summary>
        public PlotModel PlotModel
        {
            get { return (PlotModel)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedAxis"/> dependency property.
        /// </summary>
        public static DependencyProperty SelectedAxisProperty = DependencyProperty.Register(
            nameof(SelectedAxis), typeof(Axis), typeof(AxesControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the currently selected axis.
        /// </summary>
        public Axis SelectedAxis
        {
            get { return (Axis)GetValue(SelectedAxisProperty); }
            set { SetValue(SelectedAxisProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TitleMinWidth"/> dependency property.
        /// </summary>
        public static DependencyProperty TitleMinWidthProp = DependencyProperty.Register(
            nameof(TitleMinWidth), typeof(int), typeof(AxesControl),
            new UIPropertyMetadata(110));

        /// <summary>
        /// Gets or sets the minimum width for title elements.
        /// </summary>
        public int TitleMinWidth
        {
            get { return (int)GetValue(TitleMinWidthProp); }
            set { SetValue(TitleMinWidthProp, value); }
        }

        /// <summary>
        /// Identifies the <see cref="LeaderLinesVisibility"/> dependency property.
        /// </summary>
        public static DependencyProperty LeaderLinesVisibilityProp = DependencyProperty.Register(
            nameof(LeaderLinesVisibility), typeof(Visibility), typeof(AxesControl),
            new UIPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Gets or sets the visibility of leader lines.
        /// </summary>
        public Visibility LeaderLinesVisibility
        {
            get { return (Visibility)GetValue(LeaderLinesVisibilityProp); }
            set { SetValue(LeaderLinesVisibilityProp, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabItemStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty TabItemStyleProperty = DependencyProperty.Register(
            nameof(TabItemStyle), typeof(Style), typeof(AxesControl));

        /// <summary>
        /// Gets or sets the style for tab items.
        /// </summary>
        public Style TabItemStyle
        {
            get { return (Style)GetValue(TabItemStyleProperty); }
            set { SetValue(TabItemStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle), typeof(Style), typeof(AxesControl));

        /// <summary>
        /// Gets or sets the style for expander controls.
        /// </summary>
        public Style ExpanderStyle
        {
            get { return (Style)GetValue(ExpanderStyleProperty); }
            set { SetValue(ExpanderStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ComboBoxStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty ComboBoxStyleProperty = DependencyProperty.Register(
            nameof(ComboBoxStyle), typeof(Style), typeof(AxesControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the style for combo box controls.
        /// </summary>
        public Style ComboBoxStyle
        {
            get { return (Style)GetValue(ComboBoxStyleProperty); }
            set { SetValue(ComboBoxStyleProperty, value); }
        }

        private void SetDefaultComboboxStyle()
        {
            ComboBoxStyle = (Style)FindResource("CleanComboBoxStyle");
        }

        private static void InitializePlotModel(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (d.GetType() != typeof(AxesControl)) return;
            var thisControl = (AxesControl)d;

            thisControl.AxesPropertyControlComboBox.ItemsSource = null;
            if (e.NewValue == null) return;
            if (e.NewValue is not PlotModel newPlotModel) return;

            if (thisControl.ComboBoxStyle == null) thisControl.SetDefaultComboboxStyle();
            thisControl.AxesPropertyControlComboBox.ItemsSource = newPlotModel.Axes;

            thisControl.AxesPropertyControlComboBox.ApplyTemplate();
            var t = thisControl.AxesPropertyControlComboBox.FindResource("ComboBoxTemplate");

            if (newPlotModel.Axes.Count > 0) thisControl.AxesPropertyControlComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxesControl"/> class.
        /// </summary>
        public AxesControl()
        {
            InitializeComponent();
        }

        private void AxesPropertyControlComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Early exit if nothing is selected.
            if (AxesPropertyControlComboBox.SelectedItem == null) return;

            var axisToSelect = AxesPropertyControlComboBox.SelectedItem as Axis;
            if (axisToSelect == null) return;
            AxisPropertiesControl.Axis = axisToSelect;
            AxisPropertiesControl.PlotModel = PlotModel;
        }

        private void DeleteAxisButton_Click(object sender, RoutedEventArgs e)
        {
            if (AxesPropertyControlComboBox.SelectedItem == null) return;
            if (PlotModel == null) return;
            if (sender == null) return;
            if (sender.GetType() != typeof(Button)) return;
            var btn = (Button)sender;
            if (btn.DataContext == null) return;

            var axisToDelete = btn.DataContext as Axis;
            if (axisToDelete == null) return;

            int index = PlotModel.Axes.IndexOf(axisToDelete);
            if (index == AxesPropertyControlComboBox.SelectedIndex)
            {
                if (index > 0) index -= 1;
                if (PlotModel.Axes.Count == 1) index = -1;
            }
            PlotModel.Axes.Remove(axisToDelete);
            AxesPropertyControlComboBox.SelectedIndex = index;

            AxisPropertiesControl.CloseExpanders();
        }

        /// <summary>
        /// Serializes all axes properties to an XML element for persistence.
        /// </summary>
        /// <param name="plotModel">The PlotModel containing the axes to serialize.</param>
        /// <returns>An XElement containing all serialized axes properties.</returns>
        public static XElement AxesPropertiesToXElement(PlotModel plotModel)
        {
            var axesProperties = new XElement(AxesPropertiesTag);
            foreach (var axis in plotModel.Axes)
            {
                axesProperties.Add(AxisControl.AxisPropertiesToXElement(axis));
            }

            return axesProperties;
        }

        /// <summary>
        /// Deserializes axes properties from an XML element and applies them to the plot model.
        /// </summary>
        /// <param name="plotModel">The PlotModel to apply settings to.</param>
        /// <param name="element">The XElement containing serialized axes properties.</param>
        /// <param name="version">The serialization format version (1 for legacy, 2 for modern).</param>
        public static void XElementToAxesProperties(PlotModel plotModel, XElement element, int version = 2)
        {
            // Early Exit
            if (element.Name != AxesPropertiesTag) return;

            // Set up the axes
            plotModel.Axes.Clear();
            Axis? tempAxis;
            foreach (var el in element.Elements(AxisControl.AxisPropertiesTag))
            {
                tempAxis = AxisControl.XElementToAxisProperties(el);
                if (tempAxis == null) continue;
                plotModel.Axes.Add(tempAxis);
            }
        }

        private void AxisPropertiesControl_AxisTypeChanged(Axis oldAxis, Axis newAxis)
        {
            AxesPropertyControlComboBox.ItemsSource = PlotModel?.Axes;
            SelectedAxis = newAxis;
            AxesPropertyControlComboBox.SelectedItem = newAxis;
        }
    }
}
