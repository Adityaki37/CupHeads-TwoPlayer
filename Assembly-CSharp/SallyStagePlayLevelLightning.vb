Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007B0 RID: 1968
Public Class SallyStagePlayLevelLightning
	Inherits AbstractProjectile

	' Token: 0x06002C3D RID: 11325 RVA: 0x001A040C File Offset: 0x0019E80C
	Public Function Create(pos As Vector2, rotation As Single, speed As Single, lightningLast As Boolean) As SallyStagePlayLevelLightning
		Dim sallyStagePlayLevelLightning As SallyStagePlayLevelLightning = TryCast(MyBase.Create(pos, rotation), SallyStagePlayLevelLightning)
		sallyStagePlayLevelLightning.speed = speed
		sallyStagePlayLevelLightning.rotation = rotation
		sallyStagePlayLevelLightning.lightningLast = lightningLast
		Return sallyStagePlayLevelLightning
	End Function

	' Token: 0x06002C3E RID: 11326 RVA: 0x001A0440 File Offset: 0x0019E840
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.sprite.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		MyBase.animator.Play(Global.UnityEngine.Random.Range(0, 3).ToStringInvariant())
		MyBase.StartCoroutine(Me.move_cr())
		AudioManager.PlayLoop("sally_sally_lightning_move_loop")
		Me.emitAudioFromObject.Add("sally_sally_lightning_move_loop")
		AudioManager.Play("sally_thunder")
		AddHandler Me.sprite.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x06002C3F RID: 11327 RVA: 0x001A04DF File Offset: 0x0019E8DF
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002C40 RID: 11328 RVA: 0x001A04FD File Offset: 0x0019E8FD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002C41 RID: 11329 RVA: 0x001A051C File Offset: 0x0019E91C
	Protected Iterator Function move_cr() As IEnumerator
		Me.velocity = MyBase.transform.right
		While True
			MyBase.transform.position += Me.velocity * Me.speed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06002C42 RID: 11330 RVA: 0x001A0538 File Offset: 0x0019E938
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		If phase = CollisionPhase.Enter AndAlso Not Me.goingBackUp Then
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ceiling), 0F)
			Me.goingBackUp = True
			Me.collisionPoint = vector - position
			MyBase.StartCoroutine(Me.change_direction_cr(Me.collisionPoint))
		End If
	End Sub

	' Token: 0x06002C43 RID: 11331 RVA: 0x001A05BC File Offset: 0x0019E9BC
	Protected Iterator Function change_direction_cr(collisionPoint As Vector3) As IEnumerator
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(-Me.rotation))
		Me.sprite.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		Me.velocity = 1F * (-2F * Vector3.Dot(Me.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + Me.velocity)
		Yield New WaitForEndOfFrame()
		AudioManager.Play("sally_thunder_impact")
		While MyBase.transform.position.y < CSng((Level.Current.Ceiling + 100))
			Yield Nothing
		End While
		If Me.lightningLast Then
			AudioManager.[Stop]("sally_sally_lightning_move_loop")
			AudioManager.Play("sally_thunder_end")
		End If
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C44 RID: 11332 RVA: 0x001A05DE File Offset: 0x0019E9DE
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040034E5 RID: 13541
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x040034E6 RID: 13542
	Private velocity As Vector3

	' Token: 0x040034E7 RID: 13543
	Private collisionPoint As Vector3

	' Token: 0x040034E8 RID: 13544
	Private speed As Single

	' Token: 0x040034E9 RID: 13545
	Private rotation As Single

	' Token: 0x040034EA RID: 13546
	Private lightningLast As Boolean

	' Token: 0x040034EB RID: 13547
	Private goingBackUp As Boolean
End Class
