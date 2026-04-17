Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000543 RID: 1347
Public Class ChessKnightLevelLightningController
	Inherits AbstractMonoBehaviour

	' Token: 0x060018BD RID: 6333 RVA: 0x000E0537 File Offset: 0x000DE937
	Private Sub Start()
		MyBase.StartCoroutine(Me.lightning_cr())
	End Sub

	' Token: 0x060018BE RID: 6334 RVA: 0x000E0548 File Offset: 0x000DE948
	Private Iterator Function lightning_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.lightningDelayRange.RandomFloat())
			MyBase.animator.Play(If((Global.UnityEngine.Random.Range(0F, 3F) >= 1F), "Short", "Long"))
			MyBase.animator.Update(0F)
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Long") Then
				Me.SFX_KOG_Thunder()
			End If
			Yield MyBase.animator.WaitForAnimationToStart(Me, "None", False)
		End While
		Return
	End Function

	' Token: 0x060018BF RID: 6335 RVA: 0x000E0564 File Offset: 0x000DE964
	Private Sub LateUpdate()
		Dim num As Integer = CInt((Me.rend.sprite.name(Me.rend.sprite.name.Length - 1) - "1"c))
		If num = 51 Then
			num = 3
		End If
		Me.glowTexture.enabled = num < 3
		If Me.glowTexture.enabled Then
			Me.glowTexture.material.SetColor("_OutlineColor", New Color(1F, 1F, 1F, Me.glowIntensity(num)))
			Me.glowTexture.material.SetFloat("_DimFactor", Me.glowIntensity(num) * 0.6F)
		End If
	End Sub

	' Token: 0x060018C0 RID: 6336 RVA: 0x000E061E File Offset: 0x000DEA1E
	Private Sub SFX_KOG_Thunder()
		AudioManager.Play("sfx_dlc_kog_knight_castlethunder")
	End Sub

	' Token: 0x040021CB RID: 8651
	<SerializeField()>
	Private lightningDelayRange As MinMax = New MinMax(3F, 8F)

	' Token: 0x040021CC RID: 8652
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x040021CD RID: 8653
	<SerializeField()>
	Private glowTexture As Renderer

	' Token: 0x040021CE RID: 8654
	<SerializeField()>
	Private glowIntensity As Single() = New Single() { 0.8F, 0.4F, 0.1F }
End Class
