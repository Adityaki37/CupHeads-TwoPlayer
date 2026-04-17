Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000550 RID: 1360
Public Class ChessRookLevelHazard
	Inherits AbstractProjectile

	' Token: 0x06001941 RID: 6465 RVA: 0x000E5260 File Offset: 0x000E3660
	Public Function Create(position As Vector3, speed As Single) As ChessRookLevelHazard
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		Me.speed = speed
		Me.Move()
		Return Me
	End Function

	' Token: 0x06001942 RID: 6466 RVA: 0x000E5288 File Offset: 0x000E3688
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001943 RID: 6467 RVA: 0x000E52A6 File Offset: 0x000E36A6
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001944 RID: 6468 RVA: 0x000E52B8 File Offset: 0x000E36B8
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.x > -740F
			MyBase.transform.position += Vector3.left * Me.speed * CupheadTime.FixedDelta
			Yield wait
		End While
		Me.Recycle()
		Return
	End Function

	' Token: 0x0400225F RID: 8799
	Private speed As Single
End Class
