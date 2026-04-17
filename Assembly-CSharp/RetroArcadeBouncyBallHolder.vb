Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200073B RID: 1851
Public Class RetroArcadeBouncyBallHolder
	Inherits RetroArcadeEnemy

	' Token: 0x06002855 RID: 10325 RVA: 0x001781C4 File Offset: 0x001765C4
	Public Function Create(manager As RetroArcadeBouncyManager, properties As LevelProperties.RetroArcade.Bouncy, pos As Vector3, ballTypes As String()) As RetroArcadeBouncyBallHolder
		Dim retroArcadeBouncyBallHolder As RetroArcadeBouncyBallHolder = Me.InstantiatePrefab(Of RetroArcadeBouncyBallHolder)()
		retroArcadeBouncyBallHolder.manager = manager
		retroArcadeBouncyBallHolder.properties = properties
		retroArcadeBouncyBallHolder.transform.position = pos
		retroArcadeBouncyBallHolder.ballTypes = ballTypes
		Return retroArcadeBouncyBallHolder
	End Function

	' Token: 0x06002856 RID: 10326 RVA: 0x001781FB File Offset: 0x001765FB
	Protected Overrides Sub Start()
		Me.hp = 1F
		Me.SetBalls()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002857 RID: 10327 RVA: 0x0017821C File Offset: 0x0017661C
	Private Sub SetBalls()
		Me.ballsHeld = New RetroArcadeBouncyBall(Me.ballPositions.Length - 1) {}
		Dim retroArcadeBouncyBall As RetroArcadeBouncyBall = Me.typeABall
		Dim num As Single = 120F
		Dim i As Integer = 0
		While i < Me.ballPositions.Length
			Dim text As String = Me.ballTypes(i)
			If text Is Nothing Then
				GoTo IL_008F
			End If
			If Not(text = "A") Then
				If Not(text = "B") Then
					If Not(text = "C") Then
						GoTo IL_008F
					End If
					retroArcadeBouncyBall = Me.typeCBall
				Else
					retroArcadeBouncyBall = Me.typeBBall
				End If
			Else
				retroArcadeBouncyBall = Me.typeABall
			End If
			IL_009F:
			Dim retroArcadeBouncyBall2 As RetroArcadeBouncyBall = retroArcadeBouncyBall.Create(Me.ballPositions(i).position, Me.manager, Me.properties, num * CSng(i))
			Me.ballsHeld(i) = retroArcadeBouncyBall2
			Me.ballsHeld(i).transform.parent = MyBase.transform
			i += 1
			Continue While
			IL_008F:
			Global.Debug.LogError("Something bad happened", Nothing)
			GoTo IL_009F
		End While
	End Sub

	' Token: 0x06002858 RID: 10328 RVA: 0x00178324 File Offset: 0x00176724
	Private Sub SeparateBalls()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		For Each retroArcadeBouncyBall As RetroArcadeBouncyBall In Me.ballsHeld
			retroArcadeBouncyBall.StartMoving(MyBase.transform.position)
		Next
		MyBase.StartCoroutine(Me.check_to_die_cr())
	End Sub

	' Token: 0x06002859 RID: 10329 RVA: 0x0017837C File Offset: 0x0017677C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.red
		For Each transform As Transform In Me.ballPositions
			Gizmos.DrawWireSphere(transform.position, 20F)
		Next
	End Sub

	' Token: 0x0600285A RID: 10330 RVA: 0x001783C8 File Offset: 0x001767C8
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x0600285B RID: 10331 RVA: 0x001783D0 File Offset: 0x001767D0
	Private Iterator Function move_cr() As IEnumerator
		Me.velocity = MathUtils.AngleToDirection(Me.properties.angleRange.RandomFloat())
		While True
			MyBase.transform.position += Me.velocity * Me.properties.groupMoveSpeed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x0600285C RID: 10332 RVA: 0x001783EC File Offset: 0x001767EC
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionCeiling(hit, phase)
		Dim vector As Vector3 = Me.velocity
		vector.y = Mathf.Min(vector.y, -vector.y)
		Me.ChangeDir(vector)
	End Sub

	' Token: 0x0600285D RID: 10333 RVA: 0x0017842C File Offset: 0x0017682C
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Dim vector As Vector3 = Me.velocity
		vector.y = Mathf.Max(vector.y, -vector.y)
		Me.ChangeDir(vector)
	End Sub

	' Token: 0x0600285E RID: 10334 RVA: 0x0017846C File Offset: 0x0017686C
	Protected Sub ChangeDir(newVelocity As Vector3)
		Me.velocity = newVelocity
		Me.currentAngle = Mathf.Atan2(Me.velocity.y, Me.velocity.x) * 57.29578F
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.currentAngle))
		For Each retroArcadeBouncyBall As RetroArcadeBouncyBall In Me.ballsHeld
			retroArcadeBouncyBall.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		Next
	End Sub

	' Token: 0x0600285F RID: 10335 RVA: 0x0017851C File Offset: 0x0017691C
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionWalls(hit, phase)
		Dim vector As Vector3 = Me.velocity
		If MyBase.transform.position.x > 0F Then
			vector.x = Mathf.Min(vector.x, -vector.x)
			Me.ChangeDir(vector)
		Else
			vector.x = Mathf.Max(vector.x, -vector.x)
			Me.ChangeDir(vector)
		End If
	End Sub

	' Token: 0x06002860 RID: 10336 RVA: 0x0017859E File Offset: 0x0017699E
	Public Overrides Sub Dead()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		Me.SeparateBalls()
	End Sub

	' Token: 0x06002861 RID: 10337 RVA: 0x001785B8 File Offset: 0x001769B8
	Private Iterator Function check_to_die_cr() As IEnumerator
		Dim allDead As Boolean = True
		While True
			allDead = True
			For i As Integer = 0 To Me.ballsHeld.Length - 1
				If Not Me.ballsHeld(i).IsDead Then
					allDead = False
				End If
			Next
			If allDead Then
				Exit For
			End If
			Yield Nothing
		End While
		MyBase.IsDead = True
		Return
	End Function

	' Token: 0x06002862 RID: 10338 RVA: 0x001785D4 File Offset: 0x001769D4
	Public Sub DestroyBallsHeld()
		For Each retroArcadeBouncyBall As RetroArcadeBouncyBall In Me.ballsHeld
			Global.UnityEngine.[Object].Destroy(retroArcadeBouncyBall.gameObject)
		Next
	End Sub

	' Token: 0x04003119 RID: 12569
	<SerializeField()>
	Private ballPositions As Transform()

	' Token: 0x0400311A RID: 12570
	<SerializeField()>
	Private typeABall As RetroArcadeBouncyBall

	' Token: 0x0400311B RID: 12571
	<SerializeField()>
	Private typeBBall As RetroArcadeBouncyBall

	' Token: 0x0400311C RID: 12572
	<SerializeField()>
	Private typeCBall As RetroArcadeBouncyBall

	' Token: 0x0400311D RID: 12573
	Private ballsHeld As RetroArcadeBouncyBall()

	' Token: 0x0400311E RID: 12574
	Private currentAngle As Single

	' Token: 0x0400311F RID: 12575
	Private ballTypes As String()

	' Token: 0x04003120 RID: 12576
	Private velocity As Vector3

	' Token: 0x04003121 RID: 12577
	Private properties As LevelProperties.RetroArcade.Bouncy

	' Token: 0x04003122 RID: 12578
	Private manager As RetroArcadeBouncyManager
End Class
