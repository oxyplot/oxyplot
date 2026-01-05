using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Annotations;

namespace OxyPlotControls
{
    /// <summary>
    /// A user control that provides a selector and editor for OxyPlot annotations.
    /// Allows users to select an annotation from a dropdown, add new annotations, and edit their properties.
    /// </summary>
    public partial class AnnotationSelectorControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationSelectorControl"/> class.
        /// </summary>
        public AnnotationSelectorControl()
        {
            InitializeComponent();
            SetDefaultComboboxStyle();
        }

        /// <summary>
        /// The XML tag name used for serializing annotations properties.
        /// </summary>
        public static readonly string AnnotationsPropertiesTag = "Annotations";

        /// <summary>
        /// Identifies the <see cref="PlotModel"/> dependency property.
        /// </summary>
        public static DependencyProperty PlotModelProperty = DependencyProperty.Register(
            nameof(PlotModel), typeof(PlotModel), typeof(AnnotationSelectorControl),
            new PropertyMetadata(null, InitializePlotModel));

        /// <summary>
        /// Gets or sets the PlotModel that contains the annotations.
        /// </summary>
        public PlotModel PlotModel
        {
            get { return (PlotModel)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedAnnotation"/> dependency property.
        /// </summary>
        public static DependencyProperty SelectedAnnotationProperty = DependencyProperty.Register(
            nameof(SelectedAnnotation), typeof(Annotation), typeof(AnnotationSelectorControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the currently selected annotation.
        /// </summary>
        public Annotation SelectedAnnotation
        {
            get { return (Annotation)GetValue(SelectedAnnotationProperty); }
            set { SetValue(SelectedAnnotationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle), typeof(Style), typeof(AnnotationSelectorControl));

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
            nameof(ComboBoxStyle), typeof(Style), typeof(AnnotationSelectorControl),
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
            if (d.GetType() != typeof(AnnotationSelectorControl)) return;
            var thisControl = (AnnotationSelectorControl)d;

            thisControl.AnnotationPropertyControlComboBox.ItemsSource = null;
            if (e.NewValue == null) return;
            if (e.NewValue is not PlotModel newPlotModel) return;

            thisControl.AddHandlers();
            thisControl.AnnotationPropertyControlComboBox.ItemsSource = newPlotModel.Annotations;

            thisControl.AnnotationPropertyControlComboBox.ApplyTemplate();
            var cntrl = FindElementByName<ItemsControl>(thisControl.AnnotationPropertyControlComboBox, "SpecialOptions");

            if (cntrl != null && cntrl.Items.Count == 0)
            {
                cntrl.Items.Add(new Separator());

                // Add arrow annotation
                var addArrow = new ComboBoxItem { Content = "Add Arrow Annotation", FontStyle = FontStyles.Italic };
                addArrow.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newArrow = new ArrowAnnotation { Text = "Arrow Annotation" };
                    thisControl.PlotModel.Annotations.Add(newArrow);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();

                    DataPoint plotCenter;
                    OxyRect plotArea = thisControl.PlotModel.PlotArea;
                    plotCenter = newArrow.InverseTransform(plotArea.Center);
                    double xShift = plotArea.Center.X + Math.Abs(plotArea.Right - plotArea.Left) * 0.1;
                    var centerXShifted = newArrow.InverseTransform(new ScreenPoint(xShift, plotArea.Center.Y));

                    newArrow.StartPoint = centerXShifted;
                    newArrow.EndPoint = plotCenter;
                };
                cntrl.Items.Add(addArrow);

                // Add text annotation
                var addText = new ComboBoxItem { Content = "Add Text Annotation", FontStyle = FontStyles.Italic };
                addText.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newText = new TextAnnotation { Text = "Text Annotation", StrokeThickness = 0 };
                    thisControl.PlotModel.Annotations.Add(newText);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();
                    newText.TextPosition = newText.InverseTransform(thisControl.PlotModel.PlotArea.Center);
                };
                cntrl.Items.Add(addText);

                // Add line annotation
                var addLine = new ComboBoxItem { Content = "Add Line Annotation", FontStyle = FontStyles.Italic };
                addLine.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newLine = new LineAnnotation { Text = "Line Annotation" };
                    thisControl.PlotModel.Annotations.Add(newLine);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();

                    DataPoint plotLL, plotUR, plotCenter;
                    OxyRect plotArea = thisControl.PlotModel.PlotArea;
                    plotLL = newLine.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                    plotUR = newLine.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                    plotCenter = newLine.InverseTransform(plotArea.Center);

                    newLine.X = plotCenter.X;
                    newLine.Y = plotCenter.Y;
                    newLine.Type = LineAnnotationType.LinearEquation;
                    newLine.Intercept = plotCenter.Y;
                    newLine.Slope = (plotUR.Y - plotLL.Y) / (plotUR.X - plotLL.X);
                };
                cntrl.Items.Add(addLine);

                // Add rectangle annotation
                var addRect = new ComboBoxItem { Content = "Add Rectangle Annotation", FontStyle = FontStyles.Italic };
                addRect.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newRect = new RectangleAnnotation { Text = "Rectangle Annotation" };
                    thisControl.PlotModel.Annotations.Add(newRect);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();

                    DataPoint plotLL, plotUR, plotCenter;
                    OxyRect plotArea = thisControl.PlotModel.PlotArea;
                    plotLL = newRect.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                    plotUR = newRect.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                    plotCenter = newRect.InverseTransform(thisControl.PlotModel.PlotArea.Center);
                    double centerXShift = Math.Abs((plotUR.X - plotLL.X) * 0.1);
                    double centerYShift = Math.Abs((plotUR.Y - plotLL.Y) * 0.1);

                    newRect.MinimumX = plotCenter.X - centerXShift;
                    newRect.MaximumX = plotCenter.X + centerXShift;
                    newRect.MinimumY = plotCenter.Y - centerYShift;
                    newRect.MaximumY = plotCenter.Y + centerYShift;
                };
                cntrl.Items.Add(addRect);

                // Add ellipse annotation
                var addEllipse = new ComboBoxItem { Content = "Add Ellipse Annotation", FontStyle = FontStyles.Italic };
                addEllipse.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newEllipse = new EllipseAnnotation { Text = "Ellipse Annotation" };
                    thisControl.PlotModel.Annotations.Add(newEllipse);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();

                    DataPoint plotLL, plotUR, plotCenter;
                    OxyRect plotArea = thisControl.PlotModel.PlotArea;
                    plotLL = newEllipse.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                    plotUR = newEllipse.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                    plotCenter = newEllipse.InverseTransform(thisControl.PlotModel.PlotArea.Center);
                    double centerXShift = Math.Abs((plotUR.X - plotLL.X) * 0.1);
                    double centerYShift = Math.Abs((plotUR.Y - plotLL.Y) * 0.1);

                    newEllipse.MinimumX = plotCenter.X - centerXShift;
                    newEllipse.MaximumX = plotCenter.X + centerXShift;
                    newEllipse.MinimumY = plotCenter.Y - centerYShift;
                    newEllipse.MaximumY = plotCenter.Y + centerYShift;
                };
                cntrl.Items.Add(addEllipse);

                // Add point annotation
                var addPoint = new ComboBoxItem { Content = "Add Point Annotation", FontStyle = FontStyles.Italic };
                addPoint.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newPoint = new PointAnnotation { Text = "Point Annotation", Size = 5 };
                    thisControl.PlotModel.Annotations.Add(newPoint);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();

                    var plotCenter = newPoint.InverseTransform(thisControl.PlotModel.PlotArea.Center);
                    newPoint.X = plotCenter.X;
                    newPoint.Y = plotCenter.Y;
                };
                cntrl.Items.Add(addPoint);

