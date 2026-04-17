Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B90 RID: 2960
	Public NotInheritable Class GetSetAttribute
		Inherits PropertyAttribute

		' Token: 0x06004816 RID: 18454 RVA: 0x0025DB74 File Offset: 0x0025BF74
		Public Sub New(name As String)
			Me.name = name
		End Sub

		' Token: 0x04004D7B RID: 19835
		Public name As String

		' Token: 0x04004D7C RID: 19836
		Public dirty As Boolean
	End Class
End Namespace
