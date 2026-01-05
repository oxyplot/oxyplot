Imports System.Collections.Specialized
Imports System.Data
Imports GenericControls
Imports OxyPlot
Public Class OxyplotToolbar

#Region "Construction"

    ''' <summary>
    ''' Create new OxyPlot properties toolbar.
    ''' </summary>
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Set up custom cursors
        Using ms As New IO.MemoryStream(My.Resources.SelectPointCursor)
            _movePointsCursor = New Cursor(ms)
        End Using
        Using ms As New IO.MemoryStream(My.Resources.AddPointCursor)
            _addPointCursor = New Cursor(ms)
        End Using
        Using ms As New IO.MemoryStream(My.Resources.PanHand)
            _panHandCursor = New Cursor(ms)
        End Using
        Using ms As New IO.MemoryStream(My.Resources.PanHandClosed)
            _panHandClosedCursor = New Cursor(ms)
        End Using
        Using ms As New IO.MemoryStream(My.Resources.ZoomIn)
            _zoomCursor = New Cursor(ms)
        End Using
        ' Create leader line canvas and leader line
        _leaderLine.StrokeThickness = 2
        _leaderLine.Visibility = Visibility.Collapsed
        _leaderLine.Stroke = New SolidColorBrush(Colors.SkyBlue)
        _leaderLine.StrokeDashArray = New DoubleCollection(LineStyle.DashDashDot.GetDashArray)
        '
        _c.Children.Add(OxyToolBar._leaderLine)

    End Sub

#End Region

#Region "Members"

    ''' <summary>
    ''' Dependency property for the Plot property.
    ''' </summary>
    Public Shared PlotProperty As DependencyProperty = DependencyProperty.Register(NameOf(Plot), GetType(Wpf.Plot), GetType(OxyplotToolbar), New PropertyMetadata(Nothing, AddressOf InitializePlot))

    ''' <summary>
    ''' Gets and sets the OxyPlot Plot associated with this tool bar.
    ''' </summary>
    Public Property Plot As Wpf.Plot
        Get
            Return DirectCast(GetValue(PlotProperty), Wpf.Plot)
        End Get
        Set(value As Wpf.Plot)
            SetValue(PlotProperty, value)
        End Set
    End Property

    ''' <summary>
    ''' Property changed callback for the Plot property.
    ''' </summary>
    Private Shared Sub InitializePlot(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(OxyplotToolbar) Then Exit Sub
        Dim oxyToolBar = DirectCast(d, OxyplotToolbar)
        'If the variable 'd' is nothing then this could cause the previous plot to continue listening for events which could cause a memory leak. However, 'd' should always be a oxyplottoolbar.
        If Not IsNothing(e.OldValue) AndAlso e.OldValue.GetType = GetType(Wpf.Plot) Then
            Dim oldPlot As Wpf.Plot = DirectCast(e.OldValue, Wpf.Plot)
            RemoveHandler oldPlot.ActualModel.MouseDown, AddressOf OxyToolBar.PlotModelMouseDown
            RemoveHandler oldPlot.ActualModel.MouseMove, AddressOf OxyToolBar.PlotModelMouseMove
            RemoveHandler oldPlot.ActualModel.MouseUp, AddressOf OxyToolBar.PlotModelMouseUp
            RemoveHandler oldPlot.Annotations.CollectionChanged, AddressOf oxyToolBar.PlotModelAnnotationCollectionChanged
            'RemoveHandler oldPlot.Series.CollectionChanged, AddressOf oxyToolBar.PlotModelSeriesCollectionChanged
            RemoveHandler oldPlot.LayoutUpdated, AddressOf oxyToolBar.ToolBarLayoutUpdated
            '
            oldPlot.grid.Children.Remove(oxyToolBar._c)
        End If
        '
        If Not IsNothing(e.NewValue) AndAlso e.NewValue.GetType = GetType(Wpf.Plot) Then
            Dim newPlot As Wpf.Plot = DirectCast(e.NewValue, Wpf.Plot)
            'set up the mouse events
            AddHandler newPlot.ActualModel.MouseDown, AddressOf oxyToolBar.PlotModelMouseDown
            AddHandler newPlot.ActualModel.MouseMove, AddressOf oxyToolBar.PlotModelMouseMove
            AddHandler newPlot.ActualModel.MouseUp, AddressOf oxyToolBar.PlotModelMouseUp
            AddHandler newPlot.Annotations.CollectionChanged, AddressOf oxyToolBar.PlotModelAnnotationCollectionChanged
            'AddHandler newPlot.Series.CollectionChanged, AddressOf oxyToolBar.PlotModelSeriesCollectionChanged
            '
            newPlot.ApplyTemplate() 'Needed to set the canvas

            'Define the zooming cursor
            newPlot.ZoomHorizontalCursor = oxyToolBar._zoomCursor
            newPlot.ZoomRectangleCursor = oxyToolBar._zoomCursor
            newPlot.ZoomVerticalCursor = oxyToolBar._zoomCursor
            'Define the pan cursor
            newPlot.PanCursor = oxyToolBar._panHandCursor
            'Set up the mouse bindings
            If oxyToolBar.PointerButton.IsChecked Then oxyToolBar.PointerButton_Click(oxyToolBar, New RoutedEventArgs())
            If oxyToolBar.ZoomButton.IsChecked Then oxyToolBar.ZoomButton_Click(oxyToolBar, New RoutedEventArgs())
            If oxyToolBar.PanButton.IsChecked Then oxyToolBar.PanButton_Click(oxyToolBar, New RoutedEventArgs())
            '
            AddHandler newPlot.LayoutUpdated, AddressOf oxyToolBar.ToolBarLayoutUpdated
            '
            'Set up leader line for adding polyline and polygon annotations
            newPlot.grid.Children.Add(oxyToolBar._c)

            ''check to see if axes can be swapped
            'oxyToolBar.SwapAxesButton.Visibility = Visibility.Visible
            'For Each s In newPlot.Series
            '    If _nonSwapSeriesTypes.Contains(s.GetType()) Then
            '        oxyToolBar.SwapAxesButton.Visibility = Visibility.Collapsed
            '        Exit For
            '    End If
            'Next
        End If

    End Sub

    Private Sub ToolBarLayoutUpdated(sender As Object, eventArgs As EventArgs)
        With Plot.ActualModel
            If String.IsNullOrEmpty(Plot.Title) = False Then
                OxyToolBar.Margin = New Thickness(OxyToolBar.Margin.Left, .ActualPlotMargins.Top + .TitleArea.Bottom - .TitlePadding, OxyToolBar.Margin.Right, OxyToolBar.Margin.Bottom)
            Else
                OxyToolBar.Margin = New Thickness(OxyToolBar.Margin.Left, .ActualPlotMargins.Top + .Padding.Top, OxyToolBar.Margin.Right, OxyToolBar.Margin.Bottom)
            End If
        End With
    End Sub


    ''' <summary>
    ''' Dependency property for the icon property size. 
    ''' </summary>
    Public Shared IconSizeProperty As DependencyProperty = DependencyProperty.Register(NameOf(IconSize), GetType(Double), GetType(OxyplotToolbar), New PropertyMetadata(CDbl(20)))

    ''' <summary>
    ''' Gets and sets the tool bar icon size.
    ''' </summary>
    Public Property IconSize As Double
        Get
            Return CType(GetValue(IconSizeProperty), Double)
        End Get
        Set(value As Double)
            SetValue(IconSizeProperty, value)
        End Set
    End Property

    ''' <summary>
    ''' Dependency property for the tool bar orientation.
    ''' </summary>
    Public Shared ToolBarOrientationProperty As DependencyProperty = DependencyProperty.Register(NameOf(ToolBarOrientation), GetType(Orientation), GetType(OxyplotToolbar), New UIPropertyMetadata(Orientation.Vertical))

    ''' <summary>
    ''' Gets and sets the tool bar orientation.
    ''' </summary>
    Public Property ToolBarOrientation As Orientation
        Get
            Return DirectCast(GetValue(ToolBarOrientationProperty), Orientation)
        End Get
        Set(value As Orientation)
            SetValue(ToolBarOrientationProperty, value)
        End Set
    End Property

    Private _textBox As TextBox = Nothing
    Private _contextMenu As ContextMenu = Nothing

    ''' <summary>
    ''' Enumeration for adding annotation tool mode.
    ''' </summary>
    Public Enum AddToolMode
        None
        AddArrowAnnotation
        AddTextAnnotation
        AddRectangleAnnotation
        AddEllipseAnnotation
        AddPointAnnotation
        AddPolygonAnnotation
        AddPolylineAnnotation
        AddVerticalLineAnnotation
        AddHorizontalLineAnnotation
    End Enum

    ' Custom Cursors
    Private _movePointsCursor As Cursor
    Private _addPointCursor As Cursor
    Private _panHandCursor As Cursor
    Private _panHandClosedCursor As Cursor
    Private _zoomCursor As Cursor

    ' Edit Annotation variables
    ' Private _hackLine As Wpf.PolylineAnnotation
    Private _doubleClicked As Boolean = False
    Private _showPoints As Boolean = False
    Private _leaderLine As New Polyline
    Private _c As New Canvas
    Private _lastScreenPoint As ScreenPoint = ScreenPoint.Undefined
    Private _moveStartPoint As Boolean = False
    Private _moveEndPoint As Boolean = False
    Private _movePointIndex As Int32 = -1
    Private _scaleMaxX As Boolean = False
    Private _scaleMaxY As Boolean = False
    Private _scaleMinX As Boolean = False
    Private _scaleMinY As Boolean = False
    Private _originalColor As Color = Colors.White
    ' Adding Annotations
    Private _addAnnotationToolMode As AddToolMode = AddToolMode.None
    Private _targetAddAnnotation As Wpf.Annotation = Nothing

    ''' <summary>
    ''' Event indicating the plot properties need to be opened. 
    ''' </summary>
    ''' <param name="targetPlot">The plot who's properties need to be opened.</param>
    ''' <param name="openProperties">Boolean value indicating if plot properties should be opened or not.</param>
    ''' <param name="propertyExpander">The property expander that needs to be expanded.</param>
    ''' <param name="selectedObject">The selected plot object to edit.</param>
    Public Event PropertiesCalled(targetPlot As Wpf.Plot, openProperties As Boolean, propertyExpander As OxyplotPropertiesControl.PropertyEXP, selectedObject As Object)

    '''' <summary>
    '''' Event indicating that the plot properties needs to be closed. 
    '''' </summary>
    'Public Event ClosePropertiesCalled()

#End Region

#Region "Pan & Zoom"

    ''' <summary>
    ''' User clicked the pointer button.
    ''' </summary>
    Private Sub PointerButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        With Plot.ActualController
            .UnbindAll()
            .BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt)
            '.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack) 'no need to click
            .BindMouseDown(OxyMouseButton.Left, PlotCommands.SnapTrack)
            '.BindMouseDown(OxyMouseButton.Right, PlotCommands.SnapTrack) GTM - Removed to avoid issues when right clicking to select objects (track shows but doesn't disappear)
            .BindMouseWheel(PlotCommands.ZoomWheel)
            .BindKeyDown(OxyKey.Escape, PlotCommands.Reset)
        End With
        '
        SetCursor()
    End Sub

    ''' <summary>
    ''' User clicked the pan button. 
    ''' </summary>
    Private Sub PanButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        With Plot.ActualController
            .UnbindAll()
            .BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt)
            .BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt)
            .BindMouseDown(OxyMouseButton.Right, PlotCommands.SnapTrack)
            .BindMouseWheel(PlotCommands.ZoomWheel)
            .BindKeyDown(OxyKey.Escape, PlotCommands.Reset)
        End With
        '
        SetCursor()
    End Sub

    ''' <summary>
    ''' User clicked zoom button.
    ''' </summary>
    Private Sub ZoomButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        With Plot.ActualController
            .UnbindAll()
            .BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt)
            .BindMouseDown(OxyMouseButton.Left, PlotCommands.ZoomRectangle)
            .BindMouseDown(OxyMouseButton.Right, PlotCommands.SnapTrack)
            .BindMouseWheel(PlotCommands.ZoomWheel)
            .BindKeyDown(OxyKey.Escape, PlotCommands.Reset)
        End With
        '
        SetCursor()
    End Sub

    ''' <summary>
    ''' User clicked zoom to extents.
    ''' </summary>
    Private Sub ZoomAllButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        With Plot
            .ResetAllAxes()
            .InvalidatePlot(False)
            .Focus()
        End With
    End Sub

    ''' <summary>
    ''' Sets the mouse cursor based on the current tool mode. 
    ''' </summary>
    Private Sub SetCursor()
        If _addAnnotationToolMode <> AddToolMode.None Then
            Plot.DefaultPlotCursor = _addPointCursor
            Plot.Cursor = _addPointCursor
        ElseIf PanButton.IsChecked Then
            Plot.PanCursor = _panHandCursor
            Plot.DefaultPlotCursor = _panHandCursor
            Plot.Cursor = _panHandCursor
        ElseIf PointerButton.IsChecked Then
            Plot.DefaultPlotCursor = Cursors.Arrow
            Plot.Cursor = Cursors.Arrow
        ElseIf ZoomButton.IsChecked Then
            Plot.DefaultPlotCursor = _zoomCursor
            Plot.Cursor = _zoomCursor
        Else
            Plot.DefaultPlotCursor = Cursors.Arrow
            Plot.Cursor = Cursors.Arrow
        End If
    End Sub

#End Region

