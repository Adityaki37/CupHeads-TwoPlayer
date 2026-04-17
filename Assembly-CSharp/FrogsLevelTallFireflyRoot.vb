Imports System
Imports UnityEngine

' Token: 0x020006C1 RID: 1729
Public Class FrogsLevelTallFireflyRoot
	Inherits AbstractMonoBehaviour

	' Token: 0x170003B6 RID: 950
	' (get) Token: 0x060024C9 RID: 9417 RVA: 0x00158FA4 File Offset: 0x001573A4
	Public ReadOnly Property radius As Single
		Get
			Return Me._radius
		End Get
	End Property

	' Token: 0x060024CA RID: 9418 RVA: 0x00158FAC File Offset: 0x001573AC
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(1F, 0F, 0F, 0.2F)
		Gizmos.DrawSphere(MyBase.baseTransform.position, Me.radius)
		Gizmos.color = New Color(1F, 0F, 0F, 1F)
		Gizmos.DrawWireSphere(MyBase.baseTransform.position, Me.radius)
	End Sub

	' Token: 0x04002D6D RID: 11629
	<SerializeField()>
	Private _radius As Single = 100F
End Class
