Imports OxyPlot
Imports OxyplotControls.OxyplotPropertiesControl
Imports System.Globalization

Public Class LineSeriesControl

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

    Public Shared SeriesProperty As DependencyProperty = DependencyProperty.Register(NameOf(Series), GetType(Wpf.LineSeries), GetType(LineSeriesControl), New PropertyMetadata(Nothing))
    Public Property Series As Wpf.LineSeries
        Get
            Return DirectCast(GetValue(SeriesProperty), Wpf.LineSeries)
        End Get
        Set(value As Wpf.LineSeries)
            SetValue(SeriesProperty, value)
        End Set
    End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(LineSeriesControl))
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
        End Select
    End Sub
End Class
Public Class LineSeriesColorConverter
    Implements IMultiValueConverter

    Private _series As Series.LineSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.LineSeries).InternalSeries, Series.LineSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualColor
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
        If OxyColor.ColorDifference(oxyCol, _series.ActualColor) = 0 Then
            With _series.ActualColor
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}
    End Function
End Class
Public Class AreaSeriesColor2Converter
    Implements IMultiValueConverter

    Private _series As Series.AreaSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.AreaSeries).InternalSeries, Series.AreaSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualColor2
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
        If OxyColor.ColorDifference(oxyCol, _series.ActualColor2) = 0 Then
            With _series.ActualColor2
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}
    End Function
End Class
Public Class AreaSeriesFillConverter
    Implements IMultiValueConverter

    Private _series As Series.AreaSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.AreaSeries).InternalSeries, Series.AreaSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualFill
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
        If OxyColor.ColorDifference(oxyCol, _series.ActualFill) = 0 Then
            With _series.ActualFill
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}
    End Function
End Class
Public Class LineSeriesMarkerFillConverter
    Implements IMultiValueConverter

    Private _series As Series.LineSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.LineSeries).InternalSeries, Series.LineSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        '
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualMarkerFill
                Return New SolidColorBrush(Color.FromArgb(255, .R, .G, .B))
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
        If OxyColor.ColorDifference(oxyCol, DirectCast(_series, Series.LineSeries).ActualMarkerFill) = 0 Then
            With _series.ActualMarkerFill
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}

    End Function
End Class
Public Class LineSeriesMarkerStrokeConverter
    Implements IMultiValueConverter

    Private _series As Series.LineSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.LineSeries).InternalSeries, Series.LineSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        '
        'Convert
        If oxyCol.IsAutomatic() Then
            With _series.ActualMarkerFill
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
        If OxyColor.ColorDifference(oxyCol, _series.ActualMarkerFill) = 0 Then
            With _series.ActualMarkerFill
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}

    End Function
End Class