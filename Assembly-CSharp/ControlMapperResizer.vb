Imports System
Imports UnityEngine

' Token: 0x02000C27 RID: 3111
Public Class ControlMapperResizer
	Inherits MonoBehaviour

	' Token: 0x06004BE5 RID: 19429 RVA: 0x00271888 File Offset: 0x0026FC88
	Private Sub Update()
		Dim num As Single = Mathf.Clamp(CSng(Screen.width) / CSng(Screen.height) / 1.7777778F, 0F, 1F)
		If Me.cachedSize <> num Then
			Me.cachedSize = num
			MyBase.transform.localScale = New Vector3(num, num)
		End If
	End Sub

	' Token: 0x040050A9 RID: 20649
	Private cachedSize As Single
End Class
