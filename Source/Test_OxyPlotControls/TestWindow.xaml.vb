Public Class TestWindow
    'Public ReadOnly Property TestStrings As New List(Of String)({"test1", "test2", "test3", "test4", "test5", "test6"})



    Private Sub TestWindow_ContentRendered(sender As Object, e As EventArgs) Handles Me.ContentRendered
        'Dim t = TestCombo2.FindResource("ComboBoxTemplate")
        'Dim v = DirectCast(t, ControlTemplate).FindName("SpecialOptions", TestCombo2)
        'DirectCast(v, ItemsControl).Items.Add(New ComboBoxItem() With {.Content = "Help"})
        'TestCombo2.GetSpecialOptionsItemsControl.Items.Add(New ComboBoxItem() With {.Content = "Help"})
    End Sub
End Class
