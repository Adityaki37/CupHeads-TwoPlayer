Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000571 RID: 1393
Public Class DevilLevelDragonHead
	Inherits AbstractCollidableObject

	' Token: 0x06001A68 RID: 6760 RVA: 0x000F1A60 File Offset: 0x000EFE60
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		For Each collisionChild As CollisionChild In Me.children.GetComponentsInChildren(Of CollisionChild)()
			AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Next
		Me.children.gameObject.SetActive(False)
	End Sub

	' Token: 0x06001A69 RID: 6761 RVA: 0x000F1AC6 File Offset: 0x000EFEC6
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A6A RID: 6762 RVA: 0x000F1ADE File Offset: 0x000EFEDE
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A6B RID: 6763 RVA: 0x000F1B08 File Offset: 0x000EFF08
	Public Sub Attack(parent As DevilLevelSittingDevil, isLeft As Boolean)
		Me.state = DevilLevelDragonHead.State.Moving
		MyBase.transform.SetScale(New Single?(CSng(If((Not isLeft), (-1), 1))), Nothing, Nothing)
		MyBase.StartCoroutine(Me.move_cr(parent, isLeft))
	End Sub

	' Token: 0x06001A6C RID: 6764 RVA: 0x000F1B5C File Offset: 0x000EFF5C
	Private Iterator Function move_cr(parent As DevilLevelSittingDevil, isLeft As Boolean) As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.transform.position = If((Not isLeft), Me.rightRoot.position, Me.leftRoot.position)
		Dim dir As Vector3 = If((Not isLeft), Vector3.left, Vector3.right)
		Yield parent.animator.WaitForAnimationToEnd(Me, "Morph_Start" + If((Not isLeft), "_Right", "_Left"), False, True)
		parent.animator.SetTrigger("OnDragonAttack")
		Me.children.gameObject.SetActive(True)
		While Me.state = DevilLevelDragonHead.State.Moving
			MyBase.transform.position += dir * (Me.speed * CupheadTime.FixedDelta) * parent.animator.speed
			Yield wait
		End While
		Yield parent.animator.WaitForAnimationToEnd(Me, "Morph_Attack", 1, False, True)
		Me.state = DevilLevelDragonHead.State.Idle
		Me.children.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x06001A6D RID: 6765 RVA: 0x000F1B85 File Offset: 0x000EFF85
	Public Sub SetPosition(pos As Vector3)
		Me.children.gameObject.SetActive(False)
		MyBase.transform.position = pos
	End Sub

	' Token: 0x04002390 RID: 9104
	<SerializeField()>
	Private speed As Single

	' Token: 0x04002391 RID: 9105
	<SerializeField()>
	Private leftRoot As Transform

	' Token: 0x04002392 RID: 9106
	<SerializeField()>
	Private rightRoot As Transform

	' Token: 0x04002393 RID: 9107
	<SerializeField()>
	Private children As Transform

	' Token: 0x04002394 RID: 9108
	Public state As DevilLevelDragonHead.State

	' Token: 0x04002395 RID: 9109
	Private Const FRAME_RATE As Single = 0.041666668F

	' Token: 0x04002396 RID: 9110
	Private damageDealer As DamageDealer

	' Token: 0x02000572 RID: 1394
	Public Enum State
		' Token: 0x04002398 RID: 9112
		Idle
		' Token: 0x04002399 RID: 9113
		Moving
		' Token: 0x0400239A RID: 9114
		Stopped
	End Enum
End Class
