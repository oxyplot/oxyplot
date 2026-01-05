using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using OxyPlot.Wpf;

namespace OxyPlotControls
{
    /// <summary>
    /// A dialog window for saving OxyPlot charts as image files.
    /// Supports PNG, PDF, and SVG formats with customizable dimensions.
    /// </summary>
    public partial class SavePlotImageDialog : Window
    {
        private Plot _plot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePlotImageDialog"/> class.
        /// </summary>
        /// <param name="thePlot">The OxyPlot Plot control to save as an image.</param>
        public SavePlotImageDialog(Plot thePlot)
        {
            InitializeComponent();

            _plot = thePlot;

            ContentRendered += SavePlotImageDialog_ContentRendered;
            Closing += SavePlotImageDialog_Closing;
        }

        private void SavePlotImageDialog_ContentRendered(object sender, EventArgs e)
        {
            WidthTextBox.ToolTip = "Current Plot Width is " + ((int)_plot.ActualWidth).ToString() + " px";
            HeightTextBox.ToolTip = "Current Plot Height is " + ((int)_plot.ActualHeight).ToString() + " px";

            var size = ((ComboBoxItem)ImageSizeComboBox.SelectedItem).Content.ToString();
            var sizes = size.Split(' ');
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
            if (_plot == null) return;

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
                var sizes = size.Split(' ');
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
                        _plot.SaveBitmap(saveFile, imageWidth, imageHeight, _plot.ActualModel.Background);
                        break;
                    case ".svg":
                        using (var fs = new FileStream(saveFile, FileMode.Create))
                        {
                            OxyPlot.SvgExporter.Export(_plot.ActualModel, fs, imageWidth, imageHeight, true);
                        }
                        break;
                    case ".pdf":
                        using (var fs = new FileStream(saveFile, FileMode.Create))
                        {
                            OxyPlot.PdfExporter.Export(_plot.ActualModel, fs, imageWidth, imageHeight);
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

        private void SavePlotImageDialog_Closing(object sender, CancelEventArgs e)
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

                WidthTextBox.Text = ((int)_plot.ActualWidth).ToString();
                HeightTextBox.Text = ((int)_plot.ActualHeight).ToString();
            }
            else
            {
                WidthTextBox.IsEnabled = false;
                HeightTextBox.IsEnabled = false;

                var size = ((ComboBoxItem)ImageSizeComboBox.SelectedItem).Content.ToString();
                var sizes = size.Split(' ');
                WidthTextBox.Text = sizes[0];
                HeightTextBox.Text = sizes[2];
            }
        }
    }
}
