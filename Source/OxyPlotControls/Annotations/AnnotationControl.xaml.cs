using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Annotations;
using static OxyPlotControls.OxyPlotSettingsSerializer;

namespace OxyPlotControls
{
    /// <summary>
    /// Control for editing annotation properties in an OxyPlot chart.
    /// Supports various annotation types including text, arrow, line, shape, and point annotations.
    /// </summary>
    public partial class AnnotationControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationControl"/> class.
        /// </summary>
        public AnnotationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// XML element tag used for serializing annotation properties.
        /// </summary>
        public static readonly string AnnotationPropertiesTag = "Annotation";

        /// <summary>
        /// Gets the available text line position options (0 to 1 in 0.1 increments).
        /// </summary>
        public static List<double> TextLinePositionOptions { get; } = new List<double> { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1 };

        /// <summary>
        /// Gets the available annotation layer options.
        /// </summary>
        public static List<AnnotationLayer> AnnotationLayerOptions { get; } =
            new List<AnnotationLayer>((AnnotationLayer[])Enum.GetValues(typeof(AnnotationLayer)));

        /// <summary>
        /// Gets the available annotation text orientation options.
        /// </summary>
        public static List<AnnotationTextOrientation> AnnotationTextOrientationOptions { get; } =
            new List<AnnotationTextOrientation>((AnnotationTextOrientation[])Enum.GetValues(typeof(AnnotationTextOrientation)));

        /// <summary>
        /// Gets the available function annotation type options.
        /// </summary>
        public static List<FunctionAnnotationType> AnnotationFunctionTypeOptions { get; } =
            new List<FunctionAnnotationType>((FunctionAnnotationType[])Enum.GetValues(typeof(FunctionAnnotationType)));

        /// <summary>
        /// Gets the available line annotation type options.
        /// </summary>
        public static List<LineAnnotationType> LineAnnotationTypeOptions { get; } =
            new List<LineAnnotationType>((LineAnnotationType[])Enum.GetValues(typeof(LineAnnotationType)));

        /// <summary>
        /// Gets the available marker type options for point annotations.
        /// </summary>
        public static List<MarkerType> MarkerTypeOptions { get; } =
            new List<MarkerType>((MarkerType[])Enum.GetValues(typeof(MarkerType)));

        /// <summary>
        /// Gets the available line join options.
        /// </summary>
        public static List<LineJoin> LineJoinOptions { get; } =
            new List<LineJoin>((LineJoin[])Enum.GetValues(typeof(LineJoin)));

        /// <summary>
        /// Gets the available line style options.
        /// </summary>
        public static List<System.Windows.Media.DoubleCollection> LineStyleOptions => GenericControls.LineStyleSelectorControl.LineStyleOptions;

        /// <summary>
        /// Identifies the Annotation dependency property.
        /// </summary>
        public static readonly DependencyProperty AnnotationProperty = DependencyProperty.Register(
            nameof(Annotation), typeof(TextualAnnotation), typeof(AnnotationControl),
            new PropertyMetadata(null, AnnotationChangedCallback));

        /// <summary>
        /// Gets or sets the annotation being edited by this control.
        /// </summary>
        public TextualAnnotation? Annotation
        {
            get => (TextualAnnotation?)GetValue(AnnotationProperty);
            set => SetValue(AnnotationProperty, value);
        }

        private static void AnnotationChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (d.GetType() != typeof(AnnotationControl)) return;
            var thisControl = (AnnotationControl)d;

            if (e.NewValue == null) return;

            // Show Expanders
            thisControl.TextEXP.Visibility = Visibility.Visible;
            thisControl.DisplayOptionsEXP.Visibility = Visibility.Visible;

            // Determine whether to show X-Y Controls
            if (e.NewValue.GetType() == typeof(LineAnnotation))
            {
                var newAnnotation = (LineAnnotation)e.NewValue;

                if (newAnnotation.Type == LineAnnotationType.Horizontal)
                {
                    thisControl.XValueControl.Visibility = Visibility.Collapsed;
                    thisControl.YValueControl.Visibility = Visibility.Visible;
                    thisControl.InterceptControl.Visibility = Visibility.Collapsed;
                    thisControl.SlopeControl.Visibility = Visibility.Collapsed;
                }
                else if (newAnnotation.Type == LineAnnotationType.Vertical)
                {
                    thisControl.XValueControl.Visibility = Visibility.Visible;
                    thisControl.YValueControl.Visibility = Visibility.Collapsed;
                    thisControl.InterceptControl.Visibility = Visibility.Collapsed;
                    thisControl.SlopeControl.Visibility = Visibility.Collapsed;
                }
                else if (newAnnotation.Type == LineAnnotationType.LinearEquation)
                {
                    thisControl.XValueControl.Visibility = Visibility.Collapsed;
                    thisControl.YValueControl.Visibility = Visibility.Collapsed;
                    thisControl.InterceptControl.Visibility = Visibility.Visible;
                    thisControl.SlopeControl.Visibility = Visibility.Visible;
                }
            }
            else if (e.NewValue.GetType() == typeof(PointAnnotation))
            {
                thisControl.XValueControl.Visibility = Visibility.Visible;
                thisControl.YValueControl.Visibility = Visibility.Visible;
                thisControl.InterceptControl.Visibility = Visibility.Collapsed;
                thisControl.SlopeControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide controls
                thisControl.XValueControl.Visibility = Visibility.Collapsed;
                thisControl.YValueControl.Visibility = Visibility.Collapsed;
                thisControl.InterceptControl.Visibility = Visibility.Collapsed;
                thisControl.SlopeControl.Visibility = Visibility.Collapsed;
            }

