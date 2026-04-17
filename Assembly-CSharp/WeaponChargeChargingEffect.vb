Imports System
Imports UnityEngine

' Token: 0x02000A48 RID: 2632
Public Class WeaponChargeChargingEffect
	Inherits AbstractMonoBehaviour

	' Token: 0x06003EBB RID: 16059 RVA: 0x002264CC File Offset: 0x002248CC
	Public Function Create(pos As Vector2) As WeaponChargeChargingEffect
		Dim weaponChargeChargingEffect As WeaponChargeChargingEffect = Me.InstantiatePrefab(Of WeaponChargeChargingEffect)()
		weaponChargeChargingEffect.transform.position = pos
		Return weaponChargeChargingEffect
	End Function
End Class
