Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005DA RID: 1498
Public Class DicePalacePachinkoLevelPipeBall
	Inherits AbstractProjectile

	' Token: 0x06001D95 RID: 7573 RVA: 0x0010FB90 File Offset: 0x0010DF90
	Protected Overrides Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If MyBase.transform.position.y < CSng((Level.Current.Ground - 20)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		MyBase.Update()
	End Sub

	' Token: 0x06001D96 RID: 7574 RVA: 0x0010FBEC File Offset: 0x0010DFEC
	Public Sub InitBall(properties As LevelProperties.DicePalacePachinko)
		Me.properties = properties
		Me.directionIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.balls.directionString.Split(New Char() { ","c }).Length)
		Me.speed = properties.CurrentState.balls.movementSpeed
		Me.onGround = False
		MyBase.StartCoroutine(Me.pick_dir_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001D97 RID: 7575 RVA: 0x0010FC68 File Offset: 0x0010E068
	Private Iterator Function move_cr() As IEnumerator
		While True
			If Me.bouncing Then
				Yield Nothing
			Else
				If Me.onGround Then
					MyBase.transform.localPosition += Vector3.right * Me.speed * CupheadTime.Delta
				Else
					MyBase.transform.localPosition += Vector3.down * Me.speed * CupheadTime.Delta
				End If
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x06001D98 RID: 7576 RVA: 0x0010FC84 File Offset: 0x0010E084
	Private Iterator Function pick_dir_cr() As IEnumerator
		While Not Me.onGround
			Yield Nothing
		End While
		Me.directionIndex += 1
		Me.ChangeDirection()
		Return
	End Function

	' Token: 0x06001D99 RID: 7577 RVA: 0x0010FCA0 File Offset: 0x0010E0A0
	Private Sub ChangeDirection()
		If Me.directionIndex >= Me.properties.CurrentState.balls.directionString.Split(New Char() { ","c }).Length Then
			Me.directionIndex = 0
		End If
		Dim c As Char = Me.properties.CurrentState.balls.directionString.Split(New Char() { ","c })(Me.directionIndex)(0)
		If c <> "L"c Then
			If c = "R"c Then
				Me.speed = Me.properties.CurrentState.balls.movementSpeed
			End If
		Else
			Me.speed = -Me.properties.CurrentState.balls.movementSpeed
		End If
	End Sub

	' Token: 0x06001D9A RID: 7578 RVA: 0x0010FD74 File Offset: 0x0010E174
	Private Iterator Function changeState_cr(grounded As Boolean, forceDirection As Boolean) As IEnumerator
		Yield Nothing
		Me.ChangeDirection()
		If grounded Then
			Me.onGround = True
			If Me.currentCollider Is Me.lastCollider Then
				Return
			End If
			MyBase.animator.SetTrigger("Bounce")
			Me.bouncing = True
			Yield Nothing
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Bounce", 1, False, True)
			Me.lastCollider = Me.currentCollider
			Dim platformAnimnator As Animator = Me.currentCollider.GetComponent(Of Animator)()
			If platformAnimnator Is Nothing Then
				Me.bouncing = False
				Return
			End If
			If MyBase.transform.position.x - Me.currentCollider.transform.position.x > 0F Then
				platformAnimnator.SetTrigger("Right")
				Me.speed = Me.properties.CurrentState.balls.movementSpeed
			Else
				platformAnimnator.SetTrigger("Left")
				Me.speed = -Me.properties.CurrentState.balls.movementSpeed
			End If
			MyBase.transform.SetParent(platformAnimnator.transform, True)
			Yield Nothing
			Me.bouncing = False
			Dim finalSpeed As Single = Me.speed
			Dim acceleration As Single = 0F
			While Me.onGround
				acceleration += CupheadTime.Delta
				Me.speed = Mathf.Min(Mathf.Lerp(0F, finalSpeed, acceleration * 2F), finalSpeed)
				Yield Nothing
			End While
			platformAnimnator.SetTrigger("Back")
			MyBase.transform.SetParent(Nothing, True)
			MyBase.transform.rotation = Quaternion.identity
		Else
			Me.onGround = False
			Me.speed = Me.properties.CurrentState.balls.movementSpeed
		End If
		Return
	End Function

	' Token: 0x06001D9B RID: 7579 RVA: 0x0010FD98 File Offset: 0x0010E198
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If phase = CollisionPhase.Enter AndAlso Not Me.onGround Then
			Me.currentCollider = hit.GetComponent(Of Collider2D)()
			If hit.GetComponent(Of LevelPlatform)() IsNot Nothing Then
				MyBase.StartCoroutine(Me.changeState_cr(True, True))
				MyBase.OnCollisionOther(hit, phase)
			ElseIf hit.GetComponent(Of DicePalacePachinkoLevelPeg)() IsNot Nothing Then
				MyBase.StartCoroutine(Me.changeState_cr(True, hit.GetComponent(Of DicePalacePachinkoLevelPeg)().forceDirection))
				MyBase.OnCollisionOther(hit, phase)
			End If
		ElseIf phase = CollisionPhase.[Exit] Then
			MyBase.StartCoroutine(Me.changeState_cr(False, False))
			MyBase.OnCollisionOther(hit, phase)
		End If
	End Sub

	' Token: 0x06001D9C RID: 7580 RVA: 0x0010FE44 File Offset: 0x0010E244
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionWalls(hit, phase)
	End Sub

	' Token: 0x06001D9D RID: 7581 RVA: 0x0010FE4E File Offset: 0x0010E24E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06001D9E RID: 7582 RVA: 0x0010FE5D File Offset: 0x0010E25D
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04002675 RID: 9845
	Private onGround As Boolean

	' Token: 0x04002676 RID: 9846
	Private speed As Single

	' Token: 0x04002677 RID: 9847
	Private directionIndex As Integer

	' Token: 0x04002678 RID: 9848
	Private properties As LevelProperties.DicePalacePachinko

	' Token: 0x04002679 RID: 9849
	Private bouncing As Boolean

	' Token: 0x0400267A RID: 9850
	Private lastCollider As Collider2D

	' Token: 0x0400267B RID: 9851
	Private currentCollider As Collider2D
End Class
