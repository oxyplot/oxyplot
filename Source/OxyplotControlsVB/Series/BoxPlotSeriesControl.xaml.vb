Imports OxyPlot
Imports OxyplotControls.OxyplotPropertiesControl
Imports System.Globalization

Public Class BoxPlotSeriesControl
    Public Shared ReadOnly BoxPlotSeriesPropertiesTag As String = "BoxPlotSeries"  'Should this be unique because you could have multiple bar series?

    Public Shared ReadOnly Property MarkerTypeOptions As List(Of MarkerType)
        Get
            Dim types As New List(Of MarkerType)(DirectCast([Enum].GetValues(GetType(MarkerType)), MarkerType()))
            types.Remove(MarkerType.Custom)
            Return types
        End Get
    End Property
    Public Shared ReadOnly Property LineStyleOptions As List(Of DoubleCollection) = GenericControls.LineStyleSelectorControl.LineStyleOptions

    Public Shared SeriesProperty As DependencyProperty = DependencyProperty.Register(NameOf(Series), GetType(Wpf.BoxPlotSeries), GetType(BoxPlotSeriesControl), New PropertyMetadata(Nothing))
    Public Property Series As Wpf.BoxPlotSeries
        Get
            Return DirectCast(GetValue(SeriesProperty), Wpf.BoxPlotSeries)
        End Get
        Set(value As Wpf.BoxPlotSeries)
            SetValue(SeriesProperty, value)
        End Set
    End Property

    Public Shared ExpanderStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(ExpanderStyle), GetType(Style), GetType(BoxPlotSeriesControl))
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
    End Sub

    Public Sub Expand(expansionZone As OxyplotPropertiesControl.PropertyEXP)
        Select Case expansionZone
            Case PropertyEXP.Series_General
                'LabelingEXP.IsExpanded = True
                DisplayEXP.IsExpanded = True
            Case PropertyEXP.Series_Display
                DisplayEXP.IsExpanded = True
        End Select
    End Sub
End Class
Public Class BoxPlotSeriesFillConverter
    Implements IMultiValueConverter

    Private _series As Series.BoxPlotSeries

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
        'Get Color
        If IsNothing(values(0)) Then Return Nothing
        If values(0).GetType <> GetType(Color) Then Return Nothing
        Dim c As Color = DirectCast(values(0), Color)
        Dim oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B)
        '
        'Get Series (this should only be set on convert with one-way binding)
        If IsNothing(values(1)) Then Return New SolidColorBrush(c)
        _series = TryCast(DirectCast(values(1), Wpf.BoxPlotSeries).InternalSeries, Series.BoxPlotSeries)
        If IsNothing(_series) Then Return New SolidColorBrush(c)
        'Convert
        If oxyCol.IsAutomatic() Then
            ' BoxPlotSeries uses GetSelectableFillColor(_series.Fill) to get the selectable color. 
            ' However, it passes selected index of -1 which essentially returns the original color. Such a backwards way of filling the boxplot rectangle vs other methods.
            With _series.Fill
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
        If OxyColor.ColorDifference(oxyCol, _series.Fill) = 0 Then
            With _series.Fill
                Return {Color.FromArgb(.A, .R, .G, .B), _series}
            End With
        End If
        '
        Return {c, _series}
    End Function
End Class
