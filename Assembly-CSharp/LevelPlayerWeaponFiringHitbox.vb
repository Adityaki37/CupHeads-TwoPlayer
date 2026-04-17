Imports System
Imports UnityEngine

' Token: 0x02000A4A RID: 2634
Public Class LevelPlayerWeaponFiringHitbox
	Inherits CollisionChild

	' Token: 0x06003EC2 RID: 16066 RVA: 0x00226640 File Offset: 0x00224A40
	Public Function Create(pos As Vector2, rotation As Single) As LevelPlayerWeaponFiringHitbox
		Dim levelPlayerWeaponFiringHitbox As LevelPlayerWeaponFiringHitbox = Me.InstantiatePrefab(Of LevelPlayerWeaponFiringHitbox)()
		levelPlayerWeaponFiringHitbox.transform.position = pos
		levelPlayerWeaponFiringHitbox.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(rotation))
		Return levelPlayerWeaponFiringHitbox
	End Function
End Class
