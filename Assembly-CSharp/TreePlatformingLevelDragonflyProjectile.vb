Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200088B RID: 2187
Public Class TreePlatformingLevelDragonflyProjectile
	Inherits BasicProjectile

	' Token: 0x060032DF RID: 13023 RVA: 0x001D9534 File Offset: 0x001D7934
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.bullet_trail_cr())
	End Sub

	' Token: 0x060032E0 RID: 13024 RVA: 0x001D954C File Offset: 0x001D794C
	Private Iterator Function bullet_trail_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.16F, 0.2F))
			Dim e As Effect = Me.bulletFX.Create(MyBase.transform.position)
			Dim r As SpriteRenderer = e.GetComponent(Of SpriteRenderer)()
			r.sortingOrder = -1
			r.sortingLayerName = "Projectiles"
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003B0D RID: 15117
	Private Const ProjectilesLayerName As String = "Projectiles"

	' Token: 0x04003B0E RID: 15118
	<SerializeField()>
	Private bulletFX As Effect
End Class
