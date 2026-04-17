Imports System
Imports UnityEngine

' Token: 0x02000795 RID: 1941
Public Class RumRunnersLevelMobBossProjectile
	Inherits BasicProjectile

	' Token: 0x06002B14 RID: 11028 RVA: 0x00192254 File Offset: 0x00190654
	Public Overrides Function Create(position As Vector2, rotation As Single, speed As Single) As BasicProjectile
		Dim basicProjectile As BasicProjectile = MyBase.Create(position, rotation, speed)
		basicProjectile.CollisionDeath.None()
		basicProjectile.DamagesType.OnlyPlayer()
		Return basicProjectile
	End Function
End Class