#Region "Annotations"

    ''' <summary>
    ''' When Add button is clicked, show context menu. 
    ''' </summary>
    Private Sub AddAnnotationToggleButton_Click(sender As Object, e As RoutedEventArgs)
        AddAnnotationToggleButton.ContextMenu.IsOpen = True
    End Sub

    ''' <summary>
    ''' Add arrow annotation. 
    ''' </summary>
    Private Sub AddArrowAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        Plot.ActualController.UnbindAll()
        _addAnnotationToolMode = AddToolMode.AddArrowAnnotation
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add text annotation.
    ''' </summary>
    Private Sub AddTextAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        Plot.ActualController.UnbindAll()
        _addAnnotationToolMode = AddToolMode.AddTextAnnotation
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add vertical line annotation.
    ''' </summary>
    Private Sub AddVerticalLineAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        Plot.ActualController.UnbindAll()
        _addAnnotationToolMode = AddToolMode.AddVerticalLineAnnotation
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add horizontal line annotation.
    ''' </summary>
    Private Sub AddHorizontalLineAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        Plot.ActualController.UnbindAll()
        _addAnnotationToolMode = AddToolMode.AddHorizontalLineAnnotation
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add rectangle annotation.
    ''' </summary>
    Private Sub AddRectangleAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        '
        Plot.ActualController.UnbindAll()
        _addAnnotationToolMode = AddToolMode.AddRectangleAnnotation
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add ellipse annotation.
    ''' </summary>
    Private Sub AddEllipseAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        Plot.ActualController.UnbindAll()
        _addAnnotationToolMode = AddToolMode.AddEllipseAnnotation
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add point annotation.
    ''' </summary>
    Private Sub AddPointAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        Plot.ActualController.UnbindAll()
        _addAnnotationToolMode = AddToolMode.AddPointAnnotation
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add polygon annotation.
    ''' </summary>
    Private Sub AddPolygonAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        _addAnnotationToolMode = AddToolMode.AddPolygonAnnotation
        _leaderLine.Visibility = Visibility.Visible
        _leaderLine.Points.Clear()
        SetCursor()
    End Sub

    ''' <summary>
    ''' Add polyline annotation.
    ''' </summary>
    Private Sub AddPolylineAnnotationItem_Click(sender As Object, e As RoutedEventArgs)
        StopAddAnnotation()
        _addAnnotationToolMode = AddToolMode.AddPolylineAnnotation
        _leaderLine.Visibility = Visibility.Visible
        _leaderLine.Points.Clear()
        SetCursor()
    End Sub

    ''' <summary>
    ''' Stop adding the annotation.
    ''' </summary>
    Private Sub StopAddAnnotation()
        If _addAnnotationToolMode <> AddToolMode.None Then
            If _addAnnotationToolMode = AddToolMode.AddPolygonAnnotation Or _addAnnotationToolMode = AddToolMode.AddPolylineAnnotation Then
                _leaderLine.Visibility = Visibility.Collapsed
                _leaderLine.Points.Clear()
                Plot.InvalidatePlot(False)
            End If
            '
            _doubleClicked = False
            _addAnnotationToolMode = AddToolMode.None
            _targetAddAnnotation = Nothing
            If PanButton.IsChecked Then
                PanButton_Click(Nothing, Nothing)
            ElseIf PointerButton.IsChecked Then
                PointerButton_Click(Nothing, Nothing)
            ElseIf ZoomButton.IsChecked Then
                ZoomButton_Click(Nothing, Nothing)
            End If
            SetCursor()

        End If
    End Sub

    '''' <summary>
    '''' A new series has been added to the plot. If the series types are supported then flip axes will be visible.
    '''' </summary>
    'Private Sub PlotModelSeriesCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
    '    If Plot Is Nothing OrElse Plot.Series Is Nothing Then Exit Sub
    '    SwapAxesButton.Visibility = Visibility.Visible
    '    For Each s In Plot.Series
    '        If _nonSwapSeriesTypes.Contains(s.GetType()) Then
    '            SwapAxesButton.Visibility = Visibility.Collapsed
    '            Exit For
    '        End If
    '    Next
    'End Sub

    ''' <summary>
    ''' A new annotation has been added to the plot. Adds the appropriate handlers. 
    ''' </summary>
    Private Sub PlotModelAnnotationCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        'Set up any new items that have been added to the collection
        If Not IsNothing(e.NewItems) Then
            For Each item In e.NewItems
                'If item.Equals(_editLineAnnotation) Then Continue For
                '
                Select Case item.GetType
                    Case GetType(Wpf.ArrowAnnotation)
                        Dim newArrow = DirectCast(item, Wpf.ArrowAnnotation)

                        AddHandler newArrow.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                              If newArrow.IsEnabled = False Then Exit Sub
                                                                              If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                              If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                              _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                              _moveStartPoint = ae.HitTestResult.Index <> 2
                                                                              _moveEndPoint = ae.HitTestResult.Index <> 1
                                                                              _originalColor = newArrow.Color
                                                                              newArrow.Color = Colors.Red
                                                                              '
                                                                              ' Left click handler must be injected here, because of ae.handled below
                                                                              GetSelectedObjects(s, ae)
                                                                              '
                                                                              Plot.ActualModel.InvalidatePlot(False)
                                                                              ae.Handled = True
                                                                          End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newArrow.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                              If newArrow.IsEnabled = False Then Exit Sub
                                                                              ' Compute the change in mouse movement in screen pixel space.
                                                                              Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                              Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                              Dim startScreenPoint = newArrow.InternalAnnotation.Transform(New DataPoint(newArrow.StartPoint.X, newArrow.StartPoint.Y))
                                                                              Dim endScreenPoint = newArrow.InternalAnnotation.Transform(New DataPoint(newArrow.EndPoint.X, newArrow.EndPoint.Y))
                                                                              ' Transform back to a data point in axis space.
                                                                              Dim startDataPoint = newArrow.InternalAnnotation.InverseTransform(New ScreenPoint(startScreenPoint.X + dx, startScreenPoint.Y + dy))
                                                                              Dim endDataPoint = newArrow.InternalAnnotation.InverseTransform(New ScreenPoint(endScreenPoint.X + dx, endScreenPoint.Y + dy))
                                                                              ' Set annotation position.
                                                                              If _moveStartPoint Then newArrow.StartPoint = startDataPoint
                                                                              If _moveEndPoint Then newArrow.EndPoint = endDataPoint
                                                                              ' Keep track of the last screen point.
                                                                              _lastScreenPoint = ae.Position
                                                                              '
                                                                              Plot.ActualModel.InvalidatePlot(False)
                                                                              ae.Handled = True
                                                                          End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newArrow.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                            If newArrow.IsEnabled = False Then Exit Sub
                                                                            newArrow.Color = _originalColor
                                                                        End Sub

                    Case GetType(Wpf.TextAnnotation)
                        Dim newText = DirectCast(item, Wpf.TextAnnotation)

                        AddHandler newText.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                             If newText.IsEnabled = False Then Exit Sub
                                                                             If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                             If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                             _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                             _moveStartPoint = (ae.HitTestResult.Index = 0)
                                                                             _originalColor = newText.Background
                                                                             newText.Background = Colors.Red
                                                                             '
                                                                             ' Left click handler must be injected here, because of ae.handled below
                                                                             GetSelectedObjects(s, ae)
                                                                             '
                                                                             Plot.ActualModel.InvalidatePlot(False)
                                                                             ae.Handled = True
                                                                         End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newText.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                             If newText.IsEnabled = False Then Exit Sub
                                                                             ' Compute the change in mouse movement in screen pixel space.
                                                                             Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                             Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                             Dim theScreenPoint = newText.InternalAnnotation.Transform(New DataPoint(newText.TextPosition.X, newText.TextPosition.Y))
                                                                             ' Transform back to a data point in axis space.
                                                                             Dim theDataPoint = newText.InternalAnnotation.InverseTransform(New ScreenPoint(theScreenPoint.X + dx, theScreenPoint.Y + dy))
                                                                             ' Set annotation position.
                                                                             If _moveStartPoint Then newText.TextPosition = theDataPoint
                                                                             ' Keep track of the last screen point.
                                                                             _lastScreenPoint = ae.Position
                                                                             '
                                                                             Plot.ActualModel.InvalidatePlot(False)
                                                                             ae.Handled = True
                                                                         End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newText.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                           If newText.IsEnabled = False Then Exit Sub
                                                                           newText.Background = _originalColor
                                                                       End Sub

                    Case GetType(Wpf.RectangleAnnotation)
                        Dim newRect = DirectCast(item, Wpf.RectangleAnnotation)

                        AddHandler newRect.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                             If newRect.IsEnabled = False Then Exit Sub
                                                                             If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                             If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                             _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                             Dim upperRight As ScreenPoint = newRect.InternalAnnotation.Transform(newRect.MaximumX, newRect.MaximumY)
                                                                             Dim lowerLeft As ScreenPoint = newRect.InternalAnnotation.Transform(newRect.MinimumX, newRect.MinimumY)
                                                                             Dim topRight As New ScreenPoint(Math.Abs(upperRight.X - ae.Position.X), Math.Abs(upperRight.Y - ae.Position.Y))
                                                                             Dim bottomLeft As New ScreenPoint(Math.Abs(lowerLeft.X - ae.Position.X), Math.Abs(lowerLeft.Y - ae.Position.Y))
                                                                             ' edges
                                                                             _scaleMaxX = topRight.X < 10
                                                                             _scaleMaxY = topRight.Y < 10
                                                                             _scaleMinX = bottomLeft.X < 10
                                                                             _scaleMinY = bottomLeft.Y < 10
                                                                             '
                                                                             ' all
                                                                             If ae.HitTestResult.Index = 0 Then
                                                                                 _moveStartPoint = (_scaleMaxX = False AndAlso _scaleMaxY = False AndAlso _scaleMinX = False AndAlso _scaleMinY = False)
                                                                             End If
                                                                             '
                                                                             _originalColor = newRect.Fill
                                                                             newRect.Fill = Colors.Red
                                                                             '
                                                                             ' Left click handler must be injected here, because of ae.handled below
                                                                             GetSelectedObjects(s, ae)
                                                                             '
                                                                             Plot.ActualModel.InvalidatePlot(False)
                                                                             ae.Handled = True
                                                                         End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newRect.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                             If newRect.IsEnabled = False Then Exit Sub
                                                                             ' Compute the change in mouse movement in screen pixel space.
                                                                             Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                             Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                             Dim upperRightScreenPoint = newRect.InternalAnnotation.Transform(newRect.MaximumX, newRect.MaximumY) '
                                                                             Dim lowerLeftScreenPoint = newRect.InternalAnnotation.Transform(newRect.MinimumX, newRect.MinimumY)
                                                                             ' Transform back to a data point in axis space.
                                                                             Dim upperRightDataPoint = newRect.InternalAnnotation.InverseTransform(New ScreenPoint(upperRightScreenPoint.X + dx, upperRightScreenPoint.Y + dy))
                                                                             Dim lowerLeftDataPoint = newRect.InternalAnnotation.InverseTransform(New ScreenPoint(lowerLeftScreenPoint.X + dx, lowerLeftScreenPoint.Y + dy))
                                                                             ' Set annotation position.
                                                                             If _scaleMaxX Then newRect.MaximumX = upperRightDataPoint.X
                                                                             If _scaleMaxY Then newRect.MaximumY = upperRightDataPoint.Y
                                                                             If _scaleMinX Then newRect.MinimumX = lowerLeftDataPoint.X
                                                                             If _scaleMinY Then newRect.MinimumY = lowerLeftDataPoint.Y
                                                                             '
                                                                             If _moveStartPoint Then
                                                                                 newRect.MaximumX = upperRightDataPoint.X
                                                                                 newRect.MaximumY = upperRightDataPoint.Y
                                                                                 newRect.MinimumX = lowerLeftDataPoint.X
                                                                                 newRect.MinimumY = lowerLeftDataPoint.Y
                                                                             End If
                                                                             ' Keep track of the last screen point.
                                                                             _lastScreenPoint = ae.Position
                                                                             '
                                                                             Plot.ActualModel.InvalidatePlot(False)
                                                                             ae.Handled = True
                                                                         End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newRect.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                           If newRect.IsEnabled = False Then Exit Sub
                                                                           newRect.Fill = _originalColor
                                                                       End Sub

                    Case GetType(Wpf.EllipseAnnotation)
                        Dim newEllipse = DirectCast(item, Wpf.EllipseAnnotation)

                        AddHandler newEllipse.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                                If newEllipse.IsEnabled = False Then Exit Sub
                                                                                If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                                If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                                _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                                Dim upperRight As ScreenPoint = newEllipse.InternalAnnotation.Transform(newEllipse.MaximumX, newEllipse.MaximumY)
                                                                                Dim lowerLeft As ScreenPoint = newEllipse.InternalAnnotation.Transform(newEllipse.MinimumX, newEllipse.MinimumY)
                                                                                Dim topRight As New ScreenPoint(Math.Abs(upperRight.X - ae.Position.X), Math.Abs(upperRight.Y - ae.Position.Y))
                                                                                Dim bottomLeft As New ScreenPoint(Math.Abs(lowerLeft.X - ae.Position.X), Math.Abs(lowerLeft.Y - ae.Position.Y))
                                                                                ' edges
                                                                                _scaleMaxX = topRight.X < 10
                                                                                _scaleMaxY = topRight.Y < 10
                                                                                _scaleMinX = bottomLeft.X < 10
                                                                                _scaleMinY = bottomLeft.Y < 10
                                                                                '
                                                                                ' all
                                                                                If ae.HitTestResult.Index = 0 Then
                                                                                    _moveStartPoint = (_scaleMaxX = False AndAlso _scaleMaxY = False AndAlso _scaleMinX = False AndAlso _scaleMinY = False)
                                                                                End If
                                                                                '
                                                                                _originalColor = newEllipse.Fill
                                                                                newEllipse.Fill = Colors.Red
                                                                                '
                                                                                ' Left click handler must be injected here, because of ae.handled below
                                                                                GetSelectedObjects(s, ae)
                                                                                '
                                                                                Plot.ActualModel.InvalidatePlot(False)
                                                                                ae.Handled = True
                                                                            End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newEllipse.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                                If newEllipse.IsEnabled = False Then Exit Sub
                                                                                ' Compute the change in mouse movement in screen pixel space.
                                                                                Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                                Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                                Dim upperRightScreenPoint = newEllipse.InternalAnnotation.Transform(newEllipse.MaximumX, newEllipse.MaximumY) '
                                                                                Dim lowerLeftScreenPoint = newEllipse.InternalAnnotation.Transform(newEllipse.MinimumX, newEllipse.MinimumY)
                                                                                ' Transform back to a data point in axis space.
                                                                                Dim upperRightDataPoint = newEllipse.InternalAnnotation.InverseTransform(New ScreenPoint(upperRightScreenPoint.X + dx, upperRightScreenPoint.Y + dy))
                                                                                Dim lowerLeftDataPoint = newEllipse.InternalAnnotation.InverseTransform(New ScreenPoint(lowerLeftScreenPoint.X + dx, lowerLeftScreenPoint.Y + dy))
                                                                                ' Set annotation position.
                                                                                If _scaleMaxX Then newEllipse.MaximumX = upperRightDataPoint.X
                                                                                If _scaleMaxY Then newEllipse.MaximumY = upperRightDataPoint.Y
                                                                                If _scaleMinX Then newEllipse.MinimumX = lowerLeftDataPoint.X
                                                                                If _scaleMinY Then newEllipse.MinimumY = lowerLeftDataPoint.Y
                                                                                '
                                                                                If _moveStartPoint Then
                                                                                    newEllipse.MaximumX = upperRightDataPoint.X
                                                                                    newEllipse.MaximumY = upperRightDataPoint.Y
                                                                                    newEllipse.MinimumX = lowerLeftDataPoint.X
                                                                                    newEllipse.MinimumY = lowerLeftDataPoint.Y
                                                                                End If
                                                                                ' Keep track of the last screen point.
                                                                                _lastScreenPoint = ae.Position
                                                                                '
                                                                                Plot.ActualModel.InvalidatePlot(False)
                                                                                ae.Handled = True
                                                                            End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newEllipse.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                              If newEllipse.IsEnabled = False Then Exit Sub
                                                                              newEllipse.Fill = _originalColor
                                                                          End Sub

                    Case GetType(Wpf.PointAnnotation)
                        Dim newPoint = DirectCast(item, Wpf.PointAnnotation)

                        AddHandler newPoint.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                              If newPoint.IsEnabled = False Then Exit Sub
                                                                              If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                              If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                              _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                              _moveStartPoint = (ae.HitTestResult.Index = 0)
                                                                              _originalColor = newPoint.Fill
                                                                              newPoint.Fill = Colors.Red
                                                                              '
                                                                              ' Left click handler must be injected here, because of ae.handled below
                                                                              GetSelectedObjects(s, ae)
                                                                              '
                                                                              Plot.ActualModel.InvalidatePlot(False)
                                                                              ae.Handled = True
                                                                          End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newPoint.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                              If newPoint.IsEnabled = False Then Exit Sub
                                                                              ' Compute the change in mouse movement in screen pixel space.
                                                                              Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                              Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                              Dim theScreenPoint = newPoint.InternalAnnotation.Transform(New DataPoint(newPoint.X, newPoint.Y))
                                                                              ' Transform back to a data point in axis space.
                                                                              Dim theDataPoint = newPoint.InternalAnnotation.InverseTransform(New ScreenPoint(theScreenPoint.X + dx, theScreenPoint.Y + dy))
                                                                              ' Set annotation position.
                                                                              If _moveStartPoint Then
                                                                                  newPoint.X = theDataPoint.X
                                                                                  newPoint.Y = theDataPoint.Y
                                                                              End If
                                                                              ' Keep track of the last screen point.
                                                                              _lastScreenPoint = ae.Position
                                                                              '
                                                                              Plot.ActualModel.InvalidatePlot(False)
                                                                              ae.Handled = True
                                                                          End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newPoint.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                            If newPoint.IsEnabled = False Then Exit Sub
                                                                            newPoint.Fill = _originalColor
                                                                        End Sub

                    Case GetType(Wpf.PolygonAnnotation)
                        Dim newPolygon = DirectCast(item, Wpf.PolygonAnnotation)

                        AddHandler newPolygon.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                                If newPolygon.IsEnabled = False Then Exit Sub
                                                                                If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                                If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                                _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                                Dim screenToData = newPolygon.InternalAnnotation.InverseTransform(ae.Position)
                                                                                Dim screen2ToData = newPolygon.InternalAnnotation.InverseTransform(New ScreenPoint(ae.Position.X - 10, ae.Position.Y - 10))
                                                                                Dim dxy = New DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y))
                                                                                Dim dPoint = newPolygon.InternalAnnotation.InverseTransform(ae.Position)
                                                                                ' Need to check if the cursor is over any points in the annotation point data
                                                                                _movePointIndex = -1
                                                                                For i As Int32 = 0 To newPolygon.Points.Count - 1
                                                                                    If Math.Abs(dPoint.X - newPolygon.Points(i).X) < dxy.X AndAlso Math.Abs(dPoint.Y - newPolygon.Points(i).Y) < dxy.Y Then
                                                                                        _movePointIndex = i
                                                                                        Exit For
                                                                                    End If
                                                                                Next
                                                                                ' Check if cursor is on an edge
                                                                                If _movePointIndex = -1 Then
                                                                                    Dim p1, p2, linePoint As ScreenPoint
                                                                                    Dim onLine As Boolean = False
                                                                                    For i As Int32 = 0 To newPolygon.Points.Count - 2
                                                                                        p1 = newPolygon.InternalAnnotation.Transform(newPolygon.Points(i))
                                                                                        p2 = newPolygon.InternalAnnotation.Transform(newPolygon.Points(i + 1))
                                                                                        linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2)
                                                                                        If (linePoint - ae.Position).Length < 10 Then
                                                                                            newPolygon.Points.Insert(i + 1, newPolygon.InternalAnnotation.InverseTransform(linePoint))
                                                                                            onLine = True
                                                                                            _movePointIndex = i + 1
                                                                                            Exit For
                                                                                        End If
                                                                                    Next
                                                                                    '
                                                                                    If onLine = False Then
                                                                                        ' check between first and last point
                                                                                        p1 = newPolygon.InternalAnnotation.Transform(newPolygon.Points(0))
                                                                                        p2 = newPolygon.InternalAnnotation.Transform(newPolygon.Points(newPolygon.Points.Count - 1))
                                                                                        linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2)
                                                                                        If (linePoint - ae.Position).Length < 10 Then
                                                                                            newPolygon.Points.Add(newPolygon.InternalAnnotation.InverseTransform(linePoint))
                                                                                            onLine = True
                                                                                            _movePointIndex = newPolygon.Points.Count - 1
                                                                                        Else
                                                                                            If ae.HitTestResult.Index = 0 Then _moveStartPoint = True
                                                                                        End If
                                                                                    End If
                                                                                End If
                                                                                '
                                                                                _originalColor = newPolygon.Fill
                                                                                newPolygon.Fill = Colors.Red
                                                                                '
                                                                                ' Left click handler must be injected here, because of ae.handled below
                                                                                GetSelectedObjects(s, ae)
                                                                                '
                                                                                Plot.ActualModel.InvalidatePlot(False)
                                                                                ae.Handled = True
                                                                            End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newPolygon.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                                If newPolygon.IsEnabled = False Then Exit Sub
                                                                                ' Compute the change in mouse movement in screen pixel space.
                                                                                Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                                Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                                ' Need to check if the cursor is over any points in the annotation point data
                                                                                If _movePointIndex > -1 Then
                                                                                    ' Compute the change in mouse movement in screen pixel space. Transform back to a data point in axis space.
                                                                                    Dim screenPoint = newPolygon.InternalAnnotation.Transform(New DataPoint(newPolygon.Points(_movePointIndex).X, newPolygon.Points(_movePointIndex).Y))
                                                                                    newPolygon.Points(_movePointIndex) = newPolygon.InternalAnnotation.InverseTransform(New ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy))
                                                                                Else
                                                                                    If _moveStartPoint Then
                                                                                        For i As Int32 = 0 To newPolygon.Points.Count - 1
                                                                                            ' Compute the change in mouse movement in screen pixel space. Transform back to a data point in axis space.
                                                                                            Dim screenPoint = newPolygon.InternalAnnotation.Transform(New DataPoint(newPolygon.Points(i).X, newPolygon.Points(i).Y))
                                                                                            newPolygon.Points(i) = newPolygon.InternalAnnotation.InverseTransform(New ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy))
                                                                                        Next
                                                                                    End If
                                                                                End If
                                                                                '
                                                                                ' Keep track of the last screen point.
                                                                                _lastScreenPoint = ae.Position
                                                                                '
                                                                                Plot.ActualModel.InvalidatePlot(False)
                                                                                ae.Handled = True
                                                                            End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newPolygon.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                              If newPolygon.IsEnabled = False Then Exit Sub
                                                                              newPolygon.Fill = _originalColor
                                                                          End Sub

                    Case GetType(Wpf.PolylineAnnotation)
                        Dim newPolyline = DirectCast(item, Wpf.PolylineAnnotation)

                        AddHandler newPolyline.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                                 If newPolyline.IsEnabled = False Then Exit Sub
                                                                                 If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                                 If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                                 _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                                 Dim screenToData = newPolyline.InternalAnnotation.InverseTransform(ae.Position)
                                                                                 Dim screen2ToData = newPolyline.InternalAnnotation.InverseTransform(New ScreenPoint(ae.Position.X - 10, ae.Position.Y - 10))
                                                                                 Dim dxy = New DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y))
                                                                                 Dim dPoint = newPolyline.InternalAnnotation.InverseTransform(ae.Position)
                                                                                 ' Need to check if the cursor is over any points in the annotation point data
                                                                                 _movePointIndex = -1
                                                                                 For i As Int32 = 0 To newPolyline.Points.Count - 1
                                                                                     If Math.Abs(dPoint.X - newPolyline.Points(i).X) < dxy.X AndAlso Math.Abs(dPoint.Y - newPolyline.Points(i).Y) < dxy.Y Then
                                                                                         _movePointIndex = i
                                                                                         Exit For
                                                                                     End If
                                                                                 Next
                                                                                 'Check if cursor is on an edge
                                                                                 Dim onLine As Boolean = False
                                                                                 If _movePointIndex = -1 AndAlso ae.IsControlDown = True Then
                                                                                     Dim p1, p2, linePoint As ScreenPoint
                                                                                     For i As Int32 = 0 To newPolyline.Points.Count - 2
                                                                                         p1 = newPolyline.InternalAnnotation.Transform(newPolyline.Points(i))
                                                                                         p2 = newPolyline.InternalAnnotation.Transform(newPolyline.Points(i + 1))
                                                                                         linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2)
                                                                                         If (linePoint - ae.Position).Length < 10 Then
                                                                                             newPolyline.Points.Insert(i + 1, newPolyline.InternalAnnotation.InverseTransform(linePoint))
                                                                                             onLine = True
                                                                                             _movePointIndex = i + 1
                                                                                             Exit For
                                                                                         End If
                                                                                     Next
                                                                                 End If
                                                                                 '
                                                                                 If onLine = False Then _moveStartPoint = True
                                                                                 '
                                                                                 _originalColor = newPolyline.Color
                                                                                 newPolyline.Color = Colors.Red
                                                                                 '
                                                                                 ' Left click handler must be injected here, because of ae.handled below
                                                                                 GetSelectedObjects(s, ae)
                                                                                 '
                                                                                 Plot.ActualModel.InvalidatePlot(False)
                                                                                 ae.Handled = True
                                                                             End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newPolyline.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                                 If newPolyline.IsEnabled = False Then Exit Sub
                                                                                 ' Compute the change in mouse movement in screen pixel space.
                                                                                 Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                                 Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                                 ' Need to check if the cursor is over any points in the annotation point data
                                                                                 If _movePointIndex > -1 Then
                                                                                     ' Compute the change in mouse movement in screen pixel space. Transform back to a data point in axis space.
                                                                                     Dim screenPoint = newPolyline.InternalAnnotation.Transform(New DataPoint(newPolyline.Points(_movePointIndex).X, newPolyline.Points(_movePointIndex).Y))
                                                                                     newPolyline.Points(_movePointIndex) = newPolyline.InternalAnnotation.InverseTransform(New ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy))
                                                                                 Else
                                                                                     If _moveStartPoint Then
                                                                                         For i As Int32 = 0 To newPolyline.Points.Count - 1
                                                                                             ' Compute the change in mouse movement in screen pixel space. Transform back to a data point in axis space.
                                                                                             Dim screenPoint = newPolyline.InternalAnnotation.Transform(New DataPoint(newPolyline.Points(i).X, newPolyline.Points(i).Y))
                                                                                             newPolyline.Points(i) = newPolyline.InternalAnnotation.InverseTransform(New ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy))
                                                                                         Next
                                                                                     End If
                                                                                 End If
                                                                                 '
                                                                                 ' Keep track of the last screen point.
                                                                                 _lastScreenPoint = ae.Position
                                                                                 '
                                                                                 Plot.ActualModel.InvalidatePlot(False)
                                                                                 ae.Handled = True
                                                                             End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newPolyline.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                               If newPolyline.IsEnabled = False Then Exit Sub
                                                                               newPolyline.Color = _originalColor
                                                                           End Sub

                    Case GetType(Wpf.LineAnnotation)
                        Dim newLine = DirectCast(item, Wpf.LineAnnotation)

                        AddHandler newLine.InternalAnnotation.MouseDown, Sub(s, ae)
                                                                             If newLine.IsEnabled = False Then Exit Sub
                                                                             If _addAnnotationToolMode <> AddToolMode.None Then Exit Sub
                                                                             If ae.ChangedButton <> OxyMouseButton.Left Then Exit Sub
                                                                             _lastScreenPoint = New ScreenPoint(ae.Position.X, ae.Position.Y)
                                                                             _moveStartPoint = (ae.HitTestResult.Index = 0)
                                                                             '
                                                                             _originalColor = newLine.Color
                                                                             newLine.Color = Colors.Red
                                                                             '
                                                                             ' Left click handler must be injected here, because of ae.handled below
                                                                             GetSelectedObjects(s, ae)

                                                                             ' Now set closed pan hand if needed
                                                                             If ((Mouse.LeftButton = MouseButtonState.Pressed And PanButton.IsChecked) Or Mouse.MiddleButton = MouseButtonState.Pressed) Then
                                                                                 Plot.PanCursor = _panHandClosedCursor
                                                                                 Plot.DefaultPlotCursor = _panHandClosedCursor
                                                                                 Plot.Cursor = _panHandClosedCursor
                                                                             End If

                                                                             OpenLineAnnotationTooltip(newLine)
                                                                             UpdateLineAnnotationTooltip(newLine)
                                                                             Plot.ActualModel.InvalidatePlot(False)
                                                                             ae.Handled = True
                                                                         End Sub

                        'Handle mouse movements (note: this is only called when the mousedown event was handled)
                        AddHandler newLine.InternalAnnotation.MouseMove, Sub(s, ae)
                                                                             If newLine.IsEnabled = False Then Exit Sub
                                                                             ' Compute the change in mouse movement in screen pixel space.
                                                                             Dim dx As Double = ae.Position.X - _lastScreenPoint.X
                                                                             Dim dy As Double = ae.Position.Y - _lastScreenPoint.Y
                                                                             Dim screenPoint = newLine.InternalAnnotation.Transform(New DataPoint(newLine.X, newLine.Y))
                                                                             ' Transform back to a data point in axis space.
                                                                             Dim dataPoint = newLine.InternalAnnotation.InverseTransform(New ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy))
                                                                             ' Set annotation position.
                                                                             If _moveStartPoint Then
                                                                                 If newLine.Type = Annotations.LineAnnotationType.LinearEquation Then
                                                                                     dx = dataPoint.X - newLine.X
                                                                                     dy = dataPoint.Y - newLine.Y
                                                                                     newLine.Intercept += (dy - newLine.Slope * dx)
                                                                                 Else
                                                                                     newLine.X = dataPoint.X
                                                                                     newLine.Y = dataPoint.Y
                                                                                 End If
                                                                                 UpdateLineAnnotationTooltip(newLine)
                                                                             End If
                                                                             '
                                                                             ' Keep track of the last screen point.
                                                                             _lastScreenPoint = ae.Position
                                                                             '
                                                                             Plot.ActualModel.InvalidatePlot(False)
                                                                             ae.Handled = True
                                                                         End Sub

                        'Handle mouse up (note: this is only called when the mousedown event was handled)
                        AddHandler newLine.InternalAnnotation.MouseUp, Sub(s, ae)
                                                                           If newLine.IsEnabled = False Then Exit Sub
                                                                           newLine.Color = _originalColor
                                                                           CloseLineAnnotationTooltip(newLine)
                                                                       End Sub

                End Select
            Next

        End If
        '
    End Sub

    ''' <summary>
    ''' Open the line annotation tool tip. 
    ''' </summary>
    ''' <param name="lineAnnotation">The line annotation.</param>
    Private Sub OpenLineAnnotationTooltip(lineAnnotation As Wpf.LineAnnotation)
        ' Close any previous tooltips
        If lineAnnotation.ToolTip IsNot Nothing Then
            CType(lineAnnotation.ToolTip, ToolTip).IsOpen = False
        End If
        ' Create new tooltip
        Dim toolTip As New ToolTip()
        toolTip.FontFamily = Plot.FontFamily
        toolTip.FontSize = Plot.FontSize
        toolTip.FontWeight = Plot.FontWeight
        toolTip.Background = Brushes.White
        toolTip.BorderBrush = Brushes.Transparent
        toolTip.Placement = Primitives.PlacementMode.Relative
        toolTip.PlacementTarget = Plot.canvas
        toolTip.Padding = New Thickness(1)
        toolTip.Margin = New Thickness(0)
        toolTip.IsOpen = True
        lineAnnotation.ToolTip = toolTip
    End Sub

    ''' <summary>
    ''' Update the line annotation tool tip. 
    ''' </summary>
    ''' <param name="lineAnnotation">The line annotation.</param>
    Private Sub UpdateLineAnnotationTooltip(lineAnnotation As Wpf.LineAnnotation)
        ' Update tooltip value
        Select Case lineAnnotation.Type
            Case Annotations.LineAnnotationType.Horizontal
                If lineAnnotation.ToolTip IsNot Nothing Then
                    Dim dataPoint As DataPoint
                    If lineAnnotation.InternalAnnotation.XAxis.IsReversed = False Then
                        dataPoint = New DataPoint(lineAnnotation.InternalAnnotation.XAxis.ActualMinimum, lineAnnotation.Y)
                    Else
                        dataPoint = New DataPoint(lineAnnotation.InternalAnnotation.XAxis.ActualMaximum, lineAnnotation.Y)
                    End If
                    Dim toolTip As ToolTip = CType(lineAnnotation.ToolTip, ToolTip)
                    toolTip.Content = lineAnnotation.InternalAnnotation.YAxis.FormatValue(lineAnnotation.Y)
                    toolTip.UpdateLayout()
                    toolTip.VerticalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).Y - toolTip.ActualHeight / 2
                    toolTip.HorizontalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).X - toolTip.ActualWidth
                End If
                Exit Select
            Case Annotations.LineAnnotationType.Vertical
                If lineAnnotation.ToolTip IsNot Nothing Then
                    Dim dataPoint As DataPoint
                    If lineAnnotation.InternalAnnotation.YAxis.IsReversed = False Then
                        dataPoint = New DataPoint(lineAnnotation.X, lineAnnotation.InternalAnnotation.YAxis.ActualMinimum)
                    Else
                        dataPoint = New DataPoint(lineAnnotation.X, lineAnnotation.InternalAnnotation.YAxis.ActualMaximum)
                    End If
                    Dim toolTip As ToolTip = CType(lineAnnotation.ToolTip, ToolTip)
                    toolTip.Content = lineAnnotation.InternalAnnotation.XAxis.FormatValue(lineAnnotation.X)
                    toolTip.UpdateLayout()
                    toolTip.VerticalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).Y
                    toolTip.HorizontalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).X - toolTip.ActualWidth / 2
                End If
                Exit Select
        End Select
    End Sub

    ''' <summary>
    ''' Close the line annotation tool tip. 
    ''' </summary>
    ''' <param name="lineAnnotation">The line annotation.</param>
    Private Sub CloseLineAnnotationTooltip(lineAnnotation As Wpf.LineAnnotation)
        If lineAnnotation.ToolTip IsNot Nothing Then
            CType(lineAnnotation.ToolTip, ToolTip).IsOpen = False
        End If
    End Sub

#End Region

#Region "Mouse Events"

    ''' <summary>
    ''' Determines the behavior for the mouse down event. 
    ''' </summary>
    Private Sub PlotModelMouseDown(sender As Object, e As OxyMouseDownEventArgs)
        If _addAnnotationToolMode <> AddToolMode.None Then
            Select Case _addAnnotationToolMode
                Case AddToolMode.AddArrowAnnotation
                    Dim newArrow As New Wpf.ArrowAnnotation() With {.Text = "Arrow Annotation"}
                    Plot.Annotations.Add(newArrow)
                    Plot.ActualModel.InvalidatePlot(False)
                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newArrow)
                    '
                    newArrow.StartPoint = newArrow.InternalAnnotation.InverseTransform(e.Position)
                    newArrow.EndPoint = newArrow.StartPoint
                    _targetAddAnnotation = newArrow

                Case AddToolMode.AddTextAnnotation
                    Dim newText As New Wpf.TextAnnotation() With {.Text = "Text Annotation"}
                    Plot.Annotations.Add(newText)
                    Plot.ActualModel.InvalidatePlot(False)
                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newText)
                    '
                    newText.TextPosition = newText.InternalAnnotation.InverseTransform(e.Position)
                    _targetAddAnnotation = newText

                Case AddToolMode.AddVerticalLineAnnotation
                    Dim newLine As New Wpf.LineAnnotation() With {.Text = "Vertical Line Annotation"}
                    Plot.Annotations.Add(newLine)
                    Plot.ActualModel.InvalidatePlot(False)
                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newLine)
                    '
                    If newLine.InternalAnnotation.YAxis.IsReversed = False Then
                        newLine.TextLinePosition = 1
                        newLine.TextHorizontalAlignment = Windows.HorizontalAlignment.Right
                    Else
                        newLine.TextLinePosition = 0
                        newLine.TextHorizontalAlignment = Windows.HorizontalAlignment.Left
                    End If
                    '
                    Dim plotArea As OxyRect = Plot.ActualModel.PlotArea
                    Dim plotLL = newLine.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                    Dim plotUR = newLine.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                    Dim dataPointClicked = newLine.InternalAnnotation.InverseTransform(e.Position)
                    newLine.X = dataPointClicked.X
                    newLine.Y = dataPointClicked.Y
                    newLine.Type = Annotations.LineAnnotationType.Vertical
                    newLine.Intercept = dataPointClicked.Y
                    newLine.Slope = (plotUR.Y - plotLL.Y) / (plotUR.X - plotLL.X)
                    _targetAddAnnotation = newLine

                    OpenLineAnnotationTooltip(newLine)
                    UpdateLineAnnotationTooltip(newLine)
                    Plot.ActualModel.InvalidatePlot(False)

                Case AddToolMode.AddHorizontalLineAnnotation
                    Dim newLine As New Wpf.LineAnnotation() With {.Text = "Horizontal Line Annotation"}
                    Plot.Annotations.Add(newLine)
                    Plot.ActualModel.InvalidatePlot(False)
                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newLine)
                    '
                    If newLine.InternalAnnotation.XAxis.IsReversed = False Then
                        newLine.TextLinePosition = 0
                        newLine.TextHorizontalAlignment = Windows.HorizontalAlignment.Left
                    Else
                        newLine.TextLinePosition = 1
                        newLine.TextHorizontalAlignment = Windows.HorizontalAlignment.Left
                    End If
                    '
                    Dim plotArea As OxyRect = Plot.ActualModel.PlotArea
                    Dim plotLL = newLine.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                    Dim plotUR = newLine.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                    Dim dataPointClicked = newLine.InternalAnnotation.InverseTransform(e.Position)
                    newLine.X = dataPointClicked.X
                    newLine.Y = dataPointClicked.Y
                    newLine.Type = Annotations.LineAnnotationType.Horizontal
                    newLine.Intercept = dataPointClicked.Y
                    newLine.Slope = (plotUR.Y - plotLL.Y) / (plotUR.X - plotLL.X)
                    _targetAddAnnotation = newLine

                    OpenLineAnnotationTooltip(newLine)
                    UpdateLineAnnotationTooltip(newLine)
                    Plot.ActualModel.InvalidatePlot(False)

                Case AddToolMode.AddRectangleAnnotation
                    Dim newRectangle As New Wpf.RectangleAnnotation() With {.Text = "Rectangle Annotation"}
                    Plot.Annotations.Add(newRectangle)
                    Plot.ActualModel.InvalidatePlot(False)
                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newRectangle)
                    '
                    Dim dataPointClicked = newRectangle.InternalAnnotation.InverseTransform(e.Position)
                    newRectangle.MinimumX = dataPointClicked.X
                    newRectangle.MaximumX = dataPointClicked.X
                    newRectangle.MinimumY = dataPointClicked.Y
                    newRectangle.MaximumY = dataPointClicked.Y
                    _targetAddAnnotation = newRectangle

                Case AddToolMode.AddEllipseAnnotation
                    Dim newEllipse As New Wpf.EllipseAnnotation() With {.Text = "Ellipse Annotation"}
                    Plot.Annotations.Add(newEllipse)
                    Plot.ActualModel.InvalidatePlot(False)
                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newEllipse)
                    '
                    Dim dataPointClicked = newEllipse.InternalAnnotation.InverseTransform(e.Position)
                    newEllipse.MinimumX = dataPointClicked.X
                    newEllipse.MaximumX = dataPointClicked.X
                    newEllipse.MinimumY = dataPointClicked.Y
                    newEllipse.MaximumY = dataPointClicked.Y
                    _targetAddAnnotation = newEllipse

                Case AddToolMode.AddPointAnnotation
                    Dim newPoint As New Wpf.PointAnnotation() With {.Text = "Point Annotation", .Size = 5}
                    Plot.Annotations.Add(newPoint)
                    Plot.ActualModel.InvalidatePlot(False)
                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newPoint)
                    '
                    Dim dataPointClicked = newPoint.InternalAnnotation.InverseTransform(e.Position)
                    Dim plotLL, plotUR, plotCenter As DataPoint
                    Dim plotArea As OxyRect = Plot.ActualModel.PlotArea
                    plotLL = newPoint.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Left, plotArea.Bottom))
                    plotUR = newPoint.InternalAnnotation.InverseTransform(New ScreenPoint(plotArea.Right, plotArea.Top))
                    plotCenter = newPoint.InternalAnnotation.InverseTransform(Plot.ActualModel.PlotArea.Center)

                    newPoint.X = dataPointClicked.X
                    newPoint.Y = dataPointClicked.Y
                    _targetAddAnnotation = newPoint
                Case AddToolMode.AddPolygonAnnotation
                    If IsNothing(_targetAddAnnotation) Then
                        Dim newPolygon As New Wpf.PolygonAnnotation() With {.Text = "Polygon Annotation", .Points = New List(Of DataPoint)}
                        '
                        Dim dataPointClicked = ConvertScreenPointToDataPoint(e.Position)
                        newPolygon.Points.Add(dataPointClicked)
                        _leaderLine.Points.Add(New Point(e.Position.X, e.Position.Y))
                        _leaderLine.Points.Add(New Point(e.Position.X, e.Position.Y))
                        '
                        _targetAddAnnotation = newPolygon
                    Else
                        _doubleClicked = e.ClickCount > 1
                        Dim polyAnnotation = DirectCast(_targetAddAnnotation, Wpf.PolygonAnnotation)
                        If polyAnnotation.Points.Count = 3 Then
                            Plot.Annotations.Add(polyAnnotation)
                            RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, polyAnnotation)
                        End If
                        If e.ClickCount < 2 Then
                            _leaderLine.Points.Add(New Point(e.Position.X, e.Position.Y))
                            polyAnnotation.Points.Add(ConvertScreenPointToDataPoint(e.Position))
                        End If
                    End If
                Case AddToolMode.AddPolylineAnnotation
                    If IsNothing(_targetAddAnnotation) Then
                        Dim newPolyline As New Wpf.PolylineAnnotation() With {.Text = "Polyline Annotation", .Points = New List(Of DataPoint)}
                        Plot.Annotations.Add(newPolyline)
                        RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, newPolyline)
                        '
                        Dim dataPointClicked = ConvertScreenPointToDataPoint(e.Position)
                        newPolyline.Points.Add(dataPointClicked)
                        _leaderLine.Points.Add(New Point(e.Position.X, e.Position.Y))
                        _leaderLine.Points.Add(New Point(e.Position.X, e.Position.Y))
                        '
                        _targetAddAnnotation = newPolyline
                    Else
                        _doubleClicked = e.ClickCount > 1
                        If e.ClickCount < 2 Then DirectCast(_targetAddAnnotation, Wpf.PolylineAnnotation).Points.Add(_targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position))
                    End If
            End Select
            '
            Exit Sub
        End If
        '
        If Mouse.RightButton = MouseButtonState.Pressed Then
            Plot.DefaultPlotCursor = Cursors.Arrow
            Plot.Cursor = Cursors.Arrow

            'Check for selected objects
            GetSelectedObjects(sender, e)

            Exit Sub
        Else
            GetSelectedObjects(sender, e)
        End If
        If PanButton.IsChecked Or Mouse.MiddleButton = MouseButtonState.Pressed Then
            Plot.PanCursor = _panHandClosedCursor
            Plot.DefaultPlotCursor = _panHandClosedCursor
            Plot.Cursor = _panHandClosedCursor
        End If
    End Sub

    ''' <summary>
    ''' Determines the behavior for the mouse move event. 
    ''' </summary>
    Private Sub PlotModelMouseMove(sender As Object, e As OxyMouseEventArgs)
        'for adding annotations
        If _addAnnotationToolMode <> AddToolMode.None AndAlso IsNothing(_targetAddAnnotation) = False Then

            Select Case _addAnnotationToolMode
                Case AddToolMode.AddArrowAnnotation
                    DirectCast(_targetAddAnnotation, Wpf.ArrowAnnotation).EndPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position)
                Case AddToolMode.AddTextAnnotation
                    DirectCast(_targetAddAnnotation, Wpf.TextAnnotation).TextPosition = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position)
                Case AddToolMode.AddVerticalLineAnnotation
                    DirectCast(_targetAddAnnotation, Wpf.LineAnnotation).X = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position).X
                    UpdateLineAnnotationTooltip(DirectCast(_targetAddAnnotation, Wpf.LineAnnotation))

                Case AddToolMode.AddHorizontalLineAnnotation
                    DirectCast(_targetAddAnnotation, Wpf.LineAnnotation).Y = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position).Y
                    UpdateLineAnnotationTooltip(DirectCast(_targetAddAnnotation, Wpf.LineAnnotation))

                Case AddToolMode.AddRectangleAnnotation
                    Dim mouseDataPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position)
                    With DirectCast(_targetAddAnnotation, Wpf.RectangleAnnotation)
                        .MaximumX = mouseDataPoint.X
                        .MaximumY = mouseDataPoint.Y
                    End With
                Case AddToolMode.AddEllipseAnnotation
                    Dim mouseDataPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position)
                    With DirectCast(_targetAddAnnotation, Wpf.EllipseAnnotation)
                        .MaximumX = mouseDataPoint.X
                        .MaximumY = mouseDataPoint.Y
                    End With
                Case AddToolMode.AddPointAnnotation
                    Dim mouseDataPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position)
                    With DirectCast(_targetAddAnnotation, Wpf.PointAnnotation)
                        .X = mouseDataPoint.X
                        .Y = mouseDataPoint.Y
                    End With
                Case AddToolMode.AddPolygonAnnotation
                    _leaderLine.Points(_leaderLine.Points.Count - 1) = New Point(e.Position.X, e.Position.Y)
                    Plot.InvalidatePlot(True)
                Case AddToolMode.AddPolylineAnnotation
                    Dim polyAnnotation = DirectCast(_targetAddAnnotation, Wpf.PolylineAnnotation)
                    _leaderLine.Points(0) = ConvertDataPointToPoint(polyAnnotation.Points.Last)
                    _leaderLine.Points(_leaderLine.Points.Count - 1) = New Point(e.Position.X, e.Position.Y)
                    Plot.InvalidatePlot(False)
            End Select

            Exit Sub
        End If

        Dim requiresRedraw As Boolean = (_showPoints = True)
        _showPoints = False
        Dim markerPoints As New List(Of ScreenPoint)
        Dim markerSizes As New List(Of Double)
        Dim updatedCursor As Cursor = Nothing
        For Each a In Plot.Annotations
            If a.IsEnabled = False Then Continue For
            Dim ht As HitTestResult = a.InternalAnnotation.HitTest(New HitTestArguments(e.Position, 10))
            If IsNothing(ht) Then Continue For
            Select Case a.GetType
                Case GetType(Wpf.ArrowAnnotation)
                    Dim arrowAnnotation = DirectCast(a, Wpf.ArrowAnnotation)
                    '
                    markerPoints.Add(arrowAnnotation.InternalAnnotation.Transform(arrowAnnotation.StartPoint))
                    markerPoints.Add(arrowAnnotation.InternalAnnotation.Transform(arrowAnnotation.EndPoint))
                    markerSizes.AddRange({2.2, 2.2})
                    '
                    Select Case ht.Index
                        Case 0
                            updatedCursor = Cursors.SizeAll
                        Case 1
                            updatedCursor = _movePointsCursor
                        Case 2
                            updatedCursor = _movePointsCursor
                    End Select
                Case GetType(Wpf.TextAnnotation)
                    If ht.Index = 0 Then updatedCursor = Cursors.SizeAll
                Case GetType(Wpf.RectangleAnnotation)
                    Dim rAnnotation = DirectCast(a, Wpf.RectangleAnnotation)
                    Dim ur As ScreenPoint = rAnnotation.InternalAnnotation.Transform(Math.Max(rAnnotation.MaximumX, rAnnotation.MinimumX), Math.Max(rAnnotation.MaximumY, rAnnotation.MinimumY))
                    Dim ll As ScreenPoint = rAnnotation.InternalAnnotation.Transform(Math.Min(rAnnotation.MinimumX, rAnnotation.MaximumX), Math.Min(rAnnotation.MinimumY, rAnnotation.MaximumY))
                    '
                    markerPoints.Add(ur)
                    markerPoints.Add(ll)
                    markerPoints.Add(New ScreenPoint(ll.X, ur.Y))
                    markerPoints.Add(New ScreenPoint(ur.X, ll.Y))
                    markerPoints.Add(New ScreenPoint(ll.X, ll.Y + (ur.Y - ll.Y) / 2))
                    markerPoints.Add(New ScreenPoint(ur.X, ll.Y + (ur.Y - ll.Y) / 2))
                    markerPoints.Add(New ScreenPoint(ll.X + (ur.X - ll.X) / 2, ll.Y))
                    markerPoints.Add(New ScreenPoint(ll.X + (ur.X - ll.X) / 2, ur.Y))
                    markerSizes.AddRange({2, 2, 2, 2, 2, 2, 2, 2})
                    '
                    Dim topRight As New ScreenPoint(Math.Abs(ur.X - e.Position.X), Math.Abs(ur.Y - e.Position.Y))
                    Dim bottomLeft As New ScreenPoint(Math.Abs(ll.X - e.Position.X), Math.Abs(ll.Y - e.Position.Y))
                    'corners
                    If topRight.X < 10 AndAlso topRight.Y < 10 Then updatedCursor = Cursors.SizeNESW : Continue For
                    If bottomLeft.X < 10 AndAlso bottomLeft.Y < 10 Then updatedCursor = Cursors.SizeNESW : Continue For
                    If bottomLeft.X < 10 AndAlso topRight.Y < 10 Then updatedCursor = Cursors.SizeNWSE : Continue For
                    If topRight.X < 10 AndAlso bottomLeft.Y < 10 Then updatedCursor = Cursors.SizeNWSE : Continue For
                    'edges
                    If topRight.X < 10 OrElse bottomLeft.X < 10 Then updatedCursor = Cursors.SizeWE : Continue For
                    If topRight.Y < 10 OrElse bottomLeft.Y < 10 Then updatedCursor = Cursors.SizeNS : Continue For
                    'all
                    If ht.Index = 0 Then updatedCursor = Cursors.SizeAll
                Case GetType(Wpf.EllipseAnnotation)
                    Dim eAnnotation = DirectCast(a, Wpf.EllipseAnnotation)
                    Dim ur As ScreenPoint = eAnnotation.InternalAnnotation.Transform(Math.Max(eAnnotation.MaximumX, eAnnotation.MinimumX), Math.Max(eAnnotation.MaximumY, eAnnotation.MinimumY))
                    Dim ll As ScreenPoint = eAnnotation.InternalAnnotation.Transform(Math.Min(eAnnotation.MinimumX, eAnnotation.MaximumX), Math.Min(eAnnotation.MinimumY, eAnnotation.MaximumY))
                    '
                    markerPoints.Add(ur)
                    markerPoints.Add(ll)
                    markerPoints.Add(New ScreenPoint(ll.X, ur.Y))
                    markerPoints.Add(New ScreenPoint(ur.X, ll.Y))
                    markerPoints.Add(New ScreenPoint(ll.X, ll.Y + (ur.Y - ll.Y) / 2))
                    markerPoints.Add(New ScreenPoint(ur.X, ll.Y + (ur.Y - ll.Y) / 2))
                    markerPoints.Add(New ScreenPoint(ll.X + (ur.X - ll.X) / 2, ll.Y))
                    markerPoints.Add(New ScreenPoint(ll.X + (ur.X - ll.X) / 2, ur.Y))
                    markerSizes.AddRange({2, 2, 2, 2, 2, 2, 2, 2})
                    '
                    Dim topRight As New ScreenPoint(Math.Abs(ur.X - e.Position.X), Math.Abs(ur.Y - e.Position.Y))
                    Dim bottomLeft As New ScreenPoint(Math.Abs(ll.X - e.Position.X), Math.Abs(ll.Y - e.Position.Y))
                    'corners
                    If topRight.X < 10 AndAlso topRight.Y < 10 Then updatedCursor = Cursors.SizeNESW : Continue For
                    If bottomLeft.X < 10 AndAlso bottomLeft.Y < 10 Then updatedCursor = Cursors.SizeNESW : Continue For
                    If bottomLeft.X < 10 AndAlso topRight.Y < 10 Then updatedCursor = Cursors.SizeNWSE : Continue For
                    If topRight.X < 10 AndAlso bottomLeft.Y < 10 Then updatedCursor = Cursors.SizeNWSE : Continue For
                    'edges
                    If topRight.X < 10 OrElse bottomLeft.X < 10 Then updatedCursor = Cursors.SizeWE : Continue For
                    If topRight.Y < 10 OrElse bottomLeft.Y < 10 Then updatedCursor = Cursors.SizeNS : Continue For
                    'all
                    If ht.Index = 0 Then updatedCursor = Cursors.SizeAll
                Case GetType(Wpf.PointAnnotation)
                    If ht.Index = 0 Then updatedCursor = Cursors.SizeAll
                Case GetType(Wpf.PolygonAnnotation)
                    'all
                    If ht.Index = 0 Then
                        Dim polyAnnotation = DirectCast(a, Wpf.PolygonAnnotation)
                        Dim screenToData = polyAnnotation.InternalAnnotation.InverseTransform(e.Position)
                        Dim screen2ToData = polyAnnotation.InternalAnnotation.InverseTransform(New ScreenPoint(e.Position.X - 10, e.Position.Y - 10))
                        Dim dxy = New DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y))
                        '
                        For Each p In polyAnnotation.Points
                            markerPoints.Add(polyAnnotation.InternalAnnotation.Transform(p))
                            markerSizes.Add(2)
                        Next
                        '
                        Dim dPoint = polyAnnotation.InternalAnnotation.InverseTransform(e.Position)
                        'Need to check if the cursor is over any points in the annotation point data
                        If polyAnnotation.Points.Any(Function(o) Math.Abs(dPoint.X - o.X) < dxy.X AndAlso Math.Abs(dPoint.Y - o.Y) < dxy.Y) Then
                            updatedCursor = _movePointsCursor
                        Else
                            Dim p1, p2, linePoint As ScreenPoint
                            Dim onLine As Boolean = False
                            For i As Int32 = 0 To polyAnnotation.Points.Count - 2
                                p1 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points(i))
                                p2 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points(i + 1))
                                linePoint = ScreenPointHelper.FindPointOnLine(e.Position, p1, p2)
                                If (linePoint - e.Position).Length < 10 Then
                                    onLine = True
                                    updatedCursor = _addPointCursor
                                    Exit For
                                End If
                            Next
                            '
                            If onLine = False Then
                                'check between first and last point
                                p1 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points(0))
                                p2 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points(polyAnnotation.Points.Count - 1))
                                linePoint = ScreenPointHelper.FindPointOnLine(e.Position, p1, p2)
                                If (linePoint - e.Position).Length < 10 Then
                                    onLine = True
                                    updatedCursor = _addPointCursor
                                Else
                                    updatedCursor = Cursors.SizeAll
                                End If
                            End If
                        End If
                    End If
                Case GetType(Wpf.PolylineAnnotation)
                    'all
                    If ht.Index = 0 Then
                        Dim polylineAnnotation = DirectCast(a, Wpf.PolylineAnnotation)
                        Dim screenToData = polylineAnnotation.InternalAnnotation.InverseTransform(e.Position)
                        Dim screen2ToData = polylineAnnotation.InternalAnnotation.InverseTransform(New ScreenPoint(e.Position.X - 10, e.Position.Y - 10))
                        Dim dxy = New DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y))
                        '
                        For Each p In polylineAnnotation.Points
                            markerPoints.Add(polylineAnnotation.InternalAnnotation.Transform(p))
                            markerSizes.Add(2)
                        Next
                        '
                        Dim dPoint = polylineAnnotation.InternalAnnotation.InverseTransform(e.Position)
                        'Need to check if the cursor is over any points in the annotation point data
                        If polylineAnnotation.Points.Any(Function(o) Math.Abs(dPoint.X - o.X) < dxy.X AndAlso Math.Abs(dPoint.Y - o.Y) < dxy.Y) Then
                            updatedCursor = _movePointsCursor
                        Else
                            If e.IsControlDown Then
                                updatedCursor = _addPointCursor
                            Else
                                updatedCursor = Cursors.SizeAll
                            End If
                        End If
                    End If
                Case GetType(Wpf.LineAnnotation)
                    If ht.Index = 0 Then updatedCursor = Cursors.SizeAll

            End Select
        Next
        '
        If markerPoints.Count > 0 Then
            _showPoints = True
            RenderingExtensions.DrawMarkers(Plot.RenderContext, Plot.ActualModel.PlotArea, markerPoints, MarkerType.Square, New List(Of ScreenPoint), markerSizes, OxyColors.White, OxyColors.Black, 2)
        Else
            If requiresRedraw = True Then Plot.InvalidatePlot(False)
        End If
        '
        If IsNothing(updatedCursor) Then
            Plot.Cursor = Plot.DefaultPlotCursor
        Else
            Plot.Cursor = updatedCursor
            Exit Sub
        End If

        'Now set closed pan hand if needed
        '
        If ((Mouse.LeftButton = MouseButtonState.Pressed And PanButton.IsChecked) Or Mouse.MiddleButton = MouseButtonState.Pressed) Then
            Plot.PanCursor = _panHandClosedCursor
            '
            Plot.DefaultPlotCursor = _panHandClosedCursor
            Plot.Cursor = _panHandClosedCursor
        End If

    End Sub

    ''' <summary>
    ''' Determines the behavior for the mouse up event. 
    ''' </summary>
    Private Sub PlotModelMouseUp(sender As Object, e As OxyMouseEventArgs)
        '
        If _addAnnotationToolMode = AddToolMode.AddPolygonAnnotation Or _addAnnotationToolMode = AddToolMode.AddPolylineAnnotation Then
            If _doubleClicked = True Then StopAddAnnotation()

        ElseIf _addAnnotationToolMode = AddToolMode.AddHorizontalLineAnnotation OrElse _addAnnotationToolMode = AddToolMode.AddVerticalLineAnnotation Then
            CloseLineAnnotationTooltip(DirectCast(_targetAddAnnotation, Wpf.LineAnnotation))
            StopAddAnnotation()

        ElseIf _addAnnotationToolMode = AddToolMode.AddRectangleAnnotation Then
            ' Check to see if the size of rectangle is at least 10 pixels in height and width
            Dim rectangle = DirectCast(_targetAddAnnotation, Wpf.RectangleAnnotation)
            Dim upperRight As ScreenPoint = rectangle.InternalAnnotation.Transform(rectangle.MaximumX, rectangle.MaximumY)
            Dim lowerLeft As ScreenPoint = rectangle.InternalAnnotation.Transform(rectangle.MinimumX, rectangle.MinimumY)
            Dim pixelWidth As Double = Math.Abs(upperRight.X - lowerLeft.X)
            Dim pixelHeight As Double = Math.Abs(upperRight.Y - lowerLeft.Y)
            ' Correct the height and width if necessary
            If pixelWidth < 10 OrElse pixelHeight < 10 Then
                Dim plotLL = rectangle.InternalAnnotation.InverseTransform(New ScreenPoint(Plot.ActualModel.PlotArea.Left, Plot.ActualModel.PlotArea.Bottom))
                Dim plotUR = rectangle.InternalAnnotation.InverseTransform(New ScreenPoint(Plot.ActualModel.PlotArea.Right, Plot.ActualModel.PlotArea.Top))
                Dim centerXShift As Double = Math.Abs((plotUR.X - plotLL.X) * 0.1)
                Dim centerYShift As Double = Math.Abs((plotUR.Y - plotLL.Y) * 0.1)
                Dim mouseDataPoint = rectangle.InternalAnnotation.InverseTransform(e.Position)
                If pixelWidth < 10 Then
                    rectangle.MinimumX = mouseDataPoint.X - centerXShift
                    rectangle.MaximumX = mouseDataPoint.X + centerXShift
                End If
                If pixelHeight < 10 Then
                    rectangle.MinimumY = mouseDataPoint.Y - centerYShift
                    rectangle.MaximumY = mouseDataPoint.Y + centerYShift
                End If
            End If
            StopAddAnnotation()

        ElseIf _addAnnotationToolMode = AddToolMode.AddEllipseAnnotation Then
            ' Check to see if the size of the ellipse is at least 10 pixels in height and width
            Dim ellipse = DirectCast(_targetAddAnnotation, Wpf.EllipseAnnotation)
            Dim upperRight As ScreenPoint = ellipse.InternalAnnotation.Transform(ellipse.MaximumX, ellipse.MaximumY)
            Dim lowerLeft As ScreenPoint = ellipse.InternalAnnotation.Transform(ellipse.MinimumX, ellipse.MinimumY)
            Dim pixelWidth As Double = Math.Abs(upperRight.X - lowerLeft.X)
            Dim pixelHeight As Double = Math.Abs(upperRight.Y - lowerLeft.Y)
            ' Correct the height and width if necessary
            If pixelWidth < 10 OrElse pixelHeight < 10 Then
                Dim plotLL = ellipse.InternalAnnotation.InverseTransform(New ScreenPoint(Plot.ActualModel.PlotArea.Left, Plot.ActualModel.PlotArea.Bottom))
                Dim plotUR = ellipse.InternalAnnotation.InverseTransform(New ScreenPoint(Plot.ActualModel.PlotArea.Right, Plot.ActualModel.PlotArea.Top))
                Dim centerXShift As Double = Math.Abs((plotUR.X - plotLL.X) * 0.1)
                Dim centerYShift As Double = Math.Abs((plotUR.Y - plotLL.Y) * 0.1)
                Dim mouseDataPoint = ellipse.InternalAnnotation.InverseTransform(e.Position)
                If pixelWidth < 10 Then
                    ellipse.MinimumX = mouseDataPoint.X - centerXShift
                    ellipse.MaximumX = mouseDataPoint.X + centerXShift
                End If
                If pixelHeight < 10 Then
                    ellipse.MinimumY = mouseDataPoint.Y - centerYShift
                    ellipse.MaximumY = mouseDataPoint.Y + centerYShift
                End If
            End If
            StopAddAnnotation()

        Else
            If _addAnnotationToolMode = AddToolMode.AddArrowAnnotation Then
                Dim arrow = DirectCast(_targetAddAnnotation, Wpf.ArrowAnnotation)
                If Math.Abs(arrow.StartPoint.X - arrow.EndPoint.X) < 0.000000001 AndAlso Math.Abs(arrow.StartPoint.Y - arrow.EndPoint.Y) < 0.000000001 Then
                    Dim plotCenter As DataPoint
                    Dim plotArea As OxyRect = Plot.ActualModel.PlotArea
                    plotCenter = ConvertScreenPointToDataPoint(plotArea.Center)
                    Dim xShift = plotArea.Center.X + Math.Abs(plotArea.Right - plotArea.Left) * 0.05
                    Dim centerXShifted = ConvertScreenPointToDataPoint(New ScreenPoint(xShift, plotArea.Center.Y))
                    '
                    arrow.StartPoint = New DataPoint(arrow.StartPoint.X + centerXShifted.X, arrow.StartPoint.Y)
                End If
            End If
            StopAddAnnotation()
        End If
        '
        SetCursor()
    End Sub

    ''' <summary>
    ''' Get the selected object on the plot.
    ''' </summary>
    Private Sub GetSelectedObjects(sender As Object, e As OxyMouseDownEventArgs)

        'Middle clicks initiate the Pan option
        If e.ChangedButton = OxyMouseButton.Middle Then Exit Sub

        'Left clicks try to edit/open the first thing clicked.  Right clicks provide more context.
        Dim leftClickBool As Boolean = False
        If e.ChangedButton = OxyMouseButton.Left Then
            leftClickBool = True
        End If

        _contextMenu = New ContextMenu

        If leftClickBool = False AndAlso Plot.ActualModel.PlotArea.Contains(e.Position) Then
            Dim item1 As New MenuItem With {
                .Header = "Format Plot Area",
                .Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Format)}
            }
            AddHandler item1.Click, Sub() RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.General_PlotArea, Plot.ActualModel.PlotArea)
            '
            _contextMenu.Items.Add(item1)
        End If

        'SERIES hit test
        Dim seriesHTRS As List(Of OxyPlot.HitTestResult) = Plot.ActualModel.HitTest(New HitTestArguments(e.Position, 10)).ToList
        For Each htr As HitTestResult In seriesHTRS
            Dim wpfSeries As OxyPlot.Wpf.Series = Nothing
            wpfSeries = Plot.Series.Where(Function(d) d.InternalSeries.Equals(htr.Element)).FirstOrDefault
            If wpfSeries IsNot Nothing Then
                If leftClickBool = True Then
                    RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Series_General, wpfSeries)
                    Exit Sub
                Else
                    Dim item1 As New MenuItem()
                    item1.Header = "Format Series: " & wpfSeries.Title
                    item1.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Format)}
                    AddHandler item1.Click, Sub()
                                                RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Series_General, wpfSeries)
                                            End Sub

                    _contextMenu.Items.Add(item1)
                End If
            End If
        Next

        Dim PlotAndAxisArea As OxyRect = Plot.ActualModel.PlotAndAxisArea
        Dim PlotArea As OxyRect = Plot.ActualModel.PlotArea

        'Legend Area custom hit test
        Dim legendArea As OxyRect = Plot.ActualModel.LegendArea
        If legendArea.Contains(e.Position) Then
            If leftClickBool = True Then
                RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Legend_Title, legendArea)
                Exit Sub
            Else
                Dim item2 As New MenuItem()
                item2.Header = "Format Legend"
                item2.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Format)}
                AddHandler item2.Click, Sub()
                                            RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Legend_Title, legendArea)
                                        End Sub

                _contextMenu.Items.Add(item2)
            End If
        End If

        'TEXT HIT TEST
        Dim textResult As IInputElement = Plot.canvas.InputHitTest(New Point(e.Position.X, e.Position.Y))
        If textResult IsNot Nothing Then
            If textResult.GetType = GetType(TextBlock) Then
                Dim txtblock As TextBlock = CType(textResult, TextBlock)
                'CHART TITLE SELECTED
                If Plot.Title = txtblock.Text AndAlso Plot.ActualModel.TitleArea.Contains(New ScreenPoint(e.Position.X, e.Position.Y)) Then
                    If leftClickBool = True Then
                        RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.General_PlotTitle, Plot.ActualModel.TitleArea)
                        CreateEditTBX(txtblock, Plot, OxyPlot.Wpf.Plot.TitleProperty, 0, Plot.canvas)
                        Exit Sub
                    Else
                        'Contextual Select
                        Dim item1 As New MenuItem()
                        item1.Header = "Edit Plot Title"
                        item1.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.EditTextbox)}
                        AddHandler item1.Click, Sub()
                                                    RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.General_PlotTitle, Plot.ActualModel.TitleArea)
                                                    CreateEditTBX(txtblock, Plot, OxyPlot.Wpf.Plot.TitleProperty, 0, Plot.canvas)
                                                End Sub

                        Dim item2 As New MenuItem()
                        item2.Header = "Format Plot Title"
                        item2.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Format)}
                        AddHandler item2.Click, Sub()
                                                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.General_PlotTitle, Plot.ActualModel.TitleArea)
                                                    RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea)
                                                End Sub
                        _contextMenu.Items.Add(item1)
                        _contextMenu.Items.Add(item2)
                    End If
                End If

                'CHART SUBTITLE SELECTED
                If Plot.Subtitle = txtblock.Text AndAlso Plot.ActualModel.TitleArea.Contains(New ScreenPoint(e.Position.X, e.Position.Y)) Then

                    If leftClickBool = True Then
                        RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea)

                        CreateEditTBX(txtblock, Plot, OxyPlot.Wpf.Plot.SubtitleProperty, 0, Plot.canvas)

                        Exit Sub
                    Else
                        Dim item1 As New MenuItem()
                        item1.Header = "Edit Plot Subtitle"
                        item1.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.EditTextbox)}
                        AddHandler item1.Click, Sub()
                                                    RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea)

                                                    CreateEditTBX(txtblock, Plot, OxyPlot.Wpf.Plot.SubtitleProperty, 0, Plot.canvas)
                                                End Sub


                        Dim item2 As New MenuItem()
                        item2.Header = "Format Plot Subtitle"
                        item2.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Format)}
                        AddHandler item2.Click, Sub()
                                                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea)
                                                End Sub
                        _contextMenu.Items.Add(item1)
                        _contextMenu.Items.Add(item2)
                    End If

                End If



                'AXES TITLES SELECTED
                If Plot.ActualModel.PlotAndAxisArea.Contains(New ScreenPoint(e.Position.X, e.Position.Y)) Then

                    Dim axes As IEnumerable(Of Wpf.Axis) = Plot.Axes.Where(Function(X) If(IsNothing(X.Title), False, txtblock.Text.Contains(X.Title)))

                    If axes.Count > 1 Then
                        'narrow it down to the selected axis area
                        Dim selectedAxisArea As OxyRect = Nothing

                        'AXIS Areas hit test
                        For Each ax As Wpf.Axis In axes

                            With ax

                                'Dummy canvas is used to render the item.  To make sure we grab the right item.
                                Dim dummyCanvas As New Canvas
                                Dim crc As New OxyPlot.Wpf.CanvasRenderContext(dummyCanvas)
                                Dim size As New Size(Plot.canvas.ActualWidth, Plot.canvas.ActualHeight)
                                dummyCanvas.Measure(size)
                                dummyCanvas.Arrange(New Rect(size))
                                dummyCanvas.UpdateLayout()

                                'Render minor items
                                'ax.InternalAxis.Render(crc, 0)

                                'Render major items and axis title
                                ax.InternalAxis.Render(crc, 1)

                                dummyCanvas.UpdateLayout()

                                'Dim w As New Window
                                'w.Content = dummyCanvas
                                'w.ShowDialog()

                                Dim axLeft As Double = Nothing
                                Dim axTop As Double = Nothing
                                Dim axWidth As Double = Nothing
                                Dim axHeight As Double = Nothing

                                For Each tbk As TextBlock In FindVisualChildren(Of TextBlock)(dummyCanvas)
                                    Dim title As String = ax.Title
                                    If ax.Unit IsNot Nothing Then
                                        title = String.Format(ax.TitleFormatString, ax.Title, ax.Unit)
                                    End If

                                    If title = tbk.Text Then

                                        If ax.Position = OxyPlot.Axes.AxisPosition.Left Or ax.Position = OxyPlot.Axes.AxisPosition.Right Then
                                            'It's vertical
                                            axLeft = GetPosition(tbk, dummyCanvas).X
                                            axTop = GetPosition(tbk, dummyCanvas).Y - tbk.ActualWidth
                                            axWidth = tbk.ActualHeight
                                            axHeight = tbk.ActualWidth
                                        Else
                                            'Its horizontal
                                            axLeft = GetPosition(tbk, dummyCanvas).X
                                            axTop = GetPosition(tbk, dummyCanvas).Y
                                            axWidth = tbk.ActualWidth
                                            axHeight = tbk.ActualHeight
                                        End If

                                        selectedAxisArea = New OxyRect(axLeft, axTop, axWidth, axHeight)

                                        'Draw the hit test
                                        'crc.DrawRectangle(selectedAxisArea, OxyColors.Blue, OxyColors.Blue, 1)
                                        'dummyCanvas.UpdateLayout()
                                        'Dim w As New Window
                                        'w.Content = dummyCanvas
                                        'w.ShowDialog()

                                        If selectedAxisArea.Contains(e.Position) Then
                                            axes = {ax}
                                            Exit For
                                        End If

                                    End If
                                Next tbk

                            End With

                        Next ax

                    End If

                    If axes.Count = 1 Then
                        Dim ax As Wpf.Axis = axes.FirstOrDefault

                        If leftClickBool = True Then

                            RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Axes_Title, ax)

                            If ax.InternalAxis.IsVertical Then
                                CreateEditTBX(txtblock, axes.FirstOrDefault, OxyPlot.Wpf.Axis.TitleProperty, -90, Plot.canvas)
                            Else
                                CreateEditTBX(txtblock, axes.FirstOrDefault, OxyPlot.Wpf.Axis.TitleProperty, 0, Plot.canvas)
                            End If

                            Exit Sub
                        Else
                            Dim item1 As New MenuItem()
                            item1.Header = "Edit Axis Title: " & ax.Title
                            item1.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.EditTextbox)}
                            AddHandler item1.Click, Sub()
                                                        RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Axes_Title, ax)

                                                        If ax.InternalAxis.IsVertical Then
                                                            CreateEditTBX(txtblock, axes.FirstOrDefault, OxyPlot.Wpf.Axis.TitleProperty, -90, Plot.canvas)
                                                        Else
                                                            CreateEditTBX(txtblock, axes.FirstOrDefault, OxyPlot.Wpf.Axis.TitleProperty, 0, Plot.canvas)
                                                        End If

                                                    End Sub

                            _contextMenu.Items.Add(item1)

                        End If

                    End If


                End If

            End If


        End If

        'AXIS Areas hit test
        For Each ax As Wpf.Axis In Plot.Axes
            With ax

                ''Dummy canvas is used to render the item.  To make sure we grab the right item.
                'Dim dummyCanvas As New Canvas
                'Dim crc As New OxyPlot.Wpf.CanvasRenderContext(dummyCanvas)
                'Dim size As New Size(Plot.canvas.ActualWidth, Plot.canvas.ActualHeight)
                'dummyCanvas.Measure(size)
                'dummyCanvas.Arrange(New Rect(size))
                'dummyCanvas.UpdateLayout()

                ''Render minor items
                'ax.InternalAxis.Render(crc, 0)

                ''Render major items and axis title
                'ax.InternalAxis.Render(crc, 1)

                'dummyCanvas.UpdateLayout()


                ''Dim w As New Window
                ''w.Content = dummyCanvas
                ''w.ShowDialog()

                Dim axLeft As Double = Nothing
                Dim axTop As Double = Nothing
                Dim axWidth As Double = Nothing
                Dim axHeight As Double = Nothing

                Select Case .Position
                    Case Axes.AxisPosition.Bottom
                        axLeft = PlotArea.Left
                        axTop = PlotArea.Bottom + .AxisDistance
                        axWidth = PlotArea.Width
                        axHeight = ax.InternalAxis.DesiredSize.Height
                    Case Axes.AxisPosition.Top
                        axLeft = PlotArea.Left
                        axTop = PlotArea.Top - .AxisDistance - ax.InternalAxis.DesiredSize.Height
                        axWidth = PlotArea.Width
                        axHeight = ax.InternalAxis.DesiredSize.Height
                    Case Axes.AxisPosition.Left
                        axLeft = PlotArea.Left - .AxisDistance - ax.InternalAxis.DesiredSize.Width
                        axTop = PlotArea.Top
                        axWidth = ax.InternalAxis.DesiredSize.Width
                        axHeight = PlotArea.Height
                    Case Axes.AxisPosition.Right
                        axLeft = PlotArea.Right + .AxisDistance
                        axTop = PlotArea.Top
                        axWidth = ax.InternalAxis.DesiredSize.Width
                        axHeight = PlotArea.Height
                End Select

                Dim axArea1 As New OxyRect(axLeft, axTop, axWidth, axHeight)
                If axArea1.Contains(e.Position) Then
                    If leftClickBool = True Then
                        RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Axes_Options, ax)
                        RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Axes_Display, ax)
                        Exit Sub
                    Else
                        Dim item1 As New MenuItem()
                        item1.Header = "Format Axis: " & ax.Title
                        item1.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Format)}
                        AddHandler item1.Click, Sub()
                                                    RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Axes_Options, ax)
                                                    RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Axes_Display, ax)
                                                End Sub
                        _contextMenu.Items.Add(item1)

                    End If
                End If
            End With
        Next

        'ANNOTATIONS hit test
        Dim annoHTRS As List(Of OxyPlot.HitTestResult) = Plot.ActualModel.HitTest(New HitTestArguments(e.Position, 10)).ToList
        For Each htr As HitTestResult In annoHTRS
            Dim wpfAnno As OxyPlot.Wpf.Annotation = Nothing
            Dim annoText As String = ""

            'Simplified select case to just this
            Dim theAnno = TryCast(htr.Element, OxyPlot.Annotations.Annotation)
            If theAnno IsNot Nothing Then wpfAnno = TryCast(Plot.Annotations.Where(Function(d) d.InternalAnnotation Is theAnno).FirstOrDefault, OxyPlot.Wpf.Annotation)

            If wpfAnno IsNot Nothing Then

                annoText = CType(wpfAnno, OxyPlot.Wpf.TextualAnnotation).Text

                If leftClickBool = True Then
                    'Change selected object from left click
                    RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, wpfAnno)
                    Exit Sub
                Else
                    Dim item1 As New MenuItem()
                    item1.Header = "Edit Annotation Text: " & annoText
                    item1.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.EditTextbox)}
                    AddHandler item1.Click, Sub()
                                                RaiseEvent PropertiesCalled(Plot, False, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, wpfAnno)

                                                'Dummy canvas is used to render the item.  To make sure we grab the right item.
                                                Dim dummyCanvas As New Canvas
                                                Dim crc As New OxyPlot.Wpf.CanvasRenderContext(dummyCanvas)
                                                Dim size As New Size(Plot.canvas.ActualWidth, Plot.canvas.ActualHeight)
                                                dummyCanvas.Measure(size)
                                                dummyCanvas.Arrange(New Rect(size))
                                                dummyCanvas.UpdateLayout()
                                                wpfAnno.InternalAnnotation.Render(crc)
                                                dummyCanvas.UpdateLayout()

                                                'Dim w As New Window
                                                'w.Content = c
                                                'w.ShowDialog()

                                                For Each tbk As TextBlock In FindVisualChildren(Of TextBlock)(dummyCanvas)

                                                    Select Case wpfAnno.GetType
                                                        Case GetType(OxyPlot.Wpf.ArrowAnnotation)
                                                            Dim anno As OxyPlot.Wpf.ArrowAnnotation = CType(wpfAnno, OxyPlot.Wpf.ArrowAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.ArrowAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case GetType(OxyPlot.Wpf.TextAnnotation)
                                                            Dim anno As OxyPlot.Wpf.TextAnnotation = CType(wpfAnno, OxyPlot.Wpf.TextAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.TextAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case GetType(OxyPlot.Wpf.RectangleAnnotation)
                                                            Dim anno As OxyPlot.Wpf.RectangleAnnotation = CType(wpfAnno, OxyPlot.Wpf.RectangleAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.RectangleAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case GetType(OxyPlot.Wpf.EllipseAnnotation)
                                                            Dim anno As OxyPlot.Wpf.EllipseAnnotation = CType(wpfAnno, OxyPlot.Wpf.EllipseAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.EllipseAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case GetType(OxyPlot.Wpf.PointAnnotation)
                                                            Dim anno As OxyPlot.Wpf.PointAnnotation = CType(wpfAnno, OxyPlot.Wpf.PointAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.PointAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case GetType(OxyPlot.Wpf.PolygonAnnotation)
                                                            Dim anno As OxyPlot.Wpf.PolygonAnnotation = CType(wpfAnno, OxyPlot.Wpf.PolygonAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.PolygonAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case GetType(OxyPlot.Wpf.PolylineAnnotation)
                                                            Dim anno As OxyPlot.Wpf.PolylineAnnotation = CType(wpfAnno, OxyPlot.Wpf.PolylineAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.PolylineAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case GetType(OxyPlot.Wpf.LineAnnotation)
                                                            Dim anno As OxyPlot.Wpf.LineAnnotation = CType(wpfAnno, OxyPlot.Wpf.LineAnnotation)
                                                            If tbk.Text = anno.Text Then
                                                                CreateEditTBX(tbk, anno, OxyPlot.Wpf.LineAnnotation.TextProperty, anno.TextRotation, dummyCanvas)
                                                            End If
                                                        Case Else

                                                    End Select


                                                Next

                                            End Sub

                    Dim item2 As New MenuItem()
                    item2.Header = "Format Annotation: " & annoText
                    item2.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Format)}
                    AddHandler item2.Click, Sub()
                                                RaiseEvent PropertiesCalled(Plot, True, OxyplotPropertiesControl.PropertyEXP.Annotations_Text, wpfAnno)
                                            End Sub

                    Dim item4 As New MenuItem()
                    item4.Header = "Delete Annotation: " & annoText
                    item4.Icon = New Image With {.Source = Bitmap2BitmapSource(My.Resources.Delete)}
                    AddHandler item4.Click, Sub()
                                                Plot.Annotations.Remove(wpfAnno)
                                                Plot.InvalidatePlot(False)
                                            End Sub

                    _contextMenu.Items.Add(item1)
                    _contextMenu.Items.Add(item2)
                    _contextMenu.Items.Add(item4)

                End If
            End If

            ' Only add one CM for annotations.
            Exit For

        Next

        If _contextMenu.Items.Count = 0 Then
            Exit Sub
        End If

        _contextMenu.Placement = Primitives.PlacementMode.MousePoint
        _contextMenu.HorizontalOffset = 0
        _contextMenu.VerticalOffset = 0
        _contextMenu.IsOpen = True


    End Sub

    ''' <summary>
    ''' Gets the position of an element on the plot. 
    ''' </summary>
    Private Function GetPosition(ByVal element As Visual, c As Canvas) As Point
        Dim positionTransform = element.TransformToAncestor(c)
        Dim areaPosition = positionTransform.Transform(New Point(0, 0))
        Return areaPosition
    End Function

    Private Sub CreateEditTBX(ExistingTextblock As TextBlock, dependencyObj As DependencyObject, dependencyProp As DependencyProperty, angle As Double, c As Canvas)
        Dim txtblckAsInputElem As IInputElement = CType(ExistingTextblock, IInputElement)
        Dim currentTextColor As Color = Colors.Black 'This is for all annotations (hiding text while editing)
        Dim currentStrokeColor As Color = Colors.Black 'This is just for the text annotation, which is a box by default

        Dim point As Point
        Try
            point = GetPosition(CType(txtblckAsInputElem, Visual), c)
        Catch ex As Exception
            Exit Sub
        End Try
        Dim left As Double = point.X
        Dim top As Double = point.Y
        Dim width As Double = ExistingTextblock.ActualWidth
        Dim height As Double = ExistingTextblock.ActualHeight
        Dim fontsize As Double = ExistingTextblock.FontSize
        Dim fontFamily As FontFamily = ExistingTextblock.FontFamily
        Dim fontWeight As FontWeight = ExistingTextblock.FontWeight
        Dim foreColor As Brush = ExistingTextblock.Foreground

        ' Create canvas for the the textbox overlay
        Dim canvasOverlay As New Canvas() With {.Name = "TextBoxCanvas"}
        canvasOverlay.Background = New SolidColorBrush(Colors.Transparent)
        Dim dockPanel As New DockPanel
        Dim plotParent As Grid = CType(Plot.canvas.Parent, Grid)
        plotParent.Children.Add(canvasOverlay)

        ' Set initial text box settings
        _textBox = New TextBox()
        _textBox.Background = Plot.Background
        _textBox.TextAlignment = TextAlignment.Center
        _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Center
        _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
        _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Stretch
        _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Stretch
        _textBox.Padding = New Thickness(-2)
        _textBox.FontSize = fontsize
        _textBox.FontFamily = fontFamily
        _textBox.FontWeight = fontWeight
        _textBox.Foreground = foreColor
        TextOptions.SetTextFormattingMode(_textBox, TextFormattingMode.Display)

        ' Normalize all angles to 0-360
        If angle < 0 Or angle >= 360 Then
            angle = angle Mod 360
            If angle < 0 Then
                angle += 360
            End If
        End If

        ' Determine what type of element was selected.
        Select Case dependencyObj.GetType

            Case GetType(OxyPlot.Wpf.Plot)
                'It must be a title or subtitle, which are handled the same way

                dockPanel.RenderTransform = New RotateTransform(angle, 0, 0)
                dockPanel.Width = Plot.ActualModel.PlotArea.Width
                dockPanel.Height = height
                Canvas.SetLeft(dockPanel, Plot.ActualModel.PlotArea.Left) 'Gets the parent canvas and moves the object
                Canvas.SetTop(dockPanel, top) 'Gets the parent canvas and moves the object

                Dim title As String = Plot.Title
                currentTextColor = Plot.TitleColor
                Plot.TitleColor = Colors.Transparent
                _textBox.Text = title

            Case GetType(OxyPlot.Wpf.LogarithmicAxis), GetType(OxyPlot.Wpf.LinearAxis), GetType(OxyPlot.Wpf.DateTimeAxis), GetType(OxyPlot.Wpf.CategoryAxis), GetType(OxyPlot.Wpf.GumbelProbabilityAxis),
                 GetType(OxyPlot.Wpf.LinearColorAxis), GetType(OxyPlot.Wpf.AngleAxis), GetType(OxyPlot.Wpf.NormalProbabilityAxis), GetType(OxyPlot.Wpf.TimeSpanAxis), GetType(OxyPlot.Wpf.MagnitudeAxis)
                Select Case angle
                    Case = 0
                        dockPanel.RenderTransform = New RotateTransform(angle, 0, 0)
                        dockPanel.Width = Plot.ActualModel.PlotArea.Width
                        dockPanel.Height = height
                        Canvas.SetLeft(dockPanel, Plot.ActualModel.PlotArea.Left) 'Gets the parent canvas and moves the object
                        Canvas.SetTop(dockPanel, top) 'Gets the parent canvas and moves the object
                    Case = 270 'Vertical text - flowing up
                        dockPanel.RenderTransform = New RotateTransform(angle, 0, 0)
                        dockPanel.Width = Plot.ActualModel.PlotArea.Height
                        dockPanel.Height = height
                        Canvas.SetTop(dockPanel, Plot.ActualModel.PlotArea.Bottom) 'Gets the parent canvas and moves the object
                        Canvas.SetLeft(dockPanel, left) 'Gets the parent canvas and moves the object
                    Case Else
                        'angle not handled
                End Select
            Case GetType(OxyPlot.Wpf.ArrowAnnotation)
                dockPanel.RenderTransform = New RotateTransform(0, 0, 0) 'angle would rotate this here... but made everything horizontal
                Dim annotation As OxyPlot.Wpf.ArrowAnnotation = CType(dependencyObj, OxyPlot.Wpf.ArrowAnnotation)
                _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Left
                _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
                _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
                _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
                _textBox.Padding = New Thickness(0)

                'hide rotated annotation
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent

                Dim startPoint As ScreenPoint = annotation.InternalAnnotation.Transform(annotation.StartPoint.X, annotation.StartPoint.Y)

                dockPanel.Width = ExistingTextblock.Width + 2
                Canvas.SetTop(dockPanel, startPoint.Y - height) 'top
                Canvas.SetLeft(dockPanel, startPoint.X) 'left

            Case GetType(OxyPlot.Wpf.LineAnnotation)
                Dim annotation As OxyPlot.Wpf.LineAnnotation = CType(dependencyObj, OxyPlot.Wpf.LineAnnotation)
                _textBox.HorizontalAlignment = annotation.TextHorizontalAlignment
                _textBox.HorizontalContentAlignment = annotation.TextHorizontalAlignment
                _textBox.VerticalAlignment = annotation.TextVerticalAlignment
                _textBox.VerticalContentAlignment = annotation.TextVerticalAlignment
                _textBox.Padding = New Thickness(0)
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent

                'Apply horizontal alignment to each (simplifies commented code below)
                'dp.RenderTransform = New RotateTransform(0, 0, 0)
                'dp.Width = ExistingTextblock.ActualWidth + 4 'Plot.ActualModel.PlotAndAxisArea.Right - left
                'tbx.Width = dp.Width
                'Canvas.SetLeft(dp, left)
                'Canvas.SetTop(dp, top)

                Select Case annotation.Type
                    Case Annotations.LineAnnotationType.Horizontal
                        dockPanel.RenderTransform = New RotateTransform(0, 0, 0)
                        dockPanel.Width = ExistingTextblock.ActualWidth + 2 'Plot.ActualModel.PlotAndAxisArea.Right - left
                        _textBox.Width = dockPanel.Width
                        Canvas.SetLeft(dockPanel, left)
                        Canvas.SetTop(dockPanel, top)
                    Case Annotations.LineAnnotationType.Vertical
                        dockPanel.RenderTransform = New RotateTransform(0, 0, 0) '-90 will make this rotated, but it's hard to read
                        dockPanel.Width = ExistingTextblock.ActualWidth + 2 'Plot.ActualModel.PlotAndAxisArea.Right - left
                        _textBox.Width = dockPanel.Width
                        Canvas.SetLeft(dockPanel, left)
                        Canvas.SetTop(dockPanel, top)
                    Case Else 'linear equation
                        dockPanel.RenderTransform = New RotateTransform(0, 0, 0) 'Math.Atan(anno.Slope)
                        dockPanel.Width = ExistingTextblock.ActualWidth + 2 'Plot.ActualModel.PlotAndAxisArea.Right - left
                        _textBox.Width = dockPanel.Width
                        Canvas.SetLeft(dockPanel, left)
                        Canvas.SetTop(dockPanel, top)
                End Select

            Case GetType(OxyPlot.Wpf.PolygonAnnotation)
                Dim annotation As OxyPlot.Wpf.PolygonAnnotation = CType(dependencyObj, OxyPlot.Wpf.PolygonAnnotation)
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent
                'just make a generic textbox near the object
                _textBox.Width = ExistingTextblock.Width
                _textBox.TextAlignment = TextAlignment.Left
                _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Left
                _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
                _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
                _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
                _textBox.Padding = New Thickness(0)
                dockPanel.RenderTransform = New RotateTransform(0, 0, 0)
                dockPanel.Width = _textBox.Width
                dockPanel.Height = height
                Canvas.SetLeft(dockPanel, left)
                Canvas.SetTop(dockPanel, top)

            Case GetType(OxyPlot.Wpf.PolylineAnnotation)
                Dim annotation As OxyPlot.Wpf.PolylineAnnotation = CType(dependencyObj, OxyPlot.Wpf.PolylineAnnotation)
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent
                'just make a generic textbox near the object
                _textBox.Width = ExistingTextblock.Width
                _textBox.TextAlignment = TextAlignment.Left
                _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Left
                _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
                _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
                _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
                _textBox.Padding = New Thickness(0)
                dockPanel.RenderTransform = New RotateTransform(0, 0, 0)
                dockPanel.Width = _textBox.Width
                dockPanel.Height = height
                Canvas.SetLeft(dockPanel, left)
                Canvas.SetTop(dockPanel, top)

            Case GetType(OxyPlot.Wpf.PointAnnotation)
                Dim annotation As OxyPlot.Wpf.PointAnnotation = CType(dependencyObj, OxyPlot.Wpf.PointAnnotation)
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent
                'just make a generic textbox near the object
                _textBox.Width = ExistingTextblock.Width
                _textBox.TextAlignment = TextAlignment.Left
                _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Left
                _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
                _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
                _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
                _textBox.Padding = New Thickness(0)
                dockPanel.RenderTransform = New RotateTransform(0, 0, 0)
                dockPanel.Width = _textBox.Width
                dockPanel.Height = height
                Canvas.SetLeft(dockPanel, left)
                Canvas.SetTop(dockPanel, top)

            Case GetType(OxyPlot.Wpf.RectangleAnnotation)
                Dim annotation As OxyPlot.Wpf.RectangleAnnotation = CType(dependencyObj, OxyPlot.Wpf.RectangleAnnotation)
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent
                'just make a generic text box near the object
                _textBox.Width = ExistingTextblock.Width
                _textBox.TextAlignment = TextAlignment.Left
                _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Left
                _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
                _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
                _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
                _textBox.Padding = New Thickness(0)
                dockPanel.RenderTransform = New RotateTransform(0, 0, 0)
                dockPanel.Width = _textBox.Width
                dockPanel.Height = height
                Canvas.SetLeft(dockPanel, left)
                Canvas.SetTop(dockPanel, top)

            Case GetType(OxyPlot.Wpf.TextAnnotation)
                Dim annotation As OxyPlot.Wpf.TextAnnotation = CType(dependencyObj, OxyPlot.Wpf.TextAnnotation)
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent
                currentStrokeColor = annotation.Stroke
                annotation.Stroke = Colors.Transparent
                'just make a generic text box near the object
                _textBox.Width = ExistingTextblock.Width
                _textBox.TextAlignment = TextAlignment.Left
                _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Left
                _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
                _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
                _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
                _textBox.Padding = New Thickness(0)
                dockPanel.RenderTransform = New RotateTransform(0, 0, 0)
                dockPanel.Width = _textBox.Width
                dockPanel.Height = height
                Canvas.SetLeft(dockPanel, left)
                Canvas.SetTop(dockPanel, top)

            Case GetType(OxyPlot.Wpf.EllipseAnnotation)
                Dim annotation As OxyPlot.Wpf.EllipseAnnotation = CType(dependencyObj, OxyPlot.Wpf.EllipseAnnotation)
                currentTextColor = annotation.TextColor
                annotation.TextColor = Colors.Transparent
                'just make a generic text box near the object
                _textBox.Width = ExistingTextblock.Width
                _textBox.TextAlignment = TextAlignment.Left
                _textBox.HorizontalAlignment = Windows.HorizontalAlignment.Left
                _textBox.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
                _textBox.VerticalAlignment = Windows.VerticalAlignment.Center
                _textBox.VerticalContentAlignment = Windows.VerticalAlignment.Center
                _textBox.Padding = New Thickness(0)
                dockPanel.RenderTransform = New RotateTransform(0, 0, 0)
                dockPanel.Width = _textBox.Width
                dockPanel.Height = height
                Canvas.SetLeft(dockPanel, left)
                Canvas.SetTop(dockPanel, top)
            Case Else


        End Select

        ' Add the textbox to the dock panel and canvas. 
        dockPanel.Children.Add(_textBox)
        canvasOverlay.Children.Add(dockPanel)
        '
        ' Focus color from template is controlling here.
        _textBox.BorderThickness = New Thickness(1)
        _textBox.Focus()
        '
        ' Set up binding
        Dim binding As New Binding With {.Mode = BindingMode.OneWay, .Source = _textBox, .Path = New PropertyPath("Text")}
        '
        ' Check if dependency object is an axis, in which case, need to ignore Axis Units
        Select Case dependencyObj.GetType
            Case GetType(OxyPlot.Wpf.LogarithmicAxis), GetType(OxyPlot.Wpf.LinearAxis), GetType(OxyPlot.Wpf.DateTimeAxis), GetType(OxyPlot.Wpf.CategoryAxis), GetType(OxyPlot.Wpf.GumbelProbabilityAxis),
                 GetType(OxyPlot.Wpf.LinearColorAxis), GetType(OxyPlot.Wpf.AngleAxis), GetType(OxyPlot.Wpf.NormalProbabilityAxis), GetType(OxyPlot.Wpf.TimeSpanAxis), GetType(OxyPlot.Wpf.MagnitudeAxis)
                Dim axis As OxyPlot.Wpf.Axis = CType(dependencyObj, OxyPlot.Wpf.Axis)
                Dim title As String = axis.Title
                currentTextColor = axis.TitleColor
                axis.TitleColor = Colors.Transparent
                _textBox.Text = title 'Set up initial text
            Case Else
                _textBox.Text = ExistingTextblock.Text 'Set up initial text
        End Select

        ' Put the cursor at the end of the textbox
        If _textBox.Text.Length > 0 Then _textBox.SelectionStart = _textBox.Text.Length
        BindingOperations.SetBinding(dependencyObj, dependencyProp, binding)

        ' If the plot size changes, remove the textbox and binding.
        AddHandler Plot.SizeChanged, Sub()
                                         binding = Nothing
                                         plotParent.Children.Remove(canvasOverlay)
                                         ' This will also fire the lost focus event below. 
                                     End Sub


        ' On key enter, remove the textbox and binding.
        AddHandler _textBox.PreviewKeyDown, Sub(t As Object, ea As KeyEventArgs)
                                                If ea.Key = Key.Enter Then
                                                    binding = Nothing
                                                    plotParent.Children.Remove(canvasOverlay)
                                                    ' This will also fire the lost focus event below. 
                                                End If
                                            End Sub


        ' On lost focus, remove the textbox and binding. 
        AddHandler _textBox.LostFocus, Sub()
                                           binding = Nothing
                                           plotParent.Children.Remove(canvasOverlay)

                                           ' Change the color of the text back from transparent for annotations
                                           Select Case dependencyObj.GetType
                                               Case GetType(OxyPlot.Wpf.RectangleAnnotation)
                                                   Dim anno As OxyPlot.Wpf.RectangleAnnotation = CType(dependencyObj, OxyPlot.Wpf.RectangleAnnotation)
                                                   anno.TextColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.LineAnnotation)
                                                   Dim anno As OxyPlot.Wpf.LineAnnotation = CType(dependencyObj, OxyPlot.Wpf.LineAnnotation)
                                                   anno.TextColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.PolygonAnnotation)
                                                   Dim anno As OxyPlot.Wpf.PolygonAnnotation = CType(dependencyObj, OxyPlot.Wpf.PolygonAnnotation)
                                                   anno.TextColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.PolylineAnnotation)
                                                   Dim anno As OxyPlot.Wpf.PolylineAnnotation = CType(dependencyObj, OxyPlot.Wpf.PolylineAnnotation)
                                                   anno.TextColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.EllipseAnnotation)
                                                   Dim anno As OxyPlot.Wpf.EllipseAnnotation = CType(dependencyObj, OxyPlot.Wpf.EllipseAnnotation)
                                                   anno.TextColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.ArrowAnnotation)
                                                   Dim anno As OxyPlot.Wpf.ArrowAnnotation = CType(dependencyObj, OxyPlot.Wpf.ArrowAnnotation)
                                                   anno.TextColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.PointAnnotation)
                                                   Dim anno As OxyPlot.Wpf.PointAnnotation = CType(dependencyObj, OxyPlot.Wpf.PointAnnotation)
                                                   anno.TextColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.TextAnnotation)
                                                   Dim anno As OxyPlot.Wpf.TextAnnotation = CType(dependencyObj, OxyPlot.Wpf.TextAnnotation)
                                                   anno.TextColor = currentTextColor
                                                   anno.Stroke = currentStrokeColor
                                               Case GetType(OxyPlot.Wpf.LogarithmicAxis), GetType(OxyPlot.Wpf.LinearAxis), GetType(OxyPlot.Wpf.DateTimeAxis), GetType(OxyPlot.Wpf.CategoryAxis), GetType(OxyPlot.Wpf.GumbelProbabilityAxis),
                                                GetType(OxyPlot.Wpf.LinearColorAxis), GetType(OxyPlot.Wpf.AngleAxis), GetType(OxyPlot.Wpf.NormalProbabilityAxis), GetType(OxyPlot.Wpf.TimeSpanAxis), GetType(OxyPlot.Wpf.MagnitudeAxis)
                                                   Dim ax As OxyPlot.Wpf.Axis = CType(dependencyObj, OxyPlot.Wpf.Axis)
                                                   ax.TitleColor = currentTextColor
                                               Case GetType(OxyPlot.Wpf.Plot)
                                                   Plot.TitleColor = currentTextColor
                                               Case Else
                                                   'do nothing
                                           End Select
                                       End Sub

    End Sub

    ''' <summary>
    ''' Support method for finding the visual child. 
    ''' </summary>
    Public Iterator Function FindVisualChildren(Of T As DependencyObject)(ByVal depObj As DependencyObject) As IEnumerable(Of T)
        If depObj IsNot Nothing Then

            For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(depObj) - 1
                Dim child As DependencyObject = VisualTreeHelper.GetChild(depObj, i)

                If child IsNot Nothing AndAlso TypeOf child Is T Then
                    Yield CType(child, T)
                End If

                For Each childOfChild As T In FindVisualChildren(Of T)(child)
                    Yield childOfChild
                Next
            Next
        End If
    End Function

    Private Function ConvertScreenPointToDataPoint(pt As ScreenPoint) As DataPoint
        Return Plot.ActualModel.DefaultXAxis.InverseTransform(pt.X, pt.Y, Plot.ActualModel.DefaultYAxis)
    End Function
    Private Function ConvertDataPointToScreenPoint(pt As DataPoint) As ScreenPoint
        Return Plot.ActualModel.DefaultXAxis.Transform(pt.X, pt.Y, Plot.ActualModel.DefaultYAxis)
    End Function
    Private Function ConvertDataPointToPoint(pt As DataPoint) As Point
        Dim sp = Plot.ActualModel.DefaultXAxis.Transform(pt.X, pt.Y, Plot.ActualModel.DefaultYAxis)
        '
        Return New Point(sp.X, sp.Y)
    End Function
    Private Function ConvertLeaderLinePoint(pointIndex As Int32) As DataPoint
        Return Plot.ActualModel.DefaultXAxis.InverseTransform(_leaderLine.Points(pointIndex).X, _leaderLine.Points(pointIndex).Y, Plot.ActualModel.DefaultYAxis)
    End Function

    Private Function MeasureString(ByVal candidate As String, Family As FontFamily, Style As FontStyle, Weight As FontWeight, Stretch As FontStretch, size As Double) As Size
        If candidate Is Nothing Then
            Return New Size(0, 0)
        End If
        Dim formattedText = New FormattedText(candidate, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, New Typeface(Family, Style, Weight, Stretch), size, Brushes.Black, New NumberSubstitution())
        Return New Size(formattedText.Width, formattedText.Height)
    End Function

#End Region

#Region "Export Series Data"

    Private Sub ExportDataButton_Click(sender As Object, e As RoutedEventArgs)

        Dim tableList As New List(Of DataTable)
        Dim tableCount As Integer = 0
        Dim badCharacters() As String = {":", "\", "/", "?", "*", "[", "]"}

        For Each series As OxyPlot.Wpf.Series In Plot.Series

            Dim dataTable As New System.Data.DataTable("Series")
            Dim seriesName As String = ""
            tableCount += 1

            Select Case series.GetType
                Case GetType(OxyPlot.Wpf.LineSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "LineSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName = seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_x", GetType(String))
                    dataTable.Columns.Add(seriesName & "_y", GetType(String))

                    Dim oxySeries As OxyPlot.Series.LineSeries = CType(series.InternalSeries, OxyPlot.Series.LineSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        'Can be list, observable collection, ienumerable, etc
                        Dim datalist As IEnumerable(Of OxyPlot.DataPoint) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.DataPoint))

                        'Check if can cast datapoints directly
                        If datalist IsNot Nothing Then
                            For Each seriesValue As DataPoint In datalist.ToList
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y})
                            Next
                        Else
                            'It's not a list of datapoint
                            For Each obj As Object In oxySeries.ItemsSource.Cast(Of Object)
                                Dim propX As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldX)
                                Dim xVal = CType(propX.GetValue(obj, Nothing), String)
                                Dim propY As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldY)
                                Dim yVal = CType(propY.GetValue(obj, Nothing), String)
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, xVal, yVal})
                            Next
                        End If

                    Else
                        'get the points from the internal collection
                        For Each seriesValue As DataPoint In oxySeries.Points
                            dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y})
                        Next
                    End If

                Case GetType(OxyPlot.Wpf.ScatterPointSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "ScatterSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_x", GetType(String))
                    dataTable.Columns.Add(seriesName & "_y", GetType(String))
                    'dataTable.Columns.Add(seriesName & "_size", GetType(String))
                    'dataTable.Columns.Add(seriesName & "_value", GetType(String))

                    Dim oxySeries As OxyPlot.Series.ScatterSeries = CType(series.InternalSeries, OxyPlot.Series.ScatterSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        'Can be list, observable collection, ienumerable, etc
                        Dim datalist As IEnumerable(Of OxyPlot.Series.ScatterPoint) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.Series.ScatterPoint))

                        'Check if can cast datapoints directly
                        If datalist IsNot Nothing Then
                            For Each seriesValue As OxyPlot.Series.ScatterPoint In datalist.ToList
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y}) ', seriesValue.Size, seriesValue.Value})
                            Next
                        Else
                            'It's not a list of datapoint
                            For Each obj As Object In oxySeries.ItemsSource.Cast(Of Object)
                                Dim propX As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldX)
                                Dim xVal = CType(propX.GetValue(obj, Nothing), String)

                                Dim propY As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldY)
                                Dim yVal = CType(propY.GetValue(obj, Nothing), String)

                                'Dim sizeVal As String = ""
                                'If oxySeries.DataFieldSize IsNot Nothing Then
                                '    Dim propSize As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldSize)
                                '    sizeVal = CType(propSize.GetValue(obj, Nothing), String)
                                'End If

                                'Dim val As String = ""
                                'If oxySeries.DataFieldValue IsNot Nothing Then
                                '    Dim propValue As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldValue)
                                '    val = CType(propValue.GetValue(obj, Nothing), String)
                                'End If
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, xVal, yVal}) ', sizeVal, val})
                            Next
                        End If

                    Else
                        'get the points from the internal collection
                        For Each seriesValue As OxyPlot.Series.ScatterPoint In oxySeries.Points
                            dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y, seriesValue.Size, seriesValue.Value})
                        Next

                    End If

                Case GetType(OxyPlot.Wpf.HistogramSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "HistogramSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_rangeStart", GetType(String))
                    dataTable.Columns.Add(seriesName & "_rangeEnd", GetType(String))
                    dataTable.Columns.Add(seriesName & "_area", GetType(String))

                    Dim oxySeries As OxyPlot.Series.HistogramSeries = CType(series.InternalSeries, OxyPlot.Series.HistogramSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        'Can be list, observable collection, ienumerable, etc
                        Dim datalist As IEnumerable(Of OxyPlot.Series.HistogramItem) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.Series.HistogramItem))

                        'Check if can cast datapoints directly
                        If datalist IsNot Nothing Then
                            For Each seriesValue As OxyPlot.Series.HistogramItem In datalist.ToList
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.RangeStart, seriesValue.RangeEnd, seriesValue.Area})
                            Next

                        Else
                            'I don't think this series type allows for this to happen (and work)
                        End If

                    Else
                        'Internal collection.  I don't think this can actually happen
                        For Each seriesItem As OxyPlot.Series.HistogramItem In oxySeries.Items
                            dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesItem.RangeStart, seriesItem.RangeEnd, seriesItem.Area})
                        Next

                    End If

                Case GetType(OxyPlot.Wpf.ColumnSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "ColumnSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName = seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_categoryIndex", GetType(String))
                    dataTable.Columns.Add(seriesName & "_color", GetType(String))
                    dataTable.Columns.Add(seriesName & "_value", GetType(String))

                    Dim oxySeries As OxyPlot.Series.ColumnSeries = CType(series.InternalSeries, OxyPlot.Series.ColumnSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        'Can be list, observable collection, ienumerable, etc
                        Dim datalist As IEnumerable(Of OxyPlot.Series.ColumnItem) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.Series.ColumnItem))

                        'Check if can cast datapoints directly
                        If datalist IsNot Nothing Then

                            For Each seriesItem As OxyPlot.Series.ColumnItem In datalist.ToList
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName, seriesItem.Value})
                            Next

                        Else
                            'Item source is not a column item enumerable
                            Dim c As Integer = 0
                            For Each obj As Object In oxySeries.ItemsSource.Cast(Of Object)
                                Dim colorVal As String = ""
                                If oxySeries.ColorField IsNot Nothing Then
                                    Dim propColor As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.ColorField)
                                    colorVal = CType(propColor.GetValue(obj, Nothing), String)
                                End If

                                Dim valueVal As String = ""
                                If oxySeries.ValueField IsNot Nothing Then
                                    Dim propValue As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.ValueField)
                                    valueVal = CType(propValue.GetValue(obj, Nothing), String)
                                End If

                                dataTable.Rows.Add({dataTable.Rows.Count + 1, c, colorVal, valueVal})

                                c += 1
                            Next
                        End If

                    Else
                        'Note: It doesn't seem it is possible to get here, try to get internal collection
                        For Each seriesItem As OxyPlot.Series.ColumnItem In oxySeries.Items
                            dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName, seriesItem.Value})
                        Next

                    End If

                Case GetType(OxyPlot.Wpf.BarSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "BarSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName = seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_categoryIndex", GetType(String))
                    dataTable.Columns.Add(seriesName & "_color", GetType(String))
                    dataTable.Columns.Add(seriesName & "_value", GetType(String))

                    Dim oxySeries As OxyPlot.Series.BarSeries = CType(series.InternalSeries, OxyPlot.Series.BarSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then
                        'Can be list, observable collection, ienumerable, etc
                        Dim datalist As IEnumerable(Of OxyPlot.Series.BarItem) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.Series.BarItem))

                        If datalist IsNot Nothing Then

                            For Each seriesItem As OxyPlot.Series.BarItem In datalist.ToList
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName, seriesItem.Value})
                            Next

                        Else
                            'Item source is not a bar item enumerable
                            Dim c As Integer = 0
                            For Each obj As Object In oxySeries.ItemsSource.Cast(Of Object)
                                Dim colorVal As String = ""
                                If oxySeries.ColorField IsNot Nothing Then
                                    Dim propColor As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.ColorField)
                                    colorVal = CType(propColor.GetValue(obj, Nothing), String)
                                End If

                                Dim valueVal As String = ""
                                If oxySeries.ValueField IsNot Nothing Then
                                    Dim propValue As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.ValueField)
                                    valueVal = CType(propValue.GetValue(obj, Nothing), String)
                                End If

                                dataTable.Rows.Add({dataTable.Rows.Count + 1, c, colorVal, valueVal})

                                c += 1
                            Next
                        End If

                    Else
                        For Each seriesItem As OxyPlot.Series.BarItem In oxySeries.Items
                            dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName, seriesItem.Value})
                        Next

                    End If

                Case GetType(OxyPlot.Wpf.AreaSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "AreaSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_x", GetType(String))
                    dataTable.Columns.Add(seriesName & "_y", GetType(String))
                    dataTable.Columns.Add(seriesName & "_x2", GetType(String))
                    dataTable.Columns.Add(seriesName & "_y2", GetType(String))

                    Dim oxySeries As OxyPlot.Series.AreaSeries = CType(series.InternalSeries, OxyPlot.Series.AreaSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        Dim s As String = oxySeries.ItemsSource.GetType.ToString

                        'TODO - is this object of the type DATAPOINTSERIES, and not datapoint?  Check if can cast datapoints directly
                        'CHECK IF THIS IS A DATAPOINT TYPE? OR SOMETHING ELSE?
                        Dim datalist As IEnumerable(Of OxyPlot.DataPoint) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.DataPoint))

                        If datalist IsNot Nothing Then

                            'Is is even possible to get here?
                            For Each seriesValue As OxyPlot.DataPoint In datalist.ToList
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y})
                            Next

                        Else

                            'It's not a list of the itemsource type
                            For Each obj As Object In oxySeries.ItemsSource.Cast(Of Object)
                                Dim propX As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldX)
                                Dim xVal = CType(propX.GetValue(obj, Nothing), String)

                                Dim propY As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldY)
                                Dim yVal = CType(propY.GetValue(obj, Nothing), String)

                                Dim xVal2 As String = ""
                                If oxySeries.DataFieldX2 IsNot Nothing Then
                                    Dim propX2 As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldX2)
                                    xVal2 = CType(propX2.GetValue(obj, Nothing), String)
                                End If

                                Dim yVal2 As String = ""
                                If oxySeries.DataFieldY2 IsNot Nothing Then
                                    Dim propY2 As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldY2)
                                    yVal2 = CType(propY2.GetValue(obj, Nothing), String)
                                End If

                                dataTable.Rows.Add({dataTable.Rows.Count + 1, xVal, yVal, xVal2, yVal2})

                            Next
                        End If

                    Else

                        'Get the points from the internal collection
                        For i As Integer = 0 To (oxySeries.Points.Count - 1)

                            If oxySeries.Points2.Count > 0 Then
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, oxySeries.Points(i).X, oxySeries.Points(i).Y, oxySeries.Points2(i).X, oxySeries.Points2(i).Y})
                            Else
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, oxySeries.Points(i).X, oxySeries.Points(i).Y, "", ""})
                            End If

                        Next i

                    End If

                Case GetType(OxyPlot.Wpf.BoxPlotSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "BoxPlotSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_position", GetType(String))
                    dataTable.Columns.Add(seriesName & "_lowerWhisker", GetType(String))
                    dataTable.Columns.Add(seriesName & "_boxMinimum", GetType(String))
                    dataTable.Columns.Add(seriesName & "_median", GetType(String))
                    dataTable.Columns.Add(seriesName & "_boxMaximum", GetType(String))
                    dataTable.Columns.Add(seriesName & "_upperWhisker", GetType(String))
                    dataTable.Columns.Add(seriesName & "_label", GetType(String))

                    Dim oxySeries As OxyPlot.Series.BoxPlotSeries = CType(series.InternalSeries, OxyPlot.Series.BoxPlotSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        Dim datalist As IEnumerable(Of OxyPlot.Series.BoxPlotItem) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.Series.BoxPlotItem))

                        If datalist IsNot Nothing Then

                            For Each bpi As OxyPlot.Series.BoxPlotItem In datalist
                                Dim r As DataRow = dataTable.Rows.Add({dataTable.Rows.Count + 1, bpi.Position, bpi.LowerWhisker, bpi.BoxMinimum, bpi.Median, bpi.BoxMaximum, bpi.UpperWhisker})

                                'Check if a X Axis Label is specified
                                If oxySeries.XAxis IsNot Nothing Then
                                    If oxySeries.XAxis.GetType = GetType(OxyPlot.Axes.CategoryAxis) Then
                                        If CType(oxySeries.XAxis, OxyPlot.Axes.CategoryAxis).LabelField IsNot Nothing Then
                                            r.Item("label") = CType(oxySeries.XAxis, OxyPlot.Axes.CategoryAxis).LabelField
                                        End If
                                    End If
                                End If

                                'Add any outliers as additional columns

                                Dim i As Integer = 1
                                For Each outlier As String In bpi.Outliers
                                    If Not dataTable.Columns.Contains("outlier" & i) Then
                                        dataTable.Columns.Add("outlier" & i, GetType(String))
                                    End If
                                    r(dataTable.Columns.IndexOf("outlier" + CStr(i))) = outlier
                                    i += 1
                                Next outlier

                            Next

                        Else
                            'It's not a list of the itemsource type
                            'This really can't happen b/c there aren't fields 
                        End If

                    Else

                        'Get the points from the items internal collection
                        For i As Integer = 0 To (oxySeries.Items.Count - 1)
                            Dim r As DataRow = dataTable.Rows.Add({dataTable.Rows.Count + 1, oxySeries.Items(i).Position, oxySeries.Items(i).LowerWhisker, oxySeries.Items(i).BoxMinimum, oxySeries.Items(i).Median, oxySeries.Items(i).BoxMaximum, oxySeries.Items(i).UpperWhisker})

                            'Check if a X Axis Label is specified
                            If oxySeries.XAxis IsNot Nothing Then
                                If oxySeries.XAxis.GetType = GetType(OxyPlot.Axes.CategoryAxis) Then
                                    If CType(oxySeries.XAxis, OxyPlot.Axes.CategoryAxis).LabelField IsNot Nothing Then
                                        r.Item("label") = CType(oxySeries.XAxis, OxyPlot.Axes.CategoryAxis).LabelField
                                    End If
                                End If
                            End If
                            'Add any outliers
                            Dim j As Integer = 1
                            For Each outlier As String In oxySeries.Items(i).Outliers
                                If Not dataTable.Columns.Contains("outlier" & j) Then
                                    dataTable.Columns.Add("outlier" & j, GetType(String))
                                End If
                                r(dataTable.Columns.IndexOf("outlier" + CStr(j))) = outlier
                                j += 1
                            Next outlier
                        Next i

                    End If

                Case GetType(OxyPlot.Wpf.HeatMapSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "HeatMapSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add("xy", GetType(String))

                    Dim oxySeries As OxyPlot.Series.HeatMapSeries = CType(series.InternalSeries, OxyPlot.Series.HeatMapSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        Dim datalist As IEnumerable(Of Double(,)) = TryCast(oxySeries.ItemsSource, IEnumerable(Of Double(,)))

                        If datalist IsNot Nothing Then

                            'Add columns
                            Dim x0 As Double = oxySeries.X0
                            Dim x1 As Double = oxySeries.X1
                            Dim xN As Integer = datalist.ToArray.GetLength(0) - 1
                            Dim xDelta As Double = (x1 - x0) / xN
                            dataTable.Columns.Add(x0.ToString(), GetType(String))
                            For i As Integer = 1 To datalist.ToArray.GetLength(0) - 1
                                x0 += xDelta
                                dataTable.Columns.Add(x0.ToString(), GetType(String))
                            Next
                            'Add rows
                            Dim y0 As Double = oxySeries.Y0
                            Dim y1 As Double = oxySeries.Y1
                            Dim yN As Integer = datalist.ToArray.GetLength(1) - 1
                            Dim yDelta As Double = (y1 - y0) / yN
                            dataTable.Rows.Add({})
                            dataTable.Rows(0).Item(0) = 1
                            dataTable.Rows(0).Item(1) = y0

                            For j As Integer = 1 To datalist.ToArray.GetLength(1) - 1
                                dataTable.Rows.Add({})
                                y0 += yDelta
                                dataTable.Rows(j).Item(0) = j + 1
                                dataTable.Rows(j).Item(1) = y0
                            Next
                            'Fill in matrix
                            For x As Integer = 0 To datalist.ToArray.GetLength(0) - 1
                                Dim xy(,) As Double = datalist.ToArray.ElementAt(x)
                                For y As Integer = 0 To datalist.ToArray.GetLength(1) - 1
                                    dataTable.Rows(y).Item(x + 2) = xy(x, y) '+1 b/c first column is id
                                Next
                            Next
                        Else
                            'TODO It's not a list of the itemsource type
                            'I don't think this can happen

                        End If

                    Else

                        'Add columns
                        Dim x0 As Double = oxySeries.X0
                        Dim x1 As Double = oxySeries.X1
                        Dim xN As Integer = oxySeries.Data.GetLength(0) - 1
                        Dim xDelta As Double = (x1 - x0) / xN
                        dataTable.Columns.Add(x0.ToString(), GetType(String))
                        For i As Integer = 1 To oxySeries.Data.GetLength(0) - 1
                            x0 += xDelta
                            dataTable.Columns.Add(x0.ToString(), GetType(String))
                        Next
                        'Add rows
                        Dim y0 As Double = oxySeries.Y0
                        Dim y1 As Double = oxySeries.Y1
                        Dim yN As Integer = oxySeries.Data.GetLength(1) - 1
                        Dim yDelta As Double = (y1 - y0) / yN
                        dataTable.Rows.Add({})
                        dataTable.Rows(0).Item(0) = 1
                        dataTable.Rows(0).Item(1) = y0

                        For j As Integer = 1 To oxySeries.Data.GetLength(1) - 1
                            dataTable.Rows.Add({})
                            y0 += yDelta
                            dataTable.Rows(j).Item(0) = j + 1
                            dataTable.Rows(j).Item(1) = y0
                        Next
                        'Fill in matrix
                        For x As Integer = 0 To oxySeries.Data.GetLength(0) - 1
                            For y As Integer = 0 To oxySeries.Data.GetLength(1) - 1
                                dataTable.Rows(y).Item(x + 2) = oxySeries.Data(x, y) '+1 b/c first column is id
                            Next
                        Next

                    End If

                Case GetType(OxyPlot.Wpf.ScatterErrorSeries)

                    If series.Title IsNot Nothing AndAlso series.Title <> "" Then
                        seriesName = series.Title
                    Else
                        seriesName = "ScatterErrorSeries_" & tableCount
                    End If
                    For i As Int32 = 0 To badCharacters.Length - 1
                        seriesName.Replace(badCharacters(i), "_")
                    Next

                    dataTable.TableName = seriesName
                    dataTable.Columns.Add("id", GetType(Integer))
                    dataTable.Columns.Add(seriesName & "_xLower", GetType(String))
                    dataTable.Columns.Add(seriesName & "_x", GetType(String))
                    dataTable.Columns.Add(seriesName & "_xUpper", GetType(String))
                    dataTable.Columns.Add(seriesName & "_yLower", GetType(String))
                    dataTable.Columns.Add(seriesName & "_y", GetType(String))
                    dataTable.Columns.Add(seriesName & "_yUpper", GetType(String))
                    'dataTable.Columns.Add(seriesName & "_size", GetType(String))
                    'dataTable.Columns.Add(seriesName & "_value", GetType(String))

                    Dim oxySeries As OxyPlot.Series.ScatterErrorSeries = CType(series.InternalSeries, OxyPlot.Series.ScatterErrorSeries)

                    If oxySeries.ItemsSource IsNot Nothing Then

                        'Can be list, observable collection, ienumerable, etc
                        Dim datalist As IEnumerable(Of OxyPlot.Series.ScatterPoint) = TryCast(oxySeries.ItemsSource, IEnumerable(Of OxyPlot.Series.ScatterPoint))

                        'Check if can cast datapoints directly
                        If datalist IsNot Nothing Then

                            For Each seriesValue As OxyPlot.Series.ScatterErrorPoint In datalist.ToList
                                dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.LowerErrorX, seriesValue.X, seriesValue.UpperErrorX, seriesValue.LowerErrorY, seriesValue.Y, seriesValue.UpperErrorY, seriesValue.Size, seriesValue.Value})
                            Next

                        Else
                            'It's not a list of scatter error point
                            For Each obj As Object In oxySeries.ItemsSource.Cast(Of Object)

                                Dim propX As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldX)
                                Dim xVal = CType(propX.GetValue(obj, Nothing), String)

                                Dim propY As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldY)
                                Dim yVal = CType(propY.GetValue(obj, Nothing), String)

                                'Dim sizeVal As String = ""
                                'If oxySeries.DataFieldSize IsNot Nothing Then
                                '    Dim propSize As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldSize)
                                '    sizeVal = CType(propSize.GetValue(obj, Nothing), String)
                                'End If

                                'Dim val As String = ""
                                'If oxySeries.DataFieldValue IsNot Nothing Then
                                '    Dim propValue As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldValue)
                                '    val = CType(propValue.GetValue(obj, Nothing), String)
                                'End If

                                Dim xLower As String = ""
                                If oxySeries.DataFieldLowerErrorX IsNot Nothing Then
                                    Dim propXlower As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldLowerErrorX)
                                    xLower = CType(propXlower.GetValue(obj, Nothing), String)
                                End If

                                Dim xUpper As String = ""
                                If oxySeries.DataFieldUpperErrorX IsNot Nothing Then
                                    Dim propXupper As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldUpperErrorX)
                                    xUpper = CType(propXupper.GetValue(obj, Nothing), String)
                                End If

                                Dim yLower As String = ""
                                If oxySeries.DataFieldLowerErrorY IsNot Nothing Then
                                    Dim propYlower As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldLowerErrorY)
                                    yLower = CType(propYlower.GetValue(obj, Nothing), String)
                                End If


                                Dim yUpper As String = ""
                                If oxySeries.DataFieldUpperErrorY IsNot Nothing Then
                                    Dim propYupper As Reflection.PropertyInfo = obj.GetType().GetProperty(oxySeries.DataFieldUpperErrorY)
                                    yUpper = CType(propYupper.GetValue(obj, Nothing), String)
                                End If


                                dataTable.Rows.Add({dataTable.Rows.Count + 1, xLower, xVal, xUpper, yLower, yVal, yUpper}) ', sizeVal, Val()})

                            Next
                        End If

                    Else
                        'get the points from the internal collection
                        For Each seriesValue As OxyPlot.Series.ScatterErrorPoint In oxySeries.Points
                            dataTable.Rows.Add({dataTable.Rows.Count + 1, seriesValue.X, seriesValue.LowerErrorX, seriesValue.UpperErrorX, seriesValue.Y, seriesValue.LowerErrorY, seriesValue.UpperErrorY, seriesValue.Size, seriesValue.Value})
                        Next

                    End If

                Case Else
                    'can't export this type
            End Select

            ' Check for item source on a x category axis
            Dim xCat As OxyPlot.Axes.CategoryAxis = TryCast(CType(series.InternalSeries, OxyPlot.Series.XYAxisSeries).XAxis, OxyPlot.Axes.CategoryAxis)
            If xCat IsNot Nothing Then
                dataTable.Columns.Add("Xcategory", GetType(String))

                If xCat.ItemsSource IsNot Nothing Then
                    Dim xCatList As IEnumerable(Of String) = TryCast(xCat.ItemsSource, IEnumerable(Of String))
                    If xCatList IsNot Nothing Then
                        For i As Integer = 0 To dataTable.Rows.Count - 1
                            dataTable.Rows(i).Item("Xcategory") = xCatList(i)
                        Next
                    Else
                        'it's part of an object to be unraveled
                        Dim i As Integer = 0
                        For Each obj As Object In xCat.ItemsSource.Cast(Of Object)
                            Dim propLabel As Reflection.PropertyInfo = obj.GetType().GetProperty(xCat.LabelField)
                            Dim xVal = CType(propLabel.GetValue(obj, Nothing), String)
                            dataTable.Rows(i).Item("Xcategory") = xVal
                        Next

                    End If
                End If
            Else
                'Check for item source on a y category axis
                Dim yCat As OxyPlot.Axes.CategoryAxis = TryCast(CType(series.InternalSeries, OxyPlot.Series.XYAxisSeries).YAxis, OxyPlot.Axes.CategoryAxis)
                If yCat IsNot Nothing Then
                    dataTable.Columns.Add("Ycategory", GetType(String))

                    If yCat.ItemsSource IsNot Nothing Then
                        Dim yCatList As IEnumerable(Of String) = TryCast(yCat.ItemsSource, IEnumerable(Of String))
                        If yCatList IsNot Nothing Then
                            For i As Integer = 0 To dataTable.Rows.Count - 1
                                dataTable.Rows(i).Item("Ycategory") = yCatList(i)
                            Next
                        Else
                            'it's part of an object to be unraveled
                            Dim i As Integer = 0
                            For Each obj As Object In yCat.ItemsSource.Cast(Of Object)
                                Dim propLabel As Reflection.PropertyInfo = obj.GetType().GetProperty(yCat.LabelField)
                                Dim xVal = CType(propLabel.GetValue(obj, Nothing), String)
                                dataTable.Rows(i).Item("Ycategory") = xVal
                            Next
                        End If
                    End If
                End If
            End If


            ' Skip over contour series for now. It doesn't add any value at this time.
            If series.GetType() = GetType(OxyPlot.Wpf.ContourSeries) Then Continue For

            tableList.Add(dataTable)

        Next

        Dim filters As String = "comma delimited(*.csv) |*.csv|Excel(*.xlsx) |*.xlsx|Sqlite(*.sqlite) |*.sqlite"
        Try
            Dim saveFileBrowser As New Microsoft.Win32.SaveFileDialog With {.Filter = filters, .FilterIndex = 1}
            If saveFileBrowser.ShowDialog = True Then

                Select Case System.IO.Path.GetExtension(saveFileBrowser.FileName.ToString)
                    Case ".csv"

                        'Combine all the tables b/c csvs only have one sheet/table
                        Dim uniqueCount As Integer = 1
                        Dim colNames As New List(Of String)
                        For Each theDT As DataTable In tableList

                            For i As Integer = 0 To theDT.Columns.Count - 1
                                If theDT.Columns(i).ColumnName = "id" Then
                                    Continue For
                                End If

                                If Not colNames.Contains(theDT.Columns(i).ColumnName) Then
                                    colNames.Add(theDT.Columns(i).ColumnName)
                                Else
                                    Do While colNames.Contains(theDT.Columns(i).ColumnName)
                                        theDT.Columns(theDT.Columns(i).ColumnName).ColumnName = theDT.Columns(i).ColumnName + "_" + CStr(uniqueCount)
                                    Loop

                                    colNames.Add(theDT.Columns(i).ColumnName)
                                End If
                            Next

                        Next

                        'Get the series data in Database_Reader.DataTableView format
                        Dim totalDT As New System.Data.DataTable("Exported_Data")
                        totalDT = MergeAll(tableList, "id")
                        'totalDT.Columns.Remove("id")

                        Dim DataView As DatabaseManager.DataTableView = New DatabaseManager.InMemoryReader(totalDT).GetTableManager(totalDT.TableName)

                        DataView.ExportToCsv(saveFileBrowser.FileName.ToString)

                    Case ".xlsx"

                        'Save each DT to the file (as new sheet)
                        For Each dt As DataTable In tableList
                            'dt.Columns.Remove("id")

                            Dim DataView As DatabaseManager.DataTableView = New DatabaseManager.InMemoryReader(dt).GetTableManager(dt.TableName)

                            ' Use the DataTableView to export to desired format.
                            If IsNothing(DataView) Then Continue For

                            DataView.ExportToXlsx(saveFileBrowser.FileName.ToString)
                        Next


                    Case ".sqlite"

                        'Save each DT to the file (as new table)
                        For Each dt As DataTable In tableList
                            'dt.Columns.Remove("id")

                            Dim DataView As DatabaseManager.DataTableView = New DatabaseManager.InMemoryReader(dt).GetTableManager(dt.TableName)

                            ' Use the DataTableView to export to desired format.
                            If IsNothing(DataView) Then Continue For

                            DataView.ExportToSqlite(saveFileBrowser.FileName.ToString, DataView.TableName)
                        Next

                    Case Else
                        Throw New Exception("selected file format extension '" & IO.Path.GetExtension(saveFileBrowser.FileName.ToString) & "' is not supported for export.")
                End Select
            Else

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function MergeAll(ByVal tables As IList(Of DataTable), ByVal primaryKeyColumn As String) As DataTable
        If Not tables.Any() Then Throw New ArgumentException("Tables must not be empty", "tables")

        If primaryKeyColumn IsNot Nothing Then

            For Each t As DataTable In tables
                If Not t.Columns.Contains(primaryKeyColumn) Then Throw New ArgumentException("All tables must have the specified primarykey column " & primaryKeyColumn, "primaryKeyColumn")
            Next
        End If

        If tables.Count = 1 Then Return tables(0)
        Dim table As DataTable = New DataTable("TblUnion")
        table.BeginLoadData()

        For Each t As DataTable In tables
            table.Merge(t)
        Next

        table.EndLoadData()

        If primaryKeyColumn IsNot Nothing Then
            Dim pkGroups = table.AsEnumerable().GroupBy(Function(r) r(primaryKeyColumn))
            Dim dupGroups = pkGroups.Where(Function(g) g.Count() > 1)

            For Each grpDup In dupGroups
                Dim firstRow As DataRow = grpDup.First()

                For Each c As DataColumn In table.Columns

                    If firstRow.IsNull(c) Then
                        Dim firstNotNullRow As DataRow = grpDup.Skip(1).FirstOrDefault(Function(r) Not r.IsNull(c))
                        If firstNotNullRow IsNot Nothing Then firstRow(c) = firstNotNullRow(c)
                    End If
                Next

                Dim rowsToRemove = grpDup.Skip(1)

                For Each rowToRemove As DataRow In rowsToRemove
                    table.Rows.Remove(rowToRemove)
                Next
            Next
        End If

        Return table
    End Function

    Public Function MergeTablesByIndex(ByVal t1 As DataTable, ByVal t2 As DataTable) As DataTable
        If t1 Is Nothing OrElse t2 Is Nothing Then Return Nothing
        Dim t3 As DataTable = t1.Clone()

        For Each col As DataColumn In t2.Columns
            Dim newColumnName As String = col.ColumnName
            Dim colNum As Integer = 1

            While t3.Columns.Contains(newColumnName)
                newColumnName = String.Format("{0}_{1}", col.ColumnName, System.Threading.Interlocked.Increment(colNum))
            End While

            t3.Columns.Add(newColumnName, col.DataType)
        Next

        Dim mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(), Function(r1, r2) r1.ItemArray.Concat(r2.ItemArray).ToArray())

        For Each rowFields As Object() In mergedRows
            t3.Rows.Add(rowFields)
        Next

        Return t3
    End Function

#End Region

#Region "Save Plot"

    ''' <summary>
    ''' On Click, open the save plot image dialog. 
    ''' </summary>
    Private Sub SaveImageButton_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(Plot) Then Exit Sub
        Dim saveImageDialog As New SavePlotImageDialog(Plot) With {.Owner = Window.GetWindow(Me)}
        saveImageDialog.ShowDialog()
    End Sub

