'--------------------------------------------------------------------------------------------------
' BouncingMessagesSS: MainProgram.vb: Entry point for the application
'    © 2026 Remus Rigo
'       v1.1.20261006
'--------------------------------------------------------------------------------------------------

Module MainProgram

   Sub Main(args As String())
      Dim mode As String = ""

      If args.Length > 0 Then mode = args(0).Trim().ToLower().Substring(0, 2)

      Application.EnableVisualStyles()

      Select Case mode
         Case "/c"
            ShowSettings()
         Case "/p"
            ShowPreview(PreviewHandle(args))
         Case "/s"
            RunScreenSaver()
         Case Else
            RunScreenSaver()
      End Select
   End Sub

   Private Sub RunScreenSaver()
      Using frm As New frmSS()
         frm.ShowDialog()
      End Using
   End Sub

   ' Windows passes the preview window handle as "/p 1234" or "/p:1234"
   Private Function PreviewHandle(args As String()) As IntPtr
      Dim value As String = ""
      If args(0).Length > 3 Then
         value = args(0).Substring(3)
      ElseIf args.Length > 1 Then
         value = args(1)
      End If

      Dim handle As Long
      If Long.TryParse(value.Trim(), handle) Then Return New IntPtr(handle)
      Return IntPtr.Zero
   End Function

   Private Sub ShowPreview(parentHandle As IntPtr)
      If parentHandle = IntPtr.Zero Then Return
      ' Application.Run (not ShowDialog): the form is a child window of the preview, not a dialog
      Application.Run(New frmSS(parentHandle))
   End Sub

   Private Sub ShowSettings()
      Using frm As New frmCfg()
         frm.ShowDialog()
      End Using
   End Sub

End Module
