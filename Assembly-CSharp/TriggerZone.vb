Imports System
Imports UnityEngine

' Token: 0x020003EC RID: 1004
Public Class TriggerZone
	Inherits MonoBehaviour

	' Token: 0x06000D8D RID: 3469 RVA: 0x0008E3E4 File Offset: 0x0008C7E4
	Private Sub OnDrawGizmos()
		Gizmos.DrawWireCube(MyBase.transform.position, Me.size)
	End Sub

	' Token: 0x06000D8E RID: 3470 RVA: 0x0008E404 File Offset: 0x0008C804
	Public Function Contains(position As Vector3) As Boolean
		Dim zero As Rect = Rect.zero
		zero.size = Me.size
		zero.center = MyBase.transform.position
		Return zero.Contains(position)
	End Function

	' Token: 0x04001706 RID: 5894
	<SerializeField()>
	Private size As Vector2
End Class
