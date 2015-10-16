Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows

Public Class frmInfoTable
    Private ReadOnly _formTitle As String

    Private _bindSenderAcc() As String
    Private _bindReceiverAcc() As String
    Private _bindIncomeStatement() As String
    Private _bindBalanceSheet() As String

    Private _headSenderAcc() As String
    Private _headReceiverAcc() As String
    Private _headIncomeStatement() As String
    Private _headBalanceSheet() As String

    Public Sub New(ByVal title As String)
        Try
            InitializeComponent()
            _formTitle = title
            MakeForm()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub MakeForm()
        Try

            If PlayerScreen.GetIsSender(PlayerData.myPlayerType) Then
                GroupInfoUC1.cReceiverAccount.Width = New GridLength(0)
                GroupInfoUC1.dockReceiverAccount.Visibility = Visibility.Hidden
                GroupInfoUC1.dockIncomeStatement.Visibility = Visibility.Hidden
                GroupInfoUC1.dockBalanceSheet.Visibility = Visibility.Hidden

                GroupInfoUC1.cSenderAccount.Width = New GridLength(400)

                GroupInfoUC1.txbSenderAccount.Text = "Your" & Environment.NewLine & "Account"

                _bindSenderAcc = New String() {"Period", "RemainingEndowment", "SenderGiven", "SenderAccount"}
                _headSenderAcc = New String() {"Period", "You@NewLine@Deposited", "@PartnerPlayerType@@NewLine@Deposited", "Ending@NewLine@Balance"}
                CreateColumns(_bindSenderAcc, _headSenderAcc, GroupInfoUC1.dgSenderAccount)

                If GameSetupVars.isShowIncomeStatement Then
                    GroupInfoUC1.dockIncomeStatement.Visibility = Visibility.Visible
                    GroupInfoUC1.cIncomeStatement.Width = New GridLength(200)
                    _bindIncomeStatement = New String() {"Received"}
                    _headIncomeStatement = New String() {"Multiplied Amount@NewLine@@PartnerPlayerType@ Received"}
                    CreateColumns(_bindIncomeStatement, _headIncomeStatement, GroupInfoUC1.dgIncomeStatement)
                End If

                If GameSetupVars.isShowBalanceSheet Then
                    GroupInfoUC1.dockBalanceSheet.Visibility = Visibility.Visible
                    _bindBalanceSheet = New String() {"Invested"}
                    _headBalanceSheet = New String() {"@PartnerPlayerType@@NewLine@Deposited"}
                    CreateColumns(_bindBalanceSheet, _headBalanceSheet, GroupInfoUC1.dgBalanceSheet)
                End If
            Else
                GroupInfoUC1.cSenderAccount.Width = New GridLength(200)
                GroupInfoUC1.cReceiverAccount.Width = New GridLength(200)
                GroupInfoUC1.cIncomeStatement.Width = New GridLength(500)

                GroupInfoUC1.txbSenderAccount.Text = GameSetupVars.senderAlias & Environment.NewLine & "Account"
                GroupInfoUC1.txbReceiverAccount.Text = "Your" & Environment.NewLine & "Account"

                _bindSenderAcc = New String() {"Period", "SenderGiven"}
                _headSenderAcc = New String() {"Period", "You@NewLine@Deposited"}

                _bindReceiverAcc = New String() {"ReceiverKept", "ReceiverAccount"}
                _headReceiverAcc = New String() {"You@NewLine@Deposited", "Ending@NewLine@Balance"}

                _bindIncomeStatement = New String() {"InvestedLastTime", "Sent", "Multiplier", "Received"}
                _headIncomeStatement = New String() {"Last Common@NewLine@Acct. Bal.", "@PartnerPlayerType@@NewLine@Deposited", "Multiplier", "Received"}

                _bindBalanceSheet = New String() {"Invested"}
                _headBalanceSheet = New String() {"You@NewLine@Deposited"}

                With GroupInfoUC1
                    CreateColumns(_bindSenderAcc, _headSenderAcc, .dgSenderAccount)
                    CreateColumns(_bindReceiverAcc, _headReceiverAcc, .dgReceiverAccount)
                    CreateColumns(_bindIncomeStatement, _headIncomeStatement, .dgIncomeStatement)
                    CreateColumns(_bindBalanceSheet, _headBalanceSheet, .dgBalanceSheet)
                End With
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Shared Sub CreateColumns(ByVal bindings As String(), ByVal header As String(), ByRef dataGrid As DataGrid)
        Try
            Dim column As DataGridTextColumn
            For i As Integer = 0 To bindings.Length - 1
                column = New DataGridTextColumn()
                column.Binding = New Binding(bindings(i))
                column.Header = PlayerScreen.ReplaceTextVariables(header(i))
                dataGrid.Columns.Add(column)
            Next

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub PopulateTable()
        Try
            Dim historyItem = New HistoricalData()

            With historyItem
                .Period = CurrentPeriod
                .RemainingEndowment = GameSetupVars.senderEndowment - PlayerData.Sent
                .Sent = PlayerData.Sent
                .Multiplier = PlayerData.Multiplier
                .Received = PlayerData.Received
                .ReceiverKept = PlayerData.ReceiverKept
                .SenderGiven = PlayerData.SenderGiven
                .Invested = PlayerData.Invested
                If CurrentPeriod = 1 Then
                    .InvestedLastTime = 0
                Else
                    .InvestedLastTime = Math.Round(.Received / .Multiplier, 2) - .Sent
                End If
                .SenderAccount = PlayerData.periodEarnings.Sum()
                .ReceiverAccount = PlayerData.periodEarnings.Sum()
            End With

            If PlayerScreen.GetIsSender(PlayerData.myPlayerType) Then
                GroupInfoUC1.dgSenderAccount.Items.Add(historyItem)
                If GameSetupVars.isShowIncomeStatement Then GroupInfoUC1.dgIncomeStatement.Items.Add(historyItem)
                If GameSetupVars.isShowBalanceSheet Then GroupInfoUC1.dgBalanceSheet.Items.Add(historyItem)
            Else
                GroupInfoUC1.dgSenderAccount.Items.Add(historyItem)
                GroupInfoUC1.dgReceiverAccount.Items.Add(historyItem)
                GroupInfoUC1.dgIncomeStatement.Items.Add(historyItem)
                GroupInfoUC1.dgBalanceSheet.Items.Add(historyItem)
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Class HistoricalData
        Public Property Period As Integer

        Public Property RemainingEndowment As Double
        Public Property Sent As Double

        Public Property Multiplier As Integer
        Public Property Received As Double
        Public Property ReceiverKept As Double
        Public Property SenderGiven As Double
        Public Property Invested As Double

        Public Property InvestedLastTime As Double

        Public Property SenderAccount As Double
        Public Property ReceiverAccount As Double
    End Class

End Class