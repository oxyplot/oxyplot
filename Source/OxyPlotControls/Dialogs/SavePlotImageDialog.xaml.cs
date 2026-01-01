using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;

namespace OxyPlotControls.Dialogs;

/// <summary>
/// Dialog for saving plot images with size presets and custom dimensions.
/// Supports PNG, PDF, and SVG export formats.
/// </summary>
public partial class SavePlotImageDialog : Window
{
    private readonly PlotView _plotView;

    /// <summary>
    /// Initializes a new instance of the <see cref="SavePlotImageDialog"/> class.
    /// </summary>
    /// <param name="plotView">The PlotView to export.</param>
    public SavePlotImageDialog(PlotView plotView)
    {
        InitializeComponent();
        _plotView = plotView ?? throw new ArgumentNullException(nameof(plotView));

        Loaded += SavePlotImageDialog_Loaded;
    }

    private void SavePlotImageDialog_Loaded(object sender, RoutedEventArgs e)
    {
        // Set tooltips showing current plot dimensions
        var currentWidth = (int)_plotView.ActualWidth;
        var currentHeight = (int)_plotView.ActualHeight;
        WidthTextBox.ToolTip = $"Current Plot Width is {currentWidth} px";
        HeightTextBox.ToolTip = $"Current Plot Height is {currentHeight} px";

        // Initialize with first preset
        UpdateDimensionsFromComboBox();
    }

    private void ImageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateDimensionsFromComboBox();
    }

    private void UpdateDimensionsFromComboBox()
    {
        if (ImageSizeComboBox == null || WidthTextBox == null || HeightTextBox == null) return;

        var isCustom = ImageSizeComboBox.SelectedIndex == ImageSizeComboBox.Items.Count - 1;

        WidthTextBox.IsEnabled = isCustom;
        HeightTextBox.IsEnabled = isCustom;

        if (!isCustom && ImageSizeComboBox.SelectedItem is ComboBoxItem item)
        {
            var sizeText = item.Content.ToString();
            if (sizeText != null)
            {
                var parts = sizeText.Split(' ');
                if (parts.Length >= 3)
                {
                    WidthTextBox.Text = parts[0];
                    HeightTextBox.Text = parts[2];
                }
            }
        }
        else if (isCustom)
        {
            // Set to current plot dimensions for custom
            WidthTextBox.Text = ((int)_plotView.ActualWidth).ToString();
            HeightTextBox.Text = ((int)_plotView.ActualHeight).ToString();
        }
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Allow only numeric input
        if (!char.IsDigit(e.Text, 0))
        {
            e.Handled = true;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Get dimensions
            if (!int.TryParse(WidthTextBox.Text, out var width) || width <= 0)
            {
                MessageBox.Show("Image width must be a valid positive number.",
                    "Invalid Width", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(HeightTextBox.Text, out var height) || height <= 0)
            {
                MessageBox.Show("Image height must be a valid positive number.",
                    "Invalid Height", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Show save file dialog
            var dialog = new SaveFileDialog
            {
                Filter = "PNG Image (*.png)|*.png|PDF Document (*.pdf)|*.pdf|SVG Vector (*.svg)|*.svg",
                DefaultExt = ".png",
                FileName = "Plot.png"
            };

            if (dialog.ShowDialog() != true)
                return;

            // Delete existing file if it exists
            if (File.Exists(dialog.FileName))
            {
                try
                {
                    File.Delete(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to delete existing file: {ex.Message}\nIt may be in use by another program.",
                        "File In Use", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Export based on file extension
            var extension = Path.GetExtension(dialog.FileName).ToLowerInvariant();
            var backgroundColor = _plotView.ActualModel?.Background ?? OxyColors.White;

            switch (extension)
            {
                case ".png":
                    PngExporter.Export(_plotView.ActualModel, dialog.FileName, width, height, backgroundColor);
                    break;

                case ".pdf":
                    PdfExporter.Export(_plotView.ActualModel, dialog.FileName, width, height);
                    break;

                case ".svg":
                    SvgExporter.Export(_plotView.ActualModel, dialog.FileName, width, height, true, backgroundColor);
                    break;

                default:
                    MessageBox.Show($"Unsupported file format: {extension}",
                        "Invalid Format", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            MessageBox.Show($"Plot saved successfully to:\n{Path.GetFileName(dialog.FileName)}",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving image: {ex.Message}",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
