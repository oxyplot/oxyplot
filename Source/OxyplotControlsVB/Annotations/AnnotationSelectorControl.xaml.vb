Imports System.Collections.Specialized
Imports OxyPlot
Imports OxyPlot.Wpf

Public Class AnnotationSelectorControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetDefaultComboboxStyle()
    End Sub

    Public Shared ReadOnly AnnotationsPropertiesTag As String = "Annotations"

    Public Shared PlotProperty As DependencyProperty = DependencyProperty.Register(NameOf(Plot), GetType(Plot), GetType(AnnotationSelectorControl), New PropertyMetadata(Nothing, AddressOf InitializePlot))
    Public Property Plot As Plot
        Get
            Return DirectCast(GetValue(PlotProperty), Plot)
        End Get
        Set(value As Plot)
            SetValue(PlotProperty, value)
        End Set
    End Property

    Public Shared SelectedAnnotationProperty As DependencyProperty = DependencyProperty.Register(NameOf(SelectedAnnotation), GetType(Annotation), GetType(AnnotationSelectorControl), New PropertyMetadata(Nothing))
    Public Property SelectedAnnotation As Annotation
        Get
            Return DirectCast(GetValue(SelectedAnnotationProperty), Annotation)
        End Get
        Set(value As Annotation)
            SetValue(SelectedAnnotationProperty, value)
        End Set
    End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(AnnotationSelectorControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property

    Public Shared ComboBoxStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ComboBoxStyle), GetType(Style), GetType(AnnotationSelectorControl), New PropertyMetadata(Nothing))

    Public Property ComboBoxStyle As Style
        Get
            Return DirectCast(GetValue(ComboBoxStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ComboBoxStyleProperty, value)
        End Set
    End Property

    Private Sub SetDefaultComboboxStyle()
        ComboBoxStyle = CType(FindResource("CleanComboBoxStyle"), Style)
    End Sub


    Private Shared Sub InitializePlot(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(AnnotationSelectorControl) Then Exit Sub
        Dim thisControl = DirectCast(d, AnnotationSelectorControl)
        '
        thisControl.AnnotationPropertyControlComboBox.ItemsSource = Nothing
        If IsNothing(e.NewValue) Then Exit Sub
        If e.NewValue.GetType <> GetType(Plot) Then Exit Sub
        Dim newPlot As Plot = DirectCast(e.NewValue, Plot)
        '
        thisControl.AddHandlers()
        thisControl.AnnotationPropertyControlComboBox.ItemsSource = newPlot.Annotations
        '
        thisControl.AnnotationPropertyControlComboBox.ApplyTemplate()
        Dim cntrl = GenericControls.FindElementByName(Of ItemsControl)(thisControl.AnnotationPropertyControlComboBox, "SpecialOptions")

        If cntrl IsNot Nothing AndAlso cntrl.Items.Count = 0 Then
            cntrl.Items.Add(New Separator())
            'Add arrow annotation
            Dim addArrow As New ComboBoxItem() With {.Content = "Add Arrow Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addArrow.PreviewMouseLeftButtonUp, Sub()
                                                              Dim newArrow As New ArrowAnnotation() With {.Text = "Arrow Annotation"}
                                                              thisControl.Plot.Annotations.Add(newArrow)
                                                              '
                                                              thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                              thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                              thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                              thisControl.AnnotationPropertiesControl.Focus()
                                                              '
                                                              Dim plotCenter As DataPoint
                                                              Dim plotArea As OxyRect = thisControl.Plot.ActualModel.PlotArea
                                                              plotCenter = newArrow.InternalAnnotation.InverseTransform(plotArea.Center)
                                                              Dim xShift = plotArea.Center.X + Math.Abs(plotArea.Right - plotArea.Left) * 0.1
                                                              Dim centerXShifted = newArrow.InternalAnnotation.InverseTransform(New ScreenPoint(xShift, plotArea.Center.Y))
                                                              '
                                                              newArrow.StartPoint = centerXShifted
                                                              newArrow.EndPoint = plotCenter
                                                          End Sub
            cntrl.Items.Add(addArrow)
            'Add text annotation
            Dim addText As New ComboBoxItem() With {.Content = "Add Text Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addText.PreviewMouseLeftButtonUp, Sub()
                                                             Dim newText As New TextAnnotation() With {.Text = "Text Annotation", .StrokeThickness = 0}
                                                             thisControl.Plot.Annotations.Add(newText)
                                                             '
                                                             thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                             thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                             thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                             thisControl.AnnotationPropertiesControl.Focus()
                                                             newText.TextPosition = newText.InternalAnnotation.InverseTransform(thisControl.Plot.ActualModel.PlotArea.Center)
                                                         End Sub
            cntrl.Items.Add(addText)
            '
            'Add line annotation
            Dim addLine As New ComboBoxItem() With {.Content = "Add Line Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addLine.PreviewMouseLeftButtonUp, Sub()
                                                             Dim newLine As New LineAnnotation() With {.Text = "Line Annotation"}
                                                             thisControl.Plot.Annotations.Add(newLine)
                                                             '
                                                             thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                             thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                             thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                             thisControl.AnnotationPropertiesControl.Focus()
                                                             '
                                                             Dim plotLL, plotUR, plotCenter As DataPoint
                                                             Dim plotArea As OxyRect = thisControl.Plot.ActualModel.PlotArea
                                                             plotLL = newLine.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                                                             plotUR = newLine.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                                                             plotCenter = newLine.InternalAnnotation.InverseTransform(plotArea.Center)

                                                             newLine.X = plotCenter.X
                                                             newLine.Y = plotCenter.Y
                                                             newLine.Type = Annotations.LineAnnotationType.LinearEquation
                                                             newLine.Intercept = plotCenter.Y
                                                             newLine.Slope = (plotUR.Y - plotLL.Y) / (plotUR.X - plotLL.X)
                                                         End Sub
            cntrl.Items.Add(addLine)
            '
            'Add rectangle annotation
            Dim addRect As New ComboBoxItem() With {.Content = "Add Rectangle Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addRect.PreviewMouseLeftButtonUp, Sub()
                                                             Dim newRect As New RectangleAnnotation() With {.Text = "Rectangle Annotation"}
                                                             thisControl.Plot.Annotations.Add(newRect)
                                                             '
                                                             thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                             thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                             thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                             thisControl.AnnotationPropertiesControl.Focus()
                                                             '
                                                             Dim plotLL, plotUR, plotCenter As DataPoint
                                                             Dim plotArea As OxyRect = thisControl.Plot.ActualModel.PlotArea
                                                             plotLL = newRect.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                                                             plotUR = newRect.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                                                             plotCenter = newRect.InternalAnnotation.InverseTransform(thisControl.Plot.ActualModel.PlotArea.Center)
                                                             Dim centerXShift As Double = Math.Abs((plotUR.X - plotLL.X) * 0.1)
                                                             Dim centerYShift As Double = Math.Abs((plotUR.Y - plotLL.Y) * 0.1)

                                                             newRect.MinimumX = plotCenter.X - centerXShift
                                                             newRect.MaximumX = plotCenter.X + centerXShift
                                                             newRect.MinimumY = plotCenter.Y - centerYShift
                                                             newRect.MaximumY = plotCenter.Y + centerYShift
                                                         End Sub
            cntrl.Items.Add(addRect)
            '
            'Add ellipse annotation
            Dim addEllipse As New ComboBoxItem() With {.Content = "Add Ellipse Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addEllipse.PreviewMouseLeftButtonUp, Sub()
                                                                Dim newEllipse As New EllipseAnnotation() With {.Text = "Ellipse Annotation"}
                                                                thisControl.Plot.Annotations.Add(newEllipse)
                                                                '
                                                                thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                                thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                                thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                                thisControl.AnnotationPropertiesControl.Focus()
                                                                '
                                                                Dim plotLL, plotUR, plotCenter As DataPoint
                                                                Dim plotArea As OxyRect = thisControl.Plot.ActualModel.PlotArea
                                                                plotLL = newEllipse.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                                                                plotUR = newEllipse.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                                                                plotCenter = newEllipse.InternalAnnotation.InverseTransform(thisControl.Plot.ActualModel.PlotArea.Center)
                                                                Dim centerXShift As Double = Math.Abs((plotUR.X - plotLL.X) * 0.1)
                                                                Dim centerYShift As Double = Math.Abs((plotUR.Y - plotLL.Y) * 0.1)

                                                                newEllipse.MinimumX = plotCenter.X - centerXShift
                                                                newEllipse.MaximumX = plotCenter.X + centerXShift
                                                                newEllipse.MinimumY = plotCenter.Y - centerYShift
                                                                newEllipse.MaximumY = plotCenter.Y + centerYShift
                                                            End Sub
            cntrl.Items.Add(addEllipse)
            '
            'Add point annotation
            Dim addPoint As New ComboBoxItem() With {.Content = "Add Point Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addPoint.PreviewMouseLeftButtonUp, Sub()
                                                              Dim newPoint As New PointAnnotation() With {.Text = "Point Annotation", .Size = 5}
                                                              thisControl.Plot.Annotations.Add(newPoint)
                                                              '
                                                              thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                              thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                              thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                              thisControl.AnnotationPropertiesControl.Focus()
                                                              '
                                                              Dim plotCenter = newPoint.InternalAnnotation.InverseTransform(thisControl.Plot.ActualModel.PlotArea.Center)
                                                              newPoint.X = plotCenter.X
                                                              newPoint.Y = plotCenter.Y
                                                          End Sub
            cntrl.Items.Add(addPoint)
            '
            'Add polygon annotation
            Dim addPolygon As New ComboBoxItem() With {.Content = "Add Polygon Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addPolygon.PreviewMouseLeftButtonUp, Sub()
                                                                Dim newPolygon As New PolygonAnnotation() With {.Text = "Polygon Annotation", .Points = New List(Of DataPoint)}
                                                                thisControl.Plot.Annotations.Add(newPolygon)
                                                                thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                                '
                                                                thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                                thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                                thisControl.AnnotationPropertiesControl.Focus()
                                                                '
                                                                Dim plotLL, plotUR As DataPoint
                                                                Dim plotArea As OxyRect = thisControl.Plot.ActualModel.PlotArea
                                                                plotArea = plotArea.Inflate(plotArea.Width * -0.25, plotArea.Height * -0.25)
                                                                plotLL = newPolygon.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                                                                plotUR = newPolygon.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                                                                '
                                                                newPolygon.Points.Add(plotLL)
                                                                newPolygon.Points.Add(New DataPoint(plotLL.X, plotUR.Y))
                                                                newPolygon.Points.Add(plotUR)
                                                                newPolygon.Points.Add(New DataPoint(plotUR.X, plotLL.Y))
                                                                thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                            End Sub
            cntrl.Items.Add(addPolygon)
            '
            'Add polyline annotation
            Dim addPolyline As New ComboBoxItem() With {.Content = "Add Polyline Annotation", .FontStyle = FontStyles.Italic}
            AddHandler addPolyline.PreviewMouseLeftButtonUp, Sub()
                                                                 Dim newPolyline As New PolylineAnnotation() With {.Text = "Polyline Annotation", .Points = New List(Of DataPoint)}
                                                                 thisControl.Plot.Annotations.Add(newPolyline)
                                                                 thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                                 '
                                                                 thisControl.AnnotationPropertyControlComboBox.SelectedItem = thisControl.Plot.Annotations.Last
                                                                 thisControl.AnnotationPropertyControlComboBox.IsDropDownOpen = False
                                                                 thisControl.AnnotationPropertiesControl.Focus()
                                                                 '
                                                                 Dim plotLL, plotUR, plotCenter As DataPoint
                                                                 Dim plotArea As OxyRect = thisControl.Plot.ActualModel.PlotArea
                                                                 plotArea = plotArea.Inflate(plotArea.Width * -0.25, plotArea.Height * -0.25)
                                                                 plotLL = newPolyline.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                                                                 plotUR = newPolyline.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                                                                 plotCenter = newPolyline.InternalAnnotation.InverseTransform(thisControl.Plot.ActualModel.PlotArea.Center)

                                                                 newPolyline.Points.Add(New DataPoint(plotCenter.X - plotLL.X, plotLL.Y))
                                                                 newPolyline.Points.Add(New DataPoint(plotCenter.X + plotUR.X, plotUR.Y))
                                                                 thisControl.Plot.ActualModel.InvalidatePlot(False)
                                                             End Sub
            cntrl.Items.Add(addPolyline)
            '

        End If
        '
        'If newPlot.Annotations.Count > 0 Then
        '    thisControl.AnnotationPropertyControlComboBox.SelectedIndex = 0
        '    thisControl.MaskTextBlock.Visibility = Visibility.Collapsed
        'End If

    End Sub

    ''' <summary>
    ''' Add required handlers. 
    ''' </summary>
    Private Sub AddHandlers()
        AddHandler Plot.Annotations.CollectionChanged, AddressOf Annotation_CollectionChanged
    End Sub

    ''' <summary>
    ''' When a new annotation is added or removed externally, update the control. 
    ''' </summary>
    Private Sub Annotation_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)

        ' New item was added. 
        If Not IsNothing(e.NewItems) Then
            'MaskTextBlock.Visibility = Visibility.Collapsed
            For Each newItem In e.NewItems
                AnnotationPropertyControlComboBox.SelectedItem = newItem
                AnnotationPropertiesControl.Annotation = TryCast(newItem, TextualAnnotation)
                Exit For
            Next
        End If

        ' Item was removed. 
        If Not IsNothing(e.OldItems) Then
            If Not IsNothing(Plot) AndAlso Plot.Annotations.Count = 0 Then
                ' MaskTextBlock.Visibility = Visibility.Visible
                AnnotationsControl.AnnotationPropertiesControl.HideExpanders()
            Else
                AnnotationPropertyControlComboBox.SelectedIndex = 0
            End If
        End If

    End Sub

    ''' <summary>
    ''' On selection changed, make sure selection is an annotation.
    ''' </summary>
    Private Sub AnnotationPropertyControlComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ' Early exit if nothing is selected.
        If IsNothing(AnnotationPropertyControlComboBox.SelectedItem) Then Exit Sub
        '
        Dim annotationToSelect As Wpf.TextualAnnotation = TryCast(AnnotationPropertyControlComboBox.SelectedItem, TextualAnnotation)
        If IsNothing(annotationToSelect) Then Exit Sub
        AnnotationPropertiesControl.Annotation = annotationToSelect
    End Sub

    ''' <summary>
    ''' Delete the selected annotation.
    ''' </summary>
    Private Sub DeleteAnnotationButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(AnnotationPropertyControlComboBox.SelectedItem) Then Exit Sub
        If IsNothing(Plot) Then Exit Sub
        If IsNothing(sender) Then Exit Sub
        If sender.GetType <> GetType(Button) Then Exit Sub
        Dim btn As Button = DirectCast(sender, Button)
        If IsNothing(btn.DataContext) Then Exit Sub
        '
        Dim annotationToDelete As Annotation = TryCast(btn.DataContext, Annotation)
        If IsNothing(annotationToDelete) Then Exit Sub
        '
        ' Delete Annotation
        Plot.Annotations.Remove(annotationToDelete)
        Plot.InvalidatePlot(False)
        '
        If Plot.Annotations.Count = 0 Then AnnotationPropertyControlComboBox.IsDropDownOpen = False
    End Sub

    Public Shared Function AnnotationsPropertiesToXElement(plot As Plot) As XElement
        Dim annotationProperties As New XElement(AnnotationsPropertiesTag)
        Dim textualAnnotation As TextualAnnotation
        For Each annotation In plot.Annotations
            textualAnnotation = TryCast(annotation, TextualAnnotation)
            If IsNothing(textualAnnotation) Then Continue For
            annotationProperties.Add(AnnotationControl.AnnotationPropertiesToXElement(textualAnnotation))
        Next
        '
        Return annotationProperties
    End Function
    '
    ''' <summary>
    ''' Load general plot property settings from XElement.
    ''' </summary>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Sub XElementToAnnotationsProperties(plot As Plot, element As XElement)
        'Early Exit
        If element.Name <> AnnotationsPropertiesTag Then Exit Sub
        'Set up the annotations
        plot.Annotations.Clear()
        Dim tempAnnotation As Annotation
        For Each el In element.Elements(AnnotationControl.AnnotationPropertiesTag)
            tempAnnotation = AnnotationControl.XElementToAnnotationProperties(el)
            If IsNothing(tempAnnotation) Then Continue For
            plot.Annotations.Add(tempAnnotation)
        Next
    End Sub
End Class