#End Region

#Region "Open Plot Properties"

    '''' <summary>
    '''' Open the plot properties.
    '''' </summary>
    'Private Sub PropertiesButton_Checked(sender As Object, e As RoutedEventArgs)
    '    If IsPlotPropertiesOpen = False Then
    '        RaiseEvent PropertiesCalled(Plot)
    '        _IsPlotPropertiesOpen = True
    '        PropertiesButton.ToolTip = "Close Plot Properties"
    '    End If
    'End Sub

    '''' <summary>
    '''' Close the plot properties.
    '''' </summary>
    'Private Sub PropertiesButton_Unchecked(sender As Object, e As RoutedEventArgs)
    '    If IsPlotPropertiesOpen = True Then
    '        RaiseEvent ClosePropertiesCalled()
    '        _IsPlotPropertiesOpen = False
    '        PropertiesButton.ToolTip = "Open Plot Properties"
    '    End If
    'End Sub

    '''' <summary>
    '''' Close the plot properties.
    '''' </summary>
    'Public Sub ClosePlotProperties()
    '    _IsPlotPropertiesOpen = False
    '    PropertiesButton.IsChecked = False
    '    PropertiesButton.ToolTip = "Open Plot Properties"
    'End Sub

    Private Sub PropertiesButton_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent PropertiesCalled(Plot, True, Nothing, Nothing)
    End Sub

    Private Shared _nonSwapSeriesTypes As New HashSet(Of Type)({GetType(OxyPlot.Wpf.HistogramSeries), GetType(OxyPlot.Wpf.BarSeries), GetType(OxyPlot.Wpf.ColumnSeries), GetType(OxyPlot.Wpf.HeatMapSeries)})
    Private Sub SwapAxesButton_Click(sender As Object, e As RoutedEventArgs)
        For Each s In Plot.Series
            If _nonSwapSeriesTypes.Contains(s.GetType()) Then Exit Sub
        Next

        For Each axis In Plot.Axes
            If axis.Position = Axes.AxisPosition.Bottom Then
                axis.Position = Axes.AxisPosition.Left
            ElseIf axis.Position = Axes.AxisPosition.Left Then
                axis.Position = Axes.AxisPosition.Bottom
            End If
        Next

        For Each s In Plot.Series
            If GetType(OxyPlot.Wpf.DataPointSeries).IsAssignableFrom(s.GetType()) Then
                Swap(DirectCast(s, OxyPlot.Wpf.DataPointSeries))
            ElseIf s.GetType() = GetType(OxyPlot.Wpf.ScatterPointSeries) Then
                Swap(DirectCast(s, OxyPlot.Wpf.ScatterPointSeries))
            ElseIf s.GetType() = GetType(OxyPlot.Wpf.ScatterErrorSeries) Then
                Swap(DirectCast(s, OxyPlot.Wpf.ScatterErrorSeries))
            ElseIf s.GetType() = GetType(OxyPlot.Wpf.ColumnSeries) Then
                ' Could convert to a bar series here.
            ElseIf s.GetType() = GetType(OxyPlot.Wpf.BarSeries) Then
                ' Could convert to a column series here.
            ElseIf s.GetType() = GetType(OxyPlot.Wpf.BoxPlotSeries) Then
                For Each axis In Plot.Axes
                    If axis.GetType() = GetType(OxyPlot.Wpf.CategoryAxis) Then
                        If axis.Position = Axes.AxisPosition.Bottom Then
                            DirectCast(s, OxyPlot.Wpf.BoxPlotSeries).IsVertical = True
                        Else
                            DirectCast(s, OxyPlot.Wpf.BoxPlotSeries).IsVertical = False
                        End If
                    End If
                Next
            ElseIf s.GetType() = GetType(OxyPlot.Wpf.HeatMapSeries) Then
                'Dim hms = DirectCast(s, OxyPlot.Wpf.HeatMapSeries)
                'If hms Is Nothing OrElse hms.Data Is Nothing Then Exit Sub
                ''Dim pnts = hms.Data.ToArray()
                ''dps.Points.Clear()
                'Dim x As Double
                'For i As Int32 = 0 To hms.Data.GetLength(0) - 1
                '    For j As Int32 = 0 To hms.Data.GetLength(1) - 1
                '        x = hms.Data(i, j)
                '        hms.Data(i, j) = hms.Data(j, i)
                '        hms.Data(j, i) = x
                '    Next
                'Next
            End If
        Next

        Plot.InvalidatePlot(True)

    End Sub

    'Private Sub Swap(hSeries As OxyPlot.Wpf.HistogramSeries)
    '    If hSeries Is Nothing Then Exit Sub
    '    If hSeries.ItemsSource Is Nothing Then
    '        SwapDataPoints(DirectCast(hSeries.InternalSeries, OxyPlot.Series.HistogramSeries))
    '    Else
    '        'Dim dfx = hSeries.
    '        'hSeries.DataFieldX = hSeries.DataFieldY
    '        'hSeries.DataFieldY = dfx
    '    End If
    'End Sub
    'Private Sub SwapDataPoints(dps As OxyPlot.Series.HistogramSeries)
    '    If dps Is Nothing OrElse dps.Items Is Nothing OrElse dps.Items.Count = 0 Then Exit Sub
    '    Dim pnts = dps.Items.ToArray()
    '    dps.Items.Clear()
    '    For Each p In pnts
    '        dps.Items.Add(New Series.HistogramItem(p.RangeStart, p.RangeEnd, p.Area))
    '    Next
    'End Sub
    Private Sub Swap(sps As OxyPlot.Wpf.ScatterSeries(Of Series.ScatterPoint))
        If sps Is Nothing Then Exit Sub
        If sps.ItemsSource Is Nothing Then
            SwapDataPoints(DirectCast(sps.InternalSeries, OxyPlot.Series.ScatterSeries))
        Else
            Dim dfx = sps.DataFieldX
            sps.DataFieldX = sps.DataFieldY
            sps.DataFieldY = dfx
        End If
    End Sub
    Private Sub Swap(sps As OxyPlot.Wpf.ScatterErrorSeries)
        If sps Is Nothing Then Exit Sub
        If sps.ItemsSource Is Nothing Then
            SwapDataPoints(DirectCast(sps.InternalSeries, OxyPlot.Series.ScatterErrorSeries))
        Else
            Dim dfx = sps.DataFieldX
            sps.DataFieldX = sps.DataFieldY
            sps.DataFieldY = dfx

            dfx = sps.DataFieldLowerErrorX
            sps.DataFieldLowerErrorX = sps.DataFieldLowerErrorY
            sps.DataFieldLowerErrorY = dfx

            dfx = sps.DataFieldUpperErrorX
            sps.DataFieldUpperErrorX = sps.DataFieldUpperErrorY
            sps.DataFieldUpperErrorY = dfx
        End If
    End Sub
    Private Sub SwapDataPoints(dps As OxyPlot.Series.ScatterErrorSeries)
        If dps Is Nothing OrElse dps.Points Is Nothing OrElse dps.Points.Count = 0 Then Exit Sub
        Dim pnts = dps.Points.ToArray()
        dps.Points.Clear()
        For Each p In pnts
            dps.Points.Add(New Series.ScatterErrorPoint(p.Y, p.X, p.LowerErrorY, p.UpperErrorY, p.LowerErrorX, p.UpperErrorX, p.Size, p.Value, p.Tag))
        Next
    End Sub
    Private Sub SwapDataPoints(dps As OxyPlot.Series.ScatterSeries)
        If dps Is Nothing OrElse dps.Points Is Nothing OrElse dps.Points.Count = 0 Then Exit Sub
        Dim pnts = dps.Points.ToArray()
        dps.Points.Clear()
        For Each p In pnts
            dps.Points.Add(New Series.ScatterPoint(p.Y, p.X, p.Size, p.Value, p.Tag))
        Next
    End Sub
    Private Sub Swap(dps As OxyPlot.Wpf.DataPointSeries)
        If dps Is Nothing Then Exit Sub
        If dps.ItemsSource Is Nothing Then
            SwapDataPoints(DirectCast(dps.InternalSeries, OxyPlot.Series.DataPointSeries))
        Else
            Dim df = DirectCast(dps, OxyPlot.Wpf.DataPointSeries)
            If df.DataFieldX Is Nothing AndAlso df.DataFieldY Is Nothing Then
                df.DataFieldX = "Y"
                df.DataFieldY = "X"
            Else
                Dim dfx = df.DataFieldX
                df.DataFieldX = df.DataFieldY
                df.DataFieldY = dfx
            End If
            '
            If dps.GetType() = GetType(OxyPlot.Wpf.AreaSeries) Then
                Dim areaSeries = DirectCast(dps, OxyPlot.Wpf.AreaSeries)
                Dim dfx2 = areaSeries.DataFieldX2
                areaSeries.DataFieldX2 = areaSeries.DataFieldY2
                areaSeries.DataFieldY2 = dfx2
            End If
        End If
    End Sub
    Private Sub SwapDataPoints(dps As OxyPlot.Series.DataPointSeries)
        If dps Is Nothing OrElse dps.Points Is Nothing OrElse dps.Points.Count = 0 Then Exit Sub
        Dim pnts = dps.Points.ToArray()
        dps.Points.Clear()
        For Each p In pnts
            dps.Points.Add(New DataPoint(p.Y, p.X))
        Next
    End Sub

#End Region

End Class
