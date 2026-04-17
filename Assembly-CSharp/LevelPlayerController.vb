Imports System
Imports UnityEngine

' Token: 0x02000A1D RID: 2589
Public Class LevelPlayerController
	Inherits AbstractPlayerController

	' Token: 0x17000531 RID: 1329
	' (get) Token: 0x06003D57 RID: 15703 RVA: 0x0021E92B File Offset: 0x0021CD2B
	Public ReadOnly Property Ducking As Boolean
		Get
			Return Me.motor.Ducking
		End Get
	End Property

	' Token: 0x17000532 RID: 1330
	' (get) Token: 0x06003D58 RID: 15704 RVA: 0x0021E938 File Offset: 0x0021CD38
	Public ReadOnly Property motor As LevelPlayerMotor
		Get
			If Me._motor Is Nothing Then
				Me._motor = MyBase.GetComponent(Of LevelPlayerMotor)()
			End If
			Return Me._motor
		End Get
	End Property

	' Token: 0x17000533 RID: 1331
	' (get) Token: 0x06003D59 RID: 15705 RVA: 0x0021E95D File Offset: 0x0021CD5D
	Public ReadOnly Property animationController As LevelPlayerAnimationController
		Get
			If Me._animationController Is Nothing Then
				Me._animationController = MyBase.GetComponent(Of LevelPlayerAnimationController)()
			End If
			Return Me._animationController
		End Get
	End Property

	' Token: 0x17000534 RID: 1332
	' (get) Token: 0x06003D5A RID: 15706 RVA: 0x0021E982 File Offset: 0x0021CD82
	Public ReadOnly Property weaponManager As LevelPlayerWeaponManager
		Get
			If Me._weaponManager Is Nothing Then
				Me._weaponManager = MyBase.GetComponent(Of LevelPlayerWeaponManager)()
			End If
			Return Me._weaponManager
		End Get
	End Property

	' Token: 0x17000535 RID: 1333
	' (get) Token: 0x06003D5B RID: 15707 RVA: 0x0021E9A7 File Offset: 0x0021CDA7
	Public ReadOnly Property parryController As LevelPlayerParryController
		Get
			If Me._parryController Is Nothing Then
				Me._parryController = MyBase.GetComponent(Of LevelPlayerParryController)()
			End If
			Return Me._parryController
		End Get
	End Property

	' Token: 0x17000536 RID: 1334
	' (get) Token: 0x06003D5C RID: 15708 RVA: 0x0021E9CC File Offset: 0x0021CDCC
	Public ReadOnly Property colliderManager As LevelPlayerColliderManager
		Get
			If Me._colliderManager Is Nothing Then
				Me._colliderManager = MyBase.GetComponent(Of LevelPlayerColliderManager)()
			End If
			Return Me._colliderManager
		End Get
	End Property

	' Token: 0x17000537 RID: 1335
	' (get) Token: 0x06003D5D RID: 15709 RVA: 0x0021E9F4 File Offset: 0x0021CDF4
	Public Overrides ReadOnly Property center As Vector3
		Get
			If MyBase.transform Is Nothing Then
				Return Vector3.zero
			End If
			Return MyBase.transform.position + New Vector3(MyBase.collider.offset.x, MyBase.collider.offset.y * Me.motor.GravityReversalMultiplier, 0F)
		End Get
	End Property

	' Token: 0x17000538 RID: 1336
	' (get) Token: 0x06003D5E RID: 15710 RVA: 0x0021EA64 File Offset: 0x0021CE64
	Public Overrides ReadOnly Property CanTakeDamage As Boolean
		Get
			Return MyBase.damageReceiver.state = PlayerDamageReceiver.State.Vulnerable AndAlso ((MyBase.stats.Loadout.charm <> Charm.charm_smoke_dash AndAlso Not MyBase.stats.CurseSmokeDash) OrElse Level.IsChessBoss OrElse Not Me.motor.Dashing) AndAlso (Not MyBase.stats.isChalice OrElse Not Me.motor.Dashing OrElse Not Me.motor.ChaliceDuckDashed) AndAlso (Not MyBase.stats.isChalice OrElse Not Me.motor.Dashing OrElse True)
		End Get
	End Property

	' Token: 0x17000539 RID: 1337
	' (get) Token: 0x06003D5F RID: 15711 RVA: 0x0021EB20 File Offset: 0x0021CF20
	Public Overrides ReadOnly Property CameraCenter As Vector3
		Get
			If Level.Current.LevelType = Level.Type.Platforming Then
				Me.cameraCenterPosition = Mathf.Lerp(Me.cameraCenterPosition, 250F * CSng(Me._motor.TrueLookDirection.x.Value), 1.2F * CupheadTime.Delta)
				Return Me.center + New Vector3(Me.cameraCenterPosition, 0F)
			End If
			Return MyBase.CameraCenter
		End Get
	End Property

	' Token: 0x06003D60 RID: 15712 RVA: 0x0021EBA0 File Offset: 0x0021CFA0
	Public Sub PauseAll()
		For Each abstractPausableComponent As AbstractPausableComponent In MyBase.GetComponents(Of AbstractPausableComponent)()
			abstractPausableComponent.enabled = False
		Next
	End Sub

	' Token: 0x06003D61 RID: 15713 RVA: 0x0021EBD4 File Offset: 0x0021CFD4
	Public Sub UnpauseAll(Optional forced As Boolean = False)
		For Each abstractPausableComponent As AbstractPausableComponent In MyBase.GetComponents(Of AbstractPausableComponent)()
			If forced Then
				abstractPausableComponent.preEnabled = True
			End If
			abstractPausableComponent.enabled = True
		Next
	End Sub

	' Token: 0x06003D62 RID: 15714 RVA: 0x0021EC14 File Offset: 0x0021D014
	Public Sub OnPitKnockUp(y As Single, Optional velocityScale As Single = 1F)
		If MyBase.damageReceiver.state = PlayerDamageReceiver.State.Vulnerable AndAlso MyBase.stats.Loadout.charm <> Charm.charm_float Then
			MyBase.stats.OnPitKnockUp()
		End If
		Me.motor.OnPitKnockUp(y, velocityScale)
	End Sub

	' Token: 0x06003D63 RID: 15715 RVA: 0x0021EC63 File Offset: 0x0021D063
	Protected Overrides Sub LevelInit(id As PlayerId)
		MyBase.LevelInit(id)
		Me.animationController.LevelInit()
		Me.weaponManager.LevelInit(id)
		If MyBase.stats.Health = 0 Then
			Me.StartDead()
		End If
	End Sub

	' Token: 0x06003D64 RID: 15716 RVA: 0x0021EC9C File Offset: 0x0021D09C
	Protected Overrides Sub OnDeath(playerId As PlayerId)
		MyBase.OnDeath(MyBase.id)
		Dim position As Vector3 = MyBase.transform.position
		If Me.motor.GravityReversed Then
			position.y += (Me.center.y - MyBase.transform.position.y) * 2F
		End If
		Dim playerDeathEffect As PlayerDeathEffect = Me.deathEffect.Create(MyBase.id, MyBase.input, position, MyBase.stats.Deaths, PlayerMode.Level, True)
		AddHandler playerDeathEffect.OnPreReviveEvent, AddressOf Me.OnPreRevive
		AddHandler playerDeathEffect.OnReviveEvent, AddressOf Me.OnRevive
		If PauseManager.state = PauseManager.State.Paused Then
			PauseManager.Unpause()
		End If
		Me.weaponManager.OnDeath()
	End Sub

	' Token: 0x06003D65 RID: 15717 RVA: 0x0021ED74 File Offset: 0x0021D174
	Public Overrides Sub OnLeave(playerId As PlayerId)
		If Not MyBase.IsDead Then
			Dim position As Vector3 = MyBase.transform.position
			If Me.motor.GravityReversed Then
				position.y += (Me.center.y - MyBase.transform.position.y) * 2F
			End If
			Me.deathEffect.CreateExplosionOnly(playerId, position, PlayerMode.Level)
		End If
		MyBase.OnLeave(playerId)
	End Sub

	' Token: 0x06003D66 RID: 15718 RVA: 0x0021EDF8 File Offset: 0x0021D1F8
	Private Sub StartDead()
		MyBase.gameObject.SetActive(False)
		Dim position As Vector3 = MyBase.transform.position
		position.y += 1000F
		Dim playerDeathEffect As PlayerDeathEffect = Me.deathEffect.Create(MyBase.id, MyBase.input, position, MyBase.stats.Deaths, PlayerMode.Level, True)
		AddHandler playerDeathEffect.OnPreReviveEvent, AddressOf Me.OnPreRevive
		AddHandler playerDeathEffect.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x06003D67 RID: 15719 RVA: 0x0021EE81 File Offset: 0x0021D281
	Public Sub DisableInput()
		Me.motor.DisableInput()
		Me.weaponManager.DisableInput()
		AudioManager.[Stop]("player_default_fire_loop")
	End Sub

	' Token: 0x06003D68 RID: 15720 RVA: 0x0021EEA3 File Offset: 0x0021D2A3
	Public Sub EnableInput()
		Me.motor.EnableInput()
		Me.weaponManager.EnableInput()
	End Sub

	' Token: 0x06003D69 RID: 15721 RVA: 0x0021EEBB File Offset: 0x0021D2BB
	Public Overrides Sub BufferInputs()
		MyBase.BufferInputs()
		Me.motor.BufferInputs()
	End Sub

	' Token: 0x06003D6A RID: 15722 RVA: 0x0021EECE File Offset: 0x0021D2CE
	Public Sub OnLevelWinPause()
		Me.PauseAll()
		MyBase.collider.enabled = False
	End Sub

	' Token: 0x06003D6B RID: 15723 RVA: 0x0021EEE4 File Offset: 0x0021D2E4
	Public Overrides Sub OnLevelWin()
		Me.UnpauseAll(False)
		Me.weaponManager.DisableInput()
		MyBase.collider.enabled = False
		AudioManager.[Stop]("player_default_fire_loop")
		If Level.Current.LevelType = Level.Type.Platforming Then
			Me.motor.OnPlatformingLevelExit()
		End If
	End Sub

	' Token: 0x06003D6C RID: 15724 RVA: 0x0021EF34 File Offset: 0x0021D334
	Public Sub ReverseControls(reverseTime As Single)
		MyBase.stats.ReverseControls(reverseTime)
	End Sub

	' Token: 0x06003D6D RID: 15725 RVA: 0x0021EF42 File Offset: 0x0021D342
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Application.isPlaying Then
			Gizmos.DrawCube(Me.CameraCenter, Vector3.one * 50F)
		End If
	End Sub

	' Token: 0x06003D6E RID: 15726 RVA: 0x0021EF6E File Offset: 0x0021D36E
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.deathEffect = Nothing
	End Sub

	' Token: 0x040044A5 RID: 17573
	Private Const PLATFORMING_CAMERA_DISTANCE_RUNNING As Single = 250F

	' Token: 0x040044A6 RID: 17574
	Private Const PLATFORMING_CAMERA_DISTANCE_STATIC As Single = 50F

	' Token: 0x040044A7 RID: 17575
	Private Const PLATFORMING_CAMERA_TIME_RUNNING As Single = 1.2F

	' Token: 0x040044A8 RID: 17576
	Private Const PLATFORMING_CAMERA_TIME_STATIC As Single = 6F

	' Token: 0x040044A9 RID: 17577
	Private initialized As Boolean

	' Token: 0x040044AA RID: 17578
	Private _motor As LevelPlayerMotor

	' Token: 0x040044AB RID: 17579
	Private _animationController As LevelPlayerAnimationController

	' Token: 0x040044AC RID: 17580
	Private _weaponManager As LevelPlayerWeaponManager

	' Token: 0x040044AD RID: 17581
	Private _parryController As LevelPlayerParryController

	' Token: 0x040044AE RID: 17582
	Private _colliderManager As LevelPlayerColliderManager

	' Token: 0x040044AF RID: 17583
	<SerializeField()>
	Private deathEffect As PlayerDeathEffect

	' Token: 0x040044B0 RID: 17584
	Private cameraCenterPosition As Single
End Class
