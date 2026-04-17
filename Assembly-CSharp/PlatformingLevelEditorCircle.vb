Imports System
Imports UnityEngine

' Token: 0x020008FE RID: 2302
<RequireComponent(GetType(CircleCollider2D))>
Public Class PlatformingLevelEditorCircle
	Inherits AbstractMonoBehaviour

	' Token: 0x17000462 RID: 1122
	' (get) Token: 0x06003606 RID: 13830 RVA: 0x001F6454 File Offset: 0x001F4854
	Private ReadOnly Property circleCollider As CircleCollider2D
		Get
			If Me._circleCollider Is Nothing Then
				Me._circleCollider = MyBase.GetComponent(Of CircleCollider2D)()
			End If
			Return Me._circleCollider
		End Get
	End Property

	' Token: 0x06003607 RID: 13831 RVA: 0x001F6479 File Offset: 0x001F4879
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.5F)
	End Sub

	' Token: 0x06003608 RID: 13832 RVA: 0x001F648C File Offset: 0x001F488C
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
	End Sub

	' Token: 0x06003609 RID: 13833 RVA: 0x001F6494 File Offset: 0x001F4894
	Private Sub DrawGizmos(alpha As Single)
		Gizmos.color = Color.cyan
		Gizmos.DrawWireSphere(MyBase.transform.position + Me.circleCollider.offset, Me.circleCollider.radius)
	End Sub

	' Token: 0x04003E0E RID: 15886
	Private _circleCollider As CircleCollider2D
End Class
