Imports System.Text
Imports System.IO
Imports System.Windows
Imports System.Windows.Documents
Imports System.Windows.Controls

'The ScreenManager class is completely responsible for what is shown on the screen at
'any given time. During an experiment, the only method that the game code calls is
'the SetupScreenText() method, which then calls the correct methods given the 
'CurrentStage variable.
Public Class ScreenManager
    Private instructionFileBase As String
    Public fileDictionary As Dictionary(Of String, Byte())
    Private encoder As ASCIIEncoding
    Const testVar = "Test Successful"

    
    'Call this to update the screen with given the CurrentStage of the experiment.
    Public Sub SetupScreenText()
        Try
            frmMain.MainControl1.txtMain_Message.Text = ""
            Select Case CurrentStage
                Case 1
                    S1Screen()
                Case 2
                    S2Screen()
                Case 3
                    S3Screen()
            End Select
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'The methods in the instructions region are used only if the showInstructions is set to
    'true in the configuration file.
#Region "Instruction"
    Public Sub InstructionsScreen()
        Try
            With frmMain.MainControl1
                CurrentInstructionPage = 1

                IsDoneWithInstructions = False

                .btnMainLeft.IsEnabled = True
                .btnMainLeft.Visibility = Visibility.Hidden
                .btnMainLeft.Content = "Previous"

                .btnMainRight.Visibility = Visibility.Visible
                .btnMainRight.IsEnabled = True
                .btnMainRight.Content = "Next"

                .btnMainCenter.IsEnabled = True
                .btnMainCenter.Visibility = Visibility.Hidden
                .btnMainCenter.Content = "Ready"

                .lblMain_Left2.Visibility = Visibility.Hidden
                .lblMain_Right2.Visibility = Visibility.Hidden
                .txtMain_Input2.Visibility = Visibility.Hidden

                .lblPeriod.Content = "Page: 1 of " & InstructionsTotalPages.ToString()

                UpdateInstructions()

            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub UpdateInstructions()
        Try
            With frmMain.MainControl1
                Dim isSender = GetIsSender(PlayerData.myPlayerType)
                Dim fileCode = instructionFileBase & CurrentInstructionPage.ToString()

                SetTextFromFileCode(fileCode)
                UpdateInstructionButtons()
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub UpdateInstructionButtons()
        Try
            With frmMain.MainControl1

                .btnMainLeft.Visibility = Visibility.Visible
                .btnMainRight.Visibility = Visibility.Visible
                .btnMainCenter.Visibility = Visibility.Hidden

                If CurrentInstructionPage = InstructionsTotalPages Then
                    .btnMainRight.Visibility = Visibility.Hidden
                    If Not IsDoneWithInstructions Then
                        .btnMainCenter.Visibility = Visibility.Visible
                    End If
                ElseIf CurrentInstructionPage = 1 Then
                    .btnMainLeft.Visibility = Visibility.Hidden
                End If

                .lblPeriod.Content = "Page: " & CurrentInstructionPage.ToString() & " of " & InstructionsTotalPages.ToString()
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
#End Region

    'This method is used to show/hide the controls on the users screen. Pass in three arrays:
    '   Array 1: The text of the [right, center, left] buttons
    '   Array 2: The text of the [top, middle, bottom] left labels
    '   Array 3: The text of the [top, middle, bottom] right labels
    'If you do not want a certain button, label, or text box to show, then pass in an empty string
    'as the text for that element. The textboxes need a left label in order to show (so the player
    'knows what goes in the box). If you really need to show a textbox without a left label, then
    'pass in a space as the left labels text " ".
    'For example if I want the left named "Send" and right named "Send Nothing" button to show and
    'have a text box in the middle with the label "Amount" and with a label to the right that said ECUs
    'then I would pass in the following arrays to UpdateUILayout:
    '   Dim buttons = New String() {"Send", "", "Send Nothing"}
    '   Dim leftLabels = New String() {"", "Amount", ""}
    '   Dim rightLabels = New String() {"", "ECUs", ""}
    'UpdateUILayout(buttons,leftLabels,rightLabels) and that is how easy it is to show and hide the 
    'control that the user will interact with!
    Private Sub UpdateUILayout(ByVal buttons As String(), ByVal leftLabels As String(), ByVal rightLabels As String())
        Try
            Dim sArray = New String() {""}

            With frmMain.MainControl1
                For i = 0 To buttons.Length - 1
                    If buttons(i).Equals("") Then
                        _buttons(i).Visibility = Visibility.Hidden
                    Else
                        _buttons(i).Visibility = Visibility.Visible
                        _buttons(i).IsEnabled = True
                        _buttons(i).Content = buttons(i)
                    End If
                Next

                For i = 0 To leftLabels.Length - 1
                    If leftLabels(i).Equals("") Then
                        _leftLabels(i).Visibility = Visibility.Hidden
                        _inputTextBoxes(i).Visibility = Visibility.Hidden
                    Else
                        _leftLabels(i).Visibility = Visibility.Visible
                        _leftLabels(i).IsEnabled = True
                        _leftLabels(i).Content = leftLabels(i)

                        _inputTextBoxes(i).Visibility = Visibility.Visible
                        _inputTextBoxes(i).IsEnabled = True
                        _inputTextBoxes(i).IsReadOnly = False
                        _inputTextBoxes(i).Text = ""
                    End If
                Next

                For i = 0 To rightLabels.Length - 1
                    If rightLabels(i).Equals("") Then
                        _rightLabels(i).Visibility = Visibility.Hidden
                    Else
                        _rightLabels(i).Visibility = Visibility.Visible
                        _rightLabels(i).IsEnabled = True
                        _rightLabels(i).Content = rightLabels(i)
                    End If
                Next
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'This is where the different screen configurations are placed.
    'When any of these methods are called, they should be able to
    'get the screen to look exactly as you want for this stage
    'regardless of what the screen look like before.
    'This means that they should not take the order of the
    'experiment into account and should work just fine if 
    'they are called out of order!
