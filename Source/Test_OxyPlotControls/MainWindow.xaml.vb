Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports MS.Internal
Imports System.Xml
Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Series
Imports OxyPlot.Wpf
Imports Numerics.Data
Imports System.IO

Class MainWindow
    ''' <summary>
    ''' Dependency property for the existing names property.
    ''' </summary>
    Public Shared TestAxisNameBindingProperty As DependencyProperty = DependencyProperty.Register(NameOf(TestAxisNameBinding), GetType(String), GetType(MainWindow), New FrameworkPropertyMetadata("Test Y"))

    Public Property TestAxisNameBinding As String
        Get
            Return CType(GetValue(TestAxisNameBindingProperty), String)
        End Get
        Set(value As String)
            SetValue(TestAxisNameBindingProperty, value)
        End Set
    End Property
    Public ReadOnly Property Points1 As New ObservableCollection(Of DataPoint)({New DataPoint(0, 0), New DataPoint(1, 2), New DataPoint(2, 3), New DataPoint(4, 5)})
    Public ReadOnly Property Points2 As New ObservableCollection(Of DataPoint)({New DataPoint(0, 0), New DataPoint(1, 2), New DataPoint(2, 3), New DataPoint(4, 5)})
    Public ReadOnly Property Points3 As New ObservableCollection(Of DataPoint)({New DataPoint(0, 0), New DataPoint(1, 2), New DataPoint(2, 3), New DataPoint(4, 5)})
    Public ReadOnly Property Points4 As New ObservableCollection(Of DataPoint)({New DataPoint(0, 0), New DataPoint(1, 2), New DataPoint(2, 3), New DataPoint(4, 5)})
    Public ReadOnly Property AreaPoints As New ObservableCollection(Of AreaPoint)

    Public ReadOnly Property ScatterPoints As New ObservableCollection(Of ScatterPoint)


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Points1.Clear()
        'Points3.Clear()
        'For Each p As DataPoint In CreateNormalDist(-5, 5, 0, 1, 200)
        '    Points1.Add(p)
        '    Points3.Add(p)
        'Next
        'Points2.Clear()
        'Points4.Clear()
        'For Each p As DataPoint In CreateNormalDist(-2, 2, 0, 0.25, 100)
        '    Points2.Add(p)
        '    Points4.Add(p)
        'Next

        'Trigger line series in combobox (unbound)
        Combobox1.SelectedIndex = 0
        'LineSeries_Create(False)


        '.ico and .cur file format (https://en.wikipedia.org/wiki/ICO_(file_format))
        'Using fs As New IO.FileStream("C:\Temp\TreeVue Icons\AddPointIcon.cur", IO.FileMode.Open)

        '    Using bw As New IO.BinaryWriter(fs)
        '        bw.BaseStream.Position = 2
        '        bw.Write(CShort(2))
        '        bw.BaseStream.Position = 10
        '        bw.Write(CShort(7))
        '        bw.Write(CShort(7))
        '    End Using



        '    Using sr As New IO.BinaryReader(fs)
        '        'up front
        '        Debug.Print("Reserved, should be zero: " & sr.ReadInt16())
        '        Debug.Print("Should be 1 for .ico and 2 for .cur: " & sr.ReadInt16())
        '        Dim nImages As Short = sr.ReadInt16()
        '        Debug.Print("# images in file: " & nImages)
        '        '
        '        'image entries
        '        For i As Int32 = 1 To nImages
        '            Debug.Print("width: " & sr.ReadByte())
        '            Debug.Print("height: " & sr.ReadByte())
        '            Debug.Print("# colors in color palette (0) if no palette: " & sr.ReadByte())
        '            Debug.Print("Reserved, should be zero: " & sr.ReadByte())
        '            Debug.Print("if .cur then hotspot from left (in pixels): " & sr.ReadInt16()) 'if .ico then specifies color planes. should be 0 or 1
        '            Debug.Print("if .cur then hotspot from top (in pixels): " & sr.ReadInt16()) 'if .ico then specifies bits per pixel.
        '            Debug.Print("size of image data in bytes: " & sr.ReadInt32())
        '            Debug.Print("specifies offset to image data from beginning of file: " & sr.ReadInt32())
        '        Next
        '    End Using
        'End Using


    End Sub
    Private Function CreateNormalDist(x0 As Double, x1 As Double, mean As Double, variance As Double, ByVal Optional n As Integer = 1001) As List(Of DataPoint)

        Dim result As New List(Of DataPoint)
        '
        For i As Integer = 0 To n - 1
            Dim x As Double = x0 + ((x1 - x0) * i / (n - 1))
            Dim f As Double = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance)
            result.Add(New DataPoint(x, f))
        Next

        Return result
    End Function

    Private Sub MainWindow_ContentRendered(sender As Object, e As EventArgs) Handles Me.ContentRendered
        'Dim t = New FontWeightConverter()
        'Dim bt = t.ConvertFromString(TestPlot.TitleFontWeight.ToString())


        'Dim b As New LinearGradientBrush(Colors.Red, Colors.Blue, 0)
        'Dim x = OxyplotControls.SerializeToXelement(b)
        'Dim y = OxyplotControls.DeserializeFromXElement(x)

        'Dim t As New OxyPlotPropertiesDialog(Nothing)
        't.Show()


        'TestPlot.LegendFontSize = 32
        'LineSeries1.FontSize = 8

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''Heavy data example
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''Dim c As New Canvas
        'Dim scb As New SolidColorBrush(Color.FromArgb(10, 0, 0, 0))
        'scb.Freeze()
        'Dim seriesPen As Pen = New Pen(scb, 1.0)
        'seriesPen.Freeze()

        'Dim sw As New Stopwatch
        'sw.Start()

        'Dim randy As New Random()
        'For i As Int32 = 0 To 200
        '    'Dim ln As New Wpf.LineSeries() With {.Color = Color.FromArgb(10, 0, 0, 0)}
        '    Dim pl As New Polyline()
        '    TestCanvas.Children.Add(pl)
        '    For Each p As DataPoint In CreateNormalDist(-5, 5, 0, randy.NextDouble)
        '        'CType(ln.InternalSeries, OxyPlot.Series.LineSeries).Points.Add(p)
        '        pl.Stroke = scb
        '        pl.StrokeThickness = 1
        '        pl.Points.Add(New Point(p.X, p.Y))
        '    Next
        '    'ln.Decimator = AddressOf Decimator.Decimate
        '    'TestPlot.Series.Add(ln)
        'Next


        ''TestPlot.InvalidatePlot(True)

        'sw.Stop()
        'Debug.Print("Oxyplot 200 lineseries 1000 points each: " & sw.Elapsed.TotalMilliseconds)

        '''''''''''''''''''''''''''''''''''''''''''''''
        ''test draw visual
        'Dim dv As DrawingVisual = New DrawingVisual()
        'Dim dc As DrawingContext = dv.RenderOpen()


        'sw = New Stopwatch
        'sw.Start()

        ''Dim xMin As Double = 
        ''Dim xMax As Double =
        ''Dim yMin As Double =
        ''Dim yMax as Double =

        'For i As Int32 = 0 To 200
        '    Dim g As StreamGeometry = New StreamGeometry()
        '    Dim sgc As StreamGeometryContext = g.Open()

        '    For Each p As DataPoint In CreateNormalDist(-5, 5, 0, randy.NextDouble)
        '        Dim firstPoint As Boolean = True
        '        'If p.X < xMin OrElse p.X > xMax Then Continue For
        '        'If p.Y < yMin OrElse p.Y > yMax Then Continue For
        '        'Dim x As Double = basePoint.X + (pointVm.XValue - xMin) * xSizePerValue
        '        'Dim y As Double = basePoint.Y - (pointVm.Value - yMin) * ySizePerValue
        '        'Dim coord As Point = New Point(x, y)

        '        If firstPoint Then
        '            firstPoint = False
        '            sgc.BeginFigure(New Point(p.X, p.Y), False, False)
        '        Else
        '            sgc.LineTo(New Point(p.X, p.Y), True, False)
        '        End If
        '    Next

        '    sgc.Close()
        '    dc.DrawGeometry(Nothing, seriesPen, g)
        'Next

        'dc.Close()

        'sw.Stop()
        'Debug.Print("drawingvisual test 200 lineseries 1000 points each: " & sw.Elapsed.TotalMilliseconds)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''Heat Map Example
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Dim data = New Double(1, 2) {}
        'data(0, 0) = 0
        'data(0, 1) = 0.2
        'data(0, 2) = 0.4
        'data(1, 0) = 0.1
        'data(1, 1) = 0.3
        'data(1, 2) = 0.2

        'With TestPlot
        '    .Title = "HeatMapSeries"
        '    .Subtitle = Title
        'End With

        ''
        ''update the series
        'HeatSeries.Data = data

        '
        'TestPlot.InvalidatePlot(True)

    End Sub

    Private Sub PlotPropertiesUpdated(targetPlot As Plot)
        Console.WriteLine("Plot Properties")
    End Sub

    Private Sub OxyplotToolBar_PropertiesCalled(targetPlot As Wpf.Plot, openProperties As Boolean, propertyExpander As OxyplotControls.OxyplotPropertiesControl.PropertyEXP, selectedObject As Object)
        PropertiesControl.ExpandProperty(propertyExpander, selectedObject)
    End Sub

    'Private Sub OxyplotToolbar_SelectedPropertyChanged(Prop As OxyplotControls.OxyplotPropertiesControl.PropertyEXP, Optional SelectedObject As Object = Nothing)
    '    PropertiesControl.ExpandProperty(Prop, SelectedObject)
    'End Sub

    Private Sub SaveSettingsButton_Click(sender As Object, e As RoutedEventArgs)
        Dim saveFile As String = GenericControls.FileSaveDialog("Plot Settings(*.xml) |*.xml", True)
        If saveFile = "" Then Exit Sub
        'save map data
        Try
            If IO.File.Exists(saveFile) Then IO.File.Delete(saveFile)
        Catch ex As Exception
            MsgBox("Error attempting to delete existing file '" & saveFile & "'." & vbNewLine & vbNewLine & ex.Message)
            Exit Sub
        End Try
        '
        Using writer As Xml.XmlWriter = Xml.XmlWriter.Create(saveFile, New Xml.XmlWriterSettings With {.Indent = True})
            OxyplotControls.OxyplotSettingsSerializer.ToXelement(TestPlot).WriteTo(writer)
        End Using

    End Sub

    Private Sub LoadSettingsButton_Click(sender As Object, e As RoutedEventArgs)
        Dim fileToOpen As String = GenericControls.FileOpenDialog("Plot Settings(*.xml) |*.xml")
        If fileToOpen = "" Then Exit Sub
        If IO.Path.GetExtension(fileToOpen) <> ".xml" Then Exit Sub

        'open map data
        If IO.File.Exists(fileToOpen) Then
            'Try
            Dim document As New Xml.XmlDocument
            document.Load(fileToOpen)

            'For testing load - Clear the axes, annotations, and series... currently, there won't be any data, but the series will be there with the loaded props
            TestPlot.Annotations.Clear()
            TestPlot.Series.Clear()
            TestPlot.Axes.Clear()

            OxyplotControls.OxyplotSettingsSerializer.FromXelement(TestPlot, XElement.Parse(document.GetElementsByTagName(OxyplotControls.OxyplotSettingsSerializer.OxyplotPropertiesTag)(0).OuterXml))
            'Catch ex As Exception
            '    MsgBox("Error occured while trying to load the map layers. Error message: " & vbNewLine & ex.Message)
            'End Try
        End If
    End Sub

    Private Sub TestButton_Click(sender As Object, e As RoutedEventArgs)
        For Each axis In TestPlot.Axes
            If axis.Position = AxisPosition.Left AndAlso axis.GetType = GetType(Wpf.LogarithmicAxis) Then
                Dim lAxis = DirectCast(axis, Wpf.LogarithmicAxis)
                Dim internalAxis = DirectCast(lAxis.InternalAxis, OxyPlot.Axes.LogarithmicAxis)
                internalAxis.Transform(0.5)
                TestPlot.InvalidatePlot()
                Exit For
            End If
        Next
        'new test
        'TestAxisNameBinding = "New Test Y"
        'TestPlot.Axes(1).Minimum = 0.3
        'old test (delete)
        'For Each axis In TestPlot.Axes
        '    If axis.Position = Axes.AxisPosition.Left Then
        '        Select Case axis.GetType
        '            Case GetType(Wpf.LinearAxis)
        '                Dim newYAxis As Wpf.LogarithmicAxis = OxyplotControls.AxisControl.ConvertAxisToLogarithmicAxis(axis)
        '                TestPlot.Axes.Remove(axis)
        '                TestPlot.Axes.Add(newYAxis)
        '                Exit For
        '            Case GetType(Wpf.LogarithmicAxis)
        '                Dim newYAxis As Wpf.LinearAxis = OxyplotControls.AxisControl.ConvertAxisToLinearAxis(axis)
        '                TestPlot.Axes.Remove(axis)
        '                TestPlot.Axes.Add(newYAxis)
        '                Exit For
        '        End Select
        '    End If
        'Next


        'TestPlot.InvalidatePlot(False)
    End Sub

    Private Sub PropertiesControl_ClosePropertiesCalled(propertiesControl As OxyplotControls.OxyplotPropertiesControl)
        MsgBox("I am not going to close. Sorry not sorry.")
    End Sub

    'Private Sub Toolbar_ClosePropertiesCalled()
    '    PropertiesControl_ClosePropertiesCalled(PropertiesControl)
    'End Sub


    Private Sub ComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CType(Combobox1.SelectedItem, ComboBoxItem).Content Is Nothing Then
            Return
        End If

        Select Case CType(Combobox1.SelectedItem, ComboBoxItem).Content.ToString
            Case "Line Series"
                LineSeries_Create(False)
            Case "Line Series (Bound)"
                LineSeries_Create(True)
            Case "Scatter Series"
                ScatterSeries_Create(False)
            Case "Scatter Series (Bound)"
                ScatterSeries_Create(True)
            Case "Histogram Series"
                HistogramSeries_Create(False)
            Case "Histogram Series (Bound)"
                HistogramSeries_Create(True)
            Case "Column Series"
                ColumnSeries_Create(False)
            Case "Column Series (Bound)"
                ColumnSeries_Create(True)
            Case "Bar Series"
                BarSeries_Create(False)
            Case "Bar Series (Bound)"
                BarSeries_Create(True)
            Case "Area Series"
                AreaSeries_Create(False)
            Case "Area Series (Bound)"
                AreaSeries_Create(True)
            Case "Heat Map Series"
                HeatMapSeries_Create(False)
            Case "Heat Map Series (Bound)"
                HeatMapSeries_Create(True)
            Case "Scatter Error Series"
                ScatterErrorSeries_Create(False)
            Case "Scatter Error Series (Bound)"
                ScatterErrorSeries_Create(True)
            Case "Box Plot Series"
                BoxPlotSeries_Create(False)
            Case "Box Plot Series (Bound)"
                BoxPlotSeries_Create(True)
            Case "Date Time Series"
                DateTimeSeries_Create(False)
            Case Else

        End Select

    End Sub

    'This is used for testing binding of series
    Partial Class DummyMultiPurposePoint
        Public Property Xval As Double
        Public Property Yval As Double
        Public Property X2val As Double
        Public Property Y2val As Double
        Public Property SizeVal As Double
        Public Property ColorVal As Double
        Public Property XLowerError As Double
        Public Property XUpperError As Double
        Public Property YLowerError As Double
        Public Property YUpperError As Double
        Public Property LabelVal As String
        Public Property Position As Double
        Public Property LowerWhisker As Double
        Public Property BoxMinimum As Double
        Public Property Median As Double
        Public Property BoxMaximum As Double
        Public Property UpperWhisker As Double
        Public Property Color As OxyPlot.OxyColor

    End Class

    Private Sub LineSeries_Create(BoundBool As Boolean)
        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()
        TestPlot.ActualModel.Series.Clear()
        TestPlot.ActualModel.Axes.Clear()

        TestPlot.Title = "Line Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}

        Dim xAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}


        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Dim lineSeries1 As New OxyPlot.Wpf.LineSeries()
        lineSeries1.Title = "Test Line Series 1"

        If BoundBool = True Then
            Dim lst As New List(Of DummyMultiPurposePoint)

            lineSeries1.DataFieldX = "Xval"
            lineSeries1.DataFieldY = "Yval"

            For Each p As DataPoint In CreateNormalDist(-10, 10, 0, 2)
                Dim dp As New DummyMultiPurposePoint()
                dp.Xval = p.X
                dp.Yval = p.Y
                lst.Add(dp)
            Next

            lineSeries1.ItemsSource = lst
        Else
            For Each p As DataPoint In CreateNormalDist(-5, 5, 0, 1)
                CType(lineSeries1.InternalSeries, OxyPlot.Series.LineSeries).Points.Add(p)
            Next
        End If

        Dim lineSeries2 As New OxyPlot.Wpf.LineSeries()
        lineSeries2.Title = "Test Line Series 2"

        If BoundBool = True Then
            Dim lst As New List(Of DummyMultiPurposePoint)

            lineSeries2.DataFieldX = "Xval"
            lineSeries2.DataFieldY = "Yval"

            For Each p As DataPoint In CreateNormalDist(-4, 4, 0, 0.5)
                Dim dp As New DummyMultiPurposePoint()
                dp.Xval = p.X
                dp.Yval = p.Y
                lst.Add(dp)
            Next

            lineSeries2.ItemsSource = lst
        Else
            For Each p As DataPoint In CreateNormalDist(-2, 2, 0, 0.25)
                CType(lineSeries2.InternalSeries, OxyPlot.Series.LineSeries).Points.Add(p)
            Next
        End If


        TestPlot.Series.Add(lineSeries1)
        TestPlot.Series.Add(lineSeries2)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)
    End Sub

    Private Sub ScatterSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Scatter Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}

        Dim xAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Dim scatterSeries = New OxyPlot.Wpf.ScatterPointSeries() With {
    .MarkerType = MarkerType.Circle,
    .Title = "Scatter Series"
}
        Dim r = New Random(314)

        Dim lst As New List(Of DummyMultiPurposePoint)

        If BoundBool = True Then
            scatterSeries.DataFieldX = "Xval"
            scatterSeries.DataFieldY = "Yval"
            scatterSeries.DataFieldSize = "SizeVal"
            scatterSeries.DataFieldValue = "ColorVal"

            For i As Integer = 0 To 50 - 1
                Dim dsp As New DummyMultiPurposePoint()
                dsp.Xval = r.NextDouble()
                dsp.Yval = r.NextDouble()
                dsp.SizeVal = CDbl(r.[Next](1, 5))
                dsp.ColorVal = CDbl(r.[Next](50, 500))
                lst.Add(dsp)
            Next

            scatterSeries.ItemsSource = lst

        Else
            For i As Integer = 0 To 100 - 1
                Dim x = r.NextDouble()
                Dim y = r.NextDouble()
                Dim size = r.[Next](5, 15)
                Dim colorValue = r.[Next](100, 1000)

                CType(scatterSeries.InternalSeries, ScatterSeries).Points.Add(New ScatterPoint(x, y, size, colorValue))
            Next
        End If

        TestPlot.Series.Add(scatterSeries)

        'Add the color Axis
        Dim lca As New OxyPlot.Wpf.LinearColorAxis With {
            .Position = AxisPosition.Right}
        'CType(lca.InternalAxis, OxyPlot.Axes.LinearColorAxis).Palette = OxyPalettes.Rainbow(100)

        lca.PaletteSize = 200
        lca.LowColor = Colors.Blue
        lca.HighColor = Colors.Red

        Dim first As New GradientStop(Colors.Red, 0)
        Dim second As New GradientStop(Colors.Yellow, 0.5)
        Dim third As New GradientStop(Colors.Green, 1)
        Dim gList As New List(Of GradientStop)
        gList.AddRange({first, second, third})
        Dim gColl As New GradientStopCollection(gList.AsEnumerable)
        lca.GradientStops = gColl
        TestPlot.Axes.Add(lca)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)

    End Sub


    Private Sub ScatterErrorSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Scatter Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}

        Dim xAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Dim scatterErrorSeries = New OxyPlot.Wpf.ScatterErrorSeries With {
    .MarkerType = MarkerType.Circle,
    .Title = "Scatter Error Series"
}
        Dim r = New Random(314)

        Dim lst As New List(Of DummyMultiPurposePoint)

        If BoundBool = True Then
            scatterErrorSeries.DataFieldX = "Xval"
            scatterErrorSeries.DataFieldY = "Yval"
            scatterErrorSeries.DataFieldSize = "SizeVal"
            scatterErrorSeries.DataFieldValue = "ColorVal"
            scatterErrorSeries.DataFieldLowerErrorX = "XLowerError"
            scatterErrorSeries.DataFieldUpperErrorX = "XUpperError"
            scatterErrorSeries.DataFieldLowerErrorY = "YLowerError"
            scatterErrorSeries.DataFieldUpperErrorY = "YUpperError"

            For i As Integer = 0 To 20 - 1
                Dim dsp As New DummyMultiPurposePoint()
                dsp.Xval = r.NextDouble()
                dsp.Yval = r.NextDouble()
                dsp.SizeVal = CDbl(r.[Next](5, 15))
                dsp.ColorVal = CDbl(r.[Next](100, 1000))
                dsp.XLowerError = dsp.Xval - (dsp.Xval / 10)
                dsp.XUpperError = dsp.Xval + (dsp.Xval / 5)
                dsp.YUpperError = dsp.Yval + (dsp.Yval / 5)
                dsp.YLowerError = dsp.Yval - (dsp.Yval / 10)
                lst.Add(dsp)
            Next

            scatterErrorSeries.ItemsSource = lst

        Else
            For i As Integer = 0 To 50 - 1
                Dim x = r.NextDouble()
                Dim y = r.NextDouble()
                Dim size = r.[Next](5, 15)
                Dim colorValue = r.[Next](100, 1000)

                CType(scatterErrorSeries.InternalSeries, OxyPlot.Series.ScatterErrorSeries).Points.Add(New ScatterErrorPoint(x, y, x - (x / 5), x + (x / 10), y - (y / 5), y + (y / 10), size, colorValue))
            Next
        End If


        TestPlot.Series.Add(scatterErrorSeries)

        'Add the color Axis
        Dim lca As New OxyPlot.Wpf.LinearColorAxis With {
            .Position = AxisPosition.Right}
        'CType(lca.InternalAxis, OxyPlot.Axes.LinearColorAxis).Palette = OxyPalettes.Rainbow(100)

        lca.PaletteSize = 200
        lca.LowColor = Colors.Blue
        lca.HighColor = Colors.Red

        Dim first As New GradientStop(Colors.Red, 0)
        Dim second As New GradientStop(Colors.Yellow, 0.5)
        Dim third As New GradientStop(Colors.Green, 1)
        Dim gList As New List(Of GradientStop)
        gList.AddRange({first, second, third})
        Dim gColl As New GradientStopCollection(gList.AsEnumerable)
        lca.GradientStops = gColl
        TestPlot.Axes.Add(lca)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)

    End Sub

    Private Sub HeatMapSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Heat Map"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}

        Dim xAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        'Add the color Axis
        Dim lca As New OxyPlot.Wpf.LinearColorAxis With {
            .Position = AxisPosition.Right}
        'CType(lca.InternalAxis, OxyPlot.Axes.LinearColorAxis).Palette = OxyPalettes.Rainbow(100)

        lca.PaletteSize = 200
        lca.LowColor = Colors.Blue
        lca.HighColor = Colors.Red

        Dim first As New GradientStop(Colors.Red, 0)
        Dim second As New GradientStop(Colors.Yellow, 0.5)
        Dim third As New GradientStop(Colors.Green, 1)
        Dim gList As New List(Of GradientStop)
        gList.AddRange({first, second, third})
        Dim gColl As New GradientStopCollection(gList.AsEnumerable)
        lca.GradientStops = gColl
        TestPlot.Axes.Add(lca)

        Dim wpfHeatMapSeries = New OxyPlot.Wpf.HeatMapSeries With {
            .X0 = 0,
            .X1 = 99,
            .Y0 = 0,
            .Y1 = 99,
            .Interpolate = True,
            .Title = "Heat Map Series"
        }

        If BoundBool = True Then
            'TODO
            'I don't think it's possible to get here, because item source takes in a 1D array... but data is a 2D array.  There are no helper fields.
            'Dim lst As New List(Of Double)

            ''generate 1d normal distribution
            'Dim singleData = New Double(99) {}
            'For x As Integer = 0 To 100 - 1
            '    singleData(x) = Math.Exp((((1 / 2) * -1) * Math.Pow(((CType(x, Double) - 50) / 20), 2)))
            'Next

            ''generate 2d normal distribution
            'Dim data = New Double(99, 99) {}
            'For x As Integer = 0 To 100 - 1
            '    For y As Integer = 0 To 100 - 1
            '        data(y, x) = singleData(x) * singleData((y + 30) Mod 100) * 100
            '    Next
            'Next

            'lst = data

            'wpfHeatMapSeries.ItemsSource = 

        Else
            'generate 1d normal distribution
            Dim singleData = New Double(99) {}
            For x As Integer = 0 To 100 - 1
                singleData(x) = Math.Exp((((1 / 2) * -1) * Math.Pow(((CType(x, Double) - 50) / 20), 2)))
            Next

            'generate 2d normal distribution
            Dim data = New Double(99, 99) {}
            For x As Integer = 0 To 100 - 1
                For y As Integer = 0 To 100 - 1
                    data(y, x) = singleData(x) * singleData((y + 30) Mod 100) * 100
                Next
            Next

            wpfHeatMapSeries.Data = data
        End If

        'Change render method
        CType(wpfHeatMapSeries.InternalSeries, OxyPlot.Series.HeatMapSeries).RenderMethod = HeatMapRenderMethod.Bitmap

        TestPlot.Series.Add(wpfHeatMapSeries)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)

    End Sub

    Private Sub AreaSeries_Click(sender As Object, e As RoutedEventArgs)
        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()
        TestPlot.ActualModel.Series.Clear()
        TestPlot.ActualModel.Axes.Clear()

        TestPlot.Title = "Area Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y"}

        Dim xAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        AreaPoints.Clear()
        For Each p As DataPoint In CreateNormalDist(-5, 5, 0, 1)
            AreaPoints.Add(New AreaPoint(New DataPoint(0, p.Y), p))
        Next

        Dim areaSeries As New OxyPlot.Wpf.AreaSeries()
        areaSeries.Title = "Test Line Series 1"
        areaSeries.DataFieldX = "X1"
        areaSeries.DataFieldY = "Y1"
        areaSeries.DataFieldX2 = "X2"
        areaSeries.DataFieldY2 = "Y2"
        areaSeries.ItemsSource = AreaPoints
        areaSeries.Title = "Area Series"

        TestPlot.Series.Add(areaSeries)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)
    End Sub
    'Private Sub BoxPlotSeries_Click(sender As Object, e As RoutedEventArgs)
    '    '  <oxy:BoxPlotSeries x: Name = "WhiskerSeries" IsVertical="False" Stroke="Black" Fill="PowderBlue" Title="Result Whisker Series"/>
    '    TestPlot.Series.Clear()
    '    TestPlot.Axes.Clear()
    '    TestPlot.ActualModel.Series.Clear()
    '    TestPlot.ActualModel.Axes.Clear()

    '    TestPlot.Title = "BoxPlot Series"

    '    Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
    '        .AxisTitleDistance = 20,
    '        .TitleFontSize = 20,
    '        .Position = AxisPosition.Left,
    '        .MajorGridlineStyle = LineStyle.Solid,
    '        .MinorGridlineStyle = LineStyle.Dash,
    '        .Title = "Test Y"}

    '    Dim xAxis As New OxyPlot.Wpf.CategoryAxis With {
    '        .AxisTitleDistance = 20,
    '        .Position = AxisPosition.Bottom,
    '        .TitleFontSize = 20,
    '        .MajorGridlineStyle = LineStyle.Solid,
    '        .MinorGridlineStyle = LineStyle.Dash,
    '        .Title = "Test X"}

    '    TestPlot.Axes.Add(xAxis)
    '    TestPlot.Axes.Add(yAxis)

    '    Dim boxPlotSeries As New OxyPlot.Wpf.BoxPlotSeries()
    '    boxPlotSeries.Title = "BoxPlot Series"
    '    boxPlotSeries.IsVertical = True

    '    Dim randy As New Random()
    '    Dim data(99) As Double
    '    For i As Int32 = 0 To data.Count - 1
    '        data(i) = randy.Next(0, 500)
    '    Next
    '    Array.Sort(data)

    '    xAxis.Labels.Clear()
    '    xAxis.Labels.Add("Random 0 - 500")
    '    boxPlotSeries.Items.Add(New BoxPlotItem(0, data(0), data(24), data(49), data(74), data(99)) With {.Mean = data.Average})

    '    TestPlot.Series.Add(boxPlotSeries)

    '    TestPlot.ResetAllAxes()
    '    TestPlot.InvalidatePlot(True)

    'End Sub




    Private Sub HistogramSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Histogram Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}

        Dim xAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Dim histogramSeries As New OxyPlot.Wpf.HistogramSeries
        histogramSeries.Title = "Histogram Series"

        Dim r = New Random(314)

        If BoundBool = True Then


            Dim vals As New List(Of HistogramItem)
            vals.Add(New HistogramItem(200, 400, 100))
            vals.Add(New HistogramItem(0, 200, 500))
            vals.Add(New HistogramItem(400, 600, 800))
            vals.Add(New HistogramItem(600, 800, 2000))

            histogramSeries.ItemsSource = vals

        Else
            'Adding items to the WPF series doesn't work. They have to be added to the internal series.
            Dim internalSeries = DirectCast(histogramSeries.InternalSeries, OxyPlot.Series.HistogramSeries)
            internalSeries.Items.Add(New HistogramItem(0, 200, 400))
            internalSeries.Items.Add(New HistogramItem(200, 400, 100))
            internalSeries.Items.Add(New HistogramItem(400, 600, 800))
            internalSeries.Items.Add(New HistogramItem(600, 800, 2000))

        End If

        TestPlot.Series.Add(histogramSeries)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)

    End Sub


    Private Sub BoxPlotSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Box Plot Series"

        Dim lst As New List(Of DummyMultiPurposePoint)

        'postion
        'lower whisker
        'box minimum
        'median
        'box max
        'upper whisker

        For i As Integer = 0 To 3
            lst.Add(New DummyMultiPurposePoint With {
                .Position = i + 10,
                .LowerWhisker = i + 2,
                .BoxMinimum = i + 5,
                .Median = i + 7,
                .BoxMaximum = i + 20,
                .UpperWhisker = i + 30,
                .LabelVal = "Category" & i})
        Next


        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}


        Dim xAxis As New OxyPlot.Wpf.CategoryAxis With {
            .ItemsSource = {"Math", "Science", "English", "History"},
            .IsTickCentered = True,
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        Dim boxPlotSeries As New OxyPlot.Wpf.BoxPlotSeries
        boxPlotSeries.BoxWidth = 0.5
        boxPlotSeries.WhiskerWidth = 0.5
        boxPlotSeries.StrokeThickness = 2
        boxPlotSeries.Title = "Box Plot Series"

        If BoundBool = True Then
            Dim outliers As New List(Of Double)
            outliers.AddRange({2, 45, 55, 60})

            Dim outliers2 As New List(Of Double)
            outliers2.AddRange({5, 52, 70})

            Dim items As New List(Of BoxPlotItem)
            items.Add(New BoxPlotItem(0, 12, 15, 17, 30, 40))
            items.Add(New BoxPlotItem(1, 7, 10, 12, 25, 35) With {
                      .Outliers = outliers2})
            items.Add(New BoxPlotItem(2, 4, 12, 14, 30, 40))
            items.Add(New BoxPlotItem(3, 3, 5, 7, 20, 30) With {
                                    .Outliers = outliers})

            boxPlotSeries.ItemsSource = items

        Else
            Dim outliers As New List(Of Double)
            outliers.AddRange({2, 50})

            Dim outliers2 As New List(Of Double)
            outliers2.AddRange({60})

            boxPlotSeries.Items.Add(New BoxPlotItem(0, 3, 5, 7, 20, 30) With {
                                    .Outliers = outliers})
            boxPlotSeries.Items.Add(New BoxPlotItem(1, 4, 12, 14, 30, 40) With {
                                    .Outliers = outliers2})
            boxPlotSeries.Items.Add(New BoxPlotItem(2, 7, 10, 12, 25, 35))
            boxPlotSeries.Items.Add(New BoxPlotItem(3, 12, 15, 17, 30, 40))

        End If

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)
        TestPlot.Series.Add(boxPlotSeries)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)

    End Sub


    Private Sub ColumnSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Column Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}


        Dim xAxis As New OxyPlot.Wpf.CategoryAxis With {
            .ItemsSource = {"Math", "Science", "English", "History"},
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Dim colSeries As New OxyPlot.Wpf.ColumnSeries
        colSeries.Title = "Column Series"

        Dim r = New Random(314)

        If BoundBool = True Then

            colSeries.ValueField = "Value"
            colSeries.ColorField = "Color"

            'Note: Area is width * value 
            Dim vals As New List(Of ColumnItem)
            vals.Add(New ColumnItem(50, 0) With {
                     .Color = OxyPlot.OxyColors.Red})
            vals.Add(New ColumnItem(10, 1))
            vals.Add(New ColumnItem(30, 2))
            vals.Add(New ColumnItem(20, 3))

            colSeries.ItemsSource = vals

        Else

            colSeries.Items.Add(New ColumnItem(30, 0))
            colSeries.Items.Add(New ColumnItem(80, 1))
            colSeries.Items.Add(New ColumnItem(10, 2) With {
                                .Color = OxyPlot.OxyColors.Red})
            colSeries.Items.Add(New ColumnItem(50, 3))


        End If

        TestPlot.Series.Add(colSeries)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)

    End Sub



    Private Sub BarSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Bar Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Bottom,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}


        Dim xAxis As New OxyPlot.Wpf.CategoryAxis With {
            .ItemsSource = {"Apple cake", "Baumkuchen", "Bundt Cake", "Chocolate cake", "Carrot cake"},
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Left,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Dim barSeries As New OxyPlot.Wpf.BarSeries
        barSeries.Title = "Bar Series"

        Dim rand = New Random()
        Dim cakePopularity As Double() = New Double(4) {}

        For i As Integer = 0 To 5 - 1
            cakePopularity(i) = rand.NextDouble()
        Next

        Dim sum = cakePopularity.Sum()

        If BoundBool = True Then

            barSeries.ValueField = "Xval"
            barSeries.ColorField = "Color"

            Dim lst As New List(Of DummyMultiPurposePoint)

            For i As Integer = 0 To 4
                Dim dp As New DummyMultiPurposePoint
                dp.Xval = cakePopularity(i) / sum * 100
                dp.Color = OxyPlot.OxyColors.Red
                lst.Add(dp)
            Next

            barSeries.ItemsSource = lst
            barSeries.LabelPlacement = LabelPlacement.Inside
            barSeries.LabelFormatString = "{0:.00}%"

        Else
            'TODO - this isn't showing properly

            'Note: Area is width * value 
            barSeries.Items.Add(New BarItem(cakePopularity(0) / sum * 100))
            barSeries.Items.Add(New BarItem(cakePopularity(1) / sum * 100))
            barSeries.Items.Add(New BarItem(cakePopularity(2) / sum * 100))
            barSeries.Items.Add(New BarItem(cakePopularity(3) / sum * 100))
            barSeries.Items.Add(New BarItem(cakePopularity(4) / sum * 100))

        End If

        TestPlot.Series.Add(barSeries)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)

    End Sub

    Private Sub AreaSeries_Create(BoundBool As Boolean)
        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()
        TestPlot.ActualModel.Series.Clear()
        TestPlot.ActualModel.Axes.Clear()

        TestPlot.Title = "Area Series"

        Dim yAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}

        Dim xAxis As New OxyPlot.Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}


        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Dim areaSeries1 As New OxyPlot.Wpf.AreaSeries
        areaSeries1.Title = "Area Series"
        areaSeries1.Fill = Colors.Green

        If BoundBool = True Then
            Dim lst As New List(Of DummyMultiPurposePoint)

            areaSeries1.DataFieldX = "Xval"
            areaSeries1.DataFieldY = "Yval"
            areaSeries1.DataFieldX2 = "X2val"
            areaSeries1.DataFieldY2 = "Y2val"

            For Each p As DataPoint In CreateNormalDist(-10, 10, 0, 2)
                Dim dp As New DummyMultiPurposePoint()
                dp.Xval = p.X
                dp.Yval = p.Y

                'When exporting, have to check if there are x2 and y2 values
                dp.X2val = p.X / 2
                dp.Y2val = p.Y / 2

                lst.Add(dp)
            Next

            areaSeries1.ItemsSource = lst
        Else
            For Each p As DataPoint In CreateNormalDist(-5, 5, 0, 1)

                CType(areaSeries1.InternalSeries, OxyPlot.Series.AreaSeries).Points.Add(p)
            Next
        End If

        TestPlot.Series.Add(areaSeries1)

        TestPlot.ResetAllAxes()
        TestPlot.InvalidatePlot(True)
    End Sub

    Private Sub DateTimeSeries_Create(BoundBool As Boolean)

        TestPlot.Series.Clear()
        TestPlot.Axes.Clear()

        TestPlot.Title = "Date Time Series"

        Dim yAxis As New Wpf.LinearAxis With {
            .AxisTitleDistance = 20,
            .TitleFontSize = 20,
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test Y",
            .Key = "y"}


        Dim xAxis As New Wpf.DateTimeAxis With {
        .AxisTitleDistance = 20,
            .Position = AxisPosition.Bottom,
            .TitleFontSize = 20,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dash,
            .Title = "Test X",
            .Key = "x"}

        TestPlot.Axes.Add(xAxis)
        TestPlot.Axes.Add(yAxis)

        Using resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Oxyplot_Properties_Testing.USGS_01134500.xml")

            'Dim x = New IO.FileStream(resource, FileMode.Open)
            Dim s = New StreamReader(resource)

            Dim t = New TimeSeries(XElement.Parse(s.ReadToEnd()))
            Dim l As New Wpf.LineSeries() With
            {
                .Title = "Time Series Data",
                .Color = Colors.Blue,
                .MarkerFill = Colors.Transparent,
                .StrokeThickness = 1,
                .LineStyle = LineStyle.Solid
            }
            l.ItemsSource = t
            l.DataFieldX = "Index"
            l.DataFieldY = "Value"
            TestPlot.Series.Add(l)
            TestPlot.InvalidatePlot(True)
        End Using

    End Sub

    Public Class AreaPoint
        Public Property X1 As Double
        Public Property X2 As Double
        Public Property Y1 As Double
        Public Property Y2 As Double
        Public Sub New(p1 As DataPoint, p2 As DataPoint)
            X1 = p1.X
            X2 = p2.X
            Y1 = p1.Y
            Y2 = p2.Y
        End Sub
    End Class

End Class
