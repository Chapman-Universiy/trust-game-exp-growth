Imports System.IO
Imports System.Windows.Documents
Imports Newtonsoft.Json

Public Class frmSetup

    Private Sub btnOpenFile_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenFile.Click
        Try
            Dim filenameJSON As String

            'dispaly open file dialog to select file
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Filter = "Parameter Files (*.json)|*.json"
            OpenFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath & "\ParameterFiles"

            OpenFileDialog1.ShowDialog()

            'if filename is not empty then continue with load
            If OpenFileDialog1.FileName = "" Then
                lblCurrentFile.Text = "No File Loaded"
                Exit Sub
            End If

            filenameJSON = OpenFileDialog1.FileName

            Dim reader As StreamReader
            reader = New StreamReader(filenameJSON)

            rtxtEditor.Text = reader.ReadToEnd()
            
            lblCurrentFile.Text = filenameJSON

            reader.Close()

        Catch ex As Exception
            appEventLog_Write("error cmdLoad_Click:", ex)
        End Try
    End Sub

    Private Sub btnSaveFile_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveFile.Click
        Try
            Const groupSize = 2

            gameSetupVars = JsonConvert.DeserializeObject(Of EnvironmentVars)(rtxtEditor.Text)
            
            If Not gameSetupVars.numberOfPlayers Mod groupSize = 0 Then
                MessageBox.Show("The number of players needs to be a multiple of " & groupSize.ToString() & ".", "Parameter Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            SaveFileDialog1.FileName = ""
            SaveFileDialog1.Filter = "Parameter Files (*.json)|*.json"
            SaveFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath & "\ParameterFiles"
            SaveFileDialog1.ShowDialog()

            If SaveFileDialog1.FileName = "" Then
                Exit Sub
            End If

            Dim writer As StreamWriter
            writer = File.CreateText(SaveFileDialog1.FileName)
            writer.Write(JsonConvert.SerializeObject(gameSetupVars, Formatting.Indented))
            writer.Close()

            Me.Close()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
            MessageBox.Show("There was an error with the file format." & Environment.NewLine & Environment.NewLine & "The file was not saved.", _
                            "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.Close()
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub btnNewFile_Click(sender As System.Object, e As System.EventArgs) Handles btnNewFile.Click
        Try
            SaveFileDialog1.FileName = ""
            SaveFileDialog1.Filter = "Parameter Files (*.json)|*.json"
            SaveFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath & "\ParameterFiles"
            SaveFileDialog1.ShowDialog()

            If SaveFileDialog1.FileName = "" Then
                Exit Sub
            End If

            Dim g As New EnvironmentVars

            Dim writer As StreamWriter
            writer = File.CreateText(SaveFileDialog1.FileName)
            writer.Write(JsonConvert.SerializeObject(g, Formatting.Indented))
            writer.Close()

            Dim reader As StreamReader
            reader = New StreamReader(SaveFileDialog1.FileName)
            
            rtxtEditor.Text = reader.ReadToEnd()

            lblCurrentFile.Text = SaveFileDialog1.FileName

            reader.Close()

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
    
End Class