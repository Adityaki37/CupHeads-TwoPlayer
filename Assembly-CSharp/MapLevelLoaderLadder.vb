Imports System
Imports UnityEngine

' Token: 0x0200093F RID: 2367
Public Class MapLevelLoaderLadder
	Inherits MapLevelLoader

	' Token: 0x06003761 RID: 14177 RVA: 0x001FD94C File Offset: 0x001FBD4C
	Public Sub EnableShadow(enabled As Boolean)
		Me.shadowRenderer.enabled = enabled
	End Sub

	' Token: 0x06003762 RID: 14178 RVA: 0x001FD95C File Offset: 0x001FBD5C
	Private Sub animationEvent_DownStarted()
		Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.smokeRenderers.Length)
		For i As Integer = 0 To Me.smokeRenderers.Length - 1
			Me.smokeRenderers(i).enabled = i = num
		Next
	End Sub

	' Token: 0x04003F76 RID: 16246
	<SerializeField()>
	Private shadowRenderer As SpriteRenderer

	' Token: 0x04003F77 RID: 16247
	<SerializeField()>
	Private smokeRenderers As SpriteRenderer()
End Class
