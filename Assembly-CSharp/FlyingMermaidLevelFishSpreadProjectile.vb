Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000689 RID: 1673
Public Class FlyingMermaidLevelFishSpreadProjectile
	Inherits BasicProjectile

	' Token: 0x06002349 RID: 9033 RVA: 0x0014B4F7 File Offset: 0x001498F7
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.smoke_cr())
		MyBase.StartCoroutine(Me.layer_change_cr())
	End Sub

	' Token: 0x0600234A RID: 9034 RVA: 0x0014B51C File Offset: 0x0014991C
	Private Iterator Function smoke_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.25F, 0.5F))
		Dim sprite As SpriteRenderer = MyBase.GetComponentInChildren(Of SpriteRenderer)()
		While Not MyBase.dead
			Dim smoke As Effect = Me.smokeEffectPrefab.Create(Me.smokeEffectRoot.position + MathUtils.RandomPointInUnitCircle() * 15F)
			Dim smokeSprite As SpriteRenderer = smoke.GetComponentInChildren(Of SpriteRenderer)()
			smokeSprite.sortingLayerID = sprite.sortingLayerID
			smokeSprite.sortingOrder = sprite.sortingOrder - 1
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.1F, 0.2F))
		End While
		Return
	End Function

	' Token: 0x0600234B RID: 9035 RVA: 0x0014B538 File Offset: 0x00149938
	Private Iterator Function layer_change_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Dim sprite As SpriteRenderer = MyBase.GetComponentInChildren(Of SpriteRenderer)()
		sprite.sortingLayerName = "Foreground"
		sprite.sortingOrder = 30
		Return
	End Function

	' Token: 0x04002BEA RID: 11242
	<SerializeField()>
	Private smokeEffectPrefab As Effect

	' Token: 0x04002BEB RID: 11243
	<SerializeField()>
	Private smokeEffectRoot As Transform
End Class
