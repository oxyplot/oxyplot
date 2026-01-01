using System;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlotControls.Controls.Base;
using OxyPlotControls.Factories;

namespace OxyPlotControls.Controls.Selectors;

/// <summary>
/// Control for selecting and managing axes in a PlotModel.
/// Provides axis selection, add/delete functionality, and property editing.
/// </summary>
public partial class AxesControl : UserControl
{
    public AxesControl()
    {
        InitializeComponent();
    }

    #region Dependency Properties

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            nameof(Model),
            typeof(PlotModel),
            typeof(AxesControl),
            new PropertyMetadata(null, OnModelChanged));

    public PlotModel? Model
    {
        get => (PlotModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AxesControl control)
        {
            control.OnModelChanged(e.NewValue as PlotModel);
        }
    }

    #endregion

    private void OnModelChanged(PlotModel? model)
    {
        if (model == null)
        {
            AxesComboBox.ItemsSource = null;
            AxisPropertyEditor.Content = null;
            return;
        }

        AxesComboBox.ItemsSource = model.Axes;

        if (model.Axes.Count > 0)
        {
            AxesComboBox.SelectedIndex = 0;
        }
    }

    private void AxesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (AxesComboBox.SelectedItem is Axis axis)
        {
            var editor = AxisControlFactory.CreateControl(axis);

            if (editor is AxisControlBase axisControl)
            {
                axisControl.Axis = axis;
                axisControl.Model = Model;
            }

            AxisPropertyEditor.Content = editor;
        }
        else
        {
            AxisPropertyEditor.Content = null;
        }
    }

    #region Axis Management

    private void DeleteAxis_Click(object sender, RoutedEventArgs e)
    {
        if (AxesComboBox.SelectedItem is Axis axis && Model != null)
        {
            var result = MessageBox.Show(
                $"Delete axis '{axis.Title}' ({axis.Position})?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Model.Axes.Remove(axis);
                Model.InvalidatePlot(false);
            }
        }
    }

    private void AddAxis_Click(object sender, RoutedEventArgs e)
    {
        // Show context menu
        if (sender is Button button && button.ContextMenu != null)
        {
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.IsOpen = true;
        }
    }

    private void AddLinearAxis_Click(object sender, RoutedEventArgs e) => AddNewAxis(new LinearAxis { Title = "Linear Axis", Position = AxisPosition.Bottom });
    private void AddLogarithmicAxis_Click(object sender, RoutedEventArgs e) => AddNewAxis(new LogarithmicAxis { Title = "Log Axis", Position = AxisPosition.Bottom });
    private void AddDateTimeAxis_Click(object sender, RoutedEventArgs e) => AddNewAxis(new DateTimeAxis { Title = "DateTime Axis", Position = AxisPosition.Bottom });
    private void AddCategoryAxis_Click(object sender, RoutedEventArgs e) => AddNewAxis(new CategoryAxis { Title = "Category Axis", Position = AxisPosition.Bottom });
    private void AddNormalProbabilityAxis_Click(object sender, RoutedEventArgs e) => AddNewAxis(new NormalProbabilityAxis { Title = "Normal Probability", Position = AxisPosition.Bottom });
    private void AddGumbelProbabilityAxis_Click(object sender, RoutedEventArgs e) => AddNewAxis(new GumbelProbabilityAxis { Title = "Gumbel Probability", Position = AxisPosition.Bottom });

    private void AddNewAxis(Axis axis)
    {
        if (Model != null)
        {
            Model.Axes.Add(axis);
            Model.InvalidatePlot(false);
            AxesComboBox.SelectedItem = axis;
        }
    }

    #endregion
}
