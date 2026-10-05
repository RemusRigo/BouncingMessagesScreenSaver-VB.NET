Module MainProgram

   Sub Main(args As String())
      Dim mode As String = "/s"

      If args.Length > 0 Then mode = args(0).Trim().ToLower().Substring(0, 2)

      Application.EnableVisualStyles()

      Select Case mode
         Case "/c"
            ShowSettings()
         Case "/p"
            ShowPreview()
         Case Else
            RunScreenSaver()
      End Select
   End Sub

   Private Sub RunScreenSaver()
      Using frm As New frmSS()
         frm.ShowDialog()
      End Using
   End Sub

   Private Sub ShowPreview()

   End Sub

   Private Sub ShowSettings()
      Using frm As New frmCfg()
         frm.ShowDialog()
      End Using
   End Sub

End Module
