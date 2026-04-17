Imports System

' Token: 0x02000A07 RID: 2567
Public Class ArcadeWeaponPeashot
	Inherits AbstractArcadeWeapon

	' Token: 0x17000520 RID: 1312
	' (get) Token: 0x06003CA2 RID: 15522 RVA: 0x00219F7A File Offset: 0x0021837A
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.ArcadeWeaponPeashot.Basic.rapidFire
		End Get
	End Property

	' Token: 0x17000521 RID: 1313
	' (get) Token: 0x06003CA3 RID: 15523 RVA: 0x00219F81 File Offset: 0x00218381
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.ArcadeWeaponPeashot.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x06003CA4 RID: 15524 RVA: 0x00219F88 File Offset: 0x00218388
	Protected Overrides Function fireBasic() As AbstractProjectile
		If Me.p IsNot Nothing AndAlso Not Me.p.dead Then
			Return Nothing
		End If
		Me.p = TryCast(MyBase.fireBasic(), ArcadeWeaponBullet)
		Me.p.Speed = WeaponProperties.ArcadeWeaponPeashot.Basic.speed
		Me.p.Damage = WeaponProperties.ArcadeWeaponPeashot.Basic.damage
		Me.p.PlayerId = Me.player.id
		Me.p.DamagesType.PlayerProjectileDefault()
		Me.p.CollisionDeath.PlayerProjectileDefault()
		Return Me.p
	End Function

	' Token: 0x06003CA5 RID: 15525 RVA: 0x0021A026 File Offset: 0x00218426
	Protected Overrides Function fireEx() As AbstractProjectile
		Return Nothing
	End Function

	' Token: 0x040043F4 RID: 17396
	Private Const Y_POS As Single = 20F

	' Token: 0x040043F5 RID: 17397
	Private Const ROTATION_OFFSET As Single = 3F

	' Token: 0x040043F6 RID: 17398
	Private p As ArcadeWeaponBullet
End Class
