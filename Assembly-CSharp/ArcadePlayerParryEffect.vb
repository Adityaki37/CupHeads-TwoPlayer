Imports System

' Token: 0x020009F6 RID: 2550
Public Class ArcadePlayerParryEffect
	Inherits AbstractParryEffect

	' Token: 0x17000516 RID: 1302
	' (get) Token: 0x06003C2B RID: 15403 RVA: 0x00218955 File Offset: 0x00216D55
	Protected Overrides ReadOnly Property IsHit As Boolean
		Get
			Return TryCast(Me.player, ArcadePlayerController).motor.IsHit
		End Get
	End Property

	' Token: 0x17000517 RID: 1303
	' (get) Token: 0x06003C2C RID: 15404 RVA: 0x0021896C File Offset: 0x00216D6C
	Private ReadOnly Property levelPlayer As ArcadePlayerController
		Get
			Return TryCast(Me.player, ArcadePlayerController)
		End Get
	End Property

	' Token: 0x06003C2D RID: 15405 RVA: 0x0021897C File Offset: 0x00216D7C
	Protected Overrides Sub SetPlayer(player As AbstractPlayerController)
		MyBase.SetPlayer(player)
		AddHandler Me.levelPlayer.motor.OnHitEvent, AddressOf Me.OnHitCancel
		AddHandler Me.levelPlayer.motor.OnGroundedEvent, AddressOf Me.OnGroundedCancel
		AddHandler Me.levelPlayer.motor.OnDashStartEvent, AddressOf Me.OnDashCancel
		AddHandler Me.levelPlayer.weaponManager.OnExStart, AddressOf Me.OnWeaponCancel
		AddHandler Me.levelPlayer.weaponManager.OnSuperStart, AddressOf Me.OnWeaponCancel
	End Sub

	' Token: 0x06003C2E RID: 15406 RVA: 0x00218A20 File Offset: 0x00216E20
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.levelPlayer.motor.OnHitEvent, AddressOf Me.OnHitCancel
		RemoveHandler Me.levelPlayer.motor.OnGroundedEvent, AddressOf Me.OnGroundedCancel
		RemoveHandler Me.levelPlayer.motor.OnDashStartEvent, AddressOf Me.OnDashCancel
		RemoveHandler Me.levelPlayer.weaponManager.OnExStart, AddressOf Me.OnWeaponCancel
		RemoveHandler Me.levelPlayer.weaponManager.OnSuperStart, AddressOf Me.OnWeaponCancel
	End Sub

	' Token: 0x06003C2F RID: 15407 RVA: 0x00218AC0 File Offset: 0x00216EC0
	Protected Overrides Sub OnHitCancel()
		MyBase.OnHitCancel()
		Me.levelPlayer.motor.OnParryHit()
	End Sub

	' Token: 0x06003C30 RID: 15408 RVA: 0x00218AD8 File Offset: 0x00216ED8
	Private Sub OnDashCancel()
		If Me.didHitSomething OrElse Me Is Nothing Then
			Return
		End If
		Me.Cancel()
	End Sub

	' Token: 0x06003C31 RID: 15409 RVA: 0x00218AF8 File Offset: 0x00216EF8
	Private Sub OnGroundedCancel()
		If Me.didHitSomething OrElse Me Is Nothing Then
			Return
		End If
		Me.Cancel()
	End Sub

	' Token: 0x06003C32 RID: 15410 RVA: 0x00218B18 File Offset: 0x00216F18
	Private Sub OnWeaponCancel()
		If Me.didHitSomething OrElse Me Is Nothing Then
			Return
		End If
		Me.Cancel()
	End Sub

	' Token: 0x06003C33 RID: 15411 RVA: 0x00218B38 File Offset: 0x00216F38
	Protected Overrides Sub Cancel()
		MyBase.Cancel()
		Me.levelPlayer.animationController.ResumeNormanAnim()
	End Sub

	' Token: 0x06003C34 RID: 15412 RVA: 0x00218B50 File Offset: 0x00216F50
	Protected Overrides Sub CancelSwitch()
		MyBase.CancelSwitch()
		Me.levelPlayer.motor.OnParryCanceled()
	End Sub

	' Token: 0x06003C35 RID: 15413 RVA: 0x00218B68 File Offset: 0x00216F68
	Protected Overrides Sub OnPaused()
		MyBase.OnPaused()
		Me.levelPlayer.animationController.OnParryPause()
		Me.levelPlayer.weaponManager.ParrySuccess()
	End Sub

	' Token: 0x06003C36 RID: 15414 RVA: 0x00218B90 File Offset: 0x00216F90
	Protected Overrides Sub OnUnpaused()
		MyBase.OnUnpaused()
		Me.levelPlayer.animationController.ResumeNormanAnim()
		Me.levelPlayer.motor.OnParryComplete()
	End Sub

	' Token: 0x06003C37 RID: 15415 RVA: 0x00218BB8 File Offset: 0x00216FB8
	Protected Overrides Sub OnSuccess()
		MyBase.OnSuccess()
		Me.levelPlayer.weaponManager.ParrySuccess()
		Me.levelPlayer.animationController.OnParrySuccess()
	End Sub

	' Token: 0x06003C38 RID: 15416 RVA: 0x00218BE0 File Offset: 0x00216FE0
	Protected Overrides Sub OnEnd()
		MyBase.OnEnd()
		Me.levelPlayer.animationController.OnParryAnimEnd()
	End Sub
End Class
