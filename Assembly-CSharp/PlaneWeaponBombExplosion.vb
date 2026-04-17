Imports System
Imports UnityEngine

' Token: 0x02000AB5 RID: 2741
Public Class PlaneWeaponBombExplosion
	Inherits Effect

	' Token: 0x060041D7 RID: 16855 RVA: 0x00239818 File Offset: 0x00237C18
	Public Sub Create(position As Vector2, damage As Single, damageMultiplier As Single, size As Single)
		Dim planeWeaponBombExplosion As PlaneWeaponBombExplosion = TryCast(MyBase.Create(position), PlaneWeaponBombExplosion)
		planeWeaponBombExplosion.damageDealer.SetDamage(damage)
		planeWeaponBombExplosion.damageDealer.DamageMultiplier *= damageMultiplier
		planeWeaponBombExplosion.damageDealer.SetDamageFlags(False, True, False)
		planeWeaponBombExplosion.transform.SetScale(New Single?(size), New Single?(size), Nothing)
	End Sub

	' Token: 0x060041D8 RID: 16856 RVA: 0x00239886 File Offset: 0x00237C86
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x060041D9 RID: 16857 RVA: 0x00239899 File Offset: 0x00237C99
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060041DA RID: 16858 RVA: 0x002398B1 File Offset: 0x00237CB1
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0400483B RID: 18491
	Private damageDealer As DamageDealer
End Class
