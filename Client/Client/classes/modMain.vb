'Original template by: Jeffrey Kirchner, kirchner@chapman.edu/jkirchner@gmail.com
'New template by: Domenic Donato, Donato@Chapman.edu/DomenicJDonato@gmail.com
'Experiment by: your name here
'Economic Science Institute & ASBE, Chapman University 2008-2010 ©

Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports System.Windows

Module modMain
    Public Sfile As String
    Public FormLocation As Drawing.Point
    Public FormBounds As Rectangle

    Public CurrentPeriod As Integer                     'current period of experiment
    Public CurrentStage As Integer
    Public MyIPAddress As String                        'IP address of client 
    Public MyPortNumber As String                       'port number of client

    Public CurrentInstructionPage As Integer            'current instruction
    Public InstructionsTotalPages As Integer
    Public IsDoneWithInstructions As Boolean

    Public WithEvents WskClient As Winsock
    Public Closing As Boolean = False

    Public ReferenceTime As Date
    Public StageStartTime As Date
    Public TimeLeft As Double

    Public PlayerScreen As ScreenManager
    Public GameSetupVars As EnvironmentVars
    Public PlayerData As PlayerVars
    Private _test As TestManager

    Public PersonalHistory As frmInfoTable

    Public IsProgressBarOrange As Boolean
    Public IsNoTimeLeft As Boolean
    Public IsStageDone As Boolean

    Private _isGameFinished As Boolean

    Public Const TextVarIndicator As Char = "@"
    Public Const NumberOfStages As Integer = 3

    Public _buttons As List(Of Controls.Button)
    Public _leftLabels As List(Of Controls.Label)
    Public _rightLabels As List(Of Controls.Label)
    Public _inputTextBoxes As List(Of Controls.TextBox)

    Dim _pauseForm As frmPause


#Region " Winsock Code "

    Private Sub wskClient_DataArrival(ByVal sender As Object, ByVal e As WinsockDataArrivalEventArgs) _
        Handles WskClient.DataArrival
        Try
            Dim buf As String = Nothing
            CType(sender, Winsock).Get(buf)

            Dim msgtokens() As String = buf.Split("#")
            Dim i As Integer

            'appEventLog_Write("data arrival: " & buf)

            For i = 1 To msgtokens.Length - 1
                takeMessage(msgtokens(i - 1))
            Next

        Catch ex As Exception
            appEventLog_Write("error wskClient_DataArrival:", ex)
        End Try
    End Sub

    Private Sub wskClient_ErrorReceived(ByVal sender As System.Object, ByVal e As WinsockErrorEventArgs) _
        Handles WskClient.ErrorReceived
        ' Log("Error: " & e.Message)
    End Sub

    Private Sub wskClient_StateChanged(ByVal sender As Object, ByVal e As WinsockStateChangingEventArgs) _
        Handles WskClient.StateChanged
        Try
            'appEventLog_Write("state changed")

            If e.New_State = WinsockStates.Closed Then
                frmConnect.Show()
            End If
        Catch ex As Exception
            appEventLog_Write("error wskClient_StateChanged:", ex)
        End Try
    End Sub

    Public Sub connect()
        Try

            WskClient = New Winsock
            WskClient.BufferSize = 8192
            WskClient.LegacySupport = False
            WskClient.LocalPort = 8080
            WskClient.MaxPendingConnections = 1
            WskClient.Protocol = WinsockProtocols.Tcp
            WskClient.RemotePort = MyPortNumber
            WskClient.RemoteServer = MyIPAddress
            WskClient.SynchronizingObject = frmMain

            WskClient.Connect()
        Catch
            frmMain.Hide()
            frmConnect.Show()
        End Try
    End Sub

#End Region

