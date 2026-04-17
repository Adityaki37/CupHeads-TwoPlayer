Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000B92 RID: 2962
	Public NotInheritable Class TrackballAttribute
		Inherits PropertyAttribute

		' Token: 0x06004818 RID: 18456 RVA: 0x0025DB92 File Offset: 0x0025BF92
		Public Sub New(method As String)
			Me.method = method
		End Sub

		' Token: 0x04004D7E RID: 19838
		Public method As String
	End Class
End Namespace
