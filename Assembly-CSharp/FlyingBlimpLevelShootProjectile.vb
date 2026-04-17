Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200063F RID: 1599
Public Class FlyingBlimpLevelShootProjectile
	Inherits AbstractProjectile

	' Token: 0x060020D3 RID: 8403 RVA: 0x0012F520 File Offset: 0x0012D920
	Public Function Create(pos As Vector2, rotation As Single, properties As LevelProperties.FlyingBlimp.Shoot) As FlyingBlimpLevelShootProjectile
		Dim flyingBlimpLevelShootProjectile As FlyingBlimpLevelShootProjectile = TryCast(MyBase.Create(), FlyingBlimpLevelShootProjectile)
		flyingBlimpLevelShootProjectile.properties = properties
		flyingBlimpLevelShootProjectile.velocity = properties.speedMin
		flyingBlimpLevelShootProjectile.transform.position = pos
		Return flyingBlimpLevelShootProjectile
	End Function

	' Token: 0x060020D4 RID: 8404 RVA: 0x0012F55E File Offset: 0x0012D95E
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060020D5 RID: 8405 RVA: 0x0012F573 File Offset: 0x0012D973
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060020D6 RID: 8406 RVA: 0x0012F591 File Offset: 0x0012D991
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060020D7 RID: 8407 RVA: 0x0012F5B0 File Offset: 0x0012D9B0
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Me.velocity < Me.properties.speedMax
			Me.velocity += Me.properties.accelerationTime * CupheadTime.FixedDelta
			Yield wait
			MyBase.transform.AddPosition(-Me.velocity * CupheadTime.FixedDelta, 0F, 0F)
		End While
		Me.Die()
		Yield wait
		Return
	End Function

	' Token: 0x060020D8 RID: 8408 RVA: 0x0012F5CB File Offset: 0x0012D9CB
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400296C RID: 10604
	Private properties As LevelProperties.FlyingBlimp.Shoot

	' Token: 0x0400296D RID: 10605
	Private velocity As Single
End Class
