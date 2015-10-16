Imports System.Drawing.Drawing2D
Imports Newtonsoft.Json

Public Class Player
    Public socketNumber As String           'winsock ID number
    Public ipAddress As String            'IP address of player's machine 
    Public myIPAddress As String            'IP address of player's machine 

    Public isCheckedIn As Boolean
    Public playerData As PlayerVars

    Public Sub begin()
        Try
            With frmServer
                'singal to clients to start the experiment
                SetTreatmentString()

                .wsk_Col.Send("17", socketNumber, JsonConvert.SerializeObject(gameSetupVars))

                playerData.periodEarnings = New Double(gameSetupVars.numberOfPeriods - 1) {}
                playerData.SubmissionTimes = New Integer(NumberOfStages - 1) {}

                SendPlayerData()

                .wsk_Col.Send("02", socketNumber, "")
            End With

        Catch ex As Exception
            appEventLog_Write("error player begin:", ex)
        End Try
    End Sub

    Private Sub SetTreatmentString()
        Try
            Dim incomeBit As Char
            Dim balanceBit As Char

            If gameSetupVars.isShowIncomeStatement Then
                incomeBit = "1"
            Else
                incomeBit = "0"
            End If

            If gameSetupVars.isShowBalanceSheet Then
                balanceBit = "1"
            Else
                balanceBit = "0"
            End If

            playerData.TreatmentString = incomeBit & balanceBit
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub SendPlayerData()
        Try
            With frmServer
                .wsk_Col.Send("18", socketNumber, JsonConvert.SerializeObject(playerData))
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub StartPlayerInstructions()
        Try
            With frmServer
                .wsk_Col.Send("10", socketNumber, "")
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub StartStage()
        Try
            With frmServer
                Dim message As String = ""
                Dim isSender = GetIsSender(playerData.myPlayerType)
                Select Case currentStage
                    Case 1
                        If isSender Then
                            message = "Deciding"
                        Else
                            message = " "
                        End If
                    Case 2
                        If isSender Then
                            message = " "
                        Else
                            message = "Deciding"
                        End If
                    Case 3
                        If isSender Then
                            message = "Reading Summary"
                        Else
                            message = "Reading Summary"
                        End If
                End Select
                updateStatusColumn(message)
                .wsk_Col.Send("08", socketNumber, currentStage.ToString())
            End With
        Catch ex As Exception
            appEventLog_Write("Error:", ex)
        End Try
    End Sub

    Public Sub nextStage(Optional ByVal timeBuffer As Integer = 3)
        Try
            'End stage early
            With frmServer
                .wsk_Col.Send("14", socketNumber, timeBuffer.ToString())
            End With
        Catch ex As Exception
            appEventLog_Write("Error:", ex)
        End Try
    End Sub

    Public Sub resetClient()
        Try
            'kill client
            With frmServer
                .wsk_Col.Send("01", socketNumber, "")
            End With
        Catch ex As Exception
            appEventLog_Write("Error:", ex)
        End Try
    End Sub

    Public Sub requsetIP(ByVal count As Integer)
        Try
            'request the client send it's IP address
            With frmServer
                .wsk_Col.Send("05", socketNumber, CStr(count))
            End With
        Catch ex As Exception
            appEventLog_Write("error requsetIP:", ex)
        End Try
    End Sub

    Public Sub SendConfirmation()
        Try
            With frmServer
                Dim message As String = ""
                Dim isSender = GetIsSender(playerData.myPlayerType)
                Select Case currentStage
                    Case 1
                        If isSender Then
                            message = "Sent: " & playerData.Sent
                        End If
                    Case 2
                        If Not isSender Then
                            message = "Kept: " & playerData.ReceiverKept & " | Returned: " & playerData.SenderGiven & " | Invested: " & playerData.Invested
                        End If
                    Case 3
                        message = "Ready"
                End Select

                updateStatusColumn(message)

                .wsk_Col.Send("15", socketNumber, "")
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub endGame()
        Try
            'tell clients to end the game
            With frmServer
                'Compute final earnings here, don't adjust for exchange rate.

                Dim exchange As Double

                If GetIsSender(playerData.myPlayerType) Then
                    exchange = gameSetupVars.senderExchangeRate
                Else
                    exchange = gameSetupVars.receiverExchangeRate
                End If

                If gameSetupVars.isRandomPeriodEarnings Then
                    playerData.paymentPeriod = paymentPeriod
                    playerData.finalEarnings = Math.Ceiling(playerData.periodEarnings(paymentPeriod - 1) / (0.25 * exchange)) * 0.25
                Else
                    Dim eSum = Math.Ceiling(playerData.periodEarnings.Sum() / (0.25 * exchange)) * 0.25

                    If eSum > gameSetupVars.paymentCap Then
                        playerData.finalEarnings = gameSetupVars.paymentCap
                        playerData.isEarningsCapped = True
                    Else
                        playerData.finalEarnings = eSum
                        playerData.isEarningsCapped = False
                    End If

                End If

                SendPlayerData()

                .dgMainTable.Rows(playerData.myID - 1).Cells(4).Value = playerData.finalEarnings.ToString("C")

                .wsk_Col.Send("06", socketNumber, "")
            End With
        Catch ex As Exception
            appEventLog_Write("error endGame:", ex)
        End Try
    End Sub

    Public Sub endEarly()
        Try
            'end experiment early

            With frmServer
                Dim outstr As String

                outstr = gameSetupVars.numberOfPeriods & ";"
                .wsk_Col.Send("07", socketNumber, outstr)
            End With
        Catch ex As Exception
            appEventLog_Write("error endEarly:", ex)
        End Try
    End Sub

    Public Sub finishedInstructions()
        Try
            With frmServer

                .wsk_Col.Send("04", socketNumber, "")
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Public Sub updateStatusColumn(ByVal message As String)
        Try
            With frmServer
                If Not message.Equals("") Then
                    .dgMainTable(3, playerData.myID - 1).Value = message
                End If
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub Pause(ByVal message As String)
        Try
            With frmServer
                .wsk_Col.Send("16", socketNumber, message)
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub SendPeriod()
        Try
            With frmServer
                .wsk_Col.Send("09", socketNumber, currentPeriod.ToString())
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
End Class
