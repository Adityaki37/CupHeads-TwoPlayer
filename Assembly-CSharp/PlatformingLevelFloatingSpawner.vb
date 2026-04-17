Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200085F RID: 2143
Public Class PlatformingLevelFloatingSpawner
	Inherits AbstractPausableComponent

	' Token: 0x060031CB RID: 12747 RVA: 0x001D13CD File Offset: 0x001CF7CD
	Private Sub Start()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.spawn_cr())
	End Sub

	' Token: 0x060031CC RID: 12748 RVA: 0x001D13E4 File Offset: 0x001CF7E4
	Private Iterator Function spawn_cr() As IEnumerator
		Dim hashadSuccessfulSpawn As Boolean = False
		While True
			If hashadSuccessfulSpawn Then
				Yield CupheadTime.WaitForSeconds(Me, Me.spawnTime.RandomFloat())
			Else
				Yield CupheadTime.WaitForSeconds(Me, Me.initialSpawnTime.RandomFloat())
			End If
			Dim spawnPos As Vector2 = MyBase.transform.position
			spawnPos.x += Global.UnityEngine.Random.Range(-Me.xRange, Me.xRange)
			If CupheadLevelCamera.Current.ContainsPoint(spawnPos, New Vector2(0F, 1000F)) Then
				Dim platformingLevelGroundMovementEnemy As PlatformingLevelGroundMovementEnemy = Me.enemyPrefab.Spawn(spawnPos, If((Not MathUtils.RandomBool()), PlatformingLevelGroundMovementEnemy.Direction.Right, PlatformingLevelGroundMovementEnemy.Direction.Left), True)
				platformingLevelGroundMovementEnemy.Float(True)
				hashadSuccessfulSpawn = True
			End If
		End While
		Return
	End Function

	' Token: 0x060031CD RID: 12749 RVA: 0x001D13FF File Offset: 0x001CF7FF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.enemyPrefab = Nothing
	End Sub

	' Token: 0x060031CE RID: 12750 RVA: 0x001D140E File Offset: 0x001CF80E
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x060031CF RID: 12751 RVA: 0x001D1421 File Offset: 0x001CF821
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x060031D0 RID: 12752 RVA: 0x001D1434 File Offset: 0x001CF834
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(1F, 1F, 0F, a)
		Gizmos.DrawLine(MyBase.baseTransform.position - New Vector3(Me.xRange, 0F, 0F), MyBase.baseTransform.position + New Vector3(Me.xRange, 0F, 0F))
	End Sub

	' Token: 0x04003A29 RID: 14889
	<SerializeField()>
	Private enemyPrefab As PlatformingLevelGroundMovementEnemy

	' Token: 0x04003A2A RID: 14890
	<SerializeField()>
	Private xRange As Single

	' Token: 0x04003A2B RID: 14891
	<SerializeField()>
	Private initialSpawnTime As MinMax

	' Token: 0x04003A2C RID: 14892
	<SerializeField()>
	Private spawnTime As MinMax
End Class
