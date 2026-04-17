Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000761 RID: 1889
Public Class RetroArcadeTrafficUFO
	Inherits AbstractCollidableObject

	' Token: 0x170003E3 RID: 995
	' (get) Token: 0x06002929 RID: 10537 RVA: 0x00180113 File Offset: 0x0017E513
	' (set) Token: 0x0600292A RID: 10538 RVA: 0x0018011B File Offset: 0x0017E51B
	Public Property IsMoving As Boolean

	' Token: 0x170003E4 RID: 996
	' (get) Token: 0x0600292B RID: 10539 RVA: 0x00180124 File Offset: 0x0017E524
	' (set) Token: 0x0600292C RID: 10540 RVA: 0x0018012C File Offset: 0x0017E52C
	Public Property IsDead As Boolean

	' Token: 0x0600292D RID: 10541 RVA: 0x00180135 File Offset: 0x0017E535
	Private Sub Start()
		MyBase.gameObject.SetActive(False)
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x0600292E RID: 10542 RVA: 0x0018014E File Offset: 0x0017E54E
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600292F RID: 10543 RVA: 0x00180166 File Offset: 0x0017E566
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002930 RID: 10544 RVA: 0x00180184 File Offset: 0x0017E584
	Public Sub StartMoving(positions As List(Of Vector3), speed As Single, delay As Single)
		Me.IsMoving = True
		MyBase.StartCoroutine(Me.check_pieces_cr())
		MyBase.StartCoroutine(Me.move_cr(positions, speed, delay))
	End Sub

	' Token: 0x06002931 RID: 10545 RVA: 0x001801AC File Offset: 0x0017E5AC
	Private Iterator Function move_cr(positions As List(Of Vector3), speed As Single, delay As Single) As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		For i As Integer = 0 To positions.Count - 1
			Yield CupheadTime.WaitForSeconds(Me, delay)
			Dim dir As Vector3 = (positions(i) - MyBase.transform.position).normalized
			While Vector3.Distance(MyBase.transform.position, positions(i)) > 3F
				MyBase.transform.position += dir * speed * CupheadTime.FixedDelta
				Yield wait
			End While
			Yield Nothing
		Next
		Me.IsMoving = False
		Return
	End Function

	' Token: 0x06002932 RID: 10546 RVA: 0x001801DC File Offset: 0x0017E5DC
	Private Iterator Function check_pieces_cr() As IEnumerator
		Dim pieces As RetroArcadeTrafficUFOPiece() = MyBase.GetComponentsInChildren(Of RetroArcadeTrafficUFOPiece)()
		Dim countDeadOnes As Integer = 0
		While True
			countDeadOnes = 0
			For i As Integer = 0 To pieces.Length - 1
				If pieces(i).IsDead Then
					countDeadOnes += 1
				End If
			Next
			If countDeadOnes >= pieces.Length Then
				Exit For
			End If
			Yield Nothing
		End While
		Me.IsDead = True
		Yield Nothing
		Return
	End Function

	' Token: 0x0400321D RID: 12829
	Private Const DIST_TO_SWITCH As Single = 3F

	' Token: 0x04003220 RID: 12832
	Private damageDealer As DamageDealer
End Class
