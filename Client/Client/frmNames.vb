Public Class frmNames

    Dim _TestNames() As String
    Dim refTime As DateTime

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Try
            'submit name at end of experiment

            If txtFirstName.Text = "" Or txtLastName.Text = "" Then
                Exit Sub
            End If

            playerData.firstName = txtFirstName.Text
            playerData.lastName = txtLastName.Text

            GameEnd()

        Catch ex As Exception
            appEventLog_Write("error cmdSubmit_Click:", ex)
        End Try
    End Sub

    Private Sub frmNames_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            If gameSetupVars.isTestMode Then
                Dim randomN As New Random(playerData.myID)
                refTime = Now.AddSeconds(randomN.Next(1, 7))
                timeNames.Enabled = True

                _TestNames = New String() {"Nita,Steigerwald", "Jami,Sabatino", "Max,Honn", "Fernando,Valenzula", "Nelson,Bady", "Emilia,Moreman", "Mathew,Rishel", _
                                           "Clare,Marlar", "Marcie,Gushiken", "Ted,Gowin", "Kenya,Calley", "Althea,Kretschmer", "Odessa,Laferriere", "Neil,Blatter", _
                                           "Javier,Loreto", "Kurt,Dibenedetto", "Alana,Maske", "Javier,Landor", "Alana,Pitzen", "Jessie,Clowers", "Alejandra,Creasy", _
                                           "Jessie,Hege", "Marcie,Bagg", "Darren,Sobieski", "Louisa,Moffet", "Marcie,Terrio", "Lonnie,Selke", "Edwina,Anker", "Jessie,Kyer", _
                                           "Carey,Sabal", "Valene,Cerrito", "Jeffry,Chudy", "Jayna,Raspberry", "Quinton,Castelluccio", "Lanie,Colten", "Raphael,Eschief", _
                                           "Eufemia,Mccullick", "Genna,Hatke", "Garret,Borromeo", "Gaylord,Filteau", "Rikki,Hannem", "Herb,Karageorge", "Ha,Mathony", _
                                           "Marcellus,Baumgartel", "Garret,Gautsch", "Renaldo,Alambar", "Santana,Norkaitis", "Soo,Dubbin", "Christopher,Harter", _
                                           "Kara,Bragg", "Matthew,Bierman", "Wayne,Perales", "Ralph,Andino", "Cathy,Grajeda", "Mark,Brinkley", "Stanley,Calabrese", _
                                           "Patty,Frazier", "Nina,Marriott", "Tony,Haire", "Jeffrey,Bard", "Kenneth,Lester", "Philip,Mcgowan", "Phyllis,Dandridge", _
                                           "Louis,Webb", "Judith,Richter", "Olivia,Magruder", "Marvin,June", "Bruce,Bono", "Walter,Chapman", "Jack,Cribb", _
                                           "Antonio,Vandyke", "Flora,Moreland", "Kay,Davie", "Daniel,Dayton", "Martin,Balderas", "Timothy,Farrar"}

                Dim firstName = _TestNames(randomN.Next(0, _TestNames.Length - 1)).Split(",")(0)
                Dim lastName = _TestNames(randomN.Next(0, _TestNames.Length - 1)).Split(",")(1)

                txtFirstName.Text = firstName
                txtLastName.Text = lastName
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub timeNames_Tick(sender As System.Object, e As System.EventArgs) Handles timeNames.Tick
        Try
            If refTime.Subtract(Now).TotalSeconds < 0 Then
                timeNames.Enabled = False
                cmdSubmit.PerformClick()
            End If
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
End Class