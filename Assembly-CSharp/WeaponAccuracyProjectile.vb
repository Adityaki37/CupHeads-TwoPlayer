Imports System
Imports UnityEngine

' Token: 0x02000A65 RID: 2661
Public Class WeaponAccuracyProjectile
	Inherits BasicProjectile

	' Token: 0x06003F7F RID: 16255 RVA: 0x0022B17C File Offset: 0x0022957C
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		Me.hitEnemy = True
	End Sub

	' Token: 0x06003F80 RID: 16256 RVA: 0x0022B18D File Offset: 0x0022958D
	Protected Overrides Sub OnDestroy()
		If Me.EnemyDeath IsNot Nothing Then
			Me.EnemyDeath(Me.hitEnemy)
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06003F81 RID: 16257 RVA: 0x0022B1B1 File Offset: 0x002295B1
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003F82 RID: 16258 RVA: 0x0022B1C4 File Offset: 0x002295C4
	Public Sub SetSize(size As Single)
		MyBase.transform.SetScale(New Single?(size), New Single?(size), Nothing)
	End Sub

	' Token: 0x04004674 RID: 18036
	Public EnemyDeath As WeaponAccuracyProjectile.OnEnemyDeath

	' Token: 0x04004675 RID: 18037
	Private hitEnemy As Boolean

	' Token: 0x02000A66 RID: 2662
	' (Invoke) Token: 0x06003F84 RID: 16260
	Public Delegate Sub OnEnemyDeath(hitEnemy As Boolean)
End Class
