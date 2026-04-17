Imports System
Imports UnityEngine

' Token: 0x02000782 RID: 1922
Public Class RumRunnersGroundTest
	Inherits MonoBehaviour

	' Token: 0x06002A40 RID: 10816 RVA: 0x0018B56C File Offset: 0x0018996C
	Private Sub OnDrawGizmos()
		Gizmos.color = Color.white
		Gizmos.DrawSphere(New Vector3(MyBase.transform.position.x, RumRunnersLevel.GroundWalkingPosY(MyBase.transform.position + Vector3.up * 50F, Me.collider, Me.yOffset, 200F)), 20F)
		Gizmos.color = Color.cyan
		Gizmos.DrawSphere(New Vector3(MyBase.transform.position.x, RumRunnersLevel.GroundWalkingPosY(New Vector3(MyBase.transform.position.x, RumRunnersLevel.GroundWalkingPosY(MyBase.transform.position, Me.collider, Me.yOffset, 200F)), Me.collider, Me.yOffset, 200F)), 20F)
		Gizmos.color = Color.yellow
		Gizmos.DrawSphere(MyBase.transform.position, 20F)
	End Sub

	' Token: 0x04003317 RID: 13079
	<SerializeField()>
	Private collider As Collider2D

	' Token: 0x04003318 RID: 13080
	<SerializeField()>
	Private yOffset As Single
End Class
