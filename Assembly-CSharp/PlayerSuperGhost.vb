Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A59 RID: 2649
Public Class PlayerSuperGhost
	Inherits AbstractPlayerSuper

	' Token: 0x06003F26 RID: 16166 RVA: 0x00229254 File Offset: 0x00227654
	Protected Overrides Sub StartSuper()
		MyBase.StartSuper()
		AudioManager.Play("player_super_ghost")
		If Not Me.player.motor.Grounded Then
			Me.cupheadBottom.enabled = False
			Me.mugmanBottom.enabled = False
		End If
		Me.createHeart = True
		MyBase.StartCoroutine(Me.super_cr())
	End Sub

	' Token: 0x06003F27 RID: 16167 RVA: 0x002292B4 File Offset: 0x002276B4
	Private Sub FixedUpdate()
		If Me.state <> PlayerSuperGhost.State.Spinning Then
			Return
		End If
		Me.t += CupheadTime.FixedDelta
		Dim localRotation As Quaternion = MyBase.transform.localRotation
		Dim num As Single
		If Me.t < WeaponProperties.LevelSuperGhost.initialSpeedTime Then
			num = Mathf.Clamp01(Me.t / WeaponProperties.LevelSuperGhost.accelerationTime) * WeaponProperties.LevelSuperGhost.initialSpeed
		Else
			Dim num2 As Single = Mathf.Clamp01((Me.t - WeaponProperties.LevelSuperGhost.initialSpeedTime) / WeaponProperties.LevelSuperGhost.accelerationTime)
			num = Mathf.Lerp(WeaponProperties.LevelSuperGhost.initialSpeed, WeaponProperties.LevelSuperGhost.maxSpeed, num2)
		End If
		Dim lookDirection As Trilean2 = New Trilean2(0, 0)
		If Me.player IsNot Nothing Then
			lookDirection = Me.player.motor.LookDirection
			If Me.player.motor.GravityReversed Then
				lookDirection.y *= -1
			End If
			If Me.player.IsDead OrElse (lookDirection.x = 0 AndAlso lookDirection.y = 0) Then
				lookDirection = Me.lookDir
			End If
		End If
		Me.lookDir = lookDirection
		Dim vector As Vector2 = New Vector2(Me.lookDir.x, Me.lookDir.y)
		Dim normalized As Vector2 = vector.normalized
		Dim vector2 As Vector2 = New Vector2(normalized.x * num, normalized.y * num)
		Me.velocity = Vector2.Lerp(Me.velocity, vector2, CupheadTime.FixedDelta * WeaponProperties.LevelSuperGhost.turnaroundEaseMultiplier)
		MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		If lookDirection.x > 0 Then
			If localRotation.z < 0.034906585F Then
				localRotation.z += 0.01F
			End If
		ElseIf localRotation.z > -0.034906585F Then
			localRotation.z -= 0.01F
		End If
		MyBase.transform.localRotation = localRotation
		If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position + New Vector2(0F, 150F * MyBase.transform.localScale.y), New Vector2(200F, 200F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06003F28 RID: 16168 RVA: 0x00229547 File Offset: 0x00227947
	Private Sub EndPlayerAnimation()
		Me.Fire()
		Me.EndSuper(True)
	End Sub

	' Token: 0x06003F29 RID: 16169 RVA: 0x00229558 File Offset: 0x00227958
	Private Iterator Function super_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Start", False, True)
		AudioManager.Play("player_super_beam")
		Me.state = PlayerSuperGhost.State.Spinning
		Me.damageDealer = New DamageDealer(WeaponProperties.LevelSuperGhost.damage, WeaponProperties.LevelSuperGhost.damageRate, DamageDealer.DamageSource.Super, False, True, True)
		Me.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier
		Me.damageDealer.PlayerId = Me.player.id
		Dim tracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Super)
		tracker.Add(Me.damageDealer)
		Me.lookDir = Me.player.motor.TrueLookDirection
		Yield CupheadTime.WaitForSeconds(Me, WeaponProperties.LevelSuperGhost.initialSpeedTime)
		MyBase.animator.SetTrigger("Continue")
		Dim t As Single = 0F
		Dim duration As Single = If((Not Me.createHeart), WeaponProperties.LevelSuperGhost.noHeartMaxSpeedTime, WeaponProperties.LevelSuperGhost.maxSpeedTime)
		While t < duration AndAlso Not Me.interrupted
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.state = PlayerSuperGhost.State.Dying
		MyBase.animator.SetTrigger("Death")
		Return
	End Function

	' Token: 0x06003F2A RID: 16170 RVA: 0x00229573 File Offset: 0x00227973
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003F2B RID: 16171 RVA: 0x00229580 File Offset: 0x00227980
	Private Sub SpawnHeart()
		If Me.player IsNot Nothing AndAlso Me.createHeart Then
			Me.heartPrefab.Create(Me.heartRoot.position, Me.player.motor.GravityReversalMultiplier)
		End If
	End Sub

	' Token: 0x06003F2C RID: 16172 RVA: 0x002295D5 File Offset: 0x002279D5
	Public Overrides Sub Interrupt()
		Me.createHeart = False
		MyBase.Interrupt()
	End Sub

	' Token: 0x06003F2D RID: 16173 RVA: 0x002295E4 File Offset: 0x002279E4
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06003F2E RID: 16174 RVA: 0x002295EC File Offset: 0x002279EC
	Private Sub SoundSuperGhostVoice()
		AudioManager.Play("player_super_ghost_voice")
		Me.emitAudioFromObject.Add("player_super_ghost_voice")
	End Sub

	' Token: 0x04004637 RID: 17975
	<SerializeField()>
	Private heartPrefab As PlayerSuperGhostHeart

	' Token: 0x04004638 RID: 17976
	<SerializeField()>
	Private heartRoot As Transform

	' Token: 0x04004639 RID: 17977
	<SerializeField()>
	Private cupheadBottom As SpriteRenderer

	' Token: 0x0400463A RID: 17978
	<SerializeField()>
	Private mugmanBottom As SpriteRenderer

	' Token: 0x0400463B RID: 17979
	Private state As PlayerSuperGhost.State

	' Token: 0x0400463C RID: 17980
	Private velocity As Vector2 = Vector2.zero

	' Token: 0x0400463D RID: 17981
	Private t As Single

	' Token: 0x0400463E RID: 17982
	Private lookDir As Trilean2

	' Token: 0x0400463F RID: 17983
	Private createHeart As Boolean

	' Token: 0x02000A5A RID: 2650
	Public Enum State
		' Token: 0x04004641 RID: 17985
		Intro
		' Token: 0x04004642 RID: 17986
		Spinning
		' Token: 0x04004643 RID: 17987
		Dying
	End Enum
End Class
