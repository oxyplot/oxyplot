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
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;

namespace OxyPlotControls
{
    /// <summary>
    /// A dialog window for saving OxyPlot charts as image files.
    /// Supports PNG, PDF, and SVG formats with customizable dimensions.
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
    public partial class SavePlotImageDialog : Window
    {
        private PlotView? _plotView;
        private double _actualWidth;
        private double _actualHeight;
        private PlotModel? _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePlotImageDialog"/> class using a PlotView.
        /// </summary>
        /// <param name="plotView">The PlotView control to save as an image.</param>
        public SavePlotImageDialog(PlotView plotView)
        {
            InitializeComponent();

            _plotView = plotView;
            _actualWidth = plotView.ActualWidth;
            _actualHeight = plotView.ActualHeight;
            _model = plotView.Model;

            ContentRendered += SavePlotImageDialog_ContentRendered;
            Closing += SavePlotImageDialog_Closing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePlotImageDialog"/> class using a PlotModel directly.
        /// </summary>
        /// <param name="model">The PlotModel to save as an image.</param>
        /// <param name="width">The current display width of the plot.</param>
        /// <param name="height">The current display height of the plot.</param>
        public SavePlotImageDialog(PlotModel model, double width = 800, double height = 600)
        {
            InitializeComponent();

            _plotView = null;
            _actualWidth = width;
            _actualHeight = height;
            _model = model;

            ContentRendered += SavePlotImageDialog_ContentRendered;
            Closing += SavePlotImageDialog_Closing;
        }

        private void SavePlotImageDialog_ContentRendered(object? sender, EventArgs e)
        {
            WidthTextBox.ToolTip = "Current Plot Width is " + ((int)_actualWidth).ToString() + " px";
            HeightTextBox.ToolTip = "Current Plot Height is " + ((int)_actualHeight).ToString() + " px";

            var size = ((ComboBoxItem)ImageSizeComboBox.SelectedItem).Content.ToString();
            var sizes = size?.Split(' ') ?? new[] { "800", "x", "600" };
            WidthTextBox.Text = sizes[0];
            HeightTextBox.Text = sizes[2];
        }

        private string FileSaveDialog(string filters)
        {
            var saveFileBrowser = new SaveFileDialog { Filter = filters };
            if (saveFileBrowser.ShowDialog() == true)
            {
                return saveFileBrowser.FileName;
            }
            return "";
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0])) e.Handled = true; // numeric only
            if (e.Text == ((char)8).ToString()) e.Handled = false; // allow Backspace
            if (e.Text == " ") e.Handled = true; // don't allow spaces
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (_model == null) return;

            string saveFile = FileSaveDialog("PNG File(*.png) |*.png|PDF File(*.pdf) |*.pdf|SVG File(*.svg) |*.svg");

            // Verify input parameters
            if (saveFile == "")
            {
                MessageBox.Show("File path that was specified for the image file is invalid.");
                return;
            }

            string extension = Path.GetExtension(saveFile.ToLower());
            if (extension != ".png" && extension != ".svg" && extension != ".pdf")
            {
                MessageBox.Show("File path that was specified for the export file is not a valid file type.");
                return;
            }

            int imageWidth, imageHeight;
            if (ImageSizeComboBox.SelectedIndex == ImageSizeComboBox.Items.Count - 1)
            {
                if (!int.TryParse(WidthTextBox.Text, out imageWidth))
                {
                    MessageBox.Show("Image width is not a valid number.");
                    return;
                }
                else if (imageWidth <= 0)
                {
                    MessageBox.Show("Image width must be greater than zero.");
                    return;
                }

                if (!int.TryParse(HeightTextBox.Text, out imageHeight))
                {
                    MessageBox.Show("Image height is not a valid number.");
                    return;
                }
                else if (imageHeight <= 0)
                {
                    MessageBox.Show("Image height must be greater than zero.");
                    return;
                }
            }
            else
            {
                var size = ((ComboBoxItem)ImageSizeComboBox.SelectedItem).Content.ToString();
                var sizes = size?.Split(' ') ?? new[] { "800", "x", "600" };
                imageWidth = int.Parse(sizes[0]);
                imageHeight = int.Parse(sizes[2]);
            }

            if (File.Exists(saveFile))
            {
                try
                {
                    File.Delete(saveFile);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to delete image file " + Path.GetFileName(saveFile) + ". It may be in use by another program.");
                    return;
                }
            }

            try
            {
                switch (extension)
                {
                    case ".png":
                        // Note: PngExporter in OxyPlot.Wpf uses the model's background automatically
                        var pngExporter = new PngExporter { Width = imageWidth, Height = imageHeight };
                        pngExporter.ExportToFile(_model, saveFile);
                        break;
                    case ".svg":
                        using (var fs = new FileStream(saveFile, FileMode.Create))
                        {
                            OxyPlot.SvgExporter.Export(_model, fs, imageWidth, imageHeight, true);
                        }
                        break;
                    case ".pdf":
                        using (var fs = new FileStream(saveFile, FileMode.Create))
                        {
                            OxyPlot.PdfExporter.Export(_model, fs, imageWidth, imageHeight);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred attempting to save the image file: " + ex.Message);
            }

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SavePlotImageDialog_Closing(object? sender, CancelEventArgs e)
        {
            if (Owner != null) Owner.Activate();
        }

        private void ImageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WidthTextBox == null || HeightTextBox == null) return;
            if (ImageSizeComboBox.SelectedIndex < 0) return;

            if (ImageSizeComboBox.SelectedIndex == ImageSizeComboBox.Items.Count - 1)
            {
                WidthTextBox.IsEnabled = true;
                HeightTextBox.IsEnabled = true;

                WidthTextBox.Text = ((int)_actualWidth).ToString();
                HeightTextBox.Text = ((int)_actualHeight).ToString();
            }
            else
            {
                WidthTextBox.IsEnabled = false;
                HeightTextBox.IsEnabled = false;

                var size = ((ComboBoxItem)ImageSizeComboBox.SelectedItem).Content.ToString();
                var sizes = size?.Split(' ') ?? new[] { "800", "x", "600" };
                WidthTextBox.Text = sizes[0];
                HeightTextBox.Text = sizes[2];
            }
        }
    }
}
