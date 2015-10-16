Imports System.Windows.Documents
Imports System.Windows
Imports System.Windows.Input
Imports System.Text.RegularExpressions
Imports System.Text


Class MainControl

    'This method enables a user to click the top of the form and move it around
    Private Sub Label_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            With frmMain
                If e.LeftButton = Windows.Input.MouseButtonState.Pressed Then
                    .SpecialMouseDown(sender, True)
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Used to apply a font color to the word you specify in wordToFormat (see rtxtMainText_TextChanged)
    Private Sub FormatWordsInRTB(ByVal wordToFormat As String, ByVal fontColor As Media.Color)
        Try
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
                            selection.ApplyPropertyValue(TextElement.ForegroundProperty, New Media.SolidColorBrush(fontColor))
                            .Selection.[Select](selection.Start, selection.[End])
                            .Focus()
                        End If
                    End If
                    current = current.GetNextContextPosition(LogicalDirection.Forward)
                End While
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Uncomment the first line below if you wanted the word "Red" to have a font color of Red 
    'whenever it appeared in the rich text box.
    'Uncomment the second line below if you wanted the word "Blue" to have a font color of Blue 
    'whenever it appeared in the rich text box.
    'You could have as many of these as you want in this method.
    Private Sub rtxtMainText_TextChanged(ByVal sender As Object, ByVal e As Controls.TextChangedEventArgs)
        Try
            'FormatWordsInRTB("Red", Media.Color.FromRgb(255, 0, 0))
            'FormatWordsInRTB("Blue", Media.Color.FromRgb(0, 0, 255))
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'The way this works is that every time a button is clicked, it is caught by this method.
    '   The center button has its own method: CenterButton()
    '   The right and left buttons share the method: LeftOrRightButton(ByVal isLeftButton As Boolean)
    '       Usually the left button is used to confirm something and the right button is used to 
    '       reject something. Or in the case of instructions or survey pages, the left button would
    '       be used go to the last page and the right button would be used to go to the next page.
   Private Sub Button_Click(sender As Controls.Button, e As System.Windows.RoutedEventArgs) Handles btnMainLeft.Click, btnMainRight.Click, btnMainCenter.Click
        Try
            Select Case sender.Name
                Case "btnMainLeft"
                    LeftOrRightButton(True)
                Case "btnMainRight"
                    LeftOrRightButton(False)
                Case "btnMainCenter"
                    CenterButton()
            End Select
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'The alt+F4 key has been replaced by the alt+K to forcefully close this program
    Private Sub UserControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles UserControl.PreviewKeyDown
        Try
            If e.Key.Equals(Key.System) Then
                If Keyboard.IsKeyDown(Key.K) Then
                    If MessageBox.Show("Close Program?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
                    modMain.closing = True
                    frmMain.Close()
                ElseIf Keyboard.IsKeyDown(Key.Q) Then
                    frmConnect.Show()
                End If
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Add to a TextBox's PreviewTextInput event handler if you want to restrict the user to entering only real numbers.
    Private Sub RealNumbersOnly_PreviewTextInput(ByVal sender As Controls.TextBox, ByVal e As System.Windows.Input.TextCompositionEventArgs) _
            Handles txtMain_Input1.PreviewTextInput, txtMain_Input2.PreviewTextInput, txtMain_Input3.PreviewTextInput
        Try
            Dim keyPressed = e.Text
            Dim currentText = sender.Text.ToCharArray()

            If keyPressed.Equals(".") Then
                If currentText.Contains(".") Then
                    e.Handled = True
                    Exit Sub
                End If
            Else
                If Not System.Text.RegularExpressions.Regex.IsMatch(keyPressed, "\d+") AndAlso Not [Char].IsControl(keyPressed) Then
                    e.Handled = True
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Add to a TextBox's PreviewTextInput event handler if you want to restrict the user to entering only whole numbers.
    Private Sub WholeNumbersOnly_PreviewTextInput(ByVal sender As Controls.TextBox, ByVal e As System.Windows.Input.TextCompositionEventArgs) _
            Handles txtMain_Input1.PreviewTextInput, txtMain_Input2.PreviewTextInput, txtMain_Input3.PreviewTextInput
        Try
            Dim keyPressed = e.Text
            Dim currentText = sender.Text.ToCharArray()

            If Not System.Text.RegularExpressions.Regex.IsMatch(keyPressed, "\d+") AndAlso Not [Char].IsControl(keyPressed) Then
                e.Handled = True
                Exit Sub
            End If

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'This is a method that is used to check the input boxes and display an error message if the user
    'typed in values that are not allowed. This is something that will be rewirten for every experiment
    'or removed if it is not needed.
    Private Sub TextBox_LostFocus(ByVal sender As Controls.TextBox, ByVal e As System.Windows.RoutedEventArgs) _
                Handles txtMain_Input1.LostFocus, txtMain_Input2.LostFocus
        Try
            If Not txtMain_Input1.Text.Equals("") Then txtMain_Input1.Text = Math.Round(CDbl(txtMain_Input1.Text), 2).ToString("F")
            If Not txtMain_Input2.Text.Equals("") Then txtMain_Input2.Text = Math.Round(CDbl(txtMain_Input2.Text), 2).ToString("F")
            
            If CurrentStage = 2 AndAlso Not PlayerScreen.GetIsSender(PlayerData.myPlayerType) Then

                If txtMain_Input1.Text.Equals("") Or txtMain_Input2.Text.Equals("") Then
                    txtMain_Message.Text = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB2_Standard)
                    Exit Sub
                End If

                Dim inputTotal = CDbl(txtMain_Input1.Text) + CDbl(txtMain_Input2.Text)
                Dim messageText As String

                If inputTotal > PlayerData.Received Then
                    txtMain_Input3.Text = 0
                    messageText = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB2_Error)
                Else
                    txtMain_Input3.Text = PlayerData.Received - CDbl(txtMain_Input1.Text) - CDbl(txtMain_Input2.Text)
                    messageText = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB2_Standard)
                End If

                txtMain_Message.Text = messageText
            ElseIf CurrentStage = 1 AndAlso PlayerScreen.GetIsSender(PlayerData.myPlayerType) Then
                If txtMain_Input2.Text.Equals("") Then Exit Sub
                Dim amountKept = GameSetupVars.senderEndowment - CDbl(txtMain_Input2.Text)

                If amountKept < 0 Then
                    txtMain_Input3.Text = 0
                Else
                    txtMain_Input3.Text = amountKept
                End If
            End If

            If Not txtMain_Input3.Text.Equals("") Then txtMain_Input3.Text = Math.Round(CDbl(txtMain_Input3.Text), 2).ToString("F")
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
End Class
