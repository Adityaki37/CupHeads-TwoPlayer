Imports System
Imports UnityEngine

' Token: 0x02000A37 RID: 2615
Public Class LevelPlayerParryEffect
	Inherits AbstractParryEffect

	' Token: 0x17000562 RID: 1378
	' (get) Token: 0x06003E34 RID: 15924 RVA: 0x00223969 File Offset: 0x00221D69
	Protected Overrides ReadOnly Property IsHit As Boolean
		Get
			Return TryCast(Me.player, LevelPlayerController).motor.IsHit
		End Get
	End Property

	' Token: 0x17000563 RID: 1379
	' (get) Token: 0x06003E35 RID: 15925 RVA: 0x00223980 File Offset: 0x00221D80
	Private ReadOnly Property levelPlayer As LevelPlayerController
		Get
			Return TryCast(Me.player, LevelPlayerController)
		End Get
	End Property

	' Token: 0x06003E36 RID: 15926 RVA: 0x00223990 File Offset: 0x00221D90
	Protected Overrides Sub SetPlayer(player As AbstractPlayerController)
		MyBase.SetPlayer(player)
		If player.stats.Loadout.charm = Charm.charm_chalice Then
			MyBase.GetComponent(Of CircleCollider2D)().offset = New Vector2(29.5F, 10F)
			MyBase.GetComponent(Of CircleCollider2D)().radius = 80F
		End If
		AddHandler Me.levelPlayer.motor.OnHitEvent, AddressOf Me.OnHitCancel
		AddHandler Me.levelPlayer.motor.OnGroundedEvent, AddressOf Me.OnGroundedCancel
		AddHandler Me.levelPlayer.motor.OnDashStartEvent, AddressOf Me.OnDashCancel
		AddHandler Me.levelPlayer.weaponManager.OnExStart, AddressOf Me.OnWeaponCancel
		AddHandler Me.levelPlayer.weaponManager.OnSuperStart, AddressOf Me.OnWeaponCancel
	End Sub

	' Token: 0x06003E37 RID: 15927 RVA: 0x00223A78 File Offset: 0x00221E78
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.levelPlayer.motor.OnHitEvent, AddressOf Me.OnHitCancel
		RemoveHandler Me.levelPlayer.motor.OnGroundedEvent, AddressOf Me.OnGroundedCancel
		RemoveHandler Me.levelPlayer.motor.OnDashStartEvent, AddressOf Me.OnDashCancel
		RemoveHandler Me.levelPlayer.weaponManager.OnExStart, AddressOf Me.OnWeaponCancel
		RemoveHandler Me.levelPlayer.weaponManager.OnSuperStart, AddressOf Me.OnWeaponCancel
	End Sub

	' Token: 0x06003E38 RID: 15928 RVA: 0x00223B18 File Offset: 0x00221F18
	Protected Overrides Sub OnHitCancel()
		MyBase.OnHitCancel()
		Me.levelPlayer.motor.OnParryHit()
	End Sub

	' Token: 0x06003E39 RID: 15929 RVA: 0x00223B30 File Offset: 0x00221F30
	Private Sub OnDashCancel()
		If Me.didHitSomething OrElse Me Is Nothing Then
			Return
		End If
		Me.Cancel()
	End Sub

	' Token: 0x06003E3A RID: 15930 RVA: 0x00223B50 File Offset: 0x00221F50
	Private Sub OnGroundedCancel()
		If Me.player.stats.isChalice Then
			Return
		End If
		If Me.didHitSomething OrElse Me Is Nothing Then
			Return
		End If
		Me.Cancel()
	End Sub

	' Token: 0x06003E3B RID: 15931 RVA: 0x00223B86 File Offset: 0x00221F86
	Private Sub OnWeaponCancel()
		If Me.didHitSomething OrElse Me Is Nothing Then
			Return
		End If
		Me.Cancel()
	End Sub

	' Token: 0x06003E3C RID: 15932 RVA: 0x00223BA6 File Offset: 0x00221FA6
	Protected Overrides Sub Cancel()
		MyBase.Cancel()
		Me.levelPlayer.animationController.ResumeNormanAnim()
	End Sub

	' Token: 0x06003E3D RID: 15933 RVA: 0x00223BBE File Offset: 0x00221FBE
	Protected Overrides Sub CancelSwitch()
		MyBase.CancelSwitch()
		Me.levelPlayer.motor.OnParryCanceled()
	End Sub

	' Token: 0x06003E3E RID: 15934 RVA: 0x00223BD6 File Offset: 0x00221FD6
	Protected Overrides Sub OnPaused()
		MyBase.OnPaused()
		Me.levelPlayer.animationController.OnParryPause()
		Me.levelPlayer.weaponManager.ParrySuccess()
	End Sub

	' Token: 0x06003E3F RID: 15935 RVA: 0x00223BFE File Offset: 0x00221FFE
	Protected Overrides Sub OnUnpaused()
		MyBase.OnUnpaused()
		Me.levelPlayer.animationController.ResumeNormanAnim()
		Me.levelPlayer.motor.OnParryComplete()
	End Sub

	' Token: 0x06003E40 RID: 15936 RVA: 0x00223C26 File Offset: 0x00222026
	Protected Overrides Sub OnSuccess()
		MyBase.OnSuccess()
		Me.levelPlayer.weaponManager.ParrySuccess()
		Me.levelPlayer.animationController.OnParrySuccess()
	End Sub

	' Token: 0x06003E41 RID: 15937 RVA: 0x00223C4E File Offset: 0x0022204E
	Protected Overrides Sub OnEnd()
		MyBase.OnEnd()
		Me.levelPlayer.motor.ResetChaliceDoubleJump()
		Me.levelPlayer.animationController.OnParryAnimEnd()
	End Sub

	' Token: 0x04004564 RID: 17764
	Private Const CHALICE_X_OFFSET As Single = 29.5F

	' Token: 0x04004565 RID: 17765
	Private Const CHALICE_Y_OFFSET As Single = 10F

	' Token: 0x04004566 RID: 17766
	Private Const CHALICE_RADIUS As Single = 80F
End Class
