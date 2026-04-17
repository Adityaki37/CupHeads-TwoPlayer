Imports System
Imports UnityEngine

' Token: 0x0200088C RID: 2188
Public Class TreePlatformingLevelDragonflyProjectileHandler
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x060032E2 RID: 13026 RVA: 0x001D9684 File Offset: 0x001D7A84
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.dragonflyShots = New TreePlatformingLevelDragonflyShot(MyBase.GetComponentsInChildren(Of TreePlatformingLevelDragonflyShot)().Length - 1) {}
		Me.dragonflyShots = MyBase.GetComponentsInChildren(Of TreePlatformingLevelDragonflyShot)()
		Me.spawnIndex = Global.UnityEngine.Random.Range(0, Me.delaySpawnString.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x060032E3 RID: 13027 RVA: 0x001D96DC File Offset: 0x001D7ADC
	Protected Overrides Sub Spawn()
		Me.spawnDelay.min = Parser.FloatParse(Me.delaySpawnString.Split(New Char() { ","c })(Me.spawnIndex))
		Me.spawnDelay.max = Parser.FloatParse(Me.delaySpawnString.Split(New Char() { ","c })(Me.spawnIndex))
		Me.Activate()
		MyBase.Spawn()
		Me.spawnIndex = (Me.spawnIndex + 1) Mod Me.delaySpawnString.Split(New Char() { ","c }).Length
	End Sub

	' Token: 0x060032E4 RID: 13028 RVA: 0x001D9778 File Offset: 0x001D7B78
	Private Sub Activate()
		Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.dragonflyShots.Length)
		If Not Me.dragonflyShots(num).isActivated Then
			Me.dragonflyShots(num).Activate()
		End If
	End Sub

	' Token: 0x04003B0F RID: 15119
	<SerializeField()>
	Private delaySpawnString As String

	' Token: 0x04003B10 RID: 15120
	Private dragonflyShots As TreePlatformingLevelDragonflyShot()

	' Token: 0x04003B11 RID: 15121
	Private spawnIndex As Integer
End Class
