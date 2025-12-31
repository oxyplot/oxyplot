Imports System.Globalization
Imports OxyPlot
Imports OxyplotControls.OxyplotPropertiesControl

Public Class GenericSeriesControl
    Public Shared ReadOnly SeriesPropertiesTag As String = "Series"

    Public Shared ReadOnly Property MarkerTypeOptions As List(Of MarkerType)
        Get
            Dim types As New List(Of MarkerType)(DirectCast([Enum].GetValues(GetType(MarkerType)), MarkerType()))
            types.Remove(MarkerType.Custom)
            Return types
        End Get
    End Property
    Public Shared ReadOnly Property LineLegendPositionOptions As New List(Of Series.LineLegendPosition)(DirectCast([Enum].GetValues(GetType(Series.LineLegendPosition)), Series.LineLegendPosition()))
    Public Shared ReadOnly Property LineJoinOptions As New List(Of LineJoin)(DirectCast([Enum].GetValues(GetType(LineJoin)), LineJoin()))
    Public Shared ReadOnly Property LineStyleOptions As List(Of DoubleCollection) = GenericControls.LineStyleSelectorControl.LineStyleOptions

    Public Shared SeriesProperty As DependencyProperty = DependencyProperty.Register(NameOf(Series), GetType(Wpf.Series), GetType(GenericSeriesControl), New PropertyMetadata(Nothing, AddressOf InitializeControl))
    Public Property Series As Wpf.Series
        Get
            Return DirectCast(GetValue(SeriesProperty), Wpf.Series)
        End Get
        Set(value As Wpf.Series)
            SetValue(SeriesProperty, value)
        End Set
    End Property

    Private Shared Sub InitializeControl(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(GenericSeriesControl) Then Exit Sub
        Dim thisControl = DirectCast(d, GenericSeriesControl)
        '
        If IsNothing(e.NewValue) Then Exit Sub
        Dim wpfSeries As Wpf.Series = TryCast(e.NewValue, Wpf.Series)
        If IsNothing(wpfSeries) Then Exit Sub
        '
        ''Need to set up the datagrid to display the data supporting the series.
        ''There are a few options, one is to bind to the series data directly. 
        ''Another option would be to use a datatable as an in-between to send information 
        ''from the series to the data grid and vice versa.
        ''A third option is to use the wpf table viewer control, it will be much snappier than the wpf datagrid.
        ''
        ''data table option
        'Dim dt As New System.Data.DataTable
        ''
        ''If a datapoint series then you can use the data directly.
        ''The following needs work to make more efficient but the building blocks are there.
        'Dim dataSeries As DataPointSeries = TryCast(wpfSeries.InternalSeries, DataPointSeries)
        'If Not IsNothing(dataSeries) Then
        '    Dim xAxis As String = "X"
        '    If Not IsNothing(dataSeries.XAxis) AndAlso dataSeries.XAxis.Title <> "" Then xAxis = dataSeries.XAxis.Title
        '    Dim yAxis As String = "Y"
        '    If Not IsNothing(dataSeries.YAxis) AndAlso dataSeries.YAxis.Title <> "" Then yAxis = dataSeries.YAxis.Title
        '    dt.Columns.Add(xAxis, GetType(Double))
        '    dt.Columns.Add(yAxis, GetType(Double))
        '    '
        '    If IsNothing(dataSeries.ItemsSource) Then
        '        'thisControl.SeriesDataGrid.ItemsSource = dataSeries.Points
        '        For Each p In dataSeries.Points
        '            dt.Rows.Add({p.X, p.Y})
        '        Next
        '    Else
        '        If IsNothing(dataSeries.DataFieldX) OrElse IsNothing(dataSeries.DataFieldY) Then
        '            'first need to check if it is a data series of datapoints
        '            thisControl.SeriesDataGrid.ItemsSource = dataSeries.ItemsSource
        '        Else
        '            'set the columns for the itemssource based on the DataFieldX and DataFieldY properties.
        '            'thisControl.SeriesDataGrid.ItemsSource = dataSeries.ItemsSource
        '            Dim pnts = New OxyPlot.ListBuilder(Of DataPoint)
        '            pnts.Add(dataSeries.DataFieldX, Double.NaN)
        '            pnts.Add(dataSeries.DataFieldY, Double.NaN)
        '            'For Each p In pnts.Fill(dataSeries.)
        '            ' thisControl.SeriesDataGrid
        '        End If
        '    End If
        'End If
        ''
        ''There are other series that we may want to include here, such as box and whisker series.



        'thisControl.SeriesDataGrid.Items.Refresh()
    End Sub

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(GenericSeriesControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property

    Public Sub CloseExpanders()
        'LabelingEXP.IsExpanded = False
        DisplayEXP.IsExpanded = False
        MarkersEXP.IsExpanded = False
        ErrorBarSettingsEXP.IsExpanded = False
        BoxAndWhiskerEXP.IsExpanded = False
    End Sub

    Public Sub Expand(expansionZone As OxyplotPropertiesControl.PropertyEXP)
        Select Case expansionZone
            Case PropertyEXP.Series_General
                'LabelingEXP.IsExpanded = True
                DisplayEXP.IsExpanded = True
            Case PropertyEXP.Series_Display
                DisplayEXP.IsExpanded = True
            Case PropertyEXP.Series_Markers
                MarkersEXP.IsExpanded = True
            Case PropertyEXP.Series_BoxAndWhiskers
                BoxAndWhiskerEXP.IsExpanded = True
            Case PropertyEXP.Series_ErrorBarSettings
                ErrorBarSettingsEXP.IsExpanded = True
            Case Else
        End Select
    End Sub

    Public Shared Function SeriesPropertiesToXElement(plot As Wpf.Plot) As XElement
        Dim seriesProperties As New XElement(SeriesPropertiesTag)
        For Each Series As Wpf.Series In plot.Series
            seriesProperties.Add(GenericSeriesControl.SeriesPropertiesToXElement(Series))
        Next
        '
        Return seriesProperties
    End Function

    Public Shared Function SeriesPropertiesToXElement(series As Wpf.Series) As XElement

        Dim seriesElement As New XElement(SeriesPropertiesTag)
        Dim seriesType As Type = series.GetType()
        seriesElement.SetAttributeValue("SeriesType", seriesType.ToString())

        'Serialize general series properties
        Dim generalProperties As New XElement("General")
        generalProperties.SetAttributeValue(NameOf(series.Name), series.Name.ToString())
        generalProperties.SetAttributeValue(NameOf(series.Title), series.Title)
        generalProperties.SetAttributeValue(NameOf(series.IsEnabled), series.IsEnabled.ToString())
        generalProperties.SetAttributeValue(NameOf(series.Visibility), series.Visibility.ToString())
        generalProperties.SetAttributeValue(NameOf(series.RenderInLegend), series.RenderInLegend.ToString())
        '
        Dim bc As New BrushConverter()
        generalProperties.SetAttributeValue(NameOf(series.Background), bc.ConvertToInvariantString(series.Background))
        generalProperties.SetAttributeValue(NameOf(series.Foreground), bc.ConvertToInvariantString(series.Foreground))
        'If Not IsNothing(series.Background) Then
        '    Dim seriesBackgroundElement As New XElement(NameOf(series.Background))
        '    seriesBackgroundElement.Add(SerializeToXelement(series.Background))
        '    generalProperties.Add(seriesBackgroundElement)
        'End If
        '
        'If Not IsNothing(series.Foreground) Then
        '    Dim seriesForegroundElement As New XElement(NameOf(series.Foreground))
        '    seriesForegroundElement.Add(SerializeToXelement(series.Foreground))
        '    generalProperties.Add(seriesForegroundElement)
        'End If
        '
        Dim fwc As New FontWeightConverter()
        Dim tc As New ThicknessConverter()
        Dim ffc As New FontFamilyConverter()
        generalProperties.SetAttributeValue(NameOf(series.Color), series.Color.ToString())
        If series.FontFamily IsNot Nothing Then generalProperties.SetAttributeValue(NameOf(series.FontFamily), ffc.ConvertToInvariantString(series.FontFamily))
        generalProperties.SetAttributeValue(NameOf(series.FontSize), series.FontSize.ToString("G17", CultureInfo.InvariantCulture))
        generalProperties.SetAttributeValue(NameOf(series.FontWeight), fwc.ConvertToInvariantString(series.FontWeight))
        generalProperties.SetAttributeValue(NameOf(series.Padding), tc.ConvertToInvariantString(series.Padding))
        generalProperties.SetAttributeValue(NameOf(series.TrackerFormatString), series.TrackerFormatString)
        generalProperties.SetAttributeValue(NameOf(series.TrackerKey), series.TrackerKey)
        '
        seriesElement.Add(generalProperties)
        '
        'Serialize xy axis series properties
        Dim xyAxisSeries = TryCast(series, Wpf.XYAxisSeries)
        If Not IsNothing(xyAxisSeries) Then
            Dim xyAxisSeriesElement As New XElement(NameOf(Wpf.XYAxisSeries))
            xyAxisSeriesElement.SetAttributeValue(NameOf(xyAxisSeries.XAxisKey), xyAxisSeries.XAxisKey)
            xyAxisSeriesElement.SetAttributeValue(NameOf(xyAxisSeries.YAxisKey), xyAxisSeries.YAxisKey)
            seriesElement.Add(xyAxisSeriesElement)
        End If

        'Serialize data point series properties
        Dim dataPointSeries = TryCast(series, Wpf.DataPointSeries)
        If Not IsNothing(dataPointSeries) Then
            Dim dataPointSeriesElement As New XElement(NameOf(Wpf.DataPointSeries))
            dataPointSeriesElement.SetAttributeValue(NameOf(dataPointSeries.CanTrackerInterpolatePoints), dataPointSeries.CanTrackerInterpolatePoints.ToString())
            dataPointSeriesElement.SetAttributeValue(NameOf(dataPointSeries.DataFieldX), dataPointSeries.DataFieldX)
            dataPointSeriesElement.SetAttributeValue(NameOf(dataPointSeries.DataFieldY), dataPointSeries.DataFieldY)
            'Public Property Mapping As Func(Of Object, DataPoint) Not sure what this is used for
            seriesElement.Add(dataPointSeriesElement)
        End If

        'Serialize bar base series properties (bar, column or histogram)
        Dim barBaseSeries = TryCast(series, Wpf.BarSeriesBase)
        If Not IsNothing(barBaseSeries) Then
            Dim barBaseElement As New XElement(NameOf(Wpf.BarSeriesBase))
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.BaseValue), barBaseSeries.BaseValue.ToString("G17", CultureInfo.InvariantCulture))
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.FillColor), barBaseSeries.FillColor)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.ColorField), barBaseSeries.ColorField)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.ValueField), barBaseSeries.ValueField)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.LabelMargin), barBaseSeries.LabelMargin.ToString("G17", CultureInfo.InvariantCulture))
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.StrokeColor), barBaseSeries.StrokeColor)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.StackGroup), barBaseSeries.StackGroup)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.NegativeFillColor), barBaseSeries.NegativeFillColor)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.LabelPlacement), barBaseSeries.LabelPlacement)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.LabelFormatString), barBaseSeries.LabelFormatString)
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.StrokeThickness), barBaseSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            barBaseElement.SetAttributeValue(NameOf(barBaseSeries.IsStacked), barBaseSeries.IsStacked)
            seriesElement.Add(barBaseElement)
        End If

        'Serialize bar series properties
        Dim barSeries = TryCast(series, Wpf.BarSeries)
        If Not IsNothing(barSeries) Then
            Dim barElement As New XElement(NameOf(Wpf.BarSeries))
            barElement.SetAttributeValue(NameOf(barSeries.BarWidth), barSeries.BarWidth.ToString("G17", CultureInfo.InvariantCulture))
            seriesElement.Add(barElement)
        End If

        'Serialize column series properties
        Dim columnSeries = TryCast(series, Wpf.ColumnSeries)
        If Not IsNothing(columnSeries) Then
            Dim columnElement As New XElement(NameOf(Wpf.ColumnSeries))
            columnElement.SetAttributeValue(NameOf(columnSeries.ColumnWidth), columnSeries.ColumnWidth.ToString("G17", CultureInfo.InvariantCulture))
            seriesElement.Add(columnElement)
        End If

        'Serialize histogram series properties
        Dim histogramSeries = TryCast(series, Wpf.HistogramSeries)
        If Not IsNothing(histogramSeries) Then
            Dim histogramElement As New XElement(NameOf(Wpf.HistogramSeries))
            histogramElement.SetAttributeValue(NameOf(histogramSeries.FillColor), histogramSeries.FillColor)
            histogramElement.SetAttributeValue(NameOf(histogramSeries.NegativeFillColor), histogramSeries.NegativeFillColor)
            histogramElement.SetAttributeValue(NameOf(histogramSeries.LabelFormatString), histogramSeries.LabelFormatString)
            histogramElement.SetAttributeValue(NameOf(histogramSeries.LabelPlacement), histogramSeries.LabelPlacement)
            histogramElement.SetAttributeValue(NameOf(histogramSeries.StrokeColor), histogramSeries.StrokeColor)
            histogramElement.SetAttributeValue(NameOf(histogramSeries.StrokeThickness), histogramSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            seriesElement.Add(histogramElement)
        End If

        'Serialize linel series properties
        Dim lineSeries = TryCast(series, Wpf.LineSeries)
        If Not IsNothing(lineSeries) Then
            Dim lineSeriesElement As New XElement(NameOf(Wpf.LineSeries))
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.LineJoin), lineSeries.LineJoin)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.LineLegendPosition), lineSeries.LineLegendPosition)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.LineStyle), lineSeries.LineStyle)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MarkerFill), lineSeries.MarkerFill)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MarkerOutline), lineSeries.MarkerOutline)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MarkerResolution), lineSeries.MarkerResolution.ToString("G17", CultureInfo.InvariantCulture))
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MarkerSize), lineSeries.MarkerSize.ToString("G17", CultureInfo.InvariantCulture))
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MarkerStroke), lineSeries.MarkerStroke)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MarkerStrokeThickness), lineSeries.MarkerStrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MarkerType), lineSeries.MarkerType)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.MinimumSegmentLength), lineSeries.MinimumSegmentLength.ToString("G17", CultureInfo.InvariantCulture))
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.StrokeThickness), lineSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.LabelFormatString), lineSeries.LabelFormatString)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.LabelMargin), lineSeries.LabelMargin.ToString("G17", CultureInfo.InvariantCulture))
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.BrokenLineColor), lineSeries.BrokenLineColor)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.BrokenLineStyle), lineSeries.BrokenLineStyle)
            lineSeriesElement.SetAttributeValue(NameOf(lineSeries.BrokenLineThickness), lineSeries.BrokenLineThickness.ToString("G17", CultureInfo.InvariantCulture))
            seriesElement.Add(lineSeriesElement)
        End If

        'Serialize area series properties
        Dim areaSeries = TryCast(series, Wpf.AreaSeries)
        If Not IsNothing(areaSeries) Then
            Dim areaSeriesElement As New XElement(NameOf(Wpf.AreaSeries))
            areaSeriesElement.SetAttributeValue(NameOf(areaSeries.Color2), areaSeries.Color2)
            areaSeriesElement.SetAttributeValue(NameOf(areaSeries.Fill), areaSeries.Fill)
            areaSeriesElement.SetAttributeValue(NameOf(areaSeries.DataFieldX2), areaSeries.DataFieldX2)
            areaSeriesElement.SetAttributeValue(NameOf(areaSeries.DataFieldY2), areaSeries.DataFieldY2)
            areaSeriesElement.SetAttributeValue(NameOf(areaSeries.Reverse2), areaSeries.Reverse2)
            seriesElement.Add(areaSeriesElement)
        End If

        'Serialize boxplot series properties
        Dim boxPlotSeries = TryCast(series, Wpf.BoxPlotSeries)
        If Not IsNothing(boxPlotSeries) Then
            Dim boxPlotElement As New XElement(NameOf(Wpf.BoxPlotSeries))
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.StrokeThickness), boxPlotSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.Stroke), boxPlotSeries.Stroke)
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.LineStyle), boxPlotSeries.LineStyle)
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.IsVertical), boxPlotSeries.IsVertical)
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.Fill), boxPlotSeries.Fill)
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.OutlierType), boxPlotSeries.OutlierType)
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.WhiskerWidth), boxPlotSeries.WhiskerWidth.ToString("G17", CultureInfo.InvariantCulture))
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.ShowMedianAsDot), boxPlotSeries.ShowMedianAsDot)
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.MedianPointSize), boxPlotSeries.MedianPointSize.ToString("G17", CultureInfo.InvariantCulture))
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.OutlierSize), boxPlotSeries.OutlierSize.ToString("G17", CultureInfo.InvariantCulture))
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.BoxWidth), boxPlotSeries.BoxWidth.ToString("G17", CultureInfo.InvariantCulture))
            boxPlotElement.SetAttributeValue(NameOf(boxPlotSeries.ShowBox), boxPlotSeries.ShowBox)
            seriesElement.Add(boxPlotElement)
        End If

        'Serialize scatter point series properties
        Dim scatterPointSeries = TryCast(series, Wpf.ScatterPointSeries)
        If Not IsNothing(scatterPointSeries) Then
            Dim scatterPointElement As New XElement(NameOf(Wpf.ScatterPointSeries))
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.DataFieldTag), scatterPointSeries.DataFieldTag)
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.DataFieldValue), scatterPointSeries.DataFieldValue)
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.ColorAxisKey), scatterPointSeries.ColorAxisKey)
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.BinSize), scatterPointSeries.BinSize.ToString("G17", CultureInfo.InvariantCulture))
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.MarkerFill), scatterPointSeries.MarkerFill)
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.MarkerOutline), scatterPointSeries.MarkerOutline)
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.MarkerSize), scatterPointSeries.MarkerSize.ToString("G17", CultureInfo.InvariantCulture))
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.MarkerStroke), scatterPointSeries.MarkerStroke)
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.MarkerStrokeThickness), scatterPointSeries.MarkerStrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            scatterPointElement.SetAttributeValue(NameOf(scatterPointSeries.MarkerType), scatterPointSeries.MarkerType)
            seriesElement.Add(scatterPointElement)
        End If

        'Serialize error bar series properties
        Dim errorBarsSeries = TryCast(series, Wpf.ScatterErrorSeries)
        If Not IsNothing(errorBarsSeries) Then
            Dim element As New XElement(NameOf(Wpf.ScatterErrorSeries))
            element.SetAttributeValue(NameOf(errorBarsSeries.DataFieldTag), errorBarsSeries.DataFieldTag)
            element.SetAttributeValue(NameOf(errorBarsSeries.DataFieldValue), errorBarsSeries.DataFieldValue)
            element.SetAttributeValue(NameOf(errorBarsSeries.ColorAxisKey), errorBarsSeries.ColorAxisKey)
            element.SetAttributeValue(NameOf(errorBarsSeries.BinSize), errorBarsSeries.BinSize.ToString("G17", CultureInfo.InvariantCulture))
            element.SetAttributeValue(NameOf(errorBarsSeries.MarkerFill), errorBarsSeries.MarkerFill)
            element.SetAttributeValue(NameOf(errorBarsSeries.MarkerOutline), errorBarsSeries.MarkerOutline)
            element.SetAttributeValue(NameOf(errorBarsSeries.MarkerSize), errorBarsSeries.MarkerSize.ToString("G17", CultureInfo.InvariantCulture))
            element.SetAttributeValue(NameOf(errorBarsSeries.MarkerStroke), errorBarsSeries.MarkerStroke)
            element.SetAttributeValue(NameOf(errorBarsSeries.MarkerStrokeThickness), errorBarsSeries.MarkerStrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            element.SetAttributeValue(NameOf(errorBarsSeries.MarkerType), errorBarsSeries.MarkerType)
            element.SetAttributeValue(NameOf(errorBarsSeries.ErrorBarStopWidth), errorBarsSeries.ErrorBarStopWidth.ToString("G17", CultureInfo.InvariantCulture))
            element.SetAttributeValue(NameOf(errorBarsSeries.MinimumErrorSize), errorBarsSeries.MinimumErrorSize.ToString("G17", CultureInfo.InvariantCulture))
            element.SetAttributeValue(NameOf(errorBarsSeries.ErrorBarStrokeThickness), errorBarsSeries.ErrorBarStrokeThickness.ToString("G17", CultureInfo.InvariantCulture))
            element.SetAttributeValue(NameOf(errorBarsSeries.ErrorBarColor), errorBarsSeries.ErrorBarColor)
            element.SetAttributeValue(NameOf(errorBarsSeries.DataFieldLowerErrorX), errorBarsSeries.DataFieldLowerErrorX)
            element.SetAttributeValue(NameOf(errorBarsSeries.DataFieldUpperErrorX), errorBarsSeries.DataFieldUpperErrorX)
            element.SetAttributeValue(NameOf(errorBarsSeries.DataFieldLowerErrorY), errorBarsSeries.DataFieldLowerErrorY)
            element.SetAttributeValue(NameOf(errorBarsSeries.DataFieldUpperErrorY), errorBarsSeries.DataFieldUpperErrorY)
            seriesElement.Add(element)
        End If

        'Serialize heat map series properties
        Dim heatMapSeries = TryCast(series, Wpf.HeatMapSeries)
        If Not IsNothing(heatMapSeries) Then
            Dim heatMapElement As New XElement(NameOf(Wpf.HeatMapSeries))
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.ColorAxisKey), heatMapSeries.ColorAxisKey)
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.Y0), heatMapSeries.Y0.ToString("G17", CultureInfo.InvariantCulture))
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.Y1), heatMapSeries.Y1.ToString("G17", CultureInfo.InvariantCulture))
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.X0), heatMapSeries.X0.ToString("G17", CultureInfo.InvariantCulture))
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.X1), heatMapSeries.X1.ToString("G17", CultureInfo.InvariantCulture))
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.HighColor), heatMapSeries.HighColor)
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.CoordinateDefinition), heatMapSeries.CoordinateDefinition)
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.Interpolate), heatMapSeries.Interpolate)
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.LabelFontSize), heatMapSeries.LabelFontSize.ToString("G17", CultureInfo.InvariantCulture))
            heatMapElement.SetAttributeValue(NameOf(heatMapSeries.LowColor), heatMapSeries.LowColor)
            seriesElement.Add(heatMapElement)
        End If

        Return seriesElement

    End Function
    '

    ''' <summary>
    ''' Load general plot property settings from XElement.
    ''' </summary>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Sub XElementToSeriesProperties(plot As Wpf.Plot, element As XElement)
        'Early Exit
        If element.Name <> SeriesPropertiesTag Then Exit Sub
        'Set up the series
        plot.Series.Clear()

        Dim tempSeries As Wpf.Series
        For Each el In element.Elements(GenericSeriesControl.SeriesPropertiesTag)
            tempSeries = GenericSeriesControl.XElementToSeriesProperties(el)
            If IsNothing(tempSeries) Then Continue For
            plot.Series.Add(tempSeries) 'This doesn't bring any data, just loads the properties
        Next

    End Sub

    ''' <summary>
    ''' Load series property settings from XElement.
    ''' </summary>
    ''' <param name="element">XElement that contains the settings.</param>
    Public Shared Function XElementToSeriesProperties(element As XElement) As Wpf.Series
        'Early Exit
        If IsNothing(element) Then Return Nothing
        If element.Name <> SeriesPropertiesTag Then Return Nothing
        'Set up converters
        Dim fontWeightConverter = New FontWeightConverter()
        Dim thicknessConverter = New ThicknessConverter()
        Dim oxycolorConverter = New Wpf.OxyColorConverter()
        Dim brushConverter = New BrushConverter()
        Dim fontFamilyConverter = New FontFamilyConverter()
        Dim booleanToVisibilityConverter = New BooleanToVisibilityConverter()

        'Set up series to return
        Dim series As Wpf.Series = Nothing

        Dim seriesType As String = Nothing
        GetStringAttribute(element, "SeriesType", seriesType)

        'Create the Series Type and Deserialize properties specific to the series
        Select Case seriesType
            Case GetType(OxyPlot.Wpf.HeatMapSeries).ToString
                series = New OxyPlot.Wpf.HeatMapSeries()
            Case GetType(OxyPlot.Wpf.LineSeries).ToString
                series = New OxyPlot.Wpf.LineSeries()
            Case GetType(OxyPlot.Wpf.ColumnSeries).ToString
                series = New OxyPlot.Wpf.ColumnSeries
            Case GetType(OxyPlot.Wpf.BarSeries).ToString
                series = New OxyPlot.Wpf.BarSeries
            Case GetType(OxyPlot.Wpf.HistogramSeries).ToString
                series = New OxyPlot.Wpf.HistogramSeries
            Case GetType(OxyPlot.Wpf.ScatterPointSeries).ToString
                series = New OxyPlot.Wpf.ScatterPointSeries
            Case GetType(OxyPlot.Wpf.ScatterErrorSeries).ToString
                series = New OxyPlot.Wpf.ScatterErrorSeries
            Case GetType(OxyPlot.Wpf.AreaSeries).ToString
                series = New OxyPlot.Wpf.AreaSeries
            Case GetType(OxyPlot.Wpf.BoxPlotSeries).ToString
                series = New OxyPlot.Wpf.BoxPlotSeries
            Case Else
                Return Nothing 'not a recognized type
        End Select

        Dim bConverter As New BrushConverter()
        'Deserialize General Series Properties
        Dim generalElement = element.Element("General")
        If Not IsNothing(generalElement) Then
            GetStringAttribute(generalElement, NameOf(series.Name), series.Name)
            GetStringAttribute(generalElement, NameOf(series.Title), series.Title)
            GetBooleanAttribute(generalElement, NameOf(series.IsEnabled), series.IsEnabled)
            If Not IsNothing(generalElement.Attribute(NameOf(series.Visibility))) Then
                Dim visibilityString = generalElement.Attribute(NameOf(series.Visibility)).Value
                If visibilityString = "Visible" Then series.Visibility = Visibility.Visible
                If visibilityString = "Hidden" Then series.Visibility = Visibility.Hidden
                If visibilityString = "Collapsed" Then series.Visibility = Visibility.Collapsed
            End If

            GetBooleanAttribute(generalElement, NameOf(series.RenderInLegend), series.RenderInLegend)
            GetBrushAttribute(generalElement, NameOf(series.Background), bConverter, series.Background)
            GetBrushAttribute(generalElement, NameOf(series.Foreground), bConverter, series.Foreground)
            GetColorAttribute(generalElement, NameOf(series.Color), series.Color)
            GetFontFamilyAttribute(generalElement, NameOf(series.FontFamily), fontFamilyConverter, series.FontFamily)
            GetDoubleAttribute(generalElement, NameOf(series.FontSize), series.FontSize)
            GetFontWeightAttribute(generalElement, NameOf(series.FontWeight), fontWeightConverter, series.FontWeight)
            GetThicknessAttribute(generalElement, NameOf(series.Padding), thicknessConverter, series.Padding)
            GetStringAttribute(generalElement, NameOf(series.TrackerFormatString), series.TrackerFormatString)
            GetStringAttribute(generalElement, NameOf(series.TrackerKey), series.TrackerKey)
        End If

        'Deserialize XY Axis Series Properties
        Dim xyAxisSeries = TryCast(series, Wpf.XYAxisSeries)
        If Not IsNothing(xyAxisSeries) Then
            Dim xyAxesSeriesElement = element.Element(NameOf(Wpf.XYAxisSeries))
            GetStringAttribute(xyAxesSeriesElement, NameOf(xyAxisSeries.XAxisKey), xyAxisSeries.XAxisKey)
            GetStringAttribute(xyAxesSeriesElement, NameOf(xyAxisSeries.YAxisKey), xyAxisSeries.YAxisKey)
        End If

        'Deserialize Data Series Properties
        Dim dataPointSeries = TryCast(series, Wpf.DataPointSeries)
        If Not IsNothing(dataPointSeries) Then
            Dim dataPointSeriesElement = element.Element(NameOf(Wpf.DataPointSeries))
            GetBooleanAttribute(dataPointSeriesElement, NameOf(dataPointSeries.CanTrackerInterpolatePoints), dataPointSeries.CanTrackerInterpolatePoints)
            GetStringAttribute(dataPointSeriesElement, NameOf(dataPointSeries.YAxisKey), dataPointSeries.YAxisKey)
            GetStringAttribute(dataPointSeriesElement, NameOf(dataPointSeries.YAxisKey), dataPointSeries.YAxisKey)
            GetStringAttribute(dataPointSeriesElement, NameOf(dataPointSeries.DataFieldX), dataPointSeries.DataFieldX)
            GetStringAttribute(dataPointSeriesElement, NameOf(dataPointSeries.DataFieldY), dataPointSeries.DataFieldY)
        End If

        'Deserialize Bar series Base properties (can be Bar, Column, or Histogram)
        Dim barBaseSeries = TryCast(series, Wpf.BarSeriesBase)
        If Not IsNothing(barBaseSeries) Then
            Dim barBaseSeriesElement = element.Element(NameOf(Wpf.BarSeriesBase))
            GetDoubleAttribute(barBaseSeriesElement, NameOf(barBaseSeries.BaseValue), barBaseSeries.BaseValue)
            GetColorAttribute(barBaseSeriesElement, NameOf(barBaseSeries.FillColor), barBaseSeries.FillColor)
            GetStringAttribute(barBaseSeriesElement, NameOf(barBaseSeries.ColorField), barBaseSeries.ColorField)
            GetStringAttribute(barBaseSeriesElement, NameOf(barBaseSeries.ValueField), barBaseSeries.ValueField)
            GetDoubleAttribute(barBaseSeriesElement, NameOf(barBaseSeries.LabelMargin), barBaseSeries.LabelMargin)
            GetColorAttribute(barBaseSeriesElement, NameOf(barBaseSeries.StrokeColor), barBaseSeries.StrokeColor)
            GetStringAttribute(barBaseSeriesElement, NameOf(barBaseSeries.StackGroup), barBaseSeries.StackGroup)
            GetColorAttribute(barBaseSeriesElement, NameOf(barBaseSeries.NegativeFillColor), barBaseSeries.NegativeFillColor)
            If GetEnumAttribute(barBaseSeriesElement, NameOf(barBaseSeries.LabelPlacement), barBaseSeries.LabelPlacement) = False Then barBaseSeries.LabelPlacement = OxyPlot.Series.LabelPlacement.Inside
            GetDoubleAttribute(barBaseSeriesElement, NameOf(barBaseSeries.StrokeThickness), barBaseSeries.StrokeThickness)
            GetBooleanAttribute(barBaseSeriesElement, NameOf(barBaseSeries.IsStacked), barBaseSeries.IsStacked)
        End If

        'Deserialize bar series properties
        Dim barSeries = TryCast(series, Wpf.BarSeries)
        If Not IsNothing(barSeries) Then
            Dim barSeriesElement = element.Element(NameOf(Wpf.BarSeries))
            GetDoubleAttribute(barSeriesElement, NameOf(barSeries.BarWidth), barSeries.BarWidth)
        End If

        'Deserialize column properties
        Dim columnSeries = TryCast(series, Wpf.ColumnSeries)
        If Not IsNothing(columnSeries) Then
            Dim columnSeriesElement = element.Element(NameOf(Wpf.ColumnSeries))
            GetDoubleAttribute(columnSeriesElement, NameOf(columnSeries.ColumnWidth), columnSeries.ColumnWidth)
        End If

        'Deserialize histogram series properties
        Dim histogramSeries = TryCast(series, Wpf.HistogramSeries)
        If Not IsNothing(histogramSeries) Then
            Dim histogramSeriesElement = element.Element(NameOf(Wpf.HistogramSeries))
            GetColorAttribute(histogramSeriesElement, NameOf(histogramSeries.FillColor), histogramSeries.FillColor)
            GetColorAttribute(histogramSeriesElement, NameOf(histogramSeries.NegativeFillColor), histogramSeries.NegativeFillColor)
            GetStringAttribute(histogramSeriesElement, NameOf(histogramSeries.LabelFormatString), histogramSeries.LabelFormatString)
            If GetEnumAttribute(histogramSeriesElement, NameOf(histogramSeries.LabelPlacement), histogramSeries.LabelPlacement) = False Then histogramSeries.LabelPlacement = OxyPlot.Series.LabelPlacement.Inside
            GetColorAttribute(histogramSeriesElement, NameOf(histogramSeries.StrokeColor), histogramSeries.StrokeColor)
            GetDoubleAttribute(histogramSeriesElement, NameOf(histogramSeries.StrokeThickness), histogramSeries.StrokeThickness)
        End If

        'Deserialize line series properties
        Dim lineSeries = TryCast(series, Wpf.LineSeries)
        If Not IsNothing(lineSeries) Then
            Dim lineSeriesElement = element.Element(NameOf(Wpf.LineSeries))
            If GetEnumAttribute(lineSeriesElement, NameOf(lineSeries.LineJoin), lineSeries.LineJoin) = False Then lineSeries.LineJoin = OxyPlot.LineJoin.Bevel
            If GetEnumAttribute(lineSeriesElement, NameOf(lineSeries.LineLegendPosition), lineSeries.LineLegendPosition) = False Then lineSeries.LineLegendPosition = OxyPlot.Series.LineLegendPosition.End
            If GetEnumAttribute(lineSeriesElement, NameOf(lineSeries.LineStyle), lineSeries.LineStyle) = False Then lineSeries.LineStyle = OxyPlot.LineStyle.Automatic
            GetColorAttribute(lineSeriesElement, NameOf(lineSeries.MarkerFill), lineSeries.MarkerFill)
            'TODO - Deserialize MarkerOutline value into point array
            GetIntegerAttribute(lineSeriesElement, NameOf(lineSeries.MarkerResolution), lineSeries.MarkerResolution)
            GetDoubleAttribute(lineSeriesElement, NameOf(lineSeries.MarkerSize), lineSeries.MarkerSize)
            GetColorAttribute(lineSeriesElement, NameOf(lineSeries.MarkerStroke), lineSeries.MarkerStroke)
            GetDoubleAttribute(lineSeriesElement, NameOf(lineSeries.MarkerStrokeThickness), lineSeries.MarkerStrokeThickness)
            If GetEnumAttribute(lineSeriesElement, NameOf(lineSeries.MarkerType), lineSeries.MarkerType) = False Then lineSeries.MarkerType = OxyPlot.MarkerType.Circle
            GetDoubleAttribute(lineSeriesElement, NameOf(lineSeries.MinimumSegmentLength), lineSeries.MinimumSegmentLength)
            GetDoubleAttribute(lineSeriesElement, NameOf(lineSeries.StrokeThickness), lineSeries.StrokeThickness)
            GetStringAttribute(lineSeriesElement, NameOf(lineSeries.LabelFormatString), lineSeries.LabelFormatString)
            GetDoubleAttribute(lineSeriesElement, NameOf(lineSeries.LabelMargin), lineSeries.LabelMargin)
            GetColorAttribute(lineSeriesElement, NameOf(lineSeries.BrokenLineColor), lineSeries.BrokenLineColor)
            If GetEnumAttribute(lineSeriesElement, NameOf(lineSeries.BrokenLineStyle), lineSeries.BrokenLineStyle) = False Then lineSeries.BrokenLineStyle = OxyPlot.LineStyle.Automatic
            GetDoubleAttribute(lineSeriesElement, NameOf(lineSeries.BrokenLineThickness), lineSeries.BrokenLineThickness)
        End If

        'Deserialize area series properties
        Dim areaSeries = TryCast(series, Wpf.AreaSeries)
        If Not IsNothing(areaSeries) Then
            Dim areaSeriesElement = element.Element(NameOf(Wpf.AreaSeries))
            GetColorAttribute(areaSeriesElement, NameOf(areaSeries.Color2), areaSeries.Color2)
            GetColorAttribute(areaSeriesElement, NameOf(areaSeries.Fill), areaSeries.Fill)
            GetStringAttribute(areaSeriesElement, NameOf(areaSeries.DataFieldX2), areaSeries.DataFieldX2)
            GetStringAttribute(areaSeriesElement, NameOf(areaSeries.DataFieldY2), areaSeries.DataFieldY2)
            GetBooleanAttribute(areaSeriesElement, NameOf(areaSeries.Reverse2), areaSeries.Reverse2)
        End If

        'Deserialize box plot properties
        Dim boxPlotSeries = TryCast(series, Wpf.BoxPlotSeries)
        If Not IsNothing(boxPlotSeries) Then
            Dim boxPlotseriesElement = element.Element(NameOf(Wpf.BoxPlotSeries))
            GetDoubleAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.StrokeThickness), boxPlotSeries.StrokeThickness)
            GetColorAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.Stroke), boxPlotSeries.Stroke)
            If GetEnumAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.LineStyle), boxPlotSeries.LineStyle) = False Then boxPlotSeries.LineStyle = OxyPlot.LineStyle.Automatic
            GetBooleanAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.IsVertical), boxPlotSeries.IsVertical)
            GetColorAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.Fill), boxPlotSeries.Fill)
            If GetEnumAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.OutlierType), boxPlotSeries.OutlierType) = False Then boxPlotSeries.OutlierType = OxyPlot.MarkerType.Circle
            GetDoubleAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.WhiskerWidth), boxPlotSeries.WhiskerWidth)
            GetBooleanAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.ShowMedianAsDot), boxPlotSeries.ShowMedianAsDot)
            GetDoubleAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.MedianPointSize), boxPlotSeries.MedianPointSize)
            GetDoubleAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.OutlierSize), boxPlotSeries.OutlierSize)
            GetDoubleAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.BoxWidth), boxPlotSeries.BoxWidth)
            GetBooleanAttribute(boxPlotseriesElement, NameOf(boxPlotSeries.ShowBox), boxPlotSeries.ShowBox)
        End If

        'Deserialize scatter point series properties
        Dim scatterPointSeries = TryCast(series, Wpf.ScatterPointSeries)
        If Not IsNothing(scatterPointSeries) Then
            Dim scatterPointSeriesElement = element.Element(NameOf(Wpf.ScatterPointSeries))
            GetStringAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.DataFieldTag), scatterPointSeries.DataFieldTag)
            GetStringAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.DataFieldValue), scatterPointSeries.DataFieldValue)
            GetStringAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.ColorAxisKey), scatterPointSeries.ColorAxisKey)
            GetIntegerAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.BinSize), scatterPointSeries.BinSize)
            GetColorAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.MarkerFill), scatterPointSeries.MarkerFill)
            GetDoubleAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.MarkerSize), scatterPointSeries.MarkerSize)
            GetColorAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.MarkerStroke), scatterPointSeries.MarkerStroke)
            GetDoubleAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.MarkerStrokeThickness), scatterPointSeries.MarkerStrokeThickness)
            If GetEnumAttribute(scatterPointSeriesElement, NameOf(scatterPointSeries.MarkerType), scatterPointSeries.MarkerType) = False Then scatterPointSeries.MarkerType = OxyPlot.MarkerType.Circle
        End If

        'Deserialize scatter error series properties
        Dim scatterErrorSeries = TryCast(series, Wpf.ScatterErrorSeries)
        If Not IsNothing(scatterErrorSeries) Then
            Dim seriesElement = element.Element(NameOf(Wpf.ScatterErrorSeries))
            GetStringAttribute(seriesElement, NameOf(scatterErrorSeries.DataFieldTag), scatterErrorSeries.DataFieldTag)
            GetStringAttribute(seriesElement, NameOf(scatterErrorSeries.DataFieldValue), scatterErrorSeries.DataFieldValue)
            GetStringAttribute(seriesElement, NameOf(scatterErrorSeries.ColorAxisKey), scatterErrorSeries.ColorAxisKey)
            GetIntegerAttribute(seriesElement, NameOf(scatterErrorSeries.BinSize), scatterErrorSeries.BinSize)
            GetColorAttribute(seriesElement, NameOf(scatterErrorSeries.MarkerFill), scatterErrorSeries.MarkerFill)
            GetDoubleAttribute(seriesElement, NameOf(scatterErrorSeries.MarkerSize), scatterErrorSeries.MarkerSize)
            GetColorAttribute(seriesElement, NameOf(scatterErrorSeries.MarkerStroke), scatterErrorSeries.MarkerStroke)
            GetDoubleAttribute(seriesElement, NameOf(scatterErrorSeries.MarkerStrokeThickness), scatterErrorSeries.MarkerStrokeThickness)
            If GetEnumAttribute(seriesElement, NameOf(scatterErrorSeries.MarkerType), scatterErrorSeries.MarkerType) = False Then scatterErrorSeries.MarkerType = OxyPlot.MarkerType.Circle
            GetDoubleAttribute(seriesElement, NameOf(scatterErrorSeries.ErrorBarStopWidth), scatterErrorSeries.ErrorBarStopWidth)
            GetDoubleAttribute(seriesElement, NameOf(scatterErrorSeries.MinimumErrorSize), scatterErrorSeries.MinimumErrorSize)
            GetDoubleAttribute(seriesElement, NameOf(scatterErrorSeries.ErrorBarStrokeThickness), scatterErrorSeries.ErrorBarStrokeThickness)
            GetColorAttribute(seriesElement, NameOf(scatterErrorSeries.ErrorBarColor), scatterErrorSeries.ErrorBarColor)
            GetStringAttribute(seriesElement, NameOf(scatterErrorSeries.DataFieldLowerErrorX), scatterErrorSeries.DataFieldLowerErrorX)
            GetStringAttribute(seriesElement, NameOf(scatterErrorSeries.DataFieldUpperErrorX), scatterErrorSeries.DataFieldUpperErrorX)
            GetStringAttribute(seriesElement, NameOf(scatterErrorSeries.DataFieldLowerErrorY), scatterErrorSeries.DataFieldLowerErrorY)
            GetStringAttribute(seriesElement, NameOf(scatterErrorSeries.DataFieldUpperErrorY), scatterErrorSeries.DataFieldUpperErrorY)
        End If

        'Deserialize heat map series properties
        Dim heatMapSeries = TryCast(series, Wpf.HeatMapSeries)
        If Not IsNothing(heatMapSeries) Then
            Dim heatMapSeriesElement = element.Element(NameOf(Wpf.HeatMapSeries))
            GetStringAttribute(heatMapSeriesElement, NameOf(heatMapSeries.ColorAxisKey), heatMapSeries.ColorAxisKey)
            GetDoubleAttribute(heatMapSeriesElement, NameOf(heatMapSeries.Y0), heatMapSeries.Y0)
            GetDoubleAttribute(heatMapSeriesElement, NameOf(heatMapSeries.Y1), heatMapSeries.Y1)
            GetDoubleAttribute(heatMapSeriesElement, NameOf(heatMapSeries.X0), heatMapSeries.X0)
            GetDoubleAttribute(heatMapSeriesElement, NameOf(heatMapSeries.X1), heatMapSeries.X1)
            GetColorAttribute(heatMapSeriesElement, NameOf(heatMapSeries.HighColor), heatMapSeries.HighColor)
            GetColorAttribute(heatMapSeriesElement, NameOf(heatMapSeries.LowColor), heatMapSeries.LowColor)
            'TODO - Deserialize coordinateDefinition value
            GetBooleanAttribute(heatMapSeriesElement, NameOf(heatMapSeries.Interpolate), heatMapSeries.Interpolate)
            GetDoubleAttribute(heatMapSeriesElement, NameOf(heatMapSeries.LabelFontSize), heatMapSeries.LabelFontSize)
        End If

        Return series

    End Function

End Class
Public Class OxySeriesColorConverter
    Implements IMultiValueConverter

    Private _series As Series.Series

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color

        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then _series = Nothing : Return False
        _series = TryCast(values(1), Wpf.Series).InternalSeries
        'Convert
        If oxyCol.IsAutomatic() Then
            Select Case _series.GetType
                Case GetType(Series.LineSeries)
                    With DirectCast(_series, Series.LineSeries).ActualColor
                        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
                    End With
                Case GetType(Series.ContourSeries)
                    With DirectCast(_series, Series.ContourSeries).ActualColor
                        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
                    End With
                Case GetType(Series.HighLowSeries)
                    With DirectCast(_series, Series.HighLowSeries).ActualColor
                        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
                    End With
                Case Else
                    '"There are currently no other series that seem to use the Color property, or at least not any that have ActualColor as a dumb-dumb property."
                    'Throw New NotImplementedException()
            End Select
        End If
        '
        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        If IsNothing(_series) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        'Get boolean value
        If value.GetType <> GetType(SolidColorBrush) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        Dim c As Color = DirectCast(value, SolidColorBrush).Color
        '
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        Select Case _series.GetType
            Case GetType(Series.LineSeries)
                If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.LineSeries).ActualColor) = 0 Then
                    With DirectCast(_series, Series.LineSeries).ActualColor
                        Return {Color.FromArgb(.A, .R, .G, .B), _series}
                    End With
                End If
            Case GetType(Series.ContourSeries)
                If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.ContourSeries).ActualColor) = 0 Then
                    With DirectCast(_series, Series.ContourSeries).ActualColor
                        Return {Color.FromArgb(.A, .R, .G, .B), _series}
                    End With
                End If
            Case GetType(Series.HighLowSeries)
                If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.HighLowSeries).ActualColor) = 0 Then
                    With DirectCast(_series, Series.HighLowSeries).ActualColor
                        Return {Color.FromArgb(.A, .R, .G, .B), _series}
                    End With
                End If
        End Select
        '
        Return {c, _series}

    End Function
End Class
Public Class OxySeriesMarkerFillConverter
    Implements IMultiValueConverter

    Private _series As Series.Series

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color

        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then _series = Nothing : Return False
        _series = TryCast(values(1), Wpf.Series).InternalSeries
        'Convert
        If oxyCol.IsAutomatic() Then
            Select Case _series.GetType
                Case GetType(Series.LineSeries)
                    With DirectCast(_series, Series.LineSeries).ActualMarkerFill
                        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
                    End With
                Case GetType(Series.ScatterSeries)
                    With DirectCast(_series, Series.ScatterSeries).ActualMarkerFillColor
                        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
                    End With
                Case Else
                    '"There are currently no other series that seem to use the Color property, or at least not any that have ActualColor as a dumb-dumb property."
                    'Throw New NotImplementedException()
            End Select
        End If
        '
        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        If IsNothing(_series) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        'Get boolean value
        If value.GetType <> GetType(SolidColorBrush) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        Dim c As Color = DirectCast(value, SolidColorBrush).Color
        '
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        Select Case _series.GetType
            Case GetType(Series.LineSeries)
                If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.LineSeries).ActualMarkerFill) = 0 Then
                    With DirectCast(_series, Series.LineSeries).ActualMarkerFill
                        Return {Color.FromArgb(.A, .R, .G, .B), _series}
                    End With
                End If
            Case GetType(Series.ScatterSeries)
                If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.ScatterSeries).ActualMarkerFillColor) = 0 Then
                    With DirectCast(_series, Series.ScatterSeries).ActualMarkerFillColor
                        Return {Color.FromArgb(.A, .R, .G, .B), _series}
                    End With
                End If
        End Select
        '
        Return {c, _series}

    End Function
End Class
Public Class OxySeriesMarkerStrokeConverter
    Implements IMultiValueConverter

    Private _series As Series.Series

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color

        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then _series = Nothing : Return False
        _series = TryCast(values(1), Wpf.Series).InternalSeries
        'Convert
        If oxyCol.IsAutomatic() Then
            Select Case _series.GetType
                Case GetType(Series.LineSeries)
                    With DirectCast(_series, Series.LineSeries).ActualMarkerFill
                        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
                    End With
                Case GetType(Series.ScatterSeries)
                    With DirectCast(_series, Series.ScatterSeries).ActualMarkerFillColor
                        Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
                    End With
                Case Else
                    '"There are currently no other series that seem to use the Color property, or at least not any that have ActualColor as a dumb-dumb property."
                    'Throw New NotImplementedException()
            End Select
        End If
        '
        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        If IsNothing(_series) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        'Get boolean value
        If value.GetType <> GetType(SolidColorBrush) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        Dim c As Color = DirectCast(value, SolidColorBrush).Color
        '
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        Select Case _series.GetType
            Case GetType(Series.LineSeries)
                If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.LineSeries).ActualMarkerFill) = 0 Then
                    With DirectCast(_series, Series.LineSeries).ActualMarkerFill
                        Return {Color.FromArgb(.A, .R, .G, .B), _series}
                    End With
                End If
            Case GetType(Series.ScatterSeries)
                If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.ScatterSeries).ActualMarkerFillColor) = 0 Then
                    With DirectCast(_series, Series.ScatterSeries).ActualMarkerFillColor
                        Return {Color.FromArgb(.A, .R, .G, .B), _series}
                    End With
                End If
        End Select
        '
        Return {c, _series}

    End Function
End Class
