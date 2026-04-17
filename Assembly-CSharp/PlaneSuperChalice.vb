Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000AAD RID: 2733
Public Class PlaneSuperChalice
	Inherits AbstractPlaneSuper

	' Token: 0x0600419B RID: 16795 RVA: 0x00238554 File Offset: 0x00236954
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.boom.gameObject.SetActive(True)
		AddHandler Me.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.player.stats.OnStoned, AddressOf Me.OnStoned
	End Sub

	' Token: 0x0600419C RID: 16796 RVA: 0x002385B0 File Offset: 0x002369B0
	Private Sub FixedUpdate()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		Me.HandleInput()
		If Not Me.exploded Then
			Me.Move()
		End If
		Me.ClampPosition()
	End Sub

	' Token: 0x0600419D RID: 16797 RVA: 0x002385E8 File Offset: 0x002369E8
	Private Sub EndIntroAnimation()
		MyBase.SnapshotAudio()
		If Me.player IsNot Nothing Then
			Me.player.UnpauseAll(False)
		End If
		Me.animHelper.IgnoreGlobal = False
		PauseManager.Unpause()
		MyBase.StartCoroutine(Me.super_cr())
	End Sub

	' Token: 0x0600419E RID: 16798 RVA: 0x00238636 File Offset: 0x00236A36
	Private Sub OnStoned()
		Me.exploded = True
	End Sub

	' Token: 0x0600419F RID: 16799 RVA: 0x0023863F File Offset: 0x00236A3F
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.exploded = True
	End Sub

	' Token: 0x060041A0 RID: 16800 RVA: 0x00238648 File Offset: 0x00236A48
	Private Iterator Function super_cr() As IEnumerator
		Me.player.damageReceiver.Vulnerable()
		Me.respawnPos = MyBase.transform.position
		Me.state = PlanePlayerWeaponManager.States.Super.Countdown
		Me.damageDealer = New DamageDealer(WeaponProperties.PlaneSuperChaliceSuperBomb.damage, WeaponProperties.PlaneSuperChaliceSuperBomb.damageRate, DamageDealer.DamageSource.Super, False, True, True)
		Me.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier
		Me.damageDealer.PlayerId = Me.player.id
		Dim tracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Super)
		tracker.Add(Me.damageDealer)
		Me.curAngle = MathUtils.DirectionToAngle(Vector3.right)
		Me.curSpeed = 0F
		While Not Me.exploded
			If Me.player IsNot Nothing Then
				Me.player.transform.position = MyBase.transform.position
			Else
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield Nothing
		End While
		Me.respawnPos = MyBase.transform.position
		Me.Fire()
		If Me.player IsNot Nothing Then
			Me.player.PauseAll()
		Else
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		MyBase.animator.SetTrigger("Explode")
		AudioManager.Play("player_plane_bomb_explosion")
		AudioManager.[Stop]("player_plane_bomb_ticktock_loop")
		Return
	End Function

	' Token: 0x060041A1 RID: 16801 RVA: 0x00238663 File Offset: 0x00236A63
	Protected Overrides Sub Fire()
		MyBase.Fire()
	End Sub

	' Token: 0x060041A2 RID: 16802 RVA: 0x0023866C File Offset: 0x00236A6C
	Private Sub PlayerReappear()
		Me.RestoreAudio(True)
		If Me.player Is Nothing Then
			Return
		End If
		Me.player.motor.OnRevive(Me.respawnPos)
		Me.player.UnpauseAll(False)
		Me.player.SetSpriteVisible(True)
		Me.player.damageReceiver.Invulnerable(2F)
	End Sub

	' Token: 0x060041A3 RID: 16803 RVA: 0x002386DA File Offset: 0x00236ADA
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060041A4 RID: 16804 RVA: 0x002386E7 File Offset: 0x00236AE7
	Private Sub StartBoomScale()
		Me.boomRoutine = MyBase.StartCoroutine(Me.boomScale_cr())
	End Sub

	' Token: 0x060041A5 RID: 16805 RVA: 0x002386FC File Offset: 0x00236AFC
	Private Iterator Function boomScale_cr() As IEnumerator
		Dim t As Single = 0F
		Dim frameTime As Single = 0.041666668F
		Dim scale As Single = 1F
		While True
			t += CupheadTime.Delta
			While t > frameTime
				t -= frameTime
				scale *= 1.15F
				Me.boom.SetScale(New Single?(scale), New Single?(scale), Nothing)
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060041A6 RID: 16806 RVA: 0x00238717 File Offset: 0x00236B17
	Public Sub Pause()
		If Me.boomRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.boomRoutine)
		End If
	End Sub

	' Token: 0x060041A7 RID: 16807 RVA: 0x00238730 File Offset: 0x00236B30
	Private Sub HandleInput()
		Dim trilean As Trilean = 0
		Dim trilean2 As Trilean = 0
		Dim num As Single = 0F
		If Me.player IsNot Nothing Then
			num = Me.player.input.actions.GetAxis(1)
		End If
		If num > 0.35F OrElse num < -0.35F Then
			trilean2 = num
		End If
		Me.curAngle += trilean2 * WeaponProperties.PlaneSuperChaliceSuperBomb.turnRate
		Me.curAngle = Mathf.Clamp(Me.curAngle, -WeaponProperties.PlaneSuperChaliceSuperBomb.maxAngle, WeaponProperties.PlaneSuperChaliceSuperBomb.maxAngle)
		Me.curAngle *= WeaponProperties.PlaneSuperChaliceSuperBomb.angleDamp
		Me.accelDirection = MathUtils.AngleToDirection(Me.curAngle)
		MyBase.animator.SetInteger("Y", trilean2)
	End Sub

	' Token: 0x060041A8 RID: 16808 RVA: 0x00238804 File Offset: 0x00236C04
	Private Sub Move()
		Dim vector As Vector2 = MyBase.transform.position
		Me.moveDirection = Me.accelDirection * Me.curSpeed
		Me.curSpeed += WeaponProperties.PlaneSuperChaliceSuperBomb.accel * CupheadTime.FixedDelta
		MyBase.transform.AddPosition(Me.moveDirection.x * CupheadTime.FixedDelta, Me.moveDirection.y * CupheadTime.FixedDelta, 0F)
		Dim vector2 As Vector2 = MyBase.transform.position
		Me._velocity = (vector2 - vector) / CupheadTime.FixedDelta
	End Sub

	' Token: 0x060041A9 RID: 16809 RVA: 0x002388AC File Offset: 0x00236CAC
	Private Sub ClampPosition()
		Dim vector As Vector2 = MyBase.transform.position
		vector.x = Mathf.Clamp(vector.x, CSng(Level.Current.Left), CSng(Level.Current.Right) - 30F)
		vector.y = Mathf.Clamp(vector.y, CSng(Level.Current.Ground), CSng(Level.Current.Ceiling))
		If MyBase.transform.position <> vector Then
			Me.exploded = True
		End If
	End Sub

	' Token: 0x060041AA RID: 16810 RVA: 0x00238944 File Offset: 0x00236D44
	Private Sub CheckPosition()
		Dim vector As Vector2 = MyBase.transform.position
		vector.x = Mathf.Clamp(vector.x, CSng(Level.Current.Left) - 350F, CSng(Level.Current.Right) + 150F)
		vector.y = Mathf.Clamp(vector.y, CSng(Level.Current.Ground) - 175F, CSng(Level.Current.Ceiling) + 325F)
		If MyBase.transform.position <> vector Then
			Me.missed = True
		End If
	End Sub

	' Token: 0x060041AB RID: 16811 RVA: 0x002389EE File Offset: 0x00236DEE
	Protected Overridable Sub RestoreAudio(Optional changePitch As Boolean = True)
		AudioManager.SnapshotReset(SceneLoader.SceneName, 2F)
		If changePitch Then
			AudioManager.ChangeBGMPitch(1F, 2F)
		End If
	End Sub

	' Token: 0x060041AC RID: 16812 RVA: 0x00238A14 File Offset: 0x00236E14
	Protected Overrides Sub OnDestroy()
		Me.RestoreAudio(True)
		MyBase.OnDestroy()
		If Me.player IsNot Nothing Then
			RemoveHandler Me.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
			RemoveHandler Me.player.stats.OnStoned, AddressOf Me.OnStoned
		End If
	End Sub

	' Token: 0x04004813 RID: 18451
	Private Const ANALOG_THRESHOLD As Single = 0.35F

	' Token: 0x04004814 RID: 18452
	Private Const PADDING_TOP As Single = 65F

	' Token: 0x04004815 RID: 18453
	Private Const PADDING_BOTTOM As Single = 35F

	' Token: 0x04004816 RID: 18454
	Private Const PADDING_LEFT As Single = 70F

	' Token: 0x04004817 RID: 18455
	Private Const PADDING_RIGHT As Single = 30F

	' Token: 0x04004818 RID: 18456
	Private superHappening As Boolean

	' Token: 0x04004819 RID: 18457
	Private invulnerable As Boolean

	' Token: 0x0400481A RID: 18458
	Private timer As Single

	' Token: 0x0400481B RID: 18459
	Private accelDirection As Vector2

	' Token: 0x0400481C RID: 18460
	Private moveDirection As Vector2

	' Token: 0x0400481D RID: 18461
	Private _velocity As Vector2

	' Token: 0x0400481E RID: 18462
	Private damageReceiver As DamageReceiver

	' Token: 0x0400481F RID: 18463
	Private exploded As Boolean

	' Token: 0x04004820 RID: 18464
	Private missed As Boolean

	' Token: 0x04004821 RID: 18465
	Private boomRoutine As Coroutine

	' Token: 0x04004822 RID: 18466
	<SerializeField()>
	Private boom As Transform

	' Token: 0x04004823 RID: 18467
	Private curAngle As Single

	' Token: 0x04004824 RID: 18468
	Private curSpeed As Single

	' Token: 0x04004825 RID: 18469
	Private respawnPos As Vector2
End Class
