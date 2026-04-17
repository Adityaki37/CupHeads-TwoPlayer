Imports System
Imports UnityEngine

' Token: 0x02000675 RID: 1653
Public Class FlyingGenieLevelObeliskBlock
	Inherits AbstractProjectile

	' Token: 0x060022CD RID: 8909 RVA: 0x00146EA8 File Offset: 0x001452A8
	Public Sub Init(pos As Vector3, properties As LevelProperties.FlyingGenie.Obelisk)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.rootPos = New Vector3(MyBase.GetComponent(Of Renderer)().bounds.size.x / 2F + 10F, 0F, 0F)
	End Sub

	' Token: 0x060022CE RID: 8910 RVA: 0x00146F04 File Offset: 0x00145304
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.darkSprite.sortingOrder = MyBase.GetComponent(Of SpriteRenderer)().sortingOrder + 1
	End Sub

	' Token: 0x060022CF RID: 8911 RVA: 0x00146F24 File Offset: 0x00145324
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060022D0 RID: 8912 RVA: 0x00146F42 File Offset: 0x00145342
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060022D1 RID: 8913 RVA: 0x00146F60 File Offset: 0x00145360
	Public Sub ShootRegular(angle As Single)
		Me.projectile.Create(MyBase.transform.position + Me.rootPos, angle, Me.properties.obeliskShootSpeed)
	End Sub

	' Token: 0x060022D2 RID: 8914 RVA: 0x00146F95 File Offset: 0x00145395
	Public Sub ShootPink(angle As Single)
		Me.pinkProjectile.Create(MyBase.transform.position + Me.rootPos, angle, Me.properties.obeliskShootSpeed)
	End Sub

	' Token: 0x04002B6F RID: 11119
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04002B70 RID: 11120
	<SerializeField()>
	Private pinkProjectile As BasicProjectile

	' Token: 0x04002B71 RID: 11121
	<SerializeField()>
	Private darkSprite As SpriteRenderer

	' Token: 0x04002B72 RID: 11122
	Private properties As LevelProperties.FlyingGenie.Obelisk

	' Token: 0x04002B73 RID: 11123
	Private rootPos As Vector3
End Class