                // Add polygon annotation
                var addPolygon = new ComboBoxItem { Content = "Add Polygon Annotation", FontStyle = FontStyles.Italic };
                addPolygon.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newPolygon = new PolygonAnnotation { Text = "Polygon Annotation", Points = new System.Collections.Generic.List<DataPoint>() };
                    thisControl.PlotModel.Annotations.Add(newPolygon);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();

                    DataPoint plotLL, plotUR;
                    OxyRect plotArea = thisControl.PlotModel.PlotArea;
                    plotArea = plotArea.Inflate(plotArea.Width * -0.25, plotArea.Height * -0.25);
                    plotLL = newPolygon.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                    plotUR = newPolygon.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));

                    newPolygon.Points.Add(plotLL);
                    newPolygon.Points.Add(new DataPoint(plotLL.X, plotUR.Y));
                    newPolygon.Points.Add(plotUR);
                    newPolygon.Points.Add(new DataPoint(plotUR.X, plotLL.Y));
                    thisControl.PlotModel.InvalidatePlot(false);
                };
                cntrl.Items.Add(addPolygon);

                // Add polyline annotation
                var addPolyline = new ComboBoxItem { Content = "Add Polyline Annotation", FontStyle = FontStyles.Italic };
                addPolyline.PreviewMouseLeftButtonUp += (s, args) =>
                {
                    var newPolyline = new PolylineAnnotation { Text = "Polyline Annotation", Points = new System.Collections.Generic.List<DataPoint>() };
                    thisControl.PlotModel.Annotations.Add(newPolyline);
                    thisControl.PlotModel.InvalidatePlot(false);
                    thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.PlotModel.Annotations[thisControl.PlotModel.Annotations.Count - 1];
                    thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = false;
                    thisControl.AnnotationPropertiesControl.Focus();

                    DataPoint plotLL, plotUR, plotCenter;
                    OxyRect plotArea = thisControl.PlotModel.PlotArea;
                    plotArea = plotArea.Inflate(plotArea.Width * -0.25, plotArea.Height * -0.25);
                    plotLL = newPolyline.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                    plotUR = newPolyline.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                    plotCenter = newPolyline.InverseTransform(thisControl.PlotModel.PlotArea.Center);

                    newPolyline.Points.Add(new DataPoint(plotCenter.X - plotLL.X, plotLL.Y));
                    newPolyline.Points.Add(new DataPoint(plotCenter.X + plotUR.X, plotUR.Y));
                    thisControl.PlotModel.InvalidatePlot(false);
                };
                cntrl.Items.Add(addPolyline);
            }
        }

        /// <summary>
        /// Add required handlers.
        /// </summary>
        private void AddHandlers()
        {
            PlotModel.Annotations.CollectionChanged += Annotation_CollectionChanged;
        }

        /// <summary>
        /// When a new annotation is added or removed externally, update the control.
        /// </summary>
        private void Annotation_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // New item was added.
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    AnnotationPropertyControlComboBox.SelectedItem = newItem;
                    AnnotationPropertiesControl.Annotation = newItem as TextualAnnotation;
                    break;
                }
            }

            // Item was removed.
            if (e.OldItems != null)
            {
                if (PlotModel != null && PlotModel.Annotations.Count == 0)
                {
                    AnnotationPropertiesControl.HideExpanders();
                }
                else
                {
                    AnnotationPropertyControlComboBox.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// On selection changed, make sure selection is an annotation.
        /// </summary>
        private void AnnotationPropertyControlComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Early exit if nothing is selected.
            if (AnnotationPropertyControlComboBox.SelectedItem == null) return;

            var annotationToSelect = AnnotationPropertyControlComboBox.SelectedItem as TextualAnnotation;
            if (annotationToSelect == null) return;
            AnnotationPropertiesControl.Annotation = annotationToSelect;
        }

        /// <summary>
        /// Delete the selected annotation.
        /// </summary>
        private void DeleteAnnotationButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnnotationPropertyControlComboBox.SelectedItem == null) return;
            if (PlotModel == null) return;
            if (sender == null) return;
            if (sender.GetType() != typeof(Button)) return;
            var btn = (Button)sender;
            if (btn.DataContext == null) return;

            var annotationToDelete = btn.DataContext as Annotation;
            if (annotationToDelete == null) return;

            // Delete Annotation
            PlotModel.Annotations.Remove(annotationToDelete);
            PlotModel.InvalidatePlot(false);

            if (PlotModel.Annotations.Count == 0) AnnotationPropertyControlComboBox.IsDropDownOpen = false;
        }

        /// <summary>
        /// Serializes all annotations properties to an XML element for persistence.
        /// </summary>
        /// <param name="plotModel">The PlotModel containing the annotations to serialize.</param>
        /// <returns>An XElement containing all serialized annotations properties.</returns>
        public static XElement AnnotationsPropertiesToXElement(PlotModel plotModel)
        {
            var annotationProperties = new XElement(AnnotationsPropertiesTag);
            foreach (var annotation in plotModel.Annotations)
            {
                var textualAnnotation = annotation as TextualAnnotation;
                if (textualAnnotation == null) continue;
                annotationProperties.Add(AnnotationControl.AnnotationPropertiesToXElement(textualAnnotation));
            }

            return annotationProperties;
        }

        /// <summary>
        /// Deserializes annotations properties from an XML element and applies them to the plot model.
        /// </summary>
        /// <param name="plotModel">The PlotModel to apply settings to.</param>
        /// <param name="element">The XElement containing serialized annotations properties.</param>
        public static void XElementToAnnotationsProperties(PlotModel plotModel, XElement element)
        {
            // Early Exit
            if (element.Name != AnnotationsPropertiesTag) return;

            // Set up the annotations
            plotModel.Annotations.Clear();
            Annotation? tempAnnotation;
            foreach (var el in element.Elements(AnnotationControl.AnnotationPropertiesTag))
            {
                tempAnnotation = AnnotationControl.XElementToAnnotationProperties(el);
                if (tempAnnotation == null) continue;
                plotModel.Annotations.Add(tempAnnotation);
            }
        }

        /// <summary>
        /// Finds a child element of the specified type and name in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="parent">The parent element to search from.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <returns>The found element, or null if not found.</returns>
        private static T FindElementByName<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            if (parent == null) return null;

            int childCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild && typedChild.Name == name)
                {
                    return typedChild;
                }

                var result = FindElementByName<T>(child, name);
                if (result != null) return result;
            }

            return null;
        }
    }
}
