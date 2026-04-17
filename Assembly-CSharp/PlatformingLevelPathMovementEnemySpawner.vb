Imports System
Imports UnityEngine

' Token: 0x0200086C RID: 2156
Public Class PlatformingLevelPathMovementEnemySpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x06003227 RID: 12839 RVA: 0x001D46A5 File Offset: 0x001D2AA5
	Protected Overrides Sub Spawn()
		Me.enemyPrefab.Spawn(MyBase.transform.position, Me.path, Me.startPosition, Me.destroyEnemyAfterLeavingScreen)
	End Sub

	' Token: 0x06003228 RID: 12840 RVA: 0x001D46D0 File Offset: 0x001D2AD0
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003229 RID: 12841 RVA: 0x001D46E3 File Offset: 0x001D2AE3
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x0600322A RID: 12842 RVA: 0x001D46F8 File Offset: 0x001D2AF8
	Private Sub DrawGizmos(a As Single)
		Me.path.DrawGizmos(a, MyBase.baseTransform.position)
		Gizmos.color = New Color(1F, 0F, 0F, a)
		Gizmos.DrawSphere(Me.path.Lerp(Me.startPosition) + MyBase.baseTransform.position, 10F)
		Gizmos.DrawWireSphere(Me.path.Lerp(Me.startPosition) + MyBase.baseTransform.position, 11F)
	End Sub

	' Token: 0x04003A81 RID: 14977
	Public enemyPrefab As PlatformingLevelPathMovementEnemy

	' Token: 0x04003A82 RID: 14978
	<Header("Path")>
	Public startPosition As Single = 0.5F

	' Token: 0x04003A83 RID: 14979
	Public direction As PlatformingLevelPathMovementEnemy.Direction = PlatformingLevelPathMovementEnemy.Direction.Forward

	' Token: 0x04003A84 RID: 14980
	Public path As VectorPath
End Class
