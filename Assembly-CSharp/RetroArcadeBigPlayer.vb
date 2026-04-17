Imports System
Imports UnityEngine

' Token: 0x0200074C RID: 1868
Public Class RetroArcadeBigPlayer
	Inherits AbstractPausableComponent

	' Token: 0x060028B2 RID: 10418 RVA: 0x0017BD6C File Offset: 0x0017A16C
	Public Sub Init(player As ArcadePlayerController)
		Me.player = player
		If player Is Nothing Then
			MyBase.gameObject.SetActive(False)
			Return
		End If
		AddHandler player.motor.OnHitEvent, AddressOf Me.OnHit
		MyBase.animator.SetBool("IsCuphead", Me.isCuphead)
	End Sub

	' Token: 0x060028B3 RID: 10419 RVA: 0x0017BDC8 File Offset: 0x0017A1C8
	Private Sub FixedUpdate()
		If Me.player Is Nothing OrElse Not Me.trackingInputs Then
			Return
		End If
		MyBase.animator.SetBool("Dead", Me.player.IsDead)
		MyBase.animator.Update(Time.deltaTime)
		MyBase.animator.Update(0F)
		If MyBase.animator.GetCurrentAnimatorStateInfo(3).IsName("Idle") Then
			If Me.player.input.actions.GetButtonDown(3) Then
				MyBase.animator.SetTrigger("A")
			End If
			If Me.player.input.actions.GetButtonDown(2) Then
				MyBase.animator.SetTrigger("B")
			End If
			If Me.player.input.actions.GetButtonDown(7) Then
				MyBase.animator.SetTrigger("C")
			End If
			MyBase.animator.SetInteger("MoveX", Me.player.input.GetAxisInt(PlayerInput.Axis.X, False, False))
		Else
			MyBase.animator.Play("Idle", 2)
			MyBase.animator.Play("Idle", 1)
			MyBase.animator.SetInteger("MoveX", 0)
		End If
	End Sub

	' Token: 0x060028B4 RID: 10420 RVA: 0x0017BF27 File Offset: 0x0017A327
	Private Sub OnHit()
		MyBase.animator.SetTrigger("Hit")
	End Sub

	' Token: 0x060028B5 RID: 10421 RVA: 0x0017BF39 File Offset: 0x0017A339
	Public Sub LevelStart()
		Me.trackingInputs = True
	End Sub

	' Token: 0x060028B6 RID: 10422 RVA: 0x0017BF42 File Offset: 0x0017A342
	Public Sub OnVictory()
		If Me.player IsNot Nothing AndAlso Not Me.player.IsDead Then
			MyBase.animator.SetTrigger("Victory")
		End If
	End Sub

	' Token: 0x060028B7 RID: 10423 RVA: 0x0017BF75 File Offset: 0x0017A375
	Private Sub PlayButtonASound()
		AudioManager.Play("level_button_a")
		Me.emitAudioFromObject.Add("level_button_a")
	End Sub

	' Token: 0x060028B8 RID: 10424 RVA: 0x0017BF91 File Offset: 0x0017A391
	Private Sub PlayButtonBSound()
		AudioManager.Play("level_button_b")
		Me.emitAudioFromObject.Add("level_button_b")
	End Sub

	' Token: 0x060028B9 RID: 10425 RVA: 0x0017BFAD File Offset: 0x0017A3AD
	Private Sub PlayButtonCSound()
		AudioManager.Play("level_button_c")
		Me.emitAudioFromObject.Add("level_button_c")
	End Sub

	' Token: 0x0400318C RID: 12684
	Private Const BOIL_LAYER As Integer = 0

	' Token: 0x0400318D RID: 12685
	Private Const BUTTON_LAYER As Integer = 1

	' Token: 0x0400318E RID: 12686
	Private Const STICK_LAYER As Integer = 2

	' Token: 0x0400318F RID: 12687
	Private Const MAIN_LAYER As Integer = 3

	' Token: 0x04003190 RID: 12688
	Private player As ArcadePlayerController

	' Token: 0x04003191 RID: 12689
	<SerializeField()>
	Private isCuphead As Boolean

	' Token: 0x04003192 RID: 12690
	Private trackingInputs As Boolean
End Class
