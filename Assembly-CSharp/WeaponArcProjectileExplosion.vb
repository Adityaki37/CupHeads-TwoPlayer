Imports System
Imports UnityEngine

' Token: 0x02000A6A RID: 2666
Public Class WeaponArcProjectileExplosion
	Inherits Effect

	' Token: 0x1700057A RID: 1402
	' (get) Token: 0x06003F9B RID: 16283 RVA: 0x0022B8F4 File Offset: 0x00229CF4
	Public ReadOnly Property DamageDealer As DamageDealer
		Get
			Return Me.damageDealer
		End Get
	End Property

	' Token: 0x06003F9C RID: 16284 RVA: 0x0022B8FC File Offset: 0x00229CFC
	Public Function Create(position As Vector2, damage As Single, damageMultiplier As Single, playerId As PlayerId) As WeaponArcProjectileExplosion
		Dim weaponArcProjectileExplosion As WeaponArcProjectileExplosion = TryCast(MyBase.Create(position), WeaponArcProjectileExplosion)
		weaponArcProjectileExplosion.damageDealer.SetDamage(damage)
		weaponArcProjectileExplosion.damageDealer.DamageMultiplier *= damageMultiplier
		weaponArcProjectileExplosion.damageDealer.SetDamageFlags(False, True, False)
		weaponArcProjectileExplosion.damageDealer.PlayerId = playerId
		Return weaponArcProjectileExplosion
	End Function

	' Token: 0x06003F9D RID: 16285 RVA: 0x0022B956 File Offset: 0x00229D56
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = New DamageDealer(1F, 0F)
	End Sub

	' Token: 0x06003F9E RID: 16286 RVA: 0x0022B973 File Offset: 0x00229D73
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003F9F RID: 16287 RVA: 0x0022B98B File Offset: 0x00229D8B
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x04004682 RID: 18050
	Private damageDealer As DamageDealer
End Class
