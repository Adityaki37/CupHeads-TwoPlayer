Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000AAC RID: 2732
Public Class PlaneSuperBomb
	Inherits AbstractPlaneSuper

	' Token: 0x0600418E RID: 16782 RVA: 0x0023802C File Offset: 0x0023642C
	Protected Overrides Sub StartSuper()
		MyBase.StartSuper()
		AddHandler Me.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.player.stats.OnStoned, AddressOf Me.OnStoned
		If(Me.player.id = PlayerId.PlayerOne AndAlso Not PlayerManager.player1IsMugman) OrElse (Me.player.id = PlayerId.PlayerTwo AndAlso PlayerManager.player1IsMugman) Then
			Me.boom.gameObject.SetActive(True)
		Else
			Me.boomMM.gameObject.SetActive(True)
		End If
	End Sub

	' Token: 0x0600418F RID: 16783 RVA: 0x002380D4 File Offset: 0x002364D4
	Private Iterator Function super_cr() As IEnumerator
		Dim t As Single = 0F
		Me.damageDealer = New DamageDealer(WeaponProperties.PlaneSuperBomb.damage, WeaponProperties.PlaneSuperBomb.damageRate, DamageDealer.DamageSource.Super, False, True, True)
		Me.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier
		Me.damageDealer.PlayerId = Me.player.id
		Dim tracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Super)
		tracker.Add(Me.damageDealer)
		While t < WeaponProperties.PlaneSuperBomb.countdownTime AndAlso Not Me.earlyExplosion
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.Fire()
		If Me.player IsNot Nothing Then
			Me.player.PauseAll()
			Me.player.SetSpriteVisible(False)
			MyBase.transform.position = Me.player.transform.position
		Else
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		MyBase.animator.SetTrigger("Explode")
		AudioManager.[Stop]("player_plane_bomb_ticktock_loop")
		AudioManager.Play("player_plane_bomb_explosion")
		Return
	End Function

	' Token: 0x06004190 RID: 16784 RVA: 0x002380EF File Offset: 0x002364EF
	Private Sub OnStoned()
		Me.earlyExplosion = True
	End Sub

	' Token: 0x06004191 RID: 16785 RVA: 0x002380F8 File Offset: 0x002364F8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.earlyExplosion = True
	End Sub

	' Token: 0x06004192 RID: 16786 RVA: 0x00238101 File Offset: 0x00236501
	Private Sub EndIntroAnimation()
		Me.StartCountdown()
		AudioManager.PlayLoop("player_plane_bomb_ticktock_loop")
		MyBase.StartCoroutine(Me.super_cr())
	End Sub

	' Token: 0x06004193 RID: 16787 RVA: 0x00238120 File Offset: 0x00236520
	Private Sub PlayerReappear()
		If Me.player IsNot Nothing Then
			Me.player.UnpauseAll(False)
			Me.player.SetSpriteVisible(True)
		End If
	End Sub

	' Token: 0x06004194 RID: 16788 RVA: 0x0023814B File Offset: 0x0023654B
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06004195 RID: 16789 RVA: 0x00238158 File Offset: 0x00236558
	Private Sub StartBoomScale()
		Me.boomRoutine = MyBase.StartCoroutine(Me.boomScale_cr())
	End Sub

	' Token: 0x06004196 RID: 16790 RVA: 0x0023816C File Offset: 0x0023656C
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
				Me.boomMM.SetScale(New Single?(scale), New Single?(scale), Nothing)
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06004197 RID: 16791 RVA: 0x00238187 File Offset: 0x00236587
	Public Sub Pause()
		If Me.boomRoutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.boomRoutine)
		End If
	End Sub

	' Token: 0x06004198 RID: 16792 RVA: 0x002381A0 File Offset: 0x002365A0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.player IsNot Nothing Then
			RemoveHandler Me.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
			RemoveHandler Me.player.stats.OnStoned, AddressOf Me.OnStoned
		End If
	End Sub

	' Token: 0x06004199 RID: 16793 RVA: 0x002381FC File Offset: 0x002365FC
	Private Sub PlaneSuperBombLaughAudio()
		AudioManager.Play("player_plane_bomb_laugh")
	End Sub

	' Token: 0x0400480F RID: 18447
	Private earlyExplosion As Boolean

	' Token: 0x04004810 RID: 18448
	Private boomRoutine As Coroutine

	' Token: 0x04004811 RID: 18449
	<SerializeField()>
	Private boom As Transform

	' Token: 0x04004812 RID: 18450
	<SerializeField()>
	Private boomMM As Transform
End Class
