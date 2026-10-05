Imports Microsoft.Win32
Imports System.IO

Public Class frmSS

   ' Message
   'Private msg As String = "Hello, Remus!"
   Private msgColor As Color = Color.Lime
   'Private msgFont As Font
   Private titleFont As Font
   Private descFont As Font
   Private msgSize As SizeF
   Private messages As New List(Of MessageItem)
   Private current As MessageItem
   Private currentIndex As Integer = 0
   Private bounceCount As Integer = 0
   Private Const BouncesPerMessage As Integer = 4   ' change message every 4 wall hits

   Private hitWall As Boolean = False

   Private rnd As New Random()
   Private rndColor As Boolean = False

   ' Locations
   Private x As Single = 50
   Private y As Single = 50
   Private dx As Single = 4      ' horizontal speed (pixels per tick)
   Private dy As Single = 3      ' vertical speed
   Private mousePos As Point = Point.Empty

   ' Monitor areas in client coordinates (the form covers the whole virtual desktop)
   Private screenRects As New List(Of RectangleF)

   Private ReadOnly Property MaxTextWidth As Integer
      Get
         ' wrap text at half the width of the narrowest monitor, so it fits on every screen
         Return CInt(screenRects.Min(Function(r) r.Width) * 0.5)
      End Get
   End Property

   Private Sub CoverAllScreens()
      Dim virt As Rectangle = SystemInformation.VirtualScreen
      Me.WindowState = FormWindowState.Normal
      Me.Bounds = virt

      screenRects.Clear()
      For Each s In Screen.AllScreens
         screenRects.Add(New RectangleF(s.Bounds.X - virt.X, s.Bounds.Y - virt.Y, s.Bounds.Width, s.Bounds.Height))
      Next

      ' start on the primary monitor
      Dim primary = Screen.PrimaryScreen.Bounds
      x = primary.X - virt.X + 50
      y = primary.Y - virt.Y + 50
   End Sub

   ' True if the message block at (px, py) is completely on visible monitor area
   Private Function IsOnScreens(px As Single, py As Single) As Boolean
      Dim block As New RectangleF(px, py, msgSize.Width, msgSize.Height)
      Dim visible As Single = 0
      For Each r In screenRects
         Dim part = RectangleF.Intersect(block, r)
         If Not part.IsEmpty Then visible += part.Width * part.Height
      Next
      Return visible >= block.Width * block.Height - 0.5F
   End Function

   Private Sub LoadSettings()
      ' Defaults (no registry values): random file from <app folder>\Messages
      Dim randomMessages As Boolean = True
      Dim msgPath As String = Path.Combine(Application.StartupPath, "Messages")
      Dim msgFile As String = ""

      ' Registry values override the defaults (empty values are ignored)
      If RegValueExists(Registry.CurrentUser, regAppPath, "RandomMessages") Then randomMessages = RegReadBool(Registry.CurrentUser, regAppPath, "RandomMessages")

      Dim regPath As String = RegReadSZ(Registry.CurrentUser, regAppPath, "msgPath")
      If regPath <> "" Then msgPath = regPath

      Dim regFile As String = RegReadSZ(Registry.CurrentUser, regAppPath, "msgFile")
      If regFile <> "" Then msgFile = regFile

      ' No file chosen means random too
      If randomMessages OrElse msgFile = "" Then
         LoadMessages(RandomMessageFile(msgPath))
      Else
         LoadMessages(Path.Combine(msgPath, msgFile))
      End If
   End Sub

   ' Returns a random *.json file from msgPath (and subfolders), or "" if there is none
   Private Function RandomMessageFile(msgPath As String) As String
      If Not Directory.Exists(msgPath) Then Return ""
      Dim jsonFiles As String() = Directory.GetFiles(msgPath, "*.json", SearchOption.AllDirectories)
      If jsonFiles.Length = 0 Then Return ""
      Return jsonFiles(rnd.Next(0, jsonFiles.Length))
   End Function

   Private Sub LoadMessages(msgFile As String)
      If IO.File.Exists(msgFile) Then
         Dim text As String = IO.File.ReadAllText(msgFile)
         Try
            Dim dict = System.Text.Json.JsonSerializer.Deserialize(Of Dictionary(Of String, String))(text)
            If dict IsNot Nothing Then
               For Each kv In dict
                  AddMessage(kv.Key, kv.Value)
               Next
            End If
         Catch ex As Exception
            ' not strict JSON (e.g. "Ctrl+"+"": "Insert options"): read it line by line
            messages.Clear()
            LoadMessagesLenient(text)
         End Try
      End If

      If messages.Count = 0 Then
         messages.Add(New MessageItem With {
            .Title = "No messages found",
            .Description = If(msgFile = "", "No *.json files in the messages folder", "Create " & msgFile)})
      End If
   End Sub

   ' Reads lines of the form  "key": "value",  where key and value may contain unescaped quotes.
   ' The key ends at the first  ": , everything between the outer quotes is taken literally
   ' (\" and \\ are still unescaped, so properly escaped lines work too).
   Private Sub LoadMessagesLenient(text As String)
      For Each rawLine In text.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.None)
         Dim line As String = rawLine.Trim().TrimEnd(","c).Trim()
         If line.Length < 2 OrElse Not line.StartsWith("""") OrElse Not line.EndsWith("""") Then Continue For

         Dim sep As Integer = line.IndexOf(""":", 1)
         If sep < 0 Then Continue For

         Dim key As String = line.Substring(1, sep - 1)
         Dim value As String = line.Substring(sep + 2).Trim()
         If value.Length >= 2 AndAlso value.StartsWith("""") Then value = value.Substring(1, value.Length - 2)

         AddMessage(Unescape(key), Unescape(value))
      Next
   End Sub

   Private Shared Function Unescape(s As String) As String
      Return s.Replace("\\", ChrW(0)).Replace("\""", """").Replace(ChrW(0), "\")
   End Function

   Private Sub AddMessage(title As String, description As String)
      If String.IsNullOrWhiteSpace(title) Then Return
      messages.Add(New MessageItem With {
         .Title = title,
         .Description = If(description, "")})
   End Sub

   Private Sub LayoutMessages()
      ' Measure every message once so Paint doesn't have to
      Using g As Graphics = Me.CreateGraphics()
         For Each m In messages
            Dim t = g.MeasureString(m.Title, titleFont, MaxTextWidth)
            Dim d = g.MeasureString(If(m.Description, ""), descFont, MaxTextWidth)
            m.TitleHeight = t.Height
            m.Width = Math.Max(t.Width, d.Width)
            m.Height = t.Height + d.Height + 6
         Next
      End Using
   End Sub

   Private Sub SetMessage(index As Integer)
      currentIndex = index
      current = messages(index)
      msgSize = New SizeF(current.Width, current.Height)

      ' keep the new (possibly bigger) block inside the monitor its center is on
      If Not IsOnScreens(x, y) Then
         Dim center As New PointF(x + msgSize.Width / 2, y + msgSize.Height / 2)
         Dim r = screenRects.FirstOrDefault(Function(sr) sr.Contains(center))
         If r.IsEmpty Then r = screenRects(0)
         x = Math.Max(r.Left, Math.Min(x, r.Right - msgSize.Width))
         y = Math.Max(r.Top, Math.Min(y, r.Bottom - msgSize.Height))
      End If
   End Sub

   Private Sub RandomColor()
      msgColor = Color.FromArgb(rnd.Next(80, 256), rnd.Next(80, 256), rnd.Next(80, 256))
   End Sub

   Private Sub NextMessage()
      bounceCount += 1
      If bounceCount >= BouncesPerMessage Then
         bounceCount = 0
         SetMessage((currentIndex + 1) Mod messages.Count)
      End If
   End Sub

   Private Sub DrawBlock(g As Graphics, item As MessageItem, px As Single, py As Single, c As Color)
      Using br As New SolidBrush(c)
         g.DrawString(item.Title, titleFont, br,
                   New RectangleF(px, py, MaxTextWidth, item.TitleHeight + 2))
         g.DrawString(If(item.Description, ""), descFont, br,
                   New RectangleF(px, py + item.TitleHeight + 6, MaxTextWidth, item.Height))
      End Using
   End Sub

   Private Sub frmSS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.KeyPreview = True
      CoverAllScreens()
      titleFont = New Font("Segoe UI", 16, FontStyle.Bold)
      descFont = New Font("Segoe UI", 12)
      LoadSettings()
      LayoutMessages()
      SetMessage(0)
      Cursor.Hide()
      tmrMove.Interval = 30
      tmrMove.Enabled = True
      rndColor = True
   End Sub

   Private Sub frmSS_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
      If current Is Nothing Then Return
      ' ClearType doesn't blend well with transparency, so use plain antialiasing
      e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias

      DrawBlock(e.Graphics, current, x, y, msgColor)
   End Sub

   Private Sub frmSS_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
      Me.Close()
   End Sub

   Private Sub frmSS_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
      Me.Close()
   End Sub

   Private Sub frmSS_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
      ' remember the first position
      If mousePos = Point.Empty Then
         mousePos = e.Location
         Return
      End If

      ' exit if the mouse moved more than 4 pixels
      If Math.Abs(e.X - mousePos.X) > 4 OrElse Math.Abs(e.Y - mousePos.Y) > 4 Then
         Me.Close()
      End If
   End Sub

   Private Sub tmrMove_Tick(sender As Object, e As EventArgs) Handles tmrMove.Tick
      ' Move the message one axis at a time; if the step would leave the visible
      ' monitor area (outer edges, or the step between monitors of different
      ' height), bounce instead of moving
      If IsOnScreens(x + dx, y) Then
         x += dx
      Else
         dx = -dx ' Reverse horizontal direction
         hitWall = True
      End If

      If IsOnScreens(x, y + dy) Then
         y += dy
      Else
         dy = -dy ' Reverse vertical direction
         hitWall = True
      End If

      If hitWall Then
         hitWall = False
         If rndColor Then RandomColor()
         NextMessage()
      End If

      ' Redraw the form to update the message position
      Me.Invalidate()
   End Sub

End Class
