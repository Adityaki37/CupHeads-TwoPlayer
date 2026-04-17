Imports System
Imports UnityEngine

' Token: 0x02000641 RID: 1601
Public Class FlyingBlimpLevelSpawnRadius
	Inherits AbstractMonoBehaviour

	' Token: 0x17000387 RID: 903
	' (get) Token: 0x060020E0 RID: 8416 RVA: 0x0012FEE3 File Offset: 0x0012E2E3
	Public ReadOnly Property radius As Single
		Get
			Return Me._radius
		End Get
	End Property

	' Token: 0x060020E1 RID: 8417 RVA: 0x0012FEEC File Offset: 0x0012E2EC
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(1F, 0F, 0F, 0.2F)
		Gizmos.DrawSphere(MyBase.baseTransform.position, Me.radius)
		Gizmos.color = New Color(1F, 0F, 0F, 1F)
		Gizmos.DrawWireSphere(MyBase.baseTransform.position, Me.radius)
	End Sub

	' Token: 0x0400297A RID: 10618
	<SerializeField()>
	Private _radius As Single = 100F
End Class
