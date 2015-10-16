<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblStage = New System.Windows.Forms.Label()
        Me.lblTimeRemaining = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.PageSetupDialog1 = New System.Windows.Forms.PageSetupDialog()
        Me.lblPeriod = New System.Windows.Forms.Label()
        Me.llESI = New System.Windows.Forms.LinkLabel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.btnBegin_1 = New System.Windows.Forms.Button()
        Me.dgMainTable = New System.Windows.Forms.DataGridView()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnPause = New System.Windows.Forms.Button()
        Me.btnNextStage = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnEnd = New System.Windows.Forms.Button()
        Me.btnSetup = New System.Windows.Forms.Button()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.btnBegin_2 = New System.Windows.Forms.Button()
        Me.lblLoadedFile = New System.Windows.Forms.Label()
        Me.lblConnections = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblLocalHost = New System.Windows.Forms.Label()
        Me.lblIP = New System.Windows.Forms.Label()
        Me.ElementHost1 = New System.Windows.Forms.Integration.ElementHost()
        Me.WpfProgressBar1 = New Server.WPFProgressBar()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgMainTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblStage
        '
        Me.lblStage.AutoSize = True
        Me.lblStage.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStage.Location = New System.Drawing.Point(189, 181)
        Me.lblStage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStage.Name = "lblStage"
        Me.lblStage.Size = New System.Drawing.Size(83, 25)
        Me.lblStage.TabIndex = 106
        Me.lblStage.Text = "Stage: 1"
        Me.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTimeRemaining
        '
        Me.lblTimeRemaining.AutoSize = True
        Me.lblTimeRemaining.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimeRemaining.Location = New System.Drawing.Point(42, 219)
        Me.lblTimeRemaining.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTimeRemaining.Name = "lblTimeRemaining"
        Me.lblTimeRemaining.Size = New System.Drawing.Size(160, 25)
        Me.lblTimeRemaining.TabIndex = 34
        Me.lblTimeRemaining.Text = "Time Remaining:"
        Me.lblTimeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer1
        '
        '
        'Timer2
        '
        '
        'Timer3
        '
        '
        'PrintDocument1
        '
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'PrintDialog1
        '
        Me.PrintDialog1.Document = Me.PrintDocument1
        Me.PrintDialog1.UseEXDialog = True
        '
        'lblPeriod
        '
        Me.lblPeriod.AutoSize = True
        Me.lblPeriod.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriod.Location = New System.Drawing.Point(42, 181)
        Me.lblPeriod.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.Size = New System.Drawing.Size(92, 25)
        Me.lblPeriod.TabIndex = 107
        Me.lblPeriod.Text = "Period: 1"
        Me.lblPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llESI
        '
        Me.llESI.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.llESI.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llESI.Location = New System.Drawing.Point(0, 804)
        Me.llESI.Name = "llESI"
        Me.llESI.Size = New System.Drawing.Size(757, 28)
        Me.llESI.TabIndex = 114
        Me.llESI.TabStop = True
        Me.llESI.Text = "Economic Science Institute, Chapman University 2008-12 ©"
        Me.llESI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label7.Location = New System.Drawing.Point(6, 747)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(519, 52)
        Me.Label7.TabIndex = 113
        Me.Label7.Text = "Designed By:   Timothy Shields and Domenic Donato" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Templated By:   Jeffrey Kirchn" & _
    "er"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdPrint
        '
        Me.cmdPrint.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.Image = CType(resources.GetObject("cmdPrint.Image"), System.Drawing.Image)
        Me.cmdPrint.Location = New System.Drawing.Point(156, 112)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(126, 44)
        Me.cmdPrint.TabIndex = 111
        Me.cmdPrint.Text = "Print "
        Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.cmdPrint.UseVisualStyleBackColor = True
        '
        'btnBegin_1
        '
        Me.btnBegin_1.Enabled = False
        Me.btnBegin_1.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBegin_1.Image = CType(resources.GetObject("btnBegin_1.Image"), System.Drawing.Image)
        Me.btnBegin_1.Location = New System.Drawing.Point(13, 12)
        Me.btnBegin_1.Name = "btnBegin_1"
        Me.btnBegin_1.Size = New System.Drawing.Size(126, 44)
        Me.btnBegin_1.TabIndex = 27
        Me.btnBegin_1.Text = "Begin"
        Me.btnBegin_1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'dgMainTable
        '
        Me.dgMainTable.AllowUserToAddRows = False
        Me.dgMainTable.AllowUserToDeleteRows = False
        Me.dgMainTable.AllowUserToResizeColumns = False
        Me.dgMainTable.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgMainTable.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgMainTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgMainTable.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgMainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgMainTable.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column5, Me.Column3, Me.Column4})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgMainTable.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgMainTable.Location = New System.Drawing.Point(10, 287)
        Me.dgMainTable.MultiSelect = False
        Me.dgMainTable.Name = "dgMainTable"
        Me.dgMainTable.ReadOnly = True
        Me.dgMainTable.RowHeadersVisible = False
        Me.dgMainTable.RowTemplate.Height = 24
        Me.dgMainTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgMainTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgMainTable.Size = New System.Drawing.Size(735, 457)
        Me.dgMainTable.TabIndex = 108
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Image = CType(resources.GetObject("btnLoad.Image"), System.Drawing.Image)
        Me.btnLoad.Location = New System.Drawing.Point(156, 62)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(126, 44)
        Me.btnLoad.TabIndex = 122
        Me.btnLoad.Text = "Load"
        Me.btnLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'btnPause
        '
        Me.btnPause.Enabled = False
        Me.btnPause.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPause.Image = CType(resources.GetObject("btnPause.Image"), System.Drawing.Image)
        Me.btnPause.Location = New System.Drawing.Point(13, 112)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(126, 44)
        Me.btnPause.TabIndex = 121
        Me.btnPause.Text = "Pause"
        Me.btnPause.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'btnNextStage
        '
        Me.btnNextStage.Enabled = False
        Me.btnNextStage.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextStage.Image = CType(resources.GetObject("btnNextStage.Image"), System.Drawing.Image)
        Me.btnNextStage.Location = New System.Drawing.Point(13, 62)
        Me.btnNextStage.Name = "btnNextStage"
        Me.btnNextStage.Size = New System.Drawing.Size(126, 44)
        Me.btnNextStage.TabIndex = 120
        Me.btnNextStage.Text = "Next"
        Me.btnNextStage.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Image = CType(resources.GetObject("btnExit.Image"), System.Drawing.Image)
        Me.btnExit.Location = New System.Drawing.Point(304, 112)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(126, 44)
        Me.btnExit.TabIndex = 119
        Me.btnExit.Text = "Exit"
        Me.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'btnEnd
        '
        Me.btnEnd.Enabled = False
        Me.btnEnd.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEnd.Image = CType(resources.GetObject("btnEnd.Image"), System.Drawing.Image)
        Me.btnEnd.Location = New System.Drawing.Point(304, 62)
        Me.btnEnd.Name = "btnEnd"
        Me.btnEnd.Size = New System.Drawing.Size(126, 44)
        Me.btnEnd.TabIndex = 118
        Me.btnEnd.Text = "End"
        Me.btnEnd.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'btnSetup
        '
        Me.btnSetup.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSetup.Image = CType(resources.GetObject("btnSetup.Image"), System.Drawing.Image)
        Me.btnSetup.Location = New System.Drawing.Point(156, 12)
        Me.btnSetup.Name = "btnSetup"
        Me.btnSetup.Size = New System.Drawing.Size(126, 44)
        Me.btnSetup.TabIndex = 117
        Me.btnSetup.Text = "Setup"
        Me.btnSetup.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'btnReset
        '
        Me.btnReset.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReset.Image = CType(resources.GetObject("btnReset.Image"), System.Drawing.Image)
        Me.btnReset.Location = New System.Drawing.Point(304, 12)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(126, 44)
        Me.btnReset.TabIndex = 116
        Me.btnReset.Text = "Reset"
        Me.btnReset.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'btnBegin_2
        '
        Me.btnBegin_2.Enabled = False
        Me.btnBegin_2.Font = New System.Drawing.Font("Segoe UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBegin_2.Image = CType(resources.GetObject("btnBegin_2.Image"), System.Drawing.Image)
        Me.btnBegin_2.Location = New System.Drawing.Point(13, 12)
        Me.btnBegin_2.Name = "btnBegin_2"
        Me.btnBegin_2.Size = New System.Drawing.Size(126, 44)
        Me.btnBegin_2.TabIndex = 123
        Me.btnBegin_2.Text = "      Begin"
        Me.btnBegin_2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnBegin_2.Visible = False
        '
        'lblLoadedFile
        '
        Me.lblLoadedFile.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoadedFile.ForeColor = System.Drawing.Color.DarkRed
        Me.lblLoadedFile.Location = New System.Drawing.Point(414, 219)
        Me.lblLoadedFile.Name = "lblLoadedFile"
        Me.lblLoadedFile.Size = New System.Drawing.Size(293, 25)
        Me.lblLoadedFile.TabIndex = 131
        Me.lblLoadedFile.Text = "Please Load a parameter file."
        Me.lblLoadedFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblConnections
        '
        Me.lblConnections.AutoSize = True
        Me.lblConnections.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConnections.Location = New System.Drawing.Point(590, 103)
        Me.lblConnections.Name = "lblConnections"
        Me.lblConnections.Size = New System.Drawing.Size(22, 25)
        Me.lblConnections.TabIndex = 129
        Me.lblConnections.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(463, 103)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(121, 25)
        Me.Label5.TabIndex = 128
        Me.Label5.Text = "Connections:"
        '
        'lblLocalHost
        '
        Me.lblLocalHost.AutoSize = True
        Me.lblLocalHost.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLocalHost.Location = New System.Drawing.Point(463, 67)
        Me.lblLocalHost.Name = "lblLocalHost"
        Me.lblLocalHost.Size = New System.Drawing.Size(92, 25)
        Me.lblLocalHost.TabIndex = 127
        Me.lblLocalHost.Text = "Localhost"
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIP.Location = New System.Drawing.Point(463, 31)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(84, 25)
        Me.lblIP.TabIndex = 125
        Me.lblIP.Text = "127.0.0.1"
        '
        'ElementHost1
        '
        Me.ElementHost1.Location = New System.Drawing.Point(44, 249)
        Me.ElementHost1.Name = "ElementHost1"
        Me.ElementHost1.Size = New System.Drawing.Size(663, 32)
        Me.ElementHost1.TabIndex = 33
        Me.ElementHost1.Text = "ElementHost1"
        Me.ElementHost1.Child = Me.WpfProgressBar1
        '
        'Column1
        '
        Me.Column1.HeaderText = "ID"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 52
        '
        'Column2
        '
        Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Column2.HeaderText = "First Name"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 117
        '
        'Column5
        '
        Me.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Column5.HeaderText = "Last Name"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 115
        '
        'Column3
        '
        Me.Column3.HeaderText = "Status"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column3.Width = 350
        '
        'Column4
        '
        Me.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Column4.HeaderText = "Earnings"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column4.Width = 82
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(757, 832)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblLoadedFile)
        Me.Controls.Add(Me.lblConnections)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblLocalHost)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.btnBegin_1)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.llESI)
        Me.Controls.Add(Me.btnPause)
        Me.Controls.Add(Me.btnSetup)
        Me.Controls.Add(Me.btnNextStage)
        Me.Controls.Add(Me.btnBegin_2)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.lblPeriod)
        Me.Controls.Add(Me.btnEnd)
        Me.Controls.Add(Me.lblTimeRemaining)
        Me.Controls.Add(Me.lblStage)
        Me.Controls.Add(Me.dgMainTable)
        Me.Controls.Add(Me.ElementHost1)
        Me.Controls.Add(Me.Label7)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Server"
        CType(Me.dgMainTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents Timer3 As System.Windows.Forms.Timer
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PageSetupDialog1 As System.Windows.Forms.PageSetupDialog
    Friend WithEvents lblStage As System.Windows.Forms.Label
    Friend WithEvents lblTimeRemaining As System.Windows.Forms.Label
    Friend WithEvents ElementHost1 As System.Windows.Forms.Integration.ElementHost
    Friend WithEvents lblPeriod As System.Windows.Forms.Label
    Friend WithEvents llESI As System.Windows.Forms.LinkLabel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmdPrint As System.Windows.Forms.Button
    Friend WithEvents btnBegin_1 As System.Windows.Forms.Button
    Friend WithEvents dgMainTable As System.Windows.Forms.DataGridView
    Friend WpfProgressBar1 As Server.WPFProgressBar
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnPause As System.Windows.Forms.Button
    Friend WithEvents btnNextStage As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnEnd As System.Windows.Forms.Button
    Friend WithEvents btnSetup As System.Windows.Forms.Button
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents btnBegin_2 As System.Windows.Forms.Button
    Friend WithEvents lblLoadedFile As System.Windows.Forms.Label
    Friend WithEvents lblConnections As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblLocalHost As System.Windows.Forms.Label
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
