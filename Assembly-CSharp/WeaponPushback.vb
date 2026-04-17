Imports System
Imports System.Collections

' Token: 0x02000A81 RID: 2689
Public Class WeaponPushback
	Inherits AbstractLevelWeapon

	' Token: 0x17000590 RID: 1424
	' (get) Token: 0x0600404A RID: 16458 RVA: 0x00230C8E File Offset: 0x0022F08E
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x17000591 RID: 1425
	' (get) Token: 0x0600404B RID: 16459 RVA: 0x00230C91 File Offset: 0x0022F091
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return Me.bulletFireRate
		End Get
	End Property

	' Token: 0x0600404C RID: 16460 RVA: 0x00230C9C File Offset: 0x0022F09C
	Private Sub Start()
		Me.speedTime = WeaponProperties.LevelWeaponPushback.Basic.speedTime
		MyBase.StartCoroutine(Me.determine_speed_cr())
		Me.forceAmount = WeaponProperties.LevelWeaponPushback.Basic.pushbackSpeed
		Me.forceLeft = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, Me.forceAmount)
		Me.forceRight = New LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, -Me.forceAmount)
	End Sub

	' Token: 0x0600404D RID: 16461 RVA: 0x00230CF4 File Offset: 0x0022F0F4
	Protected Overrides Function fireBasic() As AbstractProjectile
		Dim basicProjectile As BasicProjectile = TryCast(MyBase.fireBasic(), BasicProjectile)
		basicProjectile.Speed = Me.bulletSpeed
		basicProjectile.Damage = WeaponProperties.LevelWeaponPushback.Basic.damage
		basicProjectile.PlayerId = Me.player.id
		Dim num As Single = Me.yPositions(Me.currentY)
		Me.currentY += 1
		If Me.currentY >= Me.yPositions.Length Then
			Me.currentY = 0
		End If
		basicProjectile.transform.AddPosition(0F, num, 0F)
		Dim flag As Boolean = Me.player.transform.localScale.x < 0F
		If Not Me.hasForce Then
			Me.AddHorizontalForce(flag)
		End If
		Return basicProjectile
	End Function

	' Token: 0x0600404E RID: 16462 RVA: 0x00230DB4 File Offset: 0x0022F1B4
	Private Sub Update()
		Me.facingLeft = Me.player.transform.localScale.x < 0F
		If(Me.hasForce AndAlso Not Me.holdingShoot) OrElse Me.forceIsLeft <> Me.facingLeft Then
			Me.player.motor.RemoveForce(Me.forceLeft)
			Me.player.motor.RemoveForce(Me.forceRight)
			Me.hasForce = False
		End If
	End Sub

	' Token: 0x0600404F RID: 16463 RVA: 0x00230E40 File Offset: 0x0022F240
	Private Sub AddHorizontalForce(facingLeft As Boolean)
		Me.hasForce = True
		Me.forceIsLeft = facingLeft
		If facingLeft Then
			Me.player.motor.AddForce(Me.forceLeft)
		Else
			Me.player.motor.AddForce(Me.forceRight)
		End If
	End Sub

	' Token: 0x06004050 RID: 16464 RVA: 0x00230E94 File Offset: 0x0022F294
	Private Iterator Function determine_speed_cr() As IEnumerator
		Dim t As Single = 0F
		Dim speedVal As Single = 0F
		Dim fireVal As Single = 0F
		While True
			If Me.holdingShoot Then
				If speedVal < 1F Then
					speedVal = t / Me.speedTime
					fireVal = 1F - t / Me.speedTime
					t += CupheadTime.Delta
				Else
					speedVal = 1F
					t = 1F
				End If
			ElseIf speedVal > 0F Then
				speedVal = t / Me.speedTime
				fireVal = 1F - t / Me.speedTime
				t -= CupheadTime.Delta
			Else
				speedVal = 0F
				t = 0F
			End If
			Me.holdingShoot = Me.player.input.actions.GetButton(3)
			Me.bulletSpeed = WeaponProperties.LevelWeaponPushback.Basic.speed.GetFloatAt(speedVal)
			Me.bulletFireRate = WeaponProperties.LevelWeaponPushback.Basic.fireRate.GetFloatAt(fireVal)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0400471D RID: 18205
	Private Const ONE As Single = 1F

	' Token: 0x0400471E RID: 18206
	Private Const Y_POS As Single = 20F

	' Token: 0x0400471F RID: 18207
	Private Const ROTATION_OFFSET As Single = 3F

	' Token: 0x04004720 RID: 18208
	Private currentY As Integer

	' Token: 0x04004721 RID: 18209
	Private yPositions As Single() = New Single() { 0F, 20F, 40F, 20F }

	' Token: 0x04004722 RID: 18210
	Private bulletSpeed As Single

	' Token: 0x04004723 RID: 18211
	Private bulletFireRate As Single

	' Token: 0x04004724 RID: 18212
	Private speedTime As Single

	' Token: 0x04004725 RID: 18213
	Private forceAmount As Single

	' Token: 0x04004726 RID: 18214
	Private holdingShoot As Boolean

	' Token: 0x04004727 RID: 18215
	Private hasForce As Boolean

	' Token: 0x04004728 RID: 18216
	Private facingLeft As Boolean

	' Token: 0x04004729 RID: 18217
	Private forceIsLeft As Boolean

	' Token: 0x0400472A RID: 18218
	Private forceLeft As LevelPlayerMotor.VelocityManager.Force

	' Token: 0x0400472B RID: 18219
	Private forceRight As LevelPlayerMotor.VelocityManager.Force
End Class
