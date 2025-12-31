Imports System.Globalization
Imports OxyPlot
Imports OxyplotControls.OxyplotPropertiesControl

Public Class ScatterSeriesControl
    Public Shared ReadOnly Property MarkerTypeOptions As List(Of MarkerType)
        Get
            Dim types As New List(Of MarkerType)(DirectCast([Enum].GetValues(GetType(MarkerType)), MarkerType()))
            types.Remove(MarkerType.Custom)
            Return types
        End Get
    End Property

    Public Shared SeriesProperty As DependencyProperty = DependencyProperty.Register(NameOf(Series), GetType(Wpf.ScatterSeries(Of Series.ScatterPoint)), GetType(ScatterSeriesControl), New PropertyMetadata(Nothing))
    Public Property Series As Wpf.ScatterSeries(Of Series.ScatterPoint)
        Get
            Return DirectCast(GetValue(SeriesProperty), Wpf.ScatterSeries(Of Series.ScatterPoint))
        End Get
        Set(value As Wpf.ScatterSeries(Of Series.ScatterPoint))
            SetValue(SeriesProperty, value)
        End Set
    End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(ScatterSeriesControl))
    Public Property ExpanderStyle As Style
        Get
            Return DirectCast(GetValue(ExpanderStyleProperty), Style)
        End Get
        Set(value As Style)
            SetValue(ExpanderStyleProperty, value)
        End Set
    End Property

    Public Sub CloseExpanders()
        LabelingEXP.IsExpanded = False
        MarkersEXP.IsExpanded = False
        ErrorBarSettingsEXP.IsExpanded = False
    End Sub
    Public Sub Expand(expansionZone As OxyplotPropertiesControl.PropertyEXP)
        Select Case expansionZone
            Case PropertyEXP.Series_General
                LabelingEXP.IsExpanded = True
            Case PropertyEXP.Series_Display, PropertyEXP.Series_Markers
                MarkersEXP.IsExpanded = True
            Case PropertyEXP.Series_ErrorBarSettings
                ErrorBarSettingsEXP.IsExpanded = True
            Case Else
        End Select
    End Sub
End Class

Public Class ScatterSeriesMarkerFillConverter
    Implements IMultiValueConverter

    Private _series As Series.ScatterSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.ScatterSeries(Of Series.ScatterPoint)).InternalSeries, Series.ScatterSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        '
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualMarkerFillColor
                Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
            End With
        End If
        '
        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        If IsNothing(_series) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        'Get color value
        If value.GetType <> GetType(SolidColorBrush) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        Dim c As Color = DirectCast(value, SolidColorBrush).Color
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        If OxyColor.ColorDifference(oxyCol, _series.ActualMarkerFillColor) = 0 Then
            With _series.ActualMarkerFillColor
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}

    End Function
End Class
Public Class ScatterSeriesMarkerStrokeConverter
    Implements IMultiValueConverter

    Private _series As Series.ScatterSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.ScatterSeries(Of Series.ScatterPoint)).InternalSeries, Series.ScatterSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        '
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualMarkerFillColor
                Return New SolidColorBrush(Color.FromArgb(.A, .R, .G, .B))
            End With
        End If
        '
        Return New SolidColorBrush(c)
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        If IsNothing(_series) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        'Get color value
        If value.GetType <> GetType(SolidColorBrush) Then Return {Color.FromArgb(255, 0, 0, 0), Nothing}
        Dim c As Color = DirectCast(value, SolidColorBrush).Color
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        If OxyColor.ColorDifference(oxyCol, _series.ActualMarkerFillColor) = 0 Then
            With _series.ActualMarkerFillColor
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}

    End Function
End Class
