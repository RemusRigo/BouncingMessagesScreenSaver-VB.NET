'--------------------------------------------------------------------------------------------------
' BouncingMessagesSS: frmCfg.vb: Configuration form
'    © 2026 Remus Rigo
'       v1.1.20261006
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32
Imports System.IO
Imports BouncingMessagesSS.API

Public Class frmCfg

   Private Const SYSMENU_ABOUT_ID As UInteger = 1000

   Protected Overrides Sub OnHandleCreated(e As EventArgs)
      MyBase.OnHandleCreated(e)
      Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
      ' Add a separator and then your custom item
      AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
      AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "About...")
   End Sub

   Protected Overrides Sub WndProc(ByRef m As Message)
      MyBase.WndProc(m)
      If m.Msg = WM_SYSCOMMAND Then
         If CUInt(m.WParam) = SYSMENU_ABOUT_ID Then
            frmAbout.ShowDialog()
         End If
      End If
   End Sub


   Private Sub ScanMessages(msgPath As String)

      Dim rootPath As String = msgPath

      ' Check if the specified folder exists
      If Not System.IO.Directory.Exists(rootPath) Then
         MessageBox.Show($"The specified folder does not exist: {rootPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Return
      End If

      ' Ensure the root path ends with a separator for clean path trimming
      If Not rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()) Then
         rootPath &= Path.DirectorySeparatorChar
      End If

      lstBoxMessages.Items.Clear()

      ' Get all JSON files recursively from the selected folder and subfolders
      Dim jsonFiles As String() = Directory.GetFiles(rootPath, "*.json", SearchOption.AllDirectories)

      For Each filePath As String In jsonFiles
         ' Strip the root path to get the relative path structure
         Dim relativePath As String = filePath.Substring(rootPath.Length)
         lstBoxMessages.Items.Add(relativePath)
      Next
   End Sub

   Private Sub frmCfg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      ' Show the saved settings (same defaults as frmSS.LoadSettings)
      If RegValueExists(Registry.CurrentUser, regAppPath, "RandomMessages") Then
         chkBoxRandomMessages.Checked = RegReadBool(Registry.CurrentUser, regAppPath, "RandomMessages")
      Else
         chkBoxRandomMessages.Checked = True
      End If

      Dim msgPath As String = RegReadSZ(Registry.CurrentUser, regAppPath, "msgPath")
      If String.IsNullOrWhiteSpace(msgPath) Then msgPath = Path.Combine(Application.StartupPath, "Messages")
      txtBoxMessagePath.Text = msgPath

      If Directory.Exists(msgPath) Then
         ScanMessages(msgPath)
         Dim msgFile As String = RegReadSZ(Registry.CurrentUser, regAppPath, "msgFile")
         If msgFile <> "" Then lstBoxMessages.SelectedItem = msgFile
      End If
   End Sub

   Private Sub chkBoxRandomMessages_CheckedChanged(sender As Object, e As EventArgs) Handles chkBoxRandomMessages.CheckedChanged
      lstBoxMessages.Enabled = Not chkBoxRandomMessages.Checked
   End Sub

   Private Sub txtBoxMessagePath_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBoxMessagePath.KeyDown
      If e.KeyCode = Keys.Enter Then ScanMessages(txtBoxMessagePath.Text)
   End Sub


   Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
      Dim dlgBrowse As New FolderBrowserDialog()
      dlgBrowse.Description = "Select the folder containing the messages"
      dlgBrowse.InitialDirectory = Application.StartupPath
      dlgBrowse.ShowNewFolderButton = True
      If dlgBrowse.ShowDialog() = DialogResult.OK Then
         txtBoxMessagePath.Text = dlgBrowse.SelectedPath
         ScanMessages(dlgBrowse.SelectedPath)
      End If
   End Sub

   Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOk.Click
      Try
         RegWriteBool(Registry.CurrentUser, regAppPath, "RandomMessages", chkBoxRandomMessages.Checked)
         RegWriteSZ(Registry.CurrentUser, regAppPath, "msgPath", txtBoxMessagePath.Text)
         RegWriteSZ(Registry.CurrentUser, regAppPath, "msgFile", If(lstBoxMessages.SelectedItem?.ToString(), ""))
         Me.DialogResult = DialogResult.OK
         Me.Close()
      Catch ex As Exception
         MessageBox.Show($"Error occurred while saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Me.DialogResult = DialogResult.None
      End Try
   End Sub

End Class