Imports System
Imports UnityEngine

' Token: 0x020009DD RID: 2525
Public Class ArcadePlayerController
	Inherits AbstractPlayerController

	' Token: 0x170004F2 RID: 1266
	' (get) Token: 0x06003B82 RID: 15234 RVA: 0x00215347 File Offset: 0x00213747
	Public ReadOnly Property motor As ArcadePlayerMotor
		Get
			If Me._motor Is Nothing Then
				Me._motor = MyBase.GetComponent(Of ArcadePlayerMotor)()
			End If
			Return Me._motor
		End Get
	End Property

	' Token: 0x170004F3 RID: 1267
	' (get) Token: 0x06003B83 RID: 15235 RVA: 0x0021536C File Offset: 0x0021376C
	Public ReadOnly Property animationController As ArcadePlayerAnimationController
		Get
			If Me._animationController Is Nothing Then
				Me._animationController = MyBase.GetComponent(Of ArcadePlayerAnimationController)()
			End If
			Return Me._animationController
		End Get
	End Property

	' Token: 0x170004F4 RID: 1268
	' (get) Token: 0x06003B84 RID: 15236 RVA: 0x00215391 File Offset: 0x00213791
	Public ReadOnly Property weaponManager As ArcadePlayerWeaponManager
		Get
			If Me._weaponManager Is Nothing Then
				Me._weaponManager = MyBase.GetComponent(Of ArcadePlayerWeaponManager)()
			End If
			Return Me._weaponManager
		End Get
	End Property

	' Token: 0x170004F5 RID: 1269
	' (get) Token: 0x06003B85 RID: 15237 RVA: 0x002153B6 File Offset: 0x002137B6
	Public ReadOnly Property parryController As ArcadePlayerParryController
		Get
			If Me._parryController Is Nothing Then
				Me._parryController = MyBase.GetComponent(Of ArcadePlayerParryController)()
			End If
			Return Me._parryController
		End Get
	End Property

	' Token: 0x170004F6 RID: 1270
	' (get) Token: 0x06003B86 RID: 15238 RVA: 0x002153DB File Offset: 0x002137DB
	Public ReadOnly Property colliderManager As ArcadePlayerColliderManager
		Get
			If Me._colliderManager Is Nothing Then
				Me._colliderManager = MyBase.GetComponent(Of ArcadePlayerColliderManager)()
			End If
			Return Me._colliderManager
		End Get
	End Property

	' Token: 0x170004F7 RID: 1271
	' (get) Token: 0x06003B87 RID: 15239 RVA: 0x00215400 File Offset: 0x00213800
	' (set) Token: 0x06003B88 RID: 15240 RVA: 0x00215408 File Offset: 0x00213808
	Public Property controlScheme As ArcadePlayerController.ControlScheme

	' Token: 0x170004F8 RID: 1272
	' (get) Token: 0x06003B89 RID: 15241 RVA: 0x00215414 File Offset: 0x00213814
	Public Overrides ReadOnly Property CanTakeDamage As Boolean
		Get
			Return MyBase.damageReceiver.state = PlayerDamageReceiver.State.Vulnerable AndAlso ((MyBase.stats.Loadout.charm <> Charm.charm_smoke_dash AndAlso Not MyBase.stats.CurseSmokeDash) OrElse Not Me.motor.Dashing)
		End Get
	End Property

	' Token: 0x06003B8A RID: 15242 RVA: 0x00215470 File Offset: 0x00213870
	Private Sub Start()
		Me.controlScheme = ArcadePlayerController.ControlScheme.Normal
	End Sub

	' Token: 0x06003B8B RID: 15243 RVA: 0x00215479 File Offset: 0x00213879
	Public Sub ChangeToRocket()
		Me.controlScheme = ArcadePlayerController.ControlScheme.Rocket
		Me.weaponManager.ChangeToRocket()
		Me.animationController.ChangeToRocket()
	End Sub

	' Token: 0x06003B8C RID: 15244 RVA: 0x00215498 File Offset: 0x00213898
	Public Sub ChangeToJetpack()
		Me.controlScheme = ArcadePlayerController.ControlScheme.Jetpack
		Me.weaponManager.ChangeToJetPack()
		Me.animationController.ChangeToJetpack()
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
	End Sub

	' Token: 0x06003B8D RID: 15245 RVA: 0x002154EC File Offset: 0x002138EC
	Public Sub PauseAll()
		For Each abstractPausableComponent As AbstractPausableComponent In MyBase.GetComponents(Of AbstractPausableComponent)()
			abstractPausableComponent.enabled = False
		Next
	End Sub

	' Token: 0x06003B8E RID: 15246 RVA: 0x00215520 File Offset: 0x00213920
	Public Sub UnpauseAll(Optional forced As Boolean = False)
		For Each abstractPausableComponent As AbstractPausableComponent In MyBase.GetComponents(Of AbstractPausableComponent)()
			If forced Then
				abstractPausableComponent.preEnabled = True
			End If
			abstractPausableComponent.enabled = True
		Next
	End Sub

	' Token: 0x06003B8F RID: 15247 RVA: 0x00215560 File Offset: 0x00213960
	Protected Overrides Sub LevelInit(id As PlayerId)
		MyBase.LevelInit(id)
		Me.animationController.LevelInit()
		Me.weaponManager.LevelInit(id)
	End Sub

	' Token: 0x06003B90 RID: 15248 RVA: 0x00215580 File Offset: 0x00213980
	Protected Overrides Sub OnDeath(playerId As PlayerId)
		MyBase.OnDeath(MyBase.id)
		Dim playerDeathEffect As PlayerDeathEffect = Me.deathEffect.Create(MyBase.id, MyBase.input, MyBase.transform.position, MyBase.stats.Deaths, PlayerMode.Level, True)
		AddHandler playerDeathEffect.OnPreReviveEvent, AddressOf Me.OnPreRevive
		AddHandler playerDeathEffect.OnReviveEvent, AddressOf Me.OnRevive
	End Sub

	' Token: 0x06003B91 RID: 15249 RVA: 0x002155F4 File Offset: 0x002139F4
	Public Sub DisableInput()
		Me.motor.DisableInput()
		Me.weaponManager.DisableInput()
	End Sub

	' Token: 0x06003B92 RID: 15250 RVA: 0x0021560C File Offset: 0x00213A0C
	Public Sub OnLevelWinPause()
		Me.PauseAll()
		MyBase.collider.enabled = False
	End Sub

	' Token: 0x06003B93 RID: 15251 RVA: 0x00215620 File Offset: 0x00213A20
	Public Overrides Sub OnLevelWin()
		Me.UnpauseAll(False)
		Me.weaponManager.DisableInput()
		MyBase.collider.enabled = False
	End Sub

	' Token: 0x06003B94 RID: 15252 RVA: 0x00215640 File Offset: 0x00213A40
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Application.isPlaying Then
			Gizmos.DrawCube(Me.CameraCenter, Vector3.one * 50F)
		End If
	End Sub

	' Token: 0x06003B95 RID: 15253 RVA: 0x0021566C File Offset: 0x00213A6C
	Public Overrides Sub BufferInputs()
		MyBase.BufferInputs()
		Me.motor.BufferInputs()
	End Sub

	' Token: 0x04004311 RID: 17169
	Private initialized As Boolean

	' Token: 0x04004312 RID: 17170
	Private _motor As ArcadePlayerMotor

	' Token: 0x04004313 RID: 17171
	Private _animationController As ArcadePlayerAnimationController

	' Token: 0x04004314 RID: 17172
	Private _weaponManager As ArcadePlayerWeaponManager

	' Token: 0x04004315 RID: 17173
	Private _parryController As ArcadePlayerParryController

	' Token: 0x04004316 RID: 17174
	Private _colliderManager As ArcadePlayerColliderManager

	' Token: 0x04004317 RID: 17175
	<SerializeField()>
	Private deathEffect As PlayerDeathEffect

	' Token: 0x020009DE RID: 2526
	Public Enum ControlScheme
		' Token: 0x0400431A RID: 17178
		Normal
		' Token: 0x0400431B RID: 17179
		Rocket
		' Token: 0x0400431C RID: 17180
		Jetpack
	End Enum
End Class
