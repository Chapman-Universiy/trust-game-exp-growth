Class EnvironmentVars
    Public numberOfPlayers As Integer                   'number of desired players
    Public numberOfPeriods As Integer                   'number of periods

    Public portNumber As Integer                     'port number sockect traffic is operation on 
    Public UpperLeftCorner_X As Integer
    Public UpperLeftCorner_Y As Integer
    Public PrimaryWindow_Width As Integer
    Public PrimaryWindow_Height As Integer

    Public showInstructions As Boolean                  'show client instructions
    Public instructionsDuration_Minutes As Integer
    Public instructionsWarning_MinutesLeft As Integer

    Public isTestMode As Boolean
    Public isRandomPeriodEarnings As Boolean
    Public areRolesFixed As Boolean
    Public isRandomRematching As Boolean
    
    Public stageLengths(NumberOfStages - 1) As Integer
    Public warningTimes(NumberOfStages - 1) As Integer
    Public timeBetweenStages As Integer

    Public multiplierList As List(Of Integer())

    Public senderExchangeRate As Double
    Public receiverExchangeRate As Double
    Public senderEndowment As Integer
    Public receiverEndowment As Integer
    Public receiverAlias As String
    Public senderAlias As String

    Public paymentCap As Integer

    'Treatments
    Public isShowBalanceSheet As Boolean
    Public isShowIncomeStatement As Boolean
    Public isShowHistoryTable As Boolean

    'Message Box
    Public MB1_Standard As String
    Public MB1_Error As String
    Public MB1_Submitted As String
    Public MB2_Standard As String
    Public MB2_Error As String
    Public MB2_Submitted As String
    Public MB3_PaymentCap As String


    'Action Messages
    Public TimeOut_M As String
    Public M1_LeftButton As String
    Public M2_LeftButton As String

    'Buttons
    Public B1_Left As String
    Public B2_Left As String
    Public B3_Center As String

    'Labels
    Public L1_Right_1 As String
    Public L1_Left_1 As String
    Public L1_Right_2 As String
    Public L1_Left_2 As String
    Public L2_Right_1 As String
    Public L2_Left_1 As String
    Public L2_Right_2 As String
    Public L2_Left_2 As String
    Public L2_Right_3 As String
    Public L2_Left_3 As String

    Public L_HistoryTable As String
End Class