#Region "Different Screens"

    'Walk through the setting up of the screens for stage 1
    Private Sub S1Screen()
        Try
            'This experiemnt has different roles, so at each stage
            'the screen layout will look different for each role.
            'Create a function that gets the role of the player that
            'you are setting up, in this case the
            'GetIsSender(ByVal myPlayerType As String) As Boolean
            'method is used (see GetIsSender for description).
            Dim isSender = GetIsSender(PlayerData.myPlayerType)

            Dim buttonText
            Dim leftLabelText
            Dim rightLabelText

            'If an additional WPF/WinForm should be displayed during this stage of the 
            'experiment, then if it is not already showing, show the form you are supposed
            'to show. 
            'The frmMain.frmMain_Move(New Object(), New EventArgs()) is an event
            'handler in the class frmMain. It is fired off everytime the main form moves
            'and moves whatever other forms are showing so that they maintain their 
            'position relative to one another (so they look like they are all connected
            'and move together).
            'This event handle is called artifically here, so that the form being shown
            'is moved to its correct position.
            If GameSetupVars.isShowHistoryTable AndAlso Not PersonalHistory.Visible Then
                PersonalHistory.Show()
                frmMain.frmMain_Move(New Object(), New EventArgs())
            End If

            'If you are role A, then this is what the screen looks like
            'If you are role B, then this is what the screen looks like
            'A switch statement could be used if there are more than 
            '2 roles, which would make the code look nicer.
            With frmMain.MainControl1
                If isSender Then
                    'Text

                    'The SetTextFromFileCode(ByVal fileCode As String) loads the file
                    'with the code that you pass in to the rich text box control on the
                    'main form. This means you can do whatever formatting to text you like
                    'in say Microsoft Word and then just load it in.
                    SetTextFromFileCode("S1_1")

                    'Sometimes you what the text to say something like: The Sender sent you
                    '@amountSent@ ECUs. So that whatever was sent shows up instead of @amountSent@.
                    'The ReplaceTextVariables(ByVal message As String) with return the message you
                    'pass in with the variable replace with their coresponding values. To define
                    'variables (key words) see the GetValueOfVar(ByVal var As String) method.
                    .txtMain_Message.Text = ReplaceTextVariables(GameSetupVars.MB1_Standard)

                    'User Interface Layout
                    With GameSetupVars
                        'All of the text for buttons, labels, and error messages is placed
                        'in the configuration file. When the experiment starts, the text is placed
                        'into the variable GameSetupVars, which is an instance of the class
                        'EnvironmentVars.

                        'Please see:
                        'UpdateUILayout(ByVal buttons As String(), ByVal leftLabels As String(), ByVal rightLabels As String())
                        'for a description of what is going on here.
                        buttonText = New String() {.B1_Left, "", ""}
                        leftLabelText = New String() {"", .L1_Left_1, .L1_Left_2}
                        rightLabelText = New String() {"", .L1_Right_1, .L1_Right_2}
                    End With
                Else
                    'Text
                    SetTextFromFileCode("R1_1")
                    'User Interface Layout
                    With GameSetupVars
                        buttonText = New String() {"", "", ""}
                        leftLabelText = New String() {"", "", ""}
                        rightLabelText = New String() {"", "", ""}
                    End With
                End If

                'Please see UpdateUILayout
                UpdateUILayout(buttonText, leftLabelText, rightLabelText)

                'Any special functionality that you need to add that is not part of the built in 
                'template of methods used for showing/hiding buttons and text boxes should be placed
                'after the UpdateUILayout method is called. In this case, this experiment has the player
                'enter amounts in the first two textboxes and the third one is calculated for them. It is
                'a good idea to make this text box read only, so that the subject cannot change the value
                'that was calculated in the code behind. This is done below:
                If _inputTextBoxes(2).IsVisible Then
                    _inputTextBoxes(2).IsReadOnly = True
                End If
            End With
            'Jeff has a nice error reporting service that outputs errors to a log file.
            'This is useful for debugging your code. So try and place everything in a 
            'Try and Catch statement when using his template. Call the appEventLog_Write("Error: ", ex)
            'in the Catch part of the statement.
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub S2Screen()
        Try
            Dim isSender = GetIsSender(PlayerData.myPlayerType)

            Dim buttonText
            Dim leftLabelText
            Dim rightLabelText

            With frmMain.MainControl1
                If isSender Then
                    'Text
                    SetTextFromFileCode("S2_1")
                    'User Interface Layout
                    With GameSetupVars
                        buttonText = New String() {"", "", ""}
                        leftLabelText = New String() {"", "", ""}
                        rightLabelText = New String() {"", "", ""}
                    End With
                Else
                    'Text
                    SetTextFromFileCode("R2_1")
                    .txtMain_Message.Text = ReplaceTextVariables(GameSetupVars.MB2_Standard)
                    'User Interface Layout
                    With GameSetupVars
                        buttonText = New String() {.B2_Left, "", ""}
                        leftLabelText = New String() {.L2_Left_1, .L2_Left_2, .L2_Left_3}
                        rightLabelText = New String() {.L2_Right_1, .L2_Right_2, .L2_Right_3}
                    End With
                End If
                UpdateUILayout(buttonText, leftLabelText, rightLabelText)
                If _inputTextBoxes(2).IsVisible Then
                    _inputTextBoxes(2).IsReadOnly = True
                End If
            End With

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Private Sub S3Screen()
        Try
            Dim isSender = GetIsSender(PlayerData.myPlayerType)

            Dim buttonText
            Dim leftLabelText
            Dim rightLabelText

            With frmMain.MainControl1
                If isSender Then
                    'Text
                    Select Case PlayerData.TreatmentString
                        Case "00" 'No Info
                            SetTextFromFileCode("S3_1")
                        Case "10" 'Income Statement
                            SetTextFromFileCode("S3_2")
                        Case "01" 'Balance Sheet
                            SetTextFromFileCode("S3_3")
                        Case "11" 'Both
                            SetTextFromFileCode("S3_4")
                    End Select
                    'User Interface Layout
                    With GameSetupVars
                        buttonText = New String() {"", .B3_Center, ""}
                        leftLabelText = New String() {"", "", ""}
                        rightLabelText = New String() {"", "", ""}
                    End With
                Else
                    'Text
                    SetTextFromFileCode("R3_1")
                    'User Interface Layout
                    With GameSetupVars
                        buttonText = New String() {"", .B3_Center, ""}
                        leftLabelText = New String() {"", "", ""}
                        rightLabelText = New String() {"", "", ""}
                    End With
                End If

                UpdateUILayout(buttonText, leftLabelText, rightLabelText)

                If GameSetupVars.isShowHistoryTable Then
                    PersonalHistory.PopulateTable()
                End If
            End With

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    Public Sub EndGameScreen()
        Try
            With frmMain.MainControl1
                If GetIsSender(PlayerData.myPlayerType) Then
                    SetTextFromFileCode("S_End")
                Else
                    SetTextFromFileCode("R_End")
                End If

                If PlayerData.isEarningsCapped Then
                    .txtMain_Message.Text = ReplaceTextVariables(GameSetupVars.MB3_PaymentCap)
                End If

                .btnMainCenter.Visibility = Visibility.Hidden
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub
#End Region

    'Make a similar function if there is more than one type of player in the
    'experiment. This makes it easy to take action according to a players type.
    'A Select Case statement could be used instead if there are more than two
    'player roles, you should return a number instead of a Boolean if this was
    'the case and then use another Select Case statement to based on the player
    'type. It may seem redundant, but in my experience it makes the code much
    'easier to read and reuse if it is done this way.
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

    'This is called a the beginning of the experiment.
    Public Sub InitializeScreenManager()
        Try
            'Get the path to the GameText folder
            Dim dirInfo = New IO.DirectoryInfo(Windows.Forms.Application.StartupPath & "\TextFiles\GameText")
            encoder = New ASCIIEncoding()
            fileDictionary = New Dictionary(Of String, Byte())()

            'Call ConvertRTFtoByteArray(directoryInfo, fileFilter) to get
            'the files in a format that can be loaded into the rich text box
            'control on the main form.
            '('*') is called a wildcard, so if you put one at the beginning of a word and one at the end
            'of the word it means: I don't care what characters are before this word or after this word.
            'So long as this word exists in the string, I want to take it.
            Dim receiverTextList = ConvertRTFtoByteArray(dirInfo, "*Receiver*")
            Dim senderTextList = ConvertRTFtoByteArray(dirInfo, "*Sender*")
            Dim commonTextList = ConvertRTFtoByteArray(dirInfo, "Common*")

            'I named my files so that they appear in order in the folder.
            'This is bad practice and should be changed as I am depending
            'on the windows operating system to sort them for me.
            'If for some reason windows does not sort them, then 
            'my program will not work as expected!!!

            'Below are the keys that I am assigning to the different files.
            'These are for me to understand as they are only referenced in the
            'code when I am loading something into the rich text box.
            Dim rKeys = New String() {"R1_1", "R2_1", "R3_1", "R_End"}
            Dim sKeys = New String() {"S1_1", "S2_1", "S3_1", "S3_2", "S3_3", "S3_4", "S_End"}
            Dim cKeys = New String() {"C_End"}

            'Add the files to the fileDictionary using the keys you just made above.
            AddToFileDictionary(receiverTextList, rKeys)
            AddToFileDictionary(senderTextList, sKeys)
            AddToFileDictionary(commonTextList, cKeys)

            'The MainControl in the WPFassets folder is a template GUI.
            'The template has timer in the top left corner and the period in the top right
            'There is a rich text box in the main part of the form that spans from the top
            'to the bottom third of the form. Below the rich text box is a small textbox.
            'The bottom of the form is reserved for user interaction and contains the following:
            '3 buttons: left, center, and right
            '3 textboxes: top, middle, and bottom (all located in the center of the form)
            '6 labels: the labels appear on each side of the textboxes

            With frmMain.MainControl1
                _buttons = New List(Of Controls.Button)
                _leftLabels = New List(Of Controls.Label)
                _rightLabels = New List(Of Controls.Label)
                _inputTextBoxes = New List(Of Controls.TextBox)

                'The three buttons
                _buttons.Add(.btnMainLeft)
                _buttons.Add(.btnMainCenter)
                _buttons.Add(.btnMainRight)

                'The three labels on the left side of the text boxes
                _leftLabels.Add(.lblMain_Left1)
                _leftLabels.Add(.lblMain_Left2)
                _leftLabels.Add(.lblMain_Left3)

                'The three labels on the right side of the text boxes
                _rightLabels.Add(.lblMain_Right1)
                _rightLabels.Add(.lblMain_Right2)
                _rightLabels.Add(.lblMain_Right3)

                'The three text boxes 1 is the top one and 3 is the bottom one
                _inputTextBoxes.Add(.txtMain_Input1)
                _inputTextBoxes.Add(.txtMain_Input2)
                _inputTextBoxes.Add(.txtMain_Input3)
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Takes a folder path and a file filter and encodes each file in that folder that passes the filter into a list of
    'List(Of Byte()) (list of byte arrays which are arrays of 1s and 0s)
    Private Function ConvertRTFtoByteArray(ByVal directoryInfo As DirectoryInfo, ByVal fileFilter As String) As List(Of Byte())
        Try
            Dim files = directoryInfo.GetFiles(fileFilter)
            Dim fileByteList As New List(Of Byte())

            For i As Integer = 0 To files.Length - 1
                fileByteList.Add(File.ReadAllBytes(files(i).FullName))
            Next
            Return fileByteList
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
            Return Nothing
        End Try
    End Function

    'This method takes a fileCode and loads it into the rich text box on the main form
    'It does some encoding to get this to work properly, which is only important if you 
    'need to deal with some sort of error with the rich text box.
    Private Sub SetTextFromFileCode(ByVal fileCode As String)
        Try
            With frmMain.MainControl1.rtxtMainText
                Dim fileBytes As Byte() = fileDictionary(fileCode)
                Dim stream = New MemoryStream(fileBytes)

                .Document.Blocks.Clear()
                .Selection.Load(stream, Forms.DataFormats.Rtf)

                Dim stream2 = New MemoryStream()

                .SelectAll()
                .Selection.Save(stream2, dataFormat:="Xaml")

                Dim text = encoder.GetString(stream2.ToArray())
                fileBytes = encoder.GetBytes(ReplaceTextVariables(text))
                stream = New MemoryStream(fileBytes)

                .Document.Blocks.Clear()
                .Selection.Load(stream, dataFormat:="Xaml")
                .Selection.Select(.Document.ContentStart, .Document.ContentStart)
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'Takes a list of files and places them in a dictionary using a list of referece keys that are supplyed.
    'The dictionary is called fileDictionary, which you should never need to touch direcly as it has been
    'designed so that you use methods to set, get, and display files.
    Private Sub AddToFileDictionary(ByVal elementsToAdd As List(Of Byte()), ByVal referenceKeys As String())
        Try
            For i As Integer = 0 To referenceKeys.Length - 1
                fileDictionary.Add(referenceKeys(i), elementsToAdd(i))
            Next
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
        End Try
    End Sub

    'This is used to replace the @variables@ in a string with their real values
    'Call this anytime you need to replace the @variables@ in a message as it will
    'return the new message.
    Public Function ReplaceTextVariables(ByVal message As String) As String
        Try
            Dim newMessage As String = message
            Dim listOfVars = GetListOfVariables(message).Distinct().ToList()
            Dim currentVar As String

            For i As Integer = 0 To listOfVars.Count - 1
                currentVar = listOfVars(i)
                newMessage = newMessage.Replace(TextVarIndicator & currentVar & TextVarIndicator, GetValueOfVar(currentVar))
            Next

            Return newMessage
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
            Return ""
        End Try
    End Function

    'Helper function of ReplaceTextVariables
    Private Function GetListOfVariables(ByVal message As String) As List(Of String)
        Try
            Dim listOfVars As New List(Of String)
            Dim sBuilder As New StringBuilder
            Dim isReading As Boolean = False
            sBuilder.Clear()

            For i As Integer = 0 To message.Length - 1
                If message(i).Equals(TextVarIndicator) AndAlso isReading Then
                    isReading = False
                    listOfVars.Add(sBuilder.ToString())
                    sBuilder.Clear()
                ElseIf message(i).Equals(TextVarIndicator) Then
                    isReading = True
                End If

                If isReading AndAlso Not message(i).Equals(TextVarIndicator) Then
                    sBuilder.Append(message(i))
                End If
            Next

            Return listOfVars

        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
            Return New List(Of String)
        End Try
    End Function

    'This is where you place all of your key words that you should be replaced
    'when the string is shown on the screen. Key words are always entered between
    'two @ symbols. For example, if you want to replace curPeriod with the real
    'current period, then you would write @curPeriod@ in your text file. Because
    'curPeriod is defined in the Select Case (Switch) Statement below, it would
    'be replaced with the number held in the CurrentPeriod variable.
    Private Function GetValueOfVar(ByVal var As String) As String
        Try
            With PlayerData
                Dim outstr As String = ""

                Select Case var
                    Case "CurPeriod"
                        outstr = CurrentPeriod.ToString()
                    Case "ExchangeRate"
                        If GetIsSender(PlayerData.myPlayerType) Then
                            outstr = GameSetupVars.senderExchangeRate.ToString()
                        Else
                            outstr = GameSetupVars.receiverExchangeRate.ToString()
                        End If
                    Case "MyPlayerType"
                        outstr = .myAlias
                    Case "PartnerPlayerType"
                        outstr = .partnerAlias
                    Case "Sent"
                        outstr = .Sent
                    Case "SenderAccountDeposit"
                        outstr = Math.Round(GameSetupVars.senderEndowment - .Sent, 2)
                    Case "Multiplier"
                        outstr = .Multiplier
                    Case "Received"
                        outstr = .Received
                    Case "CommonAccountStart"
                        outstr = Math.Round(.Received / .Multiplier, 2) - .Sent
                    Case "CommonAccountEnd"
                        outstr = Math.Round(.Received / .Multiplier, 2)
                    Case "Invested"
                        outstr = .Invested
                    Case "ReceiverKept"
                        outstr = .ReceiverKept
                    Case "SenderGiven"
                        outstr = .SenderGiven
                    Case "Input1"
                        outstr = Math.Round(CDbl(_inputTextBoxes(0).Text), 2)
                    Case "Input2"
                        outstr = Math.Round(CDbl(_inputTextBoxes(1).Text), 2)
                    Case "Input3"
                        outstr = Math.Round(CDbl(_inputTextBoxes(2).Text), 2)
                    Case "S2_InputSum"
                        outstr = _inputTextBoxes.Where(Function(box) Not box.Text.Equals("")).Sum(Function(box) CDbl(box.Text))
                    Case "PeriodEarnings"
                        outstr = .periodEarnings(CurrentPeriod - 1).ToString()
                    Case "FinalEarnings"
                        outstr = .finalEarnings.ToString("C")
                    Case "SenderEndowment"
                        outstr = GameSetupVars.senderEndowment
                    Case "ReceiverEndowment"
                        outstr = GameSetupVars.receiverEndowment
                    Case "PaymentCap"
                        outstr = GameSetupVars.paymentCap
                    Case "SingleButtonText"
                        Select Case CurrentStage
                            Case 1
                                outstr = GameSetupVars.B1_Left
                            Case 2
                                outstr = GameSetupVars.B2_Left
                            Case 3
                                outstr = GameSetupVars.B3_Center
                        End Select
                    Case "NewLine"
                        outstr = Environment.NewLine
                    Case "sQuote"
                        outstr = "'"
                    Case "dQuote"
                        outstr = Chr(34)
                    Case "testVar"
                        outstr = testVar
                End Select

                Return outstr
            End With
        Catch ex As Exception
            appEventLog_Write("Error: ", ex)
            Return ""
        End Try
    End Function
End Class
