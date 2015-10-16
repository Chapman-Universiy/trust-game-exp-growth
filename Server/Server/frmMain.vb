Imports System
Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.IO
Imports Newtonsoft.Json
Imports System.Text

Public Class frmMain
#Region " Winsock Code "
    Public WithEvents wsk_Col As New WinsockCollection
    Private _users As New UserCollection
    Public WithEvents wskListener As Winsock

    Private Sub wskListener_ConnectionRequest(ByVal sender As Object, ByVal e As WinsockClientReceivedEventArgs) Handles wskListener.ConnectionRequest
        Try
            connectionCount += 1

            'Log("Connection received from: " & e.ClientIP)
            Dim y As New clsUser
            Dim i As Integer
            Dim ID As String = connectionCount.ToString()



            _users.Add(y)
            Dim x As New Winsock(Me)
            wsk_Col.Add(x, ID)
            x.Accept(e.Client)

            If IsExperimentStarted Then
                For i = 1 To playerCount
                    If playerList(i).myIPAddress = e.ClientIP Then
                        playerList(i).socketNumber = ID 'wsk_Col.Count - 1
                        Exit For
                    End If
                Next
                Exit Sub
            End If

            playerCount += 1
            playerList.Add(New Player())
            playerList(playerCount).playerData = New PlayerVars()
            playerList(playerCount).playerData.myID = playerCount
            playerList(playerCount).socketNumber = ID 'wsk_Col.Count - 1
            playerList(playerCount).myIPAddress = e.ClientIP

            playerList(playerCount).requsetIP(playerCount)

            lblConnections.Text = wsk_Col.Count.ToString()

            'appEventLog_Write("connection request: " & e.ClientIP)
        Catch ex As Exception
            appEventLog_Write("error wskListener_ConnectionRequest:", ex)
        End Try
    End Sub

    Private Sub wskListener_ErrorReceived(ByVal sender As System.Object, ByVal e As WinsockErrorEventArgs) Handles wskListener.ErrorReceived
        Try
            appEventLog_Write("winsock error: " & e.Message)
        Catch ex As Exception
            appEventLog_Write("error wskListener_ErrorReceived:", ex)
        End Try
    End Sub

    Private Sub wskListener_StateChanged(ByVal sender As Object, ByVal e As WinsockStateChangingEventArgs) Handles wskListener.StateChanged
        'Log("Listener state changed from " & e.Old_State.ToString & " to " & e.New_State.ToString)
        'lblListenState.Text = "State: " & e.New_State.ToString
        'cmdListen.Enabled = False
        'cmdClose.Enabled = False
        'Select Case e.New_State
        '    Case WinsockStates.Closed
        '        cmdListen.Enabled = True
        '    Case WinsockStates.Listening
        '        cmdClose.Enabled = True
        'End Select
    End Sub

    'Private Sub Log(ByVal val As String)
    '    lstLog.SelectedIndex = lstLog.Items.Add(val)
    '    lstLog.SelectedIndex = -1
    'End Sub

    Private Sub Wsk_DataArrival(ByVal sender As Object, ByVal e As WinsockDataArrivalEventArgs) Handles wsk_Col.DataArrival
        Try
            Dim sender_key As String = wsk_Col.GetKey(sender)
            Dim buf As String = Nothing
            CType(sender, Winsock).Get(buf)

            Dim msgtokens() As String = buf.Split("#")
            Dim i As Integer

            'appEventLog_Write("data arrival: " & buf)

            For i = 1 To msgtokens.Length - 1
                takeMessage(msgtokens(i - 1))
            Next

        Catch ex As Exception
            appEventLog_Write("error Wsk_DataArrival:", ex)
        End Try
    End Sub

    Private Sub Wsk_Disconnected(ByVal sender As Object, ByVal e As System.EventArgs) Handles wsk_Col.Disconnected
        Try
            wsk_Col.Remove(sender)
            If Not isResettingConnections Then
                MsgBox("A client has been disconnected.", MsgBoxStyle.Critical)
                appEventLog_Write("client disconnected")
            End If
            lblConnections.Text = wsk_Col.Count.ToString()
        Catch ex As Exception
            appEventLog_Write("error Wsk_Disconnected:", ex)
        End Try
    End Sub
    Private Sub Wsk_Connected(ByVal sender As Object, ByVal e As System.EventArgs) Handles wsk_Col.Connected
        'lblConNum.Text = "Connected: " & wsk_Col.Count
    End Sub

    Private Sub ShutDownServer()
        Try
            GC.Collect()
        Catch ex As Exception
            appEventLog_Write("error ShutDownServer:", ex)
        End Try

    End Sub
