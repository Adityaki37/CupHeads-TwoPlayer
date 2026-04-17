Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000713 RID: 1811
Public Class OldManLevelSpitProjectile
	Inherits AbstractProjectile

	' Token: 0x0600275B RID: 10075 RVA: 0x001719F8 File Offset: 0x0016FDF8
	Public Sub Move(position As Vector3, speedX As Single, speedY As Single, stopPosX As Single, gravity As Single, apexTime As Single, count As Integer)
		MyBase.transform.position = position
		Me.speed = New Vector3(speedX, speedY)
		Me.stopPosX = stopPosX
		Me.gravity = gravity
		Me.apexTime = apexTime
		Me.smokeTimer = Me.firstSmokeDelay
		Me.count = count
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.changeAnimations_cr())
		Me.SFX_OMM_MouthCauldron_ProjLoop()
	End Sub

	' Token: 0x0600275C RID: 10076 RVA: 0x00171A6A File Offset: 0x0016FE6A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600275D RID: 10077 RVA: 0x00171A88 File Offset: 0x0016FE88
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		MyBase.GetComponent(Of SpriteRenderer)().color = If((Not parryable), Color.white, Color.magenta)
	End Sub

	' Token: 0x0600275E RID: 10078 RVA: 0x00171AB4 File Offset: 0x0016FEB4
	Private Iterator Function changeAnimations_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.apexTime + 0.22916667F)
		MyBase.animator.SetTrigger("OnApex")
		Yield Nothing
		Return
	End Function

	' Token: 0x0600275F RID: 10079 RVA: 0x00171AD0 File Offset: 0x0016FED0
	Private Iterator Function move_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.22916667F)
		While MyBase.transform.position.x > Me.stopPosX
			Me.speed += New Vector3(Me.gravity * CupheadTime.FixedDelta, 0F)
			MyBase.transform.Translate(Me.speed * CupheadTime.FixedDelta)
			Yield New WaitForFixedUpdate()
		End While
		Dim time As Single = 0.6F
		Dim t As Single = 0F
		While MyBase.transform.position.x > CSng(Level.Current.Left) - 100F
			If MyBase.transform.position.x <= CSng(Level.Current.Left) Then
				Me.SFX_OMM_MouthCauldron_ProjLoopEnd()
			End If
			If Me.speed.y > 0F Then
				Me.speed.y = Mathf.Lerp(Me.speed.y, 0F, t / time)
				t += CupheadTime.FixedDelta
			End If
			MyBase.transform.Translate(Me.speed * CupheadTime.FixedDelta)
			Yield New WaitForFixedUpdate()
		End While
		Me.SFX_OMM_MouthCauldron_ProjLoopEnd()
		Me.Recycle()
		Return
	End Function

	' Token: 0x06002760 RID: 10080 RVA: 0x00171AEC File Offset: 0x0016FEEC
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.smokeTimer -= CupheadTime.Delta
		If Me.smokeTimer <= 0F Then
			Me.smokeTimer += Me.smokeDelay
			CType(Level.Current, OldManLevel).CreateFX(MyBase.transform.position, False, MyBase.CanParry)
		End If
	End Sub

	' Token: 0x06002761 RID: 10081 RVA: 0x00171B5A File Offset: 0x0016FF5A
	Private Sub AnimationEvent_SFX_OMM_MouthCauldron_ProjStart()
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_projectile_loop_start")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_loop_start")
	End Sub

	' Token: 0x06002762 RID: 10082 RVA: 0x00171B78 File Offset: 0x0016FF78
	Private Sub SFX_OMM_MouthCauldron_ProjLoop()
		AudioManager.PlayLoop("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + Me.count.ToString())
		Me.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + Me.count.ToString())
	End Sub

	' Token: 0x06002763 RID: 10083 RVA: 0x00171BCB File Offset: 0x0016FFCB
	Private Sub AnimationEvent_SFX_OMM_MouthCauldron_ProjHitPlayer()
		Me.SFX_OMM_MouthCauldron_ProjLoopEnd()
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_projectile_damageplayer")
		Me.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_damageplayer")
	End Sub

	' Token: 0x06002764 RID: 10084 RVA: 0x00171BED File Offset: 0x0016FFED
	Private Sub SFX_OMM_MouthCauldron_ProjLoopEnd()
		AudioManager.[Stop]("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + Me.count.ToString())
	End Sub

	' Token: 0x04003019 RID: 12313
	Private Const OFFSCREEN_OFFSET As Single = 100F

	' Token: 0x0400301A RID: 12314
	Private speed As Vector3

	' Token: 0x0400301B RID: 12315
	Private gravity As Single

	' Token: 0x0400301C RID: 12316
	Private stopPosX As Single

	' Token: 0x0400301D RID: 12317
	Private apexTime As Single

	' Token: 0x0400301E RID: 12318
	<SerializeField()>
	Private firstSmokeDelay As Single = 1F

	' Token: 0x0400301F RID: 12319
	<SerializeField()>
	Private smokeDelay As Single = 0.05F

	' Token: 0x04003020 RID: 12320
	Private smokeTimer As Single

	' Token: 0x04003021 RID: 12321
	Private count As Integer
End Class
