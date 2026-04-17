Imports System
Imports UnityEngine

' Token: 0x020008CA RID: 2250
Public Class HarbourPlatformingLevelIcebergSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x06003497 RID: 13463 RVA: 0x001E874C File Offset: 0x001E6B4C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.spawn = Me.spawnDelayString.Split(New Char() { ","c })
		Me.spawnIndex = Global.UnityEngine.Random.Range(0, Me.spawn.Length)
	End Sub

	' Token: 0x06003498 RID: 13464 RVA: 0x001E8784 File Offset: 0x001E6B84
	Protected Overrides Sub Spawn()
		Me.spawnDelay.min = Parser.FloatParse(Me.spawn(Me.spawnIndex))
		Me.spawnDelay.max = Parser.FloatParse(Me.spawn(Me.spawnIndex))
		Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.icebergPrefabs.Length)
		Dim num2 As Single = CupheadLevelCamera.Current.transform.position.x + CupheadLevelCamera.Current.Width / 2F + (Me.icebergPrefabs(num).GetComponent(Of Renderer)().bounds.size.x + Me.icebergPrefabs(num).GetComponent(Of Renderer)().bounds.size.x / 2F)
		Dim num3 As Single = CupheadLevelCamera.Current.transform.position.y - 100F
		Me.icebergPrefabs(num).Spawn(New Vector3(num2, num3))
		MyBase.Spawn()
		Me.spawnIndex = (Me.spawnIndex + 1) Mod Me.spawn.Length
	End Sub

	' Token: 0x04003CC1 RID: 15553
	<SerializeField()>
	Private icebergPrefabs As HarbourPlatformingLevelIceberg()

	' Token: 0x04003CC2 RID: 15554
	<SerializeField()>
	Private spawnDelayString As String = "5.5,7.0"

	' Token: 0x04003CC3 RID: 15555
	Private spawn As String()

	' Token: 0x04003CC4 RID: 15556
	Private spawnIndex As Integer
End Class
