Imports System

' Token: 0x02000A08 RID: 2568
Public Class ArcadeWeaponRocketPeashot
	Inherits AbstractArcadeWeapon

	' Token: 0x17000522 RID: 1314
	' (get) Token: 0x06003CA7 RID: 15527 RVA: 0x0021A031 File Offset: 0x00218431
	Protected Overrides ReadOnly Property rapidFire As Boolean
		Get
			Return WeaponProperties.ArcadeWeaponRocketPeashot.Basic.rapidFire
		End Get
	End Property

	' Token: 0x17000523 RID: 1315
	' (get) Token: 0x06003CA8 RID: 15528 RVA: 0x0021A038 File Offset: 0x00218438
	Protected Overrides ReadOnly Property rapidFireRate As Single
		Get
			Return WeaponProperties.ArcadeWeaponRocketPeashot.Basic.rapidFireRate
		End Get
	End Property

	' Token: 0x06003CA9 RID: 15529 RVA: 0x0021A040 File Offset: 0x00218440
	Protected Overrides Function fireBasic() As AbstractProjectile
		If Me.p IsNot Nothing AndAlso Not Me.p.dead Then
			Return Nothing
		End If
		Me.p = TryCast(MyBase.fireBasic(), ArcadeWeaponBullet)
		Me.p.Speed = WeaponProperties.ArcadeWeaponRocketPeashot.Basic.speed
		Me.p.Damage = WeaponProperties.ArcadeWeaponRocketPeashot.Basic.damage
		Me.p.PlayerId = Me.player.id
		Me.p.DamagesType.PlayerProjectileDefault()
		Me.p.CollisionDeath.PlayerProjectileDefault()
		Return Me.p
	End Function

	' Token: 0x040043F7 RID: 17399
	Private p As ArcadeWeaponBullet
End Class
