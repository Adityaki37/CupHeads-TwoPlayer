Imports System
Imports UnityEngine

' Token: 0x02000869 RID: 2153
Public Class PlatformingLevelJumpTrigger
	Inherits AbstractCollidableObject

	' Token: 0x06003207 RID: 12807 RVA: 0x001D3410 File Offset: 0x001D1810
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter Then
			Dim component As PlatformingLevelGroundMovementEnemy = hit.GetComponent(Of PlatformingLevelGroundMovementEnemy)()
			If component IsNot Nothing AndAlso component.direction = Me.direction Then
				component.Jump()
			End If
		End If
	End Sub

	' Token: 0x06003208 RID: 12808 RVA: 0x001D3455 File Offset: 0x001D1855
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003209 RID: 12809 RVA: 0x001D3468 File Offset: 0x001D1868
	Private Sub DrawGizmos(a As Single)
		Dim component As BoxCollider2D = MyBase.GetComponent(Of BoxCollider2D)()
		Gizmos.color = New Color(1F, 1F, 0F, a)
		Gizmos.DrawWireCube(component.bounds.center, component.bounds.size)
	End Sub

	' Token: 0x04003A6D RID: 14957
	<SerializeField()>
	Private direction As PlatformingLevelGroundMovementEnemy.Direction
End Class
