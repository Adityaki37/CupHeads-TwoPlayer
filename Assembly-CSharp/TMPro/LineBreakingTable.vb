Imports System
Imports System.Collections.Generic

Namespace TMPro
	' Token: 0x02000CA1 RID: 3233
	<Serializable()>
	Public Class LineBreakingTable
		' Token: 0x06005188 RID: 20872 RVA: 0x002998BB File Offset: 0x00297CBB
		Public Sub New()
			Me.leadingCharacters = New Dictionary(Of Integer, Char)()
			Me.followingCharacters = New Dictionary(Of Integer, Char)()
		End Sub

		' Token: 0x0400543C RID: 21564
		Public leadingCharacters As Dictionary(Of Integer, Char)

		' Token: 0x0400543D RID: 21565
		Public followingCharacters As Dictionary(Of Integer, Char)
	End Class
End Namespace
