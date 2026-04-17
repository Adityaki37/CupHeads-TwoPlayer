Imports System
Imports System.Collections.Generic

Namespace DialoguerCore
	' Token: 0x02000B6F RID: 2927
	<Serializable()>
	Public Class DialoguerGlobalVariables
		' Token: 0x06004699 RID: 18073 RVA: 0x0024E952 File Offset: 0x0024CD52
		Public Sub New()
			Me.booleans = New List(Of Boolean)()
			Me.floats = New List(Of Single)()
			Me.strings = New List(Of String)()
		End Sub

		' Token: 0x0600469A RID: 18074 RVA: 0x0024E97B File Offset: 0x0024CD7B
		Public Sub New(booleans As List(Of Boolean), floats As List(Of Single), strings As List(Of String))
			Me.booleans = booleans
			Me.floats = floats
			Me.strings = strings
		End Sub

		' Token: 0x04004C78 RID: 19576
		Public booleans As List(Of Boolean)

		' Token: 0x04004C79 RID: 19577
		Public floats As List(Of Single)

		' Token: 0x04004C7A RID: 19578
		Public strings As List(Of String)
	End Class
End Namespace
