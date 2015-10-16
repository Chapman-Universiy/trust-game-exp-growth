Imports System.Windows.Media
Imports System.Runtime.InteropServices

Public Class frmMain
    'Public WithEvents mobjSocketClient As TCPConnection
    Delegate Sub SetTextCallback(ByVal [text] As String)
    Delegate Sub SetTextCallback2()

    Public Const HT_CAPTION As Integer = &H2
    Public Const WM_NCLBUTTONDOWN As Integer = &HA1

    <DllImportAttribute("user32.dll")> _
    Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
    End Function
    <DllImportAttribute("user32.dll")> _
    Public Shared Function ReleaseCapture() As Boolean
    End Function

    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Try

            'if ALT+K are pressed kill the client
            'if ALT+Q are pressed bring up connection box
            If e.Alt = True Then
                If CInt(e.KeyValue) = CInt(Keys.K) Then
                    If MessageBox.Show("Close Program?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
                    modMain.closing = True
                    Me.Close()
                ElseIf CInt(e.KeyValue) = CInt(Keys.Q) Then
                    frmConnect.Show()
                End If
            End If
        Catch ex As Exception
            appEventLog_Write("error frmChat_KeyDown:", ex)
        End Try
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            sfile = System.Windows.Forms.Application.StartupPath & "\client.ini"

            playerData = New PlayerVars()

            'take IP from command line
            Dim commandLine As String = Command()
            Dim args() As String = commandLine.Split(" ")

            If args.Any() Then
                writeINI(sfile, "Settings", "ip", args(0))
                If args.Count() = 3 Then
                    Dim point = args(1).Split(",").ToList().ConvertAll(Function(item) CInt(item))
                    formLocation = New Point(point(0), point(1))
                    Dim bound = args(2).Split(",").ToList().ConvertAll(Function(item) CInt(item))
                    formBounds = New Rectangle(formLocation, New Size(bound(0), bound(1)))
                End If
            End If

            'connect
            myIPAddress = getINI(sfile, "Settings", "ip")
            myPortNumber = getINI(sfile, "Settings", "port")
            connect()

        Catch ex As Exception
            appEventLog_Write("errorfrmChat_Load :", ex)
        End Try

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainTimer.Tick
        Try
            Dim timeElapsed As Double
            timeLeft = referenceTime.Subtract(Now).TotalSeconds
            If timeLeft <= 0 Then
                timeLeft = 0
                If gameSetupVars.showInstructions Then
                    MainControl1.lblTime.Content = "Time Remaining: " & modMain.timeConversion(Math.Ceiling(TimeLeft)) & " Minutes"
                Else
                    MainControl1.lblTime.Content = "Time Remaining: " & Math.Ceiling(TimeLeft) & " Seconds"
                End If
                'MainControl1.pbarTime.Value = 1000
                'MainControl1.pbarTime.Foreground = Brushes.Red
                MainTimer.Enabled = False
                isNoTimeLeft = True
                'SendPlayerData(True)
                If Not isStageDone AndAlso Not gameSetupVars.isTestMode Then
                    MessageBox.Show(playerScreen.ReplaceTextVariables(gameSetupVars.TimeOut_M), "Time", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Else
                If GameSetupVars.showInstructions Then
                    timeElapsed = GameSetupVars.instructionsDuration_Minutes * 60 - TimeLeft
                    MainControl1.lblTime.Content = "Time Remaining: " & modMain.timeConversion(Math.Ceiling(TimeLeft)) & " Minutes"
                    'MainControl1.pbarTime.Value = Math.Round(timeElapsed / (GameSetupVars.instructionsDuration_Minutes * 60), 4) * 1000

                    If Not IsProgressBarOrange AndAlso TimeLeft < GameSetupVars.instructionsWarning_MinutesLeft * 60 Then
                        'MainControl1.pbarTime.Foreground = Brushes.OrangeRed
                        IsProgressBarOrange = True
                    End If
                Else
                    timeElapsed = GameSetupVars.stageLengths(CurrentStage - 1) - TimeLeft
                    MainControl1.lblTime.Content = "Time Remaining: " & Math.Ceiling(TimeLeft) & " Seconds"
                    'MainControl1.pbarTime.Value = Math.Round(timeElapsed / GameSetupVars.stageLengths(CurrentStage - 1), 4) * 1000

                    If Not IsProgressBarOrange AndAlso TimeLeft < GameSetupVars.warningTimes(CurrentStage - 1) Then
                        'MainControl1.pbarTime.Foreground = Brushes.OrangeRed
                        IsProgressBarOrange = True
                    End If
                End If
            End If

        Catch ex As Exception
            appEventLog_Write("error Timer1_Tick:", ex)
        End Try
    End Sub



    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Try

        Catch ex As Exception
            appEventLog_Write("error Timer2_Tick client:", ex)
        End Try
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If Not modMain.closing Then e.Cancel = True
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub SpecialMouseDown(sender As System.Object, e As Boolean)
        If e Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If
    End Sub

    Public Sub frmMain_Move(sender As System.Object, e As System.EventArgs) Handles MyBase.Move
        Try
            Const gapBetweenForms As Integer = 5
            Dim mainBounds = Bounds

            If IsNothing(GameSetupVars) Then Exit Sub

            Dim width As Integer
            Dim height As Integer = mainBounds.Height

            If PlayerScreen.GetIsSender(PlayerData.myPlayerType) Then
                With PersonalHistory.GroupInfoUC1
                    Select Case PlayerData.TreatmentString
                        Case "00" 'No Info
                            width = .cSenderAccount.ActualWidth + 5
                        Case "11" 'Both
                            width = .cSenderAccount.ActualWidth + .cIncomeStatement.ActualWidth + .cBalanceSheet.ActualWidth
                        Case "10" '10 (Income Statement) 
                            width = .cSenderAccount.ActualWidth + .cIncomeStatement.ActualWidth + 5
                        Case "01" '01 (Balance Sheet)
                            width = .cSenderAccount.ActualWidth + .cBalanceSheet.ActualWidth
                    End Select
                End With
            Else
                With PersonalHistory.GroupInfoUC1
                    width = .cSenderAccount.ActualWidth + .cReceiverAccount.ActualWidth + .cIncomeStatement.ActualWidth + .cBalanceSheet.ActualWidth
                End With
            End If

            If Not IsNothing(PersonalHistory) Then
                PersonalHistory.Bounds = New Rectangle(mainBounds.Right + gapBetweenForms, mainBounds.Bottom - height, width, height)
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub frmMain_Resize(sender As System.Object, e As System.EventArgs) Handles MyBase.Resize
        Try
            MainControl1.UserControl.Width = ElementHost1.Width
            MainControl1.UserControl.Height = ElementHost1.Height
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
End Class
