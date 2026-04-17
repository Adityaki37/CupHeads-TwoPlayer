Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200073C RID: 1852
Public Class RetroArcadeBouncyManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x06002864 RID: 10340 RVA: 0x001787FD File Offset: 0x00176BFD
	Public Sub StartBouncy()
		MyBase.StartCoroutine(Me.spawn_balls_cr())
	End Sub

	' Token: 0x06002865 RID: 10341 RVA: 0x0017880C File Offset: 0x00176C0C
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.black
		For Each transform As Transform In Me.spawnPoints
			Gizmos.DrawWireSphere(transform.position, 50F)
		Next
	End Sub

	' Token: 0x06002866 RID: 10342 RVA: 0x00178858 File Offset: 0x00176C58
	Private Iterator Function spawn_balls_cr() As IEnumerator
		Dim p As LevelProperties.RetroArcade.Bouncy = MyBase.properties.CurrentState.bouncy
		Dim counter As Integer = 0
		Dim holders As List(Of RetroArcadeBouncyBallHolder) = New List(Of RetroArcadeBouncyBallHolder)()
		Dim typeMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.typeString.Length)
		Dim typeString As String() = p.typeString(typeMainIndex).Split(New Char() { ","c })
		Dim typeIndex As Integer = Global.UnityEngine.Random.Range(0, typeString.Length)
		Dim ballTypes As String() = New String(2) {}
		While counter < p.waveCount
			typeString = p.typeString(typeMainIndex).Split(New Char() { ","c })
			Dim posIndex As Integer = Global.UnityEngine.Random.Range(0, Me.spawnPoints.Length)
			For i As Integer = 0 To 3 - 1
				ballTypes(i) = typeString(typeIndex)
				If typeIndex < typeString.Length - 1 Then
					typeIndex += 1
				Else
					typeMainIndex = (typeMainIndex + 1) Mod p.typeString.Length
					typeIndex = 0
				End If
			Next
			Dim holder As RetroArcadeBouncyBallHolder = Me.ballHolder.Create(Me, p, Me.spawnPoints(posIndex).position, ballTypes)
			holders.Add(holder)
			counter += 1
			Yield CupheadTime.WaitForSeconds(Me, p.spawnRange.RandomFloat())
		End While
		Dim allDead As Boolean = True
		While True
			allDead = True
			For j As Integer = 0 To holders.Count - 1
				If Not holders(j).IsDead Then
					allDead = False
				End If
			Next
			If allDead Then
				Exit For
			End If
			Yield Nothing
		End While
		MyBase.properties.DealDamageToNextNamedState()
		For Each retroArcadeBouncyBallHolder As RetroArcadeBouncyBallHolder In holders
			retroArcadeBouncyBallHolder.DestroyBallsHeld()
			Global.UnityEngine.[Object].Destroy(retroArcadeBouncyBallHolder.gameObject)
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x04003123 RID: 12579
	<SerializeField()>
	Private ballHolder As RetroArcadeBouncyBallHolder

	' Token: 0x04003124 RID: 12580
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x04003125 RID: 12581
	Private Const BALLCOUNT As Integer = 3
End Class
