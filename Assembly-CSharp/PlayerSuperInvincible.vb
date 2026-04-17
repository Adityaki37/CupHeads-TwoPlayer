Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A5C RID: 2652
Public Class PlayerSuperInvincible
	Inherits AbstractPlayerSuper

	' Token: 0x06003F34 RID: 16180 RVA: 0x00229964 File Offset: 0x00227D64
	Protected Overrides Sub StartSuper()
		MyBase.StartSuper()
		AudioManager.Play("player_super_invincibility")
		If Me.player.id = PlayerId.PlayerOne Then
			Me.shadowMugman.SetActive(False)
			Me.shadow = Me.shadowCuphead
		Else
			Me.shadowCuphead.SetActive(False)
			Me.shadow = Me.shadowMugman
		End If
		MyBase.transform.position = Me.player.transform.position
		MyBase.StartCoroutine(Me.super_cr())
		If Not Me.player.motor.Grounded Then
			Me.shadow.SetActive(False)
			Me.shadow.transform.position = Me.player.GetComponent(Of LevelPlayerShadow)().ShadowPosition() + Me.shadowOffset
		End If
		Level.ScoringData.superMeterUsed += 5
	End Sub

	' Token: 0x06003F35 RID: 16181 RVA: 0x00229A4C File Offset: 0x00227E4C
	Public Overrides Sub Interrupt()
		Me.StopAllCoroutines()
		AudioManager.ChangeBGMPitch(1F, 1.5F)
		If Me.player IsNot Nothing Then
			Me.player.animationController.SetOldMaterial()
			Me.player.stats.SetInvincible(False)
		End If
	End Sub

	' Token: 0x06003F36 RID: 16182 RVA: 0x00229AA0 File Offset: 0x00227EA0
	Private Iterator Function super_cr() As IEnumerator
		If Me.player IsNot Nothing Then
			Me.player.stats.SetInvincible(True)
		End If
		Yield CupheadTime.WaitForSeconds(Me, WeaponProperties.LevelSuperInvincibility.durationInvincible)
		If Me.player IsNot Nothing Then
			Me.player.stats.SetInvincible(False)
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06003F37 RID: 16183 RVA: 0x00229ABC File Offset: 0x00227EBC
	Private Iterator Function invincibility_fx_cr() As IEnumerator
		Dim sparkleRoutine As IEnumerator = Me.sparkle_cr()
		MyBase.StartCoroutine(sparkleRoutine)
		If Me.player IsNot Nothing Then
			Me.player.animationController.SetMaterial(Me.superMaterial)
		End If
		Yield CupheadTime.WaitForSeconds(Me, WeaponProperties.LevelSuperInvincibility.durationFX - 1.25F)
		AudioManager.ChangeBGMPitch(1.8F, 1.5F)
		For i As Integer = 0 To 5 - 1
			If Me.player IsNot Nothing Then
				Me.player.animationController.SetOldMaterial()
			End If
			Yield CupheadTime.WaitForSeconds(Me, 0.125F)
			If Me.player IsNot Nothing Then
				Me.player.animationController.SetMaterial(Me.superMaterial)
			End If
			Yield CupheadTime.WaitForSeconds(Me, 0.125F)
		Next
		AudioManager.ChangeBGMPitch(1F, 1.5F)
		If Me.player IsNot Nothing Then
			Me.player.animationController.SetOldMaterial()
		End If
		MyBase.StopCoroutine(sparkleRoutine)
		Yield Nothing
		Return
	End Function

	' Token: 0x06003F38 RID: 16184 RVA: 0x00229AD8 File Offset: 0x00227ED8
	Private Iterator Function sparkle_cr() As IEnumerator
		While True AndAlso Me.player IsNot Nothing
			Dim x As Single = Global.UnityEngine.Random.Range(-Me.player.colliderManager.Width, Me.player.colliderManager.Width)
			Dim y As Single = Global.UnityEngine.Random.Range(Me.player.colliderManager.Height * -0.5F, Me.player.colliderManager.Height * 1.5F)
			Me.sparkle.Create(Me.player.transform.position + New Vector3(x, y, 0F))
			Yield CupheadTime.WaitForSeconds(Me, Me.sparkleSpawnTime)
		End While
		Return
	End Function

	' Token: 0x06003F39 RID: 16185 RVA: 0x00229AF4 File Offset: 0x00227EF4
	Private Sub EndPlayerAnimation()
		Me.Fire()
		Me.EndSuper(False)
		MyBase.StartCoroutine(Me.invincibility_fx_cr())
		MyBase.StartCoroutine(Me.super_cr())
		If Me.player IsNot Nothing Then
			Me.player.animationController.SetSpriteProperties(SpriteLayer.Effects, 3000)
		End If
	End Sub

	' Token: 0x06003F3A RID: 16186 RVA: 0x00229B50 File Offset: 0x00227F50
	Private Sub BigCupAppears()
		If Not Me.player.motor.Grounded Then
			Me.shadow.SetActive(True)
			Dim num As Single = Mathf.Abs(Me.player.transform.position.y - Me.shadow.transform.position.y)
			Dim num2 As Single = Mathf.Max(0F, 1F - num / 500F)
			Me.shadow.transform.localScale = Vector3.one * num2
		End If
	End Sub

	' Token: 0x06003F3B RID: 16187 RVA: 0x00229BE8 File Offset: 0x00227FE8
	Private Sub ResetSpriteOrder()
		If Me.player IsNot Nothing Then
			Me.player.animationController.ResetSpriteProperties()
		End If
	End Sub

	' Token: 0x04004646 RID: 17990
	Private Const maxShadowDistance As Single = 500F

	' Token: 0x04004647 RID: 17991
	<SerializeField()>
	Private superMaterial As Material

	' Token: 0x04004648 RID: 17992
	<SerializeField()>
	Private sparkle As Effect

	' Token: 0x04004649 RID: 17993
	<SerializeField()>
	Private sparkleSpawnTime As Single

	' Token: 0x0400464A RID: 17994
	<SerializeField()>
	Private shadowOffset As Vector3

	' Token: 0x0400464B RID: 17995
	<SerializeField()>
	Private shadowCuphead As GameObject

	' Token: 0x0400464C RID: 17996
	<SerializeField()>
	Private shadowMugman As GameObject

	' Token: 0x0400464D RID: 17997
	Private shadow As GameObject
End Class
