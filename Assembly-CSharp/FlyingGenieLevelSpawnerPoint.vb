Imports System
Imports UnityEngine

' Token: 0x0200067A RID: 1658
Public Class FlyingGenieLevelSpawnerPoint
	Inherits AbstractProjectile

	' Token: 0x1700039D RID: 925
	' (get) Token: 0x060022F2 RID: 8946 RVA: 0x001482D4 File Offset: 0x001466D4
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060022F3 RID: 8947 RVA: 0x001482DC File Offset: 0x001466DC
	Public Function Create(pos As Vector2, rotation As Single, properties As LevelProperties.FlyingGenie.Bullets) As FlyingGenieLevelSpawnerPoint
		Dim flyingGenieLevelSpawnerPoint As FlyingGenieLevelSpawnerPoint = TryCast(MyBase.Create(pos, rotation), FlyingGenieLevelSpawnerPoint)
		flyingGenieLevelSpawnerPoint.properties = properties
		Return flyingGenieLevelSpawnerPoint
	End Function

	' Token: 0x060022F4 RID: 8948 RVA: 0x00148300 File Offset: 0x00146700
	Public Sub Shoot()
		Me.effect.Create(Me.root.transform.position)
		Dim num As Integer = Global.UnityEngine.Random.Range(1, 4)
		Dim basicProjectile As BasicProjectile = Me.projectile.Create(Me.root.transform.position, MyBase.transform.eulerAngles.z - 90F, Me.properties.childSpeed)
		basicProjectile.GetComponent(Of Animator)().Play("Bullet_" + num)
	End Sub

	' Token: 0x060022F5 RID: 8949 RVA: 0x00148391 File Offset: 0x00146791
	Public Sub Dead()
		Me.Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x060022F6 RID: 8950 RVA: 0x0014839F File Offset: 0x0014679F
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x060022F7 RID: 8951 RVA: 0x001483A7 File Offset: 0x001467A7
	Protected Overrides Sub RandomizeVariant()
	End Sub

	' Token: 0x04002B8D RID: 11149
	<SerializeField()>
	Private effect As Effect

	' Token: 0x04002B8E RID: 11150
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04002B8F RID: 11151
	<SerializeField()>
	Private root As Transform

	' Token: 0x04002B90 RID: 11152
	Private properties As LevelProperties.FlyingGenie.Bullets
End Class
