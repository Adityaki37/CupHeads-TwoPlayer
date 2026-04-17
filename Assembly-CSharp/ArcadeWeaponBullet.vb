Imports System
Imports UnityEngine

' Token: 0x02000A06 RID: 2566
Public Class ArcadeWeaponBullet
	Inherits BasicProjectile

	' Token: 0x06003CA0 RID: 15520 RVA: 0x00219F08 File Offset: 0x00218308
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If hit.GetComponent(Of RetroArcadeEnemy)() Then
			ArcadeWeaponBullet.IN_COMBO = True
			ArcadeWeaponBullet.POINTS_BONUS_ACCURACY += RetroArcadeLevel.ACCURACY_BONUS
		ElseIf ArcadeWeaponBullet.IN_COMBO Then
			RetroArcadeLevel.TOTAL_POINTS += ArcadeWeaponBullet.POINTS_BONUS_ACCURACY
			ArcadeWeaponBullet.POINTS_BONUS_ACCURACY = 0F
			ArcadeWeaponBullet.IN_COMBO = False
		End If
	End Sub

	' Token: 0x040043F2 RID: 17394
	Private Shared POINTS_BONUS_ACCURACY As Single

	' Token: 0x040043F3 RID: 17395
	Private Shared IN_COMBO As Boolean
End Class