#Region " General Functions "

    Public Sub main()
        AppEventLog_Init()
        appEventLog_Write("Begin")

        ToggleScreenSaverActive(False)

        Forms.Application.EnableVisualStyles()
        Forms.Application.Run(frmMain)

        ToggleScreenSaverActive(True)

        appEventLog_Write("End")
        AppEventLog_Close()
    End Sub

    Public Function timeConversion(ByVal sec As Integer) As String
        Try
            'appEventLog_Write("time conversion :" & sec)
            timeConversion = Format((sec \ 60), "00") & ":" & Format((sec Mod 60), "00")
        Catch ex As Exception
            appEventLog_Write("error timeConversion:", ex)
            timeConversion = ""
        End Try
    End Function

    Public Sub setID(ByVal sinstr As String)
        Try
            'appEventLog_Write("set id :" & sinstr)

            Dim msgtokens() As String

            msgtokens = sinstr.Split(";")

            PlayerData.myID = msgtokens(0)

            appEventLog_Write("Client# = " & PlayerData.myID)

        Catch ex As Exception
            appEventLog_Write("error setID:", ex)
        End Try
    End Sub


    Public Sub sendIPAddress(ByVal sinstr As String)
        Try
            'appEventLog_Write("send ip :" & sinstr)

            With frmMain
                'Dim outstr As String

                PlayerData.myID = sinstr

                appEventLog_Write("Client# = " & PlayerData.myID)

                'outstr = SystemInformation.ComputerName
                '.wskClient.Send("03", outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error sendIPAddress:", ex)
        End Try
    End Sub

    Public Function numberSuffix(ByVal sinstr As Integer) As String
        numberSuffix = sinstr
        Try
            Select Case sinstr
                Case 1
                    numberSuffix = sinstr & "st"
                Case 2
                    numberSuffix = sinstr & "nd"
                Case 3
                    numberSuffix = sinstr & "rd"
                Case Is >= 4
                    numberSuffix = sinstr & "th"
            End Select
        Catch ex As Exception
            appEventLog_Write("error numberSuffix:", ex)
        End Try
    End Function

#End Region

    Private Sub takeMessage(ByVal sinstr As String)
        Try
            'take message from server
            'msgtokens(0) has type of message sent, having different types of messages allows you to send different formats for different actions.
            'msgtokens(1) has the semicolon delimited data that is to be parsed and acted upon.

            Dim msgtokens() As String
            msgtokens = sinstr.Split("|")

            Select Case msgtokens(0) 'case statement to handle each of the different types of messages
                Case "01"
                    'close client
                    Closing = True

                    WskClient.Close()
                    frmMain.Close()

                Case "02"
                    Begin()
                Case "03"
                    setID(msgtokens(1))
                Case "04"
                    finishedInstructions()
                Case "05"
                    sendIPAddress(msgtokens(1))
                Case "06"
                    StartEndGame()
                Case "07"
                    endEarly(msgtokens(1))
                Case "08"
                    StartStage(msgtokens(1))
                Case "09"
                    TakePeriodUpdate(msgtokens(1))
                Case "10"
                    StartPlayerInstructions()
                Case "11"

                Case "12"

                Case "13"

                Case "14"
                    nextStage(msgtokens(1))
                Case "15"
                    TakeConfirmation()
                Case "16"
                    TakePauseOrResume(msgtokens(1))
                Case "17"
                    SetEnvironmentVars(msgtokens(1))
                Case "18"
                    SetPlayerVars(msgtokens(1))
                Case "19"

            End Select
        Catch ex As Exception
            appEventLog_Write("error takeMessage:", ex)
        End Try
    End Sub


#Region "Game Flow"

    Private Sub Begin()
        With frmMain
            Try
                InitializeElements()

                If FormBounds.Width = 0 Then
                    With GameSetupVars
                        FormLocation = New Drawing.Point(.UpperLeftCorner_X, .UpperLeftCorner_Y)
                        FormBounds = New Rectangle(.UpperLeftCorner_X, .UpperLeftCorner_Y, .PrimaryWindow_Width, .PrimaryWindow_Height)
                    End With
                End If

                .Location = FormLocation
                .Bounds = FormBounds

                _isGameFinished = False
            Catch ex As Exception
                appEventLog_Write("error begin:", ex)
            End Try

        End With
    End Sub

    Private Sub StartStage(ByVal stageNumber As String)
        Try
            With frmMain
                CurrentStage = CInt(stageNumber)
                If GameSetupVars.showInstructions Then
                    GameSetupVars.showInstructions = False
                End If
                PlayerScreen.SetupScreenText()
                StartTime()

                'Overrides
                Dim isSender = PlayerScreen.GetIsSender(PlayerData.myPlayerType)
                Dim isReceiver = Not isSender
                Select Case CurrentStage
                    Case 1
                        If isReceiver Then
                            IsStageDone = True
                        End If
                    Case 2
                        If isSender Then
                            IsStageDone = True
                        End If
                    Case 3

                End Select

                If GameSetupVars.isTestMode Then
                    _test.AutomatedStageResponse(CurrentStage)
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
#End Region