#End Region    'communication code

#Region " Extra Functions "
    Public Function convertY(ByVal p As Integer, ByVal graphMin As Integer, ByVal graphMax As Integer, _
                                 ByVal panelHeight As Integer, ByVal bottomOffset As Integer, ByVal topOffset As Integer) As Double
        Try
            Dim tempD As Double

            tempD = p - graphMin

            tempD = (tempD * (panelHeight - bottomOffset - topOffset)) / (graphMax - graphMin)
            tempD = panelHeight - (bottomOffset + topOffset) - tempD

            convertY = tempD + topOffset
        Catch ex As Exception
            Return 0
            appEventLog_Write("error convertY:", ex)
        End Try
    End Function

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Try
            Dim i As Integer
            Dim f As New Font("Arial", 8, FontStyle.Bold)
            Dim tempN As Integer
            Dim horizontalCoordinates = New Integer() {10, 200, 400}


            e.Graphics.DrawString(filename2, f, Brushes.Black, 10, 10)

            f = New Font("Arial", 15, FontStyle.Bold)

            e.Graphics.DrawString("Last Name", f, Brushes.Black, horizontalCoordinates(0), 30)
            e.Graphics.DrawString("First Name", f, Brushes.Black, horizontalCoordinates(1), 30)
            e.Graphics.DrawString("Earnings", f, Brushes.Black, horizontalCoordinates(2), 30)


            f = New Font("Arial", 12, FontStyle.Bold)

            tempN = 55

            For i = 1 To dgMainTable.RowCount
                If i Mod 2 = 0 Then
                    e.Graphics.FillRectangle(Brushes.Aqua, 0, tempN, 500, 19)
                End If
                e.Graphics.DrawString(dgMainTable.Rows(i - 1).Cells(2).Value, f, Brushes.Black, horizontalCoordinates(0), tempN)
                e.Graphics.DrawString(dgMainTable.Rows(i - 1).Cells(1).Value, f, Brushes.Black, horizontalCoordinates(1), tempN)
                e.Graphics.DrawString(dgMainTable.Rows(i - 1).Cells(4).Value, f, Brushes.Black, horizontalCoordinates(2), tempN)

                tempN += 20
            Next

        Catch ex As Exception
            appEventLog_Write("error PrintDocument1_PrintPage:", ex)
        End Try

    End Sub
