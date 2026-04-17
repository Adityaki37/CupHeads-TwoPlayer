Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000677 RID: 1655
Public Class FlyingGenieLevelPyramid
	Inherits AbstractCollidableObject

	' Token: 0x060022DB RID: 8923 RVA: 0x00147310 File Offset: 0x00145710
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		For Each gameObject As GameObject In Me.beams
			AddHandler gameObject.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Next
	End Sub

	' Token: 0x060022DC RID: 8924 RVA: 0x00147365 File Offset: 0x00145765
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060022DD RID: 8925 RVA: 0x00147383 File Offset: 0x00145783
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060022DE RID: 8926 RVA: 0x0014739C File Offset: 0x0014579C
	Public Sub Init(properties As LevelProperties.FlyingGenie.Pyramids, startPos As Vector2, startAngle As Single, speed As Single, pivot As Transform, number As Integer, isClockWise As Boolean)
		MyBase.transform.position = startPos
		Me.angle = startAngle
		Me.speed = speed
		Me.pivotPoint = pivot
		Me.number = number
		Me.properties = properties
		Me.isClockwise = isClockWise
		MyBase.StartCoroutine(Me.path_cr())
	End Sub

	' Token: 0x060022DF RID: 8927 RVA: 0x001473F8 File Offset: 0x001457F8
	Public Iterator Function beam_cr() As IEnumerator
		Me.finishedATK = False
		AudioManager.Play("genie_pyramid_attack")
		Me.emitAudioFromObject.Add("genie_pyramid_attack")
		MyBase.animator.SetTrigger("OnStartAttack")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Open_Start", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.warningDuration)
		MyBase.animator.SetTrigger("OnShoot")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Shoot_Start", False, True)
		For Each gameObject As GameObject In Me.beams
			gameObject.GetComponent(Of Animator)().SetBool("IsAttacking", True)
		Next
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.beamDuration)
		MyBase.animator.SetTrigger("OnEnd")
		For Each gameObject2 As GameObject In Me.beams
			gameObject2.GetComponent(Of Animator)().SetBool("IsAttacking", False)
		Next
		Me.finishedATK = True
		Return
	End Function

	' Token: 0x060022E0 RID: 8928 RVA: 0x00147414 File Offset: 0x00145814
	Private Iterator Function path_cr() As IEnumerator
		While True
			Me.PathMovement()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060022E1 RID: 8929 RVA: 0x00147430 File Offset: 0x00145830
	Private Sub PathMovement()
		Me.angle += Me.speed * CupheadTime.Delta
		Dim vector As Vector3
		If Me.isClockwise Then
			vector = New Vector3(Mathf.Sin(Me.angle) * Me.properties.pyramidLoopSize, 0F, 0F)
		Else
			vector = New Vector3(-Mathf.Sin(Me.angle) * Me.properties.pyramidLoopSize, 0F, 0F)
		End If
		Dim vector2 As Vector3 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.properties.pyramidLoopSize, 0F)
		MyBase.transform.position = Me.pivotPoint.position
		MyBase.transform.position += vector + vector2
	End Sub

	' Token: 0x04002B78 RID: 11128
	Public number As Integer

	' Token: 0x04002B79 RID: 11129
	Public finishedATK As Boolean

	' Token: 0x04002B7A RID: 11130
	<SerializeField()>
	Private beams As GameObject()

	' Token: 0x04002B7B RID: 11131
	Private properties As LevelProperties.FlyingGenie.Pyramids

	' Token: 0x04002B7C RID: 11132
	Private damageDealer As DamageDealer

	' Token: 0x04002B7D RID: 11133
	Private damageReceiver As DamageReceiver

	' Token: 0x04002B7E RID: 11134
	Private pivotPoint As Transform

	' Token: 0x04002B7F RID: 11135
	Private speed As Single

	' Token: 0x04002B80 RID: 11136
	Private angle As Single

	' Token: 0x04002B81 RID: 11137
	Private isClockwise As Boolean
End Class
