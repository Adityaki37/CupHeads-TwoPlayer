Imports System
Imports UnityEngine

' Token: 0x02000894 RID: 2196
Public Class TreePlatformingLevelLogProjectile
	Inherits BasicProjectile

	' Token: 0x06003311 RID: 13073 RVA: 0x001DB268 File Offset: 0x001D9668
	Public Function Create(position As Vector2, rotation As Single, speed As Single, isLeft As Boolean, parry As Boolean) As AbstractProjectile
		Dim treePlatformingLevelLogProjectile As TreePlatformingLevelLogProjectile = TryCast(MyBase.Create(position, rotation, speed), TreePlatformingLevelLogProjectile)
		treePlatformingLevelLogProjectile.animator.SetFloat("Direction", CSng(If((Not isLeft), (-1), 1)))
		treePlatformingLevelLogProjectile.animator.SetTrigger("Start")
		treePlatformingLevelLogProjectile.SetParryable(parry)
		Return treePlatformingLevelLogProjectile
	End Function

	' Token: 0x06003312 RID: 13074 RVA: 0x001DB2BC File Offset: 0x001D96BC
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		MyBase.animator.SetBool("Parry", parryable)
	End Sub

	' Token: 0x06003313 RID: 13075 RVA: 0x001DB2D6 File Offset: 0x001D96D6
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub
End Class
