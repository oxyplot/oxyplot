Imports System.ComponentModel
Imports OxyPlot
Public Class OxyPlotPropertiesDialog
    Public Sub New(plot As Wpf.Plot)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        PropertiesControl.Plot = plot
    End Sub

    Private Sub OxyPlotPropertiesDialog_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not IsNothing(Owner) Then Owner.Activate()
    End Sub
End Class
