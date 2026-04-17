Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200035F RID: 863
Public Class Path2D
	Inherits MonoBehaviour

	' Token: 0x06000998 RID: 2456 RVA: 0x0007C104 File Offset: 0x0007A504
	Protected Overridable Sub OnDrawGizmos()
		Me.DrawGizmos(0.1F)
	End Sub

	' Token: 0x06000999 RID: 2457 RVA: 0x0007C111 File Offset: 0x0007A511
	Protected Overridable Sub OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x0600099A RID: 2458 RVA: 0x0007C120 File Offset: 0x0007A520
	Private Sub DrawGizmos(a As Single)
		For i As Integer = 0 To Me.nodes.Count - 1
			If i > 0 Then
				Gizmos.DrawLine(Me.nodes(i), Me.nodes(i - 1))
			End If
		Next
	End Sub

	' Token: 0x0400143B RID: 5179
	Public space As Path2D.Space

	' Token: 0x0400143C RID: 5180
	Public nodes As List(Of Vector2) = New List(Of Vector2)(2)

	' Token: 0x02000360 RID: 864
	Public Enum Space
		' Token: 0x0400143E RID: 5182
		[Global]
		' Token: 0x0400143F RID: 5183
		Local
	End Enum
End Class
