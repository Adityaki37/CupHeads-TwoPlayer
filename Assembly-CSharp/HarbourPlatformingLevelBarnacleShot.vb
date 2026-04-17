Imports System
Imports UnityEngine

' Token: 0x020008C3 RID: 2243
Public Class HarbourPlatformingLevelBarnacleShot
	Inherits BasicProjectile

	' Token: 0x06003469 RID: 13417 RVA: 0x001E700C File Offset: 0x001E540C
	Public Overrides Function Create(position As Vector2, rotation As Single, speed As Single) As BasicProjectile
		Dim harbourPlatformingLevelBarnacleShot As HarbourPlatformingLevelBarnacleShot = TryCast(MyBase.Create(position, rotation, speed), HarbourPlatformingLevelBarnacleShot)
		harbourPlatformingLevelBarnacleShot.animator.SetFloat("Speed", If((Not Rand.Bool()), 1F, (-1F)) * 1F * Global.UnityEngine.Random.Range(0.9F, 1.1F))
		Return harbourPlatformingLevelBarnacleShot
	End Function

	' Token: 0x04003C95 RID: 15509
	Private Const ProjectileSpeed As Single = 1F
End Class