#Region "User Initiated"
    'This is called when the left or right button is clicked.
    'Place your logic here with regards to collecting the data
    'that you need and sending it to the server.
    Public Sub LeftOrRightButton(ByVal isLeftButton As Boolean)
        Try
            With frmMain.MainControl1
                Dim message As String
                Dim outString As String

                If GameSetupVars.showInstructions Then
                    Dim nextPage As Integer

                    If isLeftButton Then
                        nextPage = CurrentInstructionPage - 1
                        CurrentInstructionPage = Math.Max(nextPage, 1)
                    Else
                        nextPage = CurrentInstructionPage + 1

                        CurrentInstructionPage = Math.Min(nextPage, InstructionsTotalPages)
                    End If
                    PlayerScreen.UpdateInstructions()

                    outString = CurrentInstructionPage.ToString() & ";"
                    outString &= InstructionsTotalPages.ToString()

                    If IsDoneWithInstructions Then
                        outString &= ";Ready"
                    End If
                    WskClient.Send("08", outString)
                Else
                    'A neat way have collecting all of the data from each of the
                    'visible textboxes. And making sure that something was entered
                    'into each of them (If args.Contains("") Then Exit Sub).
                    If isLeftButton Then
                        Dim args = (From textBox In _inputTextBoxes
                                   Where textBox.Visibility = Visibility.Visible
                                   Order By textBox.Name
                                   Select textBox.Text).ToArray()

                        If args.Contains("") Then Exit Sub

                        'Send the list of inputs to the LeftButtonClick(ByVal args As String())
                        'Note: the list can be one, so this method works no matter how many text boxes
                        '       showing.
                        LeftButtonClick(args)
                    End If
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Particular to this experiment. However, by 
    Private Sub LeftButtonClick(ByVal args As String())
        Try
            Dim lowBound As Double
            Dim highBound As Double
            Dim message As String

            'Make sure none of the text box values were a negative number.
            If args.Where(Function(item) CDbl(item) < 0).ToList().Any() Then Exit Sub

            'Given the CurrentStage make sure that the inputs are error free
            Select Case CurrentStage
                Case 1
                    lowBound = 0
                    highBound = GameSetupVars.senderEndowment
                    Dim sum = args.Sum(Function(text) CDbl(text))

                    If (CDbl(highBound) - sum) < 0.0 Then
                        frmMain.MainControl1.txtMain_Message.Text = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB1_Error)
                        Exit Sub
                    Else
                        PlayerData.Sent = CDbl(args(0))
                        frmMain.MainControl1.txtMain_Message.Text = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB1_Submitted)
                    End If

                    message = GameSetupVars.M1_LeftButton
                Case 2
                    highBound = PlayerData.Received

                    Dim sum = args.Sum(Function(text) CDbl(text))

                    If Not Math.Abs(sum - highBound) < 0.001 Then
                        frmMain.MainControl1.txtMain_Message.Text = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB2_Error)
                        Exit Sub
                    Else
                        PlayerData.ReceiverKept = CDbl(args(0))
                        PlayerData.SenderGiven = CDbl(args(1))
                        PlayerData.Invested = CDbl(args(2))
                        frmMain.MainControl1.txtMain_Message.Text = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB2_Submitted)
                    End If

                    message = GameSetupVars.M2_LeftButton
            End Select

            'Ask them is they are sure before sending their submission to the server
            If Not ConfirmSubmission(message) Then
                Select Case currentStage
                    Case 1
                        frmMain.MainControl1.txtMain_Message.Text = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB1_Standard)
                    Case 2
                        frmMain.MainControl1.txtMain_Message.Text = PlayerScreen.ReplaceTextVariables(GameSetupVars.MB2_Standard)
                End Select
                Exit Sub
            End If

            'Keep track of when the action was taken
            PlayerData.SubmissionTimes(CurrentStage - 1) = Now.Subtract(StageStartTime).TotalSeconds

            'VERY IMPORTANT: this is the method used to send information to the server.
            'This is all you have to call anytime you need to push the information that
            'is on the clients side to the information on the server side.
            'Please see SendPlayerData() for more information.
            SendPlayerData()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub CenterButton()
        Try
            If GameSetupVars.showInstructions Then
                WskClient.Send("07", "")
            Else
                Select Case CurrentStage
                    Case 3
                        SendPlayerData(True)
                End Select
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
#End Region

