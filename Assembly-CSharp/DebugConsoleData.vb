Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000422 RID: 1058
Public Class DebugConsoleData
	Inherits ScriptableObject

	' Token: 0x17000268 RID: 616
	' (get) Token: 0x06000F55 RID: 3925 RVA: 0x00097288 File Offset: 0x00095688
	Public ReadOnly Property Current As DebugConsoleData.Command
		Get
			Me.CleanIndex()
			Return Me.commands(Me.index)
		End Get
	End Property

	' Token: 0x06000F56 RID: 3926 RVA: 0x000972A1 File Offset: 0x000956A1
	Public Sub PrepareForSave()
	End Sub

	' Token: 0x06000F57 RID: 3927 RVA: 0x000972A3 File Offset: 0x000956A3
	Private Sub CleanIndex()
		If Me.index >= Me.commands.Count Then
			Me.index = Me.commands.Count - 1
		End If
	End Sub

	' Token: 0x0400185F RID: 6239
	Public Shared PATH As String = "TC_DebugConsole/tc_debug_console_data"

	' Token: 0x04001860 RID: 6240
	Public index As Integer

	' Token: 0x04001861 RID: 6241
	Public commands As List(Of DebugConsoleData.Command) = New List(Of DebugConsoleData.Command)()

	' Token: 0x02000423 RID: 1059
	<Serializable()>
	Public Class Command
		' Token: 0x04001862 RID: 6242
		Public command As String = "new.command"

		' Token: 0x04001863 RID: 6243
		Public key As KeyCode

		' Token: 0x04001864 RID: 6244
		Public rewiredAction As String = String.Empty

		' Token: 0x04001865 RID: 6245
		Public arguments As List(Of DebugConsoleData.Command.Argument) = New List(Of DebugConsoleData.Command.Argument)()

		' Token: 0x04001866 RID: 6246
		Public help As String = String.Empty

		' Token: 0x04001867 RID: 6247
		Public code As String = String.Empty

		' Token: 0x04001868 RID: 6248
		Public closeConsole As Boolean

		' Token: 0x02000424 RID: 1060
		<Serializable()>
		Public Class Argument
			' Token: 0x04001869 RID: 6249
			Public type As DebugConsoleData.Command.Argument.Type

			' Token: 0x0400186A RID: 6250
			Public name As String = "argName"

			' Token: 0x02000425 RID: 1061
			Public Enum Type
				' Token: 0x0400186C RID: 6252
				Int
				' Token: 0x0400186D RID: 6253
				Float
				' Token: 0x0400186E RID: 6254
				Bool
				' Token: 0x0400186F RID: 6255
				[String]
			End Enum
		End Class
	End Class
End Class
