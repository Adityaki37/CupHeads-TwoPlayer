Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E9 RID: 2281
Public Class MountainPlatformingLevelMudmanSpawner
	Inherits AbstractPausableComponent

	' Token: 0x0600357E RID: 13694 RVA: 0x001F295F File Offset: 0x001F0D5F
	Public Sub SpawnMudmen()
		MyBase.StartCoroutine(Me.spawn_cr())
	End Sub

	' Token: 0x0600357F RID: 13695 RVA: 0x001F2970 File Offset: 0x001F0D70
	Private Iterator Function spawn_cr() As IEnumerator
		Dim mudmanSize As String() = Me.mudmanSizeString.Split(New Char() { ","c })
		Dim mudmanBig As String() = Me.mudmanBigSpawnString.Split(New Char() { ","c })
		Dim mudmanSmall As String() = Me.mudmanSmallSpawnString.Split(New Char() { ","c })
		Dim mudmanSizeIndex As Integer = Global.UnityEngine.Random.Range(0, mudmanSize.Length)
		Dim mudmanBigIndex As Integer = Global.UnityEngine.Random.Range(0, mudmanBig.Length)
		Dim mudmanSmallIndex As Integer = Global.UnityEngine.Random.Range(0, mudmanSmall.Length)
		Dim dir As PlatformingLevelGroundMovementEnemy.Direction = PlatformingLevelGroundMovementEnemy.Direction.Left
		Yield CupheadTime.WaitForSeconds(Me, Me.initialDelayRange.RandomFloat())
		While MountainPlatformingLevelElevatorHandler.elevatorIsMoving
			If mudmanSize(mudmanSizeIndex)(0) = "B"c Then
				Dim array As String() = mudmanBig(mudmanBigIndex).Split(New Char() { "-"c })
				For Each text As String In array
					Dim num As Integer = 1
					Parser.IntTryParse(text, num)
					dir = If((num >= 3), PlatformingLevelGroundMovementEnemy.Direction.Left, PlatformingLevelGroundMovementEnemy.Direction.Right)
					Dim mountainPlatformingLevelMudman As MountainPlatformingLevelMudman = Global.UnityEngine.[Object].Instantiate(Of MountainPlatformingLevelMudman)(Me.bigMudman)
					mountainPlatformingLevelMudman.Init(Me.spawnPoints(num - 1).position, dir)
				Next
				mudmanBigIndex = (mudmanBigIndex + 1) Mod mudmanBig.Length
			ElseIf mudmanSize(mudmanSizeIndex)(0) = "S"c Then
				Dim array3 As String() = mudmanSmall(mudmanSmallIndex).Split(New Char() { "-"c })
				For Each text2 As String In array3
					Dim num2 As Integer = 1
					Parser.IntTryParse(text2, num2)
					dir = If((num2 >= 3), PlatformingLevelGroundMovementEnemy.Direction.Left, PlatformingLevelGroundMovementEnemy.Direction.Right)
					Dim mountainPlatformingLevelMudman2 As MountainPlatformingLevelMudman = Global.UnityEngine.[Object].Instantiate(Of MountainPlatformingLevelMudman)(Me.smallMudman)
					mountainPlatformingLevelMudman2.Init(Me.spawnPoints(num2 - 1).position, dir)
				Next
				mudmanSmallIndex = (mudmanSmallIndex + 1) Mod mudmanSmall.Length
			End If
			mudmanSizeIndex = (mudmanSizeIndex + 1) Mod mudmanSize.Length
			Yield CupheadTime.WaitForSeconds(Me, Me.spawnDelayRange.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003580 RID: 13696 RVA: 0x001F298B File Offset: 0x001F0D8B
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003581 RID: 13697 RVA: 0x001F299E File Offset: 0x001F0D9E
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06003582 RID: 13698 RVA: 0x001F29B4 File Offset: 0x001F0DB4
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(1F, 0F, 0F, a)
		For Each transform As Transform In Me.spawnPoints
			Gizmos.DrawWireSphere(transform.position, 30F)
		Next
	End Sub

	' Token: 0x04003D94 RID: 15764
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x04003D95 RID: 15765
	<SerializeField()>
	Private bigMudman As MountainPlatformingLevelMudman

	' Token: 0x04003D96 RID: 15766
	<SerializeField()>
	Private smallMudman As MountainPlatformingLevelMudman

	' Token: 0x04003D97 RID: 15767
	<SerializeField()>
	Private spawnDelayRange As MinMax

	' Token: 0x04003D98 RID: 15768
	<SerializeField()>
	Private initialDelayRange As MinMax

	' Token: 0x04003D99 RID: 15769
	<SerializeField()>
	Private mudmanSizeString As String

	' Token: 0x04003D9A RID: 15770
	<SerializeField()>
	Private mudmanBigSpawnString As String

	' Token: 0x04003D9B RID: 15771
	<SerializeField()>
	Private mudmanSmallSpawnString As String
End Class
