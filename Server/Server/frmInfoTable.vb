Imports System.Windows.Controls
Imports System.Windows.Data

Public Class frmInfoTable
    Private ReadOnly _formTitle As String

    Public Sub New(ByVal title As String)
        Try
            InitializeComponent()
            _formTitle = title
            GroupInfoUC1.LayoutRoot.Background = GroupInfoUC1.FindResource("GreyGradientFixed")
            MakeForm()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub MakeForm()
        Try
            GroupInfoUC1.lblTitle.Content = _formTitle
            Dim bindingList() As String
            Dim headerList() As String

            bindingList = New String() {"myID", "myColor", "partnerID", "partnerColor", "messagePercentText", "amountSent", "amountReceived", "messageValueText", _
                                        "amountReturned", "askedForDisputeText", "disputeOutcomeText"}
            headerList = New String() {"Receiver" & Environment.NewLine & "ID", "Receiver" & Environment.NewLine & "Color", "Sender" & Environment.NewLine & "ID", _
                                       "Sender" & Environment.NewLine & "Color", "Message" & Environment.NewLine & "Percent", "Sender" & Environment.NewLine & "Sent", _
                                       "Receiver" & Environment.NewLine & "Received", "Message" & Environment.NewLine & "Amount", _
                                       "Receiver" & Environment.NewLine & "Returned", "Dispute" & Environment.NewLine & "Filed", "Dispute" & Environment.NewLine & "Outcome"}

            CreateColumns(bindingList, headerList)
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub CreateColumns(ByVal bindingList As String(), ByVal headerList As String())
        Try
            With GroupInfoUC1.dgGroupInfo
                Dim column As DataGridTextColumn
                For i As Integer = 0 To bindingList.Length - 1
                    column = New DataGridTextColumn()
                    column.Binding = New Binding(bindingList(i))
                    column.Header = headerList(i)

                    .Columns.Add(column)
                Next
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Public Sub PopulateTable()
    '    Try
    '        GroupInfoUC1.dgGroupInfo.Items.Clear()

    '        Dim playerQuery = From player In playerList
    '                          Where Not IsNothing(player.playerData) AndAlso player.playerData.myPlayerType.Equals("Receiver")
    '                          Order By player.playerData.myColor, player.playerData.myID
    '                          Select player.playerData

    '        For Each playerData In playerQuery
    '            GroupInfoUC1.dgGroupInfo.Items.Add(playerData)
    '        Next
    '    Catch ex As Exception
    '        appEventLog_Write("Error: ", ex)
    '    End Try
    'End Sub
End Class