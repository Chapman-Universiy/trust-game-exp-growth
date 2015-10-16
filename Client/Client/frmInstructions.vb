Public Class frmInstructions
    Dim tempS As Boolean
    Dim startPressed As Boolean
    Public Sub nextInstruction()
        Try
            'load the next page of instructions

            RichTextBox1.LoadFile(System.Windows.Forms.Application.StartupPath & _
                 "\instructions\page" & currentInstructionPage & ".rtf")

            variables()

            RichTextBox1.SelectionStart = 1
            RichTextBox1.ScrollToCaret()

            If Not startPressed Then wskClient.Send("01", currentInstructionPage & ";")
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub variables()
        Try
            'load variables into instructions

            Dim tempN As Integer = 0
            Dim outstr As String = ""
            Select Case currentInstructionPage
                Case 1
                    'Use the following command to insert varibles into the instructions.
                    'Call RepRTBfield("playerCount-1", numberOfPlayers - 1)
                Case 2

                Case 3

                Case 4

                Case 5

                Case 6

                Case 7

                Case 8


            End Select
        Catch ex As Exception
            appEventLog_Write("error variables:", ex)
        End Try
    End Sub


    Private Sub frmInstructions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            startPressed = False
            currentInstructionPage = 1
            nextInstruction()
            tempS = False
        Catch ex As Exception
            appEventLog_Write("error frmInstructions_Load:", ex)
        End Try
    End Sub

    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        Try
            'client done with instructions
            wskClient.Send("02", "")
            cmdStart.Visible = False
            startPressed = True
        Catch ex As Exception
            appEventLog_Write("error instructinos start:", ex)
        End Try
    End Sub

    Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Try
            'load next page of instructions

            If currentInstructionPage = 8 Then Exit Sub

            currentInstructionPage += 1

            If currentInstructionPage = 8 And Not tempS Then
                cmdStart.Visible = True
                tempS = True
            End If

            nextInstruction()
        Catch ex As Exception
            appEventLog_Write("error cmdNext_Click:", ex)
        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            'previous page of instructions

            If currentInstructionPage = 1 Then Exit Sub

            currentInstructionPage -= 1

            nextInstruction()
        Catch ex As Exception
            appEventLog_Write("error cmdBack_Click :", ex)
        End Try
    End Sub
End Class