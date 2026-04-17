Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E7 RID: 2279
Public Class MountainPlatformingLevelMinerSpawner
	Inherits AbstractPausableComponent

	' Token: 0x06003566 RID: 13670 RVA: 0x001F211D File Offset: 0x001F051D
	Private Sub Start()
		MyBase.StartCoroutine(Me.spawn_cr())
	End Sub

	' Token: 0x06003567 RID: 13671 RVA: 0x001F212C File Offset: 0x001F052C
	Private Iterator Function spawn_cr() As IEnumerator
		While True
			Dim spawnPos As Vector2 = MyBase.transform.position
			spawnPos.x += Global.UnityEngine.Random.Range(-Me.xRange, Me.xRange)
			If Me.isRespawning Then
				Yield CupheadTime.WaitForSeconds(Me, Me.deathDelayTime.RandomFloat())
			Else
				Yield CupheadTime.WaitForSeconds(Me, Me.spawnTime.RandomFloat())
			End If
			Me.enemySpawned = Me.enemyPrefab.Spawn(spawnPos, If((Not MathUtils.RandomBool()), PlatformingLevelGroundMovementEnemy.Direction.Right, PlatformingLevelGroundMovementEnemy.Direction.Left), False)
			Me.enemySpawned.Float(False)
			While Me.enemySpawned IsNot Nothing
				Yield Nothing
			End While
			Me.isRespawning = True
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003568 RID: 13672 RVA: 0x001F2147 File Offset: 0x001F0547
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003569 RID: 13673 RVA: 0x001F215A File Offset: 0x001F055A
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x0600356A RID: 13674 RVA: 0x001F2170 File Offset: 0x001F0570
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(1F, 1F, 0F, a)
		Gizmos.DrawLine(MyBase.baseTransform.position - New Vector3(Me.xRange, 0F, 0F), MyBase.baseTransform.position + New Vector3(Me.xRange, 0F, 0F))
	End Sub

	' Token: 0x04003D89 RID: 15753
	<SerializeField()>
	Private enemyPrefab As PlatformingLevelGroundMovementEnemy

	' Token: 0x04003D8A RID: 15754
	Private enemySpawned As PlatformingLevelGroundMovementEnemy

	' Token: 0x04003D8B RID: 15755
	<SerializeField()>
	Private xRange As Single

	' Token: 0x04003D8C RID: 15756
	<SerializeField()>
	Private deathDelayTime As MinMax

	' Token: 0x04003D8D RID: 15757
	<SerializeField()>
	Private spawnTime As MinMax

	' Token: 0x04003D8E RID: 15758
	Private isRespawning As Boolean
End Class
