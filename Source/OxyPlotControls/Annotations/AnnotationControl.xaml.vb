Imports System.Globalization
Imports OxyPlot
Imports OxyPlot.Annotations

Public Class AnnotationControl


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Shared ReadOnly AnnotationPropertiesTag As String = "Annotation"


    Public Shared ReadOnly Property TextLinePositionOptions As New List(Of Double)({0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1})
    Public Shared ReadOnly Property AnnotationLayerOptions As New List(Of AnnotationLayer)(DirectCast([Enum].GetValues(GetType(AnnotationLayer)), AnnotationLayer()))
    Public Shared ReadOnly Property AnnotationTextOrientationOptions As New List(Of AnnotationTextOrientation)(DirectCast([Enum].GetValues(GetType(AnnotationTextOrientation)), AnnotationTextOrientation()))
    Public Shared ReadOnly Property AnnotationFunctionTypeOptions As New List(Of FunctionAnnotationType)(DirectCast([Enum].GetValues(GetType(FunctionAnnotationType)), FunctionAnnotationType()))
    Public Shared ReadOnly Property LineAnnotationTypeOptions As New List(Of LineAnnotationType)(DirectCast([Enum].GetValues(GetType(LineAnnotationType)), LineAnnotationType()))


    Public Shared ReadOnly Property MarkerTypeOptions As New List(Of MarkerType)(DirectCast([Enum].GetValues(GetType(MarkerType)), MarkerType()))
    Public Shared ReadOnly Property LineJoinOptions As New List(Of LineJoin)(DirectCast([Enum].GetValues(GetType(LineJoin)), LineJoin()))
    Public Shared ReadOnly Property LineStyleOptions As List(Of DoubleCollection) = GenericControls.LineStyleSelectorControl.LineStyleOptions

    Public Shared AnnotationProperty As DependencyProperty = DependencyProperty.Register(NameOf(Annotation), GetType(Wpf.TextualAnnotation), GetType(AnnotationControl), New PropertyMetadata(Nothing, AddressOf AnnotationChangedCallback))

    Public Property Annotation As Wpf.TextualAnnotation
        Get
            Return DirectCast(GetValue(AnnotationProperty), Wpf.TextualAnnotation)
        End Get
        Set(value As Wpf.TextualAnnotation)
            SetValue(AnnotationProperty, value)
        End Set
    End Property

    Private Shared Sub AnnotationChangedCallback(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(AnnotationControl) Then Exit Sub
        Dim thisControl = DirectCast(d, AnnotationControl)

        If IsNothing(e.NewValue) Then Exit Sub

        ' Show Expanders
        thisControl.TextEXP.Visibility = Visibility.Visible
        thisControl.DisplayOptionsEXP.Visibility = Visibility.Visible

        ' Determine whether to show X-Y Controls
        If e.NewValue.GetType = GetType(Wpf.LineAnnotation) Then

            Dim newAnnotation = DirectCast(e.NewValue, Wpf.LineAnnotation)

            If newAnnotation.Type = LineAnnotationType.Horizontal Then
                thisControl.XValueControl.Visibility = Visibility.Collapsed
                thisControl.YValueControl.Visibility = Visibility.Visible
                thisControl.InterceptControl.Visibility = Visibility.Collapsed
                thisControl.SlopeControl.Visibility = Visibility.Collapsed

            ElseIf newAnnotation.Type = LineAnnotationType.Vertical Then
                thisControl.XValueControl.Visibility = Visibility.Visible
                thisControl.YValueControl.Visibility = Visibility.Collapsed
                thisControl.InterceptControl.Visibility = Visibility.Collapsed
                thisControl.SlopeControl.Visibility = Visibility.Collapsed

            ElseIf newAnnotation.Type = LineAnnotationType.LinearEquation Then
                thisControl.XValueControl.Visibility = Visibility.Collapsed
                thisControl.YValueControl.Visibility = Visibility.Collapsed
                thisControl.InterceptControl.Visibility = Visibility.Visible
                thisControl.SlopeControl.Visibility = Visibility.Visible

            End If

        ElseIf e.NewValue.GetType = GetType(Wpf.PointAnnotation) Then
            thisControl.XValueControl.Visibility = Visibility.Visible
            thisControl.YValueControl.Visibility = Visibility.Visible
            thisControl.InterceptControl.Visibility = Visibility.Collapsed
            thisControl.SlopeControl.Visibility = Visibility.Collapsed

        Else
            ' Hide controls
            thisControl.XValueControl.Visibility = Visibility.Collapsed
            thisControl.YValueControl.Visibility = Visibility.Collapsed
            thisControl.InterceptControl.Visibility = Visibility.Collapsed
            thisControl.SlopeControl.Visibility = Visibility.Collapsed
        End If


        ' Determine whether to show the Text Angle Control
        If e.NewValue.GetType = GetType(Wpf.LineAnnotation) OrElse
            e.NewValue.GetType = GetType(Wpf.PolylineAnnotation) Then
            thisControl.TextAngleControl.Visibility = Visibility.Collapsed
        Else
            thisControl.TextAngleControl.Visibility = Visibility.Visible
        End If


    End Sub

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(AnnotationControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property


    Public Shared Function AnnotationPropertiesToXElement(annotation As Wpf.TextualAnnotation) As XElement
        Dim annotationProperties As New XElement(AnnotationPropertiesTag)
        Dim annotationType As Type = annotation.GetType()
        annotationProperties.SetAttributeValue("AnnotationType", annotationType.ToString())
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Annotation Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim generalProperties As New XElement("General")
        generalProperties.SetAttributeValue(NameOf(annotation.Name), annotation.Name.ToString())
        generalProperties.SetAttributeValue(NameOf(annotation.IsEnabled), annotation.IsEnabled.ToString())
        generalProperties.SetAttributeValue(NameOf(annotation.Layer), annotation.Layer.ToString())
        generalProperties.SetAttributeValue(NameOf(annotation.XAxisKey), annotation.XAxisKey)
        generalProperties.SetAttributeValue(NameOf(annotation.YAxisKey), annotation.YAxisKey)
        annotationProperties.Add(generalProperties)
        '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Textual Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim weightConverter As New FontWeightConverter()
        Dim ffc As New FontFamilyConverter()
        Dim textualProperties As New XElement("Textual")
        textualProperties.SetAttributeValue(NameOf(annotation.Text), annotation.Text)
        textualProperties.SetAttributeValue(NameOf(annotation.TextColor), annotation.TextColor.ToString()) 'ToString() returns the hexadecimal notation, use ColorConverter.ConvertFromString() to convert from hexidecimal to color.
        If annotation.FontFamily IsNot Nothing Then textualProperties.SetAttributeValue(NameOf(annotation.FontFamily), ffc.ConvertToInvariantString(annotation.FontFamily))
        textualProperties.SetAttributeValue(NameOf(annotation.FontSize), annotation.FontSize.ToString("G17", CultureInfo.InvariantCulture))
        textualProperties.SetAttributeValue(NameOf(annotation.FontWeight), weightConverter.ConvertToInvariantString(annotation.FontWeight))
        textualProperties.SetAttributeValue(NameOf(annotation.TextPosition), annotation.TextPosition.ToPrettyText())
        textualProperties.SetAttributeValue(NameOf(annotation.TextRotation), annotation.TextRotation.ToString("G17", CultureInfo.InvariantCulture))
        textualProperties.SetAttributeValue(NameOf(annotation.TextHorizontalAlignment), annotation.TextHorizontalAlignment.ToString())
        textualProperties.SetAttributeValue(NameOf(annotation.TextVerticalAlignment), annotation.TextVerticalAlignment.ToString())
        annotationProperties.Add(textualProperties)
        'Must be either arrow (concrete), text (concrete), shape (abstract), or path (abstract)
        Select Case annotationType
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Arrow Properties
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Case GetType(Wpf.ArrowAnnotation)
                Dim arrowAnnotation As Wpf.ArrowAnnotation = DirectCast(annotation, Wpf.ArrowAnnotation)
                Dim arrowProperties As New XElement("Arrow")
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.Color), arrowAnnotation.Color.ToString())
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.ArrowDirection), arrowAnnotation.ArrowDirection.ToPrettyText())
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.StartPoint), arrowAnnotation.StartPoint.ToPrettyText())
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.EndPoint), arrowAnnotation.EndPoint.ToPrettyText())
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.HeadLength), arrowAnnotation.HeadLength.ToString("G17", CultureInfo.InvariantCulture))
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.HeadWidth), arrowAnnotation.HeadWidth.ToString("G17", CultureInfo.InvariantCulture))
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.Veeness), arrowAnnotation.Veeness.ToString("G17", CultureInfo.InvariantCulture))
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.LineJoin), arrowAnnotation.LineJoin.ToString())
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.LineStyle), arrowAnnotation.LineStyle.ToString())
                arrowProperties.SetAttributeValue(NameOf(arrowAnnotation.StrokeThickness), arrowAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
                textualProperties.Add(arrowProperties)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Text Properties
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Case GetType(Wpf.TextAnnotation)
                Dim thickConvert As New ThicknessConverter()
                Dim textAnnotation As Wpf.TextAnnotation = DirectCast(annotation, Wpf.TextAnnotation)
                Dim textProperties As New XElement("Text")
                textProperties.SetAttributeValue(NameOf(textAnnotation.Background), textAnnotation.Background.ToString())
                textProperties.SetAttributeValue(NameOf(textAnnotation.Offset), textAnnotation.Offset.ToPrettyText())
                textProperties.SetAttributeValue(NameOf(textAnnotation.Padding), thickConvert.ConvertToInvariantString(textAnnotation.Padding)) 'Use a ThicknessConverter() to convert from string to thickness
                textProperties.SetAttributeValue(NameOf(textAnnotation.Stroke), textAnnotation.Stroke.ToString())
                textProperties.SetAttributeValue(NameOf(textAnnotation.StrokeThickness), textAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
                textualProperties.Add(textProperties)
                '
        End Select
        'Must be shape or path annotation 
        Dim shapeAnnotation As Wpf.ShapeAnnotation = TryCast(annotation, Wpf.ShapeAnnotation)
        If Not IsNothing(shapeAnnotation) Then
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Shape Properties
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim shapeProperties As New XElement("Shape")
            shapeProperties.SetAttributeValue(NameOf(shapeAnnotation.Fill), shapeAnnotation.Fill.ToString())
            shapeProperties.SetAttributeValue(NameOf(shapeAnnotation.Stroke), shapeAnnotation.Stroke.ToString())
            shapeProperties.SetAttributeValue(NameOf(shapeAnnotation.StrokeThickness), shapeAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            textualProperties.Add(shapeProperties)
            'Must be Ellipse, Point, Rectangle, or Polygon Annotations (all concrete)
            Select Case annotationType
                Case GetType(Wpf.EllipseAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Ellipse Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim ellipseAnnotation As Wpf.EllipseAnnotation = DirectCast(shapeAnnotation, Wpf.EllipseAnnotation)
                    Dim ellipseProperties As New XElement("Ellipse")
                    ellipseProperties.SetAttributeValue(NameOf(ellipseAnnotation.MinimumX), ellipseAnnotation.MinimumX.ToString("G17", CultureInfo.InvariantCulture))
                    ellipseProperties.SetAttributeValue(NameOf(ellipseAnnotation.MaximumY), ellipseAnnotation.MaximumY.ToString("G17", CultureInfo.InvariantCulture))
                    ellipseProperties.SetAttributeValue(NameOf(ellipseAnnotation.MaximumX), ellipseAnnotation.MaximumX.ToString("G17", CultureInfo.InvariantCulture))
                    ellipseProperties.SetAttributeValue(NameOf(ellipseAnnotation.MinimumY), ellipseAnnotation.MinimumY.ToString("G17", CultureInfo.InvariantCulture))
                    shapeProperties.Add(ellipseProperties)
                Case GetType(Wpf.RectangleAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Rectangle Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim rectangleAnnotation As Wpf.RectangleAnnotation = DirectCast(shapeAnnotation, Wpf.RectangleAnnotation)
                    Dim rectangleProperties As New XElement("Rectangle")
                    rectangleProperties.SetAttributeValue(NameOf(rectangleAnnotation.MinimumX), rectangleAnnotation.MinimumX.ToString("G17", CultureInfo.InvariantCulture))
                    rectangleProperties.SetAttributeValue(NameOf(rectangleAnnotation.MaximumY), rectangleAnnotation.MaximumY.ToString("G17", CultureInfo.InvariantCulture))
                    rectangleProperties.SetAttributeValue(NameOf(rectangleAnnotation.MaximumX), rectangleAnnotation.MaximumX.ToString("G17", CultureInfo.InvariantCulture))
                    rectangleProperties.SetAttributeValue(NameOf(rectangleAnnotation.MinimumY), rectangleAnnotation.MinimumY.ToString("G17", CultureInfo.InvariantCulture))
                    shapeProperties.Add(rectangleProperties)
                Case GetType(Wpf.PointAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Point Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim pointAnnotation As Wpf.PointAnnotation = DirectCast(shapeAnnotation, Wpf.PointAnnotation)
                    Dim pointProperties As New XElement("Point")
                    pointProperties.SetAttributeValue(NameOf(pointAnnotation.X), pointAnnotation.X.ToString("G17", CultureInfo.InvariantCulture))
                    pointProperties.SetAttributeValue(NameOf(pointAnnotation.Y), pointAnnotation.Y.ToString("G17", CultureInfo.InvariantCulture))
                    pointProperties.SetAttributeValue(NameOf(pointAnnotation.Size), pointAnnotation.Size.ToString("G17", CultureInfo.InvariantCulture))
                    pointProperties.SetAttributeValue(NameOf(pointAnnotation.TextMargin), pointAnnotation.TextMargin.ToString("G17", CultureInfo.InvariantCulture))
                    pointProperties.SetAttributeValue(NameOf(pointAnnotation.Shape), pointAnnotation.Shape.ToString())
                    shapeProperties.Add(pointProperties)
                Case GetType(Wpf.PolygonAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Polygon Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim polygonAnnotation As Wpf.PolygonAnnotation = DirectCast(shapeAnnotation, Wpf.PolygonAnnotation)
                    Dim polygonProperties As New XElement("Polygon")
                    polygonProperties.SetAttributeValue(NameOf(polygonAnnotation.LineJoin), polygonAnnotation.LineJoin.ToString())
                    polygonProperties.SetAttributeValue(NameOf(polygonAnnotation.LineStyle), polygonAnnotation.LineStyle.ToString())
                    polygonProperties.Add(polygonAnnotation.Points.ToXElement(NameOf(polygonAnnotation.Points)))
                    shapeProperties.Add(polygonProperties)
            End Select
        Else
            Dim pathAnnotation As Wpf.PathAnnotation = TryCast(annotation, Wpf.PathAnnotation)
            If Not IsNothing(pathAnnotation) Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Path Properties
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim pathProperties As New XElement("Path")
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.Color), pathAnnotation.Color.ToString())
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.ClipByXAxis), pathAnnotation.ClipByXAxis.ToString())
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.ClipByYAxis), pathAnnotation.ClipByYAxis.ToString())
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.ClipText), pathAnnotation.ClipText.ToString())
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.LineJoin), pathAnnotation.LineJoin.ToString())
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.LineStyle), pathAnnotation.LineStyle.ToString())
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.StrokeThickness), pathAnnotation.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.TextMargin), pathAnnotation.TextMargin.ToString("G17", CultureInfo.InvariantCulture))
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.TextOrientation), pathAnnotation.TextOrientation.ToString())
                pathProperties.SetAttributeValue(NameOf(pathAnnotation.TextLinePosition), pathAnnotation.TextLinePosition.ToString("G17", CultureInfo.InvariantCulture))
                textualProperties.Add(pathProperties)
                'Must be Line, Polyline, or Function Annotations (all concrete)
                Select Case annotationType
                    Case GetType(Wpf.LineAnnotation)
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'Line Properties
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim lineAnnotation As Wpf.LineAnnotation = DirectCast(pathAnnotation, Wpf.LineAnnotation)
                        Dim lineProperties As New XElement("Line")
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.Type), lineAnnotation.Type.ToString())
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.X), lineAnnotation.X.ToString("G17", CultureInfo.InvariantCulture))
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.Y), lineAnnotation.Y.ToString("G17", CultureInfo.InvariantCulture))
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.Intercept), lineAnnotation.Intercept.ToString("G17", CultureInfo.InvariantCulture))
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.MinimumX), lineAnnotation.MinimumX.ToString("G17", CultureInfo.InvariantCulture))
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.MaximumY), lineAnnotation.MaximumY.ToString("G17", CultureInfo.InvariantCulture))
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.MaximumX), lineAnnotation.MaximumX.ToString("G17", CultureInfo.InvariantCulture))
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.MinimumY), lineAnnotation.MinimumY.ToString("G17", CultureInfo.InvariantCulture))
                        lineProperties.SetAttributeValue(NameOf(lineAnnotation.Slope), lineAnnotation.Slope.ToString("G17", CultureInfo.InvariantCulture))
                        pathProperties.Add(lineProperties)
                    Case GetType(Wpf.PolylineAnnotation)
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'Polyine Properties
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim polylineAnnotation As Wpf.PolylineAnnotation = DirectCast(pathAnnotation, Wpf.PolylineAnnotation)
                        Dim polylineProperties As New XElement("Polyline")
                        polylineProperties.SetAttributeValue(NameOf(polylineAnnotation.MinimumSegmentLength), polylineAnnotation.MinimumSegmentLength.ToString("G17", CultureInfo.InvariantCulture))
                        polylineProperties.Add(polylineAnnotation.Points.ToXElement(NameOf(polylineAnnotation.Points)))
                        pathProperties.Add(polylineProperties)
                End Select
            End If
        End If
        '
        Return annotationProperties
    End Function
    '
    ''' <summary>
    ''' Load general plot property settings from XElement.
    ''' </summary>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Function XElementToAnnotationProperties(element As XElement) As Wpf.TextualAnnotation
        'Early Exit
        If element.Name <> AnnotationPropertiesTag Then Return Nothing
        'Set up converters
        Dim fontWeightConverter = New FontWeightConverter()
        Dim thicknessConverter = New ThicknessConverter()
        'Set up annotation to return
        Dim annotation As Wpf.TextualAnnotation
        Dim annotationTypeString As String = ""
        If Not IsNothing(element.Attribute("AnnotationType")) Then annotationTypeString = element.Attribute("AnnotationType").Value
        If annotationTypeString = GetType(Wpf.ArrowAnnotation).ToString Then
            annotation = New Wpf.ArrowAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.TextAnnotation).ToString Then
            annotation = New Wpf.TextAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.EllipseAnnotation).ToString Then
            annotation = New Wpf.EllipseAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.RectangleAnnotation).ToString Then
            annotation = New Wpf.RectangleAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.PointAnnotation).ToString Then
            annotation = New Wpf.PointAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.PolygonAnnotation).ToString Then
            annotation = New Wpf.PolygonAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.LineAnnotation).ToString Then
            annotation = New Wpf.LineAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.PolylineAnnotation).ToString Then
            annotation = New Wpf.PolylineAnnotation()
        ElseIf annotationTypeString = GetType(Wpf.FunctionAnnotation).ToString Then
            annotation = New Wpf.FunctionAnnotation()
        Else
            annotation = New Wpf.TextAnnotation()
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Annotation Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim generalElement = element.Element("General")
        If Not IsNothing(generalElement) Then
            GetStringAttribute(generalElement, NameOf(annotation.Name), annotation.Name)
            GetBooleanAttribute(generalElement, NameOf(annotation.IsEnabled), annotation.IsEnabled)
            GetEnumAttribute(generalElement, NameOf(annotation.Layer), annotation.Layer)
            GetStringAttribute(generalElement, NameOf(annotation.XAxisKey), annotation.XAxisKey)
            GetStringAttribute(generalElement, NameOf(annotation.YAxisKey), annotation.YAxisKey)
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Textual Properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim textualElement = element.Element("Textual")
        If Not IsNothing(textualElement) Then
            Dim ffc As New FontFamilyConverter()
            GetStringAttribute(textualElement, NameOf(annotation.Text), annotation.Text)
            GetColorAttribute(textualElement, NameOf(annotation.TextColor), annotation.TextColor)
            GetFontFamilyAttribute(textualElement, NameOf(annotation.FontFamily), ffc, annotation.FontFamily)
            GetDoubleAttribute(textualElement, NameOf(annotation.FontSize), annotation.FontSize)
            GetFontWeightAttribute(textualElement, NameOf(annotation.FontWeight), fontWeightConverter, annotation.FontWeight)
            GetDataPointAttribute(textualElement, NameOf(annotation.TextPosition), annotation.TextPosition)
            GetDoubleAttribute(textualElement, NameOf(annotation.TextRotation), annotation.TextRotation)
            GetEnumAttribute(textualElement, NameOf(annotation.TextHorizontalAlignment), annotation.TextHorizontalAlignment)
            GetEnumAttribute(textualElement, NameOf(annotation.TextVerticalAlignment), annotation.TextVerticalAlignment)

            ' Backwards Compatibility
            GetColorAttribute(textualElement, "Color", annotation.TextColor)
            GetFontFamilyAttribute(textualElement, "Font", ffc, annotation.FontFamily)
            GetDoubleAttribute(textualElement, "Size", annotation.FontSize)
            GetFontWeightAttribute(textualElement, "Weight", fontWeightConverter, annotation.FontWeight)
            GetDataPointAttribute(textualElement, "Position", annotation.TextPosition)
            GetDoubleAttribute(textualElement, "Rotation", annotation.TextRotation)
            GetEnumAttribute(textualElement, "HorizontalAlignment", annotation.TextHorizontalAlignment)
            GetEnumAttribute(textualElement, "VerticalAlignment", annotation.TextVerticalAlignment)
        End If
        '
        Select Case annotation.GetType()
            Case GetType(Wpf.ArrowAnnotation)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Arrow Properties
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim arrowElement = textualElement.Element("Arrow")
                If Not IsNothing(arrowElement) Then
                    Dim arrowAnnotation As Wpf.ArrowAnnotation = DirectCast(annotation, Wpf.ArrowAnnotation)
                    GetColorAttribute(arrowElement, NameOf(arrowAnnotation.Color), arrowAnnotation.Color)
                    GetScreenVectorAttribute(arrowElement, NameOf(arrowAnnotation.ArrowDirection), arrowAnnotation.ArrowDirection)
                    GetDataPointAttribute(arrowElement, NameOf(arrowAnnotation.StartPoint), arrowAnnotation.StartPoint)
                    GetDataPointAttribute(arrowElement, NameOf(arrowAnnotation.EndPoint), arrowAnnotation.EndPoint)
                    GetDoubleAttribute(arrowElement, NameOf(arrowAnnotation.HeadLength), arrowAnnotation.HeadLength)
                    GetDoubleAttribute(arrowElement, NameOf(arrowAnnotation.HeadWidth), arrowAnnotation.HeadWidth)
                    GetDoubleAttribute(arrowElement, NameOf(arrowAnnotation.Veeness), arrowAnnotation.Veeness)
                    GetEnumAttribute(arrowElement, NameOf(arrowAnnotation.LineJoin), arrowAnnotation.LineJoin)
                    GetEnumAttribute(arrowElement, NameOf(arrowAnnotation.LineStyle), arrowAnnotation.LineStyle)
                    GetDoubleAttribute(arrowElement, NameOf(arrowAnnotation.StrokeThickness), arrowAnnotation.StrokeThickness)

                    ' Backwards Compatibility
                    GetScreenVectorAttribute(arrowElement, "Direction", arrowAnnotation.ArrowDirection)
                    GetDoubleAttribute(arrowElement, "BarbLength", arrowAnnotation.Veeness)
                End If
            Case GetType(Wpf.TextAnnotation)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Text Properties
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim textElement = textualElement.Element("Text")
                If Not IsNothing(textElement) Then
                    Dim textAnnotation As Wpf.TextAnnotation = DirectCast(annotation, Wpf.TextAnnotation)
                    GetColorAttribute(textElement, NameOf(textAnnotation.Background), textAnnotation.Background)
                    GetVectorAttribute(textElement, NameOf(textAnnotation.Offset), textAnnotation.Offset)
                    GetThicknessAttribute(textElement, NameOf(textAnnotation.Padding), thicknessConverter, textAnnotation.Padding) 'Use a ThicknessConverter() to convert from string to thickness
                    GetColorAttribute(textElement, NameOf(textAnnotation.Stroke), textAnnotation.Stroke)
                    GetDoubleAttribute(textElement, NameOf(textAnnotation.StrokeThickness), textAnnotation.StrokeThickness)
                End If
        End Select
        '
        'Must be shape or path annotation 
        Dim shapeAnnotation As Wpf.ShapeAnnotation = TryCast(annotation, Wpf.ShapeAnnotation)
        If Not IsNothing(shapeAnnotation) Then
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Shape Properties
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim shapeElement = textualElement.Element("Shape")
            If Not IsNothing(shapeElement) Then
                GetColorAttribute(shapeElement, NameOf(shapeAnnotation.Fill), shapeAnnotation.Fill)
                GetColorAttribute(shapeElement, NameOf(shapeAnnotation.Stroke), shapeAnnotation.Stroke)
                GetDoubleAttribute(shapeElement, NameOf(shapeAnnotation.StrokeThickness), shapeAnnotation.StrokeThickness)
            End If
            'Must be Ellipse, Point, Rectangle, or Polygon Annotations (all concrete)
            Select Case annotation.GetType
                Case GetType(Wpf.EllipseAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Ellipse Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim ellipseElement = shapeElement.Element("Ellipse")
                    If Not IsNothing(ellipseElement) Then
                        Dim ellipseAnnotation As Wpf.EllipseAnnotation = DirectCast(shapeAnnotation, Wpf.EllipseAnnotation)
                        GetDoubleAttribute(ellipseElement, NameOf(ellipseAnnotation.MinimumX), ellipseAnnotation.MinimumX)
                        GetDoubleAttribute(ellipseElement, NameOf(ellipseAnnotation.MaximumY), ellipseAnnotation.MaximumY)
                        GetDoubleAttribute(ellipseElement, NameOf(ellipseAnnotation.MaximumX), ellipseAnnotation.MaximumX)
                        GetDoubleAttribute(ellipseElement, NameOf(ellipseAnnotation.MinimumY), ellipseAnnotation.MinimumY)
                    End If
                Case GetType(Wpf.RectangleAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Rectangle Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim rectangleElement = shapeElement.Element("Rectangle")
                    If Not IsNothing(rectangleElement) Then
                        Dim rectangleAnnotation As Wpf.RectangleAnnotation = DirectCast(shapeAnnotation, Wpf.RectangleAnnotation)
                        GetDoubleAttribute(rectangleElement, NameOf(rectangleAnnotation.MinimumX), rectangleAnnotation.MinimumX)
                        GetDoubleAttribute(rectangleElement, NameOf(rectangleAnnotation.MaximumY), rectangleAnnotation.MaximumY)
                        GetDoubleAttribute(rectangleElement, NameOf(rectangleAnnotation.MaximumX), rectangleAnnotation.MaximumX)
                        GetDoubleAttribute(rectangleElement, NameOf(rectangleAnnotation.MinimumY), rectangleAnnotation.MinimumY)
                    End If
                Case GetType(Wpf.PointAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Point Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim pointElement = shapeElement.Element("Point")
                    If Not IsNothing(pointElement) Then
                        Dim pointAnnotation As Wpf.PointAnnotation = DirectCast(shapeAnnotation, Wpf.PointAnnotation)
                        GetDoubleAttribute(pointElement, NameOf(pointAnnotation.X), pointAnnotation.X)
                        GetDoubleAttribute(pointElement, NameOf(pointAnnotation.Y), pointAnnotation.Y)
                        GetDoubleAttribute(pointElement, NameOf(pointAnnotation.Size), pointAnnotation.Size)
                        GetDoubleAttribute(pointElement, NameOf(pointAnnotation.TextMargin), pointAnnotation.TextMargin)
                        GetEnumAttribute(pointElement, NameOf(pointAnnotation.Shape), pointAnnotation.Shape)
                    End If
                Case GetType(Wpf.PolygonAnnotation)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Polygon Properties
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim polygonElement = shapeElement.Element("Polygon")
                    If Not IsNothing(polygonElement) Then
                        Dim polygonAnnotation As Wpf.PolygonAnnotation = DirectCast(shapeAnnotation, Wpf.PolygonAnnotation)
                        GetEnumAttribute(polygonElement, NameOf(polygonAnnotation.LineJoin), polygonAnnotation.LineJoin)
                        GetEnumAttribute(polygonElement, NameOf(polygonAnnotation.LineStyle), polygonAnnotation.LineStyle)
                        Dim polyPointsElement = polygonElement.Element(NameOf(polygonAnnotation.Points))
                        If Not IsNothing(polyPointsElement) Then polygonAnnotation.Points = polyPointsElement.PointsFromXElement()

                        ' Backwards compatibility
                        polyPointsElement = polygonElement.Element("DataPoints")
                        If Not IsNothing(polyPointsElement) Then polygonAnnotation.Points = polyPointsElement.PointsFromXElement()
                    End If
            End Select
        Else
            Dim pathAnnotation As Wpf.PathAnnotation = TryCast(annotation, Wpf.PathAnnotation)
            If Not IsNothing(pathAnnotation) Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Path Properties
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim pathElement = textualElement.Element("Path")
                If Not IsNothing(pathElement) Then
                    GetColorAttribute(pathElement, NameOf(pathAnnotation.Color), pathAnnotation.Color)
                    GetBooleanAttribute(pathElement, NameOf(pathAnnotation.ClipByXAxis), pathAnnotation.ClipByXAxis)
                    GetBooleanAttribute(pathElement, NameOf(pathAnnotation.ClipByYAxis), pathAnnotation.ClipByYAxis)
                    GetBooleanAttribute(pathElement, NameOf(pathAnnotation.ClipText), pathAnnotation.ClipText)
                    GetEnumAttribute(pathElement, NameOf(pathAnnotation.LineJoin), pathAnnotation.LineJoin)
                    GetEnumAttribute(pathElement, NameOf(pathAnnotation.LineStyle), pathAnnotation.LineStyle)
                    GetDoubleAttribute(pathElement, NameOf(pathAnnotation.StrokeThickness), pathAnnotation.StrokeThickness)
                    GetDoubleAttribute(pathElement, NameOf(pathAnnotation.TextMargin), pathAnnotation.TextMargin)
                    GetEnumAttribute(pathElement, NameOf(pathAnnotation.TextOrientation), pathAnnotation.TextOrientation)
                    GetDoubleAttribute(pathElement, NameOf(pathAnnotation.TextLinePosition), pathAnnotation.TextLinePosition)
                End If
                'Must be Line, Polyline, or Function Annotations (all concrete)
                Select Case annotation.GetType
                    Case GetType(Wpf.LineAnnotation)
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'Line Properties
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim lineElement = pathElement.Element("Line")
                        If Not IsNothing(lineElement) Then
                            Dim lineAnnotation As Wpf.LineAnnotation = DirectCast(pathAnnotation, Wpf.LineAnnotation)
                            GetEnumAttribute(lineElement, NameOf(lineAnnotation.Type), lineAnnotation.Type)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.X), lineAnnotation.X)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.Y), lineAnnotation.Y)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.Intercept), lineAnnotation.Intercept)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.MinimumX), lineAnnotation.MinimumX)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.MaximumY), lineAnnotation.MaximumY)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.MaximumX), lineAnnotation.MaximumX)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.MinimumY), lineAnnotation.MinimumY)
                            GetDoubleAttribute(lineElement, NameOf(lineAnnotation.Slope), lineAnnotation.Slope)
                        End If
                    Case GetType(Wpf.PolylineAnnotation)
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'Polyine Properties
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Dim polylineElement = pathElement.Element("Polyline")
                        If Not IsNothing(polylineElement) Then
                            Dim polylineAnnotation As Wpf.PolylineAnnotation = DirectCast(pathAnnotation, Wpf.PolylineAnnotation)
                            GetDoubleAttribute(polylineElement, NameOf(polylineAnnotation.MinimumSegmentLength), polylineAnnotation.MinimumSegmentLength)
                            Dim polyPointsElement = polylineElement.Element(NameOf(polylineAnnotation.Points))
                            If Not IsNothing(polyPointsElement) Then polylineAnnotation.Points = polyPointsElement.PointsFromXElement()

                            ' Backwards compatibility
                            polyPointsElement = polylineElement.Element("DataPoints")
                            If Not IsNothing(polyPointsElement) Then polylineAnnotation.Points = polyPointsElement.PointsFromXElement()
                        End If
                End Select
            End If
        End If
        '
        Return annotation
    End Function

    'Useful for when delete was pushed
    Public Sub HideExpanders()
        TextEXP.Visibility = Visibility.Collapsed
        DisplayOptionsEXP.Visibility = Visibility.Collapsed
    End Sub

    ''' <summary>
    ''' When the line annotation type changes, determine whether to show XY controls. 
    ''' </summary>
    Private Sub LineTypeControl_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        If Annotation.GetType <> GetType(Wpf.LineAnnotation) Then Exit Sub

        If CType(CType(LineTypeControl.InnerContent, ComboBox).SelectedItem, LineAnnotationType) = LineAnnotationType.Horizontal Then
            XValueControl.Visibility = Visibility.Collapsed
            YValueControl.Visibility = Visibility.Visible
            InterceptControl.Visibility = Visibility.Collapsed
            SlopeControl.Visibility = Visibility.Collapsed

        ElseIf CType(CType(LineTypeControl.InnerContent, ComboBox).SelectedItem, LineAnnotationType) = LineAnnotationType.Vertical Then
            XValueControl.Visibility = Visibility.Visible
            YValueControl.Visibility = Visibility.Collapsed
            InterceptControl.Visibility = Visibility.Collapsed
            SlopeControl.Visibility = Visibility.Collapsed

        ElseIf CType(CType(LineTypeControl.InnerContent, ComboBox).SelectedItem, LineAnnotationType) = LineAnnotationType.LinearEquation Then
            XValueControl.Visibility = Visibility.Collapsed
            YValueControl.Visibility = Visibility.Collapsed
            InterceptControl.Visibility = Visibility.Visible
            SlopeControl.Visibility = Visibility.Visible

        End If

    End Sub



End Class
Public Class DataPointToPointConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNothing(value) Then Return value
        If value.GetType <> GetType(DataPoint) Then Return Nothing
        Dim dp As DataPoint = DirectCast(value, DataPoint)
        '
        Return New Point(dp.X, dp.Y)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNothing(value) Then Return value
        If value.GetType <> GetType(Point) Then Return Nothing
        Dim p As Point = DirectCast(value, Point)
        '
        Return New DataPoint(p.X, p.Y)
    End Function
End Class
Public Class ScreenVectorToPointConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNothing(value) Then Return value
        If value.GetType <> GetType(ScreenVector) Then Return Nothing
        Dim sv As ScreenVector = DirectCast(value, ScreenVector)
        '
        Return New Point(sv.X, sv.Y)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNothing(value) Then Return value
        If value.GetType <> GetType(Point) Then Return Nothing
        Dim p As Point = DirectCast(value, Point)
        '
        Return New ScreenVector(p.X, p.Y)
    End Function
End Class