#Region "Support Methods"
    'This is where you create an instance of all the classes that will be used for
    'the rest of the experiment. If you create additional forms, you will want to 
    'initialize them here (e.g. like the frmInfoTable below).
    Private Sub InitializeElements()
        Try
            With frmMain
                PlayerScreen = New ScreenManager()
                PlayerScreen.InitializeScreenManager()
                _pauseForm = New frmPause()
                If GameSetupVars.isTestMode Then
                    _test = New TestManager()
                    _test.InitializeTestFunctions()
                End If

                PersonalHistory = New frmInfoTable(GameSetupVars.L_HistoryTable)

            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    '
    Private Sub StartTime()
        Try
            With frmMain
                IsNoTimeLeft = False
                IsStageDone = False
                StageStartTime = Now
                If GameSetupVars.showInstructions Then
                    ReferenceTime = Now.AddMinutes(GameSetupVars.instructionsDuration_Minutes)
                Else
                    ReferenceTime = Now.AddSeconds(GameSetupVars.stageLengths(CurrentStage - 1))
                End If
                '.MainControl1.pbarTime.Maximum = 1000
                '.MainControl1.pbarTime.Value = 0
                '.MainControl1.pbarTime.Foreground = Windows.Media.Brushes.Black
                IsProgressBarOrange = False
                .MainTimer.Enabled = True
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Function ConfirmSubmission(ByVal message As String) As Boolean
        Try
            If GameSetupVars.isTestMode Then
                Return True
            End If

            Const caption As String = "Confirm Submission"
            Dim result = Forms.MessageBox.Show(PlayerScreen.ReplaceTextVariables(message), caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

            If result = DialogResult.OK Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
            Return False
        End Try
    End Function
#End Region

#Region "Client Send"

    Public Sub SendPlayerData(Optional ByVal isSendNothing As Boolean = False)
        Try
            Dim outstr As String
            If isSendNothing Then
                outstr = ""
            Else
                outstr = JsonConvert.SerializeObject(PlayerData)
            End If
            WskClient.Send("05", outstr)
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub GameEnd()
        Try
            With frmMain
                If _isGameFinished Then
                    Dim outstr = JsonConvert.SerializeObject(PlayerData)
                    WskClient.Send("04", outstr)
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
#End Region

#Region "Server Updates"
    Private Sub TakeConfirmation()
        Try
            With frmMain.MainControl1

                If GameSetupVars.showInstructions Then
                    IsDoneWithInstructions = True
                    IsStageDone = True
                    .btnMainCenter.Visibility = Visibility.Hidden
                    Exit Sub
                End If

                If _isGameFinished Then
                    If frmNames.Visible Then
                        frmNames.Close()
                        frmMain.Enabled = True
                        PersonalHistory.Enabled = True

                    End If
                    PlayerScreen.EndGameScreen()
                    Exit Sub
                End If

                Dim vButtons = From button In _buttons
                               Where button.Visibility = Visibility.Visible
                               Select button

                Dim vInputTexts = From textbox In _inputTextBoxes
                                  Where textbox.Visibility = Visibility.Visible
                                  Select textbox

                For Each button As Controls.Button In vButtons
                    button.IsEnabled = False
                Next

                For Each textBox As Controls.TextBox In vInputTexts
                    textBox.IsReadOnly = True
                Next

                'Mark stage as complete
                IsStageDone = True
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub TakePeriodUpdate(ByVal period As String)
        Try
            CurrentPeriod = CInt(period)
            frmMain.MainControl1.lblPeriod.Content = "Period " & CurrentPeriod.ToString()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub StartPlayerInstructions()
        Try
            With frmMain
                PlayerScreen.InstructionsScreen()
                StartTime()
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub FinishedInstructions()
        Try
            With frmMain
                'close the instructions and start experiment

                frmInstructions.Close()
                GameSetupVars.showInstructions = False
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub NextStage(ByVal timeBuffer As String)
        Try
            ReferenceTime = Now.AddSeconds(CInt(timeBuffer))
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub EndEarly(ByVal sinstr As String)
        Try
            'end experiment early
            Dim msgtokens() As String
            msgtokens = sinstr.Split(";")

            GameSetupVars.numberOfPeriods = msgtokens(0)
        Catch ex As Exception
            appEventLog_Write("error endEarly:", ex)
        End Try
    End Sub

    Private Sub StartEndGame()
        Try
            _isGameFinished = True
            frmMain.Enabled = False
            PersonalHistory.Enabled = False
            frmNames.Show()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub TakePauseOrResume(ByVal message As String)
        Try
            With frmMain
                If frmMain.Enabled = True Then
                    If GameSetupVars.isTestMode Then
                        _test.TimerSubmit.Enabled = False
                    End If
                    .MainTimer.Enabled = False
                    frmMain.Enabled = False
                    _pauseForm.Show()
                Else
                    If GameSetupVars.isTestMode Then
                        _test.SetSubmitTime()
                    End If
                    ReferenceTime = Now.AddSeconds(TimeLeft)
                    frmMain.Enabled = True
                    .MainTimer.Enabled = True
                    _pauseForm.Hide()
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub SetEnvironmentVars(ByVal objectAsString As String)
        Try
            GameSetupVars = JsonConvert.DeserializeObject(Of EnvironmentVars)(objectAsString)
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub SetPlayerVars(ByVal objectAsString As String)
        Try
            PlayerData = JsonConvert.DeserializeObject(Of PlayerVars)(objectAsString)
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
#End Region

End Module
