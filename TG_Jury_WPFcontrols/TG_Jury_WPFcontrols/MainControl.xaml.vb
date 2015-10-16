Class MainControl
    Private Sub Label_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        'Try
        '    With frmMain
        '        If e.LeftButton = Windows.Input.MouseButtonState.Pressed Then
        '            .SpecialMouseDown(sender, True)
        '        End If
        '    End With
        'Catch ex As Exception
        '    appEventLog_Write("Error: ", ex)
        'End Try
    End Sub

    Private Sub FormatWordsInRTB(ByVal wordToFormat As String, ByVal fontColor As Windows.Media.Color)
        With rtxtMainText
            Dim text As New TextRange(.Document.ContentStart, .Document.ContentEnd)
            Dim current As TextPointer = text.Start.GetInsertionPosition(LogicalDirection.Forward)

            While current IsNot Nothing
                Dim textInRun As String = current.GetTextInRun(LogicalDirection.Forward)
                If Not String.IsNullOrWhiteSpace(textInRun) Then
                    Dim index As Integer = textInRun.IndexOf(wordToFormat)

                    If index <> -1 Then
                        Dim selectionStart As TextPointer = current.GetPositionAtOffset(index, LogicalDirection.Forward)
                        Dim selectionEnd As TextPointer = selectionStart.GetPositionAtOffset(wordToFormat.Length, LogicalDirection.Forward)
                        Dim selection As New TextRange(selectionStart, selectionEnd)
                        selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold)
                        selection.ApplyPropertyValue(TextElement.ForegroundProperty, New Windows.Media.SolidColorBrush(fontColor))
                        .Selection.[Select](selection.Start, selection.[End])
                        .Focus()
                    End If
                End If
                current = current.GetNextContextPosition(LogicalDirection.Forward)
            End While
        End With
    End Sub

    Private Sub rtxtMainText_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs)
        FormatWordsInRTB("Red", Windows.Media.Color.FromRgb(255, 0, 0))
        FormatWordsInRTB("Blue", Windows.Media.Color.FromRgb(0, 0, 255))
    End Sub

    Private Sub txtMain2_Input_PreviewKeyUp(ByVal sender as Object, ByVal e as System.Windows.Input.KeyEventArgs)
    	'TODO: Add event handler implementation here.
    End Sub
End Class
