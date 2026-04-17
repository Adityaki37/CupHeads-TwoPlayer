Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004EF RID: 1263
Public Class BaronessLevelGumballProjectile
	Inherits AbstractProjectile

	' Token: 0x0600160D RID: 5645 RVA: 0x000C6028 File Offset: 0x000C4428
	Public Function Create(pos As Vector2, velocity As Vector2, gravity As Single) As BaronessLevelGumballProjectile
		Dim baronessLevelGumballProjectile As BaronessLevelGumballProjectile = TryCast(MyBase.Create(), BaronessLevelGumballProjectile)
		baronessLevelGumballProjectile.velocity = velocity
		baronessLevelGumballProjectile.transform.position = pos
		baronessLevelGumballProjectile.gravity = gravity
		Return baronessLevelGumballProjectile
	End Function

	' Token: 0x0600160E RID: 5646 RVA: 0x000C6061 File Offset: 0x000C4461
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.spawn_trail_cr())
	End Sub

	' Token: 0x0600160F RID: 5647 RVA: 0x000C6078 File Offset: 0x000C4478
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If MyBase.transform.position.y <= -360F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001610 RID: 5648 RVA: 0x000C60C4 File Offset: 0x000C44C4
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.isDead Then
			Return
		End If
		MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x06001611 RID: 5649 RVA: 0x000C6133 File Offset: 0x000C4533
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001612 RID: 5650 RVA: 0x000C6154 File Offset: 0x000C4554
	Private Iterator Function spawn_trail_cr() As IEnumerator
		While True
			Yield Nothing
			Me.trail.Create(MyBase.transform.position)
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		End While
		Return
	End Function

	' Token: 0x06001613 RID: 5651 RVA: 0x000C616F File Offset: 0x000C456F
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		Me.isDead = True
		MyBase.Die()
		MyBase.animator.SetTrigger("Death")
	End Sub

	' Token: 0x06001614 RID: 5652 RVA: 0x000C6194 File Offset: 0x000C4594
	Private Sub Kill()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001615 RID: 5653 RVA: 0x000C61A1 File Offset: 0x000C45A1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.trail = Nothing
	End Sub

	' Token: 0x04001F5C RID: 8028
	<SerializeField()>
	Private trail As Effect

	' Token: 0x04001F5D RID: 8029
	Private velocity As Vector2

	' Token: 0x04001F5E RID: 8030
	Private gravity As Single

	' Token: 0x04001F5F RID: 8031
	Private isDead As Boolean
End Class
