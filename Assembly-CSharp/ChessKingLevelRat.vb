Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200053E RID: 1342
Public Class ChessKingLevelRat
	Inherits AbstractCollidableObject

	' Token: 0x0600187E RID: 6270 RVA: 0x000DDBA0 File Offset: 0x000DBFA0
	Public Sub Init(position As Vector3, speed As Single)
		MyBase.transform.position = position
		Me.startPosX = MyBase.transform.position.x
		Me.speed = speed
		Me.Move()
	End Sub

	' Token: 0x0600187F RID: 6271 RVA: 0x000DDBDF File Offset: 0x000DBFDF
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001880 RID: 6272 RVA: 0x000DDBF2 File Offset: 0x000DBFF2
	Protected Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001881 RID: 6273 RVA: 0x000DDC0A File Offset: 0x000DC00A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001882 RID: 6274 RVA: 0x000DDC28 File Offset: 0x000DC028
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001883 RID: 6275 RVA: 0x000DDC38 File Offset: 0x000DC038
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim leftBound As Single = CSng(Level.Current.Left) + 75F
		Dim rightBound As Single = Me.startPosX
		Dim goingRight As Boolean = False
		While True
			If goingRight Then
				While MyBase.transform.position.x < rightBound
					MyBase.transform.position += Vector3.right * Me.speed * CupheadTime.FixedDelta
					Yield wait
				End While
				goingRight = False
				MyBase.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
			Else
				While MyBase.transform.position.x > leftBound
					MyBase.transform.position += Vector3.left * Me.speed * CupheadTime.FixedDelta
					Yield wait
				End While
				goingRight = True
				MyBase.transform.SetScale(New Single?(-1F), New Single?(1F), New Single?(1F))
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040021A3 RID: 8611
	Private speed As Single

	' Token: 0x040021A4 RID: 8612
	Private startPosX As Single

	' Token: 0x040021A5 RID: 8613
	Private damageDealer As DamageDealer
End Class
