Imports System
Imports UnityEngine

' Token: 0x02000B10 RID: 2832
Public Class EffectRadius
	Inherits AbstractPausableComponent

	' Token: 0x17000626 RID: 1574
	' (get) Token: 0x060044B6 RID: 17590 RVA: 0x00246305 File Offset: 0x00244705
	Public ReadOnly Property radius As Single
		Get
			Return Me._radius
		End Get
	End Property

	' Token: 0x17000627 RID: 1575
	' (get) Token: 0x060044B7 RID: 17591 RVA: 0x0024630D File Offset: 0x0024470D
	Public ReadOnly Property offset As Vector2
		Get
			Return Me._offset
		End Get
	End Property

	' Token: 0x060044B8 RID: 17592 RVA: 0x00246318 File Offset: 0x00244718
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(1F, 0F, 0F, 1F)
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.offset, Me.radius)
	End Sub

	' Token: 0x060044B9 RID: 17593 RVA: 0x00246374 File Offset: 0x00244774
	Public Sub CreateInRadius()
		Dim vector As Vector2 = MyBase.baseTransform.position + Me.offset
		Dim vector2 As Vector2 = New Vector2(Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)), Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)))
		Me.target = vector + vector2.normalized * Me.radius * Global.UnityEngine.Random.value
		Me.effect.Create(Me.target)
	End Sub

	' Token: 0x04004A6F RID: 19055
	<SerializeField()>
	Private effect As Effect

	' Token: 0x04004A70 RID: 19056
	<SerializeField()>
	Private _radius As Single = 100F

	' Token: 0x04004A71 RID: 19057
	<SerializeField()>
	Private _offset As Vector2 = Vector2.zero

	' Token: 0x04004A72 RID: 19058
	Private target As Vector2
End Class
