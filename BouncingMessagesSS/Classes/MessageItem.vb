Public Class MessageItem
   Public Property Title As String
   Public Property Description As String

   ' Layout cache, filled once at startup (the JSON serializer ignores fields)
   Public TitleHeight As Single
   Public Width As Single
   Public Height As Single

End Class