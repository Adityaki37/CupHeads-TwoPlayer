Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E0 RID: 2272
Public Class MountainPlatformingLevelDragonSpawner
	Inherits AbstractPausableComponent

	' Token: 0x0600352C RID: 13612 RVA: 0x001EF154 File Offset: 0x001ED554
	Private Sub Start()
		MyBase.StartCoroutine(Me.spawn_cr())
		Me.spawnIndex = Global.UnityEngine.Random.Range(0, Me.spawnString.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x0600352D RID: 13613 RVA: 0x001EF188 File Offset: 0x001ED588
	Private Iterator Function spawn_cr() As IEnumerator
		While True
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(0F, 1000F)) Then
				If(Me.isElevator AndAlso MountainPlatformingLevelElevatorHandler.elevatorIsMoving) OrElse Not Me.isElevator Then
					Dim dragonPrefab As MountainPlatformingLevelDragon = Nothing
					Dim scale As Integer = 1
					Dim spawnPoint As Integer = 1
					Parser.IntTryParse(Me.spawnString.Split(New Char() { ","c })(Me.spawnIndex), spawnPoint)
					Dim startPos As Vector3 = New Vector3(Me.spawnPoints(spawnPoint - 1).position.x, Me.spawnPoints(spawnPoint - 1).position.y + 500F)
					If spawnPoint <> 1 Then
						If spawnPoint <> 2 Then
							If spawnPoint = 3 Then
								dragonPrefab = Me.dragonSidePrefab
								scale = -1
							End If
						Else
							dragonPrefab = Me.dragonMiddlePrefab
						End If
					Else
						dragonPrefab = Me.dragonSidePrefab
					End If
					Dim dragon As MountainPlatformingLevelDragon = Global.UnityEngine.[Object].Instantiate(Of MountainPlatformingLevelDragon)(dragonPrefab)
					dragon.Init(startPos, Me.spawnPoints(spawnPoint - 1).position)
					dragon.transform.SetScale(New Single?(CSng(scale)), Nothing, Nothing)
					Me.spawnIndex = (Me.spawnIndex + 1) Mod Me.spawnString.Split(New Char() { ","c }).Length
					Yield CupheadTime.WaitForSeconds(Me, Me.spawnDelay)
				End If
				Yield Nothing
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600352E RID: 13614 RVA: 0x001EF1A3 File Offset: 0x001ED5A3
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x0600352F RID: 13615 RVA: 0x001EF1B6 File Offset: 0x001ED5B6
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06003530 RID: 13616 RVA: 0x001EF1CC File Offset: 0x001ED5CC
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(1F, 0F, 1F, a)
		For Each transform As Transform In Me.spawnPoints
			Gizmos.DrawWireSphere(transform.position, 30F)
		Next
	End Sub

	' Token: 0x04003D53 RID: 15699
	<SerializeField()>
	Private isElevator As Boolean

	' Token: 0x04003D54 RID: 15700
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x04003D55 RID: 15701
	<SerializeField()>
	Private dragonMiddlePrefab As MountainPlatformingLevelDragon

	' Token: 0x04003D56 RID: 15702
	<SerializeField()>
	Private dragonSidePrefab As MountainPlatformingLevelDragon

	' Token: 0x04003D57 RID: 15703
	<SerializeField()>
	Private spawnString As String

	' Token: 0x04003D58 RID: 15704
	<SerializeField()>
	Private spawnDelay As Single

	' Token: 0x04003D59 RID: 15705
	Private spawnIndex As Integer
End Class
