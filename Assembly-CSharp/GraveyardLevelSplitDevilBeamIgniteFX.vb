Imports System
Imports UnityEngine

' Token: 0x020006C9 RID: 1737
Public Class GraveyardLevelSplitDevilBeamIgniteFX
	Inherits Effect

	' Token: 0x060024FC RID: 9468 RVA: 0x0015B064 File Offset: 0x00159464
	Public Function Create(position As Vector3, fireBeamAnimator As Animator) As Effect
		Dim graveyardLevelSplitDevilBeamIgniteFX As GraveyardLevelSplitDevilBeamIgniteFX = TryCast(MyBase.Create(position), GraveyardLevelSplitDevilBeamIgniteFX)
		graveyardLevelSplitDevilBeamIgniteFX.fireBeamAnimator = fireBeamAnimator
		graveyardLevelSplitDevilBeamIgniteFX.UpdateFade(1F)
		Return graveyardLevelSplitDevilBeamIgniteFX
	End Function

	' Token: 0x060024FD RID: 9469 RVA: 0x0015B094 File Offset: 0x00159494
	Private Sub Update()
		Me.frameTimer += CupheadTime.Delta
		While Me.frameTimer > 0.041666668F
			Me.frameTimer -= 0.041666668F
			Me.UpdateFade(0.25F)
		End While
	End Sub

	' Token: 0x060024FE RID: 9470 RVA: 0x0015B0EC File Offset: 0x001594EC
	Private Sub UpdateFade(amount As Single)
		Dim bool As Boolean = Me.fireBeamAnimator.GetBool("Smoke")
		For Each spriteRenderer As SpriteRenderer In Me.groundRends
			spriteRenderer.color = New Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + If((Not bool), (-amount), amount), 0F, 1F))
		Next
		For Each spriteRenderer2 As SpriteRenderer In Me.noGroundRends
			spriteRenderer2.color = New Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + If((Not bool), amount, (-amount)), 0F, 1F))
		Next
	End Sub

	' Token: 0x04002DAB RID: 11691
	<SerializeField()>
	Private fireBeamAnimator As Animator

	' Token: 0x04002DAC RID: 11692
	<SerializeField()>
	Private groundRends As SpriteRenderer()

	' Token: 0x04002DAD RID: 11693
	<SerializeField()>
	Private noGroundRends As SpriteRenderer()

	' Token: 0x04002DAE RID: 11694
	Private frameTimer As Single
End Class
