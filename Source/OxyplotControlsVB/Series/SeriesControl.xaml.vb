Imports OxyPlot.Wpf

Public Class SeriesControl
    Public Shared SeriesProperty As DependencyProperty = DependencyProperty.Register(NameOf(Series), GetType(Series), GetType(SeriesControl), New PropertyMetadata(Nothing, AddressOf InitializeControl))
    Public Property Series As Series
        Get
            Return DirectCast(GetValue(SeriesProperty), Series)
        End Get
        Set(value As Series)
            SetValue(SeriesProperty, value)
        End Set
    End Property

    Private Shared Sub InitializeControl(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(SeriesControl) Then Exit Sub
        Dim thisControl = DirectCast(d, SeriesControl)
        'Clear the properties controls
        thisControl.SeriesGrid.Children.Clear()
        If thisControl._genericControl IsNot Nothing Then
            thisControl._genericControl.Series = Nothing
            thisControl._genericControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._scatterControl IsNot Nothing Then
            thisControl._scatterControl.Series = Nothing
            thisControl._scatterControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._lineControl IsNot Nothing Then
            thisControl._lineControl.Series = Nothing
            thisControl._lineControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._boxPlotControl IsNot Nothing Then
            thisControl._boxPlotControl.Series = Nothing
            thisControl._boxPlotControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._barControl IsNot Nothing Then
            thisControl._barControl.Series = Nothing
            thisControl._barControl.ExpanderStyle = thisControl.ExpanderStyle
        End If

        ' Get the new series
        If e.NewValue Is Nothing Then Exit Sub
        Dim wpfSeries As Series = TryCast(e.NewValue, Series)
        If wpfSeries Is Nothing Then Exit Sub
        '
        'Bar/Column
        Dim barSeries = TryCast(wpfSeries, BarSeriesBase)
        If barSeries IsNot Nothing Then
            If thisControl._barControl Is Nothing Then thisControl._barControl = New BarSeriesControl() With {.ExpanderStyle = thisControl.ExpanderStyle}
            thisControl._barControl.Series = barSeries
            thisControl.SeriesGrid.Children.Add(thisControl._barControl)
            Exit Sub
        End If
        '
        'Line/Area/StairStep/ThreeColorLine/TwoColorLine
        Dim lineSeries = TryCast(wpfSeries, LineSeries)
        If lineSeries IsNot Nothing Then
            If thisControl._lineControl Is Nothing Then thisControl._lineControl = New LineSeriesControl() With {.ExpanderStyle = thisControl.ExpanderStyle}
            thisControl._lineControl.Series = lineSeries
            thisControl.SeriesGrid.Children.Add(thisControl._lineControl)
            Exit Sub
        End If
        '
        'Scatter/ScatterError
        Dim scatterSeries = TryCast(wpfSeries, ScatterSeries(Of OxyPlot.Series.ScatterPoint))
        If scatterSeries IsNot Nothing Then
            If thisControl._scatterControl Is Nothing Then thisControl._scatterControl = New ScatterSeriesControl() With {.ExpanderStyle = thisControl.ExpanderStyle}
            thisControl._scatterControl.Series = scatterSeries
            thisControl.SeriesGrid.Children.Add(thisControl._scatterControl)
            Exit Sub
        End If
        '
        'BoxPlot
        Dim boxPlotSeries = TryCast(wpfSeries, BoxPlotSeries)
        If boxPlotSeries IsNot Nothing Then
            If thisControl._boxPlotControl Is Nothing Then thisControl._boxPlotControl = New BoxPlotSeriesControl() With {.ExpanderStyle = thisControl.ExpanderStyle}
            thisControl._boxPlotControl.Series = boxPlotSeries
            thisControl.SeriesGrid.Children.Add(thisControl._boxPlotControl)
            Exit Sub
        End If
        '
        If thisControl._genericControl Is Nothing Then thisControl._genericControl = New GenericSeriesControl() With {.ExpanderStyle = thisControl.ExpanderStyle}
        thisControl._genericControl.Series = wpfSeries
        thisControl.SeriesGrid.Children.Add(thisControl._genericControl)

    End Sub

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(SeriesControl), New PropertyMetadata(Nothing, AddressOf ExpandStyleChanged))

    Private Shared Sub ExpandStyleChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If IsNothing(d) Then Exit Sub
        If d.GetType <> GetType(SeriesControl) Then Exit Sub
        Dim thisControl = DirectCast(d, SeriesControl)
        If thisControl._genericControl IsNot Nothing Then
            thisControl._genericControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._scatterControl IsNot Nothing Then
            thisControl._scatterControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._lineControl IsNot Nothing Then
            thisControl._lineControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._boxPlotControl IsNot Nothing Then
            thisControl._boxPlotControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
        If thisControl._barControl IsNot Nothing Then
            thisControl._barControl.ExpanderStyle = thisControl.ExpanderStyle
        End If
    End Sub

    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property
    '
    'Lazy loading to improve initialization times
    Private _genericControl As GenericSeriesControl
    Private _scatterControl As ScatterSeriesControl
    Private _lineControl As LineSeriesControl
    Private _boxPlotControl As BoxPlotSeriesControl
    Private _barControl As BarSeriesControl

    Public Sub CloseExpanders()
        If _genericControl IsNot Nothing Then _genericControl.CloseExpanders()
        If _scatterControl IsNot Nothing Then _scatterControl.CloseExpanders()
        If _lineControl IsNot Nothing Then _lineControl.CloseExpanders()
        If _boxPlotControl IsNot Nothing Then _boxPlotControl.CloseExpanders()
        If _barControl IsNot Nothing Then _barControl.CloseExpanders()
    End Sub

    Public Sub Expand(expansionZone As OxyplotPropertiesControl.PropertyEXP)
        If _genericControl IsNot Nothing AndAlso _genericControl.Series IsNot Nothing Then _genericControl.Expand(expansionZone)
        If _scatterControl IsNot Nothing AndAlso _scatterControl.Series IsNot Nothing Then _scatterControl.Expand(expansionZone)
        If _lineControl IsNot Nothing AndAlso _lineControl.Series IsNot Nothing Then _lineControl.Expand(expansionZone)
        If _boxPlotControl IsNot Nothing AndAlso _boxPlotControl.Series IsNot Nothing Then _boxPlotControl.Expand(expansionZone)
        If _barControl IsNot Nothing AndAlso _barControl.Series IsNot Nothing Then _barControl.Expand(expansionZone)
    End Sub
End Class
