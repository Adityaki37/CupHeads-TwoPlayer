Imports System
Imports UnityEngine

' Token: 0x02000AC3 RID: 2755
Public Class PlayerCameraController
	Inherits AbstractPlayerComponent

	' Token: 0x06004228 RID: 16936 RVA: 0x0023BAC4 File Offset: 0x00239EC4
	Public Sub LevelInit()
		Me.rect = Nothing
		Me.rect.x = MyBase.transform.position.x - 100F
		Me.rect.width = 200F
		Me.rect.y = MyBase.transform.position.y - 150F
		Me.rect.height = 300F
	End Sub

	' Token: 0x06004229 RID: 16937 RVA: 0x0023BB48 File Offset: 0x00239F48
	Private Sub Update()
		If MyBase.basePlayer.right > Me.rect.x + Me.rect.width Then
			Me.rect.x = MyBase.basePlayer.right - 200F
		End If
		If MyBase.basePlayer.left < Me.rect.xMin Then
			Me.rect.x = MyBase.basePlayer.left
		End If
		If MyBase.basePlayer.top > Me.rect.y + 300F Then
			Me.rect.y = MyBase.basePlayer.top - 300F
		End If
		If MyBase.basePlayer.bottom < Me.rect.y Then
			Me.rect.y = MyBase.basePlayer.bottom
		End If
	End Sub

	' Token: 0x0600422A RID: 16938 RVA: 0x0023BC38 File Offset: 0x0023A038
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Not PlayerDebug.Enabled Then
			Return
		End If
		Gizmos.color = New Color(1F, 0F, 0F, 0.5F)
		Gizmos.DrawWireCube(Me.rect.center, New Vector3(Me.rect.width, Me.rect.height, 1F))
		Gizmos.color = Color.white
	End Sub

	' Token: 0x170005CC RID: 1484
	' (get) Token: 0x0600422B RID: 16939 RVA: 0x0023BCB3 File Offset: 0x0023A0B3
	Public ReadOnly Property center As Vector2
		Get
			Return Me.rect.center
		End Get
	End Property

	' Token: 0x040048A3 RID: 18595
	Public Const WIDTH As Single = 200F

	' Token: 0x040048A4 RID: 18596
	Public Const HEIGHT As Single = 300F

	' Token: 0x040048A5 RID: 18597
	Private rect As Rect = Nothing
End Class
