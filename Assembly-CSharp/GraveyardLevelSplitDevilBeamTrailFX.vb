Imports System
Imports UnityEngine

' Token: 0x020006CA RID: 1738
Public Class GraveyardLevelSplitDevilBeamTrailFX
	Inherits Effect

	' Token: 0x06002500 RID: 9472 RVA: 0x0015B228 File Offset: 0x00159628
	Public Function Create(position As Vector3, scale As Vector3, main As GraveyardLevelSplitDevilBeam, anim As Integer) As Effect
		Dim graveyardLevelSplitDevilBeamTrailFX As GraveyardLevelSplitDevilBeamTrailFX = TryCast(MyBase.Create(position), GraveyardLevelSplitDevilBeamTrailFX)
		graveyardLevelSplitDevilBeamTrailFX.transform.localScale = scale
		graveyardLevelSplitDevilBeamTrailFX.main = main
		graveyardLevelSplitDevilBeamTrailFX.animator.Play(anim.ToString())
		graveyardLevelSplitDevilBeamTrailFX.animator.Update(0F)
		graveyardLevelSplitDevilBeamTrailFX.rend.sortingOrder = -5 + anim
		graveyardLevelSplitDevilBeamTrailFX.UpdateFade(1F)
		Return graveyardLevelSplitDevilBeamTrailFX
	End Function

	' Token: 0x06002501 RID: 9473 RVA: 0x0015B29C File Offset: 0x0015969C
	Private Sub Update()
		Me.frameTimer += CupheadTime.Delta
		While Me.frameTimer > 0.041666668F
			Me.frameTimer -= 0.041666668F
			Me.UpdateFade(0.25F)
		End While
	End Sub

	' Token: 0x06002502 RID: 9474 RVA: 0x0015B2F4 File Offset: 0x001596F4
	Private Sub UpdateFade(amount As Single)
		Me.rend.color = New Color(Me.rend.color.r, Me.rend.color.g, Me.rend.color.b, Mathf.Clamp(Me.rend.color.a + If(Me.main.devil.isAngel, (-amount), amount), 0F, 1F))
	End Sub

	' Token: 0x04002DAF RID: 11695
	<SerializeField()>
	Private main As GraveyardLevelSplitDevilBeam

	' Token: 0x04002DB0 RID: 11696
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04002DB1 RID: 11697
	Private frameTimer As Single
End Class
