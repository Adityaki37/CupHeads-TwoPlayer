Imports System
Imports UnityEngine

' Token: 0x0200035A RID: 858
Public Class ColorAttribute
	Inherits PropertyAttribute

	' Token: 0x06000950 RID: 2384 RVA: 0x0007BE2E File Offset: 0x0007A22E
	Public Sub New(w As Single)
		Me.color = New Color(w, w, w, 1F)
	End Sub

	' Token: 0x06000951 RID: 2385 RVA: 0x0007BE49 File Offset: 0x0007A249
	Public Sub New(r As Single, g As Single, b As Single)
		Me.color = New Color(r, g, b, 1F)
	End Sub

	' Token: 0x06000952 RID: 2386 RVA: 0x0007BE64 File Offset: 0x0007A264
	Public Sub New(r As Single, g As Single, b As Single, a As Single)
		Me.color = New Color(r, g, b, a)
	End Sub

	' Token: 0x04001427 RID: 5159
	Public color As Color
End Class
