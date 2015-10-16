'Programmed by Jeffrey Kirchner and Your Name Here
'kirchner@chapman.edu/jkirchner@gmail.com
'Economic Science Institute, Chapman University 2008-2010 ©

Imports System.IO
Imports System.Text

Module modMain

    Public playerList As List(Of Player)
    'array of players
    Public playerCount As Integer                    'number of players connected
    'Public sfile As String                           'location of intialization file  
    Public checkin As Integer                        'global player counter 
    Public connectionCount As Integer                'total number of connections made since server start 
    Public SW_Summary As StreamWriter                 'data file
    Public SW_Earnings As StreamWriter                'earnings file
    Public frmServer As New frmMain                  'main form 
    Public filename As String                        'location of data file
    Public filename2 As String                       'location of data file

    'global variables here
    Public randomGen As Random
    Public randomPlayer As Random
    Public randomMultiplier As Random

    Public paymentPeriod As Integer
    Public currentInstruction As Integer             'current page of instructions
    Public currentPeriod As Integer                     'current period
    Public currentStage As Integer

    Public gameSetupVars As EnvironmentVars

    Public isPaused As Boolean
    Public referenceTime As Date
    Public timeLeft As Double
    Public isProgressBarOrange As Boolean
    Public isNoTimeLeft As Boolean
    Public ShowStageEndWarning As Boolean

    Public Senders As List(Of Player)
    Public Receivers As List(Of Player)

    Public isResettingConnections As Boolean

    Public IsExperimentStarted As Boolean
    Public IsExperimentFinished As Boolean

    'Public statusForm As frmInfoTable

    Public Const NumberOfStages As Integer = 3


#Region " General Functions "
    Public Sub main(ByVal args() As String)
        connectionCount = 0

        AppEventLog_Init()
        appEventLog_Write("Load")

        ToggleScreenSaverActive(False)

        Application.EnableVisualStyles()
        Application.Run(frmServer)

        ToggleScreenSaverActive(True)

        appEventLog_Write("Exit")
        AppEventLog_Close()
    End Sub

    Public Sub takeIP(ByVal sinstr As String, ByVal index As Integer)
        Try
            playerList(index).ipAddress = sinstr
        Catch ex As Exception
            appEventLog_Write("error takeIP:", ex)
        End Try
    End Sub

