Imports System
Imports UnityEngine

' Token: 0x02000A99 RID: 2713
Public Class PlanePlayerController
	Inherits AbstractPlayerController

	' Token: 0x170005A6 RID: 1446
	' (get) Token: 0x060040F7 RID: 16631 RVA: 0x0023541D File Offset: 0x0023381D
	Public ReadOnly Property Shrunk As Boolean
		Get
			Return Me.animationController.ShrinkState = PlanePlayerAnimationController.ShrinkStates.Shrunk
		End Get
	End Property

	' Token: 0x170005A7 RID: 1447
	' (get) Token: 0x060040F8 RID: 16632 RVA: 0x0023542D File Offset: 0x0023382D
	Public ReadOnly Property Parrying As Boolean
		Get
			Return Me.parryController.State = PlanePlayerParryController.ParryState.Parrying
		End Get
	End Property

	' Token: 0x170005A8 RID: 1448
	' (get) Token: 0x060040F9 RID: 16633 RVA: 0x0023543D File Offset: 0x0023383D
	Public ReadOnly Property WeaponBusy As Boolean
		Get
			Return Me.weaponManager.state <> PlanePlayerWeaponManager.State.Ready OrElse Not Me.weaponManager.CanInterupt
		End Get
	End Property

	' Token: 0x170005A9 RID: 1449
	' (get) Token: 0x060040FA RID: 16634 RVA: 0x00235461 File Offset: 0x00233861
	Public ReadOnly Property motor As PlanePlayerMotor
		Get
			If Me._motor Is Nothing Then
				Me._motor = MyBase.GetComponent(Of PlanePlayerMotor)()
			End If
			Return Me._motor
		End Get
	End Property

	' Token: 0x170005AA RID: 1450
	' (get) Token: 0x060040FB RID: 16635 RVA: 0x00235486 File Offset: 0x00233886
	Public ReadOnly Property animationController As PlanePlayerAnimationController
		Get
			If Me._animationController Is Nothing Then
				Me._animationController = MyBase.GetComponent(Of PlanePlayerAnimationController)()
			End If
			Return Me._animationController
		End Get
	End Property

	' Token: 0x170005AB RID: 1451
	' (get) Token: 0x060040FC RID: 16636 RVA: 0x002354AB File Offset: 0x002338AB
	Public ReadOnly Property audioController As PlanePlayerAudioController
		Get
			If Me._audioController Is Nothing Then
				Me._audioController = MyBase.GetComponent(Of PlanePlayerAudioController)()
			End If
			Return Me._audioController
		End Get
	End Property

	' Token: 0x170005AC RID: 1452
	' (get) Token: 0x060040FD RID: 16637 RVA: 0x002354D0 File Offset: 0x002338D0
	Public ReadOnly Property weaponManager As PlanePlayerWeaponManager
		Get
			If Me._weaponManager Is Nothing Then
				Me._weaponManager = MyBase.GetComponent(Of PlanePlayerWeaponManager)()
			End If
			Return Me._weaponManager
		End Get
	End Property

	' Token: 0x170005AD RID: 1453
	' (get) Token: 0x060040FE RID: 16638 RVA: 0x002354F5 File Offset: 0x002338F5
	Public ReadOnly Property parryController As PlanePlayerParryController
		Get
			If Me._parryController Is Nothing Then
				Me._parryController = MyBase.GetComponent(Of PlanePlayerParryController)()
			End If
			Return Me._parryController
		End Get
	End Property

	' Token: 0x170005AE RID: 1454
	' (get) Token: 0x060040FF RID: 16639 RVA: 0x0023551C File Offset: 0x0023391C
	Public Overrides ReadOnly Property CanTakeDamage As Boolean
		Get
			Return MyBase.damageReceiver.state = PlayerDamageReceiver.State.Vulnerable AndAlso ((MyBase.stats.Loadout.charm <> Charm.charm_smoke_dash AndAlso Not MyBase.stats.CurseSmokeDash) OrElse Not Me.animationController.Shrinking)
		End Get
	End Property

	' Token: 0x06004100 RID: 16640 RVA: 0x00235578 File Offset: 0x00233978
	Private Sub Start()
		If Not Level.Current.Started Then
			Me.motor.enabled = False
		End If
	End Sub

	' Token: 0x06004101 RID: 16641 RVA: 0x00235595 File Offset: 0x00233995
	Public Overrides Sub PlayIntro()
		MyBase.PlayIntro()
		Me.animationController.PlayIntro()
	End Sub

	' Token: 0x06004102 RID: 16642 RVA: 0x002355A8 File Offset: 0x002339A8
	Protected Overrides Sub LevelInit(id As PlayerId)
		MyBase.LevelInit(id)
		Me.animationController.LevelInit()
		Me.audioController.LevelInit()
		If MyBase.stats.Health = 0 Then
			Me.StartDead()
		End If
	End Sub

	' Token: 0x06004103 RID: 16643 RVA: 0x002355DD File Offset: 0x002339DD
	Public Overrides Sub LevelStart()
		MyBase.LevelStart()
		Me.motor.enabled = True
	End Sub

	' Token: 0x06004104 RID: 16644 RVA: 0x002355F1 File Offset: 0x002339F1
	Public Sub GetStoned(stoneTime As Single)
		MyBase.stats.GetStoned(stoneTime)
	End Sub

	' Token: 0x06004105 RID: 16645 RVA: 0x00235600 File Offset: 0x00233A00
	Protected Overrides Sub OnDeath(playerId As PlayerId)
		MyBase.OnDeath(MyBase.id)
		Dim playerDeathEffect As PlayerDeathEffect = Me.deathEffect.Create(MyBase.id, MyBase.input, MyBase.transform.position, MyBase.stats.Deaths, PlayerMode.Plane, True)
		AddHandler playerDeathEffect.OnPreReviveEvent, AddressOf Me.OnPreRevive
		AddHandler playerDeathEffect.OnReviveEvent, AddressOf Me.OnRevive
		If PauseManager.state = PauseManager.State.Paused Then
			PauseManager.Unpause()
		End If
	End Sub

	' Token: 0x06004106 RID: 16646 RVA: 0x00235684 File Offset: 0x00233A84
	Public Overrides Sub OnLeave(playerId As PlayerId)
		If Not MyBase.IsDead Then
			Me.deathEffect.CreateExplosionOnly(MyBase.id, MyBase.transform.position, PlayerMode.Plane)
		End If
		MyBase.OnLeave(playerId)
	End Sub

	' Token: 0x06004107 RID: 16647 RVA: 0x002356BC File Offset: 0x00233ABC
	Private Sub StartDead()
		MyBase.gameObject.SetActive(False)
		Dim position As Vector3 = MyBase.transform.position
		position.y += 1000F
		Dim playerDeathEffect As PlayerDeathEffect = Me.deathEffect.Create(MyBase.id, MyBase.input, position, MyBase.stats.Deaths, PlayerMode.Plane, True)
		AddHandler playerDeathEffect.OnPreReviveEvent, AddressOf Me.OnPreRevive
		AddHandler playerDeathEffect.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x06004108 RID: 16648 RVA: 0x00235748 File Offset: 0x00233B48
	Public Sub PauseAll()
		For Each abstractPausableComponent As AbstractPausableComponent In MyBase.GetComponents(Of AbstractPausableComponent)()
			abstractPausableComponent.enabled = False
		Next
	End Sub

	' Token: 0x06004109 RID: 16649 RVA: 0x0023577C File Offset: 0x00233B7C
	Public Sub UnpauseAll(Optional forced As Boolean = False)
		For Each abstractPausableComponent As AbstractPausableComponent In MyBase.GetComponents(Of AbstractPausableComponent)()
			If forced Then
				abstractPausableComponent.preEnabled = True
			End If
			abstractPausableComponent.enabled = True
		Next
	End Sub

	' Token: 0x0600410A RID: 16650 RVA: 0x002357BC File Offset: 0x00233BBC
	Public Sub SetSpriteVisible(visibility As Boolean)
		Me.animationController.SetSpriteVisible(visibility)
	End Sub

	' Token: 0x0600410B RID: 16651 RVA: 0x002357CA File Offset: 0x00233BCA
	Public Overrides Sub BufferInputs()
		MyBase.BufferInputs()
		Me.motor.BufferInputs()
	End Sub

	' Token: 0x0400479E RID: 18334
	Public Const INTRO_TIME As Single = 1F

	' Token: 0x0400479F RID: 18335
	Private _motor As PlanePlayerMotor

	' Token: 0x040047A0 RID: 18336
	Private _animationController As PlanePlayerAnimationController

	' Token: 0x040047A1 RID: 18337
	Private _audioController As PlanePlayerAudioController

	' Token: 0x040047A2 RID: 18338
	Private _weaponManager As PlanePlayerWeaponManager

	' Token: 0x040047A3 RID: 18339
	Private _parryController As PlanePlayerParryController

	' Token: 0x040047A4 RID: 18340
	<SerializeField()>
	Private deathEffect As PlayerDeathEffect
End Class
