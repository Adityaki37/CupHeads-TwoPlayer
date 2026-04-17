Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000AA0 RID: 2720
Public Class PlanePlayerParryController
	Inherits AbstractPlanePlayerComponent
	Implements IParryAttack

	' Token: 0x170005B2 RID: 1458
	' (get) Token: 0x06004132 RID: 16690 RVA: 0x00236599 File Offset: 0x00234999
	' (set) Token: 0x06004133 RID: 16691 RVA: 0x002365A1 File Offset: 0x002349A1
	Public Property State As PlanePlayerParryController.ParryState
		Get
			Return Me.state
		End Get
		Set(value As PlanePlayerParryController.ParryState)
			Me.state = value
		End Set
	End Property

	' Token: 0x140000A2 RID: 162
	' (add) Token: 0x06004134 RID: 16692 RVA: 0x002365AC File Offset: 0x002349AC
	' (remove) Token: 0x06004135 RID: 16693 RVA: 0x002365E4 File Offset: 0x002349E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParryStartEvent As Action

	' Token: 0x140000A3 RID: 163
	' (add) Token: 0x06004136 RID: 16694 RVA: 0x0023661C File Offset: 0x00234A1C
	' (remove) Token: 0x06004137 RID: 16695 RVA: 0x00236654 File Offset: 0x00234A54
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnParrySuccessEvent As Action

	' Token: 0x170005B3 RID: 1459
	' (get) Token: 0x06004138 RID: 16696 RVA: 0x0023668A File Offset: 0x00234A8A
	' (set) Token: 0x06004139 RID: 16697 RVA: 0x00236692 File Offset: 0x00234A92
	Public Property AttackParryUsed As Boolean Implements IParryAttack.AttackParryUsed

	' Token: 0x170005B4 RID: 1460
	' (get) Token: 0x0600413A RID: 16698 RVA: 0x0023669B File Offset: 0x00234A9B
	' (set) Token: 0x0600413B RID: 16699 RVA: 0x002366A3 File Offset: 0x00234AA3
	Public Property HasHitEnemy As Boolean Implements IParryAttack.HasHitEnemy

	' Token: 0x0600413C RID: 16700 RVA: 0x002366AC File Offset: 0x00234AAC
	Private Sub Start()
		AddHandler MyBase.player.OnReviveEvent, AddressOf Me.OnRevive
		AddHandler MyBase.player.stats.OnStoned, AddressOf Me.OnStoned
	End Sub

	' Token: 0x0600413D RID: 16701 RVA: 0x002366E4 File Offset: 0x00234AE4
	Private Sub FixedUpdate()
		Dim parryState As PlanePlayerParryController.ParryState = Me.state
		If parryState <> PlanePlayerParryController.ParryState.Ready Then
			If parryState = PlanePlayerParryController.ParryState.Cooldown Then
				Me.UpdateCooldown()
			End If
		Else
			Me.UpdateReady()
		End If
	End Sub

	' Token: 0x0600413E RID: 16702 RVA: 0x00236728 File Offset: 0x00234B28
	Private Sub UpdateReady()
		If MyBase.player.Shrunk OrElse MyBase.player.WeaponBusy OrElse MyBase.player.stats.StoneTime > 0F Then
			Return
		End If
		If Me.state <> PlanePlayerParryController.ParryState.Ready Then
			Return
		End If
		If MyBase.player.input.actions.GetButtonDown(2) OrElse MyBase.player.motor.HasBufferedInput(PlanePlayerMotor.BufferedInput.Jump) Then
			MyBase.player.motor.ClearBufferedInput()
			Me.StartParry()
		End If
	End Sub

	' Token: 0x0600413F RID: 16703 RVA: 0x002367C4 File Offset: 0x00234BC4
	Private Sub UpdateCooldown()
		Me.timeSinceParry += CupheadTime.FixedDelta
		If Me.timeSinceParry > 0.3F Then
			Me.state = PlanePlayerParryController.ParryState.Ready
			Me.AttackParryUsed = False
		End If
	End Sub

	' Token: 0x06004140 RID: 16704 RVA: 0x002367F6 File Offset: 0x00234BF6
	Public Overrides Sub OnLevelStart()
		MyBase.OnLevelStart()
		Me.state = PlanePlayerParryController.ParryState.Ready
	End Sub

	' Token: 0x06004141 RID: 16705 RVA: 0x00236808 File Offset: 0x00234C08
	Private Sub StartParry()
		If Me.state <> PlanePlayerParryController.ParryState.Ready Then
			Return
		End If
		Me.state = PlanePlayerParryController.ParryState.Parrying
		If Me.OnParryStartEvent IsNot Nothing Then
			Me.OnParryStartEvent()
		End If
		Me.effectInstance = TryCast(Me.effect.Create(MyBase.player), PlanePlayerParryEffect)
		If MyBase.player.stats.isChalice Then
			Me.effectInstance.GetComponent(Of CircleCollider2D)().radius *= 1.3F
		End If
	End Sub

	' Token: 0x06004142 RID: 16706 RVA: 0x0023688C File Offset: 0x00234C8C
	Public Sub OnParrySuccess()
		If Me.OnParrySuccessEvent IsNot Nothing Then
			Me.OnParrySuccessEvent()
		End If
		Me.state = PlanePlayerParryController.ParryState.Cooldown
		Me.timeSinceParry = 0F
		If Me.effectInstance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.effectInstance.gameObject)
		End If
	End Sub

	' Token: 0x06004143 RID: 16707 RVA: 0x002368E2 File Offset: 0x00234CE2
	Private Sub OnParryEnd()
		Me.state = PlanePlayerParryController.ParryState.Cooldown
		Me.timeSinceParry = 0F
		Me.HasHitEnemy = False
		If Me.effectInstance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.effectInstance.gameObject)
		End If
	End Sub

	' Token: 0x06004144 RID: 16708 RVA: 0x0023691E File Offset: 0x00234D1E
	Private Sub OnRevive(pos As Vector3)
		Me.state = PlanePlayerParryController.ParryState.Ready
	End Sub

	' Token: 0x06004145 RID: 16709 RVA: 0x00236927 File Offset: 0x00234D27
	Private Sub OnStoned()
		Me.state = PlanePlayerParryController.ParryState.Ready
	End Sub

	' Token: 0x06004146 RID: 16710 RVA: 0x00236930 File Offset: 0x00234D30
	Private Sub SoundPlaneParry()
		AudioManager.Play("player_plane_parry")
		Me.emitAudioFromObject.Add("player_plane_parry")
	End Sub

	' Token: 0x040047CF RID: 18383
	Public Const COOLDOWN_DURATION As Single = 0.3F

	' Token: 0x040047D0 RID: 18384
	Public Const CHALICE_PARRY_SIZE_MODIFIER As Single = 1.3F

	' Token: 0x040047D1 RID: 18385
	Private state As PlanePlayerParryController.ParryState

	' Token: 0x040047D2 RID: 18386
	<SerializeField()>
	Private effect As PlanePlayerParryEffect

	' Token: 0x040047D5 RID: 18389
	Private effectInstance As PlanePlayerParryEffect

	' Token: 0x040047D6 RID: 18390
	Private timeSinceParry As Single

	' Token: 0x02000AA1 RID: 2721
	Public Enum ParryState
		' Token: 0x040047DA RID: 18394
		Init
		' Token: 0x040047DB RID: 18395
		Ready
		' Token: 0x040047DC RID: 18396
		Cooldown
		' Token: 0x040047DD RID: 18397
		Parrying
		' Token: 0x040047DE RID: 18398
		Disabled
	End Enum
End Class