#End Region

    Public Sub takeMessage(ByVal sinstr As String)
        'when a message is received from a client it is parsed here
        'msgtokens(1) has type of message sent, having different types of messages allows you to send different formats for different actions.
        'msgtokens(2) has the semicolon delimited data that is to be parsed and acted upon.  
        'index has the client ID that sent the data.  Client ID is assigned by connection order, indexed from 1.

        Try
            With frmServer
                Dim msgtokens() As String

                msgtokens = sinstr.Split("|")

                Dim index As Integer
                index = msgtokens(0)

                Application.DoEvents()

                Select Case msgtokens(1) 'case statement to handle each of the different types of messages
                    Case "01"
                        UpdateInstructionDisplay(msgtokens(2), index)
                    Case "02"
                        checkin += 1
                        .dgMainTable.Rows(index - 1).Cells(3).Value = "Waiting"

                        If checkin = gameSetupVars.numberOfPlayers Then
                            gameSetupVars.showInstructions = False
                            checkin = 0

                            MessageBox.Show("Begin Game.", "Start", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            For i As Integer = 1 To gameSetupVars.numberOfPlayers
                                playerList(i).finishedInstructions()
                                .dgMainTable.Rows(i - 1).Cells(3).Value = "Playing"
                            Next i

                            .btnNextStage.Enabled = True

                        End If
                    Case "03"
                        takeIP(msgtokens(2), index)
                    Case "04"
                        TakeGameEnd(msgtokens(2), index)
                    Case "05"
                        TakePlayerData(msgtokens(2), index)
                    Case "06"
                        TakeActionUpdate(msgtokens(2), index)
                    Case "07"
                        InstructionsComplete(index)
                    Case "08"
                        InstructionsPageUpdate(msgtokens(2), index)
                    Case "09"

                    Case "10"

                    Case "11"

                    Case "12"

                    Case "13"

                End Select
                Application.DoEvents()
            End With
            'all subs/functions should have an error trap
        Catch ex As Exception
            appEventLog_Write("error takeMessage: " & sinstr & " : ", ex)
        End Try
    End Sub

#Region "Experiment Flow"

    Public Sub StageComplete()
        Try
            Dim uncheckedSenders As Integer = Aggregate sender In Senders
                                              Where Not sender.isCheckedIn
                                              Into Count()

            Dim uncheckedReceivers As Integer = Aggregate receiver In Receivers
                                                Where Not receiver.isCheckedIn
                                                Into Count()

            Select Case currentStage
                Case 1
                    If uncheckedSenders > 0 Then Exit Sub
                    StageEndWarning()
                    If Not isNoTimeLeft Then Exit Sub
                Case 2
                    If uncheckedReceivers > 0 Then Exit Sub
                    StageEndWarning()
                    If Not isNoTimeLeft Then Exit Sub
                Case 3
                    If uncheckedSenders + uncheckedReceivers > 0 Then Exit Sub
                    StageEndWarning()
                    If Not isNoTimeLeft Then Exit Sub
                    outputPeriodResults()

                    If currentPeriod = gameSetupVars.numberOfPeriods Then
                        If gameSetupVars.isRandomPeriodEarnings Then
                            paymentPeriod = randomGen.Next(1, gameSetupVars.numberOfPeriods)
                        Else
                            paymentPeriod = 0
                        End If
                        IsExperimentFinished = True

                        For Each person In Senders.Concat(Receivers)
                            person.isCheckedIn = False
                            person.endGame()
                        Next

                        frmServer.btnNextStage.Enabled = False
                    End If
            End Select

            If Not IsExperimentFinished Then SetupNextStage()

        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub SetupNextStage()
        Try
            Select Case currentStage
                Case 1
                    Dim sender As Player
                    Dim vSent
                    Dim vMultiplier
                    Dim vReceived

                    For Each receiver In Receivers
                        sender = playerList(receiver.playerData.partnerID)

                        vSent = sender.playerData.Sent
                        receiver.playerData.Sent = vSent

                        vMultiplier = GetPeriodMultiplier(receiver.playerData.groupID)
                        vReceived = (vSent + receiver.playerData.Invested) * vMultiplier

                        receiver.playerData.Multiplier = vMultiplier
                        sender.playerData.Multiplier = vMultiplier

                        sender.playerData.Received = vReceived
                        receiver.playerData.Received = vReceived

                    Next
                    InitializeNextStage()
                Case 2
                    Dim sender As Player
                    Dim vInvested
                    Dim vReceiverKept
                    Dim vSenderGiven

                    For Each receiver In Receivers
                        sender = playerList(receiver.playerData.partnerID)
                        vInvested = receiver.playerData.Invested
                        vReceiverKept = receiver.playerData.ReceiverKept
                        vSenderGiven = receiver.playerData.SenderGiven

                        sender.playerData.Invested = vInvested
                        sender.playerData.ReceiverKept = vReceiverKept
                        sender.playerData.SenderGiven = vSenderGiven

                        sender.playerData.periodEarnings(currentPeriod - 1) = gameSetupVars.senderEndowment - sender.playerData.Sent + vSenderGiven
                        receiver.playerData.periodEarnings(currentPeriod - 1) = gameSetupVars.receiverEndowment + vReceiverKept

                        If currentPeriod = gameSetupVars.numberOfPeriods Then
                            sender.playerData.periodEarnings(currentPeriod - 1) += vInvested / 2
                            receiver.playerData.periodEarnings(currentPeriod - 1) += vInvested / 2
                        End If
                    Next
                    InitializeNextStage()
                Case 3
                    InitializeNextStage()
            End Select

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub StartTime()
        Try
            With frmServer
                Dim allPlayers = Senders.Concat(Receivers)

                For Each player As Player In allPlayers
                    player.isCheckedIn = False
                Next

                ShowStageEndWarning = True
                isNoTimeLeft = False
                'statusForm.PopulateTable()
                If gameSetupVars.showInstructions Then
                    referenceTime = Now.AddMinutes(gameSetupVars.instructionsDuration_Minutes)
                Else
                    referenceTime = Now.AddSeconds(gameSetupVars.stageLengths(currentStage - 1))
                End If
                .WpfProgressBar1.pbarTimeProgress.Maximum = 1000
                .WpfProgressBar1.pbarTimeProgress.Value = 0
                .WpfProgressBar1.pbarTimeProgress.Style = .WpfProgressBar1.FindResource("ProgressBarStyleGreen")
                isProgressBarOrange = False
                .Timer1.Enabled = True
                .lblStage.Text = "Stage: " & currentStage.ToString()
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Function NextStage(Optional ByVal stageOveride As Integer = -1) As Integer
        Try
            If stageOveride > 0 Then
                currentStage = stageOveride
            Else
                If currentStage = NumberOfStages Then
                    currentStage = 1
                    currentPeriod += 1
                Else
                    currentStage += 1
                End If
            End If
            Return currentStage
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Function
#End Region

#Region "Helper Functions"

    Private Sub CheckinPlayer(ByVal index As Integer)
        Try

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Function GetPeriodMultiplier(ByVal ID As Integer) As Integer
        Try
            Return gameSetupVars.multiplierList(ID)(currentPeriod - 1)
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Function
    
    Public Function GetIsSender(ByVal myPlayerType As String) As Boolean
        Try
            If myPlayerType.Equals("Sender") Then
                Return True
            ElseIf myPlayerType.Equals("Receiver") Then
                Return False
            Else
                Return Nothing
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
            Return Nothing
        End Try
    End Function

    Public Sub GetPairings(Optional ByVal rematchRoles As Boolean = False)
        Try
            If rematchRoles Then
                Dim shuffledPlayers = Enumerable.Range(1, gameSetupVars.numberOfPlayers).OrderBy(Function() randomPlayer.Next()).ToList()
                Dim halfThePlayers = shuffledPlayers.Count() / 2

                For i = 0 To shuffledPlayers.Count() - 1
                    If (i + 1) > halfThePlayers Then
                        playerList(shuffledPlayers(i)).playerData.myPlayerType = "Sender"
                        playerList(shuffledPlayers(i)).playerData.myAlias = gameSetupVars.senderAlias
                    Else
                        playerList(shuffledPlayers(i)).playerData.myPlayerType = "Receiver"
                        playerList(shuffledPlayers(i)).playerData.myAlias = gameSetupVars.receiverAlias
                    End If
                Next
            End If

            Senders = (From player In playerList
                      Where Not IsNothing(player.playerData) AndAlso player.playerData.myPlayerType.Equals("Sender")).ToList()

            Receivers = (From player In playerList
                        Where Not IsNothing(player.playerData) AndAlso player.playerData.myPlayerType.Equals("Receiver")
                        Order By randomPlayer.Next()).ToList()

            For i = 0 To Senders.Count() - 1
                'Tell Sender Who their partner is
                Senders(i).playerData.partnerID = Receivers(i).playerData.myID
                Senders(i).playerData.partnerPlayerType = Receivers(i).playerData.myPlayerType
                Senders(i).playerData.partnerAlias = Receivers(i).playerData.myAlias
                Senders(i).playerData.groupID = i
                'Tell Receiver Who their partner is
                Receivers(i).playerData.partnerID = Senders(i).playerData.myID
                Receivers(i).playerData.partnerPlayerType = Senders(i).playerData.myPlayerType
                Receivers(i).playerData.partnerAlias = Senders(i).playerData.myAlias
                Receivers(i).playerData.groupID = i
            Next

            'For i = 1 To gameSetupVars.numberOfPlayers
            '    ResetPlayerData(i)
            'Next

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub ResetPlayerData(ByVal index As Integer)
        Try
            With playerList(index).playerData
                .Sent = -1
                .Multiplier = -1
                .Received = -1
                .Invested = -1
                .ReceiverKept = -1
                .SenderGiven = -1

                .SubmissionTimes = New Integer(NumberOfStages - 1) {}
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
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

#End Region

#Region "Server To Clients"

    Public Sub StartInstructions()
        Try
            For i As Integer = 1 To gameSetupVars.numberOfPlayers
                playerList(i).StartPlayerInstructions()
            Next

            StartTime()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub InitializeNextStage(Optional ByVal stageOveride As Integer = -1)
        Try
            currentStage = NextStage(stageOveride)

            If currentStage = 1 AndAlso gameSetupVars.isRandomRematching Then
                If gameSetupVars.areRolesFixed Then
                    GetPairings()
                Else
                    GetPairings(True)
                End If
            End If

            For i As Integer = 1 To gameSetupVars.numberOfPlayers
                playerList(i).SendPeriod()
                playerList(i).SendPlayerData()
                playerList(i).StartStage()
            Next
            frmServer.lblPeriod.Text = "Period: " & currentPeriod.ToString()
            StartTime()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub StageEndWarning()
        Try
            If ShowStageEndWarning Then
                If timeLeft > gameSetupVars.timeBetweenStages Then
                    For i As Integer = 1 To gameSetupVars.numberOfPlayers
                        playerList(i).nextStage(gameSetupVars.timeBetweenStages)
                    Next
                    referenceTime = Now.AddSeconds(gameSetupVars.timeBetweenStages)
                End If
                ShowStageEndWarning = False
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
#End Region

#Region "Client Initiated Communication"

    Private Sub TakePlayerData(ByVal playerData As String, ByVal index As Integer)
        Try
            If playerList(index).isCheckedIn Then
                playerList(index).SendConfirmation()
                Exit Sub
            End If

            If playerData.Equals("") Then
                playerList(index).isCheckedIn = True
                playerList(index).SendConfirmation()
                StageComplete()
                Exit Sub
            End If

            playerList(index).playerData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of PlayerVars)(playerData)
            playerList(index).isCheckedIn = True
            playerList(index).SendConfirmation()
            'statusForm.PopulateTable()
            StageComplete()

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub TakeActionUpdate(ByVal message As String, ByVal index As Integer)
        Try
            With frmServer
                playerList(index).updateStatusColumn(message)
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub UpdateInstructionDisplay(ByVal sinstr As String, ByVal index As Integer)
        Try
            With frmServer
                Dim msgtokens() As String = sinstr.Split(";")
                Dim nextToken As Integer = 0

                .dgMainTable.Rows(index - 1).Cells(3).Value = "Page " & msgtokens(nextToken)
                nextToken += 1
            End With
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub InstructionsPageUpdate(ByVal inString As String, ByVal index As Integer)
        Try
            Dim pageInfo = inString.Split(";")
            Dim message = "Page " & pageInfo(0) & " of " & pageInfo(1)

            If pageInfo.Length = 3 Then
                message &= "     (Ready)"
            End If
            playerList(index).updateStatusColumn(message)
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub InstructionsComplete(ByVal index As Integer)
        Try
            If playerList(index).isCheckedIn Then
                playerList(index).SendConfirmation()
                Exit Sub
            End If

            playerList(index).isCheckedIn = True
            playerList(index).SendConfirmation()
            playerList(index).updateStatusColumn("Ready")


            Dim notCheckedIn = Receivers.Concat(Senders).Count(Function(x) Not x.isCheckedIn)
            If notCheckedIn > 0 Then Exit Sub

            frmServer.btnBegin_2.Visible = True
            frmServer.btnBegin_2.Enabled = True
            frmServer.btnBegin_1.Visible = False


        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub TakeGameEnd(ByVal playerData As String, ByVal index As Integer)
        Try
            If Not IsExperimentFinished Then Exit Sub

            If playerList(index).playerData.lastName IsNot Nothing Then
                playerList(index).SendConfirmation()
                Exit Sub
            End If

            playerList(index).playerData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of PlayerVars)(playerData)
            CheckinPlayer(index)
            playerList(index).SendConfirmation()
            playerList(index).updateStatusColumn("Finished")
            frmServer.dgMainTable(1, index - 1).Value = playerList(index).playerData.firstName
            frmServer.dgMainTable(2, index - 1).Value = playerList(index).playerData.lastName

            Dim nameless As Integer = Aggregate player In playerList
                                      Where player.playerData IsNot Nothing AndAlso IsNothing(player.playerData.lastName)
                                      Into Count()
            If nameless = 0 Then
                outputEarnings()
                MessageBox.Show("Experiment Complete!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

#End Region

#Region "Output"

    Public Sub writeSummaryData(ByVal strings As String(), ByRef writer As StreamWriter)
        Try
            'Take an array of strings and join them into one large sting separated by commas
            Dim outstr As String = ""
            outstr = strings.Aggregate(outstr, Function(current, stringItem) current & stringItem & ",")

            writer.WriteLine(outstr)
        Catch ex As Exception
            appEventLog_Write("error write summary data:", ex)
        End Try
    End Sub

    Private Sub outputPeriodResults()
        Try
            Dim outputString As New List(Of String)()
            Dim orderedSenders = From person In Senders
                                 Order By person.playerData.myID
                                 Select person

            For Each person As Player In orderedSenders
                With person.playerData
                    outputString.Add(currentPeriod)
                    outputString.Add(.myID)
                    outputString.Add(.SubmissionTimes(0))
                    outputString.Add(.partnerID)
                    outputString.Add(playerList(.partnerID).playerData.SubmissionTimes(1))
                    outputString.Add(.Sent)
                    outputString.Add(.Multiplier)
                    outputString.Add(.Received)
                    outputString.Add(.Invested)
                    outputString.Add(.ReceiverKept)
                    outputString.Add(.SenderGiven)
                End With
                writeSummaryData(outputString.ToArray(), SW_Summary)
                outputString.Clear()
            Next
        Catch ex As Exception
            appEventLog_Write("Error:", ex)
        End Try
    End Sub

    Public Sub outputEarnings()
        Try
            With frmServer
                'write earnings to output file
                Dim earningsFile As String
                Dim outstr As String

                'create unique file name for storing data, CSVs are excel readable, Comma Separted Value files.
                earningsFile = "TrustGame_Earnings_" & .tempTime & ".csv"
                earningsFile = System.Windows.Forms.Application.StartupPath & "\datafiles\earnings\" & earningsFile

                SW_Earnings = File.CreateText(earningsFile)
                SW_Earnings.AutoFlush = True

                outstr = "Player ID,Player Type,First Name,Last Name,Earnings,Capped"
                SW_Earnings.WriteLine(outstr)

                Dim outputString As New List(Of String)

                Dim playersByLastName = From person In playerList
                                        Where Not IsNothing(person.playerData)
                                        Order By person.playerData.lastName

                For Each player As Player In playersByLastName
                    outputString.Clear()
                    With player.playerData
                        outputString.Add(.myID)
                        If gameSetupVars.areRolesFixed Then
                            outputString.Add(.myPlayerType)
                        Else
                            outputString.Add("Random")
                        End If
                        outputString.Add(.firstName)
                        outputString.Add(.lastName)
                        outputString.Add(.finalEarnings.ToString("C"))
                        outputString.Add(.isEarningsCapped)
                    End With
                    writeSummaryData(outputString.ToArray(), SW_Earnings)
                Next

                SW_Earnings.Close()

            End With
        Catch ex As Exception
            appEventLog_Write("Error:", ex)
        End Try
    End Sub

#End Region
    
End Module
