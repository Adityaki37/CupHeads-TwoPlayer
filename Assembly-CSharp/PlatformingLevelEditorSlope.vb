Imports System
Imports UnityEngine

' Token: 0x02000901 RID: 2305
<RequireComponent(GetType(PolygonCollider2D))>
Public Class PlatformingLevelEditorSlope
	Inherits AbstractMonoBehaviour

	' Token: 0x17000463 RID: 1123
	' (get) Token: 0x06003610 RID: 13840 RVA: 0x001F6A18 File Offset: 0x001F4E18
	Private ReadOnly Property polygonCollider As PolygonCollider2D
		Get
			If Me._polygonCollider Is Nothing Then
				Me._polygonCollider = MyBase.GetComponent(Of PolygonCollider2D)()
			End If
			Return Me._polygonCollider
		End Get
	End Property

	' Token: 0x06003611 RID: 13841 RVA: 0x001F6A3D File Offset: 0x001F4E3D
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.5F)
	End Sub

	' Token: 0x06003612 RID: 13842 RVA: 0x001F6A50 File Offset: 0x001F4E50
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
	End Sub

	' Token: 0x06003613 RID: 13843 RVA: 0x001F6A58 File Offset: 0x001F4E58
	Private Sub DrawGizmos(alpha As Single)
		Gizmos.color = Color.cyan
		For i As Integer = 0 To Me.polygonCollider.points.Length - 1
			Dim vector As Vector3 = Me.polygonCollider.points(i)
			Dim vector2 As Vector3 = If((i <> Me.polygonCollider.points.Length - 1), Me.polygonCollider.points(i + 1), Me.polygonCollider.points(0))
			Gizmos.DrawLine(vector, vector2)
		Next
	End Sub

	' Token: 0x04003E1C RID: 15900
	Private _polygonCollider As PolygonCollider2D
End Class
