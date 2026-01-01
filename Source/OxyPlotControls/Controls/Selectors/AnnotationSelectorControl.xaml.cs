using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlotControls.Controls.Base;
using OxyPlotControls.Factories;

namespace OxyPlotControls.Controls.Selectors;

/// <summary>
/// Control for selecting and managing annotations in a PlotModel.
/// Provides annotation selection, add/delete functionality, and property editing.
/// </summary>
public partial class AnnotationSelectorControl : UserControl
{
    public AnnotationSelectorControl()
    {
        InitializeComponent();
        Resources.Add("TypeNameConverter", new TypeNameConverter());
    }

    #region Dependency Properties

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            nameof(Model),
            typeof(PlotModel),
            typeof(AnnotationSelectorControl),
            new PropertyMetadata(null, OnModelChanged));

    public PlotModel? Model
    {
        get => (PlotModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AnnotationSelectorControl control)
        {
            control.OnModelChanged(e.NewValue as PlotModel);
        }
    }

    #endregion

    private void OnModelChanged(PlotModel? model)
    {
        if (model == null)
        {
            AnnotationComboBox.ItemsSource = null;
            AnnotationPropertyEditor.Content = null;
            return;
        }

        AnnotationComboBox.ItemsSource = model.Annotations;

        if (model.Annotations.Count > 0)
        {
            AnnotationComboBox.SelectedIndex = 0;
        }
    }

    private void AnnotationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (AnnotationComboBox.SelectedItem is Annotation annotation)
        {
            var editor = AnnotationControlFactory.CreateControl(annotation);

            if (editor is AnnotationControlBase annotationControl)
            {
                annotationControl.Annotation = annotation;
            }

            AnnotationPropertyEditor.Content = editor;
        }
        else
        {
            AnnotationPropertyEditor.Content = null;
        }
    }

    #region Annotation Management

    private void DeleteAnnotation_Click(object sender, RoutedEventArgs e)
    {
        if (AnnotationComboBox.SelectedItem is Annotation annotation && Model != null)
        {
            var text = annotation is TextualAnnotation textual ? textual.Text : annotation.GetType().Name;

            var result = MessageBox.Show(
                $"Delete annotation '{text}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Model.Annotations.Remove(annotation);
                Model.InvalidatePlot(false);
            }
        }
    }

    private void AddAnnotation_Click(object sender, RoutedEventArgs e)
    {
        // Show context menu
        if (sender is Button button && button.ContextMenu != null)
        {
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.IsOpen = true;
        }
    }

    private void AddArrowAnnotation_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new ArrowAnnotation { Text = "Arrow", StartPoint = new DataPoint(0, 0), EndPoint = new DataPoint(1, 1) });
    private void AddTextAnnotation_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new TextAnnotation { Text = "Text Annotation", TextPosition = new DataPoint(0, 0) });
    private void AddLineAnnotationV_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 0, Text = "Vertical Line" });
    private void AddLineAnnotationH_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 0, Text = "Horizontal Line" });
    private void AddRectangleAnnotation_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new RectangleAnnotation { MinimumX = 0, MaximumX = 1, MinimumY = 0, MaximumY = 1, Text = "Rectangle" });
    private void AddEllipseAnnotation_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new EllipseAnnotation { X = 0.5, Y = 0.5, Width = 1, Height = 1, Text = "Ellipse" });
    private void AddPointAnnotation_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new PointAnnotation { X = 0, Y = 0, Text = "Point" });
    private void AddPolygonAnnotation_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new PolygonAnnotation { Text = "Polygon", Points = { new DataPoint(0, 0), new DataPoint(1, 0), new DataPoint(0.5, 1) } });
    private void AddPolylineAnnotation_Click(object sender, RoutedEventArgs e) => AddNewAnnotation(new PolylineAnnotation { Text = "Polyline", Points = { new DataPoint(0, 0), new DataPoint(1, 1) } });

    private void AddNewAnnotation(Annotation annotation)
    {
        if (Model != null)
        {
            Model.Annotations.Add(annotation);
            Model.InvalidatePlot(false);
            AnnotationComboBox.SelectedItem = annotation;
        }
    }

    #endregion

    /// <summary>
    /// Converter to display annotation type name.
    /// </summary>
    private class TypeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.GetType().Name.Replace("Annotation", "") ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
