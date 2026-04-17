Imports System
Imports UnityEngine

' Token: 0x020008B2 RID: 2226
Public Class FunhousePlatformingLevelConveyorBelt
	Inherits ScrollingSprite

	' Token: 0x060033E3 RID: 13283 RVA: 0x001E1998 File Offset: 0x001DFD98
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.point += MyBase.transform.position
	End Sub

	' Token: 0x060033E4 RID: 13284 RVA: 0x001E19BC File Offset: 0x001DFDBC
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.DrawSphere(Me.point + MyBase.transform.position, 10F)
	End Sub

	' Token: 0x060033E5 RID: 13285 RVA: 0x001E19E4 File Offset: 0x001DFDE4
	Protected Overrides Sub Update()
		Me.wait -= CupheadTime.Delta
		If Me.wait > 0F Then
			Return
		End If
		MyBase.Update()
		For i As Integer = 0 To MyBase.copyRenderers.Count - 1
			Dim position As Vector3 = MyBase.copyRenderers(i).transform.position
			position.z = position.x - Me.point.x
			If Me.rightToCenter Then
				position.z *= -1F
			End If
			MyBase.copyRenderers(i).transform.position = position
		Next
	End Sub

	' Token: 0x04003C29 RID: 15401
	Public point As Vector3

	' Token: 0x04003C2A RID: 15402
	Public rightToCenter As Boolean

	' Token: 0x04003C2B RID: 15403
	Public wait As Single
End Class
