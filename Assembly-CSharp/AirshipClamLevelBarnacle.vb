Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004CB RID: 1227
Public Class AirshipClamLevelBarnacle
	Inherits AbstractProjectile

	' Token: 0x060014CB RID: 5323 RVA: 0x000BA49E File Offset: 0x000B889E
	Protected Overrides Sub Update()
		Me.damageDealer.Update()
		MyBase.Update()
	End Sub

	' Token: 0x060014CC RID: 5324 RVA: 0x000BA4B1 File Offset: 0x000B88B1
	Public Sub InitBarnacle(dir As Integer, properties As LevelProperties.AirshipClam)
		Me.properties = properties
		Me.direction = dir
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060014CD RID: 5325 RVA: 0x000BA4D0 File Offset: 0x000B88D0
	Private Iterator Function move_cr() As IEnumerator
		Me.velocity = New Vector3(Me.properties.CurrentState.barnacles.initialArcMovementX * CSng(Me.direction), Me.properties.CurrentState.barnacles.initialArcMovementY, 0F)
		While True
			MyBase.transform.position += Me.velocity * CupheadTime.Delta
			Me.velocity.y = Me.velocity.y + Me.properties.CurrentState.barnacles.parryGravity
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060014CE RID: 5326 RVA: 0x000BA4EB File Offset: 0x000B88EB
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		If phase = CollisionPhase.Enter Then
			Me.velocity.x = 0F
		End If
		MyBase.OnCollisionWalls(hit, phase)
	End Sub

	' Token: 0x060014CF RID: 5327 RVA: 0x000BA50C File Offset: 0x000B890C
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Me.velocity.y = 0F
		Me.velocity.x = Me.properties.CurrentState.barnacles.rollingSpeed * CSng((-CSng(Me.direction)))
	End Sub

	' Token: 0x060014D0 RID: 5328 RVA: 0x000BA55A File Offset: 0x000B895A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060014D1 RID: 5329 RVA: 0x000BA57C File Offset: 0x000B897C
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04001E31 RID: 7729
	Private direction As Integer

	' Token: 0x04001E32 RID: 7730
	Private velocity As Vector3

	' Token: 0x04001E33 RID: 7731
	Private properties As LevelProperties.AirshipClam
End Class
