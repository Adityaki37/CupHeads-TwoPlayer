Imports System
Imports UnityEngine

' Token: 0x020004C8 RID: 1224
Public Class AirplaneLevelTurretBullet
	Inherits AbstractProjectile

	' Token: 0x060014B7 RID: 5303 RVA: 0x000BA108 File Offset: 0x000B8508
	Public Function Create(pos As Vector2, velocity As Vector2, gravity As Single) As AirplaneLevelTurretBullet
		Dim airplaneLevelTurretBullet As AirplaneLevelTurretBullet = TryCast(MyBase.Create(), AirplaneLevelTurretBullet)
		airplaneLevelTurretBullet.velocity = velocity
		airplaneLevelTurretBullet.transform.position = pos
		airplaneLevelTurretBullet.gravity = gravity
		Return airplaneLevelTurretBullet
	End Function

	' Token: 0x060014B8 RID: 5304 RVA: 0x000BA144 File Offset: 0x000B8544
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x060014B9 RID: 5305 RVA: 0x000BA1A7 File Offset: 0x000B85A7
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060014BA RID: 5306 RVA: 0x000BA1C5 File Offset: 0x000B85C5
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase = CollisionPhase.Enter Then
			Me.damageDealer.DealDamage(hit)
			AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_impact")
		End If
	End Sub

	' Token: 0x04001E27 RID: 7719
	Private velocity As Vector2

	' Token: 0x04001E28 RID: 7720
	Private gravity As Single
End Class