#End Region

    Public tempTime As String 'time stamp at start of experiment

    Private Sub frmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            playerList = New List(Of Player)()
            playerList.Add(New Player())

            IsExperimentStarted = False

            wskListener = New Winsock
            wskListener.BufferSize = 8192
            wskListener.LegacySupport = False
            wskListener.LocalPort = 12345
            wskListener.MaxPendingConnections = 1
            wskListener.Protocol = WinsockProtocols.Tcp
            wskListener.RemotePort = 8080
            wskListener.RemoteServer = "localhost"
            wskListener.SynchronizingObject = Me

            wskListener.Listen()

            playerCount = 0

            lblIP.Text = wskListener.LocalIP()
            lblLocalHost.Text = SystemInformation.ComputerName
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            Dim timeElapsed As Double
            timeLeft = referenceTime.Subtract(Now).TotalSeconds

            If timeLeft <= 0 Then
                timeLeft = 0
                If gameSetupVars.showInstructions Then
                    lblTimeRemaining.Text = "Time Remaining: " & modMain.timeConversion(Math.Ceiling(timeLeft)) & " Minutes"
                Else
                    lblTimeRemaining.Text = "Time Remaining: " & Math.Ceiling(timeLeft) & " Seconds"
                End If
                WpfProgressBar1.pbarTimeProgress.Value = 1000
                WpfProgressBar1.pbarTimeProgress.Style = WpfProgressBar1.FindResource("ProgressBarStyleRed")
                Timer1.Enabled = False
                isNoTimeLeft = True
                StageComplete()
            Else
                If gameSetupVars.showInstructions Then
                    timeElapsed = gameSetupVars.instructionsDuration_Minutes * 60 - timeLeft
                    lblTimeRemaining.Text = "Time Remaining: " & modMain.timeConversion(Math.Ceiling(timeLeft)) & " Minutes"
                    WpfProgressBar1.pbarTimeProgress.Value = Math.Round(timeElapsed / (gameSetupVars.instructionsDuration_Minutes * 60), 4) * 1000

                    If Not isProgressBarOrange AndAlso timeLeft < gameSetupVars.instructionsWarning_MinutesLeft * 60 Then
                        WpfProgressBar1.pbarTimeProgress.Style = WpfProgressBar1.FindResource("ProgressBarStyleOrange")
                        isProgressBarOrange = True
                    End If
                Else
                    timeElapsed = gameSetupVars.stageLengths(currentStage - 1) - timeLeft
                    lblTimeRemaining.Text = "Time Remaining: " & Math.Ceiling(timeLeft) & " Seconds"
                    WpfProgressBar1.pbarTimeProgress.Value = Math.Round(timeElapsed / gameSetupVars.stageLengths(currentStage - 1), 4) * 1000

                    If Not isProgressBarOrange AndAlso timeLeft < gameSetupVars.warningTimes(currentStage - 1) Then
                        WpfProgressBar1.pbarTimeProgress.Style = WpfProgressBar1.FindResource("ProgressBarStyleOrange")
                        isProgressBarOrange = True
                    End If
                End If
            End If
        Catch ex As Exception
            appEventLog_Write("error Timer1_Tick:", ex)
        End Try
    End Sub

    Private Sub btnBegin_2_Click(sender As System.Object, e As System.EventArgs) Handles btnBegin_2.Click
        Try
            Dim result = MessageBox.Show("Start the experiment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                gameSetupVars.showInstructions = False
                InitializeNextStage()
                btnBegin_2.Hide()
                btnBegin_1.Show()
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub cmdBegin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBegin_1.Click
        Try
            'when a button is pressed it's click event is fired
            Dim nextToken As Integer = 0
            Dim str As String

            If CInt(lblConnections.Text) <> gameSetupVars.numberOfPlayers Then
                MsgBox("Incorrect number of connections.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'define timestamp for recording data
            tempTime = DateTime.Now.Month & "-" & DateTime.Now.Day & "-" & DateTime.Now.Year & "_" & DateTime.Now.Hour & _
                     "_" & DateTime.Now.Minute & "_" & DateTime.Now.Second

            'create unique file name for storing data, CSVs are excel readable, Comma Separted Value files.
            filename = "TrustGame_Data_" & tempTime & ".csv"
            filename = System.Windows.Forms.Application.StartupPath & "\datafiles\data\" & filename

            SW_Summary = File.CreateText(filename)
            SW_Summary.AutoFlush = True
            str = "Period,Sender ID,Sender Decision Length,Receiver ID,Receiver Decision Length,Sent,Multiplier,Received,Invested,ReceiverKept,SenderGiven"
            
            SW_Summary.WriteLine(str)

            dgMainTable.RowCount = gameSetupVars.numberOfPlayers


            'setup for display results
            'setup player type
            For i As Integer = 1 To gameSetupVars.numberOfPlayers
                dgMainTable.Rows(i - 1).Cells(0).Value = i

                dgMainTable.Rows(i - 1).Cells(0).Value = i
                dgMainTable.Rows(i - 1).Cells(1).Value = playerList(i).myIPAddress
                If gameSetupVars.showInstructions Then
                    dgMainTable.Rows(i - 1).Cells(3).Value = "Page 1"
                Else
                    dgMainTable.Rows(i - 1).Cells(3).Value = "Playing"
                End If
                dgMainTable.Rows(i - 1).Cells(4).Value = "0"
            Next

            currentPeriod = 1
            currentStage = 0
            lblPeriod.Text = "Period: " & currentPeriod.ToString()
            checkin = 0

            'disable/enable buttons needed when the experiment starts
            btnSetup.Enabled = False
            btnExit.Enabled = False
            btnBegin_1.Enabled = False
            btnEnd.Enabled = True
            isResettingConnections = False
            frmServer.btnPause.Enabled = True
            frmServer.btnNextStage.Enabled = True

            IsExperimentStarted = True
            IsExperimentFinished = False

            filename2 = filename

            'Place the players into groups
            randomGen = New Random(dateSeed())
            randomPlayer = New Random(10)
            randomMultiplier = New Random(777)

            GetPairings(True)

            'signal clients to begin
            For i As Integer = 1 To gameSetupVars.numberOfPlayers
                playerList(i).begin()
            Next

            If gameSetupVars.showInstructions Then
                StartInstructions()
            Else
                InitializeNextStage()
            End If

        Catch ex As Exception
            appEventLog_Write("error cmdBegin_Click:", ex)
        End Try

    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            'when reset is pressed bring server back to state to start another experiment

            'disable timers
            Timer1.Enabled = False
            Timer2.Enabled = False
            Timer3.Enabled = False

            'close data files
            If SW_Summary IsNot Nothing Then SW_Summary.Close()
            isResettingConnections = True
            'shut down clients
            Dim i As Integer
            For i = 1 To CInt(lblConnections.Text)
                playerList(i).resetClient()
            Next

            playerList.Clear()
            playerList.Add(New Player())


            'enable/disable buttons
            btnSetup.Enabled = True
            btnBegin_1.Visible = True
            btnBegin_1.Enabled = False
            btnExit.Enabled = True
            btnEnd.Enabled = False
            btnNextStage.Enabled = False
            btnPause.Enabled = False
            btnLoad.Enabled = True
            btnBegin_2.Visible = False

            IsExperimentStarted = False
            IsExperimentFinished = False

            lblLoadedFile.Text = "Please Load a parameter file."
            lblLoadedFile.ForeColor = Color.DarkRed

            playerCount = 0

            WpfProgressBar1.pbarTimeProgress.Value = 0
            WpfProgressBar1.pbarTimeProgress.Style = WpfProgressBar1.FindResource("ProgressBarStyleGreen")

            dgMainTable.RowCount = 0

            frmInstructions.Close()

        Catch ex As Exception
            appEventLog_Write("error cmdReset_Click:", ex)
        End Try
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Try
            'exit program

            Timer1.Enabled = False
            ShutDownServer()

            Me.Close()
        Catch ex As Exception
            appEventLog_Write("error cmdExit_Click:", ex)
        End Try
    End Sub

    Private Sub cmdGameSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        Try
            frmSetup.Show()
        Catch ex As Exception
            appEventLog_Write("error cmdGameSetup_Click:", ex)
        End Try
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Try

        Catch ex As Exception
            appEventLog_Write("error Timer2_Tick:", ex)
        End Try
    End Sub


    Private Sub cmdEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnd.Click
        Try
            'end experiment early

            Dim i As Integer
            btnEnd.Enabled = False

            gameSetupVars.numberOfPeriods = currentPeriod

            For i = 1 To gameSetupVars.numberOfPlayers
                playerList(i).endEarly()
            Next
        Catch ex As Exception
            appEventLog_Write("error cmdEnd_Click:", ex)
        End Try
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Try

        Catch ex As Exception
            appEventLog_Write("error timer3 tick:", ex)
        End Try
    End Sub

    Private Sub llESI_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llESI.LinkClicked
        Try
            System.Diagnostics.Process.Start("http://www.chapman.edu/esi/")
        Catch ex As Exception
            appEventLog_Write("error :", ex)
        End Try
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        dgMainTable.Sort(dgMainTable.Columns(1), ListSortDirection.Ascending)
        dgMainTable.Sort(dgMainTable.Columns(2), ListSortDirection.Ascending)

        Try
            If PrintDialog1.ShowDialog = DialogResult.OK Then
                PrintDocument1.Print()
            End If

        Catch ex As Exception
            appEventLog_Write("error cmdPrint_Click:", ex)
        End Try
    End Sub

    Private Sub dgMainTable_ColumnHeaderMouseDoubleClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgMainTable.ColumnHeaderMouseDoubleClick
        dgMainTable.Sort(dgMainTable.Columns(e.ColumnIndex), ListSortDirection.Ascending)
    End Sub

    Private Function dateSeed()
        Dim referenceDate As Date = "11/24/2001"
        Dim timeSpan As TimeSpan
        Dim numberOfSeconds As Integer

        timeSpan = Now.Subtract(referenceDate)
        numberOfSeconds = timeSpan.Seconds

        Return numberOfSeconds
    End Function

    Private Sub btnNextStage_Click(sender As System.Object, e As System.EventArgs) Handles btnNextStage.Click
        Try
            For i As Integer = 1 To gameSetupVars.numberOfPlayers
                playerList(i).nextStage()
            Next
            referenceTime = Now.AddSeconds(3)
        Catch ex As Exception
            appEventLog_Write("Error:", ex)
        End Try
    End Sub


    Private Sub cmdPause_Click(sender As Button, e As System.EventArgs) Handles btnPause.Click
        Try
            Dim result = MessageBox.Show(btnPause.Text, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)

            If result = DialogResult.Cancel Then
                Exit Sub
            Else
                If isPaused Then
                    btnPause.Enabled = False
                    For i As Integer = 1 To gameSetupVars.numberOfPlayers
                        playerList(i).Pause("The experiment has been resumed.")
                    Next
                    sender.Text = "        Pause"
                    referenceTime = Now.AddSeconds(timeLeft)
                    Timer1.Start()
                    isPaused = False
                    btnPause.Enabled = True
                Else
                    Timer1.Stop()
                    btnPause.Enabled = False
                    For i As Integer = 1 To gameSetupVars.numberOfPlayers
                        playerList(i).Pause("The experiment has been paused.")
                    Next
                    sender.Text = "        Resume"
                    isPaused = True
                    btnPause.Enabled = True
                End If
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub btnLoad_Click(sender As System.Object, e As System.EventArgs) Handles btnLoad.Click
        Try
            Dim filenameJSON As String

            'dispaly open file dialog to select file
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Filter = "Parameter Files (*.json)|*.json"
            OpenFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath & "\ParameterFiles"

            OpenFileDialog1.ShowDialog()

            'if filename is not empty then continue with load
            If OpenFileDialog1.FileName = "" Then
                lblLoadedFile.Text = "Please Load a parameter file."
                lblLoadedFile.ForeColor = Color.DarkRed
                Exit Sub
            End If

            'close data files
            If SW_Summary IsNot Nothing Then SW_Summary.Close()

            WpfProgressBar1.pbarTimeProgress.Value = 0
            WpfProgressBar1.pbarTimeProgress.Style = WpfProgressBar1.FindResource("ProgressBarStyleGreen")

            filenameJSON = OpenFileDialog1.FileName

            Dim reader As StreamReader
            reader = New StreamReader(filenameJSON)
            gameSetupVars = JsonConvert.DeserializeObject(Of EnvironmentVars)(reader.ReadToEnd())
            reader.Close()

            lblLoadedFile.Text = filenameJSON.Replace(OpenFileDialog1.InitialDirectory.ToString() & "\", "")
            lblLoadedFile.ForeColor = Color.DarkGreen

            btnBegin_1.Enabled = True
            btnLoad.Enabled = False

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
    
    Public Sub frmMain_Move(sender As System.Object, e As System.EventArgs) Handles MyBase.Move
        Try
            'If IsNothing(statusForm) Then Exit Sub

            'Const gapBetweenForms As Integer = 5
            'Dim mainBounds = Bounds
            'Dim sideFormWidth As Integer
            'Dim formHeight As Integer
            'Dim winformHeader = RectangleToScreen(ClientRectangle).Top - mainBounds.Top

            'If gameSetupVars.isDisputeTreatment Then
            '    sideFormWidth = 1000
            'Else
            '    sideFormWidth = 850
            'End If

            'With statusForm.GroupInfoUC1
            '    formHeight = Math.Min(.lblTitle.ActualHeight + .dgGroupInfo.ActualHeight + .dgGroupInfo.Margin.Top + 15, _
            '                                       dgMainTable.Height)
            'End With

            'Dim newBounds = New Rectangle(mainBounds.Right + gapBetweenForms, mainBounds.Top + dgMainTable.Location.Y + winformHeader, sideFormWidth, formHeight)
            'statusForm.Bounds = newBounds

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
    
End Class