            // Determine whether to show the Text Angle Control
            if (e.NewValue.GetType() == typeof(LineAnnotation) ||
                e.NewValue.GetType() == typeof(PolylineAnnotation))
            {
                thisControl.TextAngleControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                thisControl.TextAngleControl.Visibility = Visibility.Visible;
            }

            // Force layout update to sync bindings after annotation change
            thisControl.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
            {
                thisControl.UpdateLayout();
            }));
        }

        /// <summary>
        /// Identifies the ExpanderStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle), typeof(Style), typeof(AnnotationControl));

        /// <summary>
        /// Gets or sets the style applied to expanders in this control.
        /// </summary>
        public Style ExpanderStyle
        {
            get => (Style)GetValue(ExpanderStyleProperty);
            set => SetValue(ExpanderStyleProperty, value);
        }

        /// <summary>
        /// Serializes annotation properties to an XElement for XML storage.
        /// </summary>
        /// <param name="annotation">The annotation to serialize.</param>
        /// <returns>An XElement containing the serialized annotation properties.</returns>
        public static XElement AnnotationPropertiesToXElement(TextualAnnotation annotation)
        {
            var annotationProperties = new XElement(AnnotationPropertiesTag);
            var annotationType = annotation.GetType();
            annotationProperties.SetAttributeValue("AnnotationType", annotationType.ToString());

            // Annotation Properties
            var generalProperties = new XElement("General");
            generalProperties.SetAttributeValue(nameof(annotation.Tag), annotation.Tag?.ToString() ?? "");
            generalProperties.SetAttributeValue(nameof(annotation.Layer), annotation.Layer.ToString());
            generalProperties.SetAttributeValue(nameof(annotation.XAxisKey), annotation.XAxisKey);
            generalProperties.SetAttributeValue(nameof(annotation.YAxisKey), annotation.YAxisKey);
            annotationProperties.Add(generalProperties);

            // Textual Properties
            var textualProperties = new XElement("Textual");
            textualProperties.SetAttributeValue(nameof(annotation.Text), annotation.Text);
            textualProperties.SetAttributeValue(nameof(annotation.TextColor), annotation.TextColor.ToByteString());
            textualProperties.SetAttributeValue(nameof(annotation.Font), annotation.Font ?? "");
            textualProperties.SetAttributeValue(nameof(annotation.FontSize), annotation.FontSize.ToString("G17", CultureInfo.InvariantCulture));
            textualProperties.SetAttributeValue(nameof(annotation.FontWeight), annotation.FontWeight.ToString("G17", CultureInfo.InvariantCulture));
            textualProperties.SetAttributeValue(nameof(annotation.TextPosition), annotation.TextPosition.ToPrettyText());
            textualProperties.SetAttributeValue(nameof(annotation.TextRotation), annotation.TextRotation.ToString("G17", CultureInfo.InvariantCulture));
            textualProperties.SetAttributeValue(nameof(annotation.TextHorizontalAlignment), annotation.TextHorizontalAlignment.ToString());
            textualProperties.SetAttributeValue(nameof(annotation.TextVerticalAlignment), annotation.TextVerticalAlignment.ToString());
            annotationProperties.Add(textualProperties);

            // Must be either arrow (concrete), text (concrete), shape (abstract), or path (abstract)
            if (annotationType == typeof(ArrowAnnotation))
            {
                var arrowAnnotation = (ArrowAnnotation)annotation;
                var arrowProperties = new XElement("Arrow");
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.Color), arrowAnnotation.Color.ToByteString());
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.ArrowDirection), arrowAnnotation.ArrowDirection.ToPrettyText());
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.StartPoint), arrowAnnotation.StartPoint.ToPrettyText());
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.EndPoint), arrowAnnotation.EndPoint.ToPrettyText());
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.HeadLength), arrowAnnotation.HeadLength.ToString("G17", CultureInfo.InvariantCulture));
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.HeadWidth), arrowAnnotation.HeadWidth.ToString("G17", CultureInfo.InvariantCulture));
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.Veeness), arrowAnnotation.Veeness.ToString("G17", CultureInfo.InvariantCulture));
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.LineJoin), arrowAnnotation.LineJoin.ToString());
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.LineStyle), arrowAnnotation.LineStyle.ToString());
                arrowProperties.SetAttributeValue(nameof(arrowAnnotation.StrokeThickness), arrowAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                textualProperties.Add(arrowProperties);
            }
            else if (annotationType == typeof(TextAnnotation))
            {
                var textAnnotation = (TextAnnotation)annotation;
                var textProperties = new XElement("Text");
                textProperties.SetAttributeValue(nameof(textAnnotation.Background), textAnnotation.Background.ToByteString());
                textProperties.SetAttributeValue(nameof(textAnnotation.Offset), textAnnotation.Offset.ToPrettyText());
                textProperties.SetAttributeValue(nameof(textAnnotation.Padding), $"{textAnnotation.Padding.Left},{textAnnotation.Padding.Top},{textAnnotation.Padding.Right},{textAnnotation.Padding.Bottom}");
                textProperties.SetAttributeValue(nameof(textAnnotation.Stroke), textAnnotation.Stroke.ToByteString());
                textProperties.SetAttributeValue(nameof(textAnnotation.StrokeThickness), textAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                textualProperties.Add(textProperties);
            }

            // Must be shape or path annotation
            var shapeAnnotation = annotation as ShapeAnnotation;
            if (shapeAnnotation != null)
            {
                var shapeProperties = new XElement("Shape");
                shapeProperties.SetAttributeValue(nameof(shapeAnnotation.Fill), shapeAnnotation.Fill.ToByteString());
                shapeProperties.SetAttributeValue(nameof(shapeAnnotation.Stroke), shapeAnnotation.Stroke.ToByteString());
                shapeProperties.SetAttributeValue(nameof(shapeAnnotation.StrokeThickness), shapeAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                textualProperties.Add(shapeProperties);

                // Must be Ellipse, Point, Rectangle, or Polygon Annotations (all concrete)
                if (annotationType == typeof(EllipseAnnotation))
                {
                    var ellipseAnnotation = (EllipseAnnotation)shapeAnnotation;
                    var ellipseProperties = new XElement("Ellipse");
                    ellipseProperties.SetAttributeValue(nameof(ellipseAnnotation.X), ellipseAnnotation.X.ToString("G17", CultureInfo.InvariantCulture));
                    ellipseProperties.SetAttributeValue(nameof(ellipseAnnotation.Y), ellipseAnnotation.Y.ToString("G17", CultureInfo.InvariantCulture));
                    ellipseProperties.SetAttributeValue(nameof(ellipseAnnotation.Width), ellipseAnnotation.Width.ToString("G17", CultureInfo.InvariantCulture));
                    ellipseProperties.SetAttributeValue(nameof(ellipseAnnotation.Height), ellipseAnnotation.Height.ToString("G17", CultureInfo.InvariantCulture));
                    shapeProperties.Add(ellipseProperties);
                }
                else if (annotationType == typeof(RectangleAnnotation))
                {
                    var rectangleAnnotation = (RectangleAnnotation)shapeAnnotation;
                    var rectangleProperties = new XElement("Rectangle");
                    rectangleProperties.SetAttributeValue(nameof(rectangleAnnotation.MinimumX), rectangleAnnotation.MinimumX.ToString("G17", CultureInfo.InvariantCulture));
                    rectangleProperties.SetAttributeValue(nameof(rectangleAnnotation.MaximumY), rectangleAnnotation.MaximumY.ToString("G17", CultureInfo.InvariantCulture));
                    rectangleProperties.SetAttributeValue(nameof(rectangleAnnotation.MaximumX), rectangleAnnotation.MaximumX.ToString("G17", CultureInfo.InvariantCulture));
                    rectangleProperties.SetAttributeValue(nameof(rectangleAnnotation.MinimumY), rectangleAnnotation.MinimumY.ToString("G17", CultureInfo.InvariantCulture));
                    shapeProperties.Add(rectangleProperties);
                }
                else if (annotationType == typeof(PointAnnotation))
                {
                    var pointAnnotation = (PointAnnotation)shapeAnnotation;
                    var pointProperties = new XElement("Point");
                    pointProperties.SetAttributeValue(nameof(pointAnnotation.X), pointAnnotation.X.ToString("G17", CultureInfo.InvariantCulture));
                    pointProperties.SetAttributeValue(nameof(pointAnnotation.Y), pointAnnotation.Y.ToString("G17", CultureInfo.InvariantCulture));
                    pointProperties.SetAttributeValue(nameof(pointAnnotation.Size), pointAnnotation.Size.ToString("G17", CultureInfo.InvariantCulture));
                    pointProperties.SetAttributeValue(nameof(pointAnnotation.TextMargin), pointAnnotation.TextMargin.ToString("G17", CultureInfo.InvariantCulture));
                    pointProperties.SetAttributeValue(nameof(pointAnnotation.Shape), pointAnnotation.Shape.ToString());
                    shapeProperties.Add(pointProperties);
                }
                else if (annotationType == typeof(PolygonAnnotation))
                {
                    var polygonAnnotation = (PolygonAnnotation)shapeAnnotation;
                    var polygonProperties = new XElement("Polygon");
                    polygonProperties.SetAttributeValue(nameof(polygonAnnotation.LineJoin), polygonAnnotation.LineJoin.ToString());
                    polygonProperties.SetAttributeValue(nameof(polygonAnnotation.LineStyle), polygonAnnotation.LineStyle.ToString());
                    polygonProperties.Add(polygonAnnotation.Points.ToXElement(nameof(polygonAnnotation.Points)));
                    shapeProperties.Add(polygonProperties);
                }
            }
            else
            {
                var pathAnnotation = annotation as PathAnnotation;
                if (pathAnnotation != null)
                {
                    var pathProperties = new XElement("Path");
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.Color), pathAnnotation.Color.ToByteString());
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.ClipByXAxis), pathAnnotation.ClipByXAxis.ToString());
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.ClipByYAxis), pathAnnotation.ClipByYAxis.ToString());
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.LineJoin), pathAnnotation.LineJoin.ToString());
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.LineStyle), pathAnnotation.LineStyle.ToString());
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.StrokeThickness), pathAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.TextMargin), pathAnnotation.TextMargin.ToString("G17", CultureInfo.InvariantCulture));
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.TextOrientation), pathAnnotation.TextOrientation.ToString());
                    pathProperties.SetAttributeValue(nameof(pathAnnotation.TextLinePosition), pathAnnotation.TextLinePosition.ToString("G17", CultureInfo.InvariantCulture));
                    textualProperties.Add(pathProperties);

                    // Must be Line, Polyline, or Function Annotations (all concrete)
                    if (annotationType == typeof(LineAnnotation))
                    {
                        var lineAnnotation = (LineAnnotation)pathAnnotation;
                        var lineProperties = new XElement("Line");
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.Type), lineAnnotation.Type.ToString());
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.X), lineAnnotation.X.ToString("G17", CultureInfo.InvariantCulture));
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.Y), lineAnnotation.Y.ToString("G17", CultureInfo.InvariantCulture));
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.Intercept), lineAnnotation.Intercept.ToString("G17", CultureInfo.InvariantCulture));
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.MinimumX), lineAnnotation.MinimumX.ToString("G17", CultureInfo.InvariantCulture));
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.MaximumY), lineAnnotation.MaximumY.ToString("G17", CultureInfo.InvariantCulture));
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.MaximumX), lineAnnotation.MaximumX.ToString("G17", CultureInfo.InvariantCulture));
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.MinimumY), lineAnnotation.MinimumY.ToString("G17", CultureInfo.InvariantCulture));
                        lineProperties.SetAttributeValue(nameof(lineAnnotation.Slope), lineAnnotation.Slope.ToString("G17", CultureInfo.InvariantCulture));
                        pathProperties.Add(lineProperties);
                    }
                    else if (annotationType == typeof(PolylineAnnotation))
                    {
                        var polylineAnnotation = (PolylineAnnotation)pathAnnotation;
                        var polylineProperties = new XElement("Polyline");
                        polylineProperties.SetAttributeValue(nameof(polylineAnnotation.MinimumSegmentLength), polylineAnnotation.MinimumSegmentLength.ToString("G17", CultureInfo.InvariantCulture));
                        polylineProperties.Add(polylineAnnotation.Points.ToXElement(nameof(polylineAnnotation.Points)));
                        pathProperties.Add(polylineProperties);
                    }
                }
            }

            return annotationProperties;
        }

        /// <summary>
        /// Deserializes annotation properties from an XElement.
        /// </summary>
        /// <param name="element">The XElement containing the annotation properties.</param>
        /// <returns>A new annotation with the deserialized properties.</returns>
        public static TextualAnnotation? XElementToAnnotationProperties(XElement element)
        {
            if (element.Name != AnnotationPropertiesTag) return null;

            TextualAnnotation annotation;
            string annotationTypeString = "";

            if (element.Attribute("AnnotationType") != null) annotationTypeString = element.Attribute("AnnotationType")!.Value;

            // Support both old Wpf types and new core types for backward compatibility
            if (annotationTypeString.Contains("ArrowAnnotation"))
                annotation = new ArrowAnnotation();
            else if (annotationTypeString.Contains("TextAnnotation"))
                annotation = new TextAnnotation();
            else if (annotationTypeString.Contains("EllipseAnnotation"))
                annotation = new EllipseAnnotation();
            else if (annotationTypeString.Contains("RectangleAnnotation"))
                annotation = new RectangleAnnotation();
            else if (annotationTypeString.Contains("PointAnnotation"))
                annotation = new PointAnnotation();
            else if (annotationTypeString.Contains("PolygonAnnotation"))
                annotation = new PolygonAnnotation();
            else if (annotationTypeString.Contains("LineAnnotation"))
                annotation = new LineAnnotation();
            else if (annotationTypeString.Contains("PolylineAnnotation"))
                annotation = new PolylineAnnotation();
            else if (annotationTypeString.Contains("FunctionAnnotation"))
                annotation = new FunctionAnnotation();
            else
                annotation = new TextAnnotation();

            // General Properties
            var generalElement = element.Element("General");
            if (generalElement != null)
            {
                if (GetStringAttribute(generalElement, "Tag", out var tag)) annotation.Tag = tag;
                // Backwards compatibility: Name -> Tag
                if (GetStringAttribute(generalElement, "Name", out var name)) annotation.Tag = name;
                if (GetEnumAttribute(generalElement, nameof(annotation.Layer), out AnnotationLayer layer)) annotation.Layer = layer;
                if (GetStringAttribute(generalElement, nameof(annotation.XAxisKey), out var xAxisKey)) annotation.XAxisKey = xAxisKey;
                if (GetStringAttribute(generalElement, nameof(annotation.YAxisKey), out var yAxisKey)) annotation.YAxisKey = yAxisKey;
            }

            // Textual Properties
            var textualElement = element.Element("Textual");
            if (textualElement != null)
            {
                if (GetStringAttribute(textualElement, nameof(annotation.Text), out var text)) annotation.Text = text;
                if (GetOxyColorAttribute(textualElement, nameof(annotation.TextColor), out var textColor)) annotation.TextColor = textColor;
                if (GetStringAttribute(textualElement, nameof(annotation.Font), out var font)) annotation.Font = font;
                // Backwards compatibility: FontFamily -> Font
                if (GetStringAttribute(textualElement, "FontFamily", out var fontFamily)) annotation.Font = fontFamily;
                if (GetDoubleAttribute(textualElement, nameof(annotation.FontSize), out var fontSize)) annotation.FontSize = fontSize;
                if (GetDoubleAttribute(textualElement, nameof(annotation.FontWeight), out var fontWeight)) annotation.FontWeight = fontWeight;
                // Backwards compatibility: WPF FontWeight to double
                if (GetFontWeightAsDoubleAttribute(textualElement, nameof(annotation.FontWeight), out var fontWeightVal)) annotation.FontWeight = fontWeightVal;
                if (GetDataPointAttribute(textualElement, nameof(annotation.TextPosition), out var textPosition)) annotation.TextPosition = textPosition;
                if (GetDoubleAttribute(textualElement, nameof(annotation.TextRotation), out var textRotation)) annotation.TextRotation = textRotation;
                if (GetEnumAttribute(textualElement, nameof(annotation.TextHorizontalAlignment), out OxyPlot.HorizontalAlignment hAlign))
                    annotation.TextHorizontalAlignment = hAlign;
                if (GetEnumAttribute(textualElement, nameof(annotation.TextVerticalAlignment), out OxyPlot.VerticalAlignment vAlign))
                    annotation.TextVerticalAlignment = vAlign;

                // Backwards Compatibility
                if (GetOxyColorAttribute(textualElement, "Color", out textColor)) annotation.TextColor = textColor;
                if (GetOxyColorAttribute(textualElement, "TextColor", out textColor)) annotation.TextColor = textColor;
                if (GetDoubleAttribute(textualElement, "Size", out fontSize)) annotation.FontSize = fontSize;
                if (GetDataPointAttribute(textualElement, "Position", out textPosition)) annotation.TextPosition = textPosition;
                if (GetDoubleAttribute(textualElement, "Rotation", out textRotation)) annotation.TextRotation = textRotation;
            }

            var currentAnnotationType = annotation.GetType();

            if (currentAnnotationType == typeof(ArrowAnnotation))
            {
                var arrowElement = textualElement?.Element("Arrow");
                if (arrowElement != null)
                {
                    var arrowAnnotation = (ArrowAnnotation)annotation;
                    if (GetOxyColorAttribute(arrowElement, nameof(arrowAnnotation.Color), out var color)) arrowAnnotation.Color = color;
                    if (GetScreenVectorAttribute(arrowElement, nameof(arrowAnnotation.ArrowDirection), out var arrowDirection)) arrowAnnotation.ArrowDirection = arrowDirection;
                    if (GetDataPointAttribute(arrowElement, nameof(arrowAnnotation.StartPoint), out var startPoint)) arrowAnnotation.StartPoint = startPoint;
                    if (GetDataPointAttribute(arrowElement, nameof(arrowAnnotation.EndPoint), out var endPoint)) arrowAnnotation.EndPoint = endPoint;
                    if (GetDoubleAttribute(arrowElement, nameof(arrowAnnotation.HeadLength), out var headLength)) arrowAnnotation.HeadLength = headLength;
                    if (GetDoubleAttribute(arrowElement, nameof(arrowAnnotation.HeadWidth), out var headWidth)) arrowAnnotation.HeadWidth = headWidth;
                    if (GetDoubleAttribute(arrowElement, nameof(arrowAnnotation.Veeness), out var veeness)) arrowAnnotation.Veeness = veeness;
                    if (GetEnumAttribute(arrowElement, nameof(arrowAnnotation.LineJoin), out LineJoin lineJoin)) arrowAnnotation.LineJoin = lineJoin;
                    if (GetEnumAttribute(arrowElement, nameof(arrowAnnotation.LineStyle), out LineStyle lineStyle)) arrowAnnotation.LineStyle = lineStyle;
                    if (GetDoubleAttribute(arrowElement, nameof(arrowAnnotation.StrokeThickness), out var strokeThickness)) arrowAnnotation.StrokeThickness = strokeThickness;

                    // Backwards Compatibility
                    if (GetScreenVectorAttribute(arrowElement, "Direction", out arrowDirection)) arrowAnnotation.ArrowDirection = arrowDirection;
                    if (GetDoubleAttribute(arrowElement, "BarbLength", out veeness)) arrowAnnotation.Veeness = veeness;
                }
            }
            else if (currentAnnotationType == typeof(TextAnnotation))
            {
                var textElement = textualElement?.Element("Text");
                if (textElement != null)
                {
                    var textAnnotation = (TextAnnotation)annotation;
                    if (GetOxyColorAttribute(textElement, nameof(textAnnotation.Background), out var background)) textAnnotation.Background = background;
                    // Note: TextAnnotation.Offset is ScreenVector in modern OxyPlot, convert from Vector
                    if (GetVectorAttribute(textElement, nameof(textAnnotation.Offset), out var offset)) textAnnotation.Offset = new ScreenVector(offset.X, offset.Y);
                    if (GetOxyThicknessAttribute(textElement, nameof(textAnnotation.Padding), out var padding)) textAnnotation.Padding = padding;
                    if (GetOxyColorAttribute(textElement, nameof(textAnnotation.Stroke), out var stroke)) textAnnotation.Stroke = stroke;
                    if (GetDoubleAttribute(textElement, nameof(textAnnotation.StrokeThickness), out var strokeThickness)) textAnnotation.StrokeThickness = strokeThickness;
                }
            }

            // Must be shape or path annotation
            var shapeAnnotation = annotation as ShapeAnnotation;
            if (shapeAnnotation != null)
            {
                var shapeElement = textualElement?.Element("Shape");
                if (shapeElement != null)
                {
                    if (GetOxyColorAttribute(shapeElement, nameof(shapeAnnotation.Fill), out var fill)) shapeAnnotation.Fill = fill;
                    if (GetOxyColorAttribute(shapeElement, nameof(shapeAnnotation.Stroke), out var stroke)) shapeAnnotation.Stroke = stroke;
                    if (GetDoubleAttribute(shapeElement, nameof(shapeAnnotation.StrokeThickness), out var strokeThickness)) shapeAnnotation.StrokeThickness = strokeThickness;

                    // Must be Ellipse, Point, Rectangle, or Polygon Annotations (all concrete)
                    if (currentAnnotationType == typeof(EllipseAnnotation))
                    {
                        var ellipseElement = shapeElement.Element("Ellipse");
                        if (ellipseElement != null)
                        {
                            var ellipseAnnotation = (EllipseAnnotation)shapeAnnotation;
                            if (GetDoubleAttribute(ellipseElement, nameof(ellipseAnnotation.X), out var x)) ellipseAnnotation.X = x;
                            if (GetDoubleAttribute(ellipseElement, nameof(ellipseAnnotation.Y), out var y)) ellipseAnnotation.Y = y;
                            if (GetDoubleAttribute(ellipseElement, nameof(ellipseAnnotation.Width), out var width)) ellipseAnnotation.Width = width;
                            if (GetDoubleAttribute(ellipseElement, nameof(ellipseAnnotation.Height), out var height)) ellipseAnnotation.Height = height;
                            // Backwards compatibility: MinimumX/MaximumX -> X/Width
                            if (GetDoubleAttribute(ellipseElement, "MinimumX", out var minX) && GetDoubleAttribute(ellipseElement, "MaximumX", out var maxX))
                            {
                                ellipseAnnotation.X = (minX + maxX) / 2;
                                ellipseAnnotation.Width = maxX - minX;
                            }
                            if (GetDoubleAttribute(ellipseElement, "MinimumY", out var minY) && GetDoubleAttribute(ellipseElement, "MaximumY", out var maxY))
                            {
                                ellipseAnnotation.Y = (minY + maxY) / 2;
                                ellipseAnnotation.Height = maxY - minY;
                            }
                        }
                    }
                    else if (currentAnnotationType == typeof(RectangleAnnotation))
                    {
                        var rectangleElement = shapeElement.Element("Rectangle");
                        if (rectangleElement != null)
                        {
                            var rectangleAnnotation = (RectangleAnnotation)shapeAnnotation;
                            if (GetDoubleAttribute(rectangleElement, nameof(rectangleAnnotation.MinimumX), out var minimumX)) rectangleAnnotation.MinimumX = minimumX;
                            if (GetDoubleAttribute(rectangleElement, nameof(rectangleAnnotation.MaximumY), out var maximumY)) rectangleAnnotation.MaximumY = maximumY;
                            if (GetDoubleAttribute(rectangleElement, nameof(rectangleAnnotation.MaximumX), out var maximumX)) rectangleAnnotation.MaximumX = maximumX;
                            if (GetDoubleAttribute(rectangleElement, nameof(rectangleAnnotation.MinimumY), out var minimumY)) rectangleAnnotation.MinimumY = minimumY;
                        }
                    }
                    else if (currentAnnotationType == typeof(PointAnnotation))
                    {
                        var pointElement = shapeElement.Element("Point");
                        if (pointElement != null)
                        {
                            var pointAnnotation = (PointAnnotation)shapeAnnotation;
                            if (GetDoubleAttribute(pointElement, nameof(pointAnnotation.X), out var x)) pointAnnotation.X = x;
                            if (GetDoubleAttribute(pointElement, nameof(pointAnnotation.Y), out var y)) pointAnnotation.Y = y;
                            if (GetDoubleAttribute(pointElement, nameof(pointAnnotation.Size), out var size)) pointAnnotation.Size = size;
                            if (GetDoubleAttribute(pointElement, nameof(pointAnnotation.TextMargin), out var textMargin)) pointAnnotation.TextMargin = textMargin;
                            if (GetEnumAttribute(pointElement, nameof(pointAnnotation.Shape), out MarkerType shape)) pointAnnotation.Shape = shape;
                        }
                    }
                    else if (currentAnnotationType == typeof(PolygonAnnotation))
                    {
                        var polygonElement = shapeElement.Element("Polygon");
                        if (polygonElement != null)
                        {
                            var polygonAnnotation = (PolygonAnnotation)shapeAnnotation;
                            if (GetEnumAttribute(polygonElement, nameof(polygonAnnotation.LineJoin), out LineJoin lineJoin)) polygonAnnotation.LineJoin = lineJoin;
                            if (GetEnumAttribute(polygonElement, nameof(polygonAnnotation.LineStyle), out LineStyle lineStyle)) polygonAnnotation.LineStyle = lineStyle;
                            var polyPointsElement = polygonElement.Element(nameof(polygonAnnotation.Points));
                            if (polyPointsElement != null)
                            {
                                polygonAnnotation.Points.Clear();
                                foreach (var pt in polyPointsElement.PointsFromXElement())
                                    polygonAnnotation.Points.Add(pt);
                            }

                            // Backwards compatibility
                            polyPointsElement = polygonElement.Element("DataPoints");
                            if (polyPointsElement != null)
                            {
                                polygonAnnotation.Points.Clear();
                                foreach (var pt in polyPointsElement.PointsFromXElement())
                                    polygonAnnotation.Points.Add(pt);
                            }
                        }
                    }
                }
            }
            else
            {
                var pathAnnotation = annotation as PathAnnotation;
                if (pathAnnotation != null)
                {
                    var pathElement = textualElement?.Element("Path");
                    if (pathElement != null)
                    {
                        if (GetOxyColorAttribute(pathElement, nameof(pathAnnotation.Color), out var color)) pathAnnotation.Color = color;
                        if (GetBooleanAttribute(pathElement, nameof(pathAnnotation.ClipByXAxis), out var clipByXAxis)) pathAnnotation.ClipByXAxis = clipByXAxis;
                        if (GetBooleanAttribute(pathElement, nameof(pathAnnotation.ClipByYAxis), out var clipByYAxis)) pathAnnotation.ClipByYAxis = clipByYAxis;
                        // Note: ClipText property was removed from PathAnnotation
                        if (GetEnumAttribute(pathElement, nameof(pathAnnotation.LineJoin), out LineJoin lineJoin)) pathAnnotation.LineJoin = lineJoin;
                        if (GetEnumAttribute(pathElement, nameof(pathAnnotation.LineStyle), out LineStyle lineStyle)) pathAnnotation.LineStyle = lineStyle;
                        if (GetDoubleAttribute(pathElement, nameof(pathAnnotation.StrokeThickness), out var strokeThickness)) pathAnnotation.StrokeThickness = strokeThickness;
                        if (GetDoubleAttribute(pathElement, nameof(pathAnnotation.TextMargin), out var textMargin)) pathAnnotation.TextMargin = textMargin;
                        if (GetEnumAttribute(pathElement, nameof(pathAnnotation.TextOrientation), out AnnotationTextOrientation textOrientation)) pathAnnotation.TextOrientation = textOrientation;
                        if (GetDoubleAttribute(pathElement, nameof(pathAnnotation.TextLinePosition), out var textLinePosition)) pathAnnotation.TextLinePosition = textLinePosition;

                        // Must be Line, Polyline, or Function Annotations (all concrete)
                        if (currentAnnotationType == typeof(LineAnnotation))
                        {
                            var lineElement = pathElement.Element("Line");
                            if (lineElement != null)
                            {
                                var lineAnnotation = (LineAnnotation)pathAnnotation;
                                if (GetEnumAttribute(lineElement, nameof(lineAnnotation.Type), out LineAnnotationType type)) lineAnnotation.Type = type;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.X), out var x)) lineAnnotation.X = x;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.Y), out var y)) lineAnnotation.Y = y;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.Intercept), out var intercept)) lineAnnotation.Intercept = intercept;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.MinimumX), out var minimumX)) lineAnnotation.MinimumX = minimumX;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.MaximumY), out var maximumY)) lineAnnotation.MaximumY = maximumY;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.MaximumX), out var maximumX)) lineAnnotation.MaximumX = maximumX;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.MinimumY), out var minimumY)) lineAnnotation.MinimumY = minimumY;
                                if (GetDoubleAttribute(lineElement, nameof(lineAnnotation.Slope), out var slope)) lineAnnotation.Slope = slope;
                            }
                        }
                        else if (currentAnnotationType == typeof(PolylineAnnotation))
                        {
                            var polylineElement = pathElement.Element("Polyline");
                            if (polylineElement != null)
                            {
                                var polylineAnnotation = (PolylineAnnotation)pathAnnotation;
                                if (GetDoubleAttribute(polylineElement, nameof(polylineAnnotation.MinimumSegmentLength), out var minimumSegmentLength)) polylineAnnotation.MinimumSegmentLength = minimumSegmentLength;
                                var polyPointsElement = polylineElement.Element(nameof(polylineAnnotation.Points));
                                if (polyPointsElement != null)
                                {
                                    polylineAnnotation.Points.Clear();
                                    foreach (var pt in polyPointsElement.PointsFromXElement())
                                        polylineAnnotation.Points.Add(pt);
                                }

                                // Backwards compatibility
                                polyPointsElement = polylineElement.Element("DataPoints");
                                if (polyPointsElement != null)
                                {
                                    polylineAnnotation.Points.Clear();
                                    foreach (var pt in polyPointsElement.PointsFromXElement())
                                        polylineAnnotation.Points.Add(pt);
                                }
                            }
                        }
                    }
                }
            }

            return annotation;
        }

        /// <summary>
        /// Hides the expander controls. Useful when an annotation is deleted.
        /// </summary>
        public void HideExpanders()
        {
            TextEXP.Visibility = Visibility.Collapsed;
            DisplayOptionsEXP.Visibility = Visibility.Collapsed;
        }

        private void LineTypeControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Annotation == null || Annotation.GetType() != typeof(LineAnnotation)) return;

            var selectedType = (LineAnnotationType)((ComboBox)LineTypeControl.InnerContent).SelectedItem;

            if (selectedType == LineAnnotationType.Horizontal)
            {
                XValueControl.Visibility = Visibility.Collapsed;
                YValueControl.Visibility = Visibility.Visible;
                InterceptControl.Visibility = Visibility.Collapsed;
                SlopeControl.Visibility = Visibility.Collapsed;
            }
            else if (selectedType == LineAnnotationType.Vertical)
            {
                XValueControl.Visibility = Visibility.Visible;
                YValueControl.Visibility = Visibility.Collapsed;
                InterceptControl.Visibility = Visibility.Collapsed;
                SlopeControl.Visibility = Visibility.Collapsed;
            }
            else if (selectedType == LineAnnotationType.LinearEquation)
            {
                XValueControl.Visibility = Visibility.Collapsed;
                YValueControl.Visibility = Visibility.Collapsed;
                InterceptControl.Visibility = Visibility.Visible;
                SlopeControl.Visibility = Visibility.Visible;
            }
        }
    }

    /// <summary>
    /// Converts between OxyPlot DataPoint and WPF Point types.
    /// </summary>
    public class DataPointToPointConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OxyPlot DataPoint to a WPF Point.
        /// </summary>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            if (value.GetType() != typeof(DataPoint)) return null;
            var dp = (DataPoint)value;
            return new System.Windows.Point(dp.X, dp.Y);
        }

        /// <summary>
        /// Converts a WPF Point back to an OxyPlot DataPoint.
        /// </summary>
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            if (value.GetType() != typeof(System.Windows.Point)) return null;
            var p = (System.Windows.Point)value;
            return new DataPoint(p.X, p.Y);
        }
    }

    /// <summary>
    /// Converts between OxyPlot ScreenVector and WPF Point types.
    /// </summary>
    public class ScreenVectorToPointConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OxyPlot ScreenVector to a WPF Point.
        /// </summary>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            if (value.GetType() != typeof(ScreenVector)) return null;
            var sv = (ScreenVector)value;
            return new System.Windows.Point(sv.X, sv.Y);
        }

        /// <summary>
        /// Converts a WPF Point back to an OxyPlot ScreenVector.
        /// </summary>
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            if (value.GetType() != typeof(System.Windows.Point)) return null;
            var p = (System.Windows.Point)value;
            return new ScreenVector(p.X, p.Y);
        }
    }
}
