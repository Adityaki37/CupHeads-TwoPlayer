Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B9 RID: 2233
Public Class FunhousePlatformingLevelJackSpawner
	Inherits AbstractPausableComponent

	' Token: 0x0600341C RID: 13340 RVA: 0x001E41B7 File Offset: 0x001E25B7
	Private Sub Start()
		MyBase.StartCoroutine(Me.spawn_cr())
	End Sub

	' Token: 0x0600341D RID: 13341 RVA: 0x001E41C8 File Offset: 0x001E25C8
	Private Iterator Function spawn_cr() As IEnumerator
		Dim hashadSuccessfulSpawn As Boolean = False
		While True
			If hashadSuccessfulSpawn Then
				Yield CupheadTime.WaitForSeconds(Me, Me.spawnTime.RandomFloat())
			Else
				Yield CupheadTime.WaitForSeconds(Me, Me.initialSpawnTime.RandomFloat())
			End If
			Dim spawnPos As Vector2 = MyBase.transform.position
			spawnPos.y = MyBase.transform.position.y
			spawnPos.x += Global.UnityEngine.Random.Range(-Me.xRange, Me.xRange)
			If CupheadLevelCamera.Current.ContainsPoint(spawnPos, New Vector2(0F, 1000F)) Then
				Me.jackPrefab.Spawn(spawnPos).SelectDirection(Me.isBottom)
				hashadSuccessfulSpawn = True
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600341E RID: 13342 RVA: 0x001E41E4 File Offset: 0x001E25E4
	Protected Overrides Sub OnDrawGizmos()
		Gizmos.color = New Color(1F, 1F, 0F, 1F)
		Gizmos.DrawLine(MyBase.baseTransform.position - New Vector3(Me.xRange, 0F, 0F), MyBase.baseTransform.position + New Vector3(Me.xRange, 0F, 0F))
	End Sub

	' Token: 0x04003C5F RID: 15455
	<SerializeField()>
	Private initialSpawnTime As MinMax

	' Token: 0x04003C60 RID: 15456
	<SerializeField()>
	Private spawnTime As MinMax

	' Token: 0x04003C61 RID: 15457
	<SerializeField()>
	Private xRange As Single

	' Token: 0x04003C62 RID: 15458
	<SerializeField()>
	Private jackPrefab As FunhousePlatformingLevelJack

	' Token: 0x04003C63 RID: 15459
	<SerializeField()>
	Private isBottom As Boolean
End Class
