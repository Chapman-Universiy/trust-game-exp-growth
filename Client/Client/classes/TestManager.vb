'A testing module that will handel each experiment case
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider

Public Class TestManager

    Private _randomGenerator As Random
    Public TimerSubmit As Timer
    Private _testSubmitTime As Date
    Dim _leftBBot As ButtonAutomationPeer
    Dim _rightBBot As ButtonAutomationPeer
    Dim _centerBBot As ButtonAutomationPeer
    Dim _pressLeft As IInvokeProvider
    Dim _pressRight As IInvokeProvider
    Dim _pressCenter As IInvokeProvider

    Dim _isSubmitted As Boolean

    Public Sub InitializeTestFunctions()
        Try
            _leftBBot = New ButtonAutomationPeer(frmMain.MainControl1.btnMainLeft)
            _rightBBot = New ButtonAutomationPeer(frmMain.MainControl1.btnMainRight)
            _centerBBot = New ButtonAutomationPeer(frmMain.MainControl1.btnMainCenter)
            _pressLeft = _leftBBot.GetPattern(PatternInterface.Invoke)
            _pressRight = _rightBBot.GetPattern(PatternInterface.Invoke)
            _pressCenter = _centerBBot.GetPattern(PatternInterface.Invoke)

            _randomGenerator = New Random(PlayerData.myID * PlayerData.partnerID)
            TimerSubmit = New Timer()
            AddHandler TimerSubmit.Tick, AddressOf TimerTick
            TimerSubmit.Interval = 500
            TimerSubmit.Enabled = False
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub TimerTick()
        Try
            Dim timeLeft = _testSubmitTime.Subtract(Now).TotalSeconds
            If timeLeft < 0 Then
                TimerSubmit.Enabled = False
                PerformStageSubmit()
            End If

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub AutomatedStageResponse(ByVal stage As Integer)
        Try
            With frmMain.MainControl1
                Dim isSender = PlayerScreen.GetIsSender(PlayerData.myPlayerType)
                Dim isReceiver = Not isSender

                Select Case stage
                    Case 1
                        If isSender Then
                            .txtMain_Input2.Focus()
                            .txtMain_Input2.Text = Math.Round(_randomGenerator.NextDouble() * GameSetupVars.senderEndowment, 2)
                            .txtMain_Input3.Focus()
                        End If
                    Case 2
                        If isReceiver Then
                            Dim toKeep As Double
                            Dim pKeep As Double
                            Dim pGive As Double

                            toKeep = Math.Round(_randomGenerator.NextDouble() * PlayerData.Received, 2)

                            pGive = toKeep - Math.Round(_randomGenerator.NextDouble() * toKeep, 2)
                            pKeep = toKeep - pGive

                            .txtMain_Input1.Focus()
                            .txtMain_Input1.Text = pKeep

                            .txtMain_Input2.Focus()
                            .txtMain_Input2.Text = pGive

                            .txtMain_Input3.Focus()
                        End If
                    Case 3

                End Select

                _isSubmitted = False
                SetSubmitTime()
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub SetSubmitTime()
        Try
            If Not _isSubmitted Then
                _testSubmitTime = Now.AddSeconds(_randomGenerator.Next(1, GameSetupVars.stageLengths(CurrentStage - 1) + _randomGenerator.NextDouble() * 3))
                TimerSubmit.Enabled = True
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub PerformStageSubmit()
        Try
            With frmMain

                With .MainControl1
                    If .btnMainCenter.IsVisible AndAlso Not .btnMainCenter.IsEnabled Then Exit Sub
                    If .btnMainLeft.IsVisible AndAlso Not .btnMainLeft.IsEnabled Then Exit Sub
                    If .btnMainRight.IsVisible AndAlso Not .btnMainRight.IsEnabled Then Exit Sub
                End With

                Dim isSender = PlayerScreen.GetIsSender(PlayerData.myPlayerType)
                Dim isReceiver = Not isSender
                Select Case CurrentStage

                    Case 1
                        If isSender Then _pressLeft.Invoke()
                    Case 2
                        If isReceiver Then _pressLeft.Invoke()
                    Case 3
                        _pressCenter.Invoke()
                End Select
                _isSubmitted = True
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

End Class
