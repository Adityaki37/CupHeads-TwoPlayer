Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200066D RID: 1645
Public Class FlyingGenieLevelGoop
	Inherits LevelProperties.FlyingGenie.Entity

	' Token: 0x06002290 RID: 8848 RVA: 0x00144C4C File Offset: 0x0014304C
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingGenie)
		MyBase.LevelInit(properties)
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002291 RID: 8849 RVA: 0x00144C78 File Offset: 0x00143078
	Public Sub ActivateGoop()
		MyBase.animator.SetTrigger("OnStartGoop")
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06002292 RID: 8850 RVA: 0x00144CC7 File Offset: 0x001430C7
	Private Sub DeactivateGoop()
		Me.moving = False
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		Me.StopAllCoroutines()
		MyBase.animator.Play("Off")
	End Sub

	' Token: 0x06002293 RID: 8851 RVA: 0x00144CFE File Offset: 0x001430FE
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06002294 RID: 8852 RVA: 0x00144D14 File Offset: 0x00143114
	Public Iterator Function move_cr() As IEnumerator
		Dim p As LevelProperties.FlyingGenie.Coffin = MyBase.properties.CurrentState.coffin
		Dim goingUp As Boolean = False
		Me.moving = True
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		While True
			If Me.moving Then
				If goingUp Then
					While MyBase.transform.position.y < Me.yMax
						MyBase.transform.AddPosition(0F, p.heartMovement * CupheadTime.Delta, 0F)
						Yield Nothing
					End While
					goingUp = Not goingUp
				Else
					While MyBase.transform.position.y > Me.yMin
						MyBase.transform.AddPosition(0F, -p.heartMovement * CupheadTime.Delta, 0F)
						Yield Nothing
					End While
					goingUp = Not goingUp
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002295 RID: 8853 RVA: 0x00144D30 File Offset: 0x00143130
	Private Iterator Function shoot_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		While True
			If Me.moving Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.coffin.heartShotDelayRange.RandomFloat())
				MyBase.animator.SetTrigger("OnAttack")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002296 RID: 8854 RVA: 0x00144D4C File Offset: 0x0014314C
	Private Sub ShootProjectiles()
		AudioManager.Play("genie_sarcophagus_eye_plop")
		Me.emitAudioFromObject.Add("genie_sarcophagus_eye_plop")
		Me.projectile.Create(Me.topRoot.position, MyBase.properties.CurrentState.coffin, True)
		Me.projectile.Create(Me.bottomRoot.position, MyBase.properties.CurrentState.coffin, False)
	End Sub

	' Token: 0x06002297 RID: 8855 RVA: 0x00144DC3 File Offset: 0x001431C3
	Public Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.StartCoroutine(Me.death_cr())
		AudioManager.Play("genie_goop_voice_exit")
		Me.emitAudioFromObject.Add("genie_goop_voice_exit")
	End Sub

	' Token: 0x06002298 RID: 8856 RVA: 0x00144E04 File Offset: 0x00143204
	Private Iterator Function death_cr() As IEnumerator
		Dim moveSpeed As Single = 50F
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		While MyBase.transform.localPosition.x < Me.endRoot.localPosition.x
			MyBase.transform.localPosition += MyBase.transform.right * moveSpeed
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.DeactivateGoop()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002299 RID: 8857 RVA: 0x00144E1F File Offset: 0x0014321F
	Private Sub SoundGenieGoopIntro()
		AudioManager.Play("genie_goop_voice_enter")
		Me.emitAudioFromObject.Add("genie_goop_voice_enter")
	End Sub

	' Token: 0x0600229A RID: 8858 RVA: 0x00144E3B File Offset: 0x0014323B
	Private Sub SoundGenieGoopAttackPre()
		AudioManager.Play("genie_goop_attack_pre")
		Me.emitAudioFromObject.Add("genie_goop_attack_pre")
	End Sub

	' Token: 0x0600229B RID: 8859 RVA: 0x00144E57 File Offset: 0x00143257
	Private Sub SoundGenieGoopAttack()
		AudioManager.Play("gene_goop_voice_attack")
		Me.emitAudioFromObject.Add("gene_goop_voice_attack")
	End Sub

	' Token: 0x04002B40 RID: 11072
	<SerializeField()>
	Private topRoot As Transform

	' Token: 0x04002B41 RID: 11073
	<SerializeField()>
	Private bottomRoot As Transform

	' Token: 0x04002B42 RID: 11074
	<SerializeField()>
	Private projectile As FlyingGenieLevelHelixProjectile

	' Token: 0x04002B43 RID: 11075
	<SerializeField()>
	Private endRoot As Transform

	' Token: 0x04002B44 RID: 11076
	Private moving As Boolean

	' Token: 0x04002B45 RID: 11077
	Private yMax As Single = 60F

	' Token: 0x04002B46 RID: 11078
	Private yMin As Single = -260F

	' Token: 0x04002B47 RID: 11079
	Private damageReceiver As DamageReceiver
End Class
