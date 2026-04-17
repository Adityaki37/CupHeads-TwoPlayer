Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200075C RID: 1884
Public Class RetroArcadeTentacleManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x06002913 RID: 10515 RVA: 0x0017EF9C File Offset: 0x0017D39C
	Public Sub StartTentacle()
		Me.SetupYSpawnpoints()
		MyBase.StartCoroutine(Me.spawn_tentacles_cr())
	End Sub

	' Token: 0x06002914 RID: 10516 RVA: 0x0017EFB4 File Offset: 0x0017D3B4
	Private Sub SetupYSpawnpoints()
		Me.YSpawnPoints = New Single(7) {}
		Dim num As Single = CSng(Level.Current.Ground) - Me.bottom.position.y
		Me.offset = (CSng(Level.Current.Height) - num) / 8F
		For i As Integer = 0 To 8 - 1
			Dim num2 As Single = Me.offset * CSng(i)
			Me.YSpawnPoints(i) = num2
		Next
	End Sub

	' Token: 0x06002915 RID: 10517 RVA: 0x0017F02C File Offset: 0x0017D42C
	Private Iterator Function spawn_tentacles_cr() As IEnumerator
		Me.octopusHead.SetActive(True)
		Dim p As LevelProperties.RetroArcade.Tentacle = MyBase.properties.CurrentState.tentacle
		Dim mainTargetIndex As Integer = Global.UnityEngine.Random.Range(0, p.targetString.Length)
		Dim targetString As String() = p.targetString(mainTargetIndex).Split(New Char() { ","c })
		Dim targetIndex As Integer = Global.UnityEngine.Random.Range(0, targetString.Length)
		Dim counter As Integer = 0
		Dim positionIndex As Integer = 0
		Dim spawningLeft As Boolean = Rand.Bool()
		Dim tentacle As RetroArcadeTentacle = Nothing
		Dim lastLeftTentacle As RetroArcadeTentacle = Nothing
		Dim lastRightTentacle As RetroArcadeTentacle = Nothing
		Me.tentacles = New RetroArcadeTentacle(p.tentacleCount - 1) {}
		While counter < p.tentacleCount
			targetString = p.targetString(mainTargetIndex).Split(New Char() { ","c })
			Parser.IntTryParse(targetString(targetIndex), positionIndex)
			Dim spawnPoint As Vector3 = New Vector3(If((Not spawningLeft), 320F, (-320F)), -500F)
			If spawningLeft Then
				While lastLeftTentacle IsNot Nothing
					If lastLeftTentacle.transform.position.x >= -240F Then
						Exit While
					End If
					Yield Nothing
				End While
			Else
				While lastRightTentacle IsNot Nothing
					If lastRightTentacle.transform.position.x <= 240F Then
						Exit While
					End If
					Yield Nothing
				End While
			End If
			tentacle = Me.tentaclePrefab.Spawn()
			tentacle.Init(spawnPoint, Me.YSpawnPoints(positionIndex), spawningLeft, p.risingSpeed, p.moveSpeed)
			Me.tentacles(counter) = tentacle
			If spawningLeft Then
				lastLeftTentacle = tentacle
			Else
				lastRightTentacle = tentacle
			End If
			If targetIndex < targetString.Length - 1 Then
				targetIndex += 1
			Else
				mainTargetIndex = (mainTargetIndex + 1) Mod p.targetString.Length
				targetIndex = 0
			End If
			spawningLeft = Not spawningLeft
			counter += 1
			Yield Nothing
		End While
		Dim countDeadOnes As Integer = 0
		While True
			countDeadOnes = 0
			For i As Integer = 0 To Me.tentacles.Length - 1
				If Me.tentacles(i) Is Nothing Then
					countDeadOnes += 1
				End If
			Next
			If countDeadOnes >= Me.tentacles.Length Then
				Exit For
			End If
			Yield Nothing
		End While
		Me.octopusHead.SetActive(False)
		MyBase.properties.DealDamageToNextNamedState()
		Yield Nothing
		Return
	End Function

	' Token: 0x040031FF RID: 12799
	Private Const LEFT_SIDE_SPAWN As Single = -320F

	' Token: 0x04003200 RID: 12800
	Private Const RIGHT_SIDE_SPAWN As Single = 320F

	' Token: 0x04003201 RID: 12801
	Private Const SPACES_COUNT As Integer = 8

	' Token: 0x04003202 RID: 12802
	Private Const SPAWN_OFFSET As Single = 240F

	' Token: 0x04003203 RID: 12803
	<SerializeField()>
	Private octopusHead As GameObject

	' Token: 0x04003204 RID: 12804
	<SerializeField()>
	Private tentaclePrefab As RetroArcadeTentacle

	' Token: 0x04003205 RID: 12805
	<SerializeField()>
	Private bottom As Transform

	' Token: 0x04003206 RID: 12806
	Private tentacles As RetroArcadeTentacle()

	' Token: 0x04003207 RID: 12807
	Private YSpawnPoints As Single()

	' Token: 0x04003208 RID: 12808
	Private offset As Single
End Class
