<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCfg
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCfg))
      btnOk = New Button()
      chkBoxRandomMessages = New CheckBox()
      txtBoxMessagePath = New TextBox()
      btnBrowse = New Button()
      lstBoxMessages = New ListBox()
      SuspendLayout()
      ' 
      ' btnOk
      ' 
      btnOk.Image = CType(resources.GetObject("btnOk.Image"), Image)
      btnOk.Location = New Point(762, 412)
      btnOk.Name = "btnOk"
      btnOk.Size = New Size(35, 35)
      btnOk.TabIndex = 0
      btnOk.UseVisualStyleBackColor = True
      ' 
      ' chkBoxRandomMessages
      ' 
      chkBoxRandomMessages.AutoSize = True
      chkBoxRandomMessages.Location = New Point(12, 3)
      chkBoxRandomMessages.Name = "chkBoxRandomMessages"
      chkBoxRandomMessages.Size = New Size(125, 19)
      chkBoxRandomMessages.TabIndex = 2
      chkBoxRandomMessages.Text = "Random Messages"
      chkBoxRandomMessages.UseVisualStyleBackColor = True
      ' 
      ' txtBoxMessagePath
      ' 
      txtBoxMessagePath.Location = New Point(12, 425)
      txtBoxMessagePath.Name = "txtBoxMessagePath"
      txtBoxMessagePath.Size = New Size(649, 23)
      txtBoxMessagePath.TabIndex = 3
      ' 
      ' btnBrowse
      ' 
      btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), Image)
      btnBrowse.Location = New Point(667, 412)
      btnBrowse.Name = "btnBrowse"
      btnBrowse.Size = New Size(35, 35)
      btnBrowse.TabIndex = 4
      btnBrowse.UseVisualStyleBackColor = True
      ' 
      ' lstBoxMessages
      ' 
      lstBoxMessages.FormattingEnabled = True
      lstBoxMessages.Location = New Point(12, 28)
      lstBoxMessages.Name = "lstBoxMessages"
      lstBoxMessages.Size = New Size(349, 394)
      lstBoxMessages.TabIndex = 5
      ' 
      ' frmCfg
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(lstBoxMessages)
      Controls.Add(btnBrowse)
      Controls.Add(txtBoxMessagePath)
      Controls.Add(chkBoxRandomMessages)
      Controls.Add(btnOk)
      Name = "frmCfg"
      Text = "frmCfg"
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents btnOk As Button
   Friend WithEvents chkBoxRandomMessages As CheckBox
   Friend WithEvents txtBoxMessagePath As TextBox
   Friend WithEvents btnBrowse As Button
   Friend WithEvents lstBoxMessages As ListBox
End Class
