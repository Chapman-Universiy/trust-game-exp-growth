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
        Me.MainTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ElementHost1 = New System.Windows.Forms.Integration.ElementHost()
        Me.MainControl1 = New Client.MainControl()
        Me.SuspendLayout()
        '
        'MainTimer
        '
        '
        'ElementHost1
        '
        Me.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ElementHost1.Location = New System.Drawing.Point(0, 0)
        Me.ElementHost1.Name = "ElementHost1"
        Me.ElementHost1.Size = New System.Drawing.Size(565, 699)
        Me.ElementHost1.TabIndex = 0
        Me.ElementHost1.Text = "ElementHost2"
        Me.ElementHost1.Child = Me.MainControl1
        '
        'frmMain
        '
        Me.ClientSize = New System.Drawing.Size(565, 699)
        Me.Controls.Add(Me.ElementHost1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMain"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents MainTimer As System.Windows.Forms.Timer
    Friend WithEvents ElementHost1 As System.Windows.Forms.Integration.ElementHost
    Friend MainControl1 As Client.MainControl

End Class
