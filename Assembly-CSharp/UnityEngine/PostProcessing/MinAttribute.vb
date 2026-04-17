Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B91 RID: 2961
	Public NotInheritable Class MinAttribute
		Inherits PropertyAttribute

		' Token: 0x06004817 RID: 18455 RVA: 0x0025DB83 File Offset: 0x0025BF83
		Public Sub New(min As Single)
			Me.min = min
		End Sub

		' Token: 0x04004D7D RID: 19837
		Public min As Single
	End Class
End Namespace
