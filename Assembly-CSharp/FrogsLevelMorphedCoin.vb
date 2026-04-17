Imports System
Imports UnityEngine

' Token: 0x020006AE RID: 1710
Public Class FrogsLevelMorphedCoin
	Inherits BasicProjectile

	' Token: 0x0600244F RID: 9295 RVA: 0x00155274 File Offset: 0x00153674
	Public Function CreateCoin(pos As Vector2, speed As Single, rotation As Single) As FrogsLevelMorphedCoin
		Dim frogsLevelMorphedCoin As FrogsLevelMorphedCoin = TryCast(MyBase.Create(pos, rotation, speed), FrogsLevelMorphedCoin)
		frogsLevelMorphedCoin.CollisionDeath.None()
		frogsLevelMorphedCoin.DamagesType.OnlyPlayer()
		Return frogsLevelMorphedCoin
	End Function
End Class
