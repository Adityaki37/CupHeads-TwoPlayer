Imports System
Imports UnityEngine

' Token: 0x020007FA RID: 2042
Public Class SnowCultLevelTable
	Inherits AbstractPausableComponent

	' Token: 0x06002EEE RID: 12014 RVA: 0x001BB0C8 File Offset: 0x001B94C8
	Public Sub Intro(startVel As Vector3)
		MyBase.transform.parent = Nothing
		MyBase.transform.position = Me.wiz.transform.position
		Me.vel = startVel / CupheadTime.FixedDelta
		Me.chase = True
		MyBase.animator.Play("Intro")
		Me.SFX_SNOWCULT_WizardTableCrystalballLoop()
	End Sub

	' Token: 0x06002EEF RID: 12015 RVA: 0x001BB12A File Offset: 0x001B952A
	Public Sub Outro()
		Me.chase = False
		Me.vel = (MyBase.transform.position - Me.lastPos) / CupheadTime.FixedDelta
		Me.outroTimer = 0.33333334F
	End Sub

	' Token: 0x06002EF0 RID: 12016 RVA: 0x001BB164 File Offset: 0x001B9564
	Private Sub FixedUpdate()
		If Not Me.rend.enabled Then
			Return
		End If
		Me.lastPos = MyBase.transform.position
		MyBase.transform.position += Me.vel * CupheadTime.FixedDelta
		If Me.chase Then
			Me.vel += (Me.wiz.transform.position - MyBase.transform.position).normalized * Me.accel * CupheadTime.FixedDelta
			If Me.vel.magnitude > Me.maxVel Then
				Me.vel = Me.vel.normalized * Me.maxVel
			End If
			If Vector2.Distance(Me.wiz.transform.position, MyBase.transform.position) > Me.maxDistance Then
				MyBase.transform.position = Me.wiz.transform.position + (MyBase.transform.position - Me.wiz.transform.position).normalized * Me.maxDistance
			End If
		Else
			Me.vel -= Me.vel * Me.decelOnDeactivate * CupheadTime.FixedDelta
		End If
		If Me.outroTimer > 0F Then
			Me.outroTimer -= CupheadTime.FixedDelta
			If Me.outroTimer <= 0F Then
				MyBase.animator.Play("Outro")
				Me.SFX_SNOWCULT_WizardTableDisappear()
			End If
		End If
	End Sub

	' Token: 0x06002EF1 RID: 12017 RVA: 0x001BB345 File Offset: 0x001B9745
	Private Sub AnimationEvent_SFX_SNOWCULT_WizardTableAppear()
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_crystalball_appear")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_appear")
	End Sub

	' Token: 0x06002EF2 RID: 12018 RVA: 0x001BB361 File Offset: 0x001B9761
	Private Sub SFX_SNOWCULT_WizardTableDisappear()
		AudioManager.[Stop]("sfx_dlc_snowcult_p1_wizard_crystalball_loop")
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_crystalball_disappear")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_disappear")
	End Sub

	' Token: 0x06002EF3 RID: 12019 RVA: 0x001BB387 File Offset: 0x001B9787
	Private Sub SFX_SNOWCULT_WizardTableCrystalballLoop()
		AudioManager.PlayLoop("sfx_dlc_snowcult_p1_wizard_crystalball_loop")
		AudioManager.FadeSFXVolume("sfx_dlc_snowcult_p1_wizard_crystalball_loop", 0.15F, 1F)
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_loop")
	End Sub

	' Token: 0x040037A3 RID: 14243
	<SerializeField()>
	Private wiz As SnowCultLevelWizard

	' Token: 0x040037A4 RID: 14244
	Private vel As Vector3

	' Token: 0x040037A5 RID: 14245
	Private chase As Boolean

	' Token: 0x040037A6 RID: 14246
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x040037A7 RID: 14247
	<SerializeField()>
	Private accel As Single = 1F

	' Token: 0x040037A8 RID: 14248
	<SerializeField()>
	Private decelOnDeactivate As Single = 1F

	' Token: 0x040037A9 RID: 14249
	<SerializeField()>
	Private maxVel As Single = 200F

	' Token: 0x040037AA RID: 14250
	<SerializeField()>
	Private maxDistance As Single = 20F

	' Token: 0x040037AB RID: 14251
	Private outroTimer As Single

	' Token: 0x040037AC RID: 14252
	Private lastPos As Vector3
End Class